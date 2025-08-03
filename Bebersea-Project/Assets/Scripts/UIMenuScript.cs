using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject panduan1;
    public GameObject panduan2;
    public GameObject panduan3;
    public GameObject tentang1;
    public GameObject tentang2;


    void Start()
    {
        ShowOnly(mainMenu);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SubScene");
    }

    public void ShowMainMenu()
    {
        ShowOnly(mainMenu);
    }

    public void ShowPanduan1()
    {
        ShowOnly(panduan1);
    }

    public void ShowPanduan2()
    {
        ShowOnly(panduan2);
    }

    public void ShowPanduan3()
    {
        ShowOnly(panduan3);
    }

    public void ShowTentang1()
    {
        ShowOnly(tentang1);
    }

    public void ShowTentang2()
    {
        ShowOnly(tentang2);
    }

    public void quitGame()
    {
        Application.Quit();
    }
    
    private void ShowOnly(GameObject toShow)
    {
        mainMenu.SetActive(false);
        panduan1.SetActive(false);
        panduan2.SetActive(false);
        panduan3.SetActive(false);
        tentang1.SetActive(false);
        tentang2.SetActive(false);
        
        toShow.SetActive(true);
    }
}
