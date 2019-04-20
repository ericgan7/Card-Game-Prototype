using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [System.Serializable]
    public class Action
    {
        public Color color;
        public Sprite image;
        public string title;
    }

    public List<Action> options;
    public RadialButton prefab;

    public void ActivateMenu()
    {
        foreach (Action type in options)
        {
            RadialButton button = Instantiate(prefab);
            
        }
    }
}
