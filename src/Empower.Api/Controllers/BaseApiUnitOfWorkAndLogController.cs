using Empower.Api.Configuration;
using System.Web.Http;

namespace Empower.Api.Controllers
{
    [UnitOfWorkAndLogger]
    public class BaseApiUnitOfWorkAndLogController : ApiController {
    }
}