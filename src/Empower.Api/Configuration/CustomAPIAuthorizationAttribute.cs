using Empower.Api.Extensions;
using Empower.DTO;
using Empower.Entities;
using Empower.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Empower.Api.Configuration
{
    public class CustomAPIAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            Task<string> content = context.Request.Content.ReadAsStringAsync();
            string body = content.Result;

            //context.Request.Headers.Add(Constants.UserId, "2");
            //return true;
            var routeData = context.ControllerContext.RouteData.Values.Values;
            HttpContext currentContext = HttpContext.Current;
            bool isAuthenticated = false;
            IEnumerable<string> headerValueForToken;

            bool accessTokenFound = context.Request.Headers.TryGetValues(Constants.AuthToken, out headerValueForToken);
            if (accessTokenFound)
                isAuthenticated = true; //ValidateAuthorization(headerValueForToken.FirstOrDefault(), context, routeData, body);
            else
                context.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);

            return isAuthenticated;
        }

        //private bool ValidateAuthorization(string token, HttpActionContext context, ICollection<object> routeParameters, string body)
        //{
        //    bool isAuthenticated = false;
        //    bool isAccessDenied = true;
        //    string message = "";

        //    var authService = (IUserTokenService)context.Request.GetDependencyScope().GetService(typeof(IUserTokenService));

        //    UserToken userToken = authService.GerUserTokenByToken(token);
        //    if (userToken == null)
        //    {
        //        message = ExceptionMessages.TokenDoesNotExists;
        //    }
        //    else
        //    {
        //        if (userToken.TokenExpirationDate < DateTime.Now)
        //        {
        //            message = ExceptionMessages.TokenIsExpired;
        //            isAccessDenied = false;
        //        }

        //        if (userToken.DeleteFlag == true)
        //        {
        //            message = ExceptionMessages.TokenIsDeleted;
        //            isAccessDenied = false;
        //        }

        //        if (string.IsNullOrWhiteSpace(message))
        //        {
        //            HttpContext currentContext = HttpContext.Current;
        //            currentContext.Items[Constants.UserId] = userToken.User.Id;
        //            currentContext.Items[Constants.AuthToken] = token;
        //            currentContext.Items[Constants.RequestedBody] = body;

        //            Constants.LogginedUserId = userToken.User.Id;

        //            bool isAdmin = false;
        //            currentContext.Items[Constants.IsAdmin] = isAdmin;

        //            isAuthenticated = true;

        //            string apiPath = context.Request.RequestUri.AbsolutePath;
        //            var apiPathInDb = apiPath;
        //            foreach (var route in routeParameters)
        //            {
        //                apiPathInDb = apiPathInDb.Replace("/" + route.ToString(), "/{0}");
        //            }

        //            apiPathInDb = apiPathInDb.Replace("v{0}", "v1");
        //            apiPathInDb = apiPathInDb.TrimEnd('/');

        //            string httpMethod = context.Request.Method.ToString();

        //            UserAccessDTO userAccessDTO = new UserAccessDTO();
        //            userAccessDTO.Permission = authService.CheckPermission(userToken.User.Id, apiPathInDb, httpMethod);
        //            userAccessDTO.UserId = userToken.User.Id;
        //            userAccessDTO.IsAdmin = false;

        //            if (userAccessDTO.Permission == null)
        //            {
        //                // isAccessDenied = false;  - This can be enabled once by passing is removed
        //                // message = ExceptionMessages.UnAuthorizedAccess; - This can be enabled once by passing is removed

        //                // BY PASSING 

        //                bool isAdminFlagStatus = authService.CheckForAdminByUserId(userToken.User.Id);
        //                userAccessDTO.IsAdmin = isAdminFlagStatus;
        //                currentContext.Items[Constants.IsAdmin] = isAdminFlagStatus;
        //            }
        //            else
        //            {
        //                if (userAccessDTO.Permission.IsView == false && httpMethod.ToLower() == "get")
        //                {
        //                    isAccessDenied = false;
        //                    message = ExceptionMessages.UnAuthorizedAccess;
        //                }
        //                else if (userAccessDTO.Permission.IsEditable == false && (httpMethod.ToLower() == "post" || httpMethod.ToLower() == "put" || httpMethod.ToLower() == "delete"))
        //                {
        //                    isAccessDenied = false;
        //                    message = ExceptionMessages.UnAuthorizedAccess;
        //                }
        //            }

        //            string serialize = JsonConvert.SerializeObject(userAccessDTO).ToString();
        //            currentContext.Items[Constants.UserData] = serialize;
        //        }

        //    }

        //    if (isAccessDenied == false)
        //    {
        //        var data = new Result { Data = null, Message = message, ResultCount = 0 };
        //        context.Response = new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.Forbidden,
        //            Content = new StringContent(JsonConvert.SerializeObject(data))
        //        };
        //    }

        //    return isAuthenticated;
        //}


    }
}