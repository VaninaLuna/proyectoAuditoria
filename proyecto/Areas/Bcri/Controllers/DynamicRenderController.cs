using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Areas.Bcri.Controllers
{

    public class DynamicRenderController : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            var res = this.JavaScriptFromView();
            res.ExecuteResult(ControllerContext);
        }
    }
}
