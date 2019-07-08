## PlayerRotateの解説
このクラスではキャラクターの回転を行っている。仕様としてはmainAimObjの方向を向き続けるもの。

```cs
private void RotatePlayer()
{
  //目標のQuaternionを取得する
  var targetRotate = Quaternion.LookRotation(mainAimObj.transform.position - transform.position);

  //現在のQuaternionから目標のQuaternionまでの補間を行い、滑らかに回転させる
  var newRotate = Quaternion.Lerp(transform.rotation, targetRotate, rotateSpeed * Time.deltaTime).eulerAngles;

  //constrainは有効(1)か無効(0)かを決めた自作クラス
  transform.eulerAngles = new Vector3(
      newRotate.x * constrainX.value,
      newRotate.y * constrainY.value,
      newRotate.z * constrainZ.value
  );
}
```

#### [Quaternion.LookRotation](https://docs.unity3d.com/ja/current/ScriptReference/Quaternion.LookRotation.html)
今回のように `Quaternion.LookRotation(TargetPosition - ThisPosition)` でTargetの方向を向かせることが出来る。第二引数を指定しない場合、オブジェクトのｙベクトルが上を向くように回転する。

#### [Quaternion.Lerp](https://docs.unity3d.com/ja/2018.2/ScriptReference/Quaternion.Lerp.html)
Quaternionを補間するメソッド。目標まで少しずつ回転させたいときに使用する。
