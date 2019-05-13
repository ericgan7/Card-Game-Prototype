using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Icons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Effect statusEffect;
    public GameObject hover;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public Image icon;

    public void SetEffect(Effect e, Character self)
    {
        statusEffect = e;
        title.text = e.name;
        description.text = e.ToString(self);
    }

    public void OnPointerEnter(PointerEventData p)
    {
        hover.SetActive(true);
    }

    public void OnPointerExit(PointerEventData p)
    {
        hover.SetActive(false);
    }
    
}
