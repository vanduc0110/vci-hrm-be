using System.Linq.Expressions;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  /// <summary>
  /// Base Service
  /// </summary>
  /// <remarks>
  /// Service thực hiện:
  /// - DeleteById
  /// - GetById
  /// - GetAll
  /// - Exist
  /// </remarks>
  /// <typeparam name="TEntity">model DB</typeparam>
  public interface IGenericService<TEntity> where TEntity : class
  {
    /// <summary>
    /// Delete Once Entity bởi ID Entity
    /// </summary>
    /// <param name="predicate">"o => o.Id == {field ID of type Entity}"</param>
    /// <returns></returns>
    Task DeleteByCondition( Expression<Func<TEntity, bool>> predicate );
    /// <summary>
    /// Chỉ lấy data Entity, ko có reference table khác
    /// </summary>
    /// <param name="predicate">"o => o.Id == {field ID of type Object}"</param>
    /// <returns></returns>
    Task<TEntity?> GetByCondition( Expression<Func<TEntity, bool>> predicate );
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

  /// <summary>
  /// Base Service xử lý model resource
  /// </summary>
  /// <remarks>
  /// Service thực hiện:
  /// - Create
  /// - Update
  /// </remarks>
  /// <typeparam name="TEntityResource">resource model DB</typeparam>
  public interface BaseServiceResource<TEntityResource> where TEntityResource : class
  {
    /// <summary>
    /// Tạo Entity mới [Create]
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="creator"></param>
    /// <returns></returns>
    Task<long> Create( TEntityResource obj, long creator );
    /// <summary>
    /// Cập nhật Entity [Update]
    /// </summary>
    /// <remarks>Phải check Entity ID có tồn tại trước</remarks>
    /// <param name="resouce"></param>
    /// <param name="editor"></param>
    /// <returns></returns>
    Task Update( TEntityResource resouce, long editor );
  }

  /// <summary>
  /// Base Service xử lý model option
  /// </summary>
  /// <remarks>
  /// Service thực hiện:
  /// - GetOption
  /// </remarks>
  /// <typeparam name="TEntityOption">option model DB</typeparam>
  public interface BaseServiceOption<TEntityOption> where TEntityOption : class
  {
    /// <summary>
    /// danh sách thu gọn của Entity [Option]
    /// </summary>
    /// <param name="teamId">phạm vi: Team ID, null là lấy tất cả</param>
    /// <returns></returns>
    Task<IEnumerable<TEntityOption>> GetOption( long? teamId = null );
  }

  /// <summary>
  /// Base Service xử lý model response
  /// </summary>
  /// <remarks>
  /// Service thực hiện:
  /// - GetList
  /// </remarks>
  /// <typeparam name="TEntityList">response model DB</typeparam>
  public interface BaseServiceList<TEntityList> where TEntityList : class
  {
    /// <summary>
    /// danh sách Entity cho view List [List]
    /// </summary>
    /// <param name="filter">điều kiện tìm kiếm
    /// - Team ID, null là lấy tất cả
    /// - User ID, lọc theo Entity liên quan tới User
    /// </param>
    /// <returns></returns>
    Task<IEnumerable<TEntityList>> GetList( BaseFilter filter );
  }

  /// <summary>
  /// Base Service xử lý model view response
  /// </summary>
  /// <remarks>
  /// Service thực hiện:
  /// - GetDetail
  /// </remarks>
  /// <typeparam name="TEntityDetail">view response model DB</typeparam>
  public interface BaseServiceDetail<TEntityDetail> where TEntityDetail : class
  {
    /// <summary>
    /// Chi tiết Entity cho view Detail [Detail]
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <returns></returns>
    Task<TEntityDetail?> GetDetail( long id );
  }
}
