namespace Enemies
{
    public interface IDie
    {
        public bool IsAlive { get; }
        public void Die();
    }
}