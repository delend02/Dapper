using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using СlothingStore.API.Database.Repository;
using СlothingStore.API.Middleware;

namespace СlothingStore.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var section = Configuration.GetSection("ConnectionStrings");
            var connectionString = section.GetSection("DefaultPostgreeSQL").Value;

            services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(connectionString));
            services.AddTransient<IClothesRepository, ClothesRepository>(provider => new ClothesRepository(connectionString));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
