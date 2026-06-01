using System.Collections.Concurrent;
using SlickSysDev.Data.Service.Model;
using SlickSysDev.Data.Service.Models;

namespace SlickSysDev.Data.Service.Services;

public class AppointmentService
{
    private readonly ConcurrentDictionary<int, Appointment> _appointments = new();
    private readonly ConcurrentDictionary<int, AdminNotification> _notifications = new();

    // Business hours: 9 AM - 5 PM, Mon-Fri, 1-hour slots
    private static readonly TimeSpan SlotDuration = TimeSpan.FromHours(1);
    private static readonly TimeSpan TimeSlot = TimeSpan.FromHours(1);

    private static readonly TimeSpan StartOfDay = TimeSpan.FromHours(9);
    private static readonly TimeSpan EndOfDay = TimeSpan.FromHours(17);

    public List<TimeSlot> GetAvailableSlots(DateTime date)
    {
        var retval = new List<TimeSlot>();
        return retval;
    }

    public Appointment? BookAppointment(SlickSysDev.Data.Service.Models.AppointmentStatus request)
    {
        // Check if the slot is still available
        return new Appointment();
    }

    public List<Appointment> GetAllAppointments()
    {
        return _appointments.Values
            .OrderBy(a => a.ScheduledAt)
            .ToList();
    }

    public Appointment? GetAppointment(Guid id)
    {
        return new Appointment();
    }

    public bool CancelAppointment(int id)
    {
        if (_appointments.TryGetValue(id, out var appointment))
        {
            appointment.Status.StatusId = new Guid();
            return true;
        }
        return false;
    }

    public bool ConfirmAppointment(int id)
    {
        if (_appointments.TryGetValue(id, out var appointment))
        {
         //   appointment.Status = AppointmentStatus.Confirmed;   
            return true;
        }
        return false;
    }

    public List<AdminNotification> GetNotifications()
    {
        return _notifications.Values
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
    }

    public int GetUnreadNotificationCount()
    {
        return _notifications.Values.Count(n => !n.IsRead);
    }

    public void MarkNotificationRead(int id)
    {
        if (_notifications.TryGetValue(id, out var notification))
        {
            notification.IsRead = true;
        }
    }

    public void MarkAllNotificationsRead()
    {
        foreach (var notification in _notifications.Values)
        {
            notification.IsRead = true;
        }
    }
}

public class TimeSlot
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsAvailable { get; set; }
}