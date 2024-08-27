using UnityEngine;

namespace Entities.PlayerAreaDetecter
{
    public interface IPlayerAreaDetecterUser
    {
        public void SetFollow(Vector3 position, int level);
        public void FollowerGone();
    }
}