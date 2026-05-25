using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokemon 
{
   public PokemonBase Base {get; set;}
   public int Level {get; set;}
   public int HP { get; set; }
   public string Nickname { get; set; }

   // Denna lista håller koll på de 4 attacker som denna specifika Pokémon kan nu
   public List<Move> Moves { get; set; }

   public string Name {
       get { return Nickname ?? Base.Name; }
   }

   public Pokemon(PokemonBase pBase, int pLevel)
   {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        // Generera attacker automatiskt baserat på Pokémonens level vid start
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base, move.Base.PP));

            if (Moves.Count >= 4)
                break; // Max 4 attacker
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

   // Funktion för att räkna ut skada och dra av HP
   public bool TakeDamage(Move move, Pokemon attacker)
   {
        // Skadeformel
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