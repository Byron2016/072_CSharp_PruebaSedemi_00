using Microsoft.EntityFrameworkCore;
using PruebaSedemi_00.MVC.Data;
using PruebaSedemi_00.MVC.Data.Entities;
using System.Net;

namespace PruebaSedemi_00.MVC.Repositories
{
    public class PokemonRepositoryDB : IPokemonRepositoryDB
    {
        private readonly ApplicationDbContext _context;

        public PokemonRepositoryDB(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Pokemon> GetByName(string name)
        {
            try
            {
                Pokemon? pokemons = await _context.Pokemons.FirstOrDefaultAsync(x => x.Name == name);
                if (pokemons != null)
                {
                    return pokemons;
                }

                return new Pokemon();

            }
            catch (Exception e)
            {
                //TODO
                var a = e;
                return new Pokemon();
            }
        }

        public async Task<int> Insert(Pokemon model)
        {
            try
            {
                Pokemon? pokemon = await _context.Pokemons.FirstOrDefaultAsync(x => x.Name == model.Name);
                if (pokemon == null)
                {
                    var result = await _context.Pokemons.AddAsync(model);
                    await _context.SaveChangesAsync();
                    //_context.SaveChanges();

                    return 1;
                }
                else
                {
                    return 1;
                }

            }
            catch (Exception e)
            {
                //TODO
                var a = e;
                return 0;
            }
        }

        public async Task<int> Update(Pokemon model)
        {
            try
            {
                Pokemon? pokemon = await _context.Pokemons.FirstOrDefaultAsync(x => x.Name == model.Name);
                if (pokemon != null)
                {
                    if (pokemon.Name != model.Name || pokemon.Url != model.Url || pokemon.IsSelected != model.IsSelected)
                    {
                        pokemon.Name = model.Name;
                        pokemon.Url = model.Url;
                        pokemon.IsSelected = model.IsSelected;

                        var pokemonChanged = _context.Pokemons.Attach(pokemon);
                        pokemonChanged.State = EntityState.Modified;
                        _context.SaveChanges();

                        return 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception e)
            {
                //TODO
                var a = e;
                return 0;
            }
        }

    }
}
