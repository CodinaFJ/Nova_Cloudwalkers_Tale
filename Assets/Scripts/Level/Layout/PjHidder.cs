using UnityEngine;

public class PjHidder : MonoBehaviour
{

    GameManager gameManager;
    [SerializeField] GameObject[] disableOnExitGOs;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            MatrixManager.instance.MakeAllWalkable();
            SpriteRenderer myRenderer = other.GetComponent<SpriteRenderer>();
            myRenderer.sortingLayerName = "Mural";
            myRenderer.sortingOrder = -1;
            foreach(GameObject go in disableOnExitGOs)
            {
                go.SetActive(false);
            }
        }
    }
}
