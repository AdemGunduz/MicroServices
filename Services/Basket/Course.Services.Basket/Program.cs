using Course.Services.Basket.Services;
using Course.Services.Basket.Settings;
using Course.Shread.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);


var Configuration = builder.Configuration;
// Add services to the container.


var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = Configuration["IdentityServerURL"];
    opt.Audience = "resource_basket";
    opt.RequireHttpsMetadata = false;

});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService , SharedIdentityService>();
builder.Services.AddScoped<IBasketService , BasketService>();
builder.Services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));


builder.Services.AddSingleton<RedisService>(sp =>
{
    var redisSetting = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

    var redis = new RedisService(redisSetting.Host, redisSetting.Port);

    redis.Connect();

    return redis;
}); 


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
