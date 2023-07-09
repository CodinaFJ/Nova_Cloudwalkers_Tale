using UnityEngine;

public class SpikeMover : MonoBehaviour
{
    [SerializeField]
    Vector3 moveVector;
    bool moved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!moved){
            other.gameObject.transform.position += moveVector;
            moved = true;
        }
    }

}
