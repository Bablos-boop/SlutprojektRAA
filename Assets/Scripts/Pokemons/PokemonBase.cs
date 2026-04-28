using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(fileName="Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    // Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int spAttack;
    [SerializeField] int spDefence;
    [SerializeField] int speed;

}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Fighting,
    Water,
    Steel,
    Electric,
    Fairy,
    Grass,
    Ground,
    Ghost,
    Dark,
    Flying,
    Poison,
    Bug,
    Psychic,
    Rock,
    Dragon,
    Ice,
}
