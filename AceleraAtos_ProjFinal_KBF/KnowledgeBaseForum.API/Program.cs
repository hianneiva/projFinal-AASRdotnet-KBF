using KnowledgeBaseForum.API.Services;
using KnowledgeBaseForum.API.Utils;
using KnowledgeBaseForum.Commons.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using static KnowledgeBaseForum.DataAccessLayer.DependencyInjection.DependencyInjectionHelper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddConfig(config).AddScopedDepedencies();
builder.Services.AddHttpClient();
builder.Services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddCors(options => options.AddPolicy(name: Constants.CORS_POLICY_NAME, policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddInfrastructureDb(config);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[Const.JWT_SECRET])),
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            //ValidAudience = config[Const.JWT_VALID_AUDIENCE],
            //ValidIssuer = config[Const.JWT_VALID_ISSUER],
            SaveSigninToken = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Const.ROLE_ADMIN, policy => policy.RequireRole(Const.ROLE_ADMIN));
    options.AddPolicy(Const.ROLE_NORMAL, policy => policy.RequireRole(Const.ROLE_NORMAL));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(Constants.CORS_POLICY_NAME);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
