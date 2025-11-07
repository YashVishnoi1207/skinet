using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
// {
//     var connString = builder.Configuration.GetConnectionString("Redis")
//         ?? throw new Exception("Cannot get redis connection string");
//     var options = ConfigurationOptions.Parse(connString, true);
//     options.Ssl = true;
//     options.AbortOnConnectFail = false;
//     return ConnectionMultiplexer.Connect(options);
// });

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<StoreContext>();

builder.Services.AddCors();

builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
.WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthorization();

app.MapControllers();

app.MapGroup("api").MapIdentityApi<AppUser>(); // api/login

// try
// {
//     using var scope = app.Services.CreateScope();
//     var services = scope.ServiceProvider;
//     var content = services.GetRequiredService<StoreContext>();
//     await content.Database.MigrateAsync();
//     await StoreContextSeed.SeedAsync(content);    
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex);
//     throw;
// }
app.Run();
