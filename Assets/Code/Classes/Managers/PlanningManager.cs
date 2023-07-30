using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using dataStructures;
public class PlanningManager : MonoBehaviour
{
    public RectTransform mapPanel;
    [SerializeField]
    public UnityEngine.UIElements.ScrollView agentView;


    [SerializeField]
    List<Agent> agents;
    [SerializeField]
    List<Transform> agentPanels;

    List<Agent> team;
    // Start is called before the first frame update
    void Start()
    {
        team = new List<Agent>();
        LoadAgents();
        LoadAgentPanels();
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
        }
        //change button to remove instead of add agent.
        UnityEngine.UI.Button button = agentPanels[agentIndex].GetComponentInChildren<UnityEngine.UI.Button>();
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Remove agent";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { RemoveAgentFromTeam(agents[agentIndex].id); });
        if (team.Count >= 3)
        {
            DisableAddButtons();
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

                break;
            }
        }
        if (team.Count < 3)
        {
            EnableAddButtons();
        }


    }
    public int GetIndexOfAgent(int agentId)
    {
        for(int i = 0; i<agents.Count; i++)
        {
            if(agents[i].id == agentId)
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


    }
    public void MoveToPlanning()
    {

    }
}
