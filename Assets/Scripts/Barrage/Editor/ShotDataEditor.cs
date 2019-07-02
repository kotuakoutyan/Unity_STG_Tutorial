using UnityEngine;
using UnityEditor;
using Barrage.Shot;

[CustomEditor(typeof(ShotData))]
public class ShotDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        ShotData data = target as ShotData;

        //共通
        data.ShotType = (ShotType)EditorGUILayout.EnumPopup("ShotType", data.ShotType);


        if (data.ShotType == ShotType.AllRange || data.ShotType == ShotType.Gather)
        {
            data.AllRangeShotShape = (AllRangeShotShape)EditorGUILayout.EnumPopup("AllRangeShotShape", data.AllRangeShotShape);
            if(data.AllRangeShotShape == AllRangeShotShape.Random || data.AllRangeShotShape == AllRangeShotShape.Plane)
            {
                data.BulletNum = Mathf.Max(1, EditorGUILayout.IntField("BulletNum", data.BulletNum));
            }
        }
        else data.BulletNum = Mathf.Max(1, EditorGUILayout.IntField("BulletNum", data.BulletNum));
        EditorGUILayout.Separator();

        if(data.ShotType == ShotType.Normal)
        {
            data.RandomDiffusion = EditorGUILayout.Toggle("RandomDiffusion", data.RandomDiffusion);
            data.BulletShake = EditorGUILayout.Slider("BulletShake", data.BulletShake, 0.0f, 1.0f);
            EditorGUILayout.Separator();
        }

        if(data.ShotType == ShotType.Gather)
        {
            data.GatherAtTarget = EditorGUILayout.Toggle("GatherAtTarget", data.GatherAtTarget);
            data.PassGatherPoint = EditorGUILayout.Toggle("PassGatherPoint", data.PassGatherPoint);
            if(data.PassGatherPoint) data.DistanceFromGatherPoint = Mathf.Max(0, EditorGUILayout.FloatField("DistanceFromGatherPoint", data.DistanceFromGatherPoint));
            EditorGUILayout.Separator();
        }
        
        data.Count = Mathf.Max(1, EditorGUILayout.IntField("Count", data.Count));
        
        if (data.Count > 1)
        {
            data.Cycle = Mathf.Max(0, EditorGUILayout.FloatField("Cycle", data.Cycle));
            if (data.AllRangeShotShape != AllRangeShotShape.Random)
            {
                data.AngularSpeed = Mathf.Max(0, EditorGUILayout.FloatField("AngularSpeed", data.AngularSpeed));
                data.Axis = EditorGUILayout.Vector3Field("Axis", data.Axis);
            }
        }

        EditorUtility.SetDirty(target);
    }

}