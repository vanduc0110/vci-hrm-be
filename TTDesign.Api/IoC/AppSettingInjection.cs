namespace TTDesign.API.IoC;

public static class AppSettingInjection
{
  public static ConfigurationManager AddAppSettings(this ConfigurationManager configuration)
  {
    configuration.AddJsonFile(
      path: $"appsettings.json",
      optional: true,
      reloadOnChange: true
    );

    configuration.AddJsonFile(
      path: $"appsettings.Common.json",
      optional: true,
      reloadOnChange: true
    );

    configuration.AddJsonFile(
      path: $"appsettings.{Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" )}.json",
      optional: true,
      reloadOnChange: true
    );

    return configuration;
  }
}