using Microsoft.AspNetCore.Mvc;
using TTDesign.API.Domain.Models;
using TTDesign.API.Persistence.Contexts;
namespace TTDesign.API.Controllers
{
  [ApiController]
  [Route( "iclock" )]
  public class IclockController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<IclockController> _logger;
    private const string SNC = "2145254600780";
    public IclockController( AppDbContext context, IWebHostEnvironment env, ILogger<IclockController> logger )
    {
      _context = context;
      _env = env;
      _logger = logger;
    }

    [HttpGet]
    [Route( "cdata" )]
    public async Task<IActionResult> getCdata()
    {
      return Ok( "Ok\n" );
    }

    [HttpPost]
    [Route( "cdata" )]
    public async Task<IActionResult> cdata()
    {
      var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
      var time = DateTime.UtcNow.ToString( "yyyy-MM-dd HH:mm:ss" );

      var queryString = string.Join( "&",
        Request.Query.Select( q => $"{q.Key}={q.Value}" ) );

      using var reader = new StreamReader( Request.Body );
      var raw = await reader.ReadToEndAsync();
      _logger.LogError( $"=============== cdata: {ip} - {time} - {queryString} ===============" );
      _logger.LogError( $"=============== Received data: {raw}" );
      var deviceId = Request.Query [ "SN" ].ToString();
      var table = Request.Query [ "table" ].ToString();
      var stamp = Request.Query [ "stamp" ].ToString();

      var lines = raw.Split( '\n', StringSplitOptions.RemoveEmptyEntries );
      var list = new List<FingerDataMachine>();
      foreach ( var line in lines ) {
        var parts = line.Split( '\t', StringSplitOptions.RemoveEmptyEntries );
        if ( parts.Length >= 5 && int.TryParse( parts [ 0 ], out int userId ) ) {
          var empId = userId;
          var punchDate = parts [ 1 ];
          var status1 = parts [ 2 ];
          var verifyType = parts [ 3 ];
          var status2 = parts [ 4 ];
          var status3 = parts [ 5 ];
          var status4 = parts [ 6 ];
          var status5 = parts [ 7 ];
          var status6 = parts [ 8 ];
          var status7 = parts [ 9 ];
          var fingerDataMachine = new FingerDataMachine
          {
            SN = deviceId,
            Table = table,
            EmpId = empId,
            PunchDate = deviceId == SNC ? DateTime.Parse( punchDate ).AddHours( -1 ) : DateTime.Parse( punchDate ),
            Status1 = int.Parse( status1 ),
            VerifyType = int.Parse( verifyType ),
            Status2 = ValidateStatus( status2 ),
            Status3 = ValidateStatus( status3 ),
            Status4 = ValidateStatus( status4 ),
            Status5 = ValidateStatus( status5 ),
            Status6 = ValidateStatus( status6 ),
            Status7 = ValidateStatus( status7 )
          };
          var existingRecord = _context.FingerDataMachines
                              .Where( a => a.EmpId == fingerDataMachine.Id )
                              .Where( a => a.Status1 == fingerDataMachine.Status1 )
                              .Where( a => a.SN == fingerDataMachine.SN )
                              .AsEnumerable()
                              .Where( a => Math.Abs( ( a.PunchDate - fingerDataMachine.PunchDate ).TotalSeconds ) <= 5 )
                              .FirstOrDefault();

          if ( existingRecord == null )
            list.Add( fingerDataMachine );
        }
      }
      if ( list.Count() > 0 ) {
        await _context.AddRangeAsync( list );
        await _context.SaveChangesAsync();
      }
      return Ok( "Ok\n" );
    }

    [HttpGet]
    [Route( "test" )]
    public async Task<IActionResult> test()
    {
      var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
      var time = DateTime.UtcNow.ToString( "yyyy-MM-dd HH:mm:ss" );

      var queryString = string.Join( "&",
          Request.Query.Select( q => $"{q.Key}={q.Value}" ) );

      var url = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

      _logger.LogError( "getrequest called at {Time} | IP: {IP} | URL: {Url} | Query: {Query}",
          time, ip, url, queryString );
      return Ok( "Ok\n" );
    }

    [HttpGet]
    [Route( "getrequest" )]
    public IActionResult getrequest()
    {
      var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
      var time = DateTime.UtcNow.ToString( "yyyy-MM-dd HH:mm:ss" );

      var queryString = string.Join( "&",
          Request.Query.Select( q => $"{q.Key}={q.Value}" ) );

      var url = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
      _logger.LogError( "getrequest called at {Time} | IP: {IP} | URL: {Url} | Query: {Query}",
          time, ip, url, queryString );

      return Ok( "Ok\n" );
    }
    private int ValidateStatus( string status )
    {
      if ( string.IsNullOrEmpty( status ) )
        return -1;
      else {
        if ( int.TryParse( status, out int result ) )
          return result;
        else
          return -1;
      }
    }
  }
}
