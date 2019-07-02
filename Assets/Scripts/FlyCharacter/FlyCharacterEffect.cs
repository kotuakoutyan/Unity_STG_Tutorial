using UnityEngine;
using UniRx;

namespace FlyCharacter
{
    public class FlyCharacterEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem SpeedLine = null;

        void Start()
        {
            GetComponent<FlyCharacterCore>().LocalVelocity.DistinctUntilChanged().Subscribe(x => SetSpeedLine(x));
        }

        public void SetSpeedLine(Vector3 velocity)
        {
            if (SpeedLine != null)
            {
                SpeedLine.transform.position = transform.position;
                if (velocity.magnitude < 1.0f) SpeedLine.Stop();
                else
                {
                    SpeedLine.Play();
                    SpeedLine.transform.rotation = Quaternion.FromToRotation(Vector3.forward, velocity);
                }
            }
        }
    }
}