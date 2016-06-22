using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekLearning.AspNetCore.FlashMessage;
using Microsoft.AspNetCore.Mvc;

namespace GeekLearning.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private IFlashMessageManager flashMessageManager;

        public HomeController(FlashMessage.IFlashMessageManager flashMessageManager)
        {
            this.flashMessageManager = flashMessageManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            this.flashMessageManager.Push(new FlashMessage.FlashMessage
            {
                Message = "test message",
                Title = "toto",
                Type = FlashMessageType.Info & FlashMessageType.Modal,
                Actions = new[]
                {
                   new FlashMessageAction{ Title = "Annuler", Action = "" },
                   new FlashMessageAction{ Title = "Valider", Action = Url.Action(nameof(Contact)) }
                }
            });

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
