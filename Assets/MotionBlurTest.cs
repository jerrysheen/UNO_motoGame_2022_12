using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MotionBlurTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position + Time.deltaTime * Vector3.right * 13f;
    }

    public void Accelerate()
    {
        
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MotionBlurTest))]
public class MotionBlurTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("test"))
        {
            var script = target as MotionBlurTest;
            script.Accelerate();
        }
    }
}

#endif
