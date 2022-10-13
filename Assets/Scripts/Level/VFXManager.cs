using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

    private List<Vector2> tilesWithCloudToJoin = new List<Vector2>();

    private void Awake() {
        if(instance == null)
           instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    public void InstantiateParticles(ParticlesVFXType particlesType) => InstantiateParticles(particlesType, MatrixManager.instance.FromMatrixIndexToWorld(PlayerBehavior.instance.pjCell[0], PlayerBehavior.instance.pjCell[1]));
    public void InstantiateParticles(ParticlesVFXType particlesType, Vector3 pos){
        GameObject particlesGO = Instantiate(AssetsRepository.instance.GetParticlesVFX(ParticlesVFXType.CrystalFloorBreak).particlesPrefab, pos, Quaternion.identity);
        particlesGO.GetComponent<ParticleSystem>().Play();
    }

    public void InstantiateGreyParticles(Vector2 coor, Vector2 coorAdjacent){
        if(tilesWithCloudToJoin.Find(x => x == coor) != null){
            Vector2 pos = (MatrixManager.instance.FromMatrixIndexToWorld(coor) + MatrixManager.instance.FromMatrixIndexToWorld(coor + coorAdjacent))/2;
            GameObject particlesGO = Instantiate(AssetsRepository.instance.GetParticlesVFX(ParticlesVFXType.GreyCloudJoin).particlesPrefab, pos, Quaternion.identity);
            particlesGO.GetComponent<ParticleSystem>().Play();
        }
    }

    public void PreInstantiateGreyParticles(int item){
        tilesWithCloudToJoin.Clear();

        int[,] itemLayoutMatrix = MatrixManager.instance.GetItemsLayoutMatrix();

        for (int i = 0; i < itemLayoutMatrix.GetLength(0); i++){
            for (int j = 0; j < itemLayoutMatrix.GetLength(1); j++){
                if(itemLayoutMatrix[i,j] == item) tilesWithCloudToJoin.Add(new Vector2(i,j));
            }
        }
    }
}
