using LeagueApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var cs = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlite(cs);
});

var AllowNg = "_allowNg";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(AllowNg, p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

builder.Services.AddSwaggerGen(o =>
{
    var xml = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var path = System.IO.Path.Combine(AppContext.BaseDirectory, xml);
    o.IncludeXmlComments(path, includeControllerXmlComments: true);
});


var app = builder.Build();
app.UseCors(AllowNg);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();


