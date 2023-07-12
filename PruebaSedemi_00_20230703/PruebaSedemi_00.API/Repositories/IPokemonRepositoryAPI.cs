using PruebaSedemi_00.API.Models;

namespace PruebaSedemi_00.API.Repositories
{
    public interface IPokemonRepositoryAPI
    {
        PokemonRecordAPI GetItems(string url);
    }
}
