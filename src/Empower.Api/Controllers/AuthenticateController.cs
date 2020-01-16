using Empower.Api.Configuration;
using Empower.Api.Extensions;
using Empower.DTO;
using Empower.Service;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Empower.Api.Controllers
{
    public class AuthenticateController : BaseApiUnitOfWorkAndLogController
    {
        #region Private Members

        // private readonly IAuthenticateService _authenticateServiceService;

        #endregion

        #region Constructors / Destructors       

        //public AuthenticateController(IAuthenticateService authenticateServiceService)
        //{
        //    _authenticateServiceService = authenticateServiceService;
        //}

        #endregion

        #region API Methods
      
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Result))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.Found, Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.LengthRequired, Type = typeof(string))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(string))]
        [HttpPost]
        [Route(APIUrlConstants.PostLogin)]
        public HttpResponseMessage Post(LoginPostDTO loginPostDTO)
        {
            //var login = _authenticateServiceService.Authenticate(loginPostDTO);
            //if (login == null)
            //    return Request.CreateResponseNotFound(ExceptionMessages.NoResults);
            //else
            //    return Request.CreateResponseOK(login, ExceptionMessages.Successful, 1);
            return Request.CreateResponseOK(loginPostDTO, ExceptionMessages.Successful, 1);
        }

        #endregion

    }

}