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
        MaxTime,
    }

    public static float GetVolume(PrefsField field)
    {
        if (field != PrefsField.Fx && field != PrefsField.Music && field != PrefsField.Master) return -1f;
        if (!PlayerPrefs.HasKey(field.ToString())) return -1;
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
        if (progress == Vector2Int.zero) return;
        PlayerPrefs.SetInt(PrefsField.Level.ToString(), progress.x);
        PlayerPrefs.SetInt(PrefsField.SubLevel.ToString(), progress.y);
    }

    public static float GetTime() => !PlayerPrefs.HasKey(PrefsField.Time.ToString())
        ? 0f
        : PlayerPrefs.GetFloat(PrefsField.Time.ToString());

    public static void SetTime(float value) => PlayerPrefs.SetFloat(PrefsField.Time.ToString(), value);

    public static float GetMaxTime() => !PlayerPrefs.HasKey(PrefsField.MaxTime.ToString())
        ? 0f
        : PlayerPrefs.GetFloat(PrefsField.MaxTime.ToString());

    public static void SetMaxTime(float value) => PlayerPrefs.SetFloat(PrefsField.MaxTime.ToString(), value);
}