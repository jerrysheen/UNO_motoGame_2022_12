using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIDialogue
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueMap", order = 2)]
    public class DialogueMap : ScriptableObject
    {
        public List<DialogueData> mapData;

    }

    [Serializable]
    public class DialogueData
    {
        public string name;
        public SingleDialogueObject singleDialogueData;
    }

}
