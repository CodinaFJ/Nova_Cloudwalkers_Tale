using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public TextMeshProUGUI display_Text;

    float elapsedTime = 0f;
 
    public void Update ()
    {
        float current = 0;
        elapsedTime += Time.deltaTime;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        string text = avgFrameRate.ToString() + " FPS";
        if(elapsedTime > 0.2f){
                display_Text.text = text;
                elapsedTime = 0f;
            } 
    }
}