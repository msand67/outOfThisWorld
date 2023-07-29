using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Plan
    {
        public Dictionary<int, List<PlanStep>> actionList; //int is AgentId

        public Plan (Dictionary<int, List<PlanStep>> iActionList)
        {
            actionList = iActionList;
        }

        public void NextAction(int id)
        {
            actionList[id].RemoveAt(0);
        }

        public void AddAction(int id, PlanStep step)
        {
            actionList[id].Add(step);
        }
        public void RemoveActionAtIndex(int id, int index)
        {
            actionList[id].RemoveAt(index);
        }

        internal double RemoveTime(int id, double time)
        {
            actionList[id][0].timeRemaining -= time;
            return actionList[id][0].timeRemaining;
        }
        internal void AddTime(int id, double time)
        {
            actionList[id][0].timeRemaining += time;
        }
        internal void ReplaceTime(int id, double time)
        {
            actionList[id][0].timeRemaining = time;
        }
        public AgentAction GetCurrentAction(int id)
        {
            return actionList[id][0].action;
        }
        public AgentAction GetNextAction(int id)
        {
            return actionList[id][1].action;
        }
        public PlanStep GetCurrentStep(int id)
        {
            return actionList[id][0];
        }
        public PlanStep GetNextStep(int id)
        {
            return actionList[id][1];
        }
    }

    public class PlanStep
    {
        public AgentAction action;
        public int roomNumber;
        public int targetRoom;
        public double timeRemaining;
        public PlanStep(AgentAction iAction, int iRoomNumber, int iTargetRoom=-1, double iTimeRemaining = -1)
        {
            action = iAction;
            roomNumber = iRoomNumber;
            targetRoom = iTargetRoom;
            timeRemaining = iTimeRemaining;
        }

       
    }
}
