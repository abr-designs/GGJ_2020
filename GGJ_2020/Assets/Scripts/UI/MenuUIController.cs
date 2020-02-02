using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    //================================================================================================================//

    [SerializeField, Required] private Button startGameButton;
    [SerializeField, Required] private Button creditButton;
    [SerializeField, Required] private Button backButton;
    [SerializeField, Required] private Button endGameButton;
    [SerializeField, Required] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeTime = 1f;

    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject creditsWindow;

    //================================================================================================================//

    // Start is called before the first frame update
    private void Start()
    {
        fadeCanvasGroup.alpha = 0f;
        startGameButton.onClick.AddListener(StartGame);
        endGameButton.onClick.AddListener(ExitGame);

        creditButton.onClick.AddListener(() =>
        {
            menuWindow.SetActive(false);
            creditsWindow.SetActive(true);
        });
        backButton.onClick.AddListener(() =>
        {
            menuWindow.SetActive(true);
            creditsWindow.SetActive(false);
        });

        menuWindow.SetActive(true);
        creditsWindow.SetActive(false);
    }

    //================================================================================================================//

    private void StartGame()
    {

        StartCoroutine(FadeAndTriggerCoroutine(fadeTime, () => { SceneController.Instance.LoadGameScene(null); }));
    }

    private void ExitGame()
    {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        return;
#endif

        Application.Quit();

    }

    private IEnumerator FadeAndTriggerCoroutine(float fadeTime, Action OnLoadedCallback)
    {
        float t = 0;

        while (t < fadeTime)
        {
            fadeCanvasGroup.alpha = t / fadeTime;

            t += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        OnLoadedCallback?.Invoke();
    }

    //================================================================================================================//
}
