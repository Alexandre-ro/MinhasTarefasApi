using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MinhasTarefasApi.Database;
using MinhasTarefasApi.Models;
using MinhasTarefasApi.Repositories;
using MinhasTarefasApi.Repositories.Contracts;

namespace MinhasTarefasApi
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
            services.AddDbContext<MinhasTarefasContext>(op => {
                op.UseNpgsql("Server=127.0.0.1;Port=5432;Database=tarefas;User Id=postgres;Password=private55;");
            });
            services.AddControllers();
            
            //Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();

            //Identity                   
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MinhasTarefasContext>();
           
            //Configura��o para quando n�o est� logado
            services.ConfigureApplicationCookie(options => {
                options.Events.OnRedirectToLogin = context => {
                    context.Response.StatusCode = 401;

                    return System.Threading.Tasks.Task.CompletedTask;
                };
            });
            
            //Suprimi a valida��o do ModelState
            services.Configure<ApiBehaviorOptions>(op => {
                op.SuppressModelStateInvalidFilter = true;
            });

             services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinhasTarefasApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinhasTarefasApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseStatusCodePages();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
