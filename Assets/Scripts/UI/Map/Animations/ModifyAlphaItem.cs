using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Legacy. I don't even know why I had this
/// </summary>
public class ModifyAlphaItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField]
    private float alpha = 1;
    public float Alpha { 
        get => alpha; 
        set
        {
            if (value == 0 || value == 1)
                alpha = value;
            else
                Debug.LogWarning(("ModifyAlphas: Wrong alphas"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        color.a = alpha; 
        image.color = color;
    }
}
