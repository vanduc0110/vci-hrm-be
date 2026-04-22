using AutoMapper;
using ClosedXML.Excel;
using System.Globalization;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Repositories;
using TTDesign.API.Domain.Services;
using TTDesign.API.Extensions;
using TTDesign.API.Resources;

namespace TTDesign.API.Services
{
  public class FingerPrinterService : GenericService<FingerPrinter>, IFingerPrinterService
  {
    private readonly IFingerPrinterRepository _fingerPrinterRepository;
    private readonly ITimesheetRepository _timesheetRepository;
    private readonly ILogger<FingerPrinterService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ISwapDayReferRepository _swapDayReferRepository;
    private readonly ISwapDayUserRepository _swapDayUserRepository;
    private readonly ITimesheetService _timesheetService;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    private readonly IUserInfoRepository _userInfoRepository;
    private readonly IFingerPrinterLogRepository _fingerPrinterLogRepository;

    public FingerPrinterService( IFingerPrinterRepository fingerPrinterRepository,
      ILogger<FingerPrinterService> logger,
      IUserRepository userRepository,
      ITimesheetRepository timesheetRepository,
      ISwapDayReferRepository swapDayReferRepository,
      ISwapDayUserRepository swapDayUserRepository,
      ITimesheetService timesheetService,
      IWebHostEnvironment env,
      IUserInfoRepository userInfoRepository,
      IMapper mapper,
      IFingerPrinterLogRepository fingerPrinterLogRepository ) : base( fingerPrinterRepository )
    {
      _fingerPrinterRepository = fingerPrinterRepository;
      _logger = logger;
      _userRepository = userRepository;
      _mapper = mapper;
      _timesheetRepository = timesheetRepository;
      _swapDayReferRepository = swapDayReferRepository;
      _swapDayUserRepository = swapDayUserRepository;
      _timesheetService = timesheetService;
      _userInfoRepository = userInfoRepository;
      _env = env;
      _fingerPrinterLogRepository = fingerPrinterLogRepository;
    }

    public async Task<IEnumerable<FingerPrinterResponse>> GetList( BaseFilter filter )
    {
      var responses = await _fingerPrinterRepository.GetFingerPrinterByFilter( filter );
      var editors = new Dictionary<long, string>();
      foreach ( var record in responses ) {
        // set editor info
        if ( editors.ContainsKey( record.ModifiedBy ) ) {
          record.ModifierName = editors [ record.ModifiedBy ];
        }
        else {
          var editor = await _userRepository.GetByConditionNoTrack( u => u.Id == record.ModifiedBy );
          if ( editor != null ) {
            editors.Add( record.ModifiedBy, editor.UserName );
            record.ModifierName = editor.UserName;
          }
        }
      }
      return responses;
    }

    public async Task Update( FingerPrinterResource resource, long editor )
    {
      var old = await _fingerPrinterRepository.GetByCondition( f => f.Id == resource.Id );
      var user = await _userRepository.GetByCondition( u => u.Id == editor );
      await _fingerPrinterLogRepository.CreateAsync( new FingerPrinterLog()
      {
        TimesheetId = old.TimesheetId,
        DateIn = old.DateIn,
        DateOut = old.DateOut,
        DateInUpdate = resource.DateTimeIn,
        DateOutUpdate = resource.DateTimeOut,
        UpdatedBy = user!.FullName,
        CreatedDate = DateTime.UtcNow,
        CreatedBy = editor
      } );

      var dateWorkTime = Common.TotalTimeWorkExcludeWeekend( resource.DateTimeIn, resource.DateTimeOut, "hour" );
      old!.DateIn = resource.DateTimeIn;
      old.DateOut = resource.DateTimeOut;
      old.Note = resource.Note;
      old.ModifiedBy = editor;
      old.HourTotal = dateWorkTime.Sum( t => t.Hours );
      _fingerPrinterRepository.Update( old );
      // kiểm tra xem có phải ngày hoán đổi không
      var ts = await _timesheetRepository.GetByConditionNoTrack( t => t.Id == old.TimesheetId );
      if ( ts != null && ts.SwapDay != null ) {
        var tsSwap = await _fingerPrinterRepository.GetByConditionNoTrack( f => f.Timesheet.Date == ts.SwapDay && f.Timesheet.UserId == ts.UserId );
        var swapDayRefer = await _swapDayReferRepository.GetByConditionNoTrack( s => s.FingerPrinterId == tsSwap!.Id );
        swapDayRefer!.DateIn = resource.DateTimeIn;
        swapDayRefer.DateOut = resource.DateTimeOut;
        swapDayRefer.HourTotal = dateWorkTime.Sum( t => t.Hours );
        _swapDayReferRepository.Update( swapDayRefer );
      }

    }

    public async Task<long> Create( FingerPrinterResource resource, long creator )
    {
      var user = await _userRepository.GetByCondition( u => u.Id == resource.UserId );
      var dateWorkTime = Common.TotalTimeWorkExcludeWeekend( resource.DateTimeIn, resource.DateTimeOut, "hour" );
      var timesheet = new Timesheet()
      {
        UserId = user!.Id,
        User = user,
        Date = resource.DateTimeIn.Date,
        CreatedBy = creator,
        ModifiedBy = creator,
        FingerPrinter = new FingerPrinter()
        {
          DateIn = resource.DateTimeIn,
          DateOut = resource.DateTimeOut,
          Note = resource.Note,
          HourTotal = dateWorkTime.Sum( t => t.Hours ),
          CreatedBy = creator,
          ModifiedBy = creator,
        }
      };
      await _timesheetRepository.CreateAsync( timesheet );
      // kiểm tra xem ngày timesheet có phải là ngày hoán đổi ko
      var swapDay = await _swapDayUserRepository.GetByConditionNoTrack( s => s.SwapDay.FromDate == resource.DateTimeIn.Date && s.UserId == resource.UserId );
      if ( swapDay != null ) {
        var tsSwap = await _fingerPrinterRepository.GetByConditionNoTrack( f => f.Timesheet.Date == resource.DateTimeIn.Date && f.Timesheet.UserId == resource.UserId );
        var swapDayRefer = await _swapDayReferRepository.GetByConditionNoTrack( s => s.FingerPrinterId == tsSwap!.Id );
        swapDayRefer!.DateIn = resource.DateTimeIn;
        swapDayRefer.DateOut = resource.DateTimeOut;
        swapDayRefer.HourTotal = dateWorkTime.Sum( t => t.Hours );
        _swapDayReferRepository.Update( swapDayRefer );
      }
      // nếu mà mới tạo finger printer thì chắc chắn là chưa có work log, nên ko cần tính toán lại overtime
      return 0;
    }

    public async Task<bool> CheckTimesheetHadLock( long id, DateTime? date = null )
    {
      var timesheet = id == 0 ? await _timesheetRepository.GetByConditionNoTrack( t => t.Date == date ) :
        await _timesheetRepository.GetByConditionNoTrack( t => t.FingerPrinter.Id == id );
      return timesheet is not null && timesheet.LockBy is not null;
    }

    public async Task Delete( long id, long editor )
    {
      var old = await _fingerPrinterRepository.GetByCondition( f => f.Id == id );
      old!.DateIn = old.DateOut = old.DateIn.Date;
      old.Note = string.Empty;
      old.ModifiedBy = editor;
      _fingerPrinterRepository.Update( old );
    }

    public async Task UpdateFingureExcel( IFormFile file, long editor )
    {
      var viCulture = new CultureInfo( "vi-VN" );
      CultureInfo.DefaultThreadCurrentCulture = viCulture;
      CultureInfo.DefaultThreadCurrentUICulture = viCulture;
      Thread.CurrentThread.CurrentCulture = viCulture;
      Thread.CurrentThread.CurrentUICulture = viCulture;
      var fileName = $"{file.FileName}";
      var path = Path.GetFullPath( Path.Combine( _env.WebRootPath, "Excel" ) );
      if ( !Directory.Exists( path ) ) {
        Directory.CreateDirectory( path );
      }
      if ( System.IO.File.Exists( Path.Combine( path, fileName ) ) ) {
        System.IO.File.Delete( Path.Combine( path, fileName ) );
      }
      using ( var fileStream = new FileStream( Path.Combine( path, fileName ), FileMode.Create ) ) {
        await file.CopyToAsync( fileStream );
      }
      var result = new List<FingerMachineFromMachine>();
      var filePath = Path.Combine( path, fileName );

      using ( var workbook = new XLWorkbook( filePath ) ) {
        var worksheet = workbook.Worksheet( 1 );
        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
        for ( int row = 6; row <= lastRow; row++ ) {
          try {
            var employee = new FingerMachineFromMachine
            {
              Code = worksheet.Cell( row, 2 ).GetString()?.Trim()!,
              Name = worksheet.Cell( row, 3 ).GetString()?.Trim()!,
            };

            var date = worksheet.Cell( row, 6 );
            if ( DateTime.TryParse( date.GetString(), out DateTime ngayParsed ) ) {
              employee.Date = ngayParsed;
            }

            var timein = worksheet.Cell( row, 7 );
            var timinStr = timein.GetString()?.Trim() ?? string.Empty;
            if ( DateTime.TryParse( timinStr, out DateTime thoiGianVaoParsed ) ) {
              employee.TimeIn = thoiGianVaoParsed.TimeOfDay;
            }

            var timeout = worksheet.Cell( row, 8 );
            if ( DateTime.TryParse( timeout.GetString(), out DateTime thoiGianRaParsed ) ) {
              employee.TimeOut = ngayParsed.Date == DateTime.UtcNow.Date ? null : thoiGianRaParsed.TimeOfDay;
            }
            result.Add( employee );
          }
          catch ( Exception ex ) {
            Console.WriteLine( $"Lỗi xử lý dòng {row}: {ex.Message}" );
          }
        }
      }
      var summary = result
            .Where( e => !string.IsNullOrEmpty( e.Code ) )
            .GroupBy( e => new { e.Code, e.Date } )
            .Select( g =>
              {
                var timeInRecords = g.Where( x => x.TimeIn.HasValue ).ToList();
                var timeOutRecords = g.Where( x => x.TimeOut.HasValue ).ToList();
                var allTime = new List<TimeSpan>();

                allTime.AddRange( timeInRecords.Where( x => x.TimeIn.HasValue ).Select( x => x.TimeIn!.Value ) );
                allTime.AddRange( timeInRecords.Where( x => x.TimeOut.HasValue ).Select( x => x.TimeOut!.Value ) );
                allTime.AddRange( timeOutRecords.Where( x => x.TimeIn.HasValue ).Select( x => x.TimeIn!.Value ) );
                allTime.AddRange( timeOutRecords.Where( x => x.TimeOut.HasValue ).Select( x => x.TimeOut!.Value ) );
                return new FingerMachineFromMachine
                {
                  Code = g.Key.Code,
                  TimeIn = allTime.Any() ? allTime.Min( x => x ) : null,
                  TimeOut = allTime.Any() ? allTime.Max( x => x ) : null,
                  Date = g.Key.Date,

                };
              } )
            .OrderBy( s => s.Code )
            .ToList();
      foreach ( var data in summary ) {
        var userInfo = await _userInfoRepository.GetByConditionNoTrack( u => u.FingerId == Convert.ToInt32( data.Code ) );
        var user = userInfo == null ? null : await _userRepository.GetByConditionNoTrack( u => u.Id == userInfo.UserId );
        if ( user == null ) {
          continue;
        }
        var timesheets = await _timesheetRepository.GetTimesheets( t => t.Date.Date == data.Date.Date );
        var fingerPrinter = timesheets.FirstOrDefault( t => t.UserId == user.Id )?.FingerPrinter;
        if ( fingerPrinter == null ) {
          continue;
        }
        fingerPrinter!.DateIn = fingerPrinter!.DateIn.TimeOfDay == TimeSpan.Zero && data.TimeIn != null ? data.Date.Add( data.TimeIn.Value ) : fingerPrinter!.DateIn;
        fingerPrinter.DateOut = data.TimeOut != null ? data.Date.Add( data.TimeOut.Value ) : fingerPrinter!.DateOut;
        fingerPrinter.ModifiedBy = editor;
        fingerPrinter.HourTotal = data.TimeOut != null ? Common.TotalTimeWorkExcludeWeekend( fingerPrinter!.DateIn, fingerPrinter.DateOut ) : 0;
        fingerPrinter.Note = "run script excel";
        _fingerPrinterRepository.Update( fingerPrinter );
      }

      System.IO.File.Delete( Path.Combine( path, fileName ) );
    }
  }
}
