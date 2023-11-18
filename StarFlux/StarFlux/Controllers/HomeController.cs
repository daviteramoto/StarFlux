using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarFlux.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StarFlux.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Logado = HelperControllers.VerificaUsuarioLogado(HttpContext.Session);
            ViewBag.Administrador = HelperControllers.VerificaUsuarioAdministrador(HttpContext.Session);
            return View();
        }

        public IActionResult Sobre()
        {
            ViewBag.Logado = HelperControllers.VerificaUsuarioLogado(HttpContext.Session);
            ViewBag.Administrador = HelperControllers.VerificaUsuarioAdministrador(HttpContext.Session);
            return View("Sobre");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
