using UnityEngine;

public class Pokemon 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   PokemonBase _base;
   int level;

   public Pokemon(PokemonBase pBase, int pLevel)
   {
    _base = pBase;
    level = pLevel;
   }

   public int Attack {
    get { return Mathf.FloorToInt(_base.Attack * level / 100f) + 5; }
   }

   public int MaxHp {
    get { return Mathf.FloorToInt(_base.MaxHp * level / 100f) + 10; }
   }

   public int Defence {
    get { return Mathf.FloorToInt(_base.Defence * level / 100f) + 5; }
   }

   public int SpAttack {
    get { return Mathf.FloorToInt(_base.SpAttack * level / 100f) + 5; }
   }

   public int SpDefence {
    get { return Mathf.FloorToInt(_base.SpDefence * level / 100f) + 5; }
   }

   public int Speed {
    get { return Mathf.FloorToInt(_base.Speed * level / 100f) + 5; }
   }
}
