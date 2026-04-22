using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Persistence.Repositories
{
  public class UserSettingRepository : GenericRepository<UserSetting>, IUserSettingRepository
  {
    public UserSettingRepository( AppDbContext context ) : base( context )
    {

    }
  }
}
