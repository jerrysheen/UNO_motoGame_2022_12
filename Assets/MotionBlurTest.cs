using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MotionBlurTest : MonoBehaviour
{
    public bool needAccerrate;
    public bool needAnimatorSpeed;
    public float maxSpeed = 200.0f;
    public float currSpeed;
    public float accerlerateDuration = 2.5f;
    public float animatorDuration = 3.5f;
    public float camswitchTime = 0.7f;

    public GameObject virtualCam00;
    public GameObject virtualCam01;
    public GameObject globalVolume;

    public Animator wheelAnimator;
    public float animatorMaxPlaySpeed = 1.5f;
    public float animatorCurrSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        needAccerrate = false;
        currSpeed = 0.0f;
        wheelAnimator = GetComponent<Animator>();
        animatorCurrSpeed = 1.0f;
        globalVolume = GameObject.Find("GlobalVolume");
        globalVolume.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.right * Time.deltaTime * 18.0f;
        if (needAccerrate) 
        {
            this.transform.position = this.transform.position + currSpeed * Time.deltaTime * Vector3.right;
        } 

        if (needAnimatorSpeed) 
        {
            wheelAnimator.speed = animatorCurrSpeed;
        }
       
    }

    public void Accerlerate()
    {
        StartCoroutine(AccerlerateDuration(accerlerateDuration));
        StartCoroutine(AnimatorSppedUpDuration(animatorDuration));
    }

    IEnumerator AccerlerateDuration(float time) 
    {
        globalVolume.SetActive(true);
        needAccerrate = true;
        virtualCam01.SetActive(true);
        while (time > 0) 
        {
            // speed up :
            if (time > accerlerateDuration / 2.0f)
            {
                currSpeed += (maxSpeed * Time.deltaTime / (0.5f * accerlerateDuration));
            }
            // slowdown:
            else 
            {
                currSpeed -= (maxSpeed * Time.deltaTime / (0.5f * accerlerateDuration));
            }
            time -= Time.deltaTime;
            Debug.Log(time);
            if (time < camswitchTime && virtualCam01.activeSelf) 
            {
                virtualCam01.SetActive(false);
            }

            yield return null;
        }
        needAccerrate = false;
        currSpeed = 0.0f;
        globalVolume.SetActive(false);
    }

    IEnumerator AnimatorSppedUpDuration(float time)
    {

        needAnimatorSpeed = true;
        while (time > 0)
        {
            // speed up :
            if (time > animatorDuration / 2.0f)
            {
                animatorCurrSpeed += (animatorMaxPlaySpeed * Time.deltaTime / (0.5f * animatorDuration));
            }
            // slowdown:
            else
            {
                animatorCurrSpeed -= (animatorMaxPlaySpeed * Time.deltaTime / (0.5f * animatorDuration));
            }
            time -= Time.deltaTime;
            Debug.Log(time);

            yield return null;
        }
        animatorCurrSpeed = 1.0f;
        needAnimatorSpeed = false;
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
            script.Accerlerate();
        }
    }
}

#endif
