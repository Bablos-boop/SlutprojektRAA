using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Tillåter Unity att spara klassen, visa den i listor och redigera inställningar i Inspektören
[System.Serializable] 
public class Pokemon 
{
    // Variabler för grunddatan från ScriptableObject och gubbens aktuella nivå
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    // Publika egenskaper (Properties) för att kunna läsa och ändra basen samt leveln från andra skript
    public PokemonBase Base { get { return _base; } set { _base = value; } }
    public int Level { get { return level; } set { level = value; } }
    
    // Aktuella värden som förändras dynamiskt under spelets eller stridens gång
    public int HP { get; set; }
    public string Nickname { get; set; }
    public List<Move> Moves { get; set; }

    // Hämtar smeknamnet om spelaren har gett gubben ett, annars används artnamnet från basfilen
    public string Name {
        get { return Nickname ?? Base.Name; }
    }

    // Konstruktor: Körs när en ny Pokémon skapas via kod (t.ex. när man bygger lag i BattleSystem)
    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        Init(); // Kör startfunktionen för att förbereda stats och attacker
    }

    // Initiering: Ger Pokémonen fullt HP och laddar dess första attacker baserat på nuvarande level
    public void Init()
    {
        HP = MaxHp; // Startar alltid med fullt liv
        Moves = new List<Move>(); // Skapar en tom lista för de aktiva attackerna
        
        // Går igenom listan på alla attacker som denna art överhuvudtaget kan lära sig
        foreach (var move in Base.LearnableMoves)
        {
            // Om attackens levelkrav är mindre än eller lika med Pokémonens nuvarande level, lägg till den
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base, move.Base.PP));

            // Spärr: En Pokémon får aldrig ha mer än 4 aktiva attacker samtidigt
            if (Moves.Count >= 4)
                break;
        }
    }

    // --- STATS-BERÄKNINGAR ---
    // Dessa matematiska formler räknar ut gubbens slutgiltiga styrka genom att skala upp grundvärdena med dess nuvarande level.

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

    // --- STRIDSLOGIK ---

    // Räknar ut inkommande skada, drar av HP och skickar tillbaka 'true' om Pokémonen svimmade (HP nådde 0)
    public bool TakeDamage(Move move, Pokemon attacker)
    {
        // En slumpmässig multiplikator mellan 85% och 100% så att attackerna gör lite olika skada varje gång
        float modifiers = Random.Range(0.85f, 1.0f);
        
        // Den officiella skadeformeln baserad på anfallarens level/attack, försvararens defence och attackens styrka
        float baseDamage = (2 * attacker.Level / 5f + 2) * move.Base.Power * ((float)attacker.Attack / Defence) / 50f + 2;
        int damage = Mathf.FloorToInt(baseDamage * modifiers);

        // Subtraherar skadan från HP, men tvingar värdet att hålla sig säkert mellan 0 och MaxHP
        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        
        return HP <= 0; // Returnerar true om Pokémonen dog/svimmade av attacken
    }

    // Sätter HP till ett specifikt exakt värde (t.ex. vid healing), men håller det inom säkra gränser (0 till MaxHP)
    public void SetHP(int hp)
    {
        HP = Mathf.Clamp(hp, 0, MaxHp);
    }
}