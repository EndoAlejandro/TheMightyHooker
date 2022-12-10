using Interfaces;
using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public class SpikesToggle : Spikes, IToggleChild
    {
        [SerializeField] private bool invertValue;
        [SerializeField] private Sprite unActiveSprite;

        public IToggle Toggle { get; private set; }
        public bool State { get; private set; }

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
            State = invertValue ? !value : value;
            renderer.sprite = State ? activeSprite : unActiveSprite;
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