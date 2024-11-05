using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuScript : MonoBehaviour
{
    [SerializeField] private Color onMaterial;
    [SerializeField] private Color offMaterial;
    [SerializeField] List<GameObject> feuxOrdonnes;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject startUI;
    void turnOn(GameObject go){
        go.GetComponent<Renderer>().material.color = onMaterial;
    }

    void turnOff(GameObject go){
        go.GetComponent<Renderer>().material.color = offMaterial;
    }

    public IEnumerator StartSequence() {
        // Eteint tout les feux
        foreach (GameObject go in feuxOrdonnes)
        {
            turnOff(go);
        }

        // Joueur off
        player.GetComponent<MovementManager>().enabled = false;
        player.GetComponent<Boost>().enabled = false;

        //Allume les feux au fur et � mesure
        foreach (GameObject go in feuxOrdonnes){
            turnOn(go);
            yield return new WaitForSeconds(1f);
            
        }
        //Eteint tout les feux
        foreach (GameObject go in feuxOrdonnes){
            turnOff(go);
        }
        //Allume tout les feux au bout de 0.2 secondes
        yield return new WaitForSeconds(0.2f);
        foreach(GameObject go in feuxOrdonnes){
            turnOn(go);
        }

        //Joueur on
        player.GetComponent<MovementManager>().enabled = true;
        player.GetComponent<Boost>().enabled = true;
        startUI.SetActive(true);

    }


    void Start(){
        StartCoroutine("StartSequence");
    }

}
