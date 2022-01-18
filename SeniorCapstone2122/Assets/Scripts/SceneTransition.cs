using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    AsyncOperation loadScene;
    Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    public void HitTrigger()
    {
        loadScene = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1, LoadSceneMode.Single);
        loadScene.allowSceneActivation = false;
    }

    public void LoadNext()
    {
        loadScene.allowSceneActivation = true;
    }
}
