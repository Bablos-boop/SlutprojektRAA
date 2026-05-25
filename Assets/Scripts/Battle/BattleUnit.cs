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

    public Pokemon Pokemon { get; set; }

public void Setup(Pokemon pokemon)
{
    Pokemon = pokemon;
    
    GetComponent<UnityEngine.UI.Image>().sprite = (isPlayerUnit) ? Pokemon.Base.BackSprite : Pokemon.Base.FrontSprite;
    
    gameObject.SetActive(true);                     
}

    public void SetupFromInspector()
{
    PokemonBase selectedBase = GetSelectedBase();
    if (selectedBase == null) return;

    Pokemon = new Pokemon(selectedBase, Mathf.Max(1, level));
    
    Setup(Pokemon, isPlayerUnit); 
}

public void Setup(Pokemon pokemon, bool isPlayer)
{
    Pokemon = pokemon;
    isPlayerUnit = isPlayer; 

    pokemonImage = GetComponent<Image>();

    if (pokemonImage == null) return;

    Animator animator = GetComponent<Animator>();
    if (animator != null)
    {
        animator.enabled = false; 
    }
    

    if (isPlayerUnit)
    {
        pokemonImage.sprite = Pokemon.Base.BackSprite;
        Debug.Log($"Spelarens bild ändrades till: {pokemonImage.sprite.name}");
    }
    else
    {
        pokemonImage.sprite = Pokemon.Base.FrontSprite;
        Debug.Log($"Fiendens bild ändrades till: {pokemonImage.sprite.name}");
    }
}
    private PokemonBase GetSelectedBase()
    {
        return baseChoice == PokemonChoice.Infernape ? infernapeBase : empoleonBase;
    }
}
