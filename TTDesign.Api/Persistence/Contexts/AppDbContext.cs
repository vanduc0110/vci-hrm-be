using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TTDesign.API.Domain.Models;

namespace TTDesign.API.Persistence.Contexts
{
  public partial class AppDbContext : IdentityDbContext<User, Role, long>
  {
    public AppDbContext()
    {
    }
    public AppDbContext( DbContextOptions<AppDbContext> options )
        : base( options )
    {
    }
    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Leave> Leaves { get; set; } = null!;
    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; } = null!;
    public virtual DbSet<LeaveRequestDetail> LeaveRequestDetails { get; set; } = null!;
    public virtual DbSet<LeaveHistory> LeaveHistorys { get; set; } = null!;
    public virtual DbSet<LeaveHistoryUsing> LeaveHistoryUsings { get; set; } = null!;
    public virtual DbSet<Project> Projects { get; set; } = null!;
    public virtual DbSet<Holiday> Holidays { get; set; } = null!;
    public virtual DbSet<Team> Teams { get; set; } = null!;
    public virtual DbSet<TeamUser> TeamUsers { get; set; } = null!;
    public virtual DbSet<Notification> Notifications { get; set; } = null!;
    public virtual DbSet<NotificationAssign> NotificationAssigns { get; set; } = null!;
    public virtual DbSet<Timesheet> Timesheets { get; set; } = null!;
    public virtual DbSet<FingerPrinter> FingerPrinters { get; set; } = null!;
    public virtual DbSet<FingerPrinterLog> FingerPrinterLogs { get; set; } = null!;
    public virtual DbSet<TeamCategory> TeamCategories { get; set; } = null!;
    public virtual DbSet<TimesheetDetail> TimesheetDetails { get; set; } = null!;
    public virtual DbSet<UserInfo> UserInfos { get; set; } = null!;
    public virtual DbSet<UserSetting> UserSettings { get; set; } = null!;
    public virtual DbSet<LeaveInformation> LeaveInformations { get; set; } = null!;
    public virtual DbSet<WfhRequest> WfhRequests { get; set; } = null!;
    public virtual DbSet<HolidayApply> HolidayApplys { get; set; } = null!;
    public virtual DbSet<TimesheetReport> TimesheetReports { get; set; } = null!;
    public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; } = null!;
    public virtual DbSet<SystemRequest> SystemRequests { get; set; } = null!;
    public virtual DbSet<SwapDay> SwapDays { get; set; } = null!;
    public virtual DbSet<SwapDayUser> SwapDayUsers { get; set; } = null!;
    public virtual DbSet<SwapDayRefer> SwapDayRefers { get; set; } = null!;
    public virtual DbSet<Config> Configs { get; set; } = null!;
    public virtual DbSet<ProjectContract> ProjectContracts { get; set; } = null!;
    public virtual DbSet<UserTask> UserTasks { get; set; } = null!;
    public virtual DbSet<UserReport> UserReports { get; set; } = null!;
    public virtual DbSet<FingerDataMachine> FingerDataMachines { get; set; } = null!;
    public virtual DbSet<Asset> Assets { get; set; } = null!;
    public virtual DbSet<AssetCategory> AssetCategories { get; set; }
    public virtual DbSet<Component> Components { get; set; }
    public virtual DbSet<AssetComponentHistory> AssetComponents { get; set; }
    public virtual DbSet<AssetHistory> AssetHistories { get; set; }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
      if ( !optionsBuilder.IsConfigured ) {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        // optionsBuilder.UseMySql( "server=dev.ttdesignco.com;port=3306;database=QuangDemo;uid=rdteam;pwd=rdTeam2020aDmin@!", ServerVersion.Parse( "8.0.33-mysql" ) );
      }
      optionsBuilder.UseLoggerFactory( GetLoggerFactory() );
    }

    private ILoggerFactory GetLoggerFactory()
    {
      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddLogging( builder =>
              builder.AddConsole()
                     .AddFilter( DbLoggerCategory.Database.Command.Name,
                              LogLevel.Information ) );
      return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>()!;
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      base.OnModelCreating( modelBuilder );
      // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
      // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
      // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
      foreach ( var entityType in modelBuilder.Model.GetEntityTypes() ) {
        var tableName = entityType.GetTableName();
        if ( tableName!.StartsWith( "AspNet" ) ) {
          entityType.SetTableName( tableName.Substring( 6 ) );
        }
      }
      // thêm để sửa lỗi datetime
      var dateTimeConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
        v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
        v => DateTime.SpecifyKind( v, DateTimeKind.Utc ) );

      var nullableDateTimeConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime?, DateTime?>(
          v => !v.HasValue ? v : ( v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime() ),
          v => v.HasValue ? DateTime.SpecifyKind( v.Value, DateTimeKind.Utc ) : v );

      foreach ( var entityType in modelBuilder.Model.GetEntityTypes() ) {
        foreach ( var property in entityType.GetProperties() ) {
          if ( property.ClrType == typeof( DateTime ) ) {
            property.SetValueConverter( dateTimeConverter );
          }
          else if ( property.ClrType == typeof( DateTime? ) ) {
            property.SetValueConverter( nullableDateTimeConverter );
          }
        }
      }
      //

      modelBuilder.Entity<FingerDataMachine>( entity =>
      {
        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" ).HasDefaultValue( 1 );
        entity.Property( e => e.CreatedDate )
        .HasColumnType( "timestamp with time zone" )
        .HasColumnName( "CreatedDate" )
        .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
      } );

      modelBuilder.Entity<FingerPrinterLog>( entity =>
      {
        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" ).HasDefaultValue( 1 );
        entity.Property( e => e.CreatedDate )
        .HasColumnType( "timestamp with time zone" )
        .HasColumnName( "CreatedDate" )
        .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
      } );

      modelBuilder.Entity<SystemRequest>( entity =>
      {
        entity.ToTable( "SystemRequest" );

        entity.HasIndex( e => new { e.Type, e.Date }, "fk_system_request_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" )
            .HasComment( "0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 TakeBackLeave" );

        entity.Property( e => e.Date )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "Date" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.Status ).HasColumnName( "Status" );

        entity.Property( e => e.ObjectId ).HasColumnName( "ObjectId" );
      } );

      modelBuilder.Entity<Client>( entity =>
      {
        entity.ToTable( "Client" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.Code )
            .HasMaxLength( 30 )
            .HasColumnName( "Code" );

        entity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;
      } );
      modelBuilder.Entity<UserTask>( entity =>
     {
       entity.ToTable( "UserTask" );

       entity.Property( e => e.Id ).HasColumnName( "Id" );

       entity.Property( e => e.Name )
           .HasMaxLength( 100 )
           .HasColumnName( "Name" );
       entity.Property( e => e.Description )
           .HasColumnName( "Description" );
       entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

       entity.Property( e => e.CreatedDate )
           .HasColumnType( "timestamp with time zone" )
           .HasColumnName( "CreatedDate" )
           .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

       entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

       entity.Property( e => e.ModifiedDate )
           .HasColumnType( "timestamp with time zone" )
           .ValueGeneratedOnAddOrUpdate()
           .HasColumnName( "ModifiedDate" )
           ;
     } );
      modelBuilder.Entity<UserReport>( entity =>
    {
      entity.ToTable( "UserReports" );

      entity.Property( e => e.Id ).HasColumnName( "Id" );

      entity.Property( e => e.CreatedDate )
          .HasColumnType( "timestamp with time zone" )
          .HasColumnName( "CreatedDate" )
          .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

      entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

      entity.Property( e => e.ModifiedDate )
          .HasColumnType( "timestamp with time zone" )
          .ValueGeneratedOnAddOrUpdate()
          .HasColumnName( "ModifiedDate" )
          ;
    } );
      modelBuilder.Entity<ProjectDocument>( entity =>
      {
        entity.ToTable( "ProjectDocument" );

        entity.HasIndex( e => e.ContractId, "fk_projectcontract_document_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );

        entity.Property( e => e.Comment )
            .HasMaxLength( 500 )
            .HasColumnName( "Comment" );

        entity.Property( e => e.Link )
            .HasMaxLength( 200 )
            .HasColumnName( "Link" );

        entity.Property( e => e.ContractId ).HasColumnName( "ContractId" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.HasOne( d => d.ProjectContract )
            .WithMany( p => p.ProjectDocuments )
            .HasForeignKey( d => d.ContractId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_projectcontract_projectdocument" );
      } );
      modelBuilder.Entity<ProjectContract>( entity =>
    {
      entity.ToTable( "ProjectContract" );

      entity.HasIndex( e => e.ProjectId, "fk_project_contract_idx" );

      entity.Property( e => e.Id ).HasColumnName( "Id" );
      entity.Property( e => e.Name )
          .HasMaxLength( 100 )
          .HasColumnName( "Name" );
      entity.Property( e => e.Code )
          .HasMaxLength( 100 )
          .HasColumnName( "Code" );
      entity.Property( e => e.ProjectId ).HasColumnName( "ProjectId" );
      entity.Property( e => e.Date ).HasColumnName( "Date" );
      entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

      entity.Property( e => e.CreatedDate )
          .HasColumnType( "timestamp with time zone" )
          .HasColumnName( "CreatedDate" )
          .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
      entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

      entity.Property( e => e.ModifiedDate )
          .HasColumnType( "timestamp with time zone" )
          .HasColumnName( "ModifiedDate" )
          ;

      entity.HasOne( d => d.Project )
          .WithMany( p => p.ProjectContracts )
          .HasForeignKey( d => d.ProjectId )
          .OnDelete( DeleteBehavior.ClientSetNull )
          .HasConstraintName( "fk_project_project_contract" );
    } );

      modelBuilder.Entity<TimesheetReport>( entity =>
      {
        entity.ToTable( "TimesheetReport" );

        entity.HasIndex( e => new { e.TimesheetId, e.ProjectId }, "fk_report_timesheet_id_idx" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.TimesheetId ).HasColumnName( "TimesheetId" );

        entity.Property( e => e.Hours ).HasColumnName( "Hours" );

        entity.Property( e => e.ProjectId ).HasColumnName( "ProjectId" );

        entity.HasOne( d => d.Timesheet )
            .WithMany( p => p.TimesheetReports )
            .HasForeignKey( d => d.TimesheetId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_timesheet_report" );
      } );

      modelBuilder.Entity<GoogleTimesheetKey>( entity =>
      {
        entity.ToTable( "GoogleTimesheetKey" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.ClientEmail )
            .HasMaxLength( 128 )
            .HasColumnName( "ClientEmail" );

        entity.Property( e => e.KeyName )
            .HasMaxLength( 128 )
            .HasColumnName( "KeyName" );

        entity.Property( e => e.PrivateKey )
            .HasMaxLength( 2000 )
            .HasColumnName( "PrivateKey" );

        entity.Property( e => e.SheetId )
            .HasMaxLength( 128 )
            .HasColumnName( "SheetId" );

        entity.Property( e => e.SpreadsheetId )
            .HasMaxLength( 128 )
            .HasColumnName( "SpreadsheetId" );
      } );

      modelBuilder.Entity<Holiday>( entity =>
      {
        entity.ToTable( "Holiday" );

        entity.HasIndex( e => new { e.Type, e.StartDate, e.EndDate }, "fk_holiday_type_date_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.StartDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "StartDate" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;

        entity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );

        entity.Property( e => e.Status )
            .HasColumnName( "Status" )
            .HasComment( "0 pending, 1 Apply, 2 Deleting" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" )
            .HasDefaultValue( 0 )
            .HasComment( "0 holiday, 1 special" );

        entity.Property( e => e.EndDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "EndDate" );
      } );

      modelBuilder.Entity<Leave>( entity =>
      {
        entity.ToTable( "Leave" );

        entity.HasIndex( e => e.UserId, "fk_leave_user_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.Using ).HasColumnName( "Using" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;

        entity.Property( e => e.Date )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "Date" );

        entity.Property( e => e.Hours ).HasColumnName( "Hours" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" )
            .HasComment( "6 Annual" );

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );
      } );

      modelBuilder.Entity<LeaveRequest>( entity =>
      {
        entity.ToTable( "LeaveRequest" );

        entity.HasIndex( e => e.CreatedBy, "fk_leave_request_created_by_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.EndDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "EndDate" );

        entity.Property( e => e.StartDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "StartDate" );

        entity.Property( e => e.Hour ).HasColumnName( "Hour" );

        entity.Property( e => e.Status ).HasColumnName( "Status" ).HasComment( "0 Pending, 1 Approve, 2 Reject" );

        entity.Property( e => e.Reviewer ).HasColumnName( "Reviewer" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" )
            .HasComment( "0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Annual, 7 Unpaid" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;

        entity.Property( e => e.Reason )
            .HasMaxLength( 500 )
            .HasColumnName( "Reason" );
      } );

      modelBuilder.Entity<LeaveHistoryUsing>( entity =>
      {
        entity.ToTable( "LeaveHistoryUsing" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.HasIndex( e => new { e.LeaveId, e.LeaveRequestId }, "fk_leave_history_using_idx" )
            .IsUnique();

        entity.HasIndex( e => e.LeaveId, "fk_leave_history_using_leave" );

        entity.HasIndex( e => e.LeaveRequestId, "fk_leave_history_using_leave_request" );

        entity.Property( e => e.LeaveId ).HasColumnName( "LeaveId" );

        entity.Property( e => e.LeaveRequestId ).HasColumnName( "LeaveRequestId" );

        entity.Property( e => e.Hours ).HasColumnName( "Hours" );

        entity.HasOne( d => d.LeaveRequest )
            .WithMany( p => p.LeaveHistoryUsings )
            .HasForeignKey( d => d.LeaveRequestId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_leave_history_using_leave_request" );
      } );

      modelBuilder.Entity<LeaveRequestDetail>( entity =>
      {
        entity.ToTable( "LeaveRequestDetail" );

        entity.HasIndex( e => e.LeaveRequestId, "fk_leave_history_using_leave_request" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.LeaveRequestId ).HasColumnName( "LeaveRequestId" );

        entity.Property( e => e.Date ).HasColumnName( "Date" );

        entity.Property( e => e.Hours ).HasColumnName( "Hours" );

        entity.HasOne( d => d.LeaveRequest )
            .WithMany( p => p.LeaveRequestDetails )
            .HasForeignKey( d => d.LeaveRequestId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_leave_request_detail" );
      } );

      modelBuilder.Entity<LeaveHistory>( entity =>
      {
        entity.ToTable( "LeaveHistory" );

        entity.HasIndex( e => e.CreatedBy, "fk_leave_history_user_idx" );

        entity.HasIndex( e => new { e.LeaveId, e.LeaveRequestId, e.Type }, "fk_leave_history_reference_type_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.LeaveId ).HasColumnName( "LeaveId" );

        entity.Property( e => e.LeaveRequestId ).HasColumnName( "LeaveRequestId" );

        entity.Property( e => e.AnnualLeave ).HasColumnName( "AnnualLeave" );

        entity.Property( e => e.Unit ).HasColumnName( "Unit" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" )
            .HasComment( "0 AddAnnualLeave, 1 TakeBackAnnualLeave, 2 UsingAnnualLeave" );

        entity.Property( e => e.Description )
            .HasMaxLength( 200 )
            .HasColumnName( "Description" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
                .HasColumnType( "timestamp with time zone" )
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnName( "ModifiedDate" )
                ;
      } );

      modelBuilder.Entity<LeaveInformation>( entity =>
      {
        entity.ToTable( "LeaveInformation" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.Type ).HasColumnName( "Type" );

        entity.Property( e => e.TypeName ).HasColumnName( "TypeName" );

        entity.Property( e => e.Detail ).HasColumnName( "Detail" );

        entity.Property( e => e.LeaveDay ).HasColumnName( "LeaveDay" );

        entity.Property( e => e.StartCondition ).HasColumnName( "StartCondition" );

        entity.Property( e => e.End ).HasColumnName( "End" );

        entity.Property( e => e.Block ).HasColumnName( "Block" );

        entity.Property( e => e.Using ).HasColumnName( "Using" );

        entity.Property( e => e.Note ).HasColumnName( "Note" );
      } );
      modelBuilder.Entity<WfhRequest>( entity =>
      {
        entity.ToTable( "WfhRequest" );

        entity.HasIndex( e => e.CreatedBy, "fk_leave_request_created_by_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.StartTime )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "StartTime" );

        entity.Property( e => e.EndTime )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "EndTime" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;

        entity.Property( e => e.Reason )
            .HasMaxLength( 500 )
            .HasColumnName( "Reason" );

        entity.Property( e => e.Reviewer ).HasColumnName( "Reviewer" );

        entity.Property( e => e.Status )
            .HasColumnName( "Status" )
            .HasDefaultValue( 0 ).HasComment( "0 Pending, 1 Approve, 2 Reject" );
      } );

      modelBuilder.Entity<Project>( entity =>
          {
            entity.ToTable( "Project" );

            entity.HasIndex( e => e.ClientId, "fk_projects_client" );

            entity.HasIndex( e => e.ProjectManagement, "fk_projects_leader_idx" );

            entity.HasIndex( e => e.TeamId, "fk_projects_teams_idx" );

            entity.Property( e => e.Id ).HasColumnName( "Id" );

            entity.Property( e => e.ClientId ).HasColumnName( "ClientId" );

            entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

            entity.Property( e => e.CreatedDate )
                    .HasColumnType( "timestamp with time zone" )
                    .HasColumnName( "CreatedDate" )
                    .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

            entity.Property( e => e.FinishedDate )
                    .HasColumnType( "timestamp with time zone" )
                    .HasColumnName( "FinishedDate" );

            entity.Property( e => e.FiscalYear ).HasColumnName( "FiscalYear" );

            entity.Property( e => e.WorkingHour ).HasColumnName( "WorkingHour" );
            entity.Property( e => e.IsPublic )
                    .HasColumnName( "IsPublic" );

            entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

            entity.Property( e => e.ModifiedDate )
                    .HasColumnType( "timestamp with time zone" )
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName( "ModifiedDate" )
                    ;

            entity.Property( e => e.Code )
                    .HasMaxLength( 30 )
                    .HasColumnName( "Code" );

            entity.Property( e => e.Name )
                    .HasMaxLength( 200 )
                    .HasColumnName( "Name" );

            entity.Property( e => e.ProjectManagement )
                    .HasColumnName( "ProjectManagement" )
                    .HasComment( "PM" );

            entity.Property( e => e.ProjectNumber ).HasColumnName( "ProjectNumber" );

            entity.Property( e => e.QuotationHour ).HasColumnName( "QuotationHour" );

            entity.Property( e => e.Type )
                    .HasMaxLength( 20 )
                    .HasColumnName( "Type" ).HasComment( "T, I, H" );

            entity.Property( e => e.StartedDate )
                    .HasColumnType( "timestamp with time zone" )
                    .HasColumnName( "StartedDate" );

            entity.Property( e => e.Status )
                    .HasColumnName( "Status" )
                    .HasComment( "0 Pending, 1 Active, 2 End" );

            entity.Property( e => e.TeamId ).HasColumnName( "TeamId" );

            entity.HasOne( d => d.Client )
                    .WithMany( p => p.Projects )
                    .HasForeignKey( d => d.ClientId )
                    .HasConstraintName( "fk_projects_client" );
          } );

      modelBuilder.Entity<HolidayApply>( entity =>
      {
        entity.HasKey( e => new { e.HolidayId, e.ApplyId } )
            .HasName( "PRIMARY" )
            .HasAnnotation( "MySql:IndexPrefixLength", new [] { 0, 0 } );

        entity.ToTable( "HolidayApply" );

        entity.HasIndex( e => new { e.HolidayId }, "fk_holiday_idx" );

        entity.Property( e => e.HolidayId ).HasColumnName( "HolidayId" );

        entity.Property( e => e.ApplyId ).HasColumnName( "ApplyId" );

        entity.Property( e => e.Type ).HasColumnName( "Type" ).HasComment( "User, Team" );

        entity.HasOne( d => d.Holiday )
            .WithMany( p => p.HolidayApplys )
            .HasForeignKey( d => d.HolidayId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_holiday_user" );
      } );

      modelBuilder.Entity<Team>( entity =>
      {
        entity.ToTable( "Team" );

        entity.HasIndex( e => e.Code, "team_code_UNIQUE" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;

        entity.Property( e => e.Code )
            .HasMaxLength( 30 )
            .HasColumnName( "Code" );

        entity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );

        entity.Property( e => e.Amount ).HasColumnName( "Amount" ).HasDefaultValue( 0 );
      } );

      modelBuilder.Entity<TeamUser>( entity =>
      {
        entity.HasKey( e => new { e.UserId, e.TeamId } )
            .HasName( "PRIMARY" )
            .HasAnnotation( "MySql:IndexPrefixLength", new [] { 0, 0 } );

        entity.ToTable( "TeamUser" );

        entity.HasIndex( e => new { e.UserId, e.TeamId }, "fk_team_users_idx" )
            .IsUnique();

        entity.HasIndex( e => e.TeamId, "fk_team_users_team" );

        entity.HasIndex( e => e.UserId, "UserId" );

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );

        entity.Property( e => e.TeamId ).HasColumnName( "TeamId" );

        entity.HasOne( d => d.Team )
            .WithMany( p => p.TeamUsers )
            .HasForeignKey( d => d.TeamId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_team_users_team" );

        entity.HasOne( d => d.User )
            .WithMany( p => p.TeamUsers )
            .HasForeignKey( d => d.UserId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_team_users_user" );
      } );

      modelBuilder.Entity<Notification>( entity =>
      {
        entity.ToTable( "Notification" );

        entity.HasIndex( e => new { e.CreatedDate }, "fk_notification_date_idx" );

        entity.HasIndex( e => new { e.CreatedDate, e.ObjectType, e.ObjectId }, "fk_notification_object_idx" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );


        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.Title ).HasMaxLength( 50 ).HasColumnName( "Title" );

        entity.Property( e => e.Content ).HasMaxLength( 200 ).HasColumnName( "Content" );

        entity.Property( e => e.Type ).HasColumnName( "Type" ).HasComment( "0 Information, 1 Approve, 2 Reject" );

        entity.Property( e => e.ObjectType ).HasColumnName( "ObjectType" ).HasComment( "0 Notification, 1 LeaveRequest, 2 WfhRequest, 3 NoticeInvite, 4 AssetInvite" );

        entity.Property( e => e.ObjectId ).HasColumnName( "ObjectId" );

        entity.Property( e => e.UserName ).HasColumnName( "UserName" ).HasMaxLength( 20 );
      } );

      modelBuilder.Entity<NotificationAssign>( entity =>
      {
        entity.ToTable( "NotificationAssign" );

        entity.HasIndex( e => new { e.UserId, e.NotificationId }, "fk_notification_assign_user_idx" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );

        entity.Property( e => e.NotificationId ).HasColumnName( "NotificationId" );

        entity.Property( e => e.Status )
            .HasColumnName( "Status" )
            .HasComment( "0 unread, 1 read" );

        entity.HasOne( d => d.Notification )
            .WithMany( p => p.NotificationAssigns )
            .HasForeignKey( d => d.NotificationId )
            .OnDelete( DeleteBehavior.ClientCascade )
            .HasConstraintName( "fk_notification_assign" );
      } );

      modelBuilder.Entity<Timesheet>( entity =>
      {
        entity.ToTable( "Timesheet" );

        entity.HasIndex( e => new { e.UserId, e.Date }, "fk_timesheet_user_date_idx" ).IsUnique();

        entity.HasIndex( e => e.UserId, "fk_timesheet_user_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.Date )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "Date" );

        entity.Property( e => e.LockBy ).HasColumnName( "LockBy" );

        entity.Property( e => e.SwapDay ).HasColumnType( "timestamp with time zone" ).HasColumnName( "SwapDay" );

        entity.Property( e => e.HolidayName ).HasMaxLength( 100 ).HasColumnName( "HolidayName" );

        //entity.Property( e => e.HourTotal ).HasColumnName( "HourTotal" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );

        entity.HasOne( d => d.User )
            .WithMany( p => p.Timesheets )
            .HasForeignKey( d => d.UserId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_timesheet_user" );

        entity.HasOne( d => d.WfhRequest )
            .WithMany( p => p.Timesheets )
            .HasForeignKey( d => d.WfhRequestId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_timesheet_wfhRequest" );
      } );

      modelBuilder.Entity<FingerPrinter>( entity =>
      {
        entity.ToTable( "FingerPrinter" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.TimesheetId ).HasColumnName( "TimesheetId" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.DateIn )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "DateIn" );

        entity.Property( e => e.DateOut )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "DateOut" );

        entity.Property( e => e.Note )
            .HasMaxLength( 200 )
            .HasColumnName( "Note" );

        entity.Property( e => e.HourTotal ).HasColumnName( "HourTotal" );

        entity.HasIndex( e => new { e.TimesheetId, e.DateIn }, "fk_timesheet_date_idx" ).IsUnique();

        entity.HasOne( d => d.Timesheet )
            .WithOne( p => p.FingerPrinter )
            .HasForeignKey<FingerPrinter>( d => d.TimesheetId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_fingerprint_timesheet" );
      } );

      modelBuilder.Entity<SwapDayRefer>( entity =>
      {
        entity.ToTable( "SwapDayRefer" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.SwapDayId ).HasColumnName( "SwapDayId" );

        entity.Property( e => e.HourTotal ).HasColumnName( "HourTotal" );

        entity.Property( e => e.DateIn )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "DateIn" );

        entity.Property( e => e.DateOut )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "DateOut" );

        entity.HasOne( d => d.FingerPrinter )
            .WithOne( p => p.SwapDayRefer )
            .HasForeignKey<SwapDayRefer>( d => d.FingerPrinterId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_swapday_refer_timesheet" );
      } );

      modelBuilder.Entity<TeamCategory>( entity =>
      {
        entity.ToTable( "TeamCategory" );

        entity.HasIndex( e => new { e.TeamId, e.Name }, "fk_timesheet_catogeries_teams_idx" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.IsUsing )
            .HasColumnName( "IsUsing" )
            .HasDefaultValue( false );

        entity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );

        entity.Property( e => e.TeamId ).HasColumnName( "TeamId" );

        entity.HasOne( d => d.Team )
            .WithMany( p => p.TeamCategories )
            .HasForeignKey( d => d.TeamId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_timesheet_categories_team" );
      } );

      modelBuilder.Entity<TimesheetDetail>( entity =>
      {
        entity.ToTable( "TimesheetDetail" );

        entity.HasIndex( e => e.ProjectId, "fk_timesheet_detail_project_id_idx" );

        entity.HasIndex( e => new { e.ReferenceId, e.Type }, "fk_timesheet_detail_type_idx" );

        entity.HasIndex( e => e.TimesheetId, "fk_timesheet_detail_timesheet_id_idx" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.Description )
            .HasMaxLength( 500 )
            .HasColumnName( "Description" );

        entity.Property( e => e.Hours ).HasColumnName( "Hours" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" );

        entity.Property( e => e.ProjectId ).HasColumnName( "ProjectId" );

        entity.Property( e => e.ReferenceId )
            .HasColumnName( "ReferenceId" )
            .HasComment( "reference to leave_request, holiday_id" );

        entity.Property( e => e.TimesheetCategoryId ).HasColumnName( "TimesheetCategoryId" );

        entity.Property( e => e.TimesheetId ).HasColumnName( "TimesheetId" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" )
            .HasComment( "0 Project, 1 UnpaidLeave, 2 PaidLeave" );

        entity.HasOne( d => d.Timesheet )
            .WithMany( p => p.TimesheetDetails )
            .HasForeignKey( d => d.TimesheetId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_timesheet_detail_timesheet" );
      } );

      modelBuilder.Entity<Role>( entity =>
      {
        entity.ToTable( "Role" );

        entity.Property( e => e.Type ).HasColumnName( "Type" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;
      } );

      modelBuilder.Entity<User>( entity =>
      {
        entity.ToTable( "User" );

        entity.Property( e => e.FullName )
                  .HasMaxLength( 200 )
                  .HasColumnName( "FullName" );

        entity.Property( e => e.DateStartWork )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "DateStartWork" );

        entity.Property( e => e.Avatar )
            .HasMaxLength( 500 )
            .HasColumnName( "Avatar" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" );

        entity.Property( e => e.IsActive )
            .HasColumnName( "IsActive" )
            .HasComment( "0 Inactive, 1 Active" );

        entity.Property( e => e.State )
            .HasColumnName( "State" )
            .HasDefaultValueSql( "0" ).HasComment( "0 Available, 1 WFH, 2 Business, 3 Busy, 4 Unavailable" );

        entity.Property( e => e.Position )
            .HasColumnName( "Position" )
            .HasMaxLength( 30 )
            .HasComment( "0 System, 1 Director, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership" );

        entity.Property( e => e.StaffId )
            .HasMaxLength( 30 )
            .HasColumnName( "StaffId" );

        entity.HasMany( d => d.Projects )
            .WithMany( p => p.Users )
            .UsingEntity<Dictionary<string, object>>(
                "ProjectUser",
                l => l.HasOne<Project>().WithMany().HasForeignKey( "ProjectId" ).OnDelete( DeleteBehavior.ClientSetNull ).HasConstraintName( "fk_project_users_project" ),
                r => r.HasOne<User>().WithMany().HasForeignKey( "UserId" ).OnDelete( DeleteBehavior.ClientSetNull ).HasConstraintName( "fk_project_users_user" ),
                j =>
                {
                  j.HasKey( "UserId", "ProjectId" ).HasName( "PRIMARY" ).HasAnnotation( "MySql:IndexPrefixLength", new [] { 0, 0 } );

                  j.ToTable( "ProjectUsers" );

                  j.HasIndex( new [] { "UserId", "ProjectId" }, "fk_project_user_idx" );

                  j.HasIndex( new [] { "ProjectId" }, "fk_project_users_project" );

                  j.IndexerProperty<long>( "UserId" ).HasColumnName( "UserId" );

                  j.IndexerProperty<long>( "ProjectId" ).HasColumnName( "ProjectId" );
                } );
      } );

      modelBuilder.Entity<UserInfo>( entity =>
      {
        entity.ToTable( "UserInfo" );

        entity.HasKey( e => e.UserId )
                  .HasName( "PRIMARY" );

        entity.HasIndex( e => e.UserId, "user_id_UNIQUE" )
            .IsUnique();

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );

        entity.Property( e => e.AboutMe )
            .HasMaxLength( 500 )
            .HasColumnName( "AboutMe" );

        entity.Property( e => e.Address )
            .HasMaxLength( 200 )
            .HasColumnName( "Address" );

        entity.Property( e => e.Birthday )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "Birthday" );

        entity.Property( e => e.Gender )
            .HasMaxLength( 20 )
            .HasColumnName( "Gender" )
            .HasDefaultValueSql( "'male'" );

        entity.Property( e => e.IdNo )
            .HasMaxLength( 20 )
            .HasColumnName( "IdNo" );

        entity.Property( e => e.IssuedBy )
            .HasMaxLength( 200 )
            .HasColumnName( "IssuedBy" );

        entity.Property( e => e.IssuedTo )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "IssuedTo" );

        entity.Property( e => e.FingerId ).HasColumnName( "FingerId" );

        entity.Property( e => e.PhoneNumber )
            .HasMaxLength( 20 )
            .HasColumnName( "PhoneNumber" );

        entity.Property( e => e.SocialInsuranceBookNo )
            .HasMaxLength( 200 )
            .HasColumnName( "SocialInsuranceBookNo" );
        entity.Property( e => e.AccountBank )
           .HasMaxLength( 50 )
           .HasColumnName( "AccountBank" );
        entity.Property( e => e.BankName )
           .HasMaxLength( 100 )
           .HasColumnName( "BankName" );
        entity.Property( e => e.Dependent )
           .HasColumnName( "Dependent" );
      } );

      modelBuilder.Entity<UserSetting>( entity =>
      {
        entity.ToTable( "UserSetting" );

        entity.HasKey( e => e.Id )
                  .HasName( "PRIMARY" );

        entity.HasIndex( e => e.Id, "user_id_UNIQUE" )
            .IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.EmailNotification )
            .HasColumnName( "EmailNotification" )
            .HasDefaultValue(true);

        entity.Property( e => e.Language )
            .HasMaxLength( 100 )
            .HasColumnName( "Language" )
            .HasDefaultValueSql( "'English'" );

        entity.Property( e => e.Timezone )
            .HasMaxLength( 100 )
            .HasColumnName( "Timezone" )
            .HasDefaultValueSql( "'BangKok, Ha Noi, Jakarta (UTC +7)'" );
      } );


      modelBuilder.Entity<SwapDay>( entity =>
      {
        entity.ToTable( "SwapDay" );

        entity.HasIndex( e => new { e.FromDate }, "fk_swap_day_idx" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.FromDate ).HasColumnType( "timestamp with time zone" ).HasColumnName( "FromDate" );

        entity.Property( e => e.ToDate ).HasColumnType( "timestamp with time zone" ).HasColumnName( "ToDate" );

        entity.Property( e => e.Description ).HasMaxLength( 200 ).HasColumnName( "Content" );
      } );

      modelBuilder.Entity<SwapDayUser>( entity =>
      {
        entity.ToTable( "SwapDayUser" );

        entity.HasIndex( e => new { e.UserId, e.SwapDayId }, "fk_swap_day_user_idx" ).IsUnique();

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.UserId ).HasColumnName( "UserId" );

        entity.Property( e => e.UserName ).HasMaxLength( 20 ).HasColumnName( "UserName" );

        entity.Property( e => e.SwapDayId ).HasColumnName( "SwapDayId" );

        entity.HasOne( d => d.SwapDay )
            .WithMany( p => p.SwapDayUsers )
            .HasForeignKey( d => d.SwapDayId )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "fk_swap_day" );
      } );

      modelBuilder.Entity<Config>( entity =>
      {
        entity.ToTable( "Config" );

        entity.Property( e => e.Id ).HasColumnName( "Id" );

        entity.Property( e => e.Code )
            .HasMaxLength( 30 )
            .HasColumnName( "Code" );

        entity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );
        entity.Property( e => e.Description )
           .HasColumnName( "Decription" );

        entity.Property( e => e.Type )
            .HasColumnName( "Type" );

        entity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );

        entity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );

        entity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );

        entity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
      } );
      modelBuilder.Entity<AssetCategory>( enity =>
      {
        enity.ToTable( "AssetCategory" );
        enity.HasIndex( e => e.Name, "asset_category_name_UNIQUE" ).IsUnique();
        enity.Property( e => e.Id ).HasColumnName( "Id" );
        enity.Property( e => e.Code ).HasMaxLength( 10 ).HasColumnName( "Code" );
        enity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );
        enity.Property( e => e.Description )
            .HasMaxLength( 200 )
            .HasColumnName( "Description" );
        enity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );
        enity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
        enity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );
        enity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" );
        enity.HasOne( ac => ac.Parent )
            .WithMany( ac => ac.Children )
            .HasForeignKey( ac => ac.ParentId )
            .OnDelete( DeleteBehavior.Restrict );
      } );
      modelBuilder.Entity<Asset>( enity =>
      {
        enity.ToTable( "Asset" );
        enity.HasIndex( e => e.AssetCode, "asset_code_UNIQUE" ).IsUnique();
        enity.Property( e => e.Id ).HasColumnName( "Id" );
        enity.Property( e => e.AssetCode )
            .HasMaxLength( 20 )
            .HasColumnName( "AssetCode" );
        enity.Property( e => e.Name )
            .HasMaxLength( 100 )
            .HasColumnName( "Name" );
        enity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );
        enity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
        enity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );
        enity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;
      } );
      modelBuilder.Entity<AssetAllocation>( enity =>
      {
        enity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );
        enity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
        enity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );
        enity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;
      } );
      modelBuilder.Entity<AssetComponentHistory>( enity =>
      {
        enity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );
        enity.Property( e => e.CreatedDate )
            .HasColumnType( "timestamp with time zone" )
            .HasColumnName( "CreatedDate" )
            .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
        enity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );
        enity.Property( e => e.ModifiedDate )
            .HasColumnType( "timestamp with time zone" )
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnName( "ModifiedDate" )
            ;
      } );
      modelBuilder.Entity<AssetHistory>( enity =>
         {
           enity.Property( e => e.CreatedBy ).HasColumnName( "CreatedBy" );
           enity.Property( e => e.CreatedDate )
               .HasColumnType( "timestamp with time zone" )
               .HasColumnName( "CreatedDate" )
               .HasDefaultValueSql( "CURRENT_TIMESTAMP" );
           enity.Property( e => e.ModifiedBy ).HasColumnName( "ModifiedBy" );
           enity.Property( e => e.ModifiedDate )
               .HasColumnType( "timestamp with time zone" )
               .ValueGeneratedOnAddOrUpdate()
               .HasColumnName( "ModifiedDate" )
               ;
         } );

      OnModelCreatingPartial( modelBuilder );

    }

    public override int SaveChanges()
    {
      NormalizeDateTimesToUtc();
      return base.SaveChanges();
    }

    public override int SaveChanges( bool acceptAllChangesOnSuccess )
    {
      NormalizeDateTimesToUtc();
      return base.SaveChanges( acceptAllChangesOnSuccess );
    }

    public override Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
    {
      NormalizeDateTimesToUtc();
      return base.SaveChangesAsync( cancellationToken );
    }

    public override Task<int> SaveChangesAsync( bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default )
    {
      NormalizeDateTimesToUtc();
      return base.SaveChangesAsync( acceptAllChangesOnSuccess, cancellationToken );
    }

    private void NormalizeDateTimesToUtc()
    {
      foreach ( var entry in ChangeTracker.Entries() ) {
        if ( entry.State != EntityState.Added && entry.State != EntityState.Modified ) {
          continue;
        }

        var properties = entry.Entity.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public );
        foreach ( var property in properties ) {
          if ( property.PropertyType == typeof( DateTime ) ) {
            var value = ( DateTime ) property.GetValue( entry.Entity )!;
            property.SetValue( entry.Entity, EnsureUtc( value ) );
          }
          else if ( property.PropertyType == typeof( DateTime? ) ) {
            var value = ( DateTime? ) property.GetValue( entry.Entity );
            if ( value.HasValue ) {
              property.SetValue( entry.Entity, EnsureUtc( value.Value ) );
            }
          }
        }
      }
    }

    private static DateTime EnsureUtc( DateTime value )
    {
      return value.Kind switch
      {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind( value, DateTimeKind.Utc )
      };
    }

    partial void OnModelCreatingPartial( ModelBuilder modelBuilder );

    //public override int SaveChanges()
    //{
    //  var entries = ChangeTracker
    //      .Entries()
    //      .Where( e => e.Entity is BaseEntity && (
    //              e.State == EntityState.Added
    //              || e.State == EntityState.Modified ) );

    //  foreach ( var entityEntry in entries ) {
    //  foreach ( var entityEntry in entries ) {
    //    ( ( BaseEntity ) entityEntry.Entity ).ModifiedDate = DateTime.UtcNow;

    //    if ( entityEntry.State == EntityState.Added ) {
    //      ( ( BaseEntity ) entityEntry.Entity ).CreatedDate = DateTime.UtcNow;
    //    }
    //  }

    //  return base.SaveChanges();
    //}
  }
}
