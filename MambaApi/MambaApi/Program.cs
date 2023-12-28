using FluentValidation.AspNetCore;
using MambaApi.Business.DTO.ProfessionDtos;
using MambaApi.Business.MappingProfile;
using MambaApi.Business.Services;
using MambaApi.Business.Services.Implementations;
using MambaApi.Core.Repositories;
using MambaApi.Data.DataAccessLayer;
using MambaApi.Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssembly(typeof(ProfessionCreateDtoValidator).Assembly);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("myDb1"));
});
builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<IWorkerService, WorkerService>();

builder.Services.AddScoped<IWorkerProfessionRepository, WorkerProfessionRepository>();

builder.Services.AddScoped<IProfessionRepository, ProfessionRepository>();
builder.Services.AddScoped<IProfessionService, ProfessionService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
