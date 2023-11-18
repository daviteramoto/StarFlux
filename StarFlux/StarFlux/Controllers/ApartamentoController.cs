using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using StarFlux.DAO;
using StarFlux.Models;

namespace StarFlux.Controllers
{
    public class ApartamentoController : PadraoController<ApartamentoViewModel>
    {
        public ApartamentoController()
        {
            DAO = new ApartamentoDAO();
            GeraProximoId = true;
            ExigeAdministrador = true;
        }

        public override IActionResult Index()
        {
            try
            {
                PreparaListaTorresParaCombo();
                return View(NomeViewIndex);
            }
            catch (Exception erro) 
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected override void ValidaDados(ApartamentoViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome.");
        }

        protected override void PreencheDadosParaView(string Operacao, ApartamentoViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            if (Operacao == "I")
                model.DataCadastro = DateTime.Now;

            PreparaListaTorresParaCombo();
        }

        private void PreparaListaTorresParaCombo()
        {
            TorreDAO torreDAO = new TorreDAO();
            var torres = torreDAO.Listagem();
            List<SelectListItem> listaTorres = new List<SelectListItem>();
            listaTorres.Add(new SelectListItem("Selecione uma torre...", "0"));

            foreach (var torre in torres)
            {
                SelectListItem item = new SelectListItem(torre.Nome, torre.ID.ToString());
                listaTorres.Add(item);
            }

            ViewBag.Torres = listaTorres;
        }

        public IActionResult BuscaApartamentos(
            int codigo,
            string nome,
            DateTime dataInicial,
            DateTime dataFinal,
            int torre)
        {
            try
            {
                ApartamentoDAO dao = new ApartamentoDAO();

                if (string.IsNullOrEmpty(nome))
                    nome = "";
                if (dataInicial.Date == Convert.ToDateTime("01/01/0001"))
                    dataInicial = SqlDateTime.MinValue.Value;
                if (dataFinal.Date == Convert.ToDateTime("01/01/0001"))
                    dataFinal = SqlDateTime.MaxValue.Value;

                var lista = dao.BuscaApartamentosFiltro(codigo, nome, dataInicial, dataFinal,
                    torre);

                return PartialView("pvGridApartamentos", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }
}
