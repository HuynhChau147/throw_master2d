using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Ball : MonoBehaviour
{
    public Rigidbody2D ballRb;
    public Collider2D ballCol;
    public Animator anim;
    bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody2D>();
        ballCol = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        isHit = false;
    }

    public void Push(Vector2 force){
        ballRb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D col2D) {
        if(col2D.collider.tag == "Enemy")
        {
            ballRb.Sleep();
            anim.SetTrigger("boomTrigger");
            isHit = true;
            Debug.Log(isHit);
        }        
    }
        

    public bool getIsHit()
    {
        return this.isHit;
    }
}
