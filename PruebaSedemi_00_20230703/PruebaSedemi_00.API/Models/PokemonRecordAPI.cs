namespace PruebaSedemi_00.API.Models
{
    public class PokemonRecordAPI
    {
        public int Count { get; set; } = 0;
        public object? Next { get; set; } = null;
        public object? Previous { get; set; } = null;
        public List<PokemonAPI>? Results { get; set; } = null;

        public ErrorAPI? ErrorStatus { get; set; } = null;
    }
}
