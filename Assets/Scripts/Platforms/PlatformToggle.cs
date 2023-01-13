using System;
using Interfaces;
using Pooling;
using UnityEngine;

namespace Platforms
{
    public class PlatformToggle : PlatformMovingAutomatic, IToggleChild
    {
        [SerializeField] private PoolAfterSeconds toggleFx;
        public IToggle Toggle { get; private set; }
        public PoolAfterSeconds ToggleFx => toggleFx;
        public bool State { get; private set; }

        [SerializeField] private Collider2D debugCenter;

        protected override void Awake()
        {
            base.Awake();
            Toggle = GetComponentInParent<IToggle>();
        }

        protected override void Start()
        {
            base.Start();
            Toggle.OnToggle += OnToggle;
        }

        protected override void Update()
        {
            if (!State) return;
            base.Update();
        }

        public void OnToggle(bool value)
        {
            var t = transform;
            ToggleFx.Get<PoolAfterSeconds>(t.position, t.rotation);
            State = value;
            CurrentStillTime = 0;
            ChangeSprites(State);
        }

        private void OnDestroy()
        {
            if (Toggle == null) return;
            Toggle.OnToggle -= OnToggle;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = debugColor;
            Gizmos.DrawCube(debugCenter.bounds.center, debugCenter.bounds.size);
        }

        [ColorUsage(true, false)] public Color debugColor;
    }
}