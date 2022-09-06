using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
   [SerializeField] Animator crossfade;

   [SerializeField] float transitionTime = 1f;

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelFading(1));
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelFading(levelName));
    }

    public void LoadLevel(int levelNumber)
    {
        StartCoroutine(LoadLevelFading(levelNumber));
    }

    IEnumerator LoadLevelFading(int levelIndex)
    {
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelFading(string levelName)
    {
        crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }

    public void FadeOut()
    {
        crossfade.Play("Crossfade_Out");
    }


    public void FadeIn()
    {
        crossfade.Play("Crossfade_In");
    }
}
