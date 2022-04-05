using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZoneController : MonoBehaviour
{
    public float timeRemaining = 5;
    public float slowMultiplier = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if(timeRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float getSlowMultiplier()
    {
        return slowMultiplier;
    }
}
