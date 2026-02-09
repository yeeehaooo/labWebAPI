using System.Data;
using Dapper;
using DMIS_Backend.Domain.Modules.Products;

namespace DMIS_Backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// 使用 Dapper 的 Product Repository 範例
/// 展示如何透過 IDbSession 共用連線和交易
///
/// 關鍵原則：
/// - 不要 new SqlConnection()，使用傳入的 _session.Connection
/// - 所有 Dapper 操作都要傳入 _session.Transaction
/// - Repository 不控制交易，只使用傳入的交易
/// </summary>
public class DapperProductRepository : IProductRepository
{
  private readonly IDbSession _session;

  public DapperProductRepository(IDbSession session)
  {
    _session = session;
  }

  public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    var sql = @"
      SELECT
        Id,
        ProductId,
        Name,
        Description,
        BasePrice,
        CreatedAt,
        UpdatedAt
      FROM Products
      WHERE Id = @Id";

    var productData = await _session.Connection.QueryFirstOrDefaultAsync<ProductDataDto>(
      new CommandDefinition(
        sql,
        new { Id = id },
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );

    if (productData == null)
    {
      return null;
    }

    // 映射到 Domain 實體（這裡簡化，實際應該有完整的映射邏輯）
    return MapToDomain(productData);
  }

  public async Task<Product?> GetByAggregateIdAsync(
    Guid aggregateId,
    CancellationToken cancellationToken = default
  )
  {
    var sql = @"
      SELECT
        Id,
        ProductId,
        Name,
        Description,
        BasePrice,
        CreatedAt,
        UpdatedAt
      FROM Products
      WHERE ProductId = @ProductId";

    var productData = await _session.Connection.QueryFirstOrDefaultAsync<ProductDataDto>(
      new CommandDefinition(
        sql,
        new { ProductId = aggregateId },
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );

    if (productData == null)
    {
      return null;
    }

    return MapToDomain(productData);
  }

  public async Task<Product?> GetByProductIdAsync(
    Guid productId,
    CancellationToken cancellationToken = default
  )
  {
    // 委派給 GetByAggregateIdAsync
    return await GetByAggregateIdAsync(productId, cancellationToken);
  }

  public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var sql = @"
      SELECT
        Id,
        ProductId,
        Name,
        Description,
        BasePrice,
        CreatedAt,
        UpdatedAt
      FROM Products";

    var productsData = await _session.Connection.QueryAsync<ProductDataDto>(
      new CommandDefinition(
        sql,
        null,
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );

    return productsData.Select(MapToDomain);
  }

  public async Task<IEnumerable<Product>> GetByPageAsync(
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default
  )
  {
    var offset = (pageNumber - 1) * pageSize;

    var sql = @"
      SELECT
        Id,
        ProductId,
        Name,
        Description,
        BasePrice,
        CreatedAt,
        UpdatedAt
      FROM Products
      ORDER BY Id
      OFFSET @Offset ROWS
      FETCH NEXT @PageSize ROWS ONLY";

    var productsData = await _session.Connection.QueryAsync<ProductDataDto>(
      new CommandDefinition(
        sql,
        new { Offset = offset, PageSize = pageSize },
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );

    return productsData.Select(MapToDomain);
  }

  public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
  {
    var sql = @"
      INSERT INTO Products (ProductId, Name, Description, BasePrice, CreatedAt, UpdatedAt)
      VALUES (@ProductId, @Name, @Description, @BasePrice, @CreatedAt, @UpdatedAt)";

    await _session.Connection.ExecuteAsync(
      new CommandDefinition(
        sql,
        new
        {
          ProductId = product.AggregateId,
          Name = product.Name.Value,
          Description = product.Description,
          BasePrice = product.BasePrice,
          CreatedAt = product.CreatedAt,
          UpdatedAt = product.UpdatedAt,
        },
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );
  }

  public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
  {
    var sql = @"
      UPDATE Products
      SET
        Name = @Name,
        Description = @Description,
        BasePrice = @BasePrice,
        UpdatedAt = @UpdatedAt
      WHERE ProductId = @ProductId";

    var rowsAffected = await _session.Connection.ExecuteAsync(
      new CommandDefinition(
        sql,
        new
        {
          ProductId = product.AggregateId,
          Name = product.Name.Value,
          Description = product.Description,
          BasePrice = product.BasePrice,
          UpdatedAt = product.UpdatedAt
        },
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );

    if (rowsAffected == 0)
    {
      throw new InvalidOperationException(
        $"Product with AggregateId {product.AggregateId} not found."
      );
    }
  }

  public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var sql = "DELETE FROM Products WHERE Id = @Id";

    var rowsAffected = await _session.Connection.ExecuteAsync(
      new CommandDefinition(
        sql,
        new { Id = id },
        _session.Transaction,
        cancellationToken: cancellationToken
      )
    );

    return rowsAffected;
  }

  /// <summary>
  /// Dapper Repository 不需要實作 SaveChangesAsync
  /// 因為交易由 Application Handler 透過 IDbSession 控制
  /// </summary>
  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    // Dapper 不需要 SaveChanges，交易由 IDbSession 控制
    return Task.FromResult(0);
  }

  // 簡化的映射邏輯（實際應該使用完整的 Mapper）
  private static Product MapToDomain(ProductDataDto data)
  {
    // 這裡應該使用完整的 Domain 建構函式重建 Aggregate
    // 簡化範例，實際應該有完整的映射邏輯
    throw new NotImplementedException("需要實作完整的 Domain 映射邏輯");
  }

  /// <summary>
  /// DTO 用於 Dapper 查詢結果映射
  /// </summary>
  private class ProductDataDto
  {
    public int Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}
