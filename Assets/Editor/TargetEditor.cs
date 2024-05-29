using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Target))]
public class TargetEditor : Editor
{
    void OnSceneGUI()
    {
        Target config = (Target)target;

        Vector3 direction2D = new Vector3(config.VelocityDir.x, config.VelocityDir.y,0).normalized;
        float radius = 0.5f;
        float angle = config.angle;

        // 计算扇形的开始方向
        Vector3 startDirection = Quaternion.Euler(0, 0,-angle / 2) * direction2D;

        // 绘制扇形
        Handles.color = Color.yellow-new Color(0,0,0,0.7f);
        Handles.DrawSolidArc(config.transform.position, Vector3.forward, startDirection, angle, radius);
    }


}
