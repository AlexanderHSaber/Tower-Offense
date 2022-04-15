using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowInitialProjectileController : MonoBehaviour
{
    public GameObject slowZonePrefab;
    private Vector2 target;

    public float speed;
    private float eSpeed;


    public GameController gc;

    public float range;

    private Vector3 lastPosition;


    private float sqDistTraveled;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sqDistTraveled = 0;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        eSpeed = gc.debug ? speed * gc.gameSpeed : speed;

        transform.Translate(Vector2.right * eSpeed * Time.deltaTime, Space.Self);

        sqDistTraveled += Vector2.SqrMagnitude(transform.position) - Vector2.SqrMagnitude(lastPosition);
        lastPosition = transform.position;

        if (range > 0 && sqDistTraveled > range * range)
        {
            SpawnSlowZone();
        }
    }

    public void setRange(float r) 
    {
        range = r;
    }

    private void SpawnSlowZone()
    {
        Instantiate(slowZonePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
