using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
   [SerializeField] Animator crossfade;
   [SerializeField] float transitionTime = 1f;

   static public LevelLoader instance;

   private void Awake() {
    instance = this;
   }

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

    public static string GetLevelContains(string levelNameID){
        string sceneToLoad = null;

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
        string[] scenes = new string[sceneCount];
        for( int i = 0; i < sceneCount; i++ ){
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ));
            if(scenes[i].Contains(levelNameID)) sceneToLoad = scenes[i];
        }

        return sceneToLoad;
    }
}
