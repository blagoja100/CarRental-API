using Autofac;
using Autofac.Integration.WebApi;
using CarRental.Data;
using CarRental.Service.Interfaces;
using Owin;
using System.Reflection;

namespace CarRental.Api
{
	/// <summary>
	///
	/// </summary>
	public class Startup
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="app"></param>
		public void Configuration(IAppBuilder app)
		{
			// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
			var configuration = System.Web.Http.GlobalConfiguration.Configuration;

			var builder = new ContainerBuilder();

			// Register controllers
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			builder.RegisterType<CarRentalDbContext>().AsSelf().InstancePerRequest();
			builder.RegisterAssemblyTypes(typeof(IClientAccountService).Assembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();

			// Set the WebApi dependency resolver.
			configuration.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
		}
	}
}