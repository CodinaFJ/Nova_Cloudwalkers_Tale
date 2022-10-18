using UnityEngine;
using TMPro;
using System;
 
public class PjVelocityDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display_Text;

    private Vector3 pjPos_0 = Vector3.zero;
    private Vector3 pjPos_1 = Vector3.zero;

    float elapsedTime = 0f;
 
    private void Update ()
    {
        elapsedTime += Time.deltaTime;
        
        if(PlayerBehavior.instance != null){
            string text = "Player Speed: " + CalculatePlayerSpeed();
            if(elapsedTime > 0.2f){
                display_Text.text = text;
                elapsedTime = 0f;
            } 
        }
        else{
            display_Text.text = "No PJ in scene";
        }
    }

    private string CalculatePlayerSpeed(){
        float speed;
        try{
            pjPos_1 = PlayerBehavior.instance.transform.position;
            speed = Vector3.Magnitude(pjPos_1 - pjPos_0)/Time.deltaTime;
            pjPos_0 = pjPos_1;
            return speed.ToString();
        }catch(Exception ex){
            return ex.Message;
        }
    }
}