using System.Collections.Generic;
using System.IO;
using Ksandr.Books.Database;
using Ksandr.Books.Utils;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Ksandr.Books
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBooksContext(Configuration);

            services.AddOptions();

            services.AddOData();

            services.AddMvc(options => {
                // https://blogs.msdn.microsoft.com/webdev/2018/08/27/asp-net-core-2-2-0-preview1-endpoint-routing/
                // Because conflicts with ODataRouting as of this version could improve performance though
                options.EnableEndpointRouting = false;
            }).AddJsonOptions(options =>
            {
                JsonSerializerSettings settings = options.SerializerSettings;

                settings.Formatting = Formatting.Indented;
                settings.ContractResolver = new DefaultContractResolver();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.Converters.Add(new StringEnumConverter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    ProjectPath = Path.Combine(env.ContentRootPath, "ClientApp"),
                    EnvParam = new Dictionary<string, string> {
                        { "NODE_ENV", "development" }
                    },
                    HotModuleReplacement = true
                });
            }

            app.UseStaticFiles();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();

                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
                routeBuilder.MapODataServiceRoute("odata", "OData", GetEdmModel());

                routeBuilder.MapSpaFallbackRoute(
                   name: "spa-fallback",
                   defaults: new { controller = "Home", action = "Index" }
                );
            });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder()
            {
                Namespace = "Books"
            };

            builder.EntitySet<Author>("Authors").EntityType
                .Function("Books").ReturnsCollectionFromEntitySet<Book>("Books");

            builder.EntitySet<Genre>("Genres").EntityType
                .Function("Books").ReturnsCollectionFromEntitySet<Book>("Books");

            builder.EntitySet<Series>("Series").EntityType
                .Function("Books").ReturnsCollectionFromEntitySet<Book>("Books");

            return builder.GetEdmModel();
        }
    }
}
