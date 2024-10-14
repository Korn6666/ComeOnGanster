using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuScript : MonoBehaviour
{
    [SerializeField] private Color onMaterial;
    [SerializeField] private Color offMaterial;
    [SerializeField] List<GameObject> feuxOrdonnes;
    [SerializeField] private GameObject player;

    void turnOn(GameObject go){
        go.GetComponent<Renderer>().material.color = onMaterial;
    }

    void turnOff(GameObject go){
        go.GetComponent<Renderer>().material.color = offMaterial;
    }

    public IEnumerator StartSequence() {
        foreach(GameObject go in feuxOrdonnes){
            turnOn(go);
            yield return new WaitForSeconds(1f);
            
        }
        foreach(GameObject go in feuxOrdonnes){
            turnOff(go);
        }
        yield return new WaitForSeconds(0.2f);
        foreach(GameObject go in feuxOrdonnes){
            turnOn(go);
        }

        player.GetComponent<MovementManager>().enabled = true;
        player.GetComponent<Boost>().enabled = true;
    }


    void Start(){
        StartCoroutine("StartSequence");
    }

}
