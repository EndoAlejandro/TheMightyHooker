using System;
using System.Collections;
using CustomUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> OnPause;

    public static bool IsPaused { get; private set; }
    public int CurrentLevel { get; private set; }
    public int CurrentSubLevel { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start() => ReturnToMainMenu();

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0;
        OnPause?.Invoke(IsPaused);
    }

    public void UnPauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1;
        OnPause?.Invoke(IsPaused);
    }

    public void ExitGame() => Application.Quit();

    public static void CreateGameManager()
    {
        var go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
    }

    public void WinLevel(int maxClusterLevel)
    {
        CurrentSubLevel++;
        if (CurrentSubLevel >= maxClusterLevel)
        {
            CurrentLevel++;
            CurrentSubLevel = 0;
        }

        LoadGameScene();
    }

    public void WinGame()
    {
        CurrentLevel = 0;
        CurrentSubLevel = 0;
        StartCoroutine(LoadEndGameSceneAsync());
    }

    public void ReturnToMainMenu()
    {
        if (IsPaused) UnPauseGame();
        StartCoroutine(GoToMainMenu());
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadEndGameSceneAsync()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("_Scenes/GameOverMenu");
    }

    private IEnumerator LoadGameSceneAsync()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("_Scenes/MainGame");
        yield return SceneManager.LoadSceneAsync("_Scenes/GameMenu", LoadSceneMode.Additive);
    }

    private IEnumerator LoadingTransition()
    {
        yield return SceneManager.LoadSceneAsync("_Scenes/LoadingScene");
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator GoToMainMenu()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("_Scenes/MainMenu");
        SoundManager.Instance.PlayMainMenu();
    }

    public void LoseLevel() => LoadGameScene();
}