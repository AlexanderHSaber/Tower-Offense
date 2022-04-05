using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//extend this class to create upgrades
public abstract class BaseUpgrade : ScriptableObject
{
    [SerializeField]
    private string name = "an upgrade name";

    [SerializeField][TextArea]
    private string description = "the description";

    [SerializeField]
    private bool repeatable = true; //can you get it more than once?

    public List<BaseUpgrade> requirements;
    
    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public bool IsRepeatable()
    {
        return repeatable;
    }
}
