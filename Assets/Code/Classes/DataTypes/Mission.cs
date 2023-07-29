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

        public List<AgentLocation> agentLocations; //int is index of what room in the map they are in.

        public Plan myPlan;

        public Mission()
        {
            myMap = new Map();
            gracePeriod = 120;
            securityInterval = SecurityInterval.medium;
            penalty = SecurityPenalty.medium;
            baseReward = 100000;
            bonusReward = 25000;
            bonusCriteria = BonusCriteria.TimeConstraint;

            agentLocations = new List<AgentLocation>();

            List<ActionList> tempPlanSteps = new List<ActionList>();
            foreach(AgentLocation a  in agentLocations)
            {
                tempPlanSteps.Add(new ActionList( a.agentId, new List<PlanStep>()));
            }


            myPlan = new Plan(tempPlanSteps);
        }
        public Mission(Map iMyMap, double iGracePeriod, SecurityInterval iMyInterval, SecurityPenalty iMyPenalty, double iBaseReward, double iBonusReward, BonusCriteria iMyCriteria, List<AgentLocation> iAgentList, Plan iMyPlan)
        {
            myMap = iMyMap;
            gracePeriod = iGracePeriod;
            securityInterval = iMyInterval;
            penalty = iMyPenalty;
            baseReward = iBaseReward;
            bonusReward = iBonusReward;
            bonusCriteria = iMyCriteria;
            agentLocations = iAgentList;
            myPlan = iMyPlan;
        }

        internal void UpdateLocation(int id, int roomNumber)
        {
            foreach(AgentLocation location in agentLocations)
            {
                if (location.agentId == id)
                {
                    location.roomId = roomNumber;
                }
            }
        }
    }
    public class AgentLocation
    {
        public int agentId;
        public int roomId;
        public AgentLocation(int iAgentId = -1, int iRoomid = -1)
        {
            agentId = iAgentId;
            roomId = iRoomid;
        }
    }
}
