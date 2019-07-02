## DisposableBulletの解説
このクラスでは弾の移動、当たったときの処理を行っている。

## 弾の初期設定・破棄
生成・破棄に関するソースコードは以下の部分。
```cs
/// <param name="attackerType">弾を発射したキャラの所属</param>
/// <param name="position">発射時の発射キャラの位置</param>
/// <param name="velocity">弾の移動ベクトル</param>
/// <param name="data">弾の基本情報</param>
public void Initialize(AttackerType attackerType, Vector3 position, Vector3 velocity, BulletData data)
{
    AttackerType = attackerType;
    transform.position = position;
    Velocity = velocity * data.Speed;
    InitializeTime = Time.time;
    Data = data;
}

void ResetObject()
{
    //このgameObjectを破棄する
    Destroy(gameObject);
}
```
#### [Destroy](https://docs.unity3d.com/jp/current/ScriptReference/Object.Destroy.html)
このオブジェクトを破棄するには `Destory(gameObject)` を呼べばよい。`Destroy(this)` を使用すると `DisposableBulletコンポーネント` のみが破棄され、オブジェクト自体は破棄されないので注意。

## 弾の移動
弾の移動に関するソースコードは以下の部分。
```cs
//初期設定で設定したvelocityを毎フレーム加算する
Transform.localPosition += Velocity * Time.fixedDeltaTime;

//現在時刻と生成時刻の差分を出し、それが弾の有効時間を超過していた場合破棄する
if (Time.time - InitializeTime > Data.ValidTime) ResetObject();
```

#### [Time.fixedDeltaTime](https://docs.unity3d.com/ja/current/ScriptReference/Time-fixedDeltaTime.html)
固定フレームレートの更新を実行するインターバルを取得できる。

#### [Time.time](https://docs.unity3d.com/ja/current/ScriptReference/Time-time.html)
ゲーム開始からの時間を取得できる。今回のように初期化時刻をあらかじめセットしておき、それとの差分をとることでタイマーのように使用することが出来る。

## 当たったときの処理
```cs
void OnTriggerEnter(Collider other)
{
    var enemy = other.GetComponent<IDamageApplicable>();
    if (enemy == null || enemy.GetAttackerType() == AttackerType) return;

    //弾の当たった場所から当たったキャラへの単位ベクトル（ノックバックに使おうと思ったが未実装）
    var direction = (transform.position - other.transform.position).normalized;

    //ダメージ処理の際に必要なデータを一つの構造体にまとめる
    var damage = new DamageData(this, Data.Damage, direction);
    other.GetComponent<IDamageApplicable>().ApplyDamage(damage);
}
```

#### [OnTriggerEnter](https://docs.unity3d.com/ja/current/ScriptReference/Collider.OnTriggerEnter.html)
この名前のメソッドに処理を記述するとUnity側で呼んでくれる。引数のColliderには当たったColliderが渡される。今回は渡されたColliderをもつオブジェクトが `IDamageApplicable(ダメージを受けるinterface)` を実装していて、なおかつ弾を撃ったキャラと違う勢力に所属していた場合にダメージの処理が行われる。なので、弾同士が接触した場合もこのメソッドは呼ばれるがダメージの処理は行われない。（なぜなら、このClassはIdamageApplicableを実装していないから）
