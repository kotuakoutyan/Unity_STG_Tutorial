using System.Linq;

namespace UnityEngine
{
    public static class RandomExtention
    {
        private static readonly Vector3 V_FORWARD = Vector3.forward;
        private static readonly Vector3 V_RIGHT = Vector3.right;

        public static Vector3 RandamShake(this Vector3 self, float shake_max)
        {
            if (shake_max == 0.0f) return self;
            float n1 = Random.value, n2 = Random.value;
            var newDirection = Quaternion.Euler(0, 0, 360f * n2) * Quaternion.Euler(shake_max * n1, 0, 0) * V_FORWARD;
            var direction = Quaternion.FromToRotation(V_FORWARD, self);
            return direction * newDirection;
        }

        public static Vector3 GetOrthogonalUnitVector(this Vector3 self)
        {
            float randomNum = Random.value;
            var newDirection = Quaternion.Euler(0, 0, 360f * randomNum) * V_RIGHT;
            return Quaternion.LookRotation(self) * newDirection;
        }

        public static Vector3[] GetOrthogonalUnitVectors(this Vector3[] self)
        {
            float randomNum = Random.value;
            var newDirection = Quaternion.Euler(0, 0, 360f * randomNum) * V_RIGHT;
            return self.Select(v => Quaternion.LookRotation(v) * newDirection).ToArray();
        }

        public static Vector3 TransformDirection(this Vector3 self, Vector3 forward)
        {
            return Quaternion.FromToRotation(V_FORWARD, self) * forward;
        }
    }
}
