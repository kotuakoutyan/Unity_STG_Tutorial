using Barrage.Bullet;
using Barrage.ObjectPool;
using Damage;
using FlyCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageApplicable
{
    [SerializeField] private GameObject Aim = null;

    [SerializeField] public GameObject MainCamera = null;

    private FlyCharacterStatus Status;

    private Animator Anim;

    public Vector3 Velocity { private set; get; }
    private Vector3 OldVelocity;

    public Vector3 LocalVelocity { private set; get; }

    // アニメーションを滑らかにするための係数
    [SerializeField] private float k = 10.0f;

    //発射する弾のプレハブ・情報
    [SerializeField] private DisposableBullet Bullet;
    [SerializeField] private BulletData Data;

    //進行速度
    [SerializeField] private float ForwardSpeed = 7.0f;

    //無敵時間
    [SerializeField] private float InvalidTimer;
    [SerializeField] readonly private float InvalidTime = 1.0f;

    void Awake()
    {
        Status = GetComponent<FlyCharacterStatus>();
        Anim = GetComponent<Animator>();

        // Objectが勝手に回転しないようにする
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        Move();
        Shot();

        if (InvalidTimer > 0) InvalidTimer -= Time.fixedDeltaTime;
    }

    //キャラの移動に関するメソッド
    private void Move()
    {
        //入力を受け取る
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //入力方向に、過去の入力を移動させている
        Velocity = Vector3.MoveTowards(OldVelocity, input, k * Time.deltaTime);
        OldVelocity = Velocity;

        //入力をMainCamera方向を基準に変換している
        LocalVelocity = MainCamera.transform.TransformDirection(Velocity);

        //y軸の成分を0にして、正規化している
        LocalVelocity = new Vector3(LocalVelocity.x, 0, LocalVelocity.z).normalized;

        //キャラの移動
        transform.localPosition += LocalVelocity * ForwardSpeed * Time.fixedDeltaTime;

        //Animatorに対して、パラメータを渡している
        Anim.SetFloat("Speed_x", Velocity.x);
        Anim.SetFloat("Speed_z", Velocity.z);
    }

    //攻撃に関するメソッド
    private void Shot()
    {
        //Fire1に対応したButtonを押されるとtrueを返す
        if (Input.GetButtonDown("Fire1"))
        {
            Shot(5, 0.1f);
        }
        //Fire2に対応したButtonを押されるとtrueを返す
        else if (Input.GetButtonDown("Fire2"))
        {
            Shot(30, 0.5f);
        }
    }

    //指定したベクトルに対してズレる最大の角度
    private readonly float MAX_SHAKE = 60.0f;

    //指定された発射数、ズレの割合に応じて発射するメソッド
    public void Shot(int bulletNum, float bulletShake)
    {
        Vector3 velocity = (Aim.transform.position - transform.position).normalized;
        for (int i = 0; i < bulletNum; i++)
        {
            //DisposableBulletクラスがもつInitializeメソッドを使用し、弾の初期化をしている
            Instantiate(Bullet).Initialize(Status.AttackerType, transform.position, VecoerShake(velocity, MAX_SHAKE * bulletShake), Data);
        }
    }

    //指定されたベクトルにズレを加えたベクトルを返すメソッド
    private Vector3 VecoerShake(Vector3 vec, float shake)
    {
        if (shake == 0.0f) return vec;
        float n1 = Random.value, n2 = Random.value;
        var newDirection = Quaternion.Euler(0, 0, 360f * n2) * Quaternion.Euler(shake * n1, 0, 0) * Vector3.forward;
        var direction = Quaternion.FromToRotation(Vector3.forward, vec);
        return direction * newDirection;
    }

    //被弾に関するメソッド
    public AttackerType GetAttackerType() => Status.AttackerType;
    public void ApplyDamage(DamageData data)
    {
        if (InvalidTimer <= 0.0f && data.Attacker.GetAttackerType() != Status.AttackerType)
        {
            InvalidTimer = InvalidTime;

            if (tag == "Player") Counter.Instance.Attacked();
            else Counter.Instance.Attack();
        }
    }
}
