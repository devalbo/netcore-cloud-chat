using ChatBackend;
using ChatBackend.Middleware.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BackendServices.AddServices(builder.Services);
BackendServices.ConfigureDb(builder.Services, false);

builder.Services.AddCors();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthentication();

app.UseAuthorization();

// custom jwt auth middleware
app.UseMiddleware<TokenMiddleware>();

app.MapControllers();

app.Run();
