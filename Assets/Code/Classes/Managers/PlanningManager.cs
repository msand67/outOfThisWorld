using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using dataStructures;
using System;

public class PlanningManager : MonoBehaviour
{
    public RectTransform mapPanel;
    [SerializeField]
    public UnityEngine.UIElements.ScrollView agentView;
    public RectTransform teamSelectionObjects;
    public RectTransform planBuildingObjects;

    public UnityEngine.UI.Button viewToggle;

    public RectTransform agent0Card;
    public RectTransform agent1Card;
    public RectTransform agent2Card;


    [SerializeField]
    List<Agent> agents;
    [SerializeField]
    List<Transform> agentPanels;

    int selectedAgentId;

    List<Agent> team;
    List<List<PlanStep>> planSteps;
    // Start is called before the first frame update
    void Start()
    {
        team = new List<Agent>();
        LoadAgents();
        LoadAgentPanels();
        selectedAgentId = -1;
        planSteps = new List<List<PlanStep>>();
        for (int i = 0; i < 3; i++)
        {
            planSteps.Add(new List<PlanStep>());
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    private void LoadAgents()
    {
        agents = new List<Agent>();
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":3},{ \"type\":3,\"level\":0},{ \"type\":4,\"level\":0},{ \"type\":5,\"level\":20}],\"name\":\"Murphius\",\"id\":2, \"currentRoom\":-1,\"isInside\":false,\"cost\":100000,\"roleDesc\":\"Fastest hands in the west..<br>--The Recruiter.\"}"));
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":4},{ \"type\":1,\"level\":0},{ \"type\":2,\"level\":0},{ \"type\":0,\"level\":0},{ \"type\":4,\"level\":4},{ \"type\":5,\"level\":0}],\"name\":\"Noe\",\"id\":3, \"currentRoom\":-1,\"isInside\":false,\"cost\":25000,\"roleDesc\":\"Beefy, Jogs in her spare time.<br>--The Recruiter\"}"));
        agents.Add(JsonUtility.FromJson<Agent>("{ \"statList\":[{ \"type\":0,\"level\":0},{ \"type\":1,\"level\":3},{ \"type\":2,\"level\":0},{ \"type\":3,\"level\":2},{ \"type\":4,\"level\":0},{ \"type\":5,\"level\":4}],\"name\":\"Smoth\",\"id\":4, \"currentRoom\":-1,\"isInside\":false,\"cost\":25000,\"roleDesc\":\"Smart, decent at clearing a room.<br>--The Recruiter\"}"));
        Debug.Log(JsonUtility.ToJson(agents[1]));
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
            AddAgentToTeamDisplay(agentIndex);
        }
        if (team.Count >= 3)
        {
            DisableAddButtons();
        }

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
        targetCard.GetComponentInChildren<UnityEngine.UI.Image>().sprite = agents[agentIndex].sprite;
        Agent a = agents[agentIndex];
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

    public void RemoveAgentFromTeam(int agentId)
    {
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].id == agentId)
            {
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
        if (team.Count < 3)
        {
            EnableAddButtons();
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
        targetCard.GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
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
        targetCard.GetComponentInChildren<UnityEngine.UI.Image>().sprite = shiftCard.GetComponentInChildren<UnityEngine.UI.Image>().sprite;
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
            t.GetComponentInChildren<UnityEngine.UI.Button>().gameObject.SetActive(true);
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
        selectedAgentId = index;
    }

    TODO: add textbox that reports status of planStep creation attempt(sucess, failure & why, etc)
    public void ConstructPlanStep(Room room)
    {
        int len = planSteps[selectedAgentId].Count;
        double[,] travelMatrix = mapPanel.GetComponent<Map>().travelMatrix;
        if (len == 0 || room.id != planSteps[selectedAgentId][len - 1].roomNumber)
        {
            //creat move if possible.
        }
        //then creat makeCheck
        planSteps[selectedAgentId].Add(new PlanStep(AgentAction.MakeCheck, room.id, -1, room.check.timeToExecute));
    }

    //overload for enter/exit actions.
    public void ConstructPlanStep(Room room, AgentAction action)
    {
        int top = planSteps[selectedAgentId].Count - 1;
        if (room.isEntrance)
        {
            planSteps[selectedAgentId].Add(new PlanStep(action, room.id, -1, 10));
        }
        else
        { //notify failure due to bad path}

        }
    }


    //overload for wait type action.
    public void ConstructPlanStep(AgentAction action, double time)
    {
        int top = planSteps[selectedAgentId].Count - 1;
        planSteps[selectedAgentId].Add(new PlanStep(AgentAction.Wait, planSteps[selectedAgentId][top].roomNumber, -1, time));
    }
}
