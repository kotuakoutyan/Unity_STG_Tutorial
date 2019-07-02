## PlayerControllerの解説
このクラスでは自機の移動、自機のアニメーション管理、攻撃の処理、被弾の処理を行っている。

## Playerの移動
自機の移動に関するソースコードは以下の部分。
```cs
void Start()
{
  // Objectが勝手に回転しないようにする
  GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
}

private void Move()
{
    //入力を受け取る
    var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    //入力方向に、過去の入力を移動させている
    Velocity = Vector3.MoveTowards(OldVelocity, input, k * Time.deltaTime);

    //過去のVelocityを更新
    OldVelocity = Velocity;

    //入力をMainCamera方向を基準に変換している
    LocalVelocity = MainCamera.transform.TransformDirection(Velocity);

    //y軸の成分を0にして、正規化している
    LocalVelocity = new Vector3(LocalVelocity.x, 0, LocalVelocity.z).normalized;

    //キャラの移動
    transform.localPosition += LocalVelocity * ForwardSpeed * Time.fixedDeltaTime;
}
```


## アニメーション
アニメーションに関するソースコードは以下の部分。
```cs
private Animator Anim;

void Start()
{
  Anim = GetComponent<Animator>();
}

void Move()
{
  //Animatorに対して、パラメータを渡している
  Anim.SetFloat("Speed_x", Velocity.x);
  Anim.SetFloat("Speed_z", Velocity.z);
}
```

#### [Animator](https://hiyotama.hatenablog.com/entry/2015/06/27/090000)
複数のアニメーションをMecanimで管理してくれる凄いやつ。今回は `Anim.SetFloat("Speed_x", Velocity.x)` と `Anim.SetFloat("Speed_z", Velocity.z)` でMecanimを制御している。<br>
今回は自機が常に画面奥に向かっているから `Velocity` を渡しているが、自機が入力方向を向くようなゲーム（MGSやMHなど）の場合、 `LocalVelocity` を渡してやるとうまくいく。

## 攻撃の処理
攻撃に関するソースコードは以下の部分。
```cs
//発射する弾のプレハブ・情報
[SerializeField] private DisposableBullet Bullet;
[SerializeField] private BulletData Data;

//攻撃に関するメソッド
private void Shot()
{
    if (Input.GetButtonDown("Fire1")) Shot(5, 0.1f);
    else if (Input.GetButtonDown("Fire2")) Shot(30, 0.5f);
}

//指定したベクトルに対してズレる最大の角度
private readonly float MAX_SHAKE = 60.0f;

//指定された発射数、ズレの割合に応じて発射するメソッド
public void Shot(int bulletNum, float bulletShake)
{
    Vector3 velocity = (Aim.transform.position - transform.position).normalized;
    for (int i = 0; i < bulletNum; i++)
    {
        //Instantiateで生成し、DisposableBulletクラスがもつInitializeメソッドを使用し、弾の初期化をしている
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
```

#### [SerializeField](https://qiita.com/makopo/items/8ef280b00f1cc18aec91)
`[SerializeField]` をつけることでprivateフィールドでもInspecter上から編集することが出来るようになる。簡単にオブジェクトをInspecter上で設定できるから便利。

#### [Instantiate](https://docs.unity3d.com/ja/current/ScriptReference/Object.Instantiate.html)
オブジェクトを複製することが出来る。頻出。

#### [Random](https://docs.unity3d.com/ja/current/ScriptReference/Random.html)
`Random.value` で0.0～1.0の乱数を返す。頻出
