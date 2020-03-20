using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.BO
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Nom { get; set; }
    }
}