using TvMaze.Data.Extensions;
using TvMaze.Scraper;
using TvMaze.Scraper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataAccess();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ITvMazeApiClient, TvMazeApiClient>();
builder.Services.AddHostedService<Scraper>();

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
