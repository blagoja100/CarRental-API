using log4net;
using Swashbuckle.Application;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace CarRental.Api
{
	/// <summary>
	///
	/// </summary>
	public static class WebApiConfig
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="config"></param>
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			ConfigureRoutes(config);
			ConfigureLogging();
			ConfigureCustomExceptions(config);
		}

		#region Private Conguration Methods

		private static void ConfigureRoutes(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
			  name: "swagger_root",
			  routeTemplate: "",
			  defaults: null,
			  constraints: null,
			  handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}"
			);
		}

		private static void ConfigureCustomExceptions(HttpConfiguration config)
		{
			config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionExpander());
			config.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());
		}

		private static void ConfigureLogging()
		{
			var logFileDir = Path.Combine("..\\", "logs");
			var logFilePath = Path.GetFullPath(Path.Combine(logFileDir, "main.log"));
			if (logFileDir.StartsWith("."))
			{
				// Relative path
				var webAppRoot = HostingEnvironment.MapPath("~/");
				logFilePath = Path.GetFullPath(Path.Combine(webAppRoot, logFileDir, "main.log"));
			}
			log4net.GlobalContext.Properties["LogFileName"] = logFilePath;
			log4net.Config.XmlConfigurator.Configure();
		}

		#endregion Private Conguration Methods

		#region UnhandledEexception Handling

		/// <summary>
		/// Entity framework exception handler filter.
		/// </summary>
		public class UnhandledExceptionExpander : ExceptionHandler
		{
			/// <inheritdoc />
			public override bool ShouldHandle(ExceptionHandlerContext context)
			{
				return true;
			}

			/// <inheritdoc />
			public override void Handle(ExceptionHandlerContext context)
			{
				ExceptionContext exceptionContext = context.ExceptionContext;

				HttpRequestMessage request = exceptionContext.Request;

				if (exceptionContext.CatchBlock == ExceptionCatchBlocks.IExceptionFilter)
				{
					// The exception filter stage propagates unhandled exceptions by default (when no filter handles the
					// exception).
					return;
				}

				context.Result = new ResponseMessageResult(request.CreateErrorResponse(HttpStatusCode.InternalServerError,
					"An error has occuerd. Please check the logs for more details."));
			}
		}

		/// <summary>
		/// Global Exception logger using log4net.
		/// </summary>
		public class UnhandledExceptionLogger : ExceptionLogger
		{
			private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

			/// <inheritdoc />
			public override void Log(ExceptionLoggerContext context)
			{
				if (context.CatchBlock == ExceptionCatchBlocks.IExceptionFilter)
				{
					return;
				}

				logger.Error(context.Exception);
				base.Log(context);
			}

			/// <inheritdoc />
			public override bool ShouldLog(ExceptionLoggerContext context)
			{
				return true;
			}
		}

		#endregion UnhandledEexception Handling
	}
}