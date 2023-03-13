using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UI;
using UnityEditor;
using UnityEngine;

namespace UIDialogue
{
	public class DialogueTrigger : MonoBehaviour {

		public DialogueMap dialogueMapData;

		public void TriggerDialogue ()
		{
			// FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
			// system.playdialogue(dialogue....)
		}

		public string testDataName; 

		// private void OnTriggerEnter2D(Collider2D other)
		// {
		// 	if (!other.CompareTag("Player")) return;
		// 	Debug.Log("Enter " + this.ToString());
		// 	UIManager.getInstance.Open<UIDialoguePanel>(dialogue);
		// }
		//
		// private void OnTriggerExit2D(Collider2D other)
		// {
		// 	if (!other.CompareTag("Player")) return;
		// 	Debug.Log("Leave " + this.ToString());
		// 	UIManager.getInstance.Close<UIDialoguePanel>();
		// }

		public void PlayDialogue(string dialogueName)
		{
			var singleDialogue = dialogueMapData.mapData.Find(x => x.name == dialogueName);
			if (singleDialogue == null) return;
			UIManager.getInstance.Open<UIDialoguePanel>(singleDialogue.singleDialogueData);
		}
	}
	
#if UNITY_EDITOR
	[CustomEditor(typeof(DialogueTrigger))]
	public class DialogueTriggerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var script = target as DialogueTrigger;
			if (GUILayout.Button("Test Dialogue"))
			{
				script.PlayDialogue(script.testDataName);
			}
		}
	}
#endif

}

