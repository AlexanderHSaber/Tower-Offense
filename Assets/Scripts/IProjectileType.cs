using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//interface for projectile classes

public interface IProjectileType
{
    public float GetDamage();
    public void SetTarget(Vector2 target);
}
