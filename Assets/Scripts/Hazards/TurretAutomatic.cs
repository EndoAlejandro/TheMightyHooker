namespace Hazards
{
    public class TurretAutomatic : Turret
    {
        protected override void Start()
        {
            base.Start();
            Renderer.sprite = activeSprite;
        }

        protected override void Update()
        {
            base.Update();
            if(CurrentShootingTime < 0)
                Shoot();
        }
    }
}