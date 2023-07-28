using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Plan
    {
        public Dictionary<int, List<PlanStep>> actionList; //int is AgentId

        public Plan (List<Agent> agents, List<List<PlanStep>> agentPlans)
        {
            actionList = new Dictionary<int, List<PlanStep>>();
            for(int i = 0; i<agents.Count; i++)
            {
                actionList.Add(agents[i].id, agentPlans[i]);
            }
        }

        public void nextAction(int id)
        {
            actionList[id].RemoveAt(0);
        }


    }

    public struct PlanStep
    {
        public Action action;
        public int roomNumber;
        public PlanStep(Action iAction, int iRoomNumber)
        {
            action = iAction;
            roomNumber = iRoomNumber;
        }
    }
}
