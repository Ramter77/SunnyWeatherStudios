﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSetup
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Player Stats")]
        public string PlayerName;
        public int PlayerXP = 0;
        public int PlayerLevel = 1;
        public int PlayerHP = 50; // increase hp per level by x amount?

        [Header("Player Skills")]
        public List<PlayerSkillSets> Skills = new List<PlayerSkillSets>();



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


