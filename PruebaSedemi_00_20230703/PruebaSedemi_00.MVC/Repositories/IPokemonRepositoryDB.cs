using PruebaSedemi_00.MVC.Data.Entities;

namespace PruebaSedemi_00.MVC.Repositories
{
    public interface IPokemonRepositoryDB
    {
        Task<Pokemon> GetByName(string name);
        Task<int> Update(Pokemon pokemon);
        Task<int> Insert(Pokemon pokemon);
    }
}
