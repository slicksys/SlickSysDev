using System.Collections.Concurrent;
using SlickSysDev.ApiService.Models;

namespace SlickSysDev.ApiService.Services;

public class AppointmentService
{
    private readonly ConcurrentDictionary<Guid, Appointment> _appointments = new();
    private readonly ConcurrentDictionary<Guid, AdminNotification> _notifications = new();

    // Business hours: 9 AM - 5 PM, Mon-Fri, 1-hour slots
    private static readonly TimeSpan SlotDuration = TimeSpan.FromHours(1);
    private static readonly TimeSpan StartOfDay = TimeSpan.FromHours(9);
    private static readonly TimeSpan EndOfDay = TimeSpan.FromHours(17);

    public List<TimeSlot> GetAvailableSlots(DateTime date)
    {
        var slots = new List<TimeSlot>();

        // Only weekdays
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return slots;

        var dayStart = date.Date.Add(StartOfDay);
        var dayEnd = date.Date.Add(EndOfDay);
        var bookedTimes = _appointments.Values
            .Where(a => a.ScheduledAt.Date == date.Date && a.Status != AppointmentStatus.Cancelled)
            .Select(a => a.ScheduledAt)
            .ToHashSet();

        for (var slotStart = dayStart; slotStart < dayEnd; slotStart = slotStart.Add(SlotDuration))
        {
            var isAvailable = !bookedTimes.Contains(slotStart) && slotStart > DateTime.UtcNow;
            slots.Add(new TimeSlot
            {
                Start = slotStart,
                End = slotStart.Add(SlotDuration),
                IsAvailable = isAvailable
            });
        }

        return slots;
    }

    public Appointment? BookAppointment(AppointmentRequest request)
    {
        // Check if the slot is still available
        var existingAtTime = _appointments.Values
            .Any(a => a.ScheduledAt == request.ScheduledAt && a.Status != AppointmentStatus.Cancelled);

        if (existingAtTime)
            return null;

        var appointment = new Appointment
        {
            ClientName = request.ClientName,
            ClientEmail = request.ClientEmail,
            ClientPhone = request.ClientPhone,
            ServiceType = request.ServiceType,
            ScheduledAt = request.ScheduledAt,
            Notes = request.Notes
        };

        _appointments[appointment.Id] = appointment;

        // Create admin notification
        var notification = new AdminNotification
        {
            Message = $"New appointment scheduled by {appointment.ClientName} for {appointment.ScheduledAt:MMMM dd, yyyy 'at' h:mm tt} — Service: {appointment.ServiceType}",
            AppointmentId = appointment.Id
        };
        _notifications[notification.Id] = notification;

        return appointment;
    }

    public List<Appointment> GetAllAppointments()
    {
        return _appointments.Values
            .OrderBy(a => a.ScheduledAt)
            .ToList();
    }

    public Appointment? GetAppointment(Guid id)
    {
        return _appointments.GetValueOrDefault(id);
    }

    public bool CancelAppointment(Guid id)
    {
        if (_appointments.TryGetValue(id, out var appointment))
        {
            appointment.Status = AppointmentStatus.Cancelled;
            return true;
        }
        return false;
    }

    public bool ConfirmAppointment(Guid id)
    {
        if (_appointments.TryGetValue(id, out var appointment))
        {
            appointment.Status = AppointmentStatus.Confirmed;
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

    public void MarkNotificationRead(Guid id)
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
