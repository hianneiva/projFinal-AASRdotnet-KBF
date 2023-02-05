using KnowledgeBaseForum.API.Utils;
using System.Text.Json.Serialization;
using static KnowledgeBaseForum.DataAccessLayer.DependencyInjection.DependencyInjectionHelper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy(name: Constants.CORS_POLICY_NAME, policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().WithHeaders(config[Constants.CONFIG_SECRET_HEADER_NAME]);
}));
builder.Services.AddInfrastructureDb(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(Constants.CORS_POLICY_NAME);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
