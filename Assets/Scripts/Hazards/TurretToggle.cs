using Interfaces;
using Pooling;
using UnityEngine;

namespace Hazards
{
    public class TurretToggle : Turret, IToggleChild
    {
        [SerializeField] private bool onlyShootAtToggle;
        [SerializeField] private bool invertValue;
        [SerializeField] private PoolAfterSeconds toggleFx;

        public IToggle Toggle { get; private set; }
        public PoolAfterSeconds ToggleFx => toggleFx;
        public bool State { get; private set; }

        private bool canShoot;

        protected override void Awake()
        {
            base.Awake();
            Toggle = GetComponentInParent<IToggle>();
        }

        protected override void Start()
        {
            base.Start();
            if (onlyShootAtToggle) Renderer.sprite = activeSprite;
            Toggle.OnToggle += OnToggle;
        }

        protected override void Update()
        {
            if (onlyShootAtToggle) return;
            base.Update();
            if (canShoot && CurrentShootingTime < 0) Shoot();
        }

        public void OnToggle(bool value)
        {
            var t = transform;
            ToggleFx.Get<PoolAfterSeconds>(t.position, t.rotation);
            
            if (onlyShootAtToggle) Shoot();
            else
            {
                Renderer.sprite = value ? activeSprite : unActiveSprite;
                canShoot = invertValue ? !value : value;
            }
        }

        private void OnDestroy()
        {
            if (Toggle == null) return;
            Toggle.OnToggle -= OnToggle;
        }
    }
}