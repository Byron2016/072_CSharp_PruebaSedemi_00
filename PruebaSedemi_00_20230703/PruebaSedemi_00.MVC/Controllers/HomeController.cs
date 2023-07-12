using Microsoft.AspNetCore.Mvc;
using PruebaSedemi_00.API.Repositories;
using PruebaSedemi_00.MVC.Models;
using System.Diagnostics;

namespace PruebaSedemi_00.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IPokemonRepositoryAPI _pokemonRepositoryAPI;

        public HomeController(ILogger<HomeController> logger, IPokemonRepositoryAPI pokemonRepositoryAPI)
        {
            _logger = logger;
            _pokemonRepositoryAPI = pokemonRepositoryAPI;
        }

        public IActionResult Index()
        {
            string url = "https://pokeapi.co/api/v2/pokemon/";

            var result = _pokemonRepositoryAPI.GetItems(url);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}