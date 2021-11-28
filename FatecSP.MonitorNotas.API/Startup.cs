using FatecSP.MonitorNotas.API.Handles;
using FatecSP.MonitorNotas.API.Interfaces;
using FatecSP.MonitorNotas.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FatecSP.MonitorNotas.API
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
            services.AddScoped<IMonitorHandle, MonitorHandle>();
            services.AddScoped<ICrawlerService, CrawlerService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FatecSP.MonitorNotas.API", Version = "v1" , Description = "Essa aplicação serve para monitorar as notas de qualquer curso no portal do aluno da FATEC SP, enviando as notas para o e-mail inserido, conforme forem sendo disponibilizadas.<br>A aplicação verifica o portal a cada meia hora, caso tenha alguma nota nova, um e-mail é disparado.<br>\rEntre meia noite e meia noite e meia, um e-mail é disparado, indicando que o monitor ainda está ativo.<br>Nenhuma informação é gravada em banco de dados, as credenciais são mantidas em memória enquanto a aplicação estiver monitorando as notas.<br><br>Para utilizar, basta clicar em \"Monitor\", depois em \"Try it out\", preencher os campos e clicar em \"Execute\".<br><br>O código dessa aplicação está disponível no repositório: https://github.com/danilloaragao/FatecSP.MonitorNotas.API" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FatecSP.MonitorNotas.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
