using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SkillSetup
{
    [CreateAssetMenu (menuName = "SkillSetp/Player/Create Skill")]

    public class Skills : ScriptableObject
    {
        public string Description;
        public Sprite Thumnail;
    }
}
