using Contenter.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;


namespace Contenter.Controllers.Api
{
    public class RootController : ApiController
    {

        protected virtual ResponseMessageResult MakeResponseError(int statusCode, string message)
        {
            return new ResponseMessageResult(Request.CreateErrorResponse((HttpStatusCode)statusCode, new HttpError(message)));
        }

        protected virtual ResponseMessage MakeErrorBlock(string code, string desc, Exception ex = null)
        {
            return new ResponseMessage { State = "err", Code = code, Desc = desc, Ex = ex };
        }

        protected virtual ResponseMessageResult MakeCustomResponse(int statusCode, object message)
        {
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)statusCode, message));
        }

        

    }
}
