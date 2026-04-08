using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;

namespace MethodConf.Cms.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect($"/{Constants.System.UmbracoPathSegment}");
    }
}
