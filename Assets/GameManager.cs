using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UIDialogue;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Manager
{

public class GameManager :  SingletonMono<GameManager>
{

        public enum GuideProcedure 
        {
            SelectCharector,
            Conversation0,
            MotoMoveControl,
            Joking,
            CollectingOrange,
            Finished
        }

        public GuideProcedure currGuideProcedure;
        public Action<string> OnDialogueFinished;

        public bool receivedInputUp;
        public bool receivedInputDown;
        //public static event Action<int> onGameStateChanged;
        public Action<int> OnScoreValueChange; 
        public int score = 0;
        [Header("Move Speed")]
        [Space]
        public float cutSceneCanvasMoveSpeed = 35.0f;
        public float midCanvasMoveSpeed = 25.0f;
        public float farCanvasMoveSpeed = 15.0f;
        public float backGroundCanvasMoveSpeed = 10.0f;
        
        public DialogueMap dialogueMapData;
        private bool finishedMotoMoveControlDialogue = false;

        private GameObject CoinPrefab;
        private bool finishedCollectingCoins = false;

        public GameObject player;
        public float tutorialOrangeDistanceFromPeople = 30.0f;
        public GameObject mountPoint0;
        public GameObject mountPoint1;
        public GameObject mountPoint2;


        [Header("Prefab Gameobject")] 
        public CharecterPicking.CHARECTER currCharecter;
        public GameObject playerPrefab0;
        public GameObject playerPrefab1;
        public GameObject playerPrefab2;
                
                
        private GameObject coins0;
        private GameObject coins1;
        private GameObject coins2;

        protected override void Awake()
        {
            base.Awake();
            // maybe later will change
            score = 0;
            currGuideProcedure = GuideProcedure.SelectCharector;
            receivedInputUp = false;
            receivedInputDown = false;
            finishedMotoMoveControlDialogue = false;
            finishedCollectingCoins = false;
            
            if(!player) player = GameObject.Find("Player");
        }

        public void ColliderWithSomeThing(CollectableItemType type, int value)
        {
            switch (type)
            {
                case CollectableItemType.Coin:
                    DealWithCoin(value);
                    break;
                case CollectableItemType.Mine:
                    DealWithMine(value);
                    break;
            }
        }

        private void DealWithCoin(int value)
        {
            score += value;
            OnScoreValueChange(value);
        }
        
        private void DealWithMine(int value)
        {
            OnScoreValueChange(-value);
        }

        public void SetGuideProcedure(GuideProcedure procecure) 
        {
            currGuideProcedure = procecure;
            GuideProcedureChange();
        }

        IEnumerator Start()
        {

            //StartCoroutine(WaitUIPanelInit());
            AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>("CoinTutorial");
            yield return new WaitForSeconds(1.0f);
            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                CoinPrefab = opHandle.Result;
            }
            else 
            {
                Debug.LogError("No resource");
            }
        }

        public void SetPlayer(CharecterPicking.CHARECTER _charecter)
        {
            currCharecter = _charecter;
        }

        // IEnumerator WaitUIPanelInit()
        // {
        //     yield return null;
        //     UIDialoguePanel currPanel =  UIManager.getInstance._uiList["UIDialoguePanel"] as UIDialoguePanel;
        //     
        //     if (currPanel)
        //     {
        //         currPanel.OnDialogueFinishedPlay += DialogueFinishedCallback;
        //     }
        //     else{
        //         Debug.Log("Error: no panel UI find");
        //     }
        // }
        //
        // private void OnDestory()
        // {
        //     UIDialoguePanel currPanel =  UIManager.getInstance._uiList["UIDialoguePanel"] as UIDialoguePanel;
        //     if (currPanel)
        //     {
        //         currPanel.OnDialogueFinishedPlay -= DialogueFinishedCallback;
        //     }
        // }


        // void DialogueFinishedCallback(string name)
        // {
        //     Debug.Log("Finished dialogue : " + name);
        //     switch (name)
        //     {
        //         case "CutScene01":
        //             FinishedMotoMoveControlDialogue = true;
        //             break;
        //     }
        // }

        public void FinishedDialogue(string name)
        {
            switch (name)
            {
                
                case "CutScene00":
                    Debug.Log("Finished play CutScene 00 dialogue");
                    if (OnDialogueFinished != null)
                    {
                        OnDialogueFinished("CutScene00");
                    }
                    break;
                
                case "CutScene01":
                    Debug.Log("Finished play CutScene 01 dialogue");
                    finishedMotoMoveControlDialogue = true;
                    break;
                
                case "CutScene03":
                    Debug.Log("Finished play CutScene 03 dialogue");
                    StartCoroutine(ListenToCollecttingOrange());
                    break;
            }
        }

        void GuideProcedureChange()
        {
            switch(currGuideProcedure) 
            {
                case GuideProcedure.Conversation0:
                    var player = GameObject.Find("Player");
                    var carPart = player.transform.Find("carPart");
                    GameObject mountPoint = carPart.transform.Find("MountPoint").gameObject;
                    switch (currCharecter)
                    {
                        case CharecterPicking.CHARECTER.CharecterA:
                            GameObject tempObj = Instantiate(playerPrefab0);
                            tempObj.transform.parent = mountPoint.transform;
                            tempObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        case CharecterPicking.CHARECTER.CharecterB:
                            tempObj = Instantiate(playerPrefab1);
                            tempObj.transform.parent = mountPoint.transform;
                            tempObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        case CharecterPicking.CHARECTER.CharecterC:
                            tempObj = Instantiate(playerPrefab2);
                            tempObj.transform.parent = mountPoint.transform;
                            tempObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                    }
                    var singleDialogue = dialogueMapData.mapData.Find(x => x.name == "CutScene00");
                    if (singleDialogue == null) return;
                    UIManager.getInstance.Open<	UIDialoguePanel>(singleDialogue.singleDialogueData);
                    break;
                case GuideProcedure.MotoMoveControl:
                    Debug.Log("Switch to moto ctrl");
                    // 先播放对话，再控制检测
                    singleDialogue = dialogueMapData.mapData.Find(x => x.name == "CutScene01");
                    if (singleDialogue == null) return;
                    UIManager.getInstance.Open<UIDialoguePanel>(singleDialogue.singleDialogueData);
                    StartCoroutine(ListenToMotoControl());

                    break;
                
                case GuideProcedure.Joking:
                    Debug.Log("Switch to Joking");
                    // 
                    singleDialogue = dialogueMapData.mapData.Find(x => x.name == "CutScene02");
                    if (singleDialogue == null) return;
                    UIManager.getInstance.Open<UIDialoguePanel>(singleDialogue.singleDialogueData);
                    if (OnDialogueFinished != null)
                    {
                        OnDialogueFinished("CutScene02");
                    }
                    break;
                
                case GuideProcedure.CollectingOrange:
                    Debug.Log("Switch to Collecting");
                    // 
                    singleDialogue = dialogueMapData.mapData.Find(x => x.name == "CutScene03");
                    if (singleDialogue == null) return;
                    UIManager.getInstance.Open<UIDialoguePanel>(singleDialogue.singleDialogueData);
 
                    break;

                case GuideProcedure.Finished:
                    Debug.Log("Switch to Finished");
                    // 
                    singleDialogue = dialogueMapData.mapData.Find(x => x.name == "CutScene04");
                    if (singleDialogue == null) return;
                    UIManager.getInstance.Open<UIDialoguePanel>(singleDialogue.singleDialogueData);
                    if(coins0.activeSelf) Destroy(coins0);
                    if(coins1.activeSelf) Destroy(coins1);
                    if(coins2.activeSelf) Destroy(coins2);
                    break;
            }
        
        }


        IEnumerator ListenToMotoControl() 
        {
            while (!receivedInputDown || !receivedInputUp)
            {
                if (finishedMotoMoveControlDialogue)
                {
                    if (Input.GetAxis("Vertical") < 0.0f)
                    {
                        receivedInputDown = true;
                    }
                    if (Input.GetAxis("Vertical") > 0.0f)
                    {
                        receivedInputUp = true;
                    }
                }
                yield return null;
            }
            
            SetGuideProcedure(GuideProcedure.Joking);
        }
        
        IEnumerator ListenToCollecttingOrange()
        {
            Debug.Log("Start");
            var tempGo = GameObject.Find("CutSceneSwitcher");
            Transform parent_transform = this.transform;
            if (tempGo)
            {
                var script = tempGo.GetComponent<CutSceneSwitcher>();
                parent_transform = script.cutSceneCanvas[0].gameObject.transform;
                for (int i = 1; i < 3; i++) 
                {
                    if (parent_transform.position.x < script.cutSceneCanvas[i].gameObject.transform.position.x) 
                    {
                        parent_transform = script.cutSceneCanvas[i].gameObject.transform;
                    }
                }
            }
            else 
            {
                Debug.LogError("No CutSceneSwitcher");
            }
            coins0 = Instantiate(CoinPrefab);
            coins1 = Instantiate(CoinPrefab);
            coins2 = Instantiate(CoinPrefab);
            coins0.transform.position = new Vector3(player.transform.position.x,mountPoint0.transform.position.y ,player.transform.position.z) + Vector3.right * tutorialOrangeDistanceFromPeople;
            coins1.transform.position = new Vector3(player.transform.position.x,mountPoint1.transform.position.y ,player.transform.position.z) + Vector3.right * tutorialOrangeDistanceFromPeople;
            coins2.transform.position = new Vector3(player.transform.position.x,mountPoint2.transform.position.y ,player.transform.position.z) + Vector3.right * tutorialOrangeDistanceFromPeople;

            coins0.transform.parent = parent_transform;
            coins1.transform.parent = parent_transform;
            coins2.transform.parent = parent_transform;
            while (!finishedCollectingCoins)
            {
                Debug.Log("Collecting Coins...!!");
                
                yield return null;
            }
            
            //SetGuideProcedure(GuideProcedure.Joking);
        }





    }

}