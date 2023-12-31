﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using StarFlux.DAO;
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
            PreencheViewBag();
            PreparaListaApartamentosParaCombo();
            return View();
        }

        public IActionResult Filtros()
        {
            PreencheViewBag();
            PreparaListaApartamentosParaCombo();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetVazaoTempoReal(string entidade)
        {
            try
            {
                var dados = await GetVazaoAPI(entidade);

                var data = new
                {
                    Timestamp = dados.timestamp.AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss"),
                    Vazao = dados.vazao
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

        private async Task<(float vazao, DateTime timestamp)> GetVazaoAPI(string entidade)
        {
            string apiUrl = $"http://46.17.108.131:1026/v2/entities/urn:ngsi-ld:{entidade}/attrs/flux";

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
                    DateTime timestamp = jsonObject["metadata"]["TimeInstant"]["value"].Value<DateTime>();

                    return (vazao, timestamp);
                }
                else
                    throw new Exception("Erro na requisição API.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetVazaoIntervalo(DateTime dataInicial, DateTime dataFinal, TimeSpan horaInicial, TimeSpan horaFinal, string entidade)
        {
            try
            {
                if (dataInicial == DateTime.MinValue)
                {
                    dataInicial = DateTime.Now.AddMinutes(-60).Date;
                    horaInicial = DateTime.Now.AddMinutes(-60).TimeOfDay;
                }

                if (dataFinal == DateTime.MinValue)
                {
                    dataFinal = DateTime.Now.Date;
                    horaFinal = DateTime.Now.TimeOfDay;
                }

                DateTime dataHoraInicial = dataInicial.Date.Add(horaInicial);
                DateTime dataHoraFinal = dataFinal.Date.Add(horaFinal);

                var valores = await GetVazaoIntervaloAPI(dataHoraInicial, dataHoraFinal, entidade);

                var data = valores.Select(valor => new
                {
                    Timestamp = valor.timestamp.AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss"),
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

        private async Task<List<(float vazao, DateTime timestamp)>> GetVazaoIntervaloAPI(DateTime dataHoraInicial, DateTime dataHoraFinal, string entidade)
        {
            string dataInicialFormatada = dataHoraInicial.AddHours(3).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string dataFinalFormatada = dataHoraFinal.AddHours(3).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            string apiUrl = $"http://46.17.108.131:8666/STH/v1/contextEntities/type/Flux/id/urn:ngsi-ld:{entidade}/attributes/flux?dateFrom={dataInicialFormatada}&dateTo={dataFinalFormatada}&hLimit=100&hOffset=0";

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

        private void PreparaListaApartamentosParaCombo()
        {
            ApartamentoDAO apartamentoDAO = new ApartamentoDAO();
            var apartamentos = apartamentoDAO.ListagemApartamentosSensores();
            List<SelectListItem> lista = new List<SelectListItem>();

            foreach (var apartamento in apartamentos)
            {
                SelectListItem item = new SelectListItem(apartamento.Nome, apartamento.EntidadeSensor);
                lista.Add(item);
            }

            ViewBag.ApartamentosSensores = lista;
        }

        private void PreencheViewBag()
        {
            ViewBag.Logado = HelperControllers.VerificaUsuarioLogado(HttpContext.Session);
            ViewBag.Administrador = HelperControllers.VerificaUsuarioAdministrador(HttpContext.Session);
        }
    }
}