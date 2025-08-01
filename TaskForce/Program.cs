using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TaskForce.Database;
using TaskForce.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUsersFasiService, UsersFasiService>();
builder.Services.AddScoped<IProgettoService, ProgettoService>();
builder.Services.AddScoped<IPresaInCaricoService, PresaInCaricoService>();
builder.Services.AddScoped<IFaseProgettoService, FaseProgettoService>();
builder.Services.AddScoped<IMacroFaseService, MacroFaseService>();
builder.Services.AddScoped<IPausaService, PausaService>();


builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskForce API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskForce API v1");
    });
}

var allowedClients = builder.Configuration.GetSection("AllowedClients").Get<string[]>();
app.UseCors(policy => policy.WithOrigins(allowedClients).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
