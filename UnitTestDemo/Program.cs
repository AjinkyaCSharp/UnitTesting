using Microsoft.EntityFrameworkCore;
using UnitTestDemo.Model;
using UnitTestDemo.Service;

namespace UnitTestDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<AccountsDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("OrgDbConnection");
                connectionString=connectionString.Replace("%CustomPath%", Environment.CurrentDirectory);
                options.UseSqlServer(connectionString
                    , options => options.EnableRetryOnFailure(
                        maxRetryCount: 5
                        , maxRetryDelay: TimeSpan.FromSeconds(5)
                        , errorNumbersToAdd: null));
            });
            builder.Services.AddScoped<IAccountsService, AccountsService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
           

            app.MapControllers();

            app.Run();
        }
    }
}