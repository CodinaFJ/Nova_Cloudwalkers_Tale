using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorAnimations : MonoBehaviour
{
    public static LevelSelectorAnimations instance;

    [Header("World Selection")]
    [SerializeField] float[] worldButtonSelectTimes = new float[3];
    [SerializeField] float[] worldButtonHideTimes = new float[3];
    [SerializeField] float[] worldButtonShowTimes = new float[3];

    private float objectAlpha;

    private void Awake() 
    {
        instance = this;
    }

    /**************************************************************************************************
    World buttons
    **************************************************************************************************/
    /// <summary>
    /// Start animations when world is selected
    /// </summary>
    /// <param name="go"> Game Object </param>
    public void PlayWorldButtonSelectAnimation(GameObject go)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformSelectAnimation(go));
        //TODO: Activate animator to fade out gameObject.
    }

    public void PlayWorldButtonHideAnimation(GameObject go, GameObject selectedWorldGO)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformHideAnimation(go, selectedWorldGO));
        //TODO: Activate animator to fade out gameObject.
    }

    public void PlayWorldButtonShowAnimation(GameObject go)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformShowAnimation(go));
        //TODO: Activate animator to fade in gameObject.
    }

    /**************************************************************************************************
    Animations Definitions
    **************************************************************************************************/
    /// <summary>
    /// World button animation when selected: moves to center and scales up
    /// </summary>
    /// <param name="go"> Game Object </param>
    public IEnumerator WorldButtonTransformSelectAnimation(GameObject go)
    {
        yield return new WaitForSeconds(worldButtonSelectTimes[0]);
        LeanTween.move(go, Vector2.zero, worldButtonSelectTimes[1]);
        LeanTween.scale(go, new Vector2(1.05f, 1.05f), worldButtonSelectTimes[1]);
        yield return new WaitForSeconds(worldButtonSelectTimes[1]);
        LeanTween.scale(go, new Vector2(1.50f, 1.50f), worldButtonSelectTimes[2]);
    }

    /// <summary>
    /// World button animation when other world is selected: moves away and fades out
    /// </summary>
    /// <param name="go"> Game Object </param>
    /// <param name="selectedWorldGO"> Other selected world game object </param>
    public IEnumerator WorldButtonTransformHideAnimation(GameObject go, GameObject selectedWorldGO)
    {
        // TODO: Animation times
        Vector2 finalPos = go.transform.position + (go.transform.position - selectedWorldGO.transform.position);
        yield return new WaitForSeconds(worldButtonHideTimes[0]);
        LeanTween.move(go, finalPos, worldButtonHideTimes[1]);
        yield return new WaitForSeconds(worldButtonHideTimes[1]);
    }

    /// <summary>
    /// World button animation when showing: scales up & moves from center to position.
    /// </summary>
    /// <param name="go"> Game Object </param>
    public IEnumerator WorldButtonTransformShowAnimation(GameObject go)
    {
        //TODO: Correctly define animation.
        //! This is just a copy paste from select animation.
        yield return new WaitForSeconds(worldButtonShowTimes[0]);
        LeanTween.move(go, Vector2.zero, worldButtonShowTimes[1]);
        LeanTween.scale(go, new Vector2(1.05f, 1.05f), worldButtonShowTimes[1]);
        yield return new WaitForSeconds(worldButtonShowTimes[1]);
        LeanTween.scale(go, new Vector2(1.50f, 1.50f), worldButtonShowTimes[2]);
    }
}
