using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class UserTaskService : GenericService<UserTask>, IUserTaskService
  {
    private readonly IUserTaskRepository _userTaskRepository;
    private readonly IMapper _mapper;
    public UserTaskService( IUserTaskRepository userTaskRepository, IMapper mapper ) : base( userTaskRepository )
    {
      _userTaskRepository = userTaskRepository;
      _mapper = mapper;
    }

    public Task<bool> CheckUserTaskBeforeDelete( long id )
    {
      return _userTaskRepository.Exist( t => t.Id == id && t.Status != Constants.Enums.StatusMark.Current );
    }

    public async Task<long> Create( UserTaskResource obj, long creator )
    {
      var userTask = _mapper.Map<UserTask>( obj );
      userTask.CreatedBy = creator;
      if ( !Enum.IsDefined( typeof( Constants.Enums.StatusMark ), obj.Status ) ) {
        userTask.Status = ( Constants.Enums.StatusMark ) Enum.Parse( typeof( Constants.Enums.StatusMark ), obj.Status );
      }
      await _userTaskRepository.CreateAsync( userTask );
      return userTask.Id;
    }

    public async Task<UserTaskResponse> GetCurrentTask( DateTime dateCheck, long id )
    {
      if ( dateCheck == new DateTime( 2001, 1, 1 ) ) {
        var taskAll = await _userTaskRepository.GetByCondition( c => c.Status == Constants.Enums.StatusMark.Current && c.CreatedBy == id );

        return _mapper.Map<UserTaskResponse>( taskAll );
      }
      var task = await _userTaskRepository.GetByCondition( c => c.Status == Constants.Enums.StatusMark.Current
      && c.CreatedDate.Month == dateCheck.Month && c.CreatedDate.Date.Year == dateCheck.Year && c.CreatedBy == id );
      return _mapper.Map<UserTaskResponse>( task );
    }

    public async Task<IEnumerable<UserTaskResponse>> GetList( BaseFilter filter )
    {
      if ( filter.DateCheck == new DateTime( 2001, 1, 1 ) ) {
        var tasksAll = await _userTaskRepository.GetListByCondition( x => x.CreatedBy == filter.UserId );
        return _mapper.Map<IEnumerable<UserTaskResponse>>( tasksAll );

      }
      var tasks = await _userTaskRepository.GetListByCondition( x => x.CreatedDate.Month == filter.DateCheck.Month
      && x.CreatedDate.Date.Year == filter.DateCheck.Year && x.CreatedBy == filter.UserId );
      return _mapper.Map<IEnumerable<UserTaskResponse>>( tasks );
    }

    public async Task Update( UserTaskResource resouce, long editor )
    {
      var old = await _userTaskRepository.GetByCondition( c => c.Id == resouce.Id );
      _mapper.Map( resouce, old );
      old!.ModifiedBy = editor;
      if ( !Enum.IsDefined( typeof( Constants.Enums.UserPosition ), resouce.Status ) ) {
        old.Status = ( Constants.Enums.StatusMark ) Enum.Parse( typeof( Constants.Enums.StatusMark ), resouce.Status );
      }
      _userTaskRepository.Update( old );
    }
  }
}
