using FluentValidation.AspNetCore;
using Infrastructure.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using TTDesign.API.Domain.Models;
using TTDesign.API.Domain.Security.Hashing;
using TTDesign.API.Domain.Security.Tokens;
using TTDesign.API.Extensions;
using TTDesign.API.Hubs;
using TTDesign.API.Hubs.Intefaces;
using TTDesign.API.IoC;
using TTDesign.API.Persistence.Contexts;
using TTDesign.API.Security.Hashing;
using TTDesign.API.Security.Tokens;
using TTDesign.API.Services;


var builder = WebApplication.CreateBuilder( args );

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration( builder.Configuration )
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog( logger );

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors( options =>
{
  //accept request header with 
  options.AddPolicy( name: MyAllowSpecificOrigins,
                    builder =>
                    {
                      builder
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .SetIsOriginAllowed( origin => true )
                             .AllowCredentials();
                    } );
} );
// Add services to the container.
builder.Services.AddControllers()
  .AddNewtonsoftJson( options =>
  {
    options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
  } )
  .AddFluentValidation( fv => fv.RegisterValidatorsFromAssemblyContaining<Program>() )
  .AddDataAnnotationsLocalization();

builder.Configuration.AddAppSettings();
builder.Services.AddVciDbContext( builder.Configuration, logger );

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>( options =>
{
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequiredLength = 3;
  options.Password.RequiredUniqueChars = 1;

  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes( 5 );
  options.Lockout.MaxFailedAccessAttempts = 5;
  options.Lockout.AllowedForNewUsers = true;

  options.User.AllowedUserNameCharacters =
      "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
  options.User.RequireUniqueEmail = true;

  options.SignIn.RequireConfirmedEmail = false;
  options.SignIn.RequireConfirmedPhoneNumber = false;

} );

builder.Services.Configure<FormOptions>( option =>
{
  option.ValueLengthLimit = int.MaxValue;
  option.MultipartBodyLengthLimit = int.MaxValue;
  option.MemoryBufferThreshold = int.MaxValue;
} );

builder.Services.Configure<MailSettings>( option =>
{
  option.DisplayName = "System Manager";
  option.Mail = builder.Configuration.GetSection( "Mail:Email" ).Value;
  option.Password = builder.Configuration.GetSection( "Mail:Password" ).Value;
  option.SystemAdminMail = builder.Configuration.GetSection( "Mail:SystemAdmin" ).Value;
  option.Host = "smtp.gmail.com";
  option.Port = 587;
} );

//builder.Services.AddSingleton( typeof( GoogleSheetService ) );
//builder.Services.AddSingleton( typeof( GoogleChatService ) );

//builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.Configure<TokenOptionsConfig>( builder.Configuration.GetSection( "TokenOptions" ) );
var tokenOptions = builder.Configuration.GetSection( "TokenOptions" ).Get<TokenOptionsConfig>();

var signingConfigurations = new SigningConfigurations( tokenOptions.Secret );
builder.Services.AddSingleton( signingConfigurations );

//builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
builder.Services.AddAuthentication( options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
} )
  .AddJwtBearer( jwtBearerOptions =>
  {
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
    {
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = tokenOptions.Issuer,
      ValidAudience = tokenOptions.Audience,
      IssuerSigningKey = signingConfigurations.SecurityKey,
      ClockSkew = TimeSpan.Zero
    };

    jwtBearerOptions.Events = new JwtBearerEvents
    {
      OnMessageReceived = context =>
      {
        var accessToken = context.Request.Query [ "access_token" ];

        // If the request is for our hub...
        //comment
        var path = context.HttpContext.Request.Path;
        if ( !string.IsNullOrEmpty( accessToken ) &&
            ( path.StartsWithSegments( "/NotificationUserHubs" ) ) ) {
          // Read the token out of the query string
          context.Token = accessToken;
        }
        return Task.CompletedTask;
      }
    };
  } );

builder.Services.AddScoped<ITokenHandler, TTDesign.API.Security.Tokens.TokenHandler>();

builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureAuthorizationPolicy();
builder.Services.ConfigureLocalization();
// Configure the API versioning properties of the project.
//builder.Services.AddApiVersioningConfigured();
// Add a Swagger generator and Automatic Request and Response annotations:
builder.Services.AddSwaggerSwashbuckleConfigured();
//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<ScopedServiceHostedService>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

//builder.Services.AddMvc( option => option.EnableEndpointRouting = false )
//  .AddNewtonsoftJson( opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore );

builder.Services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies() );
builder.Services.Configure<EmailSenderOptions>( builder.Configuration.GetSection( nameof( EmailSenderOptions ) ) );
var emailSenderOptions = new EmailSenderOptions();
builder.Configuration.GetSection( nameof( EmailSenderOptions ) ).Bind( emailSenderOptions );
builder.Services.AddSingleton( emailSenderOptions );

var app = builder.Build();

// if (Environment.GetEnvironmentVariable("RUN_MIGRATIONS") == "true")
// {
//   using (var scope = app.Services.CreateScope())
//   {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     await context.Database.MigrateAsync();
//   }
// }



using ( var scope = app.Services.CreateScope() ) {
  var services = scope.ServiceProvider;
  try {
    var dbContext = services.GetService<AppDbContext>();
    if ( dbContext == null )
      throw new ArgumentException();

    if ( Environment.GetEnvironmentVariable( "RUN_MIGRATIONS" ) == "true" ) {
      await dbContext.Database.MigrateAsync();
      Console.WriteLine( "=============> Database Auto Migration Successful!" );
    }

    var roleManager = services.GetService<RoleManager<Role>>();
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleSystemAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleDirectorAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleLeaderAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleSubleadAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRolePMAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleOfficalAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleProbationaryAsync( roleManager! );
    await TTDesign.API.Seeds.DefaultRoles.SeedDefaultRoleInternshipAsync( roleManager! );

    var userManager = services.GetService<UserManager<User>>();
    await TTDesign.API.Seeds.DefaultUsers.SeedSuperAdminAsync( userManager! );
    await TTDesign.API.Seeds.DefaultProjectType.SeedDefaultProjectType( dbContext! );
    await TTDesign.API.Seeds.DefaultAssetCategory.SeedDefaultAssetCategory( dbContext! );

    logger.Information( "Finished Seeding Default Data" );
    logger.Information( "Application Starting" );
    Console.WriteLine( "=============> Check Seeds Successful!" );
  }
  catch ( Exception ex ) {
    logger.Warning( ex, "An error occurred seeding the DB" );
  }
}

//var supportedCultures = new [] { "en-US", "vi" };
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture( supportedCultures [ 0 ] )
//  .AddSupportedCultures( supportedCultures )
//  .AddSupportedUICultures( supportedCultures );
//localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
//app.UseRequestLocalization( localizationOptions );
app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() || app.Environment.IsStaging() ) {
  app.UseSwagger();
  app.UseSwaggerUI();
  // Enable middleware to serve Swagger-UI (HTML, JS, CSS, etc.) by specifying the Swagger JSON endpoint(s).
  //var descriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
  //app.UseSwaggerUI( options =>
  //{
  //  // Build a swagger endpoint for each discovered API version
  //  foreach ( var description in descriptionProvider.ApiVersionDescriptions ) {
  //    options.SwaggerEndpoint( $"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant() );
  //  }
  //} );
}

//request
app.UseCors( MyAllowSpecificOrigins );

//add cors cho response
app.Use( async ( context, next ) =>
{
  context.Response.Headers.Add( "Access-Control-Allow-Origin", "*" );
  context.Response.Headers.Add( "Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS" );
  context.Response.Headers.Add( "Access-Control-Allow-Headers", "Origin,DNT,X-Mx-ReqToken,Keep-Alive,User-Agent,X-Requested-With,If-Modified-Since,Cache-Control,Content-Type" );
  await next();
} );

DefaultFilesOptions defaultFiles = new();
defaultFiles.DefaultFileNames.Clear();
defaultFiles.DefaultFileNames.Add( "index.html" );
app.UseDefaultFiles( defaultFiles );

app.UseStaticFiles( new StaticFileOptions()
{
  FileProvider = new PhysicalFileProvider( Path.Combine( Environment.CurrentDirectory, @"Upload" ) ),
  RequestPath = new PathString( "/Upload" )
} );

app.UseMiddleware<ExceptionMiddleware>();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationUserHub>( "/NotificationUserHubs" );

app.MapControllers();

app.Lifetime.ApplicationStarted.Register( () =>
{
  foreach ( var address in app.Urls ) {
    Console.WriteLine( $"[Started] Listening on: {address}" );
  }
} );

app.Run();

public partial class Program
{ }
