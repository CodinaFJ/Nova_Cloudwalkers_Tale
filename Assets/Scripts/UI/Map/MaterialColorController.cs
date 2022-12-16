using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialColorController : MonoBehaviour
{
    [SerializeField]
    float color_r;
    [SerializeField]
    float color_g;
    [SerializeField]
    float color_b;
    [SerializeField]
    float color_a;

    Material myMaterial;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.SetColor("Color_e7fe3bb708f846cd94141b3c465d79cc", new Color(color_r, color_g, color_b, color_a));
    }
}
