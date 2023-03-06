using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

namespace UI
{
    public interface IHomeUI
    {
        /// <summary>
        /// OnOpen 为打开这个UI的时候做的操作
        /// </summary>
        /// <param name="datas"></param>
        void OnOpen(params object[] datas);

        /// <summary>
        /// OnClose 为关闭这个UI时候做的操作
        /// </summary>
        /// <param name="datas"></param>
        void OnClose();
    }

    public class UIManager : SingletonMono<UIManager>
    {
        private Dictionary<string, IHomeUI> _uiList = new Dictionary<string, IHomeUI>();

        public void AddUI(IHomeUI ui)
        {
            var uiname = ui.GetType().ToString();

            if (!_uiList.ContainsKey(uiname))
            {
                _uiList.Add(uiname, ui);
            }
        }

        public void RemoveUI(IHomeUI ui)
        {
            var uiname = ui.GetType().ToString();
            
            if (_uiList.ContainsKey(uiname))
            {
                _uiList.Remove(uiname);
            }
        }

        public void Open<T>(params object[] datas) where T : IHomeUI
        {
            var uiname = typeof(T).ToString();
            
            if (_uiList.TryGetValue(uiname, out IHomeUI ui))
            {
                ui.OnOpen(datas);
            }
        }
        
        public void Close<T>() where T : IHomeUI
        {
            var uiname = typeof(T).ToString();
            
            if (_uiList.TryGetValue(uiname, out IHomeUI ui))
            {
                ui.OnClose();
            }
        }

        public void Clear()
        {
            if (_uiList != null)
            {
                _uiList.Clear();
            }
        }
    }
}