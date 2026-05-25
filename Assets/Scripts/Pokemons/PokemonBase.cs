using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(fileName="Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string pokemonName;

    [TextArea]
    [SerializeField] string description;
    [SerializeField] RuntimeAnimatorController frontAnimator;
    [SerializeField] RuntimeAnimatorController backAnimator;

    public RuntimeAnimatorController FrontAnimator => frontAnimator;
    public RuntimeAnimatorController BackAnimator => backAnimator;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    // Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int spAttack;
    [SerializeField] int spDefence;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

  
public string Name  
    {
        get { return pokemonName;}
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

    public List<LearnableMove> LearnableMoves
    {
        get {return learnableMoves;}
    }
}

[System.Serializable]

public class LearnableMove
{
    [SerializeField] Movebase moveBase;
    [SerializeField] int level;

    public Movebase Base
    {
        get { return moveBase;}
    }

    public int Level
    {
        get { return level;}
    }
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
