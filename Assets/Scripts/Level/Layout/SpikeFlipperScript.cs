using UnityEngine;

public class SpikeFlipperScript : MonoBehaviour
{
    bool flipped = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!flipped){
            if(other.gameObject.GetComponent<SpriteRenderer>().flipY) other.gameObject.GetComponent<SpriteRenderer>().flipY = false;
            else other.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            flipped = true;
            Debug.Log("spikedetected");
        }
    }

}
