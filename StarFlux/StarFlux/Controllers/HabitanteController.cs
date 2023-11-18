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
    public class HabitanteController : PadraoController<HabitanteViewModel>
    {
        public HabitanteController()
        {
            DAO = new HabitanteDAO();
            GeraProximoId = true;
            ExigeAdministrador = true;
        }

        public override IActionResult Index()
        {
            try
            {
                PreparaListaApartamentosParaCombo();
                return View(NomeViewIndex);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public byte[] ConverterFotoToByte(IFormFile foto)
        {
            if (foto != null)
                using (var ms = new MemoryStream())
                {
                    foto.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        protected override void ValidaDados(HabitanteViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome.");

            if (model.Foto == null && operacao == "I")
                ModelState.AddModelError("Foto", "Anexe uma foto.");

            if (model.Foto != null && model.Foto.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Foto", "Foto limitada a 2MB.");

            if (ModelState.IsValid)
            {
                if (operacao == "A" && model.Foto == null)
                {
                    HabitanteViewModel habitante = DAO.Consulta(model.ID);
                    model.FotoByte = habitante.FotoByte;
                }
                else
                    model.FotoByte = ConverterFotoToByte(model.Foto);
            }

        }

        protected override void PreencheDadosParaView(string Operacao, HabitanteViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);

            PreparaListaApartamentosParaCombo();
        }

        private void PreparaListaApartamentosParaCombo()
        {
            ApartamentoDAO apartamentoDAO = new ApartamentoDAO();
            var apartamentos = apartamentoDAO.Listagem();
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem("Selecione um apartamento...", ""));

            foreach (var apartamento in apartamentos)
            {
                SelectListItem item = new SelectListItem(apartamento.Nome, apartamento.ID.ToString());
                lista.Add(item);
            }

            ViewBag.Apartamentos = lista;
        }

        public IActionResult BuscaHabitantes(
            int codigo,
            string nome,
            DateTime dataInicial,
            DateTime dataFinal,
            int apartamento)
        {
            try
            {
                HabitanteDAO dao = new HabitanteDAO();

                if (string.IsNullOrEmpty(nome))
                    nome = "";
                if (dataInicial.Date == Convert.ToDateTime("01/01/0001"))
                    dataInicial = SqlDateTime.MinValue.Value;
                if (dataFinal.Date == Convert.ToDateTime("01/01/0001"))
                    dataFinal = SqlDateTime.MaxValue.Value;

                var lista = dao.BuscaHabitantesFiltro(codigo, nome, dataInicial, dataFinal,
                    apartamento);

                return PartialView("pvGridHabitantes", lista);
            }
            catch (Exception erro)
            {
                return Json(new { erro = true, msg = erro.Message });
            }
        }
    }
}
