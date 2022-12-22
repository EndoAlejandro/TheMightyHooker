using System;
using System.Collections;
using CustomUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> OnPause;
    public event Action<int> OnDeath;
    public event Action<Vector2Int> OnLoadingLevel;

    private Vector2Int currentProgress;
    public Vector2Int CurrentProgress => currentProgress;

    public static bool IsPaused { get; private set; }
    public static bool AssistMode { get; private set; }
    public PlayerMetrics PlayerMetrics { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        currentProgress = SaveSystem.GetProgress();
    }

    private void Start()
    {
        PlayerMetrics = SaveSystem.GetPlayerMetrics();
        ReturnToMainMenu();
    }

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
        currentProgress.y++;
        if (currentProgress.y >= maxClusterLevel)
        {
            currentProgress.x++;
            currentProgress.y = 0;
        }

        SaveProgress();
        LoadContinueGame();
    }

    public void WinGame()
    {
        currentProgress.x = 0;
        currentProgress.y = 0;
        StartCoroutine(LoadEndGameSceneAsync());
    }

    public void ReturnToMainMenu()
    {
        SaveProgress();
        if (IsPaused) UnPauseGame();
        StartCoroutine(GoToMainMenu());
    }

    public void Options() => StartCoroutine(GoToOptions());

    public void Credits() => StartCoroutine(GoToCredits());

    public void LoadContinueGame() => StartCoroutine(LoadGameSceneAsync());

    public void LoadNewGame()
    {
        PlayerMetrics = new PlayerMetrics();
        currentProgress = Vector2Int.zero;
        SaveProgress();
        StartCoroutine(LoadGameSceneAsync());
    }

    private void SaveProgress()
    {
        SaveSystem.SetPlayerMetrics(PlayerMetrics);
        SaveSystem.SetProgress(currentProgress);
    }

    public void PlayerDeath()
    {
        PlayerMetrics.RegisterDeath();
        OnDeath?.Invoke(PlayerMetrics.DeathCount);
    }

    private IEnumerator GoToCredits()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("Credits_UI");
    }

    private IEnumerator GoToOptions()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("Options_UI");
        SoundManager.Instance.PlayMainMenu();
    }

    private IEnumerator LoadEndGameSceneAsync()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("GameOver_UI");
    }

    private IEnumerator LoadGameSceneAsync()
    {
        AssistMode = SaveSystem.GetAssistMode();
        yield return LoadingTransition(true);
        SceneManager.LoadSceneAsync("Main");
        yield return SceneManager.LoadSceneAsync("Pause_UI", LoadSceneMode.Additive);
    }

    private IEnumerator LoadingTransition(bool loadingGameScene = false)
    {
        yield return SceneManager.LoadSceneAsync("Loading");
        if (loadingGameScene) OnLoadingLevel?.Invoke(CurrentProgress);
        yield return new WaitForSeconds(loadingGameScene ? 2f : 0.25f);
    }

    private IEnumerator GoToMainMenu()
    {
        yield return LoadingTransition();
        yield return SceneManager.LoadSceneAsync("Title_UI");
        SoundManager.Instance.PlayMainMenu();
    }
}

public class PlayerMetrics
{
    public float PlayTime { get; private set; }
    public int DeathCount { get; private set; }

    public PlayerMetrics(float playTime, int deathCount)
    {
        PlayTime = playTime;
        DeathCount = deathCount;
    }

    public PlayerMetrics()
    {
        PlayTime = 0f;
        DeathCount = 0;
    }

    public void RegisterDeath() => DeathCount++;
    public void Tick(float time) => PlayTime += time;
    public string GetTimeWithFormat() => TimeSpan.FromSeconds(PlayTime).ToString("hh':'mm':'ss");
}