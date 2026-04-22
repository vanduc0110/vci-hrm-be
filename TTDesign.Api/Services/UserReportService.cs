using AutoMapper;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class UserReportService : GenericService<UserReport>, IUserReportService
  {
    private readonly IUserReportRepository _userReportRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public UserReportService( IUserReportRepository userReportRepository, IMapper mapper, IUserRepository userRepository ) : base( userReportRepository )
    {
      _userReportRepository = userReportRepository;
      _mapper = mapper;
      _userRepository = userRepository;
    }

    public async Task<long> Create( UserReportResource obj, long creator )
    {
      var userReport = _mapper.Map<UserReport>( obj );
      userReport.CreatedBy = creator;
      await _userReportRepository.CreateAsync( userReport );
      return userReport.Id;
    }

    public async Task<IEnumerable<UserReportResponse>> GetList( BaseFilter filter )
    {
      var reports = await _userReportRepository.GetListByCondition( x => x.CreatedDate.Year == filter.DateCheck.Year && x.CreatedDate.Month == filter.DateCheck.Month );
      if ( filter.DateCheck.Date == new DateTime( 2001, 1, 1 ) ) {
        reports = await _userReportRepository.GetAll();
      }
      if ( filter.Status != -1 ) {
        reports = reports.Where( x => x.Status == filter.Status );
      }
      var results = _mapper.Map<IEnumerable<UserReportResponse>>( reports );
      foreach ( var rp in results ) {
        if ( rp.ModifiedBy != 0 ) {
          var user = await _userRepository.GetByConditionNoTrack( x => x.Id == rp.ModifiedBy );
          rp.ModifiedName = user!.FullName;
        }
        var userCr = await _userRepository.GetByConditionNoTrack( x => x.Id == rp.CreatedBy );
        rp.CreatedName = userCr!.FullName;
      }
      return results;
    }

    public async Task Update( UserReportResource resouce, long editor )
    {
      var old = await _userReportRepository.GetByCondition( c => c.Id == resouce.Id );
      _mapper.Map( resouce, old );
      old!.ModifiedBy = editor;
      _userReportRepository.Update( old );
    }

    public async Task UpdateStatus( long id, long editor, int status )
    {
      var old = await _userReportRepository.GetByCondition( c => c.Id == id );
      old!.Status = status;
      old!.ModifiedBy = editor;
      _userReportRepository.Update( old );
    }
  }
}
