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
        HitTrigger();
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

    public void Menu()
    {
        loadScene = null;
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LoadNext();
        }
    }
}
