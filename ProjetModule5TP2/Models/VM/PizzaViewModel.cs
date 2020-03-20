using Models.BO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Models.VM
{
    public class PizzaViewModel
    {
        public Pizza Pizza { get; set; }
        public List<SelectListItem> Ingredients { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Pates { get; set; } = new List<SelectListItem>();

        public int IdPate { get; set; }
        public List<int> IdsIngredients { get; set; } = new List<int>();
        
    }
}