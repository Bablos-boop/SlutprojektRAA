using UnityEngine;

public class Move
{
    public Movebase Base {  get;  set;   }
    public int PP {get; set;}

    public Move (Movebase pBase, int pp)
    {
        Base = pBase;
        PP = pp;
    }
}
