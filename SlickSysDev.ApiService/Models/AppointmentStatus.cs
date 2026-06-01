#nullable disable

using SlickSysDev.Data.Service.Model;

namespace SlickSysDev.Data.Service.Models;
public partial class AppointmentStatus : BaseEntity
{
    public Guid StatusId { get; set; }

    public Guid PracticeId { get; set; }

    public string StatusName { get; set; }
    public static bool Cancelled { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }
    
    static bool Confirmed { get; set; }


    public string ColorCode { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Practice Practice { get; set; }
}