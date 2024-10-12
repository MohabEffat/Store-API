using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.Services.ProductService.Dtos;
using Store.Service.Services.ProductService;
using Store.Service.HandleResponse;
using Store.Service.Services.CacheService;
using Store.Service.Services.BasketService.DTOs;
using Store.Service.Services.BasketService;
using Store.Repository.Basket;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService;

namespace Store.Web.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                .Where(model => model.Value?.Errors.Count() > 0)
                                .SelectMany(model => model.Value!.Errors)
                                .Select(error => error.ErrorMessage).ToList();

                    var errorRespone = new ValidationErrorResopnse { Errors = errors };

                    return new BadRequestObjectResult(errorRespone);
                };
            });

            return services;

        }
    }
}
 