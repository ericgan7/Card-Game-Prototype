using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : ScriptableObject
{
    public new string name;
    public Sprite art;
    public int energyCost;

    public bool stat;

    public virtual string GetDescription(Character origin) { return ""; }
    public virtual string Stats()
    {
        return "";
    }
}
