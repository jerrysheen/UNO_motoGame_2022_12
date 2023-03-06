using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIDialogue
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SingleDialogue", order = 1)]
    public class SingleDialogue : ScriptableObject
    {

        public string npcNameA;
        public string npcNameB;
        public Sprite characterASprite;
        public Sprite characterBSprite;

        public List<Sentence> conversations;

    }

    [Serializable]
    public class Sentence
    {
        public string words;
        public Roles owners;
    }

    public enum Roles
    {
        RoleA, RoleB
    }
}
