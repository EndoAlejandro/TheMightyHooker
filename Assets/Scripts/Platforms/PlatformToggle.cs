using Interfaces;

namespace Platforms
{
    public class PlatformToggle : PlatformMovingAutomatic, IToggleChild
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

        protected override void Update()
        {
            if (!State) return;
            base.Update();
        }

        public void OnToggle(bool value)
        {
            State = value;
            CurrentStillTime = 0;
            ChangeSprites(State);
        }

        private void OnDestroy()
        {
            if (Toggle == null) return;
            Toggle.OnToggle -= OnToggle;
        }
    }
}