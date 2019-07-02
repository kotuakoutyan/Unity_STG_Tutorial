using UnityEngine;
using UniRx;

namespace FlyCharacter
{
    public class FlyCharacterAnimator : BaseFlyCharacter
    {
        private Animator Anim;
        private int UpLayer;
        private int DownLayer;

        private bool IsOnGround = false;
        
        void Awake()
        {
            Anim = GetComponent<Animator>();
            UpLayer = Anim.GetLayerIndex("Up Layer");
            DownLayer = Anim.GetLayerIndex("Down Layer");

        }

        override protected void Start()
        {
            base.Start();
            //GetComponent<FlyCharacterCore>().IsOnGround.DistinctUntilChanged().Subscribe(x => { Anim.SetBool("IsLand", x); IsOnGround = x; });
            GetComponent<FlyCharacterCore>().Velocity.DistinctUntilChanged().Subscribe(x => SetVelocity(x));
        }

        /// <summary>
        /// Animatorに対してVelocityを渡すメソッド
        /// </summary>
        /// <param name="velocity">進行方向</param>
        private void SetVelocity(Vector3 velocity)
        {
            if (IsOnGround)
            {
                Anim.SetFloat("Speed_z", velocity.magnitude);

                Anim.SetLayerWeight(UpLayer, 0.0f);
                Anim.SetLayerWeight(DownLayer, 0.0f);
            }
            else
            {
                Anim.SetFloat("Speed_x", velocity.x);
                Anim.SetFloat("Speed_z", velocity.z);

                Anim.SetLayerWeight(UpLayer, Mathf.Clamp(velocity.y > 0 ? velocity.y : 0.0f, 0.0f, 1.0f));
                Anim.SetLayerWeight(DownLayer, Mathf.Clamp(velocity.y < 0 ? -velocity.y : 0.0f, 0.0f, 1.0f));
            }
            
        }

        public void PlayBarrageAnimation()
        {
            Anim.SetBool("BarrageAttack", true);
        }

        public void PlaySwordAnimation()
        {
            Anim.SetBool("SwordAttack", true);
        }

        public void PlayDamageAnimation(Vector3 direction)
        {

        }

    }
}
