using System;
using System.Web.Mvc;
using System.Linq;
using System.Linq.Expressions;

namespace System.Web.Mvc
{
    public class ErrorResult : ActionResult
    {
        private string _error;
        private readonly bool _withModelError;

        public ErrorResult(string error)
            : this(error, false)
        {

        }

        public ErrorResult(string error, bool withModelError)
        {
            this._error = error;
            this._withModelError = withModelError;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 500;
            response.ContentType = "text/html";
            var controller = context.Controller as Controller;
            if ((_withModelError) && (controller != null))
            {
                _error += String.Join(
                    " ",
                    (from state in controller.ModelState select state)
                        .SelectMany(s => s.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray());
            }
            response.Write(_error);

        }
    }
}