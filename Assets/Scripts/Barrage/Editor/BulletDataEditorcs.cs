
using UnityEngine;
using UnityEditor;
using Barrage.Bullet;
using Barrage.Shot;
using Barrage;
using System.Linq;

[CustomEditor(typeof(BulletData))]
public class BulletDataEditor : Editor
{
    bool folding = true;
    int Size = -1;

    public override void OnInspectorGUI()
    {
        BulletData data = target as BulletData;

        //共通
        data.BulletType = (BulletType)EditorGUILayout.EnumPopup("BulletType", data.BulletType);
        EditorGUILayout.Separator();

        data.Speed = Mathf.Max(0, EditorGUILayout.FloatField("Speed", data.Speed));
        if (data.BulletType != BulletType.Funnel) data.ValidTime = Mathf.Max(0, EditorGUILayout.FloatField("ValidTime", data.ValidTime));
        else
        {
            if (data.SplitData != default && data.SplitData.ShotDatas.Any())
            {
                var validTime = data.SplitData.ShotDatas.Select(d => d.Cycle * d.Count + d.Delay).OrderByDescending(x => x).FirstOrDefault();
                data.ValidTime = validTime;
                EditorGUILayout.LabelField("ValidTime", validTime.ToString());
            }
            else EditorGUILayout.LabelField("ValidTime", "Split Data not set");
        }
        EditorGUILayout.LabelField("Range", Mathf.CeilToInt(data.Speed * data.ValidTime).ToString());
        data.Damage = Mathf.Max(1, EditorGUILayout.IntField("Damage", data.Damage));
        EditorGUILayout.Separator();


        if (data.BulletType == BulletType.Split || data.BulletType == BulletType.Funnel)
        {

            EditorGUILayout.BeginHorizontal();
            folding = EditorGUILayout.Foldout(folding, "SplitData");
            data.SplitData = EditorGUILayout.ObjectField(data.SplitData, typeof(BarrageData), false) as BarrageData;
            EditorGUILayout.EndHorizontal();

            if (folding)
            {
                EditorGUI.indentLevel++;

                if (data.SplitData == default) EditorGUILayout.LabelField("Split Data not set");
                else
                {
                    if (Size == -1)
                    {
                        Initialize();
                        for (int i = data.SplitData.ShotDatas.Count - 1; i >= 0; i--)
                        {
                            if (data.SplitData.ShotDatas[i] == default && data.SplitData.BulletDatas[i] == default) BothRemoveAt(i);
                        }
                        Size = EditorGUILayout.IntField("Size", data.SplitData.ShotDatas.Count);
                    }
                    else
                    {
                        for (int i = data.SplitData.ShotDatas.Count - 1; i >= Size; i--)
                        {
                            BothRemoveAt(i);
                        }
                        Size = EditorGUILayout.IntField("Size", Size);
                    }
                    // リスト表示
                    EditorGUILayout.LabelField("ShotData / BulletData");
                    for (int i = 0; i < Size; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (i >= data.SplitData.ShotDatas.Count || i >= data.SplitData.BulletDatas.Count) BothAdd();
                        if (data.SplitData.ShotDatas[i] != default) data.SplitData.ShotDatas[i] = EditorGUILayout.ObjectField(data.SplitData.ShotDatas[i], typeof(ShotData), false, GUILayout.MinWidth(100)) as ShotData;
                        else data.SplitData.ShotDatas[i] = data.SplitData.ShotDatas[i] = EditorGUILayout.ObjectField(default, typeof(ShotData), false, GUILayout.MinWidth(100)) as ShotData;

                        if (data.SplitData.BulletDatas[i] != default) data.SplitData.BulletDatas[i] = EditorGUILayout.ObjectField(data.SplitData.BulletDatas[i], typeof(BulletData), false, GUILayout.MinWidth(100)) as BulletData;
                        else data.SplitData.BulletDatas[i] = EditorGUILayout.ObjectField(default, typeof(BulletData), false, GUILayout.MinWidth(100)) as BulletData;

                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();
        }

        if (data.BulletType == BulletType.Funnel)
        {
            data.DeploymentDistance = Mathf.Max(0, EditorGUILayout.FloatField("DeploymentDistance", data.DeploymentDistance));
            data.DeploymentTime = Mathf.Max(0, EditorGUILayout.FloatField("DeploymentTime", data.DeploymentTime));
        }

        if (data.BulletType == BulletType.Curve || data.BulletType == BulletType.Funnel)
        {
            data.AngularSpeed = Mathf.Max(0, EditorGUILayout.FloatField("AngularSpeed", data.AngularSpeed));
            data.Axis = EditorGUILayout.Vector3Field("Axis", data.Axis);
        }

        else if (data.BulletType == BulletType.Chase)
        {
            data.ChasePower = Mathf.Max(0, EditorGUILayout.FloatField("ChasePower", data.ChasePower));
            data.ChaseEnableTime = Mathf.Max(0, EditorGUILayout.FloatField("ChaseEnableTime", data.ChaseEnableTime));
        }

        EditorUtility.SetDirty(target);

        void Initialize()
        {
            if (data.SplitData != default)
            {
                while (data.SplitData.BulletDatas.Count > data.SplitData.ShotDatas.Count) data.SplitData.ShotDatas.Add(default);
                while (data.SplitData.BulletDatas.Count < data.SplitData.ShotDatas.Count) data.SplitData.BulletDatas.Add(default);
            }
        }

        void BothAdd()
        {
            data.SplitData.ShotDatas.Add(default); data.SplitData.BulletDatas.Add(default);
        }
        void BothRemoveAt(int num)
        {
            data.SplitData.ShotDatas.RemoveAt(num); data.SplitData.BulletDatas.RemoveAt(num);
        }
    }

}