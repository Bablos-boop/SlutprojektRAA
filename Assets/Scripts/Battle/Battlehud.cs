using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Battlehud : MonoBehaviour
{
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text LevelText;
    [SerializeField] Hpbar hpBar;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon; // Spara en referens till denna pokemon

        NameText.text = pokemon.Name;
        LevelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);
    }

    // Ber hpBar att köra sin jämna sänkning
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp);
    }
}