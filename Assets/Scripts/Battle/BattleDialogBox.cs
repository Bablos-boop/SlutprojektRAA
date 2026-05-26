using UnityEngine.UI;
using TMPro; // Krävs för att koden ska kunna styra TextMeshPro-komponenter
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;   // Hur snabbt texten skrivs ut (bokstäver per sekund)
    [SerializeField] Color highlightedColor; // Färgen som texten får när man markerar ett menyval

    [SerializeField] TMP_Text dialogText;      // Själva textrutan där spelet pratar med dig (t.ex. "Pikachu fainted!")
    [SerializeField] GameObject actionSelector; // Hela meny-rutan för valen (Fight, Bag, Pokemon, Run)
    [SerializeField] GameObject moveSelector;   // Hela meny-rutan för attackerna
    [SerializeField] GameObject moveDetails;    // Den lilla rutan bredvid attackerna som visar PP och Typ

    // Listor med textobjekt som vi drar in via Unity-inspektören
    [SerializeField] List<TMP_Text> moveTexts;   // De fyra textfälten för attackernas namn
    [SerializeField] List<TMP_Text> actionTexts; // Textfälten för Fight, Bag, Pokemon, Run
    [SerializeField] List<TMP_Text> typeText;    // Textfälten inuti detaljrutan (0 för PP, 1 för Element-typ)

    // Skriver ut namnen på de attacker din Pokémon faktiskt har i sina fyra rutor
    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            // Om Pokémonen har en attack på den här indexplatsen, skriv ut dess namn
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            // Om platsen är tom (t.ex. gubben bara kan 2 attacker), skriv ut ett streck
            else
                moveTexts[i].text = "-";
        }
    }

    // Hanterar färgmarkeringen i attackmenyn och uppdaterar PP samt Typ-detaljerna till höger
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            // Den valda attacken får markeringsfärgen, de andra blir svarta
            if (i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }

        // Om en giltig attack är vald, uppdatera informationen i detaljrutan
        if (move != null)
        {
            // typeText[0] visar nuvarande PP kvar dividerat på max PP (t.ex. "PP: 14/15")
            if (typeText.Count >= 1) typeText[0].text = $"PP: {move.PP}/{move.Base.PP}";
            // typeText[1] visar element-typen (t.ex. "Fire" eller "Water")
            if (typeText.Count >= 2) typeText[1].text = move.Base.Type.ToString();
        }
    }

    // Slänger upp en hel textsträng i rutan på ett bråkdel av en sekund (utan animation)
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

    // En Coroutine som skriver ut texten bokstav för bokstav så det ser ut som i de klassiska spelen
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = ""; // Tömmer rutan först

        // Går igenom varje enskild bokstav i texten
        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter; // Lägger till bokstaven i rutan
            yield return new WaitForSeconds(1f / lettersPerSecond); // Väntar en kort stund innan nästa bokstav tas
        }
    }

    // Slår på eller av den stora dialogtexten
    public void EnableDialogText(bool enabled) 
    {
        dialogText.enabled = enabled;
    }

    // Slår på eller av huvudmenyn (Fight, Bag, Pokemon, Run)
    public void EnableActionSelector(bool enabled) 
    {
        actionSelector.SetActive(enabled);
    }

    // Slår på eller av attackmenyn och dess tillhörande detaljruta samtidigt
    public void EnableMoveSelector(bool enabled) 
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    // Hanterar färgmarkeringen i huvudmenyn (Fight, Bag, Pokemon, Run)
    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            // Det menyval du hovrar över får markeringsfärgen, de andra blir svarta
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