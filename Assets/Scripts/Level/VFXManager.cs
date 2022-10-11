using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager instance;

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
}
