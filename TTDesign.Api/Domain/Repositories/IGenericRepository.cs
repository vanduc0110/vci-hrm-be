using System.Linq.Expressions;

namespace TTDesign.API.Domain.Repositories
{
  /// <summary>
  /// Base Repository
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  public interface IGenericRepository<TEntity> where TEntity : class
  {
    /// <summary>
    /// Create Once Entity
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task CreateAsync( TEntity obj );
    /// <summary>
    /// Create Entities
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Task CreatesAsync( IEnumerable<TEntity> obj );
    /// <summary>
    /// Update Once Entity
    /// </summary>
    /// <param name="obj"></param>
    void Update( TEntity obj );
    /// <summary>
    /// Update Entities
    /// </summary>
    /// <param name="obj"></param>
    void Updates( TEntity [] obj );
    /// <summary>
    /// Delete Once Entity bởi ID Entity
    /// </summary>
    /// <param name="predicate">"o => o.Id == {field ID of type Entity}"</param>
    /// <returns></returns>
    Task DeleteByCondition( Expression<Func<TEntity, bool>> predicate );
    /// <summary>
    /// Delete Once Entity
    /// </summary>
    /// <param name="obj"></param>
    void Delete( TEntity obj );
    /// <summary>
    /// Delete Entities
    /// </summary>
    /// <param name="obj"></param>
    void Deletes( IEnumerable<TEntity> obj );
    /// <summary>
    /// Chỉ lấy data Entity, ko có reference table khác, no track
    /// </summary>
    /// <param name="predicate">"o => o.Id == {field ID of type Entity}"</param>
    /// <returns></returns>
    Task<TEntity?> GetByConditionNoTrack( Expression<Func<TEntity, bool>> predicate );
    /// <summary>
    /// Chỉ lấy data Entity, ko có reference table khác
    /// </summary>
    /// <param name="predicate">"o => o.Id == {field ID of type Object}"</param>
    /// <returns></returns>
    Task<TEntity?> GetByCondition( Expression<Func<TEntity, bool>> predicate );
    /// <summary>
    /// Chỉ lấy data Entity, không có reference table khác, với bộ điều kiện
    /// </summary>
    /// <param name="predicate">"o => o.{field of type Entity} == {field of type Object}"</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetListByCondition( Expression<Func<TEntity, bool>> predicate );
    /// <summary>
    /// Chỉ lấy data Entity, không có reference table khác, với bộ điều kiện
    /// </summary>
    /// <param name="predicate">"o => o.{field of type Entity} == {field of type Object}"</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetListByConditionTrack( Expression<Func<TEntity, bool>> predicate );
    /// <summary>
    /// Lấy tất cả Entity trong DB
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetAll();
    /// <summary>
    /// Kiểm tra sự tồn tại của Entity trong DB qua bộ điều kiện
    /// </summary>
    /// <param name="predicate">"o => o.{field of type Entity} == {field of type Object}"</param>
    /// <returns>true: exist, false: not exist</returns>
    Task<bool> Exist( Expression<Func<TEntity, bool>> predicate );
  }
}
