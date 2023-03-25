using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{

public class GameManager :  SingletonMono<GameManager>
{
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

}

}