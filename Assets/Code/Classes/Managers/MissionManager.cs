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
    bool success = true;

    public GameObject missionLostDisplay;
    public TMPro.TextMeshProUGUI missionFinishText;

    public RectTransform Entrance1;

    public RectTransform Entrance2;

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

    public RectTransform agentMini0;
    public IEnumerator agent0Coroutine;
    public RectTransform agentMini1;
    public IEnumerator agent1Coroutine;
    public RectTransform agentMini2;
    public IEnumerator agent2Coroutine;


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


        //StartupUI();


        //missionPhaseContainer.SetActive(true);
        Debug.Log(JsonUtility.ToJson(mission));
        Debug.Log("init finished.");
        //SaveAgents();
    }

    private void LoadAgents()
    {
        agents = new List<Agent>();
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":3},{ \"type\":3,\"level\":0},{ \"type\":4,\"level\":0},{ \"type\":5,\"level\":20}],\"name\":\"Murphius\",\"id\":2, \"currentRoom\":-1,\"isInside\":false}"));
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":4},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":0},{ \"type\":0,\"level\":0},{ \"type\":4,\"level\":4},{ \"type\":5,\"level\":0}],\"name\":\"Noe\",\"id\":3, \"currentRoom\":-1,\"isInside\":false}"));
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":3},{ \"type\":2,\"level\":0},{ \"type\":3,\"level\":2},{ \"type\":4,\"level\":0},{ \"type\":5,\"level\":4}],\"name\":\"Smoth\",\"id\":4, \"currentRoom\":-1,\"isInside\":false}"));
        Debug.Log(JsonUtility.ToJson(agents[1]));
    }
    private void SaveAgents()
    {
        foreach (Agent a in agents)
        {
            using (System.IO.StreamWriter myWriter = new System.IO.StreamWriter($"Assets/Agents/{a.name}.json"))
            {
                myWriter.Write(JsonUtility.ToJson(a));
            }
        }
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

    private void SetStartingAgentPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            PlanStep currentStep = plan.GetCurrentStep(agents[i].id);

            double timeToMove = agents[i].ComputeTime(currentStep.baseTime, AgentAction.Enter);
            if (currentStep.targetRoom == 1)
            {
                if (i == 0)
                {
                    agentMini0.SetParent(Entrance1);
                    agentMini0.localPosition = new Vector3(0, 0, 0);
                    MoveAgentMini(agents[i].id, currentStep.targetRoom);
                    StartCoroutine(MoveFromTo(agentMini0, timeToMove));
                }
                else if (i == 1)
                {
                    agentMini1.SetParent(Entrance1);
                    agentMini1.localPosition = new Vector3(0, 0, 0);
                    StartCoroutine(MoveFromTo(agentMini1, timeToMove));
                    MoveAgentMini(agents[i].id, currentStep.targetRoom);
                }
                else
                {
                    agentMini2.SetParent(Entrance1);
                    agentMini2.localPosition = new Vector3(0, 0, 0);
                    StartCoroutine(MoveFromTo(agentMini2, timeToMove));
                    MoveAgentMini(agents[i].id, currentStep.targetRoom);
                }
            }
            else
            {

                if (i == 0)
                {
                    agentMini0.SetParent(Entrance2);
                    agentMini0.localPosition = new Vector3(0, 0, 0);
                    StartCoroutine(MoveFromTo(agentMini0, timeToMove));
                    MoveAgentMini(agents[i].id, plan.GetCurrentStep(agents[i].id).targetRoom);
                }
                else if (i == 1)
                {
                    agentMini1.SetParent(Entrance2);
                    agentMini1.localPosition = new Vector3(0, 0, 0);
                    StartCoroutine(MoveFromTo(agentMini1, timeToMove));
                    MoveAgentMini(agents[i].id, currentStep.targetRoom);
                }
                else
                {
                    agentMini2.SetParent(Entrance2);
                    agentMini2.localPosition = new Vector3(0, 0, 0);
                    StartCoroutine(MoveFromTo(agentMini2, timeToMove));
                    MoveAgentMini(agents[i].id, currentStep.targetRoom);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized && missionPhaseContainer.activeSelf)
        {
            return;
        }
        if (missionPhaseContainer.activeSelf)
        {
            double timeElapsed = Time.deltaTime * (timeSlider.value / 2);
            AddTime(timeElapsed);


            //RefreshUI();
            if (agents.TrueForAll(AgentIsExtracted))
            {
                Debug.Log("All agents extracted.");
                WinMission();
                return;
            }
            UpdatePlanSteps(timeElapsed);
            UpdateAgentPanel();
            map.UpdateRoomTooltips();
        }
    }

    public void NewMissionStartUp(List<Agent> iAgents = null, List<List<PlanStep>> iPLanSteps = null)
    {
        missionLostDisplay.gameObject.SetActive(false);
        initialized = true;
        Debug.Log("init start");
        securityLevel = 0;
        timeSlider.value = 0;
        timeOnMission = 0;
        if (map == null) { map.init(); }

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

        SetStartingAgentPositions();
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
    private void LoseMission()
    {
        timeSlider.value = 0;
        success = false;
        map.transform.parent.gameObject.SetActive(false);
        missionLostDisplay.gameObject.SetActive(true);
        if (planningPhaseContainer.GetComponent<PlanningManager>().cash < 0)
        {
            missionFinishText.text = "Oh no! You couldn't get away with the goods. Also, you are now bankrupt so please return to the main menu. ";
            missionFinishText.transform.parent.GetComponentInChildren<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            missionFinishText.transform.parent.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(BackToMenu);

        }
        else
        {
            missionFinishText.text = "Oh no! Your agents were detected before they could extract with the goods. You'll have to try again!";
        }
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Landing Scene");
        initialized = false;
    }

    private void WinMission()
    {

        timeSlider.value = 0;
        success = true;
        map.transform.parent.gameObject.SetActive(false);
        missionLostDisplay.gameObject.SetActive(true);
        missionFinishText.text = $"Success!! Your agents got the goods, and you got the cash. ${mission.baseReward} to be exact!";
    }

    public void FinishMission()
    {
        initialized = false;

        agentMini0.SetParent(this.transform);
        agentMini1.SetParent(this.transform);
        agentMini2.SetParent(this.transform);
        missionPhaseContainer.SetActive(false);
        planningPhaseContainer.SetActive(true);

        if (success)
        {
            planningPhaseContainer.GetComponent<PlanningManager>().EarnMoney(mission.baseReward);
        }
        planningPhaseContainer.GetComponent<PlanningManager>().EarnMoney(0);
        planningPhaseContainer.GetComponent<PlanningManager>().UpdateMissionCostText();
        map.gameObject.SetActive(true);
        map.gameObject.transform.parent.transform.localPosition = new Vector3(253, 83, 0);
    }

    private void UpdateAgentPanel()
    {
        agentMini0.GetComponent<UnityEngine.UI.Image>().sprite = topAgentPortrait.sprite = GetSpriteForAgent(agents[0].id);
        topAgentStatField.SetText($"{agents[0].name}\nBest Stat: {agents[0].GetBestStat()}\nFailures: {failureCount[agents[0].id]}");
        topAgentTaskField.SetText(GetAgentActionDescription(agents[0]));
        topAgentProgressBar.size = (float)GetProgressPercentage(agents[0]);

        agentMini1.GetComponent<UnityEngine.UI.Image>().sprite = midAgentPortrait.sprite = GetSpriteForAgent(agents[1].id);
        midAgentStatField.SetText($"{agents[1].name}\nBest Stat: {agents[1].GetBestStat()}\nFailures: {failureCount[agents[1].id]}");
        midAgentTaskField.SetText(GetAgentActionDescription(agents[1]));
        midAgentProgressBar.size = (float)GetProgressPercentage(agents[1]);

        agentMini2.GetComponent<UnityEngine.UI.Image>().sprite = botAgentPortrait.sprite = GetSpriteForAgent(agents[2].id);
        botAgentStatField.SetText($"{agents[2].name}\nBest Stat: {agents[2].GetBestStat()}\nFailures: {failureCount[agents[2].id]}");
        botAgentTaskField.SetText(GetAgentActionDescription(agents[2]));
        botAgentProgressBar.size = (float)GetProgressPercentage(agents[2]);
    }

    private bool AgentIsExtracted(Agent a)
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
        switch (id)
        {
            case 0: return planningPhaseContainer.GetComponent<PlanningManager>().agent0Icon;
            case 1: return planningPhaseContainer.GetComponent<PlanningManager>().agent1Icon;
            case 2: return planningPhaseContainer.GetComponent<PlanningManager>().agent2Icon;
            case 3: return planningPhaseContainer.GetComponent<PlanningManager>().agent3Icon;
            case 4: return planningPhaseContainer.GetComponent<PlanningManager>().agent4Icon;
            case 5: return planningPhaseContainer.GetComponent<PlanningManager>().agent5Icon;
            default: return planningPhaseContainer.GetComponent<PlanningManager>().agent0Icon;
        }
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
        if (securityLevel >= 5) { LoseMission(); }
    }



    void UpdatePlanSteps(double timeElapsed)
    {
        foreach (Agent a in agents)
        {
            if(a.currentRoom == -3)
            {
                continue;
            }
            VerifyRoomStatus(a);
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
        if (plan.GetCurrentAction(a.id) == AgentAction.Move)
        {
            MoveAgentMini(a.id, plan.GetCurrentStep(a.id).targetRoom);
        }
        if (plan.GetCurrentAction(a.id) == AgentAction.Exit)
        {
            MoveAgentMini(a.id, plan.GetCurrentStep(a.id).targetRoom);

        }
    }

    void MoveRooms(Agent agent, int targetRoom)
    {
        agent.currentRoom = targetRoom;
        if (targetRoom > 0)
        {
            RevealHiddenRoom(targetRoom);
        }
        SetNextAction(agent);
    }
    void RevealHiddenRoom(int roomId)
    {
        map.roomList[roomId].UpdatePlanningDescription(false);

    }
    void PerformCheck(Agent agent)
    {
        String attemptStatus = "";
        PlanStep step = plan.GetCurrentStep(agent.id);
        int roomNumber = step.roomNumber;
        CheckType check = map.GetRoomCheckType(roomNumber);
        int difficultyMod = (int)Math.Floor(securityLevel * ((double)mission.penalty/2) * (int)mission.securityInterval);
        String statusString = "";


        (bool, int, double) result = map.PerformCheck(roomNumber, agent.statList, difficultyMod, agent.GetFailureBonus());
        //restructure using the checkType get method instead of passing statlist object around. RNG should happen here.

        if (result.Item1)
        {
            SetNextAction(agent);
            attemptStatus = "succeeded";
            map.roomList[roomNumber].DisplayRequiredStatus(true);
            statusString = $"\n{agent.name} {attemptStatus} a {check} check!";
            agent.ResetFailures();
        }
        else
        {
            timeOnMission += result.Item2;
            plan.AddTime(agent.id, result.Item3);
            attemptStatus = "failed";
            statusString = $"\n{agent.name} {attemptStatus} a {check} check! Time penalty of {result.Item2} added.";
            failureCount[agent.id]++;
            agent.AddFailure();
        }
        actionLogField.text += (statusString);
        //report success.
    }
    private int GetAgentIndex( int id)
    {
        for(int i=0; i <agents.Count; i++)
        {
            if(agents[i].id == id)
            {
                return i;
            }
        }
        return -1;
    }
    public void MoveAgentMini(int agentId, int roomId)
    {
        double timeToMove = agents[GetAgentIndex(agentId)].ComputeTime(plan.GetCurrentStep(agentId).baseTime, AgentAction.Move);
        switch (GetAgentIndex(agentId))
        {
            case 0:
                if (roomId < 0) { agentMini0.SetParent(GetExitTarget(agentId)); }
                else
                {
                    agentMini0.SetParent(map.roomList[roomId].transform);
                }
                if(agent0Coroutine != null)
                {
                    StopCoroutine(agent0Coroutine);
                }
                agent0Coroutine = MoveFromTo(agentMini0, timeToMove);
                StartCoroutine(agent0Coroutine);
                break;
            case 1:
                if (roomId < 0) { agentMini1.SetParent(GetExitTarget(agentId)); }
                else
                {
                    agentMini1.SetParent(map.roomList[roomId].transform);
                }
                if (agent1Coroutine != null)
                {
                    StopCoroutine(agent1Coroutine);
                }
                agent1Coroutine = MoveFromTo(agentMini1, timeToMove);
                StartCoroutine(agent1Coroutine);
                break;
            case 2:
                if (roomId < 0) { agentMini2.SetParent(GetExitTarget(agentId)); }
                else
                {
                    agentMini2.SetParent(map.roomList[roomId].transform);
                }
                if (agent2Coroutine != null)
                {
                    StopCoroutine(agent2Coroutine);
                }
                agent2Coroutine = MoveFromTo(agentMini2, timeToMove);
                StartCoroutine(agent2Coroutine);
                break;
            default:
                break;
        }
    }

    private Transform GetExitTarget(int agentId)
    {
        if (agents[GetAgentIndex( agentId)].currentRoom == 1)
        {
            return Entrance1;
        }
        else
        {
            return Entrance2;
        }
    }

    IEnumerator MoveFromTo(RectTransform myIcon, double timeToMove) { 
        Vector3 start = myIcon.localPosition;
        double timeSpent = 0;
        while ((timeSpent / timeToMove) < 1)
        {
            timeSpent +=  Time.deltaTime* (timeSlider.value / 2); ;
            myIcon.localPosition = Vector3.Lerp(start, new Vector3(0, 0, 0), (float)(timeSpent / timeToMove));
            yield return null;
        }
        myIcon.localPosition = new Vector3(0, 0, 0);
        yield return null;
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
        return map.GetRoomCheckType(roomNumber);
    }
    void ExitLevel(Agent a)
    {
        a.isInside = false;
        a.currentRoom = -3;
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
        if (map.IsRoomCheckComplete(a.currentRoom) && plan.GetCurrentAction(a.id) == AgentAction.MakeCheck)
        {
            actionLogField.text += $"\n{a.name}'s room was cleared by someone else. Moving to room {plan.GetNextStep(a.id).targetRoom}";
            SetNextAction(a);
            MoveAgentMini(a.id, plan.GetCurrentStep(a.id).targetRoom);
        }
    }
    void SetNextAction(Agent agent)
    {
        //check if next action is move and if current room check is done. (this is in the event they arrive before the agent assigned to the task)
        //Possibly add option for player to determine which checks they will attempt, which they will wait for.
        bool canMove = false;
        if (agent.currentRoom > 0 || map.IsRoomCheckComplete(agent.currentRoom))
        {
            canMove = true;
        }
        if (plan.GetNextAction(agent.id) == AgentAction.Move && canMove == false)
        {
            CreatePlanStepFromCurrentRoom(agent);
        }
        plan.NextAction(agent.id);

    }

    private void CreatePlanStepFromCurrentRoom(Agent agent)
    {
        CreatePlanStepFromCurrentRoom(agent, agent.currentRoom);
    }

    private void CreatePlanStepFromCurrentRoom(Agent agent, int currentRoom)
    {
        plan.AddAction(agent.id, new PlanStep(AgentAction.MakeCheck, currentRoom, -1, map.GetRoom(currentRoom).check.timeToExecute));
    }
}
