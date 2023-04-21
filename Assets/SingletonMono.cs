using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public abstract class SingletonMono<T>: MonoBehaviour where T: SingletonMono<T> {
		
        private static T _instance = null;
		
        public static T getInstance {
            get {
                return _instance;
            }
        }
		
        protected virtual void Awake() {
            if (_instance != null)
            {
                Debug.LogError(name + "error: already initialized", this);
                DestoryDuplicated();
                Destroy(this);
            }
            _instance = (T)this;
        }

        protected virtual void DestoryDuplicated()
        {
            
        }

        protected void OnDestroy()
        {
            _instance = null;
        }
    }
}