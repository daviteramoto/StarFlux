using Microsoft.AspNetCore.Mvc;
using StarFlux.DAO;
using StarFlux.Models;
using System;

namespace StarFlux.Controllers
{
    public class TorreController : PadraoController<TorreViewModel>
    {
        public TorreController()
        {
            DAO = new TorreDAO();
            GeraProximoId = true;
            ExigeAdministrador = true;
        }

        protected override void ValidaDados(TorreViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome da torre.");
        }

        public IActionResult BuscaTorres(int codigo, string nome)
        {
            try
            {
                TorreDAO dao = new TorreDAO();

                if (string.IsNullOrEmpty(nome))
                    nome = "";

                var lista = dao.BuscaTorresFiltro(codigo, nome);

                return PartialView("pvGridTorres", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }
}
