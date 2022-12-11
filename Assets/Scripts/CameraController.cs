using System.Collections;
using CustomUtils;
using PlayerComponents;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Player player;

    private Vector3 initialPosition = new Vector3();
    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
    }

    public void CamShake(Vector2 values) => StartCoroutine(CamShakeSequence(values.x, values.y));

    private IEnumerator CamShakeSequence(float time, float magnitude)
    {
        var currentTime = 0f;
        while (currentTime < time)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y) + initialPosition;

            currentTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
    }
    
}