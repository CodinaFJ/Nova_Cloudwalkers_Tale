using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockDisabler : MonoBehaviour
{
    [SerializeField] Image lockImage;
    [SerializeField] GameObject counter;
    [SerializeField] bool lockEnabled = true;

    void Update()
    {
        lockImage.enabled = lockEnabled;
        counter.SetActive(lockEnabled);
    }
}
