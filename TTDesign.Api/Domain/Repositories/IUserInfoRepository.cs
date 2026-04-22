using System.Linq.Expressions;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Domain.Repositories
{
  public interface IUserInfoRepository : IGenericRepository<UserInfo>
  {
    Task<UserInfo?> GetUserByCondition( Expression<Func<UserInfo, bool>> predicate );
  }
}
