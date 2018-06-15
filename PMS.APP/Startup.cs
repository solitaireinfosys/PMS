using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMS.APP.Context;

namespace PMS.APP
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
            services.AddMvc();

            //services.AddTransient<IDomainToolMicroservice, DomainToolMicroservice>();

            services.AddDbContext<PMSContext>(o => o.UseSqlServer(Configuration.GetConnectionString("PMSConnection"), b => b.UseRowNumberForPaging()));
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<PMSContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("PMSConnection"))
                    // this is needed unless you are on mssql 2012 or higher
                    //.UseRowNumberForPaging()
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
