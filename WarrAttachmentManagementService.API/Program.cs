using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarrAttachmentManagementService.API;
using WarrAttachmentManagementService.API.Constants;
using WarrAttachmentManagementService.API.FilterAttributes;
using WarrAttachmentManagementService.API.Swagger;
using WarrAttachmentManagementService.Application;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Application.Validators;
using WarrAttachmentManagementService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilterAttribute>();
        options.Filters.Add<ValidationFilterAttribute>();
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<RepairOrderAttachmentValidator>();
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressInferBindingSourcesForParameters = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddAPIServices();

builder.Services.AddAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddAuthorizationWithPolicy();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

var app = builder.Build();

app.UseSwagger(app.Environment);

app.UseDatabaseInitializer();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();

internal static class ServiceCollectionExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.DocumentFilter<SwaggerDocumentFilter>();
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "WarrCloud Attachment Management API"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
        });
    }

    public static void AddCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public static void AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddAuthentication()
            .AddJwtBearer(AuthenticationSchemes.WarrCloud, options =>
            {
                var issuer = configuration["Jwt:Issuer"];

                options.RequireHttpsMetadata = !environment.IsDevelopment();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = "UserType"
                };
            })
            .AddJwtBearer(AuthenticationSchemes.Cognito, options =>
            {
                var issuer = $"{configuration["Cognito:Url"]}" +
                    $"{configuration["Cognito:UserPoolId"]}";
                var secretKeyUrl = $"{issuer}/.well-known/jwks.json";
                var audience = configuration["Cognito:AppClientId"];

                options.RequireHttpsMetadata = !environment.IsDevelopment();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = "custom:user_type",
                    IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) =>
                    {
                        if (securityToken.Issuer != issuer)
                            return Enumerable.Empty<JsonWebKey>();

                        // Wrap to avoid deadlocks
                        var task = Task.Run(async () =>
                    {
                        // Get the secret key from Cognito as it changes frequently
                        using var httpClient = new HttpClient();
                        return await httpClient.GetStringAsync(new Uri(secretKeyUrl));
                    });
                        task.Wait();

                        var keysJson = task.Result;

                        return JsonWebKeySet.Create(keysJson).Keys;
                    },
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userId = context.Principal
                            .FindFirst(p => p.Type == "custom:user_id")?
                            .Value;

                        var claims = new List<Claim>
                        {
                                new Claim(JwtRegisteredClaimNames.NameId, userId)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims);

                        context.Principal.AddIdentity(claimsIdentity);

                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddAuthorizationWithPolicy(
        this IServiceCollection services)
    {
        services
            .AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(
                        AuthenticationSchemes.WarrCloud,
                        AuthenticationSchemes.Cognito)
                    .Build();
            });
    }
}

internal static class WebAppExtensions
{
    public static void UseSwagger(
        this WebApplication app,
        IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment() ||
            environment.IsStaging())
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/attachment-swagger/{documentname}/swagger.json";

                if (environment.IsStaging())
                {
                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                                new OpenApiServer
                                {
                                    Url = $"{app.Configuration.GetValue<string>("ApiGatewayUrl")}"
                                }
                        };
                    });
                }
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "WarrCloud Attachment Management API V1");
                options.RoutePrefix = "attachment-swagger";
            });
        }
    }

    public static void UseDatabaseInitializer(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initialiser = scope.ServiceProvider
            .GetRequiredService<IAppDbContextInitializer>();

        initialiser.Migrate();
    }
}