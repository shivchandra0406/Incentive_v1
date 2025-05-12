using Incentive.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Use only one of the service extension methods to avoid duplicate registrations
builder.Services.AddNewApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Incentive Management API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Use only one of the middleware extension methods to avoid duplicate registrations
app.UseNewApplicationMiddleware(app.Environment.IsDevelopment());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
