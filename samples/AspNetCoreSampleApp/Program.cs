using Microsoft.EntityFrameworkCore;
using AspNetCoreSampleApp.Data;
using AspNetCoreSampleApp.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using EExpansions.AspNetCore.Caching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DbConnection")
    );
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});
builder.Services.AddDistributedMemoryCache();

builder.Services.AddEntityCache<ApplicationDbContext, RedisCache, DistributedEntityCacheValueContainer<MemoryDistributedCache>>(options =>
{
    options.KeyPrefix = "AspNetCoreSampleApp";
    options.DistributedCacheEntryOptions =
        new DistributedCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromHours(24))
        .SetSlidingExpiration(TimeSpan.FromHours(6));
    options.SemaphoreTimeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddSession<RedisCache>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
