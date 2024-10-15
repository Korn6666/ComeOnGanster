using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class finishLineBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI speedUI;
    public int nLaps;
    public float lapTime;

    void OnTriggerEnter(Collider col){
        nLaps +=1;
        lapTime = 0;
    }

    void Update(){
        //Debug.Log("oui");
        if(nLaps > 0) {
            lapTime += Time.deltaTime;
            //Debug.Log(lapTime);
            speedUI.text = "" + lapTime;
        }

    }
}
