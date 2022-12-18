using UnityEngine;

public static class SaveSystem
{
    public enum PrefsField
    {
        Master,
        Music,
        Fx,
        Level,
        SubLevel,
        Time,
        DeathCount,
        AssistMode
    }

    public static float GetVolume(PrefsField field)
    {
        if (field != PrefsField.Fx && field != PrefsField.Music && field != PrefsField.Master) return 10f;
        if (!PlayerPrefs.HasKey(field.ToString())) return 10;
        return PlayerPrefs.GetFloat(field.ToString());
    }

    public static void SetVolume(PrefsField field, float value)
    {
        if (field != PrefsField.Fx && field != PrefsField.Music && field != PrefsField.Master) return;
        PlayerPrefs.SetFloat(field.ToString(), value);
    }

    public static Vector2Int GetProgress()
    {
        var x = PlayerPrefs.HasKey(PrefsField.Level.ToString()) ? PlayerPrefs.GetInt(PrefsField.Level.ToString()) : 0;
        var y = PlayerPrefs.HasKey(PrefsField.SubLevel.ToString())
            ? PlayerPrefs.GetInt(PrefsField.SubLevel.ToString())
            : 0;
        return new Vector2Int(x, y);
    }

    public static void SetProgress(Vector2Int progress)
    {
        PlayerPrefs.SetInt(PrefsField.Level.ToString(), progress.x);
        PlayerPrefs.SetInt(PrefsField.SubLevel.ToString(), progress.y);
    }

    public static PlayerMetrics GetPlayerMetrics()
    {
        var time = PlayerPrefs.HasKey(PrefsField.Time.ToString())
            ? PlayerPrefs.GetFloat(PrefsField.Time.ToString())
            : 0f;
        var deathCount = PlayerPrefs.HasKey(PrefsField.DeathCount.ToString())
            ? PlayerPrefs.GetInt(PrefsField.DeathCount.ToString())
            : 0;
        return new PlayerMetrics(time, deathCount);
    }

    public static void SetPlayerMetrics(PlayerMetrics metrics)
    {
        PlayerPrefs.SetInt(PrefsField.DeathCount.ToString(), metrics.DeathCount);
        PlayerPrefs.SetFloat(PrefsField.Time.ToString(), metrics.PlayTime);
    }

    public static bool GetAssistMode()
    {
        var assistValue = PlayerPrefs.HasKey(PrefsField.AssistMode.ToString())
            ? PlayerPrefs.GetInt(PrefsField.AssistMode.ToString())
            : 0;
        return assistValue == 1;
    }

    public static void SetAssistMode(bool value) => PlayerPrefs.SetInt(PrefsField.AssistMode.ToString(), value ? 1 : 0);
}