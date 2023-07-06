using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber;
    [SerializeField] GameObject dotsParrent;
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] float dotSpacing;
    [SerializeField] [Range(0.01f, 0.1f)] float dotMinScale;
    [SerializeField] [Range(0.1f, 0.5f)] float dotMaxScale;

    Transform[] dotsList;
    Vector2 pos;
    float timeStamp;

    private void Start() {
        Hide();
        prepareDots();
    }

    void prepareDots(){
        dotsList = new Transform[dotsNumber];
        dotsPrefab.transform.localScale = Vector3.one * dotMaxScale;

        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;

        for(int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotsPrefab, null).transform;
            dotsList[i].parent = dotsParrent.transform;

            dotsList[i].localScale = Vector3.one * scale;
            if(scale > dotMinScale)
            {
                scale -= scaleFactor;
            }
        }
    }

    public void updateDots(Vector3 ballPos, Vector2 forceApplied){
        timeStamp = dotSpacing;

        for(int i = 0; i < dotsNumber; i++)
        {
            pos.x = (ballPos.x + forceApplied.x * timeStamp);
            pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }
    }

    public void Hide()
    {
        dotsParrent.SetActive(false);
    }

    public void Show()
    {
        dotsParrent.SetActive(true);
    }
}
