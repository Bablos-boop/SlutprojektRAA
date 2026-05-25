using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] // NYTT: Låter Unity hantera listor av Pokémon
public class Pokemon 
{
    // Vi gör basen och leveln sparade så vi kan lägga till dem i listor
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base { get { return _base; } set { _base = value; } }
    public int Level { get { return level; } set { level = value; } }
    
    public int HP { get; set; }
    public string Nickname { get; set; }

    public List<Move> Moves { get; set; }

    public string Name {
        get { return Nickname ?? Base.Name; }
    }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        Init(); // Kallar på nya Init-funktionen
    }

    //  Denna förbereder en Pokémon (används av Party-systemet)
    public void Init()
    {
        HP = MaxHp;
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base, move.Base.PP));

            if (Moves.Count >= 4)
                break;
        }
    }

    public int Attack {
        get { return Mathf.FloorToInt(Base.Attack * Level / 100f) + 5; }
    }

    public int MaxHp {
        get { return Mathf.FloorToInt(Base.MaxHp * Level / 100f) + 10; }
    }

    public int Defence {
        get { return Mathf.FloorToInt(Base.Defence * Level / 100f) + 5; }
    }

    public int SpAttack {
        get { return Mathf.FloorToInt(Base.SpAttack * Level / 100f) + 5; }
    }

    public int SpDefence {
        get { return Mathf.FloorToInt(Base.SpDefence * Level / 100f) + 5; }
    }

    public int Speed {
        get { return Mathf.FloorToInt(Base.Speed * Level / 100f) + 5; }
    }

    public bool TakeDamage(Move move, Pokemon attacker)
    {
        float modifiers = Random.Range(0.85f, 1.0f);
        float baseDamage = (2 * attacker.Level / 5f + 2) * move.Base.Power * ((float)attacker.Attack / Defence) / 50f + 2;
        int damage = Mathf.FloorToInt(baseDamage * modifiers);

        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        
        return HP <= 0; 
    }

    public void SetHP(int hp)
    {
        HP = Mathf.Clamp(hp, 0, MaxHp);
    }
}