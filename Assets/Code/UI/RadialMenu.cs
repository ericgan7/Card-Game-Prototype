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
    public float menuSize;
    public List<RadialButton> buttons;
    float offset = Mathf.Deg2Rad * 90f;

    public GameController game;

    public void Start()
    {
        buttons = new List<RadialButton>();
        game = FindObjectOfType<GameController>();
    }

    public void ActivateMenu(List<Action> actions)
    {
        Debug.Log("active");
        if (actions != null)
        {
            options = actions;
        }
        for (int i = 0; i < options.Count; ++i)
        {
            RadialButton button = Instantiate(prefab);
            buttons.Add(button);
            button.transform.SetParent(transform);
            //can change to variable number of buttons
            float theta = (2 * Mathf.PI / 7) * i;
            float x = Mathf.Sin(-theta - offset);
            float y = Mathf.Cos(-theta - offset);
            button.transform.localPosition = new Vector3(x, y, 0f) * menuSize;
            button.icon.sprite = options[i].image;
            button.frame.color = options[i].color;
            button.actionName = options[i].title;
            button.menu = this;
        }
    }

    public void DeactiveMenu()
    {
        Debug.Log("clear");
        foreach (RadialButton b in buttons)
        {
            Destroy(b.gameObject);
        }
        buttons.Clear();
    }
}
