using System.Collections.Generic;
using UnityEngine;


namespace SkillSetup
{
    [CreateAssetMenu (menuName = "SkillSetup/Player/Create Skill")]

    public class Skills : ScriptableObject
    {
        public string Description;
        public Sprite Icon;
        public int LevelNeeded;
        public int XpNeeded;

        public List<PlayerAttributes> AffectedAttributes = new List<PlayerAttributes>();



    }
}
