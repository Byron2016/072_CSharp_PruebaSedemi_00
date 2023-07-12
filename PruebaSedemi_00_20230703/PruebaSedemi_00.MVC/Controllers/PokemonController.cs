using Microsoft.AspNetCore.Mvc;
using PruebaSedemi_00.API.Repositories;
using PruebaSedemi_00.MVC.Data.Entities;
using PruebaSedemi_00.MVC.Repositories;
using PruebaSedemi_00.MVC.ViewModels;

namespace PruebaSedemi_00.MVC.Controllers
{
    public class PokemonController : Controller
    {
        private readonly IPokemonRepositoryAPI _pokemonRepositoryAPI;

        private readonly IPokemonRepositoryDB _pokemonRepositorDB;

        private IConfiguration _configuration;

        public PokemonController(IPokemonRepositoryAPI pokemonRepositoryAPI, IPokemonRepositoryDB pokemonRepositorDB, IConfiguration configuration)
        {
            _pokemonRepositoryAPI = pokemonRepositoryAPI;
            _pokemonRepositorDB = pokemonRepositorDB;
            _configuration = configuration;
        }

        public async Task Testear()
        {

            try
            {
                var pokemonFromDB = await _pokemonRepositorDB.GetByName("test01");

            }
            catch (Exception e)
            {
                //TODO
                var a = e;
            }
        }
        public async Task<ActionResult> Index(string url)
        {
            if(url == null)
            {
                url = _configuration["API_URL"];
            }

            var pokemonFromAPI = _pokemonRepositoryAPI.GetItems(url);
            var pokemons = pokemonFromAPI.Results;

            PokemonViewModel pokemonViewModel = new PokemonViewModel();
            pokemonViewModel.Count = pokemonFromAPI.Count;
            pokemonViewModel.Next = pokemonFromAPI.Next;
            pokemonViewModel.Previous = pokemonFromAPI.Previous;

            if (pokemons != null)
            {
                List<Pokemon> pokemonsTemp = new List<Pokemon>();

                foreach (var pokemon in pokemons)
                {
                    var pokemonFromDB = await _pokemonRepositorDB.GetByName(pokemon.Name);

                    if (pokemonFromDB != null && pokemonFromDB.Name != null)
                    {
                        pokemonsTemp.Add(
                            new Pokemon
                            {
                                Name = pokemon.Name,
                                Url = pokemon.Url,
                                IsSelected = pokemonFromDB.IsSelected
                            });
                    }
                    else
                    {
                        pokemonsTemp.Add(
                            new Pokemon
                            {
                                Name = pokemon.Name,
                                Url = pokemon.Url,
                                IsSelected = pokemon.IsSelected
                            });
                    }
                }

                pokemonViewModel.Results = pokemonsTemp;

            }

            ViewData["urlRetorn"] = url;
            return View(pokemonViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PokemonViewModel model, string urlRetorn)
        {
            if (model.Results != null)
            {
                foreach (var pokemon in model.Results)
                {
                    Pokemon pok = new Pokemon()
                    {
                        Name = pokemon.Name,
                        Url = pokemon.Url,
                        IsSelected = pokemon.IsSelected
                    };

                    var resul = await _pokemonRepositorDB.Update(pok);

                    if (resul == 2)
                    {
                        //No existe en base hay que insertarlo.
                        resul = await _pokemonRepositorDB.Insert(pok);
                    }

                    if (resul == 0)
                    {
                        Console.WriteLine($"Existio un problema con {pokemon.Name}");
                    }
                }
            }
            
            return RedirectToAction("index", "Pokemon", new { url = urlRetorn });
        }
    }
}
