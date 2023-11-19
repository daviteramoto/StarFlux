using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StarFlux.Classes;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StarFlux.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Logado = HelperControllers.VerificaUsuarioLogado(HttpContext.Session);
            ViewBag.Administrador = HelperControllers.VerificaUsuarioAdministrador(HttpContext.Session);
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetVazaoTempoReal()
        {
            try
            {
                float vazao = await GetVazaoAPI();

                var data = new VazaoTempoRealDto
                {
                    Timestamp = DateTime.Now,
                    Vazao = vazao
                };

                return Json(data);
            }
            catch (Exception erro)
            {
                var erroJson = new
                {
                    Erro = erro.Message
                };

                return Json(erroJson);
            }
        }

        private async Task<float> GetVazaoAPI()
        {
            string apiUrl = "http://46.17.108.131:1026/v2/entities/urn:ngsi-ld:Flux:021/attrs/flux";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("fiware-service", "smart");
                client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(jsonContent);
                    float vazao = jsonObject["value"].Value<float>();

                    return vazao;
                }
                else
                    throw new Exception("Erro na requisição API.");
            }
        }
    }
}