using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private float _target;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScence(int SceneIndex)
    {
        
        var scene = SceneManager.LoadSceneAsync(SceneIndex);
        scene.allowSceneActivation = false;


        do{
            await UniTask.Delay(1000);
            _target = scene.progress;
            Debug.Log(_target);
        }while (scene.progress < 0.9f);
        await UniTask.Delay(1000);

        scene.allowSceneActivation = true;
    }
}
