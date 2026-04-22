using AutoMapper;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Mapping
{
  public class ResourceToModelProfile : Profile
  {
    public ResourceToModelProfile()
    {
      #region Team
      CreateMap<TeamResource, Team>();
      #endregion     
      #region Category
      CreateMap<CategoryResource, TeamCategory>();
      #endregion
      #region Object
      #endregion
      #region User
      CreateMap<UserResource, User>()
        .ForMember( a => a.UserName, opt => opt.MapFrom( a => Common.FormatUserNameFromFullName( a.FullName ) ) )
        .ForMember( a => a.Position, opt => opt.MapFrom( a => !Enum.IsDefined( typeof( Constants.Enums.UserPosition ), a.Position ) ?
        ( int ) Constants.Enums.UserPosition.Internship : ( int ) Enum.Parse( typeof( Constants.Enums.UserPosition ), a.Position ) ) );
      CreateMap<UserResource, UserInfo>();
      CreateMap<YourSelfResource, UserInfo>();
      CreateMap<UserSettingResource, UserSetting>();
      #endregion
      #region Role
      #endregion
      #region Project
      CreateMap<ProjectResource, Project>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => !Enum.IsDefined( typeof( Enums.ProjectStatus ), a.Status! ) ?
        ( int ) Constants.Enums.ProjectStatus.Pending : ( int ) Enum.Parse( typeof( Enums.ProjectStatus ), a.Status! ) ) );
      #endregion    
      #region leave
      CreateMap<LeaveRequestResource, LeaveRequest>()
        .ForMember( a => a.Hour, opt => opt.MapFrom( a => a.Hours ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => ( int ) Enum.Parse( typeof( Enums.LeaveType ), a.Type ) ) );
      CreateMap<LeaveRequestDetailData, LeaveRequestDetail>();
      #endregion
      #region wfh
      CreateMap<WfhResource, WfhRequest>();
      #endregion
      #region holiday
      CreateMap<HolidayResource, Holiday>()
        .ForMember( a => a.HolidayApplys, opt => opt.MapFrom( a => a.ApplyFor!.Select( o => new HolidayApply() { Type = o.Type, ApplyId = o.Id } ) ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => ( int ) Enum.Parse( typeof( Enums.HolidayType ), a.Type ) ) );
      #endregion 
     
      #region Client
      CreateMap<ClientResource, Client>();
      #endregion

      #region Config
      CreateMap<ConfigResource, Config>();
      #endregion
      CreateMap<ProjectContractResource, ProjectContract>()
        .ForMember( a => a.Code, opt => opt.MapFrom( a => a.ContractCode ) )
        .ForMember( a => a.Name, opt => opt.MapFrom( a => a.ContractName ) )
        .ForMember( a => a.Date, opt => opt.MapFrom( a => a.ContractDate ) );

      CreateMap<UserTaskResource, UserTask>();
      CreateMap<UserReportResource, UserReport>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.Parse( typeof( Enums.ReportStatus ), a.Status ) ) )
        .ForMember( a => a.Type, opt => opt.MapFrom( a => Enum.Parse( typeof( Enums.ReportType ), a.Type ) ) );

      CreateMap<AssetCategoryResource, AssetCategory>();
      CreateMap<AssetResource, Asset>()
        .ForMember( a => a.Status, opt => opt.MapFrom( a => Enum.Parse( typeof( Enums.AssetStatus ), a.Status ) ) )
        .ForMember( a => a.Condition, opt => opt.MapFrom( a => Enum.Parse( typeof( Enums.AssetCondition ), a.Condition ) ) );
      CreateMap<ComputerSpecs, AssetDetailResponse>().ReverseMap();
      CreateMap<MonitorSpecs, AssetDetailResponse>().ReverseMap();
      CreateMap<ComponentSpecs, AssetDetailResponse>().ReverseMap();
    }
  }
}
