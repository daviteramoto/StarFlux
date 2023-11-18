using Microsoft.AspNetCore.Http;
using System;

namespace StarFlux.Models
{
    public class HabitanteViewModel : PadraoViewModel
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int ID_Apartamento { get; set; }
        public byte[] FotoByte { get; set; }

        public string NomeApartamento { get; set; }
        public IFormFile Foto { get; set; }
        public string FotoBase64
        {
            get
            {
                if (FotoByte != null)
                    return Convert.ToBase64String(FotoByte);
                else
                    return string.Empty;
            }
        }
    }
}
