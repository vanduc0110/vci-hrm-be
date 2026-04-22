using TTDesign.API.Domain.Models;
using TTDesign.API.Resources;

namespace TTDesign.API.Domain.Services
{
  public interface IRoleService: IGenericService<Role>, 
    BaseServiceResource<RoleResource>, BaseServiceOption<RoleOption>, BaseServiceList<RoleResponse>, BaseServiceDetail<RoleDetailResponse>
  {
  }
}
