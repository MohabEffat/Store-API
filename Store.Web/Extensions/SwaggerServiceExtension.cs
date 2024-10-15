using Microsoft.OpenApi.Models;

namespace Store.Web.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Store Api", Version = "V1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "bearer",
                        Type = ReferenceType.SecurityScheme
                    }

                };
                options.AddSecurityDefinition("bearer", securityScheme);

                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {securityScheme, new[] {"bearer"} }
                };
                options.AddSecurityRequirement(securityRequirements);
            });

            return services;

        }
    }
}
