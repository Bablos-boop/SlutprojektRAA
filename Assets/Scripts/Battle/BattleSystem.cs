using System;
using System.Collections;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] Battlehud playerHud;
    [SerializeField] Battlehud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

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

        playerUnit.SetupFromInspector();
        enemyUnit.SetupFromInspector();
        
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
{
    state = BattleState.PlayerAction; 
    
    // Sätt på huvudtexten igen när vi går tillbaka
    dialogBox.EnableDialogText(true); 
    
    StartCoroutine(dialogBox.TypeDialog("Choose an Action"));
    dialogBox.EnableActionSelector(true);
    dialogBox.EnableMoveSelector(false);
    dialogBox.UpdateActionSelection(currentAction);
}


public void EnableDialogText(bool enabled)
{
    dialogBox.enabled = enabled; // 
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

    // Navigering i (Fight, Bag, Pokemon, Run)
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

    // Navigering bland de 4 attackerna 
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

        // Gå tillbaka till huvudmenyn med X
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerAction();
        }

        // Välj attack med Z / Enter
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            dialogBox.EnableMoveSelector(false);
            StartCoroutine(PerformPlayerMove());
        }
    }

    // UTFÖR SPELARENS ATTACK
    // UTFÖR SPELARENS ATTACK
IEnumerator PerformPlayerMove()
{
    state = BattleState.Busy;
    var move = playerUnit.Pokemon.Moves[currentMove];
    
    // NYTT: Sätt på dialogtexten igen så att attack-meddelandet faktiskt syns!
    dialogBox.EnableDialogText(true); 
    
    yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Name} used {move.Base.Name}!");
    yield return new WaitForSeconds(1f);

    // Fienden tar skada
    bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
    
    // Vänta här tills fiendens HP-bar har glidit ner klart på skärmen
    yield return enemyHud.UpdateHP();

    if (isFainted)
    {
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Name} fainted!");
    }
    else
    {
        StartCoroutine(EnemyMove());
    }
}
    // FIENDENS TUR (Slumpmässig attack)
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        int randomIndex = UnityEngine.Random.Range(0, enemyUnit.Pokemon.Moves.Count);
        var move = enemyUnit.Pokemon.Moves[randomIndex];

        yield return dialogBox.TypeDialog($"Wild {enemyUnit.Pokemon.Name} used {move.Base.Name}!");
        yield return new WaitForSeconds(1f);

        // Spelaren tar skada
        bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        
        // NYTT: Vänta här tills spelarens HP-bar har glidit ner klart på skärmen!
        yield return playerHud.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Name} fainted!");
        }
        else
        {
            PlayerAction();
        }
    }
}