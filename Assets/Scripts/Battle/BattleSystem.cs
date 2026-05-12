using System;
using System.Collections;
using UnityEngine;
public enum BattleState{Start, PlayerAction, PlayerMove, EnemyMove, Busy
}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] Battlehud playerHud;
    [SerializeField] Battlehud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    private void Start()
    {
        Debug.Log($"BattleSystem.Start: dialogBox={dialogBox}, playerUnit={playerUnit}, enemyUnit={enemyUnit}");
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.SetupFromInspector();
        enemyUnit.SetupFromInspector();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        yield return dialogBox.TypeDialog($"A wild {playerUnit.Pokemon.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();

    
    }
    void PlayerAction()
    {
        state = BattleState.PlayerAction; 
        StartCoroutine(dialogBox.TypeDialog("Choose an Action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.UpdateActionSelection(currentAction);
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction) 
        {
            HandleActionSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
            ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            --currentAction;
        }
        dialogBox.UpdateActionSelection(currentAction); 
    }
}
