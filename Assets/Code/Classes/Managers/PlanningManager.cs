using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using dataStructures;
using System;

public class PlanningManager : MonoBehaviour
{
    public Transform mapPanel;
    [SerializeField]
    public UnityEngine.UIElements.ScrollView agentView;
    public RectTransform teamSelectionObjects;
    public RectTransform planBuildingObjects;
    public MissionManager missionManager;
    public GameObject missionPhaseObject;

    public UnityEngine.UI.Button viewToggle;

    public RectTransform agent0Card;
    public RectTransform agent1Card;
    public RectTransform agent2Card;

    public TMPro.TextMeshProUGUI planningBoxTitle;
    public RectTransform planningBoxContent;
    public List<List<TMPro.TextMeshProUGUI>> planningBoxTextLIst;

    public TMPro.TextMeshProUGUI planStepReportingLog;

    public TMPro.TextMeshProUGUI planStepBox;

    public TMPro.TextMeshProUGUI roomDetailsBox;

    private List<(int, bool)> requiredRoomsAssigned;
    private List<(int, bool)> hiddenRoomsRevealed;

    public Sprite agent0Icon;
    public Sprite agent1Icon;
    public Sprite agent2Icon;
    public Sprite agent3Icon;
    public Sprite agent4Icon;
    public Sprite agent5Icon;

    public List<ColorPicker> colorPickers;

    public RectTransform gameEndBox;
    public RectTransform menuPanel;


    public int cash = 100000;
    int cost = 0;


    public TMPro.TextMeshProUGUI cashText;


    [SerializeField]
    List<Agent> agents;
    [SerializeField]
    List<Transform> agentPanels;

    int selectedAgentId;
    int selectedStepId;

    string agentFolder = "/Agents/";

    List<Agent> team;
    List<List<PlanStep>> planSteps;
    public GameObject pinPrefab;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMissionCostText();
        team = new List<Agent>();
        agents = new List<Agent>();
        LoadAgents();
        LoadAgentPanels();
        selectedAgentId = -1;
        selectedStepId = -1;
        planSteps = new List<List<PlanStep>>();
        for (int i = 0; i < 3; i++)
        {
            planSteps.Add(new List<PlanStep>());
        }

        planningBoxTextLIst = new List<List<TMPro.TextMeshProUGUI>>();
        for (int i = 0; i < 3; i++)
        {
            planningBoxTextLIst.Add(new List<TMPro.TextMeshProUGUI>());
        }
        foreach (UnityEngine.UI.Button b in planBuildingObjects.GetComponentsInChildren<UnityEngine.UI.Button>())
        {
            if (b.name == "ExitButton1")
                if (b.name == "ExitButton1")
                {
                    b.onClick.AddListener(delegate { ConstructPlanStep(GetRoomById(1), AgentAction.Exit); });
                }
            if (b.name == "ExitButton2")
            {
                b.onClick.AddListener(delegate { ConstructPlanStep(GetRoomById(5), AgentAction.Exit); });
            }
            if (b.name == "EnterButton1")
            {
                b.onClick.AddListener(delegate { ConstructPlanStep(GetRoomById(1), AgentAction.Enter); });
            }
            if (b.name == "EnterButton2")
            {
                b.onClick.AddListener(delegate { ConstructPlanStep(GetRoomById(5), AgentAction.Enter); });
            }
            if (b.name == "WaitButton")
            {

                b.onClick.AddListener(delegate { ConstructPlanStep(AgentAction.Wait, double.Parse(b.GetComponentInChildren<TMPro.TMP_InputField>().text)); });
            }
        }

    }

    internal Sprite GetImageByAgentId(int id)
    {
        switch (id)
        {
            case 0: return agent0Icon;
            case 1: return agent1Icon;
            case 2: return agent2Icon;
            case 3: return agent3Icon;
            case 4: return agent4Icon;
            case 5: return agent5Icon;
            default: return agent0Icon;
        }
    }

    internal void DisplayRoomDetails(Room room)
    {
        bool hidden = false;
        foreach ((int, bool) set in hiddenRoomsRevealed)
        {
            if (set.Item1 == room.id)
            {
                hidden = set.Item2;
            }
        }
        roomDetailsBox.text = room.GetRoomDescription(hidden);
    }
    private Room GetRoomById(int roomId)
    {
        foreach (Room r in mapPanel.GetComponentsInChildren<Room>())
        {
            if (r.id == roomId)
            {
                return r;
            }
        }
        throw new Exception("Could not find room");
    }


    // Update is called once per frame
    void Update()
    {
        if (requiredRoomsAssigned == null && hiddenRoomsRevealed == null)
        {

            requiredRoomsAssigned = mapPanel.GetComponentInChildren<Map>().GetRequiredRooms();
            hiddenRoomsRevealed = mapPanel.GetComponentInChildren<Map>().GetHIddenRooms();
            mapPanel.GetComponentInChildren<Map>().UpdateRoomTextBoxes(requiredRoomsAssigned, hiddenRoomsRevealed);
        }
    }
    private void LoadAgents()
    {

        string[] fileList = System.IO.Directory.GetFiles(Application.streamingAssetsPath + agentFolder);
        List<Agent> tempAgents = new List<Agent>();
        foreach (string fName in fileList)
        {
            if (fName.Contains("json") && !fName.Contains("meta"))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fName))
                {
                    tempAgents.Add(JsonUtility.FromJson<Agent>(sr.ReadToEnd()));
                }
            }
        }
        for (int i = 0; i < tempAgents.Count; i++)
        {
            for (int j = 0; i < tempAgents.Count; j++)
                if (tempAgents[j].id == i)
                {
                    agents.Add(tempAgents[j]);
                    break;
                }
        }
    }

    private void LoadAgentPanels()
    {
        for (int i = 0; i < agentPanels.Count; i++)
        {
            foreach (TMPro.TextMeshProUGUI textBox in agentPanels[i].GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                if (textBox.name == "DescriptionText")
                {
                    textBox.text = agents[i].GetDescriptionText();
                }
                if (textBox.name == "StatText1")
                {
                    textBox.text = agents[i].GetStatText();
                }
                if (textBox.name == "StatNums1")
                {
                    textBox.text = agents[i].GetStatNums();
                }
            }
            agentPanels[i].GetComponent<Tooltip>().message = agents[i].roleDesc;
            int j = i;
            agentPanels[i].GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(delegate { AddAgentToTeam(j); });
        }
    }
    public void AddAgentToTeam(int agentIndex)
    {
        if (team.Count < 3)
        {
            team.Add(agents[agentIndex]);
            //change button to remove instead of add agent.
            UnityEngine.UI.Button button = agentPanels[agentIndex].GetComponentInChildren<UnityEngine.UI.Button>();
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Remove agent";
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { RemoveAgentFromTeam(agents[agentIndex].id); });
            cost += agents[agentIndex].cost;
            UpdateMissionCostText();
            AddAgentToTeamDisplay(agentIndex);
        }
        if (team.Count >= 3)
        {
            DisableAddButtons();
        }

    }

    public void UpdateMissionCostText()
    {
        cashText.text = $"Goal: Retire\n${cash}/$500000\nMission Cost:\n{cost}";
    }

    private void AddAgentToTeamDisplay(int agentIndex)
    {
        RectTransform targetCard = null;
        switch (team.Count)
        {
            case 1:
                targetCard = agent0Card;
                break;
            case 2:
                targetCard = agent1Card;
                break;
            case 3:
                targetCard = agent2Card;
                break;
            default:
                break;
        }
        targetCard.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = GetImageByAgentId(agentIndex);
        Agent a = agents[agentIndex];
        targetCard.Find("Image").GetComponent<UnityEngine.UI.Image>().color = a.agentColor;
        string statString = $"B  {a.statList[0].level},  F  {a.statList[4].level}\nE  {a.statList[1].level},  FH {a.statList[5].level}\nST {a.statList[2].level}\nG {a.statList[3].level}";
        foreach (TMPro.TextMeshProUGUI t in targetCard.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            if (t.name == "StatBox")
            {
                t.text = statString;
            }
            if (t.name == "NameBox")
            {
                t.text = a.name;
            }
        }

    }

    internal void EarnMoney(double baseReward)
    {
        cash += (int)baseReward;
        if (cash > 500000)
        {
            ShowGameEndPanel(true);
            return;
        }
        if (cash < 0)
        {
            ShowGameEndPanel(false);
            return;
        }

        ClearForNewPlan();
        mapPanel.gameObject.SetActive(true);
    }

    private void ShowGameEndPanel(bool gameWon)
    {
        teamSelectionObjects.gameObject.SetActive(false);
        planBuildingObjects.gameObject.SetActive(false);
        gameEndBox.gameObject.SetActive(true);
        if (gameWon)
        {
            gameEndBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Congratulations! You have broken free of the viscious cycle of employment and managed to retire!! (The fact you did that on just $500,000 is frankly more miraculous than the fact you retired at all.)";
        }
        else
        {
            gameEndBox.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Your attempts to retire early have bankrupted you! Now you will work until your corporate overlords decide it isn't profitable to keep you around anymore.";
        }
    }

    void ClearForNewPlan()
    {
        requiredRoomsAssigned = null;
        hiddenRoomsRevealed = null;

        planSteps = new List<List<PlanStep>>();
        planSteps.Add(new List<PlanStep>());
        planSteps.Add(new List<PlanStep>());
        planSteps.Add(new List<PlanStep>());
        for (int i = 0; i < team.Count; i++)
        {
            team[i].isInside = false;
            foreach (TMPro.TextMeshProUGUI t in planningBoxTextLIst[i])
            {
                t.gameObject.SetActive(true);
                Destroy(t.gameObject);
            }
        }
        for (int i = 0; i < planningBoxContent.childCount; i++)
        {
            Destroy(planningBoxContent.GetChild(i));
        }
        planningBoxTextLIst = new List<List<TMPro.TextMeshProUGUI>>();
        planningBoxTextLIst.Add(new List<TMPro.TextMeshProUGUI>());
        planningBoxTextLIst.Add(new List<TMPro.TextMeshProUGUI>());
        planningBoxTextLIst.Add(new List<TMPro.TextMeshProUGUI>());





    }

    public void RemoveAgentFromTeam(int agentId)
    {
        EnableAddButtons();
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].id == agentId)
            {
                cost -= team[i].cost;
                UpdateMissionCostText();
                team.RemoveAt(i);
                int j = GetIndexOfAgent(agentId);
                //change button to add instead of remove agent.
                UnityEngine.UI.Button button = agentPanels[j].GetComponentInChildren<UnityEngine.UI.Button>();
                button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Add to Team";
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate { AddAgentToTeam(j); });
                RemoveAgentFromDisplay(j, i);
                break;
            }
        }


    }

    private void RemoveAgentFromDisplay(int agentIndex, int teamIndex)
    {
        RectTransform targetCard = null;
        RectTransform shiftCard = null;
        int numTimesToShiftLeft = team.Count - teamIndex;


        for (int i = 0; i < numTimesToShiftLeft; i++)
        {
            switch (teamIndex)
            {
                case 0:
                    targetCard = agent0Card;
                    shiftCard = agent1Card;
                    break;
                case 1:
                    targetCard = agent1Card;
                    shiftCard = agent2Card;
                    break;
                default:
                    break;
            }
            ShiftAgentCardLeft(targetCard, shiftCard);
            teamIndex++;
        }

        switch (teamIndex)
        {
            case 0:
                targetCard = agent0Card;
                break;
            case 1:
                targetCard = agent1Card;
                break;
            case 2:
                targetCard = agent2Card;
                break;
            default:
                break;
        }

        ClearAgentCard(targetCard);

    }

    private void ClearAgentCard(RectTransform targetCard)
    {
        foreach (TMPro.TextMeshProUGUI t in targetCard.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            t.text = "";
        }

        targetCard.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = null;
    }

    private void ShiftAgentCardLeft(RectTransform targetCard, RectTransform shiftCard)
    {
        foreach (TMPro.TextMeshProUGUI t in targetCard.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            foreach (TMPro.TextMeshProUGUI s in shiftCard.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                if (t.name == s.name)
                {
                    t.text = s.text;
                    break;
                }
            }
        }

        targetCard.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = shiftCard.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite;
        targetCard.Find("Image").GetComponent<UnityEngine.UI.Image>().color = shiftCard.Find("Image").GetComponent<UnityEngine.UI.Image>().color;
    }

    public int GetIndexOfAgent(int agentId)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            if (agents[i].id == agentId)
            {
                return i;
            }
        }
        return -1;
    }

    public void DisableAddButtons()
    {
        foreach (Transform t in agentPanels)
        {
            UnityEngine.UI.Button button = t.GetComponentInChildren<UnityEngine.UI.Button>();
            if (button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text.Contains("Add"))
            {
                button.gameObject.SetActive(false);
            }

        }
    }
    public void EnableAddButtons()
    {
        foreach (Transform t in agentPanels)
        {
            t.Find("AddRemoveButton").GetComponent<UnityEngine.UI.Button>().gameObject.SetActive(true);
        }
    }

    private void UpdateAgentPanels()
    {

    }

    public void MoveToTeamSelection()
    {
        mapPanel.gameObject.SetActive(false);
        teamSelectionObjects.gameObject.SetActive(true);
        planBuildingObjects.gameObject.SetActive(false);
        viewToggle.onClick.RemoveAllListeners();
        viewToggle.onClick.AddListener(MoveToPlanning);
        viewToggle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Planning";
        viewToggle.GetComponent<Tooltip>().message = "Move to a screen where you can create your plan.";

    }
    public void MoveToPlanning()
    {
        mapPanel.gameObject.SetActive(true);
        teamSelectionObjects.gameObject.SetActive(false);
        planBuildingObjects.gameObject.SetActive(true);
        viewToggle.onClick.RemoveAllListeners();
        viewToggle.onClick.AddListener(MoveToTeamSelection);
        viewToggle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Team Select";
        viewToggle.GetComponent<Tooltip>().message = "Move to a screen where you can select your team.";
    }

    public void ChangeSelectedAgent(int index)
    {
        //disable old agent step list
        if (selectedAgentId >= 0)
        {
            foreach (TMPro.TextMeshProUGUI t in planningBoxTextLIst[selectedAgentId])
            {
                t.gameObject.SetActive(false);
            }
        }
        selectedAgentId = index;

        //enable current agent step list
        foreach (TMPro.TextMeshProUGUI t in planningBoxTextLIst[selectedAgentId])
        {
            t.gameObject.SetActive(true); ;
        }
        //highlight selected agent

        //hide plansteps and change name
        planningBoxTitle.text = $"{team[selectedAgentId].name}'s Plan";
    }

    public void ConstructPlanStep(Room room)
    {
        int len = planSteps[selectedAgentId].Count;
        int currentRoom = -1;
        PlanStep myStep;
        if (len == 0)
        {
            //report that they need to have the character enter the building first.
            planStepReportingLog.text = "Could not create plan step, please have the character enter the site first.";
        }
        //set flags for if entering building, or in a room previously.
        if (len == 1)
        {
            currentRoom = planSteps[selectedAgentId][len - 1].targetRoom;
        }
        else
        {
            currentRoom = planSteps[selectedAgentId][len - 1].roomNumber;
        }
        double distance = mapPanel.GetComponentInChildren<Map>().GetTimeBetweenRooms(currentRoom, room.id);
        if (len == 0 || room.id != currentRoom)
        {
            if (distance > 0)
            {
                //if we have a step distinct from previous, 
                myStep = new PlanStep(AgentAction.Move, currentRoom, room.id, team[selectedAgentId].ComputeTime(distance, AgentAction.Move));
                if (!myStep.Compare(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]))
                {
                    planSteps[selectedAgentId].Add(myStep);
                    myStep.pin = PlacePin(room, team[selectedAgentId].numMovements);
                    team[selectedAgentId].numMovements++;
                    AddPlanStepToDisplay(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]);
                }
                //move to an adjacent room if possible
            }
            else
            {
                planStepReportingLog.text = $"Could not create plan step, rooms {room.id} and {currentRoom} are not adjacent.";
                return;

            }
        }
        //then creat makeCheck
        myStep = new PlanStep(AgentAction.MakeCheck, room.id, -1, team[selectedAgentId].ComputeTime(room.check.timeToExecute, AgentAction.MakeCheck));
        if (!myStep.Compare(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]))
        {
            //if we have a distinct step, create and update.
            planSteps[selectedAgentId].Add(myStep);

            AddPlanStepToDisplay(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]);
            UpdateIsRequiredFlags(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1], true);
            planStepReportingLog.text = $"Move to {room.id} and clear actions added to {team[selectedAgentId].name}'s plan";
        }
        else
        {
            planStepReportingLog.text = "Could not create plan step as it is a duplicate of the previous.";
        }

    }

    private GameObject PlacePin(Room room, int num)
    {
        GameObject pin = Instantiate(pinPrefab);

        pin.transform.SetParent(room.transform);
        pin.transform.localPosition = new Vector3(0, 0, -1);
        pin.GetComponent<PinInterface>().SetNumber(num);
        pin.GetComponent<PinInterface>().SetColor(team[selectedAgentId].agentColor);
        LineRenderer lineRenderer = pin.GetComponent<LineRenderer>();
        Debug.Log(lineRenderer.startColor);
        //Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        //lineRenderer.material = whiteDiffuseMat;
        lineRenderer.startColor = team[selectedAgentId].agentColor;
        lineRenderer.endColor = team[selectedAgentId].agentColor;
        Debug.Log(lineRenderer.startColor);

        lineRenderer.startWidth = lineRenderer.endWidth = 0.3f;

        lineRenderer.SetPosition(0, pin.transform.position);
        lineRenderer.SetPosition(1, GetPreviousPinLocation(pin));

        return pin;
    }

    private Vector3 GetPreviousPinLocation(GameObject pin)
    {
        if(planSteps[selectedAgentId].Count == 1)
        {
            return pin.transform.position;
        }
        if (planSteps[selectedAgentId][planSteps[selectedAgentId].Count-2].pin != null)
        {
            return planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 2].pin.transform.position;
        }
        else if(planSteps[selectedAgentId].Count >2)
        {
            return planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 3].pin.transform.position;
        }

        return pin.transform.position;
    }

    private void UpdateIsRequiredFlags(PlanStep step, bool flag)
    {
        for (int i = 0; i < requiredRoomsAssigned.Count; i++)
        {
            if (requiredRoomsAssigned[i].Item1 == step.roomNumber)
            {
                requiredRoomsAssigned.RemoveAt(i);
                requiredRoomsAssigned.Add((step.roomNumber, flag));
            }
        }
        mapPanel.GetComponentInChildren<Map>().UpdateRoomTextBoxes(requiredRoomsAssigned, hiddenRoomsRevealed);

    }

    //overload for enter/exit actions.
    public void ConstructPlanStep(Room room, AgentAction action)
    {
        if (room.isEntrance)
        {
            if (action == AgentAction.Enter && !team[selectedAgentId].isInside)
            {
                planSteps[selectedAgentId].Add(new PlanStep(action, room.id, room.id, 10));
                AddPlanStepToDisplay(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]);
                planStepReportingLog.text = $"{action.ToString()} action added to {team[selectedAgentId].name}'s plan";
                planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1].pin = PlacePin(room, team[selectedAgentId].numMovements);
                team[selectedAgentId].numMovements++;
                ConstructPlanStep(room);
                team[selectedAgentId].isInside = true;
            }
            else if (action == AgentAction.Exit && team[selectedAgentId].isInside)
            {

                planSteps[selectedAgentId].Add(new PlanStep(action, room.id, -1, 10));
                AddPlanStepToDisplay(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]);
                planStepReportingLog.text = $"{action.ToString()} action added to {team[selectedAgentId].name}'s plan";
                planSteps[selectedAgentId][planSteps[selectedAgentId].Count-1].pin =  PlacePin(room, planSteps[selectedAgentId].Count);
                team[selectedAgentId].isInside = false;
            }
            else
            {
                planStepReportingLog.text = $"Agent cannot {action.ToString()} as they have already done so.";
            }

        }
        else
        {
            planStepReportingLog.text = $"Could not create plan step, room {room.id} is not an entrance.";
        }
    }


    //overload for wait type action.
    public void ConstructPlanStep(AgentAction action, double time)
    {
        int currentRoom = -1;
        if (planSteps[selectedAgentId].Count > 0)
        {
            int top = planSteps[selectedAgentId].Count - 1;
            currentRoom = planSteps[selectedAgentId][top].roomNumber;
        }
        planSteps[selectedAgentId].Add(new PlanStep(AgentAction.Wait, currentRoom, -1, time));
        planStepReportingLog.text = $"Wait action added to {team[selectedAgentId].name}'s plan";
        AddPlanStepToDisplay(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1]);
    }
    public void ClearPlanSteps()
    {
        while (planSteps[selectedAgentId].Count > 0)
        {
            RemovePlanStep();
            planStepReportingLog.text = $"{team[selectedAgentId].name}'s plan erased.";
        }
        planSteps[selectedAgentId] = new List<PlanStep>();
        planningBoxTextLIst[selectedAgentId] = new List<TMPro.TextMeshProUGUI>();
        team[selectedAgentId].isInside = false;
    }

    public void RemovePlanStep()
    {
        //can currently only remove the newest step, due to massive logical dependencies for removing an old step. Someday...
        AgentAction action = planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1].action;
        if (action == AgentAction.Enter)
        {
            team[selectedAgentId].isInside = false;
        }
        if (action == AgentAction.Exit)
        {
            team[selectedAgentId].isInside = true;
        }
        planStepReportingLog.text = $"{action.ToString()} action removed from {team[selectedAgentId].name}'s plan";
        UpdateIsRequiredFlags(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1], false);
        RemovePlanStepFromDisplay();
        if(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1].pin != null)
        {
            team[selectedAgentId].numMovements--;
        }
        Destroy(planSteps[selectedAgentId][planSteps[selectedAgentId].Count - 1].pin);
        planSteps[selectedAgentId].RemoveAt(planSteps[selectedAgentId].Count - 1);
    }
    public void AddPlanStepToDisplay(PlanStep step)
    {
        string descriptor = "";
        int index = planningBoxTextLIst[selectedAgentId].Count;
        TMPro.TextMeshProUGUI foo = (TMPro.TextMeshProUGUI)Instantiate(planStepBox, planningBoxContent);
        foo.text = "temp";
        planningBoxTextLIst[selectedAgentId].Add(foo);
        //format text box here
        switch (step.action)
        {
            case AgentAction.Enter:
                descriptor = $"Enter via room {step.targetRoom}";
                break;
            case AgentAction.Move:
                descriptor = $"Move from room {step.roomNumber} to room {step.targetRoom}";
                break;
            case AgentAction.MakeCheck:
                descriptor = $"Clear room {step.roomNumber} with {step.action.ToString()} check";
                break;
            case AgentAction.Wait:
                descriptor = $"Wait ";
                break;
            case AgentAction.Exit:
                descriptor = $"Exit site via room {step.roomNumber}";
                break;
            default:
                descriptor = "Error, failed to load step";
                break;

        }

        planningBoxTextLIst[selectedAgentId][index].text = $"{descriptor} for {step.baseTime} seconds";
    }

    public void RemovePlanStepFromDisplay()
    {
        GameObject.Destroy(planningBoxTextLIst[selectedAgentId][planningBoxTextLIst[selectedAgentId].Count - 1].gameObject);
        planningBoxTextLIst[selectedAgentId].RemoveAt(planningBoxTextLIst[selectedAgentId].Count - 1);
    }

    public void ChangeSelecteStepId(int id)
    {
        selectedStepId = id;
        //highlight selected step
    }

    public void StartMission()
    {
        if (team.Count < 3)
        {
            planStepReportingLog.text = "Cannot start mission, fewer than three agents selected.";
            return;
        }
        if (!GetRequiredStatus())
        {
            planStepReportingLog.text = "Cannot start mission, not all required rooms are marked to completion.";
            return;
        }
        if (!AgentsWillExtract())
        {
            planStepReportingLog.text = "Cannot start mission, not all agents will extract.";
            return;
        }
        //check if start is valid
        SetUpMission();
    }
    public void SetUpMission()
    {
        cash -= cost;
        missionPhaseObject.SetActive(true);
        mapPanel.gameObject.transform.localPosition = new Vector3(-14, 107, 0);
        this.gameObject.SetActive(false);
        missionManager.mission.LoadMissionData(missionManager.mission.missionId);
        mapPanel.GetComponentInChildren<Map>().FetchDataFromFile(missionManager.mission.missionId, 9);
        missionManager.NewMissionStartUp(team, planSteps);
    }

    private bool AgentsWillExtract()
    {
        bool willExtract = true;
        for (int i = 0; i < planSteps.Count; i++)
        {
            if (planSteps[i][planSteps[i].Count - 1].action != AgentAction.Exit)
            {
                willExtract = false;
            }
        }
        return willExtract;
    }

    private bool GetRequiredStatus()
    {
        bool status = true;
        foreach ((int, bool) set in requiredRoomsAssigned)
        {
            if (set.Item2 == false)
            {
                status = false;
            }
        }
        return status;
    }
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Landing Scene");
    }
    public void ToggleMenu()
    {
        menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
    }

    public void HideMenu()
    {
        menuPanel.gameObject.SetActive(false);
    }
    public void NextMission()
    {
        int loadNumber = missionManager.mission.missionId + 1;
        if (loadNumber > 1)
        {
            loadNumber = 0;
        }
        missionManager.mission.LoadMissionData(loadNumber);
        ClearForNewPlan();
        mapPanel.GetComponentInChildren<Map>().FetchDataFromFile(loadNumber, 9);
        ShowMissionDetails();

    }
    public void PrevMission()
    {
        int loadNumber = missionManager.mission.missionId - 1;
        if (loadNumber < 0)
        {
            loadNumber = 1;
        }

        missionManager.mission.LoadMissionData(loadNumber);
        ClearForNewPlan();
        mapPanel.GetComponentInChildren<Map>().FetchDataFromFile(loadNumber, 9);
        ShowMissionDetails();


    }

    public void ShowMissionDetails()
    {
        roomDetailsBox.text = missionManager.mission.GetDescription();
    }

    public void SetAgentColor(int agentIndex)
    {
        if (agentIndex < team.Count)
        {
            team[agentIndex].agentColor = colorPickers[agentIndex].GetColor();
        }
        RectTransform targetCard;

        switch (agentIndex)
        {
            case 0:
                targetCard = agent0Card;
                break;
            case 1:
                targetCard = agent1Card;
                break;
            case 2:
                targetCard = agent2Card;
                break;
            default:
                return;
        }
        targetCard.Find("Image").GetComponent<UnityEngine.UI.Image>().color = colorPickers[agentIndex].GetColor();
    }
}
