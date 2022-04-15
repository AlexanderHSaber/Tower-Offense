using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject tower;
    public float speed;
    public Vector2 direction;
    public float stoppingDistance;
    public float health = 3;

    public int damage = 1;


    private float maxHealth;
    private HealthBarController healthBar;

    private Material material;
    private Coroutine flashRoutine;

    GameController gc;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();        

        if (!tower) tower = GameObject.FindGameObjectWithTag("Player");

        maxHealth = health;
        healthBar = GetComponentInChildren<HealthBarController>();

        material = GetComponent<SpriteRenderer>().material; // creates a new material instance for this renderer ; has to be destroyed manually before destroying gameobject
        if (gc.debug)
        {
            speed = gc.gameSpeed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        this.MoveTowardsTower();
    }

    private void MoveTowardsTower() {

        // Check distance from tower
        if (Vector2.Distance(tower.transform.position, this.transform.position) >= stoppingDistance)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, tower.transform.position, speed * Time.deltaTime);
        }
        else Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IProjectileType projectile = collision.gameObject.GetComponent<IProjectileType>();
        
        if (projectile != null)
        {
            TakeDamage(projectile.GetDamage());
        }

        TowerController tower = collision.gameObject.GetComponent<TowerController>();
        if (tower)
        {
            Die();
        }

        SlowZoneController slowZone = collision.gameObject.GetComponent<SlowZoneController>();
        if (slowZone)
        {
            Debug.Log("enter slow zone");
            speed = slowZone.getSlowMultiplier() * speed;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SlowZoneController slowZone = collision.gameObject.GetComponent<SlowZoneController>();
        if (slowZone)
        {
            Debug.Log("exit slow zone");
            speed = speed / slowZone.getSlowMultiplier();
        }
    }

    public void TakeDamage(float damage) 
    {
        health -= damage;        

        if (health <= 0)
        {            
            Die();
            return;
        }

        //flash on hit
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
        flashRoutine = StartCoroutine(Flash(0.2f, 0.1f));

        //update health bar on damage
        if (healthBar)
        {
            healthBar.SetValue(health / maxHealth);
        }
    }

    //lerp from alternate color to normal sprite appearance over rampTime
    private IEnumerator Flash(float rampTime, float holdTime = 0)
    {
        
        //hold effect at full strength before lerp
        if(holdTime > 0) {
            if (!material) yield break;

            material.SetFloat("_EffectStrength", 1);
            yield return new WaitForSeconds(holdTime);
        }

        float startTime = Time.time;

        //lerp back to normal appearance
        while(Time.time - startTime < rampTime)
        {

            if (!material) yield break;
            float t = (Time.time - startTime) / rampTime;
            float strength = Mathf.Lerp(1, 0, t);            
            
            material.SetFloat("_EffectStrength", strength);
            
            yield return null;
        }

        if(material) material.SetFloat("_EffectStrength", 0);
    }

    private void Die()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        Destroy(material); //clean up material instance
        Destroy(gameObject);
    }
}
