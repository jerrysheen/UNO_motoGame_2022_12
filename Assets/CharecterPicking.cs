using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Manager;
using UnityEngine;

public class CharecterPicking : MonoBehaviour
{
    // Start is called before the first frame update
    
    public enum  CHARECTER
    {
        CharecterA = 0,
        CharecterB = 1,
        CharecterC = 2,
    }


    public float unresponseTime = 0.5f;
    public float countDown = 0.0f;
    

    private GameObject rootPanel;
    private GameObject selectedObjPanel;
    private GameObject unselectedObjPanel;
    public AudioClip selectedSound;
    public AudioClip submitSound;
    
    private GameObject CharecterA;
    private GameObject CharecterB;
    private GameObject CharecterC;

    public bool isInSelectedStage = false;
    public bool isButtonEnter = false;
    public CHARECTER currCharector;
    void Start()
    {
        if(isInSelectedStage) 
        rootPanel = transform.Find("RootPanel")?.gameObject;
        if(!rootPanel) Debug.LogError("Root Panel is not exist!");
        selectedObjPanel = rootPanel.transform.Find("Selected")?.gameObject;
        unselectedObjPanel = rootPanel.transform.Find("UnSelected")?.gameObject;
        if(!selectedObjPanel || !unselectedObjPanel) Debug.LogError("unselectedObjPanel/selectedObjPanel Panel is not exist!");
        
        // set default one;
        CharecterA = selectedObjPanel.transform.Find("CharecterA").gameObject;
        CharecterB = selectedObjPanel.transform.Find("CharecterB").gameObject;
        CharecterC = selectedObjPanel.transform.Find("CharecterC").gameObject;

        CharecterA.SetActive(false);
        CharecterC.SetActive(false);
        currCharector = CHARECTER.CharecterB;

        isInSelectedStage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInSelectedStage)
        {
            // if (countDown > 0.0f)
            // {
            //     countDown -= Time.deltaTime;
            //     return;
            // }

            var currIndex = (int) currCharector;
            
            //if(in)
            if (Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxis("Horizontal") < 0.0f)
                {
                    currIndex -= 1;
                    //countDown = unresponseTime;
                }
                else if (Input.GetAxis("Horizontal") > 0.0f)
                {
                    currIndex += 1;
                    //countDown = unresponseTime;

                }
                PlayAudioEffectOnce(selectedSound);
            }



            int length = Enum.GetNames(typeof(CHARECTER)).Length;
            currIndex = Mathf.Max(currIndex, 0);
            currIndex = Mathf.Min(currIndex, length - 1);
            currCharector = (CHARECTER)currIndex;

            switch (currCharector)
            {
                case CHARECTER.CharecterA:
                        CharecterA.SetActive(true);
                        CharecterB.SetActive(false);
                        CharecterC.SetActive(false);
                    break;
                case CHARECTER.CharecterB:
                        CharecterA.SetActive(false);
                        CharecterB.SetActive(true);
                        CharecterC.SetActive(false);
                    break;
                case CHARECTER.CharecterC:
                        CharecterA.SetActive(false);
                        CharecterB.SetActive(false);
                        CharecterC.SetActive(true);
                    break;
            }

            if (Input.GetAxis("Submit") > 0.0f && !isButtonEnter)
            {
                PlayAudioEffectOnce(submitSound);
                StartCoroutine(PlayEnterGameEffect());
                isButtonEnter = true;
                isInSelectedStage = false;
            }
        }
    }

    IEnumerator PlayEnterGameEffect()
    {
        // play animation

        
        yield return null;

        var animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetTrigger("Disapear");
        }

        GameManager.getInstance.SetPlayer(currCharector);
        GameManager.getInstance.SetGuideProcedure(GameManager.GuideProcedure.Conversation0);
        yield return new WaitForSeconds(2.0f);
        rootPanel.SetActive(false);
    }
    
    public void PlayAudioEffectOnce(AudioClip currClip)
    {
        // 快速写法， 这边其实应该编写一个Instance 来继承
       
        var Go = GameObject.Find("OtherSound");
        if (!Go) return;
        AudioSource tempSource = Go.GetComponent<AudioSource>();
        tempSource.clip = currClip;
        tempSource.loop = false;
        tempSource.Play();
    }
}
