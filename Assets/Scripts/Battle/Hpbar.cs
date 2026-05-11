using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] Slider hpSlider;

    public void SetHP(float hpNormalized)
    {
        hpSlider.value = hpNormalized;
    }
}
