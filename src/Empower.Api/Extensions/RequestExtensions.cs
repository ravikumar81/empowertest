using Empower.DTO;
using System.Net;
using System.Net.Http;

namespace Empower.Api.Extensions
{
    public static class RequestExtensions
    {
        public static HttpResponseMessage CreateResponseOK(this HttpRequestMessage @this, string message)
        {
            return @this.CreateResponse(HttpStatusCode.OK, new Result { Data = null, Message = message, ResultCount = 0 });
        }
        
        public static HttpResponseMessage CreateResponseOK(this HttpRequestMessage @this, object content, string message, int count)
        {
            return @this.CreateResponse(HttpStatusCode.OK, new Result { Data = content, Message = message, ResultCount = count });
        }


        public static HttpResponseMessage CreateResponseNotFound(this HttpRequestMessage @this, string message)
        {
            return @this.CreateResponse(HttpStatusCode.NotFound, new Result { Data = null, Message = message, ResultCount = 0 });
        }

        public static HttpResponseMessage CreateResponseNotFound(this HttpRequestMessage @this, object content, string message)
        {
            return @this.CreateResponse(HttpStatusCode.NotFound, new Result { Data = content, Message = message, ResultCount = 0 });
        }

        public static HttpResponseMessage CreateResponseAccepted(this HttpRequestMessage @this, object content, string message)
        {
            return @this.CreateResponse(HttpStatusCode.Accepted, new Result { Data = content, Message = message, ResultCount = 0 });
        }

        public static HttpResponseMessage CreateResponseBadRequest(this HttpRequestMessage @this, string message)
        {
            return @this.CreateResponse(HttpStatusCode.BadRequest, new Result { Data = null, Message = message, ResultCount = 0 });
        }

        public static HttpResponseMessage WithLocationHeader(this HttpResponseMessage @this, string location, string message)
        {
            if (@this == null) return null;
            @this.Headers.Add("Location", location);
            return @this;
        }
    }
}