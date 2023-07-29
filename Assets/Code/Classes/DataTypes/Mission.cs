using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Mission
    {
        public Map myMap;
        public double gracePeriod;
        public SecurityInterval securityInterval;
        public SecurityPenalty penalty;
        public double baseReward;
        public double bonusReward;
        public BonusCriteria bonusCriteria;


        public Mission(Map iMyMap)
        {
            myMap = iMyMap;
            gracePeriod = 120;
            securityInterval = SecurityInterval.medium;
            penalty = SecurityPenalty.medium;
            baseReward = 100000;
            bonusReward = 25000;
            bonusCriteria = BonusCriteria.TimeConstraint;
            List<ActionList> tempPlanSteps = new List<ActionList>();

        }
        public Mission(Map iMyMap, double iGracePeriod, SecurityInterval iMyInterval, SecurityPenalty iMyPenalty, double iBaseReward, double iBonusReward, BonusCriteria iMyCriteria)
        {
            myMap = iMyMap;
            gracePeriod = iGracePeriod;
            securityInterval = iMyInterval;
            penalty = iMyPenalty;
            baseReward = iBaseReward;
            bonusReward = iBonusReward;
            bonusCriteria = iMyCriteria;
        }
    }
}
