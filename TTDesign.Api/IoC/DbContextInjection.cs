using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using TTDesign.API.Persistence.Contexts;

namespace TTDesign.API.IoC;

public static class DbContextInjection
{
  public static IServiceCollection AddVciDbContext(this IServiceCollection service, ConfigurationManager configuration, Logger logger )
  {
    var connectionString = configuration.GetConnectionString( "VCI_DB" );
    var tag = Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" ) ?? string.Empty;

    if ( tag.Equals( Environments.Staging ) || tag.Equals( Environments.Production ) ) {
      connectionString = Environment.GetEnvironmentVariable( "DB_CONNECTION_STRING" );
      Console.WriteLine( $"==============> Load Environment Variable for: {tag}" );
    }
    
    service.AddDbContext<AppDbContext>(
      options =>
      {
        options.UseNpgsql( connectionString );
      }
    );

    return service;
  }
}