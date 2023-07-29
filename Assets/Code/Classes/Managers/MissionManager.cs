using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dataStructures;
using System;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public Plan plan { get; set; }
    public Mission mission { get; set; }
    public double timeOnMission { get; set; }
    public int securityLevel { get; set; }

    [SerializeField]
    UnityEngine.UI.Slider timeSlider;
    [SerializeField]
    TMPro.TextMeshProUGUI timeField;
    [SerializeField]
    TMPro.TextMeshProUGUI deadlineField;
    [SerializeField]
    TMPro.TextMeshProUGUI securityLevelField;
    [SerializeField]
    TMPro.TextMeshProUGUI securityPenaltyField;
    [SerializeField]
    TMPro.TextMeshProUGUI securityIntervalField;
    [SerializeField]
    TMPro.TextMeshProUGUI actionLogField;




    public List<Agent> agents;





    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("init start");
        securityLevel = 0;
        timeSlider.value = 0;
        timeOnMission = 0;
        mission = new Mission();

        StartupUI();
        Debug.Log("init finished.");

        LoadAgents();
        MoveRooms(agents[0], 1);
        List<PlanStep> tempStepList = new List<PlanStep>();
        tempStepList.Add(new PlanStep(AgentAction.MakeCheck, 1, -1, 10));
        List<List<PlanStep>> foo = new List<List<PlanStep>>();
        foo.Add(tempStepList);

        plan = new Plan(agents, foo);
        Debug.Log(JsonUtility.ToJson( plan));
    }

    private void LoadAgents()
    {
        agents = new List<Agent>();
        

        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":-10},{ \"type\":3,\"level\":-10},{ \"type\":4,\"level\":3},{ \"type\":5,\"level\":0}],\"name\":\"Murphius\",\"id\":2}"));
        Debug.Log(JsonUtility.ToJson( agents[0]));
    }

    void StartupUI()
    {
        int seconds = ((int)mission.gracePeriod % 60);
        int minutes = ((int)mission.gracePeriod / 60);
        deadlineField.SetText("Deadline: {0:00}:{1:00}.000", minutes, seconds);
        actionLogField.text = "";

        securityPenaltyField.SetText($"Security Penalty: {mission.penalty}");
        securityIntervalField.SetText($"Security Interval: {mission.securityInterval}");
    }

    // Update is called once per frame
    void Update()
    {
        double timeElapsed = Time.deltaTime * (timeSlider.value/2);
        AddTime(timeElapsed);


        //RefreshUI();
        UpdatePlanSteps(timeElapsed);
    }

    private void RefreshUI()
    {
    }

    void AddTime(double timeElapsed)
    {
        timeOnMission += timeElapsed;
        if (timeOnMission > mission.gracePeriod) { CheckSecurityLevel(timeOnMission-mission.gracePeriod); }
        int seconds = ((int)timeOnMission % 60);
        int minutes = ((int)timeOnMission / 60);
        double millis = timeOnMission - (int)timeOnMission;

        timeField.SetText(string.Format("Time: {0:00}:{1:00}{2:.000}", minutes, seconds, millis));
    }

    void CheckSecurityLevel(double excessTime)
    {
        securityLevel = (int)System.Math.Floor(excessTime / (int)mission.securityInterval)+1;
        securityLevelField.SetText($"Security Level: {securityLevel}");
    }
    void UpdatePlanSteps(double timeElapsed)
    {
        foreach(Agent a in agents)
        {
            //put in logic to use multiplier stats
            double timeLeft = plan.RemoveTime(a.id, timeElapsed);
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
        String statusString = "";


        (bool, int, double) result = mission.myMap.PerformCheck(roomNumber, agent.statList,difficultyMod);
        //restructure using the checkType get method instead of passing statlist object around. RNG should happen here.

        if (result.Item1)
        {
            plan.NextAction(agent.id);
            attemptStatus = "succeeded";
           statusString = $"\n{agent.name} {attemptStatus} a {check} check!";
        }
        else
        {
            timeOnMission += result.Item2;
            plan.AddTime(agent.id, agent.ComputeTime(result.Item3, step.action));
            attemptStatus = "failed";
            statusString = $"\n{agent.name} {attemptStatus} a {check} check! Time penalty of {result.Item2} added.";
        }
        actionLogField.text+=(statusString);
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
