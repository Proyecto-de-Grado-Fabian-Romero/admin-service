using AdminService.Src.Application.Interfaces;
using AdminService.src.Application.Mapping;
using AdminService.Src.Application.Services;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Adapters;
using AdminService.Src.Infraestructure.Data;
using AdminService.Src.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IEnvironmentServiceAdapter, EnvironmentServiceAdapter>();

builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped<ITour360RequestService, Tour360RequestService>();
builder.Services.AddScoped<ITour360RequestRepository, Tour360RequestRepository>();
builder.Services.AddScoped<IOwnerPaymentService, OwnerPaymentService>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();

builder.Services.AddScoped<IAdminDebtService, AdminDebtService>();
builder.Services.AddScoped<IAdminDebtRepository, AdminDebtRepository>();
builder.Services.AddScoped<IAdminPaymentService, AdminPaymentService>();
builder.Services.AddScoped<IAdminPaymentRepository, AdminPaymentRepository>();

builder.Services.AddScoped<IEnvironmentServiceAdapter, EnvironmentServiceAdapter>();
builder.Services.AddScoped<IObjectDetectionAdapter, ObjectDetectionAdapter>();
builder.Services.AddScoped<ITourUploaderAdapter, TourUploaderAdapter>();

builder.Services.AddHttpClient<IEnvironmentServiceAdapter, EnvironmentServiceAdapter>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5150");
});

builder.Services.AddHttpClient<IObjectDetectionAdapter, ObjectDetectionAdapter>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5151");
});

builder.Services.AddAutoMapper(typeof(AdminProfile));

var app = builder.Build();

app.MapControllers();
app.UseCors("AllowFrontEnd");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
