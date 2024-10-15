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
    public static UIManager UIManagerInstance { get; private set; }
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI speedUI;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject ResumeButton;
    private bool onPause;

    [SerializeField] private GameObject EndCanvas;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private finishLineBehavior finishLineBehavior;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (UIManagerInstance != null && UIManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            UIManagerInstance = this;
        }
    }
    private void Start()
    {
        onPause = false;
        PauseCanvas.SetActive(onPause);
        EndCanvas.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        speedUI.text = "" + (int)rb.velocity.magnitude;
    }

    public void Pause(InputAction.CallbackContext context)
    {
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

    public void EndCourse()
    {
        Debug.Log("ya");
        if (onPause)
        {
            return;
        }
        else
        {
            onPause = true;
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(restartButton);
        }
        EndCanvas.SetActive(onPause);
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
        EndCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
        onPause = false;
        finishLineBehavior.ResetForRestart();
        gameManager.GameManagerInstance.RestartCourse();
    }
}
