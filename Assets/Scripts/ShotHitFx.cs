using Pooling;
using UnityEngine;

public class ShotHitFx : PoolAfterSeconds
{
    private Animator animator;
    private static readonly int Hit = Animator.StringToHash("Hit");
    private void Awake() => animator = GetComponent<Animator>();
    public void PlayAnimation() => animator.SetTrigger(Hit);
}