using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    Camera cam;
    public float pushForce = 4f;
    public Trajectory trajectory;
    public Transform centerPoint;
    public Vector3 currentPos;
    public float maxLength = 3f;
    public AudioSource audioSource;
    public AudioClip attackSound;
    public float timeLimit = 10f;
    private float timer = 0f;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Animator ninjaAnimator;
    [SerializeField] Enemy zombie;
    [SerializeField] UIManager UI;
    Rigidbody2D ballRb;
    Collider2D ballCol;
    private int bombNumbers = 3;
    private bool isLastBomb;
    bool isDragging;
    bool canShooting;

    Vector2 direction;
    Vector2 force;
    float distance;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        canShooting = true;
        isLastBomb = false;
    }

    // Update is called once per frame
    private void Update()
    {

        Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

        currentPos = cam.ScreenToWorldPoint(Input.mousePosition);
        currentPos = centerPoint.position + Vector3.ClampMagnitude(currentPos
            - centerPoint.position, maxLength);

        if (Input.GetMouseButtonDown(0) && canShooting)
        {
            if(bombNumbers < 0)
            {return;}
            isDragging = true;
            if(ballRb == null)
            {
                CreateBall();
            }
            ballPrefab.SetActive(true);
            ballRb.isKinematic = true;
            
            isDragging = true;
            OnDragStart();

            if (ballCol)
            {
                ballCol.enabled = true;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(ballRb != null)
            {
                isDragging = false;
                ninjaAnimator.SetBool("isThrow",false);
                OnDragEnd();
            }
        }

        if(isDragging)
        {
            OnDrag();
        }

        if(ballRb != null && ballRb.GetComponent<Ball>().getIsHit() )
        {
            Destroy(ballRb.gameObject,0.5f);
        }

        if(isLastBomb)
        {
            timer += Time.deltaTime;

            if (timer >= timeLimit)
            {
                if(zombie.getWinStatus() == false)
                {
                    audioSource.Stop();
                    UI.GameOver();
                }
            }
        }
    }

    #region Drag

    private void OnDragStart() {
        ninjaAnimator.SetBool("isThrow",true);
    }

    private void OnDrag() {
        trajectory.Show();
        distance = Vector2.Distance(currentPos, centerPoint.position);
        direction = (currentPos - centerPoint.position).normalized;
        force = distance*direction*pushForce;

        trajectory.updateDots(ballRb.transform.position,force);
    }

    private void OnDragEnd() {
        
        if(!canShooting)
        {
            return;
        }

        if(bombNumbers > 0)
        {
            ballRb.isKinematic = false;
            audioSource.PlayOneShot(attackSound);
            ballRb.GetComponent<Ball>().Push(force);
            Invoke("CreateBall",2);
            trajectory.Hide();
        }

        if(bombNumbers == 0)
        {
            ballRb.isKinematic = false;
            ballRb.GetComponent<Ball>().Push(force);
            trajectory.Hide();
            isLastBomb = true;
        }
        
        canShooting = false;
    }
    #endregion

    void CreateBall()
    {
        if(isLastBomb == false)
        {
            ballRb = Instantiate(ballPrefab).GetComponent<Rigidbody2D>();
            ballCol = ballRb.GetComponent<Collider2D>();
            ballCol.enabled = false;

            ballRb.transform.position = centerPoint.position;
            ballRb.isKinematic = true;
            canShooting = true;
            bombNumbers--;
            Debug.Log($"Boom left : {bombNumbers}");
        }
        
    }

    public int getBombLeft()
    {
        return this.bombNumbers;
    }

}
