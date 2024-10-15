using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool carPassed;
    private void Start()
    {
        carPassed = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        carPassed = true;

    }
}
