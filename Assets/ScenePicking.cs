using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Manager;
using UnityEngine;

public class ScenePicking : MonoBehaviour
{
    // Start is called before the first frame update
    
    public enum  GameScene
    {
        SceneA = 0,
        SceneB = 1,
        SceneC = 2,
        SceneD = 3,
    }


    public float unresponseTime = 0.5f;
    public float countDown = 0.0f;
    

    private GameObject rootPanel;
    private GameObject selectedObjPanel;
    private GameObject unselectedObjPanel;
    
    private GameObject CharecterA;
    private GameObject CharecterB;
    private GameObject CharecterC;
    private GameObject CharecterD;

    
    // 这里的charecter应该是白边， 
    public bool isInSelectedStage = true;
    public bool isButtonEnter = false;
    public GameScene currCharector;
    void Start()
    {
        rootPanel = transform.Find("RootPanel")?.gameObject;
        if(!rootPanel) Debug.LogError("Root Panel is not exist!");
        selectedObjPanel = rootPanel.transform.Find("Selected")?.gameObject;
        unselectedObjPanel = rootPanel.transform.Find("UnSelected")?.gameObject;
        if(!selectedObjPanel || !unselectedObjPanel) Debug.LogError("unselectedObjPanel/selectedObjPanel Panel is not exist!");
        
        // set default one;
        CharecterA = unselectedObjPanel.transform.Find("CharecterA").gameObject;
        CharecterB = unselectedObjPanel.transform.Find("CharecterB").gameObject;
        CharecterC = unselectedObjPanel.transform.Find("CharecterC").gameObject;
        CharecterD = unselectedObjPanel.transform.Find("CharecterD").gameObject;

        CharecterA.SetActive(false);
        CharecterC.SetActive(false);
        CharecterD.SetActive(false);
        currCharector = GameScene.SceneB;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInSelectedStage)
        {
            if (countDown > 0.0f)
            {
                countDown -= Time.deltaTime;
                return;
            }

            var currIndex = (int) currCharector;
            
            //if(in)
            if (Input.GetAxis("Horizontal") < 0.0f)
            {
                currIndex -= 1;
                countDown = unresponseTime;
            }
            else if (Input.GetAxis("Horizontal") > 0.0f)
            {
                currIndex += 1;
                countDown = unresponseTime;

            }

            int length = Enum.GetNames(typeof(GameScene)).Length;
            currIndex = Mathf.Max(currIndex, 0);
            currIndex = Mathf.Min(currIndex, length - 1);
            currCharector = (GameScene)currIndex;

            switch (currCharector)
            {
                case GameScene.SceneA:
                        CharecterA.SetActive(true);
                        CharecterB.SetActive(false);
                        CharecterC.SetActive(false);
                        CharecterD.SetActive(false);
                    break;
                case GameScene.SceneB:
                        CharecterA.SetActive(false);
                        CharecterB.SetActive(true);
                        CharecterC.SetActive(false);
                        CharecterD.SetActive(false);
                    break;
                case GameScene.SceneC:
                        CharecterA.SetActive(false);
                        CharecterB.SetActive(false);
                        CharecterC.SetActive(true);
                        CharecterD.SetActive(false);
                    break;
                case GameScene.SceneD:
                    CharecterA.SetActive(false);
                    CharecterB.SetActive(false);
                    CharecterC.SetActive(false);
                    CharecterD.SetActive(true);
                    break;
            }

            if (Input.GetAxis("Submit") > 0.0f && !isButtonEnter)
            {
                StartCoroutine(PlayEnterGameEffect());
                isButtonEnter = true;
            }
        }
    }

    IEnumerator PlayEnterGameEffect()
    {
        // play animation
        GameManager.getInstance.SetScene(currCharector);
        
        yield return new WaitForSeconds(0.5f);
        GameManager.getInstance.SetGuideProcedure(GameManager.GuideProcedure.SelectCharector);

        var animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetTrigger("Disapear");
        }

        //yield return new WaitForSeconds(3.0f);
        rootPanel.SetActive(false);
    }
}
