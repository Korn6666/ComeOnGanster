using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class finishLineBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private TextMeshProUGUI bestTimerUI;
    [SerializeField] private TextMeshProUGUI turnTimersUI;


    public int nLaps;
    public float lapTime;
    public float bestLapTime;
    private List<float> turnTimers;
    [SerializeField] private CheckPoint checkPointSript;
    private bool carPassedCheckPoint;
    private void Start()
    {
        turnTimers = new List<float>();
        bestLapTime = 100;
    }
    void OnTriggerEnter(Collider col){
        if (nLaps == 0)
        {
            turnTimers.Clear();
            turnTimersUI.text = "";
            checkPointSript.carPassed = true;
        }
        carPassedCheckPoint = checkPointSript.carPassed;

        //Arrivé d'un tour
        if (nLaps>0 && carPassedCheckPoint)
        {
            // turnTimers
            turnTimers.Add(lapTime);
            turnTimersUI.text += "Turn" + turnTimers.Count + ": " + lapTime.ToString("0.00") + "<br>";
        }

        //Incrémentation si carPassedCheckPoint. Au nLaps = 0, il est déjà initialisé à true
        if (carPassedCheckPoint)
        {
            checkPointSript.carPassed = false;
            nLaps += 1;
        }
        

        // Arrivée du 3ème/dernier tour
        if (nLaps > 3)
        {
            if (lapTime < bestLapTime)
            {
                bestLapTime = lapTime;
                bestTimerUI.text = "Record: " + bestLapTime.ToString("0.00");
            }
            lapTime = 0;
            nLaps = 0;

            UIManager.UIManagerInstance.EndCourse();

        }
    }

    public void ResetForRestart()
    {
        lapTime = 0;
        nLaps = 0;

        timerUI.text = "Timer: " + lapTime;
    }

    void Update(){
        //Debug.Log("oui");
        if(nLaps > 0) {
            lapTime += Time.deltaTime;
            //Debug.Log(lapTime);
            timerUI.text = "Timer: " + lapTime.ToString("0.00");
        }
    }
}
