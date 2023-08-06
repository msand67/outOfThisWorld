using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Mission
    {
        public double gracePeriod;
        public SecurityInterval securityInterval;
        public SecurityPenalty penalty;
        public double baseReward;
        public double bonusReward;
        public BonusCriteria bonusCriteria;
        public int missionId = 0;


        public Mission(Map iMyMap)
        {
            gracePeriod = 120;
            securityInterval = SecurityInterval.medium;
            penalty = SecurityPenalty.medium;
            baseReward = 150000;
            bonusReward = 25000;
            bonusCriteria = BonusCriteria.TimeConstraint;
            List<ActionList> tempPlanSteps = new List<ActionList>();

        }
        public Mission(Map iMyMap, double iGracePeriod, SecurityInterval iMyInterval, SecurityPenalty iMyPenalty, double iBaseReward, double iBonusReward, BonusCriteria iMyCriteria)
        {
            gracePeriod = iGracePeriod;
            securityInterval = iMyInterval;
            penalty = iMyPenalty;
            baseReward = iBaseReward;
            bonusReward = iBonusReward;
            bonusCriteria = iMyCriteria;
        }
        public void LoadMissionData(int missionNum)
        {
            string path = Application.streamingAssetsPath + "/MapData/map_" + missionNum + "/missionData.json";
            using(System.IO.StreamReader myReader = new System.IO.StreamReader(path))
            {
                JsonUtility.FromJsonOverwrite(myReader.ReadToEnd(), this);
            }
        }

        internal string GetDescription()
        {
            string desc = $"Grace Period: {gracePeriod}\nSecurity Interval: {securityInterval}\nSecurity Penalty: {penalty.ToString()}\nReward: ${baseReward}";


            return desc;
        }
    }
}
