using SlickSysDev.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<AppointmentService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/", () => "API service is running. Navigate to /weatherforecast to see sample data.");

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// ── Scheduling / Appointment endpoints ──────────────────────────────

app.MapGet("/appointments/slots", (DateTime date, AppointmentService svc) =>
{
    return Results.Ok(svc.GetAvailableSlots(date));
})
.WithName("GetAvailableSlots");

app.MapPost("/appointments", (SlickSysDev.ApiService.Models.AppointmentRequest request, AppointmentService svc) =>
{
    var appointment = svc.BookAppointment(request);
    return appointment is null
        ? Results.Conflict(new { message = "Time slot is no longer available." })
        : Results.Ok(appointment);
})
.WithName("BookAppointment");

app.MapGet("/appointments", (AppointmentService svc) =>
{
    return Results.Ok(svc.GetAllAppointments());
})
.WithName("GetAllAppointments");

app.MapGet("/appointments/{id:guid}", (Guid id, AppointmentService svc) =>
{
    var appointment = svc.GetAppointment(id);
    return appointment is null ? Results.NotFound() : Results.Ok(appointment);
})
.WithName("GetAppointment");

app.MapPut("/appointments/{id:guid}/cancel", (Guid id, AppointmentService svc) =>
{
    return svc.CancelAppointment(id) ? Results.Ok() : Results.NotFound();
})
.WithName("CancelAppointment");

app.MapPut("/appointments/{id:guid}/confirm", (Guid id, AppointmentService svc) =>
{
    return svc.ConfirmAppointment(id) ? Results.Ok() : Results.NotFound();
})
.WithName("ConfirmAppointment");

// ── Admin Notification endpoints ────────────────────────────────────

app.MapGet("/notifications", (AppointmentService svc) =>
{
    return Results.Ok(svc.GetNotifications());
})
.WithName("GetNotifications");

app.MapGet("/notifications/unread-count", (AppointmentService svc) =>
{
    return Results.Ok(new { count = svc.GetUnreadNotificationCount() });
})
.WithName("GetUnreadNotificationCount");

app.MapPut("/notifications/{id:guid}/read", (Guid id, AppointmentService svc) =>
{
    svc.MarkNotificationRead(id);
    return Results.Ok();
})
.WithName("MarkNotificationRead");

app.MapPut("/notifications/read-all", (AppointmentService svc) =>
{
    svc.MarkAllNotificationsRead();
    return Results.Ok();
})
.WithName("MarkAllNotificationsRead");

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
