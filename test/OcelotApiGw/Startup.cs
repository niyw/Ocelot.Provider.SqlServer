﻿using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Administration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.SqlServer;
using System;
using System.Reflection;

namespace OcelotApiGw {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            Action<IdentityServerAuthenticationOptions> idpOptions = o => {
                o.Authority = @"http://localhost:8500";
                o.ApiName = "apigw.admin";
                o.RequireHttpsMetadata = false;
                o.SupportedTokens = SupportedTokens.Both;
            };

            services.AddOcelot()
                .AddConfigStoredInSQLServer(cfg=> {
                    //set ocelot config db connectionstring name in appsetting.json, default is "OcelotConfigDB"
                    cfg.ConnectionName = "OcelotConfigDB";
                    cfg.ConnectionString = Configuration.GetConnectionString("OcelotConfigDB");
                    //set ocelot config db connectionstring herer, default is empty; you must set one of connectionName and ConnectionString
                    //cfg.ConnectionString = nsbAuthDBConnStr;
                    //set ocelot config table in sqlserver, default is "Ocelot_Configs"
                    //cfg.ConfigTableName = "Ocelot_Configs";
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            NLog.GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("OcelotConfigDB"));
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseOcelot().Wait();
        }
    }
}
