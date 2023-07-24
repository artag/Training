using Microsoft.AspNetCore.Mvc;

namespace ViewComponentExample.ViewComponents;

public class ContentFooter : ViewComponent
{
    private readonly IConfiguration _config;

    public ContentFooter(IConfiguration config)
    {
        _config = config;
    }

    public IViewComponentResult Invoke()
    {
        var model = _config.GetSection("About")["Production"];
        return View("Default", model);
    }
}
