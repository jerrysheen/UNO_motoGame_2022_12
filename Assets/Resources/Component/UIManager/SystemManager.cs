using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
    public interface IHomeSystem
    {
        void OnCreate();
        void OnOpen(params object[] datas);
        void OnClose();
        void OnRelease();

        void HandleMessage(string msg, params object[] datas);
    }

    /// <summary>
    /// system manager
    /// create or remove systems
    /// push systemevent to systems
    /// </summary>
    public class SystemManager : SingletonMono<SystemManager>
    {
        class MessageObject
        {
            public Type target;
            public string msg;
            public object[] args;

        }
        
        private Dictionary<Type, IHomeSystem> _systems;
        /// <summary>
        /// message cache, for the correct running order of system's init
        /// </summary>
        private Dictionary<Type, List<MessageObject>> _cacheMessages;

        protected override void Awake()
        {
            base.Awake();

            if(_systems == null)
                _systems = new Dictionary<Type, IHomeSystem>();
        }

        public SystemManager()
        {
            _systems = new Dictionary<Type, IHomeSystem>();
            _cacheMessages = new Dictionary<Type, List<MessageObject>>();
        }

        public T GetSystem<T>() where  T : IHomeSystem
        {
            if (_systems == null)
            {
                _systems = new Dictionary<Type, IHomeSystem>();
            }

            // var typename = typeof(T).ToString();
            if (_systems.TryGetValue(typeof(T), out IHomeSystem system))
            {
                return (T)system;
            }


            return default(T);
        }
        
        public T CreateSystem<T>() where  T : IHomeSystem
        {
            // var typename = typeof(T).ToString();
            //
            // return (T)CreateSystem(typename);
            
            return (T)CreateSystem(typeof(T));
        }

        private IHomeSystem CreateSystem(Type type)
        {
            if (_systems.ContainsKey(type))
            {
                Debug.LogWarning("already have system in manager");
            }
            
            // Type type = Type.GetType(sysname);
            IHomeSystem system = null;
            if (type != null)
            {
                var syscomp = this.gameObject.AddComponent(type);

                system = syscomp as IHomeSystem;
            }
            
            _systems[type] = system;
            
            system.OnCreate();

            return system;
        }

        public T OpenSystem<T>(params object[] datas) where T : IHomeSystem
        {
            // var typename = typeof(T).ToString();
            var type = typeof(T);
            var system = GetSystem<T>();
            if (system == null)
            {
                system = CreateSystem<T>();
            }
            
            system.OnOpen(datas);
            
            if (_cacheMessages.ContainsKey(type))
            {
                List<MessageObject> list = _cacheMessages[type];
                for (int i = 0; i < list.Count; i++)
                {
                    MessageObject msgobj = list[i];
                    system.HandleMessage(msgobj.msg, msgobj.args);
                }
                _cacheMessages.Remove(type);
            }

            return system;
        }
        
        public void CloseSystem<T>() where T : IHomeSystem
        {
            var typename = typeof(T).ToString();
            var system = GetSystem<T>();
            if (system != null)
            {
                system.OnClose();
            }
        }

        public void ReleaseSystem<T>() where T : MonoBehaviour, IHomeSystem
        {
            // var typename = typeof(T).ToString();
            var type = typeof(T);
            var system = GetSystem<T>();
            if (system != null)
            {
                var sysname = system.GetType().ToString();
                Debug.Log("release " + sysname);
                if (_systems.ContainsKey(type))
                {
                    _systems.Remove(type);
                    system.OnClose();
                    system.OnRelease();

                    GameObject.Destroy(system.gameObject);
                }
            }
        }

        
        public void ReleaseAll()
        {
            _cacheMessages.Clear();

            foreach (var system in _systems)
            {
                system.Value.OnClose();
                system.Value.OnRelease();
                //
                // GameObject.Destroy(system);
            }
            _systems.Clear();
        }
        //--------------------------
        //event
        public void SendMessage<T>(string msg, params object[] datas)
        {
            // var target = typeof(T).ToString();
            var target = typeof(T);
            if(_systems.ContainsKey(target))
            {
                _systems[target].HandleMessage(msg, datas);
            }
            else
            {
                var cache = GetCacheMessageList(target);
                var m = new MessageObject();
                m.msg = msg;
                m.target = target;
                m.args = datas;
                cache.Add(m);
            }
        }
        private List<MessageObject> GetCacheMessageList(Type type)
        {
            List<MessageObject> list = null;
            if (!_cacheMessages.ContainsKey(type))
            {
                list = new List<MessageObject>();
                _cacheMessages.Add(type, list);
            }
            else
            {
                list = _cacheMessages[type];
            }
            return list;
        }
    }
}

