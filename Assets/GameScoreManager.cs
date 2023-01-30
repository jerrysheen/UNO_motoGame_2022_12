using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{

public class GameScoreManager :  SingletonMono<GameScoreManager>
{
        //public static event Action<int> onGameStateChanged;
        public float score = 0;
        protected override void Awake()
        {
            base.Awake();
            // maybe later will change
            score = 0;
        }

        public void ColliderWithSomeThing(CollectableItemType type, float value)
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

        private void DealWithCoin(float value)
        {
            score += value;
        }
        
        private void DealWithMine(float value)
        {
            
        }

}

}