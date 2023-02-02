using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorAnimations : MonoBehaviour
{
    public static LevelSelectorAnimations   instance;

    [Header("World Animation Times")]
    [SerializeField] float[]    worldButtonSelectTimes = new float[3];
    [SerializeField] float[]    worldButtonHideTimes = new float[2];
    [SerializeField] float[]    worldButtonShowTimes = new float[3];
    [SerializeField] float[]    worldButtonUnlockTimes = new float[3];
    [SerializeField] float[]    worldsContainerShowTimes = new float[2];

    [Header ("UI Animation Times")]
    [SerializeField] float[]    UIMenuButtonTimes;

    [Header("Easing curves")]
    [SerializeField] LeanTweenType  easeOpenWorldMoveCenter;
    [SerializeField] LeanTweenType  easeOpenWorldScaleUp;
    [SerializeField] LeanTweenType  easeWorldsContainerScaleDown;

    private float   objectAlpha;

    private void Awake() 
    {
        instance = this;
    }

    /**************************************************************************************************
    World buttons actions
    **************************************************************************************************/
    /// <summary>
    /// Start world opening animations when world is selected
    /// </summary>
    /// <param name="go"> Game Object </param>
    public void PlayWorldButtonSelectAnimation(GameObject go)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformSelectAnimation(go));
    }

    /// <summary>
    /// Start hiding animations when world is selected
    /// </summary>
    /// <param name="go"> Game Object to hide </param>
    /// <param name="selectedWorldGO"> Selected world Game Object </param>
    public void PlayWorldButtonHideAnimation(GameObject go, GameObject selectedWorldGO)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformHideAnimation(go, selectedWorldGO));
    }

    /// <summary>
    /// Start show animations when a world is closed
    /// </summary>
    /// <param name="go"> Game Object to show </param>
    public void PlayWorldButtonShowAnimation(GameObject go)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformShowAnimation(go));
    }

    /// <summary>
    /// Start unlock animations when a world is unlocked
    /// </summary>
    /// <param name="go"> Game Object to show </param>
    public void PlayWorldButtonUnlockAnimation(GameObject go)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldButtonTransformUnlockAnimation(go));
    }

    public void PlayWorldsContainerScaleDownAnimation(GameObject go)
    {
        LeanTween.cancel(go);
        StartCoroutine(WorldsContainerScaleDownAnimation(go));
    }

    /**************************************************************************************************
    Animations Definitions
    **************************************************************************************************/

    /***********************
    World buttons
    ***********************/
    /// <summary>
    /// World button animation when selected: moves to center and scales up
    /// </summary>
    /// <param name="go"> Game Object </param>
    private IEnumerator WorldButtonTransformSelectAnimation(GameObject go)
    {
        yield return new WaitForSeconds(worldButtonSelectTimes[0]);
        LeanTween.move(go, Vector2.zero, worldButtonSelectTimes[1]).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(go, new Vector2(1.05f, 1.05f), worldButtonSelectTimes[1]).setEase(LeanTweenType.easeInOutSine);
        yield return new WaitForSeconds(worldButtonSelectTimes[1]);
        LeanTween.scale(go, new Vector2(1.50f, 1.50f), worldButtonSelectTimes[2]).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(worldButtonSelectTimes[2]);
        go.transform.position = go.GetComponent<WorldSelectorAnimatedItem>().GetInitialPos();
        go.transform.localScale = go.GetComponent<WorldSelectorAnimatedItem>().GetInitialScale();
    }

    /// <summary>
    /// World button animation when other world is selected: moves away and fades out
    /// </summary>
    /// <param name="go"> Game Object </param>
    /// <param name="selectedWorldGO"> Other selected world game object </param>
    private IEnumerator WorldButtonTransformHideAnimation(GameObject go, GameObject selectedWorldGO)
    {
        Vector2 finalPos = go.transform.position + (go.transform.position - selectedWorldGO.transform.position);
        yield return new WaitForSeconds(worldButtonHideTimes[0]);
        LeanTween.move(go, finalPos, worldButtonHideTimes[1]);
        LeanTween.scale(go, new Vector2(0.6f, 0.6f), worldButtonHideTimes[1]);
        yield return new WaitForSeconds(worldButtonHideTimes[1]);
        go.transform.position = go.GetComponent<WorldSelectorAnimatedItem>().GetInitialPos();
        go.transform.localScale = go.GetComponent<WorldSelectorAnimatedItem>().GetInitialScale();
    }

    /// <summary>
    /// World button animation when showing: scales up & moves from center to position.
    /// </summary>
    /// <param name="go"> Game Object </param>
    private IEnumerator WorldButtonTransformShowAnimation(GameObject go)
    {
        WorldSelectorAnimatedItem animatedItem;

        animatedItem = null;
        go.transform.position = Vector2.zero;
        go.transform.localScale = Vector2.zero;
        try{animatedItem = go.GetComponent<WorldSelectorAnimatedItem>();}
        catch(Exception ex){Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name +
            "-Not an animated item: " + ex.Message);}
        yield return new WaitForSeconds(worldButtonShowTimes[0]);
        LeanTween.move(go, animatedItem.GetInitialPos(), worldButtonShowTimes[1]);
        LeanTween.scale(go, animatedItem.GetInitialScale(), worldButtonShowTimes[1]);
    }

    /// <summary>
    /// World button animation when unlocked
    /// </summary>
    /// <param name="go"> Game Object </param>
    private IEnumerator WorldButtonTransformUnlockAnimation(GameObject go)
    {
        yield return new WaitForSeconds(worldButtonSelectTimes[0]);
        LeanTween.scale(go, new Vector2(0.44f, 0.44f), worldButtonSelectTimes[1]).setEase(LeanTweenType.easeInOutSine);
        yield return new WaitForSeconds(worldButtonSelectTimes[1] + worldButtonSelectTimes[2]);
        LeanTween.scale(go, new Vector2(0.4f, 0.4f), worldButtonSelectTimes[3]).setEase(LeanTweenType.easeInOutSine);
    }

    /***********************
    Worlds Container
    ***********************/

    private IEnumerator WorldsContainerScaleDownAnimation(GameObject go)
    {
        go.transform.localScale = new Vector2(5, 5);
        LeanTween.scale(go, new Vector2(1, 1), worldsContainerShowTimes[0]).setEase(easeWorldsContainerScaleDown);
        yield return new WaitForSeconds(worldsContainerShowTimes[0]);
    }
}
