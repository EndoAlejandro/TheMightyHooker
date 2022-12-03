using System.Collections;
using CustomUtils;
using PlayerComponents;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Player player;

    private Vector3 initialPosition = new Vector3();
    private void Start() => initialPosition = transform.localPosition;

    public void CamShake(Vector2 values) => StartCoroutine(CamShakeSequence(values.x, values.y));

    private IEnumerator CamShakeSequence(float time, float magnitude)
    {
        var currentTime = 0f;
        while (currentTime < time)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, transform.position.z);

            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
    
}