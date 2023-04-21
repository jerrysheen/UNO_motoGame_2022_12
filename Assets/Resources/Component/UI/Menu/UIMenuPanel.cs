using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;
using Manager;
using UnityEngine.Rendering.UI;
using System.Linq;
using System.Drawing;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace UI
{
    public enum DialogueDisplayState
    {
        Show, Disable
    }

    public class UIMenuPanel : MonoBehaviour, IHomeUI
    {
        public RectTransform panelRoot;
        
        public GameObject rootCanvasGO;

        public GameObject menuIcon;
        public GameObject menuIcon_On;

        public GameObject pauseIcon;
        public GameObject pauseIcon_On;

        public GameObject continueIcon;
        public GameObject continueIcon_On;

        public GameObject restartIcon;
        public GameObject restartIcon_On;      
        
        public GameObject exitsIcon;
        public GameObject exitsIcon_On;


        public Animator dialogueDisplayAnim;

        public bool isMenuOn = false;
        public bool isPauseOn = false;
        public bool isContinueOn = false;
        public bool isRestartOn = false;
        public bool isExitsOn = false;

        private void Start()
        {

            if (UIManager.getInstance != null)
            {
                UIManager.getInstance.AddUI(this);
            }

            rootCanvasGO = GameObject.Find("DialogueCanvas")?.gameObject;
            panelRoot = this.transform.Find("Content")?.GetComponent<RectTransform>();
            if(panelRoot == null) Debug.LogError("Please Assign Content");

            //dialogueDisplayAnim = panelRoot.GetComponent<Animator>();
            GameObject menu = panelRoot.transform.Find("Menu").gameObject;
            menuIcon = menu.transform.Find("MenuIcon")?.gameObject;
            menuIcon_On = menu.transform.Find("MenuIcon_On")?.gameObject;
            GameObject pause = panelRoot.transform.Find("Pause").gameObject;
            pauseIcon = pause.transform.Find("PauseIcon")?.gameObject;
            pauseIcon_On = pause.transform.Find("PauseIcon_On")?.gameObject;
            GameObject continueGo = panelRoot.transform.Find("Continue").gameObject;
            continueIcon = continueGo.transform.Find("ContinueIcon")?.gameObject;
            continueIcon_On = continueGo.transform.Find("ContinueIcon_On")?.gameObject;
            GameObject restart = panelRoot.transform.Find("Restart").gameObject;
            restartIcon = restart.transform.Find("RestartIcon")?.gameObject;
            restartIcon_On = restart.transform.Find("RestartIcon_On")?.gameObject;
            GameObject exits = panelRoot.transform.Find("Exits").gameObject;
            exitsIcon = exits.transform.Find("ExitsIcon")?.gameObject;
            exitsIcon_On = exits.transform.Find("ExitsIcon_On")?.gameObject;

            //if (rootCanvasGO == null)rootCanvasGO.SetActive(false);

            menuIcon_On.SetActive(false);

            pauseIcon.SetActive(false);
            pauseIcon_On.SetActive(false);

            continueIcon.SetActive(false);
            continueIcon_On.SetActive(false);

            restartIcon.SetActive(false); 
            restartIcon_On.SetActive(false);

            exitsIcon.SetActive(false);
            exitsIcon_On.SetActive(false);
            Hide();

            isMenuOn = false;
            isPauseOn = false;
            isContinueOn = false;
            isRestartOn = false;
            isExitsOn = false;
        }

        private void OnDestroy()
        {
            if(UIManager.getInstance != null)
            UIManager.getInstance.RemoveUI(this);
        }

        
        public void EnterBtnClick()
        {
            // enter quest
            // Debug.Log("<color=green>ENTER QUEST!!!</color>");
            //
            // Debug.LogFormat("<color=green>guid {0}</color>", currentBuildingCtrl.GUID);
            //
            // HomeSystemManager.getInstance.GetSystem<WildMissionManager>()?.EnterLymphMission(currentBuildingCtrl.GUID, currentBuildingCtrl.gameObject.GetComponent<LymphNodeComponent>().lymphIndex);
            //
            // //hide ui
            // Hide();
        }
        
         public void CancleBtnClick()
         {
             // //hide ui
             // Hide();
         }       
        //-------------------------------------------------
        public void OnOpen(params object[] datas)
        {   
            // Show();
            // 打开的时候， 根据ScriptableObject来读取数据,
            // todo: 数据类型检测？
            Show(datas);
        }

        public void OnClose()
        {
            Hide();
        }

        private void Show(params object[] datas)
        {
            Debug.Log("Dialogue Open!");
            //if (rootCanvasGO.activeSelf || datas == null || datas.Length == 0) return;
            if(!rootCanvasGO.activeSelf)rootCanvasGO.SetActive(true);
            if(!panelRoot.gameObject.activeSelf) panelRoot.gameObject.SetActive(true);
            //dialogueDisplayAnim.SetBool("PlayDialogue", true);
        }

        public void Hide()
        {
            //rootCanvasGO.SetActive(false);
        }

        IEnumerator DelayDisable(float time)
        {
            yield return new WaitForSeconds(time / 2.0f);
            dialogueDisplayAnim.SetBool("PlayDialogue", false);
            yield return new WaitForSeconds(time / 2.0f);
            panelRoot.gameObject.SetActive(false);
        }

        public void MenuButtonClick()
        {
            Debug.Log("Test");
            isMenuOn = !isMenuOn;
            if (isMenuOn)
            {
                TurnOnMenu();
                menuIcon.SetActive(false);
                menuIcon_On.SetActive(true);
            }
            else 
            {
                TurnOffMenu();
                menuIcon.SetActive(true);
                menuIcon_On.SetActive(false);
            }
        
        }

        private void TurnOnMenu() 
        {
            PlayMenuTurnOnEffect();

            pauseIcon.SetActive(!isPauseOn);
            pauseIcon_On.SetActive(isPauseOn);
            continueIcon.SetActive(true);
            continueIcon_On.SetActive(false);
            restartIcon.SetActive(true);
            restartIcon_On.SetActive(false);
            exitsIcon.SetActive(true);
            exitsIcon_On.SetActive(false);

        }

        private void TurnOffMenu() 
        {
            PlayMenuTyrnOffEffect();
            pauseIcon.SetActive(false);
            pauseIcon_On.SetActive(false);
            continueIcon.SetActive(false);
            continueIcon_On.SetActive(false);
            restartIcon.SetActive(false);
            restartIcon_On.SetActive(false);
            exitsIcon.SetActive(false);
            exitsIcon_On.SetActive(false);
        }

        private void PlayMenuTurnOnEffect() { }
        private void PlayMenuTyrnOffEffect() { }


        public void ContinueButtonClick()
        {
            if (!isPauseOn) return;
            isPauseOn = false;
            Debug.Log("Test");
            pauseIcon.SetActive(true);
            pauseIcon_On.SetActive(false);
            continueIcon.SetActive(false);
            continueIcon_On.SetActive(true);
            Time.timeScale = 1.0f;
        }

        public void PauseButtonClick()
        {
            isPauseOn = true;
            Debug.Log("Test");
            pauseIcon.SetActive(false);
            pauseIcon_On.SetActive(true);
            continueIcon.SetActive(true);
            continueIcon_On.SetActive(false);
            Time.timeScale = 0.0f;

        }

        public void RestartButtonClick()
        {
            Debug.Log("Test");
            SceneManager.LoadScene("CodeTest 1");
            StartCoroutine(ChangeProcessToTutorialFinished(1.5f));
        }

        IEnumerator ChangeProcessToTutorialFinished(float time)
        {
            yield return new WaitForSeconds(1.5f);
            Debug.Log("Test this process not being destroy");
        }

        public void ExitsButtonClick()
        {
            Debug.Log("Test");

            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
        public AudioClip buttonClickMusic;

        public void PlayButtonClickMusic()
        {
            var Go = GameObject.Find("OtherSound");
            if (!Go) return;
            AudioSource tempSource = Go.GetComponent<AudioSource>();
            tempSource.clip = buttonClickMusic;
            tempSource.loop = false;
            tempSource.Play();
            
        }
    }
}
