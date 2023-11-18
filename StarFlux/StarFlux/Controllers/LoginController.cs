using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using StarFlux.Models;
using StarFlux.DAO;

namespace StarFlux.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AcessoNegado()
        {
            return View("AcessoNegado");
        }

        public IActionResult FazLogin(string login, string senha)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            UsuarioViewModel u = usuarioDAO.VerificaUsuario(login, senha);
            if (u != null)
            {
                if (u.Administrador == true)
                    HttpContext.Session.SetString("Administrador", "true");
                else
                    HttpContext.Session.SetString("Administrador", "false");
                HttpContext.Session.SetString("Logado", "true");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Erro = "Usuário ou senha inválidos!";
                return View("Index");
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                UsuarioViewModel model = new UsuarioViewModel();
                PreencheDadosParaView("I", model);
                return View("Cadastro", model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Save(UsuarioViewModel model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View("Cadastro", model);
                }
                else
                {
                    UsuarioDAO DAO = new UsuarioDAO();
                    if (Operacao == "I")
                        DAO.Insert(model);
                    else
                        DAO.Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected void PreencheDadosParaView(string Operacao, UsuarioViewModel model)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            if (Operacao == "I")
                model.ID = usuarioDAO.ProximoId();
        }

        protected void ValidaDados(UsuarioViewModel model, string operacao)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            ModelState.Clear();
            if (operacao == "I" && usuarioDAO.Consulta(model.ID) != null)
                ModelState.AddModelError("Id", "Código já está em uso!");
            if (operacao == "A" && usuarioDAO.Consulta(model.ID) == null)
                ModelState.AddModelError("Id", "Este registro não existe!");
            if (model.ID <= 0)
                ModelState.AddModelError("Id", "Id inválido!");            
            if (usuarioDAO.VerificaLoginExistente(model.Login))
                ModelState.AddModelError("Login", "Login já existente.");
            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("Senha", "Forneça uma senha.");
        }
    }
}
