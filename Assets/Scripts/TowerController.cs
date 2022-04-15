using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerController : UpgradeableTower
{
    public GameController gc;

    

    private Material material;    
    private Coroutine flashRoutine;


    private HealthBarController healthBar;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUpgradeState();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        material = GetComponent<SpriteRenderer>().sharedMaterial; // project-wide reference to this material type; changes will show in all gameobjects using it
        material.SetFloat("_EffectStrength", 0);
        
        healthBar = GetComponentInChildren<HealthBarController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        //Debug.log("HELO");
        if (enemy)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(Flash(0.2f, 0.1f));

            //Debug.log("HELO2");
            TakeDamage(enemy.damage);
            
        }
    }

    private IEnumerator Flash(float rampTime, float holdTime = 0)
    {
        if(holdTime > 0)
        {
            material.SetFloat("_EffectStrength", 1);
            yield return new WaitForSeconds(holdTime);
        }

        float startTime = Time.time;

        while(Time.time - startTime < rampTime)
        {
            float t = (Time.time - startTime) / rampTime;
            float strength = Mathf.Lerp(1, 0, t);
            material.SetFloat("_EffectStrength", strength);            
            yield return null;
        }

        material.SetFloat("_EffectStrength", 0);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"took {amount} damage ; remaining {health} / {maxHealth}");
        UpdateHealthBar();
        if (health <= 0)
        {
            //Debug.log("DEAD");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar) healthBar.SetValue(Mathf.Max(0, health / maxHealth));
    }

    public override void ApplyUpgrade(BaseUpgrade upgrade)
    {
        base.ApplyUpgrade(upgrade);
        UpdateHealthBar();
    }
}
