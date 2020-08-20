using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using Godwit.Common.Data;
using Godwit.Common.Data.Core;
using Godwit.Common.Data.Core.Repository;
using Godwit.Common.Data.Model;
using Godwit.HandleEmailConfirmedEvent.Model;
using Godwit.HandleEmailConfirmedEvent.Model.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Godwit.HandleEmailConfirmedEvent {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var dbConn =
                $"Server={Configuration["DB_HOST"]};Database={Configuration["HASURA_DB"]};User Id={Configuration["DB_USERNAME"]};Password={Configuration["DB_PASSWORD"]};";

            services.AddControllers()
                .AddFluentValidation(cfg => cfg.AutomaticValidationEnabled = false);
            services
                .AddDbContextPool<KetoDbContext>(opt => {
                    opt.UseNpgsql(dbConn, builder => {
                            builder.EnableRetryOnFailure(10, TimeSpan.FromMilliseconds(100), null!);
                            builder.CommandTimeout(60);
                            builder.UseAdminDatabase("postgres");
                            builder.UseNodaTime();
                        })
                        .UseSnakeCaseNamingConvention();
                });
            services.AddScoped<IRepository<Notification, long>, EfRepository<Notification, KetoDbContext, long>>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork<KetoDbContext>>();
            services.AddSingleton<IValidator<HasuraEvent>, HasuraEventValidator>();

            services.AddHealthChecks()
                .AddNpgSql(dbConn);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCors(opt => {
                opt.AllowAnyHeader();
                opt.WithMethods("POST");
                opt.AllowAnyOrigin();
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            });
        }
    }
}