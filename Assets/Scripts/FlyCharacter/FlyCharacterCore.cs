using UniRx;
using UnityEngine;
using UniRx.Triggers;
using Damage;
using Barrage.Bullet;

namespace FlyCharacter
{
    public enum MoveType
    {
        xy, zx, OnGround
    }

    // 必要なコンポーネントの列記
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(FlyCharacterStatus))]
    [RequireComponent(typeof(FlyCharacterAction))]
    public class FlyCharacterCore : BaseFlyCharacter, IDamageApplicable
    {
        [SerializeField] private bool IsMine = true;
        
        private FlyCharacterStatus Status;
        private FlyCharacterAction Action;
        private Rigidbody rb;

        [SerializeField] public GameObject MainCamera = null;

        // キャラクターコントローラ（カプセルコライダ）の移動量
        protected MoveType MoveType = MoveType.zx;

        public IReadOnlyReactiveProperty<Vector3> Velocity => velocity;
        private ReactiveProperty<Vector3> velocity = new ReactiveProperty<Vector3>();
        private Vector3 OldVelocity;

        public IReadOnlyReactiveProperty<Vector3> LocalVelocity=> localVelocity;
        private ReactiveProperty<Vector3> localVelocity = new ReactiveProperty<Vector3>();

        /// <summary>
        /// アニメーションを滑らかにするための係数
        /// </summary>
        [SerializeField] private float k = 100.0f;

        private CapsuleCollider col;
        
        public IReadOnlyReactiveProperty<bool> IsOnGround => isOnGround;
        private readonly ReactiveProperty<bool> isOnGround = new ReactiveProperty<bool>(false);
        
        void Awake()
        {
            Status = GetComponent<FlyCharacterStatus>();
            Action = GetComponent<FlyCharacterAction>();

            col = GetComponent<CapsuleCollider>();
        }
        
        override protected void Start()
        {
            base.Start();
            this.UpdateAsObservable().Subscribe(_ => isOnGround.Value = GroundCheck());
            
            InputEventProvider.B_Button.Subscribe(x => { if (x) TypeChange(); });

            this.FixedUpdateAsObservable().Where(_ => IsMine).Subscribe(_ => Move(InputEventProvider.L_Stick.Value));

            this.FixedUpdateAsObservable().Where(_ => InvalidTimer > 0).Subscribe(_ => InvalidTimer -= Time.fixedDeltaTime);

            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            rb = GetComponent<Rigidbody>();
            //RigidbodyのConstraintsを3つともチェック入れて
            //勝手に回転しないようにする
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        /// <summary>
        /// 接地判定を行うメソッド
        /// </summary>
        private bool GroundCheck()
        {
            float radiusRate = 0.5f;
            float maxDistance = col.radius * radiusRate * 1.5f;
            RaycastHit hitInfo;
            return Physics.SphereCast(col.transform.position + Vector3.up * maxDistance, col.radius * radiusRate,
                Vector3.down, out hitInfo, maxDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// 操作タイプを変更するメソッド
        /// </summary>
        private void TypeChange()
        {
            if (MoveType == MoveType.zx) MoveType = MoveType.xy;
            else if (MoveType == MoveType.xy) MoveType = MoveType.zx;
        }
        
        [SerializeField] private float ForwardSpeed = 7.0f;
        private void Move(Vector3 input)
        {
            //移動用のパラメータ
            if (MoveType == MoveType.zx) velocity.SetValueAndForceNotify(Vector3.MoveTowards(OldVelocity, input, k * Time.deltaTime));
            else if (MoveType == MoveType.xy) velocity.SetValueAndForceNotify(Vector3.MoveTowards(OldVelocity, new Vector3(input.x, 0, input.y), k * Time.deltaTime));
            OldVelocity = Velocity.Value;

            localVelocity.SetValueAndForceNotify(MainCamera.transform.TransformDirection(Velocity.Value));
            transform.localPosition += LocalVelocity.Value * ForwardSpeed * Time.fixedDeltaTime;
        }

        [SerializeField] private float InvalidTimer;
        [SerializeField] readonly private float InvalidTime = 1.0f;
        public AttackerType GetAttackerType() => Status.AttackerType;
        public void ApplyDamage(DamageData data)
        {
            if(InvalidTimer <= 0.0f && data.Attacker.GetAttackerType() != Status.AttackerType)
            {
                InvalidTimer = InvalidTime;
                
                if (tag == "Player") Counter.Instance.Attacked();
                else Counter.Instance.Attack();
            }
        }
    }
}
