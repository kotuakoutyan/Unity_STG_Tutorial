using Barrage.Iterators;
using UnityEngine;
using System.Linq;

namespace Barrage.Bullet
{
    public class RotateOrigin: MonoBehaviour
    {
        [SerializeField]private GameObject Target;
        private Vector3 Axis;
        private float AngularSpeed;

        void Awake()
        {
            gameObject.name = "RotateOrigin";
        }

        public void Initialize(GameObject shooter, float angularSpeed, Vector3 axis, GameObject target)
        {
            gameObject.SetActive(true);
            if(target != default) transform.SetParent(shooter.transform);
            transform.position = shooter.transform.position;
            transform.rotation = Quaternion.identity;
            Target = target;
            Axis = axis;
            AngularSpeed = angularSpeed;
        }

        void FixedUpdate()
        {
            if (Target != default)
            {
                transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position, transform.up);
                transform.rotation *= Quaternion.AngleAxis(AngularSpeed * Time.fixedDeltaTime, transform.forward);
            }

            else
            {
                transform.rotation *= Quaternion.AngleAxis(AngularSpeed * Time.fixedDeltaTime, Axis);
            }
        }

        public void ResetObject()
        {
            var bullets = GetComponentsInChildren<BaseBullet>();
            foreach (BaseBullet bullet in bullets) bullet.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
