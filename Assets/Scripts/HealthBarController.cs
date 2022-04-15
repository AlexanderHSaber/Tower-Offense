using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBarController : MonoBehaviour
{
    private Material hbm; // HealthBarMaterial used by SpriteRenderer on this GameObject
    private const string materialValueParam = "_Value"; // reference name in HealthBarShader for Value property 

    // Start is called before the first frame update
    void Start()
    {
        hbm = GetComponent<SpriteRenderer>().material;
    }

    public void SetValue(float value)
    {
        hbm.SetFloat(materialValueParam, value);
    }

    private void OnDestroy()
    {
        if(hbm) Destroy(hbm);
    }
}
