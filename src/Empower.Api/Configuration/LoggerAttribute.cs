using Empower.DTO;
using Empower.Repository;

using Newtonsoft.Json;
using Serilog;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Empower.Api.Configuration
{
    public class LoggerAttribute : ActionFilterAttribute {

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext) {
            
            string rawRequest;
            if (actionContext.Request.Content.Headers.ContentType != null && actionContext.Request.Content.Headers.ContentType.MediaType == "multipart/form-data") {
                rawRequest = string.Empty;
            } else {
                using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result)) {
                    stream.BaseStream.Position = 0;
                    rawRequest = stream.ReadToEnd();
                }
            }

            Log.Information(actionContext.Request.RequestUri + "\r\n" + actionContext.Request.Method + "\r\n" + actionContext.Request.Headers + "\r\n" + rawRequest);

        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
            
            if (actionExecutedContext.Exception != null) {
                string message = actionExecutedContext.Exception.Message;
                HttpContent responseContent = new StringContent(JsonConvert.SerializeObject(new Result { Data = null, Message = message, ResultCount = 0 }));
                
                if (actionExecutedContext.Exception is IsInvalidException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest, Content = responseContent };
                }
                else if (actionExecutedContext.Exception is IsExistException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Found, Content = responseContent };
                }
                else if (actionExecutedContext.Exception is IsNotFoundException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound, Content = responseContent };
                }
                else if (actionExecutedContext.Exception is IsRequiredException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest, Content = responseContent };
                }
                else if (actionExecutedContext.Exception is IsDeletedException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound, Content = responseContent };
                }
                else if (actionExecutedContext.Exception is IsLengthRequired)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.LengthRequired, Content = responseContent };
                }
                else if (actionExecutedContext.Exception is IsForbiddenException)
                {
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Forbidden, Content = responseContent };
                }
                else
                {
                    HttpContent responseContentDefault = new StringContent(JsonConvert.SerializeObject(new Result { Data = null, Message = string.Format("Internal Server Error : {0}", actionExecutedContext.Exception), ResultCount = 0 }));
                    actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError, Content = responseContentDefault };
                }
                InsertErrorLogs(actionExecutedContext);
                Log.Error(actionExecutedContext.Request + "\r\n" + " Internal Server Error " + "\r\n", actionExecutedContext.Exception);

            } else {                

                Log.Information(actionExecutedContext.Response.RequestMessage.RequestUri + "\r\n" + actionExecutedContext.Request.Method + "\r\n" + actionExecutedContext.Response.StatusCode + "\r\n");

            }
        }

        public void InsertErrorLogs(HttpActionExecutedContext actionExecutedContext)
        {
            string status = ConfigurationManager.AppSettings["Empower-APILogs"];
            if (status == "on")
            {
                string connection = ConfigurationManager.ConnectionStrings["EmpowerContext"].ConnectionString;
                try
                {
                    string method = actionExecutedContext.Request.Method.ToString();
                    string requestUri = actionExecutedContext.Request.RequestUri.ToString();
                    string error = actionExecutedContext.Exception.Message;
                    int loggedInUserId = (int)HttpContext.Current.Items[Constants.UserId];
                    string body = (string)HttpContext.Current.Items[Constants.RequestedBody];

                    using (SqlConnection con = new SqlConnection(connection))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand("Insert into APILog(Method,Request,Body,Response,SystemCreatedDateTime,CreatedBy,DeleteFlag)values('" + method + "','" + requestUri + "','" + body + "','" + error + "','" + DateTime.UtcNow + "'," + loggedInUserId + ",0)", con))
                        {
                            command.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                }
                catch (SystemException ex)
                {
                }
            }
        }

    }
}