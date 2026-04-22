using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class UserInfoRepository : GenericRepository<UserInfo>, IUserInfoRepository
  {
    public UserInfoRepository( AppDbContext context ) : base( context )
    {

    }

    public async Task<UserInfo?> GetUserByCondition( Expression<Func<UserInfo, bool>> predicate )
    {
      return await _context.UserInfos.FirstOrDefaultAsync( predicate );
    }
  }
}
