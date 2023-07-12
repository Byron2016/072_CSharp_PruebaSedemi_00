using PruebaSedemi_00.MVC.Data.Entities;

namespace PruebaSedemi_00.MVC.ViewModels
{
    public class PokemonViewModel
    {
        public int? Count { get; set; }
        public object? Next { get; set; }
        public object? Previous { get; set; }
        public List<Pokemon>? Results { get; set; }
    }
}
