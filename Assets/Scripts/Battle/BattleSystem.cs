using System;
using System.Collections;
using System.Collections.Generic; // Lägger till stöd för listor
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

// Denna lilla klass gör att du får snygga rutor i Unity där du väljer Pokémon + Level
[System.Serializable]
public class PokemonSetup
{
    public PokemonBase pokemonBase;
    public int level = 5;
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] Battlehud playerHud;
    [SerializeField] Battlehud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    // NYTT: Här skapas listorna som syns i Unitys Inspector!
    [Header("Party Setup")]
    [SerializeField] List<PokemonSetup> playerPartySetup;
    [SerializeField] List<PokemonSetup> enemyPartySetup;

    // De faktiska lag-listorna som spelet använder under striden
    List<Pokemon> playerParty;
    List<Pokemon> enemyParty;

    BattleState state;
    int currentAction; // 0: Fight, 1: Bag, 2: Pokemon, 3: Run
    int currentMove;   // 0 till 3 för de fyra attackerna

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);

        // Bygg spelarens lag utifrån vad du valt i inspektören
        playerParty = new List<Pokemon>();
        foreach (var setup in playerPartySetup)
        {
            if (setup.pokemonBase != null)
                playerParty.Add(new Pokemon(setup.pokemonBase, Mathf.Max(1, setup.level)));
        }

        //  Bygg fiendens lag utifrån vad du valt i inspektören
        enemyParty = new List<Pokemon>();
        foreach (var setup in enemyPartySetup)
        {
            if (setup.pokemonBase != null)
                enemyParty.Add(new Pokemon(setup.pokemonBase, Mathf.Max(1, setup.level)));
        }

        // Hämta den första levande Pokémonen från vardera lag
        var firstPlayer = GetHealthyPlayerPokemon();
        var firstEnemy = GetHealthyEnemyPokemon();

        if (firstPlayer != null) playerUnit.Setup(firstPlayer, true);
        if (firstEnemy != null) enemyUnit.Setup(firstEnemy, false);
        
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    // Hjälpfunktioner för att hitta nästa friska Pokémon i listorna
    Pokemon GetHealthyPlayerPokemon()
    {
        foreach (var pokemon in playerParty)
        {
            if (pokemon.HP > 0) return pokemon;
        }
        return null;
    }

    Pokemon GetHealthyEnemyPokemon()
    {
        foreach (var pokemon in enemyParty)
        {
            if (pokemon.HP > 0) return pokemon;
        }
        return null;
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction; 
        
        dialogBox.EnableDialogText(true); 
        
        StartCoroutine(dialogBox.TypeDialog("Choose an Action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.UpdateActionSelection(currentAction);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogBox.enabled = enabled; 
    }

    void PlayerMoveSelection()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        
        dialogBox.EnableDialogText(false); 
        
        dialogBox.EnableMoveSelector(true);
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction) 
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

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

        dialogBox.UpdateActionSelection(currentAction); 

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction == 0) // FIGHT
            {
                PlayerMoveSelection();
            }
        }
    }

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

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerAction();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            dialogBox.EnableMoveSelector(false);
            StartCoroutine(PerformPlayerMove());
        }
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        var move = playerUnit.Pokemon.Moves[currentMove];
        
        dialogBox.EnableDialogText(true); 
        
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Name} used {move.Base.Name}!");
        yield return new WaitForSeconds(1f);

        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        
        yield return enemyHud.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Name} fainted!");
            yield return new WaitForSeconds(1f);

            //  Kolla om fienden har fler Pokémon i sin lista
            var nextPokemon = GetHealthyEnemyPokemon();
            if (nextPokemon != null)
            {
                enemyUnit.Setup(nextPokemon, false);
                enemyHud.SetData(nextPokemon);
                yield return dialogBox.TypeDialog($"Enemy sent out {nextPokemon.Name}!");
                yield return new WaitForSeconds(1f);
                PlayerAction(); 
            }
            else
            {
                yield return dialogBox.TypeDialog("You won the battle!");
            }
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        int randomIndex = UnityEngine.Random.Range(0, enemyUnit.Pokemon.Moves.Count);
        var move = enemyUnit.Pokemon.Moves[randomIndex];

        yield return dialogBox.TypeDialog($"Wild {enemyUnit.Pokemon.Name} used {move.Base.Name}!");
        yield return new WaitForSeconds(1f);

        bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        
        yield return playerHud.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Name} fainted!");
            yield return new WaitForSeconds(1f);

            //  Kolla om du har fler Pokémon i din lista
            var nextPokemon = GetHealthyPlayerPokemon();
            if (nextPokemon != null)
            {
                playerUnit.Setup(nextPokemon, true);
                playerHud.SetData(nextPokemon);
                dialogBox.SetMoveNames(nextPokemon.Moves); // Uppdaterar attack-knapparna till den nya gubben
                yield return dialogBox.TypeDialog($"Go {nextPokemon.Name}!");
                yield return new WaitForSeconds(1f);
                PlayerAction();
            }
            else
            {
                yield return dialogBox.TypeDialog("You lost the battle...");
            }
        }
        else
        {
            PlayerAction();
        }
    }
}