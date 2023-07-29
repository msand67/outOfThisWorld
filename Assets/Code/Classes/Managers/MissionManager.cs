using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dataStructures;
using System;

public class MissionManager : MonoBehaviour
{
    public Plan plan { get; set; }
    public Mission mission { get; set; }
    public double timeElapsed { get; set; }
    public int securityLevel { get; set; }

    public List<dataStructures.Agent> agents;





    // Start is called before the first frame update
    void Start()
    {
        securityLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AddTime();
        UpdatePlanSteps();
    }

    void AddTime()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > mission.gracePeriod) { CheckSecurityLevel(timeElapsed-mission.gracePeriod); }
    }

    void CheckSecurityLevel(double excessTime)
    {
        securityLevel = (int)System.Math.Floor(excessTime / (int)mission.securityInterval);
    }
    void UpdatePlanSteps()
    {
        foreach(Agent a in agents)
        {
            //put in logic to use multiplier stats
            double timeLeft = plan.RemoveTime(a.id, Time.deltaTime);
            if (timeLeft <= 0)
            {
                CompleteStep(a);
            }
        }
    }

    private void CompleteStep(Agent a)
    {
        AgentAction currentAction = plan.GetCurrentAction(a.id);
        switch (currentAction)
        {
            case AgentAction.Enter:
                EnterLevel();
                break;
            case AgentAction.Exit:
                ExitLevel();
                break;
            case AgentAction.MakeCheck:
                PerformCheck(a);
                break;
            case AgentAction.Move:
                MoveRooms(a, plan.GetCurrentStep(a.id).targetRoom);
                    break;
            default:
                break;
        }
    }

    void MoveRooms(Agent agent, int targetRoom)
    {
        mission.UpdateLocation(agent.id, targetRoom);
    }
    void PerformCheck(Agent agent)
    {
        String attemptStatus = "";
        PlanStep step = plan.GetCurrentStep(agent.id);
        int roomNumber = step.roomNumber;
        CheckType check = mission.myMap.GetRoomCheckType(roomNumber);
        int difficultyMod = (int)Math.Floor(securityLevel * 0.5 * (int)mission.securityInterval);


        (bool, int) result = mission.myMap.PerformCheck(roomNumber, agent.statList,difficultyMod);
        //restructure using the checkType get method instead of passing statlist object around. RNG should happen here.

        if (result.Item1)
        {
            plan.NextAction(agent.id);
            attemptStatus = "succeeded";
        }
        else
        {
            plan.AddTime(agent.id, agent.ComputeTime(result.Item2, step.action));
            attemptStatus = "failed";
        }
        String statusString = $"{agent.name} {attemptStatus} a {check} check!";
        Debug.Log(statusString);
        //report success.
    }
    void ExitLevel()
    {
        //largely UI stuff
    }
    void EnterLevel()
    {
        //Largely UI stuff
    }
    void SetNextAction(Agent agent)
    {
        plan.NextAction(agent.id);
        PlanStep step = plan.GetCurrentStep(agent.id);
        plan.ReplaceTime(agent.id, agent.ComputeTime(step.timeRemaining, step.action));

    }

}
