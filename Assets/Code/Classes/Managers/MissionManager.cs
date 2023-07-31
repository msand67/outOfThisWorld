using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dataStructures;
using System;
using TMPro;

[Serializable]
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

    [SerializeField]
    UnityEngine.UI.Image topAgentPortrait;
    [SerializeField]
    TMPro.TextMeshProUGUI topAgentStatField;
    [SerializeField]
    TMPro.TextMeshProUGUI topAgentTaskField;
    [SerializeField]
    UnityEngine.UI.Scrollbar topAgentProgressBar;

    [SerializeField]
    UnityEngine.UI.Image midAgentPortrait;
    [SerializeField]
    TMPro.TextMeshProUGUI midAgentStatField;
    [SerializeField]
    TMPro.TextMeshProUGUI midAgentTaskField;
    [SerializeField]
    UnityEngine.UI.Scrollbar midAgentProgressBar;

    [SerializeField]
    UnityEngine.UI.Image botAgentPortrait;
    [SerializeField]
    TMPro.TextMeshProUGUI botAgentStatField;
    [SerializeField]
    TMPro.TextMeshProUGUI botAgentTaskField;
    [SerializeField]
    UnityEngine.UI.Scrollbar botAgentProgressBar;

    [SerializeField]
    GameObject missionPhaseContainer;
    [SerializeField]
    GameObject planningPhaseContainer;

    [SerializeField]
    public List<Agent> agents;

    [SerializeField]
    Map map;

    Dictionary<int, int> failureCount;
    bool initialized;





    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("init start");
        securityLevel = 0;
        timeSlider.value = 2;
        timeOnMission = 0;
        map.init();
        mission = new Mission(map);


        LoadAgents();

        failureCount = new Dictionary<int, int>();
        foreach (Agent a in agents)
        {
            failureCount.Add(a.id, 0);
        }


        List<List<PlanStep>> foo = new List<List<PlanStep>>();
        for (int i = 0; i < 3; i++)
        {

            List<PlanStep> tempStepList = new List<PlanStep>();
            tempStepList.Add(new PlanStep(AgentAction.Enter, -2, 1, 10));
            tempStepList.Add(new PlanStep(AgentAction.MakeCheck, 1, -1, 10));
            tempStepList.Add(new PlanStep(AgentAction.Move, 1, 2, 10));
            tempStepList.Add(new PlanStep(AgentAction.Exit, 2, -1, 15));
            foo.Add(tempStepList);
        }

        plan = new Plan(agents, foo);
        Debug.Log(JsonUtility.ToJson(plan));


        StartupUI();
        Debug.Log(JsonUtility.ToJson(agents[1]));


        //missionPhaseContainer.SetActive(true);
        Debug.Log("init finished.");
    }

    private void LoadAgents()
    {
        agents = new List<Agent>();
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":3},{ \"type\":3,\"level\":0},{ \"type\":4,\"level\":0},{ \"type\":5,\"level\":20}],\"name\":\"Murphius\",\"id\":2, \"currentRoom\":-1,\"isInside\":false}"));
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":4},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":0},{ \"type\":0,\"level\":0},{ \"type\":4,\"level\":4},{ \"type\":5,\"level\":0}],\"name\":\"Noe\",\"id\":3, \"currentRoom\":-1,\"isInside\":false}"));
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":3},{ \"type\":2,\"level\":0},{ \"type\":3,\"level\":2},{ \"type\":4,\"level\":0},{ \"type\":5,\"level\":4}],\"name\":\"Smoth\",\"id\":4, \"currentRoom\":-1,\"isInside\":false}"));
        Debug.Log(JsonUtility.ToJson(agents[1]));
    }

    void StartupUI()
    {
        int seconds = ((int)mission.gracePeriod % 60);
        int minutes = ((int)mission.gracePeriod / 60);
        deadlineField.SetText("Deadline: {0:00}:{1:00}.000", minutes, seconds);
        actionLogField.text = "";

        securityPenaltyField.SetText($"Security Penalty: {mission.penalty}");
        securityIntervalField.SetText($"Security Interval: {mission.securityInterval}");

        UpdateAgentPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized && missionPhaseContainer.activeSelf)
        {
            NewMissionStartUp();
            initialized = true;
        }
        if (missionPhaseContainer.activeSelf)
        {
            double timeElapsed = Time.deltaTime * (timeSlider.value / 2);
            AddTime(timeElapsed);


            //RefreshUI();
            if (agents.TrueForAll(AgentIsExtracted))
            {
                Debug.Log("All agents extracted.");
                FinishMission();
                return;
            }
            UpdatePlanSteps(timeElapsed);
            UpdateAgentPanel();
            map.UpdateRoomTooltips();
        }
    }

    public void NewMissionStartUp(List<Agent> iAgents=null, List<List<PlanStep>> iPLanSteps = null)
    {
        Debug.Log("init start");
        securityLevel = 0;
        timeSlider.value = 0;
        timeOnMission = 0;
        if (map == null) { map.init(); }
        mission = new Mission(map);

        ResetRoomRequiredDisplay();

        if (iAgents == null) { LoadAgents(); } else { agents = iAgents; }

        failureCount = new Dictionary<int, int>();
        foreach (Agent a in agents)
        {
            failureCount.Add(a.id, 0);
        }


        if (iPLanSteps == null)
        {
            List<List<PlanStep>> foo = new List<List<PlanStep>>();
            for (int i = 0; i < 3; i++)
            {

                List<PlanStep> tempStepList = new List<PlanStep>();
                tempStepList.Add(new PlanStep(AgentAction.Enter, -2, 1, 10));
                tempStepList.Add(new PlanStep(AgentAction.MakeCheck, 1, -1, 10));
                tempStepList.Add(new PlanStep(AgentAction.Move, 1, 2, 10));
                tempStepList.Add(new PlanStep(AgentAction.Exit, 2, -1, 15));
                foo.Add(tempStepList);
            }

            plan = new Plan(agents, foo);
        }
        else
            plan = new Plan(agents, iPLanSteps);
        Debug.Log(JsonUtility.ToJson(plan));


        StartupUI();
        Debug.Log(JsonUtility.ToJson(agents[1]));


        missionPhaseContainer.SetActive(true);
        initialized = true;
        Debug.Log("init finished.");
    }

    private void ResetRoomRequiredDisplay()
    {
        foreach (Room r in map.roomList)
        {
            r.DisplayRequiredStatus(false);
        }
    }

    private void FinishMission()
    {
        missionPhaseContainer.SetActive(false);
        planningPhaseContainer.SetActive(true);

        map.gameObject.transform.parent.transform.localPosition = new Vector3(297, 118, 0);
    }

    private void UpdateAgentPanel()
    {
        topAgentPortrait.sprite = GetSpriteForAgent(agents[0].id);
        topAgentStatField.SetText($"{agents[0].name}\nBest Stat: {agents[0].GetBestStat()}\nFailures: {failureCount[agents[0].id]}");
        topAgentTaskField.SetText(GetAgentActionDescription(agents[0]));
        topAgentProgressBar.size = (float)GetProgressPercentage(agents[0]);

        midAgentPortrait.sprite = GetSpriteForAgent(agents[1].id);
        midAgentStatField.SetText($"{agents[1].name}\nBest Stat: {agents[1].GetBestStat()}\nFailures: {failureCount[agents[1].id]}");
        midAgentTaskField.SetText(GetAgentActionDescription(agents[1]));
        midAgentProgressBar.size = (float)GetProgressPercentage(agents[1]);

        botAgentPortrait.sprite = GetSpriteForAgent(agents[2].id);
        botAgentStatField.SetText($"{agents[2].name}\nBest Stat: {agents[2].GetBestStat()}\nFailures: {failureCount[agents[2].id]}");
        botAgentTaskField.SetText(GetAgentActionDescription(agents[2]));
        botAgentProgressBar.size = (float)GetProgressPercentage(agents[2]);
    }

    private  bool AgentIsExtracted(Agent a)
    {
        return (!a.isInside && plan.GetCurrentAction(a.id) == AgentAction.Exit);
    }
    private string GetAgentActionDescription(Agent agent)
    {
        switch (plan.GetCurrentAction(agent.id))
        {
            case AgentAction.Enter:
                return "Entering site";
            case AgentAction.Exit:
                if (agent.isInside) { return "Extracting"; } else return "Extracted";
            case AgentAction.Move:
                return $"Moving to room {plan.GetCurrentStep(agent.id).targetRoom}";
            case AgentAction.MakeCheck:
                return GetCheckDescription(GetAgentCheckType(agent));
            default:
                return "Waiting";
        }
    }

    string GetCheckDescription(CheckType check)
    {
        switch (check)
        {
            case CheckType.Breach:
                return "Removing obstacles";
            case CheckType.Egghead:
                return "Thinking real hard";
            case CheckType.Gumshoe:
                return "Searching the room";
            case CheckType.SmoothTalk:
                return "Applying silver tongue";
            default:
                return "unknown check type found";
        }
    }

    private Sprite GetSpriteForAgent(int id)
    {
        return null;
    }

    void AddTime(double timeElapsed)
    {
        timeOnMission += timeElapsed;
        if (timeOnMission > mission.gracePeriod) { CheckSecurityLevel(timeOnMission - mission.gracePeriod); }
        int seconds = ((int)timeOnMission % 60);
        int minutes = ((int)timeOnMission / 60);
        double millis = timeOnMission - (int)timeOnMission;

        timeField.SetText(string.Format("Time: {0:00}:{1:00}{2:.000}", minutes, seconds, millis));
    }

    void CheckSecurityLevel(double excessTime)
    {
        securityLevel = (int)System.Math.Floor(excessTime / (int)mission.securityInterval) + 1;
        securityLevelField.SetText($"Security Level: {securityLevel}");
    }
    void UpdatePlanSteps(double timeElapsed)
    {
        foreach (Agent a in agents)
        {
            VerifyRoomStatus(a);
            //put in logic to use multiplier stats
            double timeLeft = plan.RemoveTime(a.id, a.ComputeTime(timeElapsed, plan.GetCurrentAction(a.id)));
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
                EnterLevel(a);
                break;
            case AgentAction.Exit:
                ExitLevel(a);
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
        agent.currentRoom = targetRoom;
        SetNextAction(agent);
    }
    void PerformCheck(Agent agent)
    {
        String attemptStatus = "";
        PlanStep step = plan.GetCurrentStep(agent.id);
        int roomNumber = step.roomNumber;
        CheckType check = mission.myMap.GetRoomCheckType(roomNumber);
        int difficultyMod = (int)Math.Floor(securityLevel * 0.5 * (int)mission.securityInterval);
        String statusString = "";


        (bool, int, double) result = mission.myMap.PerformCheck(roomNumber, agent.statList, difficultyMod);
        //restructure using the checkType get method instead of passing statlist object around. RNG should happen here.

        if (result.Item1)
        {
            SetNextAction(agent);
            attemptStatus = "succeeded";
            map.roomList[roomNumber].DisplayRequiredStatus(true);
            statusString = $"\n{agent.name} {attemptStatus} a {check} check!";
        }
        else
        {
            timeOnMission += result.Item2;
            plan.AddTime(agent.id, result.Item3);
            attemptStatus = "failed";
            statusString = $"\n{agent.name} {attemptStatus} a {check} check! Time penalty of {result.Item2} added.";
            failureCount[agent.id]++;
        }
        actionLogField.text += (statusString);
        //report success.
    }

    double GetProgressPercentage(Agent a)
    {
        double progress = 0;
        double timeLeft = 0;
        double baseTime = 0;
        PlanStep currentStep = plan.GetCurrentStep(a.id);
        baseTime = currentStep.baseTime;
        timeLeft = currentStep.timeRemaining;
        //needs to be zero when the two are equal, and approach one when timeremaining approaches 0;
        progress = Math.Abs(1 - timeLeft / baseTime);

        return progress;

    }

    CheckType GetAgentCheckType(Agent a)
    {
        int roomNumber = plan.GetCurrentStep(a.id).roomNumber;
        return mission.myMap.GetRoomCheckType(roomNumber);
    }
    void ExitLevel(Agent a)
    {
        a.isInside = false;
        a.currentRoom = -1;
        //largely UI stuff
    }
    void EnterLevel(Agent a)
    {
        //do ui stuff here
        a.isInside = true;
        MoveRooms(a, plan.GetCurrentStep(a.id).targetRoom);
    }
    void VerifyRoomStatus(Agent a)
    {
        if (mission.myMap.IsRoomCheckComplete(a.currentRoom) && plan.GetCurrentAction(a.id)==AgentAction.MakeCheck)
        {
            actionLogField.text += $"\n{a.name}'s room was cleared by someone else. Moving to room {plan.GetNextStep(a.id).targetRoom}";
            SetNextAction(a);
        }
    }
    void SetNextAction(Agent agent)
    {
        //check if next action is move and if current room check is done. (this is in the event they arrive before the agent assigned to the task)
        //Possibley add option for player to determine which checks they will attempt, which they will wait for.
        bool canMove = false;
        if (agent.currentRoom>0 ||  mission.myMap.IsRoomCheckComplete(agent.currentRoom))
        {
            canMove = true;
        }
        if(plan.GetNextAction(agent.id)==AgentAction.Move && canMove == false)
        {
            CreatePlanStepFromCurrentRoom(agent);
        }
        plan.NextAction(agent.id);
        PlanStep step = plan.GetCurrentStep(agent.id);
        plan.ReplaceTime(agent.id, step.timeRemaining);

    }

    private void CreatePlanStepFromCurrentRoom(Agent agent)
    {
        CreatePlanStepFromCurrentRoom(agent, agent.currentRoom);
    }

    private void CreatePlanStepFromCurrentRoom(Agent agent, int currentRoom)
    {
        plan.AddAction(agent.id, new PlanStep(AgentAction.MakeCheck, currentRoom, -1, mission.myMap.GetRoom(currentRoom).check.timeToExecute));
    }
}
