using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Choice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RewardManager rw;
    public Vector3 scale;
    public float speed;
    public bool isChoice;

    public Image artwork;
    public TextMeshProUGUI cardname;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;

    public void Start()
    {
        rw = FindObjectOfType<RewardManager>();
        isChoice = false;
        scale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void UpdateStat(Reward r, Character c)
    {
        if (!r.stat)
        {
            artwork.sprite = r.art;
            cardname.text = r.name;
            description.text = r.GetDescription(c);
            cost.text = r.energyCost.ToString();
        }
        else
        {
            artwork.sprite = r.art;
            cardname.text = r.name;
            description.text = r.Stats();
            cost.text = "";
        }
    }

    public void Update()
    {
        transform.localScale = Vector3.Slerp(transform.localScale, scale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData p)
    {
        scale = new Vector3(3.0f, 3.0f, 3.0f);
    }

    public void OnPointerExit(PointerEventData p)
    {
        scale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerClick(PointerEventData p)
    {
        rw.MakeChoice(this);
    }
}
