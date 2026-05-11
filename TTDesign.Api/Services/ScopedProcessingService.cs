using Microsoft.EntityFrameworkCore;
using TTDesign.API.Constants;
using TTDesign.API.Domain.Models;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.Services
{
  internal interface IScopedProcessingService
  {
    Task DoWork( CancellationToken stoppingToken, AppDbContext _context );
    Task MatchingFingerMachine( CancellationToken stoppingToken, AppDbContext _context );
    Task MatchingFingerMachineDateOut( CancellationToken stoppingToken, AppDbContext _context );
  }

  internal class ScopedProcessingService : IScopedProcessingService
  {
    private int executionCount = 0;
    private readonly ILogger _logger;
    //private IMongoCollection<ProductInfo> _productInfoCollection;

    public ScopedProcessingService( ILogger<ScopedProcessingService> logger )
    {
      _logger = logger;
    }

    public async Task DoWork( CancellationToken stoppingToken, AppDbContext _context )
    {
      while ( !stoppingToken.IsCancellationRequested ) {
        executionCount++;

        _logger.LogInformation( "Scoped Processing Service is working. Count: {Count}", executionCount );

        #region processing
        await DefineTimesheet( _context );
        await AddLeave( _context );
        await TakeBackLeave( _context );
        await ApplyHoliday( _context );
        await CleanNotification( _context );
        await AddLeaveOffice( _context );
        #endregion
        var nextRun = DateTime.UtcNow.Date.AddDays( 1 ).AddHours( 1 );
        await Task.Delay( ( int ) ( ( nextRun - DateTime.UtcNow ).TotalSeconds * 1000 ), stoppingToken ); // sleep one day, next time run is 1 a.m next day
        //await Task.Delay( 60000 );
        //stoppingToken.ThrowIfCancellationRequested();
      }
    }
    public async Task MatchingFingerMachine( CancellationToken stoppingToken, AppDbContext _context )
    {
      while ( !stoppingToken.IsCancellationRequested ) {
        //var now = DateTime.UtcNow;
        //var nextRun = now.Date.AddHours( 23 );

        //if ( now > nextRun )
        //  nextRun = nextRun.AddDays( 1 );

        //var delay = nextRun - now;
        //await Task.Delay( delay, stoppingToken );
        executionCount++;
        // lấy danh sách record FingerPrinter kèm FingerId cần cập nhật thông tin
        var fingerIds = _context.Timesheets
                   .Include( x => x.FingerPrinter )
                   .Where( t => t.Date == DateTime.UtcNow.Date )
                   .Select( t => new
                   {
                     FingerId = _context.UserInfos
                           .Where( u => u.UserId == t.UserId )
                           .Select( u => u.FingerId )
                           .FirstOrDefault(),
                     Id = t.Id
                   } ).ToDictionary( x => x.FingerId, x => x.Id );
        #region swap day
        // lấy danh sách Swap Day (ngày hoán đổi) cần apply
        var swapDayIds = _context.Timesheets
                    .Include( t => t.FingerPrinter )
                        .ThenInclude( f => f.SwapDayRefer )
                    .Where( t => _context.Timesheets
                        .Any( tmp => tmp.Date == DateTime.UtcNow.Date &&
                                   tmp.SwapDay != null &&
                                   tmp.UserId == t.UserId &&
                                   tmp.SwapDay == t.Date ) )
                    .Select( t => new
                    {
                      TmpId = _context.Timesheets
                            .Where( tmp => tmp.Date == DateTime.UtcNow.Date &&
                                         tmp.SwapDay != null &&
                                         tmp.UserId == t.UserId &&
                                         tmp.SwapDay == t.Date )
                            .Select( tmp => tmp.Id )
                            .First(),
                      SwapDayReferId = t.FingerPrinter!.SwapDayRefer!.Id,
                      Date = t.Date
                    } ).ToDictionary( x => x.SwapDayReferId, x => new object [] { x.Date, x.TmpId } );
        #endregion

        var dataFingreMachines = await _context.FingerDataMachines
            .Where( x => x.PunchDate.Date == DateTime.UtcNow.Date )
            .GroupBy( x => new { x.EmpId, Date = x.PunchDate.Date } )
            .Select( g => new
            {
              g.Key.EmpId,
              g.Key.Date,
              TimeIn = g.Min( x => x.PunchDate ),
              TimeOut = g.Max( x => x.PunchDate )
            } )
            .ToListAsync();
        var fingerPrinters = new List<FingerPrinter>();
        foreach ( var item in fingerIds ) {
          var dataFingreMachine = dataFingreMachines.FirstOrDefault( x => x.EmpId == item.Key );
          if ( dataFingreMachine == null )
            continue;
          var fingerPrinter = await _context.FingerPrinters
            .Where( x => x.TimesheetId == item.Value )
            .FirstOrDefaultAsync();
          fingerPrinter!.DateIn = dataFingreMachine!.TimeIn;
          fingerPrinter!.Note = "async data";
          fingerPrinters.Add( fingerPrinter );
        }
        if ( fingerPrinters.Count() > 0 ) {
          _context.FingerPrinters.UpdateRange( fingerPrinters );
          await _context.SaveChangesAsync();
        }

        await Task.Delay( TimeSpan.FromHours( 1 ), stoppingToken );

        #region xử lý swap day
        //if ( swapDayIds != null && swapDayIds.Count() > 0 ) {
        //  var swapDayRefer = new List<SwapDayRefer>();
        //  foreach ( var item in swapDayIds ) {
        //    var dataFingreMachine = dataFingreMachines.FirstOrDefault( x => x.EmpId == item[] );
        //    if ( dataFingreMachine == null )
        //      continue;
        //    var fingerPrinter = await _context.FingerPrinters
        //      .Where( x => x.TimesheetId == item.Value )
        //      .FirstOrDefaultAsync();
        //    fingerPrinter!.DateIn = dataFingreMachine!.TimeIn;
        //    fingerPrinter!.DateOut = dataFingreMachine!.TimeOut;
        //    fingerPrinter!.Note = "async data";
        //    fingerPrinter!.HourTotal = TotalTimeWorkExcludeWeekend( dataFingreMachine!.TimeIn, dataFingreMachine!.TimeOut );
        //    fingerPrinters.Add( fingerPrinter );
        //  }
        //  _context.FingerPrinters.UpdateRange( fingerPrinters );
        //  await _context.SaveChangesAsync();
        //}
        #endregion
      }
    }

    public async Task MatchingFingerMachineDateOut( CancellationToken stoppingToken, AppDbContext _context )
    {
      while ( !stoppingToken.IsCancellationRequested ) {
        executionCount++;
        // lấy danh sách record FingerPrinter kèm FingerId cần cập nhật thông tin
        var fingerIds = _context.Timesheets
                   .Include( x => x.FingerPrinter )
                   .Where( t => t.Date == DateTime.UtcNow.Date.AddDays( -1 ) )
                   .Select( t => new
                   {
                     FingerId = _context.UserInfos
                           .Where( u => u.UserId == t.UserId )
                           .Select( u => u.FingerId )
                           .FirstOrDefault(),
                     Id = t.Id
                   } ).ToDictionary( x => x.FingerId, x => x.Id );
        #region swap day
        // lấy danh sách Swap Day (ngày hoán đổi) cần apply
        //var swapDayIds = _context.Timesheets
        //            .Include( t => t.FingerPrinter )
        //                .ThenInclude( f => f.SwapDayRefer )
        //            .Where( t => _context.Timesheets
        //                .Any( tmp => tmp.Date == DateTime.UtcNow.Date &&
        //                           tmp.SwapDay != null &&
        //                           tmp.UserId == t.UserId &&
        //                           tmp.SwapDay == t.Date ) )
        //            .Select( t => new
        //            {
        //              TmpId = _context.Timesheets
        //                    .Where( tmp => tmp.Date == DateTime.UtcNow.Date &&
        //                                 tmp.SwapDay != null &&
        //                                 tmp.UserId == t.UserId &&
        //                                 tmp.SwapDay == t.Date )
        //                    .Select( tmp => tmp.Id )
        //                    .First(),
        //              SwapDayReferId = t.FingerPrinter!.SwapDayRefer!.Id,
        //              Date = t.Date
        //            } ).ToDictionary( x => x.SwapDayReferId, x => new object [] { x.Date, x.TmpId } );
        #endregion

        var dataFingreMachines = await _context.FingerDataMachines
            .Where( x => x.PunchDate.Date == DateTime.UtcNow.Date.AddDays( -1 ) )
            .GroupBy( x => new { x.EmpId, Date = x.PunchDate.Date } )
            .Select( g => new
            {
              g.Key.EmpId,
              g.Key.Date,
              TimeIn = g.Min( x => x.PunchDate ),
              TimeOut = g.Max( x => x.PunchDate )
            } )
            .ToListAsync();
        var fingerPrinters = new List<FingerPrinter>();
        foreach ( var item in fingerIds ) {
          var dataFingreMachine = dataFingreMachines.FirstOrDefault( x => x.EmpId == item.Key );
          if ( dataFingreMachine == null )
            continue;
          var fingerPrinter = await _context.FingerPrinters
            .Where( x => x.TimesheetId == item.Value )
            .FirstOrDefaultAsync();

          fingerPrinter!.DateOut = dataFingreMachine!.TimeOut;
          fingerPrinter!.HourTotal = TotalTimeWorkExcludeWeekend( dataFingreMachine!.TimeIn, dataFingreMachine!.TimeOut );
          fingerPrinters.Add( fingerPrinter );
        }
        if ( fingerPrinters.Count() > 0 ) {
          _context.FingerPrinters.UpdateRange( fingerPrinters );
          await _context.SaveChangesAsync();
        }

        #region xử lý swap day
        //if ( swapDayIds != null && swapDayIds.Count() > 0 ) {
        //  var swapDayRefer = new List<SwapDayRefer>();
        //  foreach ( var item in swapDayIds ) {
        //    var dataFingreMachine = dataFingreMachines.FirstOrDefault( x => x.EmpId == item[] );
        //    if ( dataFingreMachine == null )
        //      continue;
        //    var fingerPrinter = await _context.FingerPrinters
        //      .Where( x => x.TimesheetId == item.Value )
        //      .FirstOrDefaultAsync();
        //    fingerPrinter!.DateIn = dataFingreMachine!.TimeIn;
        //    fingerPrinter!.DateOut = dataFingreMachine!.TimeOut;
        //    fingerPrinter!.Note = "async data";
        //    fingerPrinter!.HourTotal = TotalTimeWorkExcludeWeekend( dataFingreMachine!.TimeIn, dataFingreMachine!.TimeOut );
        //    fingerPrinters.Add( fingerPrinter );
        //  }
        //  _context.FingerPrinters.UpdateRange( fingerPrinters );
        //  await _context.SaveChangesAsync();
        //}
        #endregion

        var _vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById( "SE Asia Standard Time" );
        var nextMidnight = DateTime.UtcNow.Date.AddDays( 1 );
        var now = DateTime.UtcNow;
        var delay = nextMidnight - now;
        _logger.LogInformation( "Next run at: {NextRun}", delay );
        _logger.LogInformation( "VietName Zone: {nextMidnight}", nextMidnight );


        await Task.Delay( delay > TimeSpan.Zero ? delay : TimeSpan.FromDays( 1 ), stoppingToken );
      }
    }
    private double TotalTimeWorkExcludeWeekend( DateTime start, DateTime end )
    {
      double minutes;
      start = start.AddSeconds( -start.Second );
      end = end.AddSeconds( -end.Second );
      if ( start.Hour < Enums.TimeWorkEndHourMorning ) {
        minutes = end.Hour < Enums.TimeWorkEndHourMorning ? ( end - start ).TotalMinutes :
          end.Hour < Enums.TimeWorkStartHourAfternoon ? ( end.Date.AddHours( Enums.TimeWorkEndHourMorning ) - start ).TotalMinutes :
          ( end - start ).TotalMinutes - 60;
      }
      else if ( start.Hour < Enums.TimeWorkStartHourAfternoon ) {
        minutes = end.Hour < Enums.TimeWorkStartHourAfternoon ? 0 : ( end - start.Date.AddHours( Enums.TimeWorkStartHourAfternoon ) ).TotalMinutes;
      }
      else {
        minutes = ( end - start ).TotalMinutes;
      }
      int partInt = ( int ) minutes / Enums.MINIMUM_PERIOD_CALENDAR;
      return ( double ) ( partInt * Enums.MINIMUM_PERIOD_CALENDAR ) / 60;
    }
    /// <summary>
    /// Khởi tạo timesheet cho các user active
    /// giải thích logic: 
    /// - các tháng, đầu tháng sẽ tạo record timesheet cho tháng sau (tức là nay mồng 1 tháng 6, thì sẽ tạo record timesheet của tháng 7)
    /// - với các user mới, ngày hôm sau record timesheet sẽ được tạo 2 tháng kể từ ngày create (user được tạo vào mồng 10, thì ngày 11 service sẽ run, tạo record timesheet từ 10 tháng này tới ngày cuối cùng của tháng sau)
    /// </summary>
    /// <param name="_context"></param>
    /// <returns></returns>
    public async Task DefineTimesheet( AppDbContext _context )
    {
      // xử lý các hành động từ hệ thống: active user/inactive user
      var userIds = new List<long>();
      var requests = await _context.SystemRequests.Where( o => o.Type == ( int ) Enums.SystemRequestType.ActiveUser || o.Type == ( int ) Enums.SystemRequestType.InactiveUser )
        .ToListAsync();
      if ( requests.Any() ) {
        var requestValid = from request in requests
                           group request by request.UserId
                           into groups
                           select groups.OrderByDescending( o => o.Id ).First();
        var timesheetRecordAdd = new List<Timesheet>();
        var timesheetRecordRemove = new List<Timesheet>();
        foreach ( var request in requestValid ) {
          userIds.Add( request.UserId );
          if ( request.Type == ( int ) Enums.SystemRequestType.ActiveUser ) {
            var dateStart = await _context.Timesheets.Where( t => t.UserId == request.UserId ).OrderByDescending( o => o.Id ).Select( t => t.Date ).FirstOrDefaultAsync();
            var dateIndex = request.Date.AddDays( 1 - request.Date.Day );
            var endDate = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( 2 );
            do {
              if ( dateIndex > dateStart ) {
                var hadHoliday = await _context.Holidays.Include( h => h.HolidayApplys )
                  .Where( h => h.Type == ( int ) Enums.HolidayType.Holiday &&
                  h.Status == ( int ) Enums.HolidayStatus.Apply && ( h.StartDate <= dateIndex && h.EndDate >= dateIndex ) ).FirstOrDefaultAsync();
                timesheetRecordAdd.Add( new Timesheet
                {
                  UserId = request.UserId,
                  User = await _context.Users.Where( u => u.Id == request.UserId ).FirstAsync(),
                  Date = dateIndex,
                  CreatedBy = Enums.SYSTEM_ID,
                  ModifiedBy = Enums.SYSTEM_ID,
                  HolidayName = hadHoliday is not null ? hadHoliday.Name : string.Empty
                } );
              }
              dateIndex = dateIndex.AddDays( 1 );
            } while ( dateIndex < endDate );
          }
          if ( request.Type == ( int ) Enums.SystemRequestType.InactiveUser ) {
            var records = await _context.Timesheets.Include( t => t.FingerPrinter ).Where( t => t.UserId == request.UserId && t.Date > request.Date ).ToListAsync();
            timesheetRecordRemove.AddRange( records );
          }
        }
        if ( timesheetRecordAdd.Count > 0 ) {
          await _context.Timesheets.AddRangeAsync( timesheetRecordAdd );
          await _context.SaveChangesAsync();
          var fingerPrinters = timesheetRecordAdd.Select( t => new FingerPrinter
          {
            TimesheetId = t.Id, // ← Id thật từ DB
            DateIn = t.Date,
            DateOut = t.Date,
            Note = string.Empty,
            CreatedBy = Enums.SYSTEM_ID,
            ModifiedBy = Enums.SYSTEM_ID,
          } ).ToList();

          await _context.FingerPrinters.AddRangeAsync( fingerPrinters );
          await _context.SaveChangesAsync();
        }
        if ( timesheetRecordRemove.Count > 0 ) {
          _context.FingerPrinters.RemoveRange( timesheetRecordRemove.Select( t => t.FingerPrinter ) );
          _context.Timesheets.RemoveRange( timesheetRecordRemove );
        }
        _context.SystemRequests.RemoveRange( requests ); // sau khi thực hiện thành công request, xóa request đó đi
        _context.SaveChanges();
      }

      // tạo timesheet cho các user active bình thường còn lại, thực hiện vào mồng 1 mỗi tháng
      var dateCheck = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).Date;

      requests = await _context.SystemRequests.Where( o => o.Type == ( int ) Enums.SystemRequestType.DefineTimesheetNextMonth && o.Date == dateCheck ).ToListAsync();
      if ( requests.Any() ) {
        // get active users
        var users = await _context.Users.Where( u => u.Id != Enums.SYSTEM_ID && u.IsActive ).ToListAsync();
        users = users.Where( u => !userIds.Contains( u.Id ) ).ToList();
        var timesheetNewUserAdd = new List<Timesheet>();
        var dateIndex = requests [ 0 ].Date.AddMonths( 1 );
        var endDate = requests [ 0 ].Date.AddMonths( 2 );
        do {
          var hadHoliday = await _context.Holidays.Include( h => h.HolidayApplys )
            .Where( h => h.Type == ( int ) Enums.HolidayType.Holiday &&
            h.Status == ( int ) Enums.HolidayStatus.Apply && ( h.StartDate <= dateIndex && h.EndDate >= dateIndex ) ).FirstOrDefaultAsync();
          timesheetNewUserAdd.AddRange( users.Select( u => new Timesheet()
          {
            UserId = u.Id,
            User = u,
            Date = dateIndex,
            CreatedBy = Enums.SYSTEM_ID,
            ModifiedBy = Enums.SYSTEM_ID,
            HolidayName = hadHoliday is not null ? hadHoliday.Name : string.Empty
          } ) );
          dateIndex = dateIndex.AddDays( 1 );
        } while ( dateIndex < endDate );
        if ( timesheetNewUserAdd.Count > 0 ) {
          await _context.Timesheets.AddRangeAsync( timesheetNewUserAdd );
          await _context.SaveChangesAsync();
          var fingerPrinters = timesheetNewUserAdd.Select( t => new FingerPrinter
          {
            TimesheetId = t.Id, // ← Id thật từ DB
            DateIn = t.Date,
            DateOut = t.Date,
            Note = string.Empty,
            CreatedBy = Enums.SYSTEM_ID,
            ModifiedBy = Enums.SYSTEM_ID,
          } ).ToList();

          await _context.FingerPrinters.AddRangeAsync( fingerPrinters );
          await _context.SaveChangesAsync();
        }
        _context.SystemRequests.RemoveRange( requests ); // sau khi thực hiện thành công request, xóa request đó đi
        _context.SaveChanges();
      }
      // create system request next time
      if ( DateTime.UtcNow.Day >= DateTime.UtcNow.AddDays( -DateTime.UtcNow.Day - 7 ).AddMonths( 1 ).Day ) {
        var nextMonth = DateTime.UtcNow.Date.AddDays( 1 - DateTime.UtcNow.Day ).AddMonths( 1 );
        if ( !await _context.SystemRequests.AnyAsync( o => o.Type == ( int ) Enums.SystemRequestType.DefineTimesheetNextMonth && o.Date == nextMonth ) ) {
          await _context.SystemRequests.AddAsync( new SystemRequest
          {
            UserId = 0,
            Type = ( int ) Enums.SystemRequestType.DefineTimesheetNextMonth,
            Date = nextMonth,
            ObjectId = 0,
          } );
          _context.SaveChanges();
        }
      }
    }

    /// <summary>
    /// thêm ngày nghỉ tự động hằng tháng cho nhân viên
    /// </summary>
    /// <param name="_context"></param>
    /// <returns></returns> 
    public async Task AddLeave( AppDbContext _context )
    {
      // check hệ thống có request yêu cầu không
      var dateCheck = DateTime.UtcNow.Date;
      var requests = await _context.SystemRequests.Where( o => o.Type == ( int ) Enums.SystemRequestType.DefineAnnualLeaveNextMonth && o.Date == dateCheck ).ToListAsync();
      if ( requests.Any() ) {
        // get active users
        var users = await _context.Users.Where( u => u.Id != Enums.SYSTEM_ID && u.IsActive &&
          u.Position != ( int ) Enums.UserPosition.Internship && u.Position != ( int ) Enums.UserPosition.Probationary ).ToListAsync();
        var leaves = users.ToList().Select( u => new Leave()
        {
          UserId = u.Id,
          Type = ( int ) Enums.LeaveType.Annual,
          Date = DateTime.UtcNow.AddDays( 1 - DateTime.UtcNow.Day ).Date,
          Hours = 8,
          CreatedBy = Enums.SYSTEM_ID,
          ModifiedBy = Enums.SYSTEM_ID,
        } );
        _context.Leaves.AddRange( leaves );
        // TODO tạo history
        var histories = new List<LeaveHistory>();
        foreach ( var user in users ) {
          var lastHistory = await _context.LeaveHistorys.Where( l => l.CreatedBy == user.Id ).OrderByDescending( t => t.Id ).FirstOrDefaultAsync();
          histories.Add( new LeaveHistory()
          {
            CreatedBy = user.Id,
            Description = $"Add {Enum.GetName( Enums.LeaveType.Annual )} of month {DateTime.UtcNow.Year}-{DateTime.UtcNow.Month}",
            Type = ( int ) Enums.LeaveHistoryType.AddAnnualLeave,
            Unit = 8,
            AnnualLeave = ( lastHistory != null ? lastHistory.AnnualLeave : 0 ) + 1,
          } );
        }
        if ( histories.Count > 0 ) {
          _context.LeaveHistorys.AddRange( histories );
        }
        _context.SystemRequests.RemoveRange( requests ); // sau khi thực hiện thành công request, xóa request đó đi
        _context.SaveChanges();
      }
      // create system request next time
      if ( DateTime.UtcNow.Day >= DateTime.UtcNow.AddDays( -DateTime.UtcNow.Date.Day - 7 ).AddMonths( 1 ).Day ) {
        // Keep UTC kind to avoid Npgsql timestamptz parameter errors.
        var nextMonth = DateTime.SpecifyKind( new DateTime( DateTime.UtcNow.Year, DateTime.UtcNow.Month, 16 ), DateTimeKind.Utc ).AddMonths( 1 );
        if ( !await _context.SystemRequests.AnyAsync( o => o.Type == ( int ) Enums.SystemRequestType.DefineAnnualLeaveNextMonth && o.Date == nextMonth ) ) {
          await _context.SystemRequests.AddAsync( new SystemRequest
          {
            UserId = 0,
            Type = ( int ) Enums.SystemRequestType.DefineAnnualLeaveNextMonth,
            Date = nextMonth,
            ObjectId = 0,
          } );
          _context.SaveChanges();
        }
      }
    }

    /// <summary>
    /// thêm ngày nghỉ tự động hằng tháng cho nhân viên mới lên chính thức
    /// </summary>
    /// <param name="_context"></param>
    /// <returns></returns>
    public async Task AddLeaveOffice( AppDbContext _context )
    {
      // check hệ thống có request yêu cầu không
      var dateCheck = DateTime.UtcNow.Date;

      var requests = await _context.SystemRequests.Where( o => o.Type == ( int ) Enums.SystemRequestType.DefineAnnualLeaveToOffical && o.Date == dateCheck ).ToListAsync();
      if ( requests.Any() ) {
        // get active users
        var users = await _context.Users.Where( u => requests.Select( x => x.UserId ).Contains( u.Id ) ).ToListAsync();
        var leaves = users.ToList().Select( u => new Leave()
        {
          UserId = u.Id,
          Type = ( int ) Enums.LeaveType.Annual,
          Date = DateTime.UtcNow.Date,
          Hours = 8 * CalMonths( u.DateStartWork!.Value, DateTime.UtcNow.Date ),
          CreatedBy = Enums.SYSTEM_ID,
          ModifiedBy = Enums.SYSTEM_ID,
        } );
        _context.Leaves.AddRange( leaves );
        // TODO tạo history
        var histories = new List<LeaveHistory>();
        foreach ( var user in users ) {
          var lastHistory = await _context.LeaveHistorys.Where( l => l.CreatedBy == user.Id ).OrderByDescending( t => t.Id ).FirstOrDefaultAsync();
          histories.Add( new LeaveHistory()
          {
            CreatedBy = user.Id,
            Description = $"Add {Enum.GetName( Enums.LeaveType.Annual )} of month {DateTime.UtcNow.AddMonths( -1 ).Year}-{DateTime.UtcNow.AddMonths( -1 ).Month}_Offical",
            Type = ( int ) Enums.LeaveHistoryType.AddAnnualLeave,
            Unit = 16,
            AnnualLeave = ( lastHistory != null ? lastHistory.AnnualLeave : 0 ) + 2,
          } );

        }
        if ( histories.Count > 0 ) {
          _context.LeaveHistorys.AddRange( histories );
        }
        _context.SystemRequests.RemoveRange( requests ); // sau khi thực hiện thành công request, xóa request đó đi
        _context.SaveChanges();
      }
    }
    private int CalMonths( DateTime start, DateTime end )
    {
      int months = ( ( end.Year - start.Year ) * 12 ) + end.Month - start.Month;
      if ( end.Day < start.Day ) {
        months--;
      }
      return months;
    }
    /// <summary>
    /// thu hồi ngày nghỉ hết hạn
    /// </summary>
    /// <param name="_context"></param>
    /// <returns></returns>
    public async Task TakeBackLeave( AppDbContext _context )
    {
      // check hệ thống có request yêu cầu không
      var dateCheck = DateTime.UtcNow.Date;
      var requests = await _context.SystemRequests.Where( o => o.Type == ( int ) Enums.SystemRequestType.TakeBackLeave && o.Date == dateCheck ).ToListAsync();
      if ( requests.Any() ) {
        // get record annual
        var dateCheckAnnual = DateTime.UtcNow.AddDays( 1 - DateTime.UtcNow.Day ).AddYears( -2 ).Date;
        var leaveAnnuals = await _context.Leaves.Where( l => ( l.Type == ( int ) Enums.LeaveType.Annual && l.Date == dateCheckAnnual ) ).ToListAsync();
        // TODO tạo history
        var histories = new List<LeaveHistory>();
        if ( leaveAnnuals.Any() ) {
          foreach ( var leave in leaveAnnuals ) {
            // add history
            var lastHistory = await _context.LeaveHistorys.Where( l => l.CreatedBy == leave.UserId ).OrderBy( l => l.Id ).LastOrDefaultAsync();
            histories.Add( new LeaveHistory()
            {
              CreatedBy = leave.UserId,
              Description = $"Take back {Enum.GetName( ( Enums.LeaveType ) leave.Type )} of month {leave.Date.Year}-{leave.Date.Month - 1}",
              Type = ( int ) Enums.LeaveHistoryType.TakeBackAnnualLeave,
              Unit = leave.Using - leave.Hours,
              AnnualLeave = ( lastHistory != null ? lastHistory.AnnualLeave : 0 ) + ( leave.Type == ( int ) Enums.LeaveType.Annual ? ( leave.Using - leave.Hours ) / 8 : 0 ),
              LeaveId = leave.Id
            } );
          }
          // add leave take back
          if ( leaveAnnuals.Count > 0 ) {
            _context.Leaves.RemoveRange( leaveAnnuals );
          }
        }
        if ( histories.Count > 0 ) {
          _context.LeaveHistorys.AddRange( histories );
        }
        _context.SystemRequests.RemoveRange( requests ); // sau khi thực hiện thành công request, xóa request đó đi
        _context.SystemRequests.RemoveRange( requests ); // sau khi thực hiện thành công request, xóa request đó đi
        _context.SaveChanges();
      }
      // create system request next time
      if ( DateTime.UtcNow.Day >= DateTime.UtcNow.AddDays( -DateTime.UtcNow.Date.Day - 7 ).AddMonths( 1 ).Day ) {
        var nextMonth = DateTime.UtcNow.Date.AddDays( 4 - DateTime.UtcNow.Day ).AddMonths( 1 );
        if ( !await _context.SystemRequests.AnyAsync( o => o.Type == ( int ) Enums.SystemRequestType.TakeBackLeave && o.Date == nextMonth ) ) {
          await _context.SystemRequests.AddAsync( new SystemRequest
          {
            UserId = 0,
            Type = ( int ) Enums.SystemRequestType.TakeBackLeave!,
            Date = nextMonth,
            ObjectId = 0,
          } );
          _context.SaveChanges();
        }
      }
    }

    /// <summary>
    /// apply/remove holiday vào timesheet
    /// </summary>
    /// <param name="_context"></param>
    /// <returns></returns>
    public async Task ApplyHoliday( AppDbContext _context )
    {
      // lấy danh sách holiday cần apply
      var holidayApply = await _context.Holidays.Include( h => h.HolidayApplys ).Where( h => h.Status != ( int ) Enums.HolidayStatus.Apply ).ToListAsync();
      foreach ( var holiday in holidayApply ) {
        if ( holiday.Type == ( int ) Enums.HolidayType.Holiday ) {
          var ts = await _context.Timesheets.Where( t => t.Date >= holiday.StartDate && t.Date <= holiday.EndDate ).ToListAsync();
          if ( ts.Count() > 0 ) {
            ts.ForEach( t => t.HolidayName = holiday.Status == ( int ) Enums.HolidayStatus.Pending ? holiday.Name : string.Empty );
            _context.Timesheets.UpdateRange( ts );
          }
        }
        else if ( holiday.Type == ( int ) Enums.HolidayType.Special ) {
          var userIds = new long [] { };
          foreach ( var apply in holiday.HolidayApplys ) {
            if ( apply.Type == Enum.GetName( Enums.DynamicOption.Team ) ) {
              var ids = await _context.TeamUsers.Include( t => t.User ).Where( t => t.TeamId == apply.ApplyId && t.User.IsActive )
                .Select( t => t.UserId ).ToArrayAsync();
              userIds = userIds.Union( ids ).ToArray();
            }
            //else if ( apply.Type == Enum.GetName( Enums.DynamicOption.Group ) ) {
            //  var ids = await _context.Groups.Include( g => g.Users ).Where( g => g.Id == apply.ApplyId ).ToArrayAsync();
            //  userIds = userIds.Union( ids.SelectMany( g => g.Users ).Where( u => u.IsActive == ( int ) Enums.Status.Active ).Select( u => u.Id ) ).ToArray();
            //}
            //else if ( apply.Type == Enum.GetName( Enums.DynamicOption.Position ) ) {
            //  var ids = await _context.Users.Where( u => u.Id != Enums.SYSTEM_ID && u.IsActive == ( int ) Enums.Status.Active && u.Position == apply.ApplyId )
            //    .Select( u => u.Id ).ToListAsync();
            //  userIds = userIds.Union( ids ).ToArray();
            //}
            else if ( apply.Type == Enum.GetName( Enums.DynamicOption.User ) && !userIds.Contains( apply.ApplyId ) ) {
              userIds.Append( apply.ApplyId );
            }
          }
          var ts = await _context.Timesheets.Where( t => t.Date >= holiday.StartDate && t.Date <= holiday.EndDate && userIds.Contains( t.UserId ) ).ToListAsync();
          if ( ts.Count() > 0 ) {
            ts.ForEach( t => t.HolidayName = t.HolidayName = holiday.Status == ( int ) Enums.HolidayStatus.Pending ? holiday.Name : string.Empty );
            _context.Timesheets.UpdateRange( ts );
          }
        }
        holiday.Status = holiday.Status == ( int ) Enums.HolidayStatus.Pending ? ( int ) Enums.HolidayStatus.Apply : ( int ) Enums.HolidayStatus.Deleting;
      }
      if ( holidayApply.Count() > 0 ) {
        _context.Holidays.UpdateRange( holidayApply.Where( h => h.Status == ( int ) Enums.HolidayStatus.Apply ) );
        _context.Holidays.RemoveRange( holidayApply.Where( h => h.Status != ( int ) Enums.HolidayStatus.Apply ) );
      }
      _context.SaveChanges();
    }
    /// <summary>
    /// xóa các record notification quá cũ (trên 1 tháng)
    /// </summary>
    /// <param name="_context"></param>
    public async Task CleanNotification( AppDbContext _context )
    {
      var notifications = await _context.Notifications.Include( n => n.NotificationAssigns )
        .Where( n => n.CreatedDate.Date < DateTime.UtcNow.Date.AddDays( -30 ) ).ToArrayAsync();
      if ( notifications.Any() ) {
        _context.Notifications.RemoveRange( notifications );
        _context.SaveChanges();
      }
    }
  }

  public class ScopedServiceHostedService : BackgroundService
  {
    private readonly ILogger<ScopedServiceHostedService> _logger;

    public ScopedServiceHostedService( IServiceProvider services,
        ILogger<ScopedServiceHostedService> logger,
        IServiceScopeFactory factory )
    {
      Services = services;
      _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync( CancellationToken stoppingToken )
    {
      _logger.LogInformation( "Scoped Service Hosted Service running." );
      var task1 = DoWork( stoppingToken );
      var task2 = UpdateFingure( stoppingToken );
      var task3 = UpdateFingureDateOut( stoppingToken );
      await Task.WhenAll( task1, task2, task3 );
    }

    private async Task DoWork( CancellationToken stoppingToken )
    {
      _logger.LogInformation( "Scoped Service Hosted Service is working." );

      using ( var scope = Services.CreateScope() ) {
        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
        AppDbContext _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await scopedProcessingService.DoWork( stoppingToken, _context );
      }
    }
    private async Task UpdateFingure( CancellationToken stoppingToken )
    {
      _logger.LogInformation( "Scoped Service Hosted Service is working." );

      using ( var scope = Services.CreateScope() ) {
        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
        AppDbContext _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await scopedProcessingService.MatchingFingerMachine( stoppingToken, _context );
      }
    }
    private async Task UpdateFingureDateOut( CancellationToken stoppingToken )
    {
      _logger.LogInformation( "Scoped Service Hosted Service is working. UpdateFingureDateOut" );

      using ( var scope = Services.CreateScope() ) {
        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
        AppDbContext _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await scopedProcessingService.MatchingFingerMachineDateOut( stoppingToken, _context );
      }
    }

    public override async Task StopAsync( CancellationToken stoppingToken )
    {
      _logger.LogInformation( "Consume Scoped Service Hosted Service is stopping." );
      await base.StopAsync( stoppingToken );
    }
  }
}
