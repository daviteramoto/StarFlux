using Microsoft.AspNetCore.Mvc;
using StarFlux.DAO;
using StarFlux.Models;
using System;

namespace StarFlux.Controllers
{
    public class SensorController : PadraoController<SensorViewModel>
    {
        public SensorController() 
        {
            DAO = new SensorDAO();
            GeraProximoId = true;
            ExigeAdministrador = true;
        }

        protected override void ValidaDados(SensorViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome do sensor.");

            if (string.IsNullOrEmpty(model.Entidade))
                ModelState.AddModelError("Entidade", "Preencha o nome da entidade.");
        }

        public IActionResult BuscaSensores(int codigo, string nome, string entidade)
        {
            try
            {
                SensorDAO dao = new SensorDAO();

                if (string.IsNullOrEmpty(nome))
                    nome = "";
                if (string.IsNullOrEmpty(entidade))
                    entidade = "";

                var lista = dao.BuscaSensoresFiltro(codigo, nome, entidade);

                return PartialView("pvGridSensores", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }
}
