using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehavior : MonoBehaviour
{
    [SerializeField] Vector3 star1;
    [SerializeField] Vector3 star2;
    [SerializeField] Vector3 star3;


    [SerializeField] float speed = 1f;
    [SerializeField] Animator IdleStar;
    [SerializeField] Animator myAnimator;

    [SerializeField] ParticleSystem starParticles;

    MatrixManager matrixManager;
    PlayerBehavior playerBehavior;

    SpriteRenderer mySpriteRenderer;

    private string currentState;

    public int[] starCell = new int[2];

    bool followPlayer = false;

    int starNumber = 0;

    [HideInInspector] public bool starCollected = false;

    private Vector3 playerPosition;
    private Vector3 offset;

    void Start()
    {
        matrixManager = FindObjectOfType<MatrixManager>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        SnapToGrid();
    }


    void Update()
    {
        if(playerBehavior.pjCell[0] == starCell[0] && playerBehavior.pjCell[1] == starCell[1] && !starCollected)
        {
            FindObjectOfType<Stars>().collectedStars++;
            starNumber = FindObjectOfType<Stars>().collectedStars;
            playerBehavior.starsCollected++;
            StarPickedUp();
            followPlayer = true;
            starCollected = true;
            
        }
        if(followPlayer)
        {
            FollowPlayer();
        }
        else StopFollowPlayer();
    }

    void SnapToGrid()
    {
        Vector3 starPosition = transform.position;

        starPosition = new Vector3 (Mathf.FloorToInt(starPosition.x), Mathf.FloorToInt(starPosition.y), Mathf.FloorToInt(starPosition.z)) + new Vector3(0.5f, 0.5f, 0f);

        starCell = matrixManager.FromWorldToMatrixIndex(starPosition);

        transform.position = starPosition;
    }

    void StarPickedUp()
    {
        FindObjectOfType<LevelInfo>().collectedStars++;

        if (playerBehavior.starsCollected == 1)
        { 
            offset = star1;
            speed = 4f;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            AudioManager.instance.PlaySound("StarTake1");
        } 
        if (playerBehavior.starsCollected == 2) 
        { 
            offset = star2;
            speed = 3.85f;
            AudioManager.instance.PlaySound("StarTake2");
        }
        if (playerBehavior.starsCollected == 3)
        { 
            offset = star3;
            speed = 4.25f;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            AudioManager.instance.PlaySound("StarTake1");
        } 
    }

    void FollowPlayer()
    {
        IdleStar.gameObject.SetActive(false);
        ChangeAnimationState("FollowPlayer");
        mySpriteRenderer.sortingLayerID = SortingLayer.NameToID("Player");
        starParticles.Stop();

        playerPosition = playerBehavior.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, playerPosition + offset, speed * Time.deltaTime);
    }

    void StopFollowPlayer()
    {
        IdleStar.gameObject.SetActive(true);
        ChangeAnimationState("Idle");
        mySpriteRenderer.sortingLayerID = SortingLayer.NameToID("Stars");
        starParticles.Play();
    }

    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;

        //play the animation
        myAnimator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    void SetFollowPlayer(bool follow)
    {
        followPlayer = follow;
        transform.position = playerBehavior.transform.position + offset;
    }

    void SetStarCollected(bool collected)
    {
        starCollected = collected;
    }

    void PlaceInInitialPosition()
    {
        transform.position = matrixManager.FromMatrixIndexToWorld(starCell[0], starCell[1]);
        starCollected = false;
    }

    public void LoadLevelStateStar(bool collected)
    {  
        SetFollowPlayer(collected);
        SetStarCollected(collected);
        if(!collected) PlaceInInitialPosition();
    }

    public bool GetStarCollected()
    {
        return starCollected;
    }
}
