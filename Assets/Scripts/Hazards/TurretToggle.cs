using Interfaces;

namespace Hazards
{
    public class TurretToggle : Turret, IToggleChild
    {
        public IToggle Toggle { get; private set; }
        public bool State { get; private set; }

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

        public void OnToggle(bool value) => Shoot();

        private void OnDestroy()
        {
            if (Toggle == null) return;
            Toggle.OnToggle -= OnToggle;
        }
    }
}