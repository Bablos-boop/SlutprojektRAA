using System;
using System.Collections;
using System.Collections.Generic; // Lägger till stöd för listor (så man kan ha party-lag)
using UnityEngine;

// Håller koll på vems tur det är eller vad som händer i striden just nu
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

// En behållare för Unitys Inspector så du kan para ihop en Pokémon-bas med en level direkt i editorn
[System.Serializable]
public class PokemonSetup
{
    public PokemonBase pokemonBase;
    public int level = 5;
}

public class BattleSystem : MonoBehaviour
{
    // Referenser till alla grafik- och gränssnittskomponenter i stridsscenen
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] Battlehud playerHud;
    [SerializeField] Battlehud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    // Listorna som syns i Unitys Inspector där du bygger lagen
    [Header("Party Setup")]
    [SerializeField] List<PokemonSetup> playerPartySetup;
    [SerializeField] List<PokemonSetup> enemyPartySetup;

    // De faktiska lag-listorna som spelet använder och modifierar under stridens gång
    List<Pokemon> playerParty;
    List<Pokemon> enemyParty;

    BattleState state; // Den nuvarande statusen i striden
    int currentAction; // Håller koll på menyvalet (0: Fight, 1: Bag, 2: Pokemon, 3: Run)
    int currentMove;   // Håller koll på vilken av de 4 attackerna du hovrar över

    private void Start()
    {
        // Startar striden direkt när skriptet vaknar till liv
        StartCoroutine(SetupBattle());
    }

    // Sätter upp striden, skapar lagen och skickar ut de första levande gubbarna
    public IEnumerator SetupBattle()
    {
        // Gömmer alla meny-väljare under laddningen
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);

        // Skapar och fyller spelarens lag baserat på dina val i Unitys Inspector
        playerParty = new List<Pokemon>();
        foreach (var setup in playerPartySetup)
        {
            if (setup.pokemonBase != null)
                playerParty.Add(new Pokemon(setup.pokemonBase, Mathf.Max(1, setup.level)));
        }

        // Skapar och fyller fiendens lag baserat på dina val i Unitys Inspector
        enemyParty = new List<Pokemon>();
        foreach (var setup in enemyPartySetup)
        {
            if (setup.pokemonBase != null)
                enemyParty.Add(new Pokemon(setup.pokemonBase, Mathf.Max(1, setup.level)));
        }

        // Letar upp den första Pokémonen i varje lag som har mer än 0 HP
        var firstPlayer = GetHealthyPlayerPokemon();
        var firstEnemy = GetHealthyEnemyPokemon();

        // Skickar in gubbarna i sina stridsrutor på skärmen
        if (firstPlayer != null) playerUnit.Setup(firstPlayer, true);
        if (firstEnemy != null) enemyUnit.Setup(firstEnemy, false);
        
        // Uppdaterar grafikmätarna (HP-bars, namn, level) till de aktuella gubbarna
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        // Laddar in den aktiva spelar-Pokémonens attacker i dialogrutan
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        if (firstEnemy != null) enemyUnit.PlayCry(); // Fienden ryter först
        yield return new WaitForSeconds(0.5f);       // Kort paus så ljuden inte krockar


        if (firstPlayer != null) playerUnit.PlayCry(); // Sen ryter din
        // Skriver ut introduktionstexten och väntar 1 sekund
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        // Ger kontrollen till spelaren
        PlayerAction();
    }

    // Loopar igenom spelarens lag och returnerar den första som lever (HP > 0)
    Pokemon GetHealthyPlayerPokemon()
    {
        foreach (var pokemon in playerParty)
        {
            if (pokemon.HP > 0) return pokemon;
        }
        return null; // Returnerar null om alla i laget har svimmat
    }

    // Loopar igenom fiendens lag och returnerar den första som lever (HP > 0)
    Pokemon GetHealthyEnemyPokemon()
    {
        foreach (var pokemon in enemyParty)
        {
            if (pokemon.HP > 0) return pokemon;
        }
        return null; // Returnerar null om hela fiendens lag har svimmat
    }

    // Startar spelarens tur där man får välja om man vill slåss, fly etc.
    void PlayerAction()
    {
        state = BattleState.PlayerAction; 
        
        dialogBox.EnableDialogText(true); 
        
        StartCoroutine(dialogBox.TypeDialog("Choose an Action"));
        dialogBox.EnableActionSelector(true); // Aktiverar rutan för Fight/Bag/Pokemon/Run
        dialogBox.EnableMoveSelector(false);
        dialogBox.UpdateActionSelection(currentAction); // Rör markören till rätt ställe
    }

    // Hjälpfunktion för att slå på/av textkomponenten i dialogrutan
    public void EnableDialogText(bool enabled)
    {
        dialogBox.enabled = enabled; 
    }

    // Öppnar attackmenyn när spelaren har tryckt på "FIGHT"
    void PlayerMoveSelection()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false); 
        dialogBox.EnableMoveSelector(true); // Aktiverar de fyra attackknapparna
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);
    }

    private void Update()
    {
        // Lyssnar på knapptryck baserat på om du är i huvudmenyn eller attackmenyn
        if (state == BattleState.PlayerAction) 
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    // Hanterar navigeringen (piltangenterna) i huvudmenyn (Fight, Bag, Pokemon, Run)
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 2) currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction >= 2) currentAction -= 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction == 0 || currentAction == 2) currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction == 1 || currentAction == 3) currentAction--;
        }

        // Uppdaterar den visuella markören i Unity
        dialogBox.UpdateActionSelection(currentAction); 

        // Om spelaren trycker på Enter/Z och står på FIGHT (0), öppna attackmenyn
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == 0) 
            {
                PlayerMoveSelection();
            }
        }
    }

    // Hanterar navigeringen (piltangenterna) bland de 4 attackerna
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < 2) currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove >= 2) currentMove -= 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove == 0 || currentMove == 2) currentMove++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove == 1 || currentMove == 3) currentMove--;
        }

        // Uppdaterar vilken attack som lyser i rutan
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        // Om man trycker på X går man bakåt till huvudmenyn
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerAction();
        }

        // Om man trycker på Enter/Z låser vi valet och utför attacken
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            dialogBox.EnableMoveSelector(false);
            StartCoroutine(RunTurns()); // FUNKTION SOM HANTERAR HASTIGHET
        }
    }
IEnumerator RunTurns()
{
    state = BattleState.Busy;

    // 1. Ta reda på vilken attack spelaren valde
    var playerMove = playerUnit.Pokemon.Moves[currentMove];
    
    // 2. Låt fienden välja en slumpmässig attack 
    int randomIndex = UnityEngine.Random.Range(0, enemyUnit.Pokemon.Moves.Count);
    var enemyMove = enemyUnit.Pokemon.Moves[randomIndex];

    // 3. Kolla vem som är snabbast
    bool playerGoesFirst = true;

    if (playerUnit.Pokemon.Speed < enemyUnit.Pokemon.Speed)
    {
        playerGoesFirst = false;
    }
    else if (playerUnit.Pokemon.Speed == enemyUnit.Pokemon.Speed)
    {
        // Om de är exakt lika snabba, singla slant (50/50 chans)
        playerGoesFirst = (UnityEngine.Random.Range(0, 2) == 0);
    }

    // 4. Kör attackerna i rätt ordning
    if (playerGoesFirst)
    {
        // Spelaren är snabbast
        yield return RunMove(playerUnit, enemyUnit, playerMove, enemyHud);
        
        // FIX: Om fienden svimmade (statusen är inte Busy längre). avbruten
        if (state != BattleState.Busy) yield break; 

        // Annars slår fienden nu
        yield return RunMove(enemyUnit, playerUnit, enemyMove, playerHud);
    }
    else
    {
        // Fienden är snabbare
        yield return RunMove(enemyUnit, playerUnit, enemyMove, playerHud);
        
        // FIX: Om spelaren svimmade (statusen är inte Busy längre), avbryt turen direkt!
        if (state != BattleState.Busy) yield break;

        // Annars slår spelaren nu
        yield return RunMove(playerUnit, enemyUnit, playerMove, enemyHud);
    }

    // 5. Om båda överlevde rundan (statusen är fortfarande Busy), ge tillbaka kontrollen till spelaren
    if (state == BattleState.Busy)
    {
        PlayerAction();
    }
}
IEnumerator RunMove(BattleUnit attacker, BattleUnit defender, Move move, Battlehud defenderHud)
{
    dialogBox.EnableDialogText(true); 
    
    yield return dialogBox.TypeDialog($"{attacker.Pokemon.Name} used {move.Base.Name}!");
    yield return new WaitForSeconds(1f);

    // Gör skada på försvararen
    bool isFainted = defender.Pokemon.TakeDamage(move, attacker.Pokemon);
    yield return defenderHud.UpdateHP();

    if (isFainted)
    {
        yield return dialogBox.TypeDialog($"{defender.Pokemon.Name} fainted!");
        yield return new WaitForSeconds(1f);

        // Kolla vem det var som svimmade genom att se om det var spelaren eller fienden
        if (defender == enemyUnit)
        {
            // Fienden svimmade -> Leta efter nästa friska fiende
            var nextPokemon = GetHealthyEnemyPokemon();
            if (nextPokemon != null)
            {
                enemyUnit.Setup(nextPokemon, false);
                enemyHud.SetData(nextPokemon);
                enemyUnit.PlayCry();
                yield return dialogBox.TypeDialog($"Enemy sent out {nextPokemon.Name}!");
                yield return new WaitForSeconds(1f);
                PlayerAction();
            }
            else
            {
                yield return dialogBox.TypeDialog("You won the battle!");
                state = BattleState.Start; // Sätter till start för att markera att striden är helt slut
            }
        }
        else
        {
            // Spelaren svimmade -> Leta efter nästa friska spelar-Pokémon
            var nextPokemon = GetHealthyPlayerPokemon();
            if (nextPokemon != null)
            {
                playerUnit.Setup(nextPokemon, true);
                playerHud.SetData(nextPokemon);
                dialogBox.SetMoveNames(nextPokemon.Moves);
                playerUnit.PlayCry();
                yield return dialogBox.TypeDialog($"Go {nextPokemon.Name}!");
                yield return new WaitForSeconds(1f);
                PlayerAction();
            }
            else
            {
                yield return dialogBox.TypeDialog("You lost the battle...");
                state = BattleState.Start; // Sätter till start för att markera att striden är helt slut
            }
        }
    }
}
    
}