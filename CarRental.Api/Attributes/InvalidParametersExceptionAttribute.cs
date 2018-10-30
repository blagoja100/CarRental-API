using CarRental.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace CarRental.Api.Attributes
{
	/// <summary>
	/// Handles the case when the Service returns an InvalidParameters exception.
	/// </summary>
	public class InvalidParametersExceptionAttribute : ExceptionFilterAttribute
	{
		/// <summary>
		/// Action taken when exception occurs in the method.
		/// </summary>
		/// <param name="actionExecutedContext"></param>
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			if(actionExecutedContext.Exception is InvalidParameterException)
			{
				var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
				{
					Content = new StringContent(actionExecutedContext.Exception.Message),
					ReasonPhrase = "InvalidParameter"
				};

				throw new HttpResponseException(resp);
			}
		}
	}
}