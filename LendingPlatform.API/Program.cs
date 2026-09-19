using LendingPlatform.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register in-memory singleton service
builder.Services.AddSingleton<ILendingService, LendingService>();

// Configure CORS for React UI
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

app.Run();