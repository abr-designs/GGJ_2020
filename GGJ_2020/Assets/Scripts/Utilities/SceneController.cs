using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    //================================================================================================================//
    //TODO Get start Scene
    [SerializeField]
    private int menuSceneIndex;

    [SerializeField]
    private int gameSceneIndex;

    //================================================================================================================//

    private Scene currentScene { get; set; }

    //================================================================================================================//
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(this);


        currentScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += LoadedScene;

    }

    [HorizontalGroup("LoadScene"), Button("Load Menu")]
    public void LoadMenuScene()
    {
        StartCoroutine(LoadTargetSceneAsync(menuSceneIndex, null));
    }
    
    [HorizontalGroup("LoadScene"), Button("Load Game")]
    public void LoadGameScene()
    {
        StartCoroutine(LoadTargetSceneAsync(gameSceneIndex, null));
    }
    //================================================================================================================//
    private IEnumerator LoadTargetSceneAsync(int targetScene, Action onSceneLoaded)
    {
        Debug.Log("Called");
        //Load Target Scene
        var asyncLoad = SceneManager.LoadSceneAsync(targetScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        onSceneLoaded?.Invoke();
        
    }
    //================================================================================================================//

    private void LoadedScene(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene;
    }
}
