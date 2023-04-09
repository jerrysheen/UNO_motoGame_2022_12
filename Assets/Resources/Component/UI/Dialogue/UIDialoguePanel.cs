using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;
using Manager;

namespace UIDialogue
{
    public enum DialogueDisplayState
    {
        Show, Disable
    }

    public class UIDialoguePanel : MonoBehaviour, IHomeUI
    {
        
        public RectTransform panelRoot;
        
        public GameObject rootCanvasGO;
        public Image characterAvatarA;
        public String characterNameA;
        
        
        public Image characterAvatarB;
        public String characterNameB;

        public Text npcName;
        public Text conversation;

        private SingleDialogueObject currDialogue;

        public Animator dialogueDisplayAnim;
        private DialogueDisplayState m_displayState;

        private Queue<Sentence> m_SentencesQueue;

        public float displaySentenceSpeed = 0.1f;
        private void Start()
        {

            if (UIManager.getInstance != null)
            {
                UIManager.getInstance.AddUI(this);
            }

            rootCanvasGO = GameObject.Find("DialogueCanvas")?.gameObject;
            panelRoot = this.transform.Find("Content")?.GetComponent<RectTransform>();
            if(panelRoot == null) Debug.LogError("Please Assign Content");

            dialogueDisplayAnim = panelRoot.GetComponent<Animator>();
            characterAvatarA = panelRoot.transform.Find("AvatarA")?.gameObject?.GetComponent<Image>();
            characterAvatarB = panelRoot.transform.Find("AvatarB")?.gameObject?.GetComponent<Image>();
            
            npcName = panelRoot.transform.Find("NpcName")?.gameObject?.GetComponent<Text>();
            conversation = panelRoot.transform.Find("Conversation")?.gameObject?.GetComponent<Text>();
            
            if(characterAvatarA == null) Debug.LogError("Please Assign Image A");
            if(characterAvatarB == null) Debug.LogError("PleaseAssign Image B");
            if(npcName == null) Debug.LogError("PleaseAssign npcName");
            if(conversation == null) Debug.LogError("PleaseAssign conversation");
            if(rootCanvasGO == null) Debug.LogError("Can't find canvas");

            m_SentencesQueue = new Queue<Sentence>();
            if(rootCanvasGO == null)rootCanvasGO.SetActive(false);
            Hide();
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
            if (m_displayState == DialogueDisplayState.Show) return;
            Show(datas);
        }

        public void OnClose()
        {
            if (m_displayState == DialogueDisplayState.Disable) return;
            Hide();
        }

        private void Show(params object[] datas)
        {
            //if (rootCanvasGO.activeSelf || datas == null || datas.Length == 0) return;
            if(!rootCanvasGO.activeSelf)rootCanvasGO.SetActive(true);
            if(!panelRoot.gameObject.activeSelf) panelRoot.gameObject.SetActive(true);
            dialogueDisplayAnim.SetBool("PlayDialogue", true);
            currDialogue =  datas[0] as SingleDialogueObject;
            StartDialogue();
            m_displayState = DialogueDisplayState.Show;
        }

        private void Hide()
        {
            if (GameManager.getInstance && currDialogue) 
            {
                Debug.Log(GameManager.getInstance);
                Debug.Log(currDialogue);
                GameManager.getInstance.GoToNextStoryLine(currDialogue.name);
            }


            dialogueDisplayAnim.SetBool("PlayDialogue", false);
            m_displayState = DialogueDisplayState.Disable;
            StartCoroutine(DelayDisable(2.5f));
            //rootCanvasGO.SetActive(false);
        }

        IEnumerator DelayDisable(float time)
        {
            yield return new WaitForSeconds(time);
            panelRoot.gameObject.SetActive(false);
        }

        public void StartDialogue ()
        {
            // init
            foreach (var VARIABLE in currDialogue.conversations)
            {
                m_SentencesQueue.Enqueue(VARIABLE);
            }

            characterAvatarA.sprite = currDialogue.characterASprite;
            characterAvatarB.sprite = currDialogue.characterBSprite;

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (m_SentencesQueue.Count == 0)
            {
                Hide();
                return;
            }

            var currSentence = m_SentencesQueue.Dequeue();
            
            switch (currSentence.owners)
            {
                case Roles.RoleA:
                    characterAvatarA.gameObject.SetActive(true);
                    characterAvatarB.gameObject.SetActive(false);
                    npcName.text = currDialogue.npcNameA;
                    break;
                case Roles.RoleB:
                    characterAvatarA.gameObject.SetActive(false);
                    characterAvatarB.gameObject.SetActive(true);
                    npcName.text = currDialogue.npcNameB;
                    break;
            }
            string words = currSentence.words;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(words));
        }

        IEnumerator TypeSentence (string words)
        {
            conversation.text = "";
            foreach (char letter in words.ToCharArray())
            {
                conversation.text += letter; 
                yield return new WaitForSeconds(displaySentenceSpeed);
            }
        }
    }
}
