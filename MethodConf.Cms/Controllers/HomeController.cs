using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using IHostingEnvironment = Umbraco.Cms.Core.Hosting.IHostingEnvironment;

namespace MethodConf.Cms.Controllers;

public class HomeController : Controller
{
    private readonly IOptions<GlobalSettings> _globalSettings;
    private readonly IHostingEnvironment _hostingEnvironment;

    public HomeController(IOptions<GlobalSettings> globalSettings, IHostingEnvironment hostingEnvironment)
    {
        _globalSettings = globalSettings;
        _hostingEnvironment = hostingEnvironment;
    }

    public IActionResult Index()
    {
        return Redirect(_globalSettings.Value.GetUmbracoMvcArea(_hostingEnvironment));
    }
}
