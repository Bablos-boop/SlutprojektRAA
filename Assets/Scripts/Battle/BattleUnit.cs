using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public enum PokemonChoice
    {
        Infernape,
        Empoleon,
    }

    [SerializeField] Image pokemonImage;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] PokemonChoice baseChoice;
    [SerializeField] int level = 1;
    [SerializeField] PokemonBase infernapeBase;
    [SerializeField] PokemonBase empoleonBase;

    public Pokemon Pokemon { get; private set; }

    private void Reset()
    {
        pokemonImage = GetComponent<Image>();
    }

    public void SetupFromInspector()
    {
        PokemonBase selectedBase = GetSelectedBase();
        if (selectedBase == null)
        {
            Debug.LogWarning($"BattleUnit: No PokemonBase assigned for {baseChoice}", gameObject);
            return;
        }

        Pokemon = new Pokemon(selectedBase, Mathf.Max(1, level));
        Setup(Pokemon);
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if (pokemonImage == null)
            pokemonImage = GetComponent<Image>();

        if (pokemonImage == null)
            return;

        pokemonImage.sprite = isPlayerUnit ? pokemon.Base.BackSprite : pokemon.Base.FrontSprite;
    }

    private PokemonBase GetSelectedBase()
    {
        return baseChoice == PokemonChoice.Infernape ? infernapeBase : empoleonBase;
    }
}
