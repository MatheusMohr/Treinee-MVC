using System.ComponentModel.DataAnnotations;

namespace Senai_MVC.Models.Escola
{
    public class EscolaModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Campo nome é Obrigatório!")]
        public string Nome { get; set; }
    }
}
