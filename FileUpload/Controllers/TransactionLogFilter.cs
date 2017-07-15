using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FileUpload.Controllers
{
    public class TransactionLogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing
            Debug.WriteLine("ACTION 1 DEBUG pre-processing logging");

            var request = actionContext.Request;
            if (request != null)
            {
                // log all this
                var httpContent = request.Content;
                var result = httpContent.ReadAsStringAsync().Result;


                // "OK"
                //var reasonPhrase = request.ReasonPhrase;

                //var httpRequestMessage = request.RequestMessage;

                //var requestUri = request.RequestMessage.RequestUri.AbsoluteUri;

                //// "POST"
                //var method = request.RequestMessage.Method.Method;

                //var isSuccessStatusCode = request.IsSuccessStatusCode;

                //// need headers/body
                //var headers = request.RequestMessage.Headers;
                //var authHeaders = headers.Authorization;

                //var headerKeys = headers.ToList();

                if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                {
                    var postArgs = actionContext.ActionArguments.ToList();
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                var type = objectContent.ObjectType; //type of the returned object
                var value = objectContent.Value; //holding the returned value
            }

            var response = actionExecutedContext.Response;
            if (response != null)
            {
                // log all this
                var httpContent = response.Content;

                // "OK"
                var reasonPhrase = response.ReasonPhrase;

                var httpRequestMessage = response.RequestMessage;

                var requestUri = response.RequestMessage.RequestUri.AbsoluteUri;

                // "POST"
                var method = response.RequestMessage.Method.Method;

                var isSuccessStatusCode = response.IsSuccessStatusCode;

                // need headers/body
                var headers = response.RequestMessage.Headers;
                var authHeaders = headers.Authorization;

                var headerKeys = headers.ToList();

                var content = response.RequestMessage.Content;
                var result = content.ReadAsStringAsync().Result;


            }

            Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
        }
    }
}