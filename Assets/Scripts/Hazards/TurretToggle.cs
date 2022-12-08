using Interfaces;
using UnityEngine;

namespace Hazards
{
    public class TurretToggle : Turret, IToggleChild
    {
        [SerializeField] private bool onlyShootAtToggle;
        [SerializeField] private bool invertValue;

        public IToggle Toggle { get; private set; }
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
            Toggle.OnToggle += OnToggle;
        }

        protected override void Update()
        {
            if (onlyShootAtToggle) return;
            base.Update();
            if (canShoot && currentShootingTime < 0) Shoot();
        }

        public void OnToggle(bool value)
        {
            if (onlyShootAtToggle) Shoot();
            else canShoot = invertValue ? !value : value;
        }

        private void OnDestroy()
        {
            if (Toggle == null) return;
            Toggle.OnToggle -= OnToggle;
        }
    }
}