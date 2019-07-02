## AimControllerの解説
このクラスはMainAimとMainCameraを回転中心で回転させたり、その回転中心を自機に追従させたりしている。

## 回転
回転に関するソースコードは以下の部分。
``` cs
//回転の上限
public float clampAngle = 60;

void Update()
{
  AimRotate(new Vector3(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"), 0));
}

//入力に応じて回転させるメソッド
private void AimRotate(Vector3 input)
{
  var newX = transform.eulerAngles.x + input.y * sensitivity * (reverseY ? -1: 1);
  newX -= newX > 180 ? 360 : 0;
  newX = Mathf.Abs(newX) > clampAngle ? clampAngle * Mathf.Sign(newX) : newX;
  var newY = transform.eulerAngles.y + input.x * sensitivity * (reverseX ? -1 : 1);
  transform.localEulerAngles = new Vector3(newX, newY, 0);
}
```

#### [Input](https://docs.unity3d.com/ja/2017.4/ScriptReference/Input.html)
`Edit/Project Setting/Input` でユーザーが設定ができ、入力に応じて値を返す。例えばこのプロジェクトでは、下記のように設定されている.
- Fire1：左クリック、Xボタン
- Fire2：右クリック、Yボタン
- Vertical：w,sキー、左スティック
- Horizontal：a,dキー、左スティック
- Right Vertical：↑↓キー、右スティック、マウススクロール
- Right Horizontal：←→キー、右スティック、マウススクロール

#### [transform.eularAngles](https://docs.unity3d.com/ja/2017.4/ScriptReference/Transform-eulerAngles.html)
このオブジェクトが向いている方向をオイラー角を返す。今回の例では、X軸の角度を `-60°< x 60°` に収めるために使用している。

## 回転中心の追従
追従に関するソースコードは以下の部分。
``` cs
//追従するオブジェクトを取得
followTarget = GameObject.FindGameObjectWithTag("Player").transform.Find("FollowTarget").gameObject;

//追従するオブジェクトとこのオブジェクトの初期差分ベクトルを求める
offset = transform.position - followTarget.transform.position;

void Update()
{
  //回転中心を追従するオブジェクトの位置に初期差分を足したものに更新する
  transform.position = followTarget.transform.position + offset;
}
```

#### [GameObject.FindGameObjectWithTag](https://docs.unity3d.com/ja/current/ScriptReference/GameObject.FindGameObjectsWithTag.html)
tagを設定したGameObjectを取得する。今回は `Player` を取得するのに使用した。ただし、私的にはあまり使う印象はない。

#### [Transform.Find](https://docs.unity3d.com/ja/2018.1/ScriptReference/Transform.Find.html)
そのオブジェクトの子から名前の一致するものを取得する。今回はキャラクターの子である `FollowTarget` を取得している。キャラクター本体ではなくその子オブジェクトに追従する理由は、キャラクターの中心と回転中心が一致せず、ずらす必要があったから。
