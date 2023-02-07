using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This was just done because I was trying to not block properties with animator.
/// TODO: remove this script and replace by functionality in code animations
/// </summary>
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
