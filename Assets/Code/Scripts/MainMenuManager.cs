using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public GameObject creditsBox;
    public GameObject tutorialBox;


    public List<Sprite> tutorialImages;
    int currentPage = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        
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
        tutorialBox.SetActive(true);
        tutorialBox.GetComponent<UnityEngine.UI.Image>().sprite = tutorialImages[currentPage];
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
