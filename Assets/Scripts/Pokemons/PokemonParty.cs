using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    // Listan över de Pokémon som finns i laget (max 6)
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons { get { return pokemons; } }

    private void Start()
    {
        // Förbereder alla Pokémon som ligger i listan från start
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    //  Funktion för att lägga till Pokémon i laget via KOD i spelet
    public void AddPokemonToParty(PokemonBase pBase, int level)
    {
        if (pokemons.Count < 6)
        {
            Pokemon newPokemon = new Pokemon(pBase, level);
            pokemons.Add(newPokemon);
        }
    }

    // Letar upp den första Pokémonen i laget som inte har svimmat (HP > 0)
    public Pokemon GetHealthyPokemon()
    {
        return pokemons.FirstOrDefault(x => x.HP > 0);
    }
}