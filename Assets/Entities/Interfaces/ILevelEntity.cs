namespace Entities.Interfaces
{
    public interface ILevelEntity
    {
        public int GetLevel();
        public void Hit();
        public void Die();
    }
}