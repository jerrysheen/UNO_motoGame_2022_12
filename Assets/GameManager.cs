using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UIDialogue;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Manager
{

public class GameManager :  SingletonMono<GameManager>
{

        public enum GuideProcedure 
        {

            Conversation0,
            MotoMoveControl,
            Joking,
            CollectingOrange,
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
        protected override void Awake()
        {
            base.Awake();
            // maybe later will change
            score = 0;
            currGuideProcedure = GuideProcedure.Conversation0;
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

        private void Start()
        {

            //StartCoroutine(WaitUIPanelInit());
            AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAsset<GameObject>("Coin");

            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                CoinPrefab = opHandle.Result;
            }
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
                case GuideProcedure.MotoMoveControl:
                    Debug.Log("Switch to moto ctrl");
                    // 先播放对话，再控制检测
                    var singleDialogue = dialogueMapData.mapData.Find(x => x.name == "CutScene01");
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
            GameObject coins = Instantiate(CoinPrefab);
            coins.transform.position = player.transform.position;
            while (!finishedCollectingCoins)
            {
                Debug.Log("Collecting Coins...!!");
                
                yield return null;
            }
            
            //SetGuideProcedure(GuideProcedure.Joking);
        }





    }

}