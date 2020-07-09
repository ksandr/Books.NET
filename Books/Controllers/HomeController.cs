using Microsoft.AspNetCore.Mvc;

namespace Ksandr.Books.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return new VirtualFileResult("index.html", "text/html");
        }
    }
}
