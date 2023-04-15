using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoints))]
public class WayPointEditor : Editor
{
    //OnEnable, OnDisavle, OnInspector, OnSceneGUI
    private void OnSceneGUI()
    {
        WayPoints _wayPoint = target as WayPoints;
        Transform _trans = _wayPoint.transform;
        List<Vector3> _list = _wayPoint.localList;
        Vector3 _worldPoint;
        for (int i = 0, imax = _list.Count; i < imax; i++)
        {
            _worldPoint = _trans.TransformPoint(_list[i]);
            EditorGUI.BeginChangeCheck();
            _worldPoint = Handles.DoPositionHandle(_worldPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_wayPoint, "WayPoint");
                EditorUtility.SetDirty(_wayPoint);
                _list[i] = _trans.InverseTransformPoint(_worldPoint);
            }
        }

    }
}