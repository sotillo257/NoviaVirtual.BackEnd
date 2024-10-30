using NoviaVirtual.BackEnd.Core;
using NoviaVirtual.BackEnd.Interfaces;
using NoviaVirtual.BackEnd.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<NoviaVirtual.BackEnd.Model.OpenAI>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddScoped<INoviaServices, NoviaServices>();
builder.Services.AddScoped<IOpenAIServices, OpenAIServices>();

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

app.UseAuthorization();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();

app.Run();
