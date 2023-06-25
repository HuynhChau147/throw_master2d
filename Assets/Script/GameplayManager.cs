using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject ballPrefab;
    Rigidbody2D ballRb;
    Collider2D ballCol;

    private int bombNumbers = 3;

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
            if(ballRb == null)
            {
                CreateBall();
            }
            ballPrefab.SetActive(true);
            ballRb.isKinematic = true;
            
            isDragging = true;
            ballRb.transform.position = centerPoint.position;
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
    }

    #region Drag

    private void OnDragStart() {
        trajectory.Show();
    }

    private void OnDrag() {
        canShooting = false;
        distance = Vector2.Distance(currentPos, centerPoint.position);
        direction = (currentPos - centerPoint.position).normalized;
        force = distance*direction*pushForce;

        trajectory.updateDots(ballRb.transform.position,force);
    }

    private void OnDragEnd() {
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
            bombNumbers--;
        }

        Invoke("Shooting",1f);
    }
    #endregion

    void CreateBall()
    {
        if(canShooting && bombNumbers > 0)
        {
            ballRb = Instantiate(ballPrefab).GetComponent<Rigidbody2D>();
            ballCol = ballRb.GetComponent<Collider2D>();
            ballCol.enabled = false;

            ballRb.isKinematic = true;
            bombNumbers--;
            Debug.Log($"Boom left : {bombNumbers}");
        }
        
    }

    public bool getIsDragging()
    {
        return this.isDragging;
    }

    public void Shooting()
    {
        canShooting = true;
    }
}
