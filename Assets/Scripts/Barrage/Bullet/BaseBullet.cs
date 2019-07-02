using UnityEngine;
using Damage;

namespace Barrage.Bullet
{
    [RequireComponent(typeof(SphereCollider))]
    abstract public class BaseBullet : MonoBehaviour, IAttacker
    {
        protected Transform Transform;

        protected AttackerType AttackerType;
        protected Vector3 Velocity;
        protected float InitializeTime;
        protected BulletData Data;
        protected GameObject Target;

        public AttackerType GetAttackerType() => AttackerType;

        virtual protected void Awake()
        {
            Transform = this.transform;
        }

        protected void FixedUpdate()
        {
            Transform.localPosition += Velocity * Time.fixedDeltaTime;
            if (Time.time - InitializeTime > Data.ValidTime) ResetObject();
            AdditionalUpdate();
        }

        /// <summary>
        /// 継承先でFixedUpdateに追加したい処理
        /// </summary>
        abstract protected void AdditionalUpdate();

        /// <summary>
        /// 弾を初期化する基本メソッド
        /// </summary>
        /// <param name="attackerType">弾を発射したキャラの所属</param>
        /// <param name="position">発射時の発射キャラの位置</param>
        /// <param name="velocity">弾の移動ベクトル</param>
        /// <param name="data">弾の基本情報</param>
        /// <param name="target">弾のターゲット</param>
        /// <param name="offset">弾の初期位置からの相対距離</param>
        virtual public void Initialize(AttackerType attackerType, Vector3 position, Vector3 velocity, BulletData data, GameObject target, Vector3 offset)
        {
            gameObject.SetActive(true);

            AttackerType = attackerType;
            transform.position = position + offset;
            Velocity = velocity * data.Speed;
            InitializeTime = Time.time;
            Data = data;
            Target = target;
        }

        /// <summary>
        /// 弾を無効にする基本メソッド
        /// </summary>
        virtual protected void ResetObject()
        {
            var parent = GetComponentInParent<RotateOrigin>();
            if (parent != null) parent.ResetObject();
            gameObject.SetActive(false);
        }

        protected void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<IDamageApplicable>();
            if (enemy == null || enemy.GetAttackerType() == AttackerType) return;

            var direction = (transform.position - other.transform.position).normalized;
            var damage = new DamageData(this, Data.Damage, direction);
            other.GetComponent<IDamageApplicable>().ApplyDamage(damage);
        }
    }
}