using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel = null;
    [SerializeField] private GameObject instructionPanel = null;
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GotoInstructions()
    {
        mainMenuPanel.SetActive(false);
        instructionPanel.SetActive(true);
    }

    public void GotoMainMenu()
    {
        mainMenuPanel.SetActive(true);
        instructionPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
