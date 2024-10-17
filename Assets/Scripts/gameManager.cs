using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public  class gameManager : MonoBehaviour
{
    public static gameManager GameManagerInstance;

    [SerializeField] private Transform playerSpawn;
    [SerializeField] private GameObject player;
    [SerializeField] private FeuScript feuScript;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (GameManagerInstance != null && GameManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            GameManagerInstance = this;
        }
    }

    public void RestartCourse()
    {
        //Player to spawn
        player.transform.position = playerSpawn.position;
        player.transform.rotation = playerSpawn.rotation;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Boost>().BoostReset();

        //Feux
        StopCoroutine(feuScript.StartSequence());
        StartCoroutine(feuScript.StartSequence());
    }
}
