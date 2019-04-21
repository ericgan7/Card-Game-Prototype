using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardEffect : MonoBehaviour
{
    public static CardEffect instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
    }
    public Image img;
    private bool opening=false;
    private bool closing = false;
    bool waiting = false;
    public void openUp() {
        waiting = true;
        opening = false;closing = false;elapsed = 0;
    }float elapsed = 0;
    // Update is called once per frame
    void Update()
    {
        if (waiting) {
            elapsed += Time.deltaTime;
            if (elapsed > 0.4f) {
                opening = true;waiting = false; elapsed = 0; }
        }else if (opening) {
            img.color = Color.Lerp(img.color, Color.white, 0.2f);
            if (img.color.a > 0.95f) { opening = false;closing = true; }
        } else if (closing)
                {
            img.color = Color.Lerp(img.color, Color.clear, 0.2f);
            if (img.color.a <0.05f) {
                img.color = Color.clear;
                closing = false; }
        }
        
    }
}
