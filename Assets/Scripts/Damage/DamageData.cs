using Barrage.Bullet;
using UnityEngine;

namespace Damage
{
    public struct DamageData
    {
        public IAttacker Attacker;
        public int Damage;
        public Vector3 Direction;

        public DamageData(IAttacker attacker, int damage, Vector3 direction)
        {
            Attacker = attacker;
            Damage = damage;
            Direction = direction;
        }
    }
}
