using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//test for health bar shader
public class LoseHealth : MonoBehaviour
{
    private Material hb;

    public float value = 1;
    public float timeScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        hb = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Sin(Time.time * timeScale);

        value = (value + 1) / 2;

        hb.SetFloat("_Value", value);
    }
}
