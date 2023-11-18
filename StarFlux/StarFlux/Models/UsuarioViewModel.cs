namespace StarFlux.Models
{
    public class UsuarioViewModel : PadraoViewModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Administrador { get; set; }
    }
}