namespace Hazards
{
    public class TurretAutomatic : Turret
    {
        protected override void Update()
        {
            base.Update();
            if(currentShootingTime < 0)
                Shoot();
        }
    }
}