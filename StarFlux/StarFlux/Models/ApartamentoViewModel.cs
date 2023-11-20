using System;

namespace StarFlux.Models
{
    public class ApartamentoViewModel : PadraoViewModel
    {
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public int ID_Torre { get; set; }
        public int ID_Sensor { get; set; }

        public string NomeTorre { get; set; }
        public string NomeSensor { get; set; }
    }
}
