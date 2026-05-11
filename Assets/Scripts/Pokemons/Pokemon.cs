using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokemon 
{
   
   public PokemonBase Base {get; set;}
   public int Level {get; set;}
   public int HP { get; set; }
   public string Nickname { get; set; }

   public string Name {
       get { return Nickname ?? Base.Name; }
   }

   public Pokemon(PokemonBase pBase, int pLevel)
   {
    Base = pBase;
    Level = pLevel;
        HP = MaxHp;
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

   public void SetHP(int hp)
   {
       HP = Mathf.Clamp(hp, 0, MaxHp);
   }
}
