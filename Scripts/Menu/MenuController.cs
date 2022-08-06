using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MenuController : MonoBehaviour
{
    public static bool GamePaused = false;
    private bool escPressed = false;
    private bool prevEscPressed = false;
    public bool inSettings = false;
    MenuInput menuInput;

    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject ThirdPersonCamera;

    void Awake()
    {
        menuInput = new MenuInput();

        menuInput.MenuControls.Menu.started += inp => { escPressed = inp.ReadValueAsButton(); };
        menuInput.MenuControls.Menu.canceled += inp => { escPressed = inp.ReadValueAsButton(); };
    }


    public void PlayButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }


    void Update()
    {
        if (prevEscPressed && !escPressed)
        {
            if (GamePaused)
            {
                if (inSettings)
                {
                    ReturnToMainMenu();
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
        prevEscPressed = escPressed;
    }

    public void GoToSettingsMenu()
    {
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
        inSettings = true;
    }

    public void ReturnToMainMenu()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
        inSettings = false;
    }

    public void ResumeGame()
    {
        MainMenu.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1;
        ThirdPersonCamera.SetActive(true);
    }
    public void PauseGame()
    {
        MainMenu.SetActive(true);
        GamePaused = true;
        Time.timeScale = 0;
        ThirdPersonCamera.SetActive(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }





    public void OnEnable()
    {
        menuInput.MenuControls.Menu.Enable();
    }

    public void OnDisable()
    {
        menuInput.MenuControls.Menu.Disable();
    }


}
