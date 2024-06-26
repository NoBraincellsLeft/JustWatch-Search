
using DownloadApi.Services;
using Hangfire;
using Hangfire.Storage.SQLite;

namespace DownloadApi
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
            builder.Services.AddCors(opt=> opt.AddPolicy("default", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            builder.Services.AddScoped<IDownloadService, VinetrimmerDownloadService>();
            builder.Services.AddHangfire(configuration =>
                configuration.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSQLiteStorage());
            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("SWAGGER") == "true")
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("default");
            app.UseHangfireDashboard();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
