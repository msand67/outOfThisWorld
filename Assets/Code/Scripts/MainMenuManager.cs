using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public GameObject creditsBox;
    public GameObject tutorialBox;


    public List<Sprite> tutorialImages;
    public List<string> tutorialText;
    public TMPro.TextMeshProUGUI tutorialTextBox;
    int currentPage = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        tutorialText.Add("Welcome to the game: Gotta get out of this world!\n You are a corporate middleman who has grown tired of your mundane life. As such, you have used an assortment of paper in your office to create a life of a high profile corporate spy!");
        tutorialText.Add("On this screen you will assembe your team. You can have three agents per mission, add three below, then click on \"Team Select\"");
        tutorialText.Add("Here you must create a plan for each agent. They will need to enter the job site. Then you can right click a room to sent them to it. Rooms that are red are your target, they will turn blue when they are being completed by someone. Afterward, right click adjacent rooms until your agent can exit.\nThe mission ends when all agents are extracted and the required rooms are done.");
        tutorialText.Add("If you need to redo some planning, please use the controls here. When you are ready, Start Mission!\n(You can left click on a room for details in the bottom right!)");
        tutorialText.Add("This plan looks good to go, as all required rooms are green!\n(As a note, you can click on \"return to menu\" to return to the menu at any time. This will let you quit the game too!");
        tutorialText.Add("This is the mission screen. Activate your time machine to start the clock. You can pause at any time.");
        tutorialText.Add("Here you can see a room has been completed, looks like Deadeye is on a roll!");
        tutorialText.Add("Now that all goals are completed, all that's left to do is wait for the agents to extract. After that, you'll get a results screen and be returned to the planning screen.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreditButtonClick()
    {
            creditsBox.gameObject.SetActive(!creditsBox.gameObject.activeSelf);
    }
    public void TutorialButtonClick()
    {
        currentPage = 0;
        tutorialBox.SetActive(true);
        tutorialBox.GetComponent<UnityEngine.UI.Image>().sprite = tutorialImages[currentPage];
        tutorialTextBox.text = tutorialText[0];
    }

    public void NextTutorialImage()
    {

        currentPage++;
        if (currentPage == tutorialImages.Count)
        {
            currentPage = 0;
            tutorialBox.SetActive(false);

        }
        tutorialBox.GetComponent<UnityEngine.UI.Image>().sprite = tutorialImages[currentPage];
        tutorialTextBox.text = tutorialText[currentPage];
    }
    public void PreviousTutorialImage()
    {
        currentPage--;
        if (currentPage <0)
        {
            currentPage = 0;
            tutorialBox.SetActive(false);
        }
        tutorialBox.GetComponent<UnityEngine.UI.Image>().sprite = tutorialImages[currentPage];
        tutorialTextBox.text = tutorialText[currentPage];
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Mission");
    }
}
