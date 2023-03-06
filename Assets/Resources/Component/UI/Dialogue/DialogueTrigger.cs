using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace UIDialogue
{
	public class DialogueTrigger : MonoBehaviour {

		public SingleDialogue dialogue;

		public void TriggerDialogue ()
		{
			// FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
			// system.playdialogue(dialogue....)
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.CompareTag("Player")) return;
			Debug.Log("Enter " + this.ToString());
			UIManager.getInstance.Open<UIDialoguePanel>(dialogue);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Player")) return;
			Debug.Log("Leave " + this.ToString());
			UIManager.getInstance.Close<UIDialoguePanel>();
		}
	}
}

