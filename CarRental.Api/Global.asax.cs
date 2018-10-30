using System.Web.Http;

namespace CarRental.Api
{
	/// <summary>
	///
	/// </summary>
	public class WebApiApplication : System.Web.HttpApplication
	{
		/// <summary>
		///
		/// </summary>
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}