using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.BO
{
    public class Pate
    {
        public int Id { get; set; }
        public string Nom { get; set; }
    }
}