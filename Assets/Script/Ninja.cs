using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    public Animator animator;
    public GameplayManager gm;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.getIsDragging())
        {
            animator.SetBool("isThrow",true);
        }
        
        if(!gm.getIsDragging())
        {
            animator.SetBool("isThrow",false);
        }
    }
}
