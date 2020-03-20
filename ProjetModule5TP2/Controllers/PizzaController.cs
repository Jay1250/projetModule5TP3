
using Models.BO;
using ProjetModule5TP2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.VM;

namespace ProjetModule5TP2.Controllers
{
    public class PizzaController : Controller
    {
        // GET: Pizza
        public ActionResult Index()
        {
            return View(FakeDb.Instance.Pizzas);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            PizzaViewModel vm = new PizzaViewModel();
            vm.Pates = transformPatesToSelectedListItem();
            vm.Ingredients = transformIngredientsToSelectedListItem();
            return View(vm);
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaViewModel vm)
        {
            vm.Pates = transformPatesToSelectedListItem();
            vm.Ingredients = transformIngredientsToSelectedListItem();

            try
            {
                if (ModelState.IsValid)
                {
                    Pizza pizza = vm.Pizza;
                    pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                    pizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(
                        x => vm.IdsIngredients.Contains(x.Id))
                        .ToList();

                    Pizza checkPizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Nom == vm.Pizza.Nom);

                    int  sameIngredientsPizza = FakeDb.Instance.Pizzas.Where(x => x.Ingredients.Count() == pizza.Ingredients.Count() && !x.Ingredients.Except(pizza.Ingredients).Any()).ToList().Count();


                    if (sameIngredientsPizza > 0)
                    {
                        ModelState.AddModelError("", "Une pizza avec ces ingrédients existe déjà");
                    }
                    else if (checkPizza != null)
                    {
                        ModelState.AddModelError("", "Ce nom de pizza existe déjà");
                    }
                    else if (pizza.Pate == null)
                    {
                        ModelState.AddModelError("", "Le paramètre Pate est obligatoire");
                    }
                    else if (pizza.Ingredients.Count() < 2)
                    {
                        ModelState.AddModelError("", "La pizza doit contenir au moins 2 ingrédients");
                    }
                    else if (pizza.Ingredients.Count() > 5)
                    {
                        ModelState.AddModelError("", "La pizza doit contenir au maximum 5 ingrédients");
                    }
                    else
                    {
                        pizza.Id = FakeDb.Instance.Pizzas.Count == 0 ? 1 : FakeDb.Instance.Pizzas.Max(x => x.Id) + 1;
                        FakeDb.Instance.Pizzas.Add(pizza);
                        return RedirectToAction("index");
                    }
                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError("","Une erreur est survenue lors de la soumission du formulaire");
                    return View(vm);
                }
            }
            catch
            {
                ModelState.AddModelError("", "WXCV");
                return View(vm);
            }
        }
        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaViewModel vm = new PizzaViewModel();

            vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);

            if (vm.Pizza.Pate != null)
            {
                vm.IdPate = vm.Pizza.Pate.Id;
            }

            if (vm.Pizza.Ingredients.Any())
            {
                vm.IdsIngredients = vm.Pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(PizzaViewModel vm)
        {
            vm.Pates = transformPatesToSelectedListItem();
            vm.Ingredients = transformIngredientsToSelectedListItem();

            if (ModelState.IsValid)
            {
                Pizza pizza = vm.Pizza;
                pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                pizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(
                    x => vm.IdsIngredients.Contains(x.Id))
                    .ToList();

                Pizza checkPizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Nom == vm.Pizza.Nom);
                int sameIngredientsPizza = FakeDb.Instance.Pizzas.Where(x => x.Ingredients.Count() == pizza.Ingredients.Count() && !x.Ingredients.Except(pizza.Ingredients).Any()).ToList().Count();

                if (sameIngredientsPizza > 0)
                {
                    ModelState.AddModelError("", "Une pizza avec ces ingrédients existe déjà");
                }
                else if (checkPizza == null)
                {
                    ModelState.AddModelError("", "Cette pizza n'existe pas");
                }
                else if (pizza.Pate == null)
                {
                    ModelState.AddModelError("", "Le paramètre Pate est obligatoire");
                }
                else if (pizza.Ingredients.Count() < 2)
                {
                    ModelState.AddModelError("", "La pizza doit contenir au moins 2 ingrédients");
                }
                else if (pizza.Ingredients.Count() > 5)
                {
                    ModelState.AddModelError("", "La pizza doit contenir au maximum 5 ingrédients");
                }
                else
                {
                    Pizza newPizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == vm.Pizza.Id);
                    newPizza.Nom = vm.Pizza.Nom;
                    newPizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                    newPizza.Ingredients = FakeDb.Instance.IngredientsDisponible.Where(x => vm.IdsIngredients.Contains(x.Id)).ToList();
                    return RedirectToAction("index");
                }
                return View(vm);
            }
            else
            {
                ModelState.AddModelError("", "Une erreur est survenue lors de la soumission du formulaire");
                return View(vm);
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            return View(FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id));
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
                FakeDb.Instance.Pizzas.Remove(pizza);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Une erreur est survenue lors de la soumission du formulaire");
                return View();
            }
        }

        private List<SelectListItem> transformPatesToSelectedListItem()
        {
            return FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();
        }

        private List<SelectListItem> transformIngredientsToSelectedListItem()
        {
            return FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();
        }
    }
}
