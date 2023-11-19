﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using StarFlux.DAO;
using StarFlux.Models;
using System.Data.SqlClient;

namespace StarFlux.Controllers
{
    public class PadraoController<T> : Controller where T : PadraoViewModel
    {
        protected PadraoDAO<T> DAO { get; set; }
        protected bool GeraProximoId { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        protected string NomeViewForm { get; set; } = "cadastro";
        protected bool ExigeAutenticacao { get; set; } = true;
        protected bool ExigeAdministrador { get; set; } = false;

        public virtual IActionResult Index()
        {
            try
            {
                var lista = DAO.Listagem();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                T model = Activator.CreateInstance<T>();
                PreencheDadosParaView("I", model);
                return View(NomeViewForm, model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected virtual void PreencheDadosParaView(string Operacao, T model)
        {
            if (GeraProximoId && Operacao == "I")
                model.ID = DAO.ProximoId();
        }

        public virtual IActionResult Save(T model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (Operacao == "I")
                        DAO.Insert(model);
                    else
                        DAO.Update(model);
                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected virtual void ValidaDados(T model, string operacao)
        {
            ModelState.Clear();
            if (operacao == "I" && DAO.Consulta(model.ID) != null)
                ModelState.AddModelError("Id", "Código já está em uso!");
            if (operacao == "A" && DAO.Consulta(model.ID) == null)
                ModelState.AddModelError("Id", "Este registro não existe!");
            if (model.ID <= 0)
                ModelState.AddModelError("Id", "Id inválido!");
        }

        public IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Operacao = "A";
                var model = DAO.Consulta(id);
                if (model == null)
                    return RedirectToAction(NomeViewIndex);
                else
                {
                    PreencheDadosParaView("A", model);
                    return View(NomeViewForm, model);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public virtual IActionResult Delete(int id)
        {
            try
            {
                DAO.Delete(id);
                return RedirectToAction(NomeViewIndex);
            }
            catch (SqlException erro)
            {
                if (erro.Number == 547)
                    return View("Error", new ErrorViewModel("Não é possível deletar o registro pois ele está sendo utilizado no sistema."));

                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ExigeAutenticacao && !HelperControllers.VerificaUsuarioLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Login");
            else if (ExigeAdministrador && !HelperControllers.VerificaUsuarioAdministrador(HttpContext.Session))
                context.Result = RedirectToAction("AcessoNegado", "Login");
            else
            {
                ViewBag.Logado = true;
                ViewBag.Administrador = HelperControllers.VerificaUsuarioAdministrador(HttpContext.Session);
                base.OnActionExecuting(context);
            }
        }
    }
}
