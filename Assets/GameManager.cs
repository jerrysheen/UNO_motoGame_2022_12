using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public Action<string> OnStoryLineChange;

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

        protected override void Awake()
        {
            base.Awake();
            // maybe later will change
            score = 0;
            currGuideProcedure = GuideProcedure.Conversation0;
            receivedInputUp = false;
            receivedInputDown = false;
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

        public void GoToNextStoryLine(string name)
        {
            if(OnStoryLineChange != null)
            OnStoryLineChange(name);
        }

        void GuideProcedureChange() 
        { 
            switch(currGuideProcedure) 
            {
                case GuideProcedure.MotoMoveControl:
                    StartCoroutine(ListenToMotoControl());
                    break;
            }
        
        }


        IEnumerator ListenToMotoControl() 
        {
            while (!receivedInputDown && !receivedInputUp) 
            {
                Debug.Log("Listen");
                Debug.Log(Input.GetAxis("Vertical"));
                yield return null;
            }
        
        }





    }

}