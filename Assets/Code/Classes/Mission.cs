using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Mission
    {
        public Map myMap;
        public double gracePeriod;
        public SecurityInterval myInterval;
        public SecurityPenalty myPenalty;
        public double baseReward;
        public double bonusReward;
        public BonusCriteria myCriteria;

        public List<(Agent, int)> agentList; //int is index of what room in the map they are in.

        public Plan myPlan;

        public Mission()
        {
            myMap = new Map();
            gracePeriod = 120;
            myInterval = SecurityInterval.medium;
            myPenalty = SecurityPenalty.medium;
            baseReward = 100000;
            bonusReward = 25000;
            myCriteria = BonusCriteria.TimeConstraint;
        }
        public Mission(Map iMyMap, double iGracePeriod, SecurityInterval iMyInterval, SecurityPenalty iMyPenalty, double iBaseReward, double iBonusReward, BonusCriteria iMyCriteria)
        {
            myMap = iMyMap;
            gracePeriod = iGracePeriod;
            myInterval = iMyInterval;
            myPenalty = iMyPenalty;
            baseReward = iBaseReward;
            bonusReward = iBonusReward;
            myCriteria = iMyCriteria;
        }
    }
}
