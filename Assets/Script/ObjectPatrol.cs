using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPatrol : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;

    private bool movingDown;
    private float topEdge;
    private float btmEdge;

    private void Awake() {
        topEdge = transform.position.y - movementDistance;
        btmEdge = transform.position.y + movementDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(movingDown)
        {
            if(transform.position.y > topEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime,transform.position.z);
            }
            else
            {
                movingDown = false;
            }
        }
        else
        {
            if(transform.position.y < btmEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime,transform.position.z);
            }
            else
            {
                movingDown = true;
            }
        }
    }
}
