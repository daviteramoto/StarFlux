using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Filtros()
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

                var data = new
                {
                    Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
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

        [HttpGet]
        public async Task<JsonResult> GetVazaoIntervalo()
        {
            try
            {
                var valores = await GetVazaoIntervaloAPI();

                var data = valores.Select(valor => new
                {
                    Timestamp = valor.timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
                    Vazao = valor.vazao
                }).ToList();

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

        private async Task<List<(float vazao, DateTime timestamp)>> GetVazaoIntervaloAPI()
        {
            string apiUrl = "http://46.17.108.131:8666/STH/v1/contextEntities/type/Flux/id/urn:ngsi-ld:Flux:021/attributes/flux?dateFrom=2023-11-19T20:00:00.000Z&dateTo=2023-11-19T20:30:00.000Z&hLimit=100&hOffset=0";

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
                    var contextResponses = jsonObject["contextResponses"];

                    if (contextResponses.HasValues)
                    {
                        var attributes = contextResponses[0]["contextElement"]["attributes"][0]["values"];

                        var retorno = new List<(float Vazao, DateTime Timestamp)>();

                        foreach (var item in attributes)
                        {
                            float vazao = item["attrValue"].Value<float>();
                            DateTime timestamp = item["recvTime"].Value<DateTime>();

                            retorno.Add((vazao, timestamp));
                        }

                        return retorno;
                    }
                    else
                        throw new Exception("Erro na requisição API.");
                }
                else
                    throw new Exception("Erro na requisição API.");
            }
        }
    }
}