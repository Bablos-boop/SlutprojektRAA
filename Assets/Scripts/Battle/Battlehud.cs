using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battlehud : MonoBehaviour
{
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text LevelText;
    [SerializeField] Hpbar hpBar;

    public void SetData(Pokemon pokemon)
    {
        NameText.text = pokemon.Base.Name;
        LevelText.text = "Lvl " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);
    }
}

