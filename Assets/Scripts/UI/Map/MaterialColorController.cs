using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Make accesible the material color from animator.
/// </summary>
public class MaterialColorController : MonoBehaviour
{
    [SerializeField] float color_r;
    [SerializeField] float color_g;
    [SerializeField] float color_b;
    [SerializeField] float color_a;

    private Material myMaterial;

    void Start()
    {
        myMaterial = GetComponent<Image>().material;
    }

    void Update()
    {
        //TODO: Not very smart to have color string hardcoded. It would make this code exportable
        myMaterial.SetColor("Color_e7fe3bb708f846cd94141b3c465d79cc", new Color(color_r, color_g, color_b, color_a));
    }
}
