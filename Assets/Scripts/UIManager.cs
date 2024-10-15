using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI speedUI;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject ResumeButton;
    private bool onPause;
    private void Start()
    {
        onPause = false;
        PauseCanvas.SetActive(onPause);
    }


    // Update is called once per frame
    void Update()
    {
        speedUI.text = "" + (int)rb.velocity.magnitude;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("ya");
        if (onPause)
        {
            onPause = false;
            Time.timeScale = 1;
        }
        else
        {
            onPause = true;
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(ResumeButton);
        }
        PauseCanvas.SetActive(onPause);
    }

    public void ContinueButton()
    {
        if (!onPause)
        {
            return;
        }
        onPause = false;
        PauseCanvas.SetActive(onPause);
        Time.timeScale = 1;
    }

    public void MenuButton()
    {
        if (!onPause)
        {
            return;
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

}
