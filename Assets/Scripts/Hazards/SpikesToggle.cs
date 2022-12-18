using Interfaces;
using PlayerComponents;
using Pooling;
using UnityEngine;

namespace Hazards
{
    public class SpikesToggle : Spikes, IToggleChild
    {
        [SerializeField] private bool invertValue;
        [SerializeField] private PoolAfterSeconds toggleFx;
        [SerializeField] private Sprite unActiveSprite;

        public IToggle Toggle { get; private set; }
        public PoolAfterSeconds ToggleFx => toggleFx;
        public bool State { get; private set; }
        public override bool IsActive => State;

        private new SpriteRenderer renderer;
        private Sprite activeSprite;

        private void Awake()
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
            Toggle = GetComponentInParent<IToggle>();

            Toggle.OnToggle += OnToggle;
            activeSprite = renderer.sprite;
        }

        public void OnToggle(bool value)
        {
            var t = transform;
            ToggleFx.Get<PoolAfterSeconds>(t.position, t.rotation);
            State = invertValue ? !value : value;
            renderer.sprite = State ? activeSprite : unActiveSprite;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;
            if (State) KillPLayer(player);
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Player player)) return;
            if (State) KillPLayer(player);
        }

        private void OnDestroy()
        {
            if (Toggle == null) return;
            Toggle.OnToggle -= OnToggle;
        }
    }
}