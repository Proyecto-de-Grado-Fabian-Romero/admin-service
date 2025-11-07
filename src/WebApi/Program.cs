using AdminService.Src.Application.Interfaces;
using AdminService.src.Application.Mapping;
using AdminService.Src.Application.Ports;
using AdminService.Src.Application.Services;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Adapters;
using AdminService.Src.Infraestructure.Data;
using AdminService.Src.Infraestructure.Messaging;
using AdminService.Src.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile(
        $"appsettings.Production.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            if (allowedOrigins != null && allowedOrigins.Length > 0)
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }
            else
            {
                policy
                    .WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }
        }
    );
});

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Servicios de aplicación
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped<ITour360RequestService, Tour360RequestService>();
builder.Services.AddScoped<ITour360RequestRepository, Tour360RequestRepository>();
builder.Services.AddScoped<IOwnerPaymentService, OwnerPaymentService>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IOwnerEarningService, OwnerEarningService>();
builder.Services.AddScoped<IOwnerEarningRepository, OwnerEarningRepository>();
builder.Services.AddScoped<IAdminDebtService, AdminDebtService>();
builder.Services.AddScoped<IAdminDebtRepository, AdminDebtRepository>();
builder.Services.AddScoped<IAdminPaymentService, AdminPaymentService>();
builder.Services.AddScoped<IAdminPaymentRepository, AdminPaymentRepository>();
builder.Services.AddScoped<IOwnerDebtRepository, OwnerDebtRepository>();

// Configuración RabbitMQ - CORREGIDO: "RabbitMQ" no "RabbitMq"
// var rabbitOptions = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();
// builder.Services.AddSingleton(rabbitOptions);
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));

// Servicios RabbitMQ como SINGLETON
builder.Services.AddSingleton<IResponseListener, ResponseListener>();
builder.Services.AddSingleton<INotificationsPublisher, NotificationsAmqpPublisher>();
builder.Services.AddSingleton<IEnvironmentsPublisher, EnvironmentsAmqpPublisher>();

// SOLO LOS ADAPTERS AMQP - elimina los HTTP clients
builder.Services.AddScoped<IEnvironmentServiceAdapter, EnvironmentServiceAdapter>();
builder.Services.AddScoped<ITourUploaderAdapter, TourUploaderAdapter>();

builder.Services.AddAutoMapper(typeof(AdminProfile));

var app = builder.Build();

app.MapControllers();
app.UseCors("AllowFrontend"); // CORREGIDO: "AllowFrontend" no "AllowFrontEnd"

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
