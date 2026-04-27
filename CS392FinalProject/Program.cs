using CS392FinalProject.Models;
using CS392FinalProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages
builder.Services.AddRazorPages();

// Bind MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoDBSettings>>().Value);

// Register ProductService (MongoDB)
builder.Services.AddSingleton<ProductService>();

// ⭐ Bind Gemini settings
builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));

// ⭐ Register GeminiService
builder.Services.AddSingleton<GeminiService>();

// Add Controllers (API)
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Map Razor Pages
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
