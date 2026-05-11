  namespace TTDesign.API.Domain.Models
  {
    /// <summary>
    /// thông tin chấm công, quan hệ 1:1 với bảng timesheet
    /// </summary>
    public partial class FingerPrinter : BaseEntity
    {
      public long TimesheetId { get; set; }
      /// <summary>
      /// giờ check in sớm nhất trong ngày
      /// </summary>
      public DateTime DateIn { get; set; }
      /// <summary>
      /// giờ check in muộn nhất trong ngày
      /// </summary>
      public DateTime DateOut { get; set; }
      /// <summary>
      /// ghi chú
      /// </summary>
      public string Note { get; set; } = null!;
      /// <summary>
      /// tổng khoảng thời gian giữa checkin và checkout (đơn vị giờ, block 15')
      /// </summary>
      public double HourTotal { get; set; }

      public virtual SwapDayRefer? SwapDayRefer { get; set; } = null!;
      public virtual Timesheet Timesheet { get; set; } = null!;
    }
  }
