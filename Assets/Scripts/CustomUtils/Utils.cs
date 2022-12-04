using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomUtils
{
    public static class Utils
    {
        public static void ColorLog(string value, Color color)
        {
            var convertedColor = ColorUtility.ToHtmlStringRGB(color);
            Debug.Log($"<color=#{convertedColor}>{value}</color>");
        }
        
        public static IEnumerator DieSequence(float animationSpeed, float bulletTime, Action callback = null)
        {
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(bulletTime);
            while (Time.timeScale < 0.95f)
            {
                Time.timeScale += Time.deltaTime * animationSpeed;
                yield return null;
            }

            Time.timeScale = 1f;
            yield return new WaitForSecondsRealtime(bulletTime);
            callback?.Invoke();
        }
    }
}