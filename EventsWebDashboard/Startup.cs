using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAppCommon.Queue;
using EventsWebDashboard.Managers;
using EventsWebDashboard.Models;
using EventsWebDashboard.Queue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventsWebDashboard
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(typeof(IMasterDictionaryManager), typeof(MasterDictionaryManager));
            services.AddScoped(typeof(IEventsManager), typeof(EventsManager));
            services.AddSingleton(typeof(IQueueMessageConsumer), typeof(AggregatedEventsMessageConsumer));
            services.AddSingleton(typeof(ISingleMessageConsumer), typeof(MasterDictionarySingleMessageConsumer));            
        }
    
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IQueueMessageConsumer queueMessageConsumer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Events}/{action=Get}");
                routes.MapRoute("cleanEvents", "{controller=Events}/{action=CleanEvents}");
            });

            queueMessageConsumer.StartConsuming(); //start listening to queue
        }
    }
}
