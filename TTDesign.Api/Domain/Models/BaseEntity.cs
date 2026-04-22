namespace TTDesign.API.Domain.Models
{
  public abstract class BaseEntity
  {
    public long Id { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
  }

  public abstract class BaseCreateEntity
  {
    public long Id { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
  }

  public abstract class BaseSimpleEntity
  {
    public long Id { get; set; }
  }
}
