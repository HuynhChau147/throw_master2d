using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider2D capCol2D;
    public AudioSource audioSource;
    public AudioClip hitSound;
    [SerializeField] private int SceneIndex;
    private bool isWin;
    

    private void Start() {
        animator = GetComponent<Animator>();
        isWin = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "bomb" && SceneManager.GetActiveScene().buildIndex < 2)
        {
            audioSource.PlayOneShot(hitSound);
            animator.SetTrigger("isDead");
            StartCoroutine(ChangeScene());
        }

        if(other.collider.tag == "bomb" && SceneManager.GetActiveScene().buildIndex == 2)
        {
            audioSource.PlayOneShot(hitSound);
            animator.SetTrigger("isDead");
            isWin = true;
        }
    }

    public bool getWinStatus()
    {
        return isWin;
    }

    public IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);
        LevelManager.Instance.LoadScence(SceneIndex);
    }
}
