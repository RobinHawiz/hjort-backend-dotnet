using HjortApi.Endpoints;
using HjortApi.Setups;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts => opts.ConfigureJson());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseAccess();
builder.Services.AddApplicationServices();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddCorsPolicies();
builder.Services.Configure<ApiBehaviorOptions>(opts => opts.ConfigureModelState());

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.AddUtilityEndpoints();

app.UseHttpsRedirection();

app.UseCorsSetup();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();