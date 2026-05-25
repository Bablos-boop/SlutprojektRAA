using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hpbar : MonoBehaviour
{
    [SerializeField] Slider hpSlider;

    public void SetHP(float hpNormalized)
    {
        hpSlider.value = hpNormalized;
    }

    // Använder MoveTowards för garanterad, jämn rörelse
    public IEnumerator SetHPSmooth(float newHpNormalized)
    {
        // Fortsätt loopa så länge avståndet mellan nuvarande värde och nya värdet är större än 0.01
        while (Mathf.Abs(hpSlider.value - newHpNormalized) > 0.01f)
        {
            // Flytta sliderns värde mot det nya värdet med en hastighet av 0.5 enheter per sekund
            hpSlider.value = Mathf.MoveTowards(hpSlider.value, newHpNormalized, Time.deltaTime * 0.5f);
            
            // Vänta till nästa frame innan loopen körs igen
            yield return null; 
        }
        
        // 
        hpSlider.value = newHpNormalized;
    }
}