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

  
public string Name  
    {
        get { return  name;}
    }

public string Description
    {
        get {return description;}
    }

public int MaxHp
    {
        get { return maxHp; }
    }

public int Attack
    {
        get { return attack; }
    }

public int Defence
    {
        get { return defence; }
    }

public int SpAttack
    {
        get { return spAttack; }
    }

public int SpDefence
    {
        get { return spDefence; }
    }

public int Speed
    {
        get { return speed; }
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
}
