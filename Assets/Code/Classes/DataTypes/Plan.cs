using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    [Serializable]
    public class Plan
    {
        public List<ActionList> actionList; //must be constructed so that elements are at the index equal to their agentId (I hate this, find a better solution)

        public Plan(List<Agent> agents)
        {
            actionList = new List<ActionList>();
            foreach(Agent a in agents)
            {
                ActionList tempActionList = new ActionList(a.id, new List<PlanStep>());
                actionList.Add(tempActionList);
            }
        }
        public Plan(List<Agent> agents, List<List<PlanStep>> planStepsList)
        {
            actionList = new List<ActionList>();
            for (int i = 0; i< agents.Count; i++)
            {
                actionList.Add(new ActionList(agents[i].id, planStepsList[i]));
            }
        }

        public Plan (List<ActionList> iActionList)
        {
            actionList = iActionList;
        }

        public void NextAction(int id)
        {
            GetActionListById(id).steps.RemoveAt(0);
        }

        public void AddAction(int id, PlanStep step)
        {
            GetActionListById(id).steps.Add(step);
        }
        public void RemoveActionAtIndex(int id, int index)
        {
            GetActionListById(id).steps.RemoveAt(index);
        }

        internal double RemoveTime(int id, double time)
        {
            GetActionListById(id).steps[0].timeRemaining -= time;
            return GetActionListById(id).steps[0].timeRemaining;
        }
        internal void AddTime(int id, double time)
        {
            GetActionListById(id).steps[0].timeRemaining += time;
        }
        internal void ReplaceTime(int id, double time)
        {
            GetActionListById(id).steps[0].timeRemaining = time;
        }
        public AgentAction GetCurrentAction(int id)
        {
            return GetActionListById(id).steps[0].action;
        }
        public AgentAction GetNextAction(int id)
        {
            return GetActionListById(id).steps[1].action;
        }
        public PlanStep GetCurrentStep(int id)
        {
            return GetActionListById(id).steps[0];
        }
        public PlanStep GetNextStep(int id)
        {
            return GetActionListById(id).steps[1];
        }

        public ActionList GetActionListById(int id)
        {
            foreach(ActionList a in actionList)
            {
                if(a.agentId == id)
                {
                    return a;
                }
            }
            return new ActionList(-1, new List<PlanStep>());
        }
    }

    [Serializable]
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

    [Serializable]
    public class ActionList {
        public int agentId;
        public List<PlanStep> steps;

        public ActionList(int iAgentId, List<PlanStep> iSteps)
        {
            agentId = iAgentId;
            steps = iSteps;
        }
    }

}
