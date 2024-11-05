using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndDeactivate());
        
    }

    private IEnumerator WaitAndDeactivate()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
