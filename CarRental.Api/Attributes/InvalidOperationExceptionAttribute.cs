using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace CarRental.Api.Attributes
{
	/// <summary>
	/// Handles the case when the Service returns an Invlaid Operation exception.
	/// </summary>
	public class InvalidOperationExceptionAttribute : ExceptionFilterAttribute
	{
		/// <summary>
		/// Action taken when exception occurs in the method.
		/// </summary>
		/// <param name="actionExecutedContext"></param>
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			if (actionExecutedContext.Exception is InvalidOperationException)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
				{
					Content = new StringContent($"Error: {actionExecutedContext.Exception.Message}"),
					ReasonPhrase = "Error"
				};

				throw new HttpResponseException(resp);
			}
		}
	}
}