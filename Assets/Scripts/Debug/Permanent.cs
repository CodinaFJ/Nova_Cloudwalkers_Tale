using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Permanent : MonoBehaviour
{
    private void Start(){
        DontDestroyOnLoad(gameObject);
    }
}
