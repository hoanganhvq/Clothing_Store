using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using vuapos_server.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.Run();

