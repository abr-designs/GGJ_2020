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
    private int gameSceneIndex = 0;

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
    public void LoadMenuScene(Action OnLoadedCallback)
    {
        StartCoroutine(LoadTargetSceneAsync(menuSceneIndex, OnLoadedCallback));
    }
    
    [HorizontalGroup("LoadScene"), Button("Load Game")]
    public void LoadGameScene(Action OnLoadedCallback)
    {
        StartCoroutine(LoadTargetSceneAsync(gameSceneIndex, OnLoadedCallback));
        
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
