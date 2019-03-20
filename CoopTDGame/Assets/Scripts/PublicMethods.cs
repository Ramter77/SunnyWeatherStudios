using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSetup
{
    [System.Serializable]
    public class PlayerSkillSets
    {
        public Skills skill;
        public int amount;

        public PlayerSkillSets(Skills skill, int amount)
        {
            this.skill = skill;
            this.amount = amount;
        }
    }
}



