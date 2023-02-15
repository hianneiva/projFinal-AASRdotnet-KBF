using KnowledgeBaseForum.API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using static KnowledgeBaseForum.DataAccessLayer.DependencyInjection.DependencyInjectionHelper;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
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
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config[Constants.JWT_VALID_ISSUER],
            ValidAudience = config[Constants.JWT_VALID_AUDIENCE],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[Constants.JWT_SECRET]))
        };
    });
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Anonymous", policy => policy.AddRequirements(new AllowAnonymousAuthorizationRequirement()));
//    options.AddPolicy("Default", new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
//                                                                 .RequireAuthenticatedUser()
//                                                                 .Build());
//    options.AddPolicy("Admin", new AuthorizationPolicyBuilder().RequireRole(ClaimRoleNames.USER_ROLE_NAMES[2])
//                                                               .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
//                                                               .RequireAuthenticatedUser()
//                                                               .Build());
//});

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
