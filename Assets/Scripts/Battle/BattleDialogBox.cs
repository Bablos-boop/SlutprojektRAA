using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor; // ← FIXED: added semicolon

    [SerializeField] TMP_Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<TMP_Text> moveTexts;
    [SerializeField] List<TMP_Text> actionTexts;
    [SerializeField] List<TMP_Text> typeText;

    public void SetDialog(string dialog)
    {
        if (dialogText == null)
        {
            Debug.LogError("BattleDialogBox: dialogText is not assigned in the inspector.");
            return;
        }

        dialogText.text = dialog;
        Debug.Log($"BattleDialogBox.SetDialog: dialogText={dialogText}, dialog=\"{dialog}\"");
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";

        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    public void EnableDialogText(bool enabled) 
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled) 
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled) 
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

   
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    }
}