using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Den här magiska raden gör att du kan högerklicka i Unity-mappen och skapa en ny Pokémon-fil!
[CreateAssetMenu(fileName="Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject 
{
    // ScriptableObject betyder bara: "Det här är en fil i Unity som sparar data, den har inga ben och kan inte gå".

    // [SerializeField] betyder: "Gör så att den här rutan syns i Unity så jag kan skriva i den".
    // string betyder: "Här ska det stå TEXT".
    [SerializeField] string pokemonName; // Här skriver du namnet (typ Pikachu).

    [TextArea] // Gör rutan i Unity jättestor så man kan skriva en hel uppsats om gubben.
    [SerializeField] string description; // Pokémon-beskrivningen (Pokedex-texten).

    [SerializeField] RuntimeAnimatorController frontAnimator; // Animationen när man möter gubben.
    [SerializeField] RuntimeAnimatorController backAnimator;  // Animationen när man ser gubben bakifrån (din egen gubbe).

    // De här pilarna (=>) betyder bara: "Andra skript får KOLLA på animationen, men de får absolut inte ändra den! Pilla inte!"
    public RuntimeAnimatorController FrontAnimator => frontAnimator;
    public RuntimeAnimatorController BackAnimator => backAnimator;

    [SerializeField] Sprite frontSprite; // Bilden på gubben framifrån (om du inte kör animationer).
    [SerializeField] Sprite backSprite;  // Bilden på gubben bakifrån (rumpan).
    [SerializeField] AudioClip cry;      // Ljudfilen för gubbens cry
    [SerializeField] PokemonType type1;  // Element-typ 1 (typ Eld, Vatten, Gräs).
    [SerializeField] PokemonType type2;  // Element-typ 2 (om den är både Eld och Flygande. Annars "None").

    // "Get" betyder: "Om ett annat skript frågar efter bilden, ge dem den, men låt dem inte förstöra originalet".
    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public AudioClip Cry
    {
        get { return cry; }
    }

    // "int" betyder HELTAL. Inga halva liv eller 1.5 i attack här inte!
    [SerializeField] int maxHp;      // Hur mycket stryk gubben tål innan den svimmar.
    [SerializeField] int attack;     // Hur hårt den slår med vanliga attacker.
    [SerializeField] int defence;    // Hur mycket den blockar vanliga attacker.
    [SerializeField] int spAttack;   // Hur hårt den skjuter med magi/special-attacker.
    [SerializeField] int spDefence;  // Hur bra den skyddar sig mot magi/special-attacker.
    [SerializeField] int speed;      // Vem som är snabbast och får slå först i striden.

    // En lista som innehåller par av "Attack + Level". Alla attacker gubben kan lära sig
    [SerializeField] List<LearnableMove> learnableMoves;

    // funktioner för gubbens stats:
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

// [System.Serializable] tvingar Unity att visa den här lilla miniklassen i Inspektören, annars gömmer Unity den.
[System.Serializable]
public class LearnableMove
{
    // Den här klassen är bara till för att para ihop en attack med en specifik level.
    [SerializeField] Movebase moveBase; // Själva attacken 
    [SerializeField] int level;         // Vilken level gubben måste nå för att lära sig den.

    public Movebase Base
    {
        get { return moveBase;}
    }

    public int Level
    {
        get { return level;}
    }
}

// En "enum" är bara en glorifierad rullgardinsmeny. 
// Istället för att skriva texten "Fire" och råka stava fel, väljer vi bara från den här listan i Unity.
public enum PokemonType
{
    None, Normal, Fire, Fighting, Water, Steel, Electric, Fairy, Grass, Ground, Ghost, Dark, Flying, Poison, Bug, Psychic, Rock, Dragon, Ice,
}