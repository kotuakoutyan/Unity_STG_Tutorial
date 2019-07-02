
using UnityEngine;
using UnityEditor;
using Barrage.Shot;
using Barrage;
using Barrage.Bullet;

[CustomEditor(typeof(BarrageData))]
public class BarrageDataEditor : Editor
{
    bool folding = true;
    int Size = -1;
    
    public override void OnInspectorGUI()
    {
        BarrageData data = target as BarrageData;

        if (folding = EditorGUILayout.Foldout(folding, "BarrageData"))
        {
            EditorGUI.indentLevel++;
            if (Size == -1)
            {
                Initialize();
                for (int i = data.ShotDatas.Count - 1; i >= 0; i--)
                {
                    if (data.ShotDatas[i] == default && data.BulletDatas[i] == default) BothRemoveAt(i);
                }
                Size = EditorGUILayout.IntField("Size", data.ShotDatas.Count);
            }
            else
            {
                Size = EditorGUILayout.IntField("Size", Size);
                for (int i = data.ShotDatas.Count - 1; i >= Size; i--)
                {
                    BothRemoveAt(i);
                }
            }
            // リスト表示
            EditorGUILayout.LabelField("ShotData / BulletData");
            for (int i = 0; i < Size; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (i >= data.ShotDatas.Count || i >= data.BulletDatas.Count) BothAdd();
                if (data.ShotDatas[i] != default) data.ShotDatas[i] = EditorGUILayout.ObjectField(data.ShotDatas[i], typeof(ShotData), false, GUILayout.MinWidth(100)) as ShotData;
                else data.ShotDatas[i] = data.ShotDatas[i] = EditorGUILayout.ObjectField(default, typeof(ShotData), false, GUILayout.MinWidth(100)) as ShotData;

                if (data.BulletDatas[i] != default) data.BulletDatas[i] = EditorGUILayout.ObjectField(data.BulletDatas[i], typeof(BulletData), false, GUILayout.MinWidth(100)) as BulletData;
                else data.BulletDatas[i] = EditorGUILayout.ObjectField(default, typeof(BulletData), false, GUILayout.MinWidth(100)) as BulletData;


                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        EditorUtility.SetDirty(target);

        void Initialize()
        {
            while (data.BulletDatas.Count > data.ShotDatas.Count) data.ShotDatas.Add(default);
            while (data.BulletDatas.Count < data.ShotDatas.Count) data.BulletDatas.Add(default);
        }

        void BothAdd()
        {
            data.ShotDatas.Add(default);data.BulletDatas.Add(default);
        }
        void BothRemoveAt(int num)
        {
            data.ShotDatas.RemoveAt(num);
            data.BulletDatas.RemoveAt(num);
        }
    }
}