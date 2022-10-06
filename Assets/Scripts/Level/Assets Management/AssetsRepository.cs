using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsRepository : MonoBehaviour
{
    public static AssetsRepository instance;

    private void Awake() {

        if(instance == null)
           instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    List<WorldAssets> worldAssetsList = new List<WorldAssets>(4);

    [SerializeField]
    List<CloudTileSpritesBundle> cloudTileSpritesBundles;
}
