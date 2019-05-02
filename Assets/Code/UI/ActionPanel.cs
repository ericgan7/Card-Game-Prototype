using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour
{
    public GameController game;
    public Animator animator;
    public float distance;
    public Image ally;
    public Image enemy;
    public List<Effect.EffectResult> results;
    public bool attacker;
    public Text damageText;
    public float speed;

    public void Start()
    {
        game = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
    }

    public void Set(List<Effect.EffectResult> r, bool allyAttacker)
    {
        /*
        gameObject.SetActive(true);
        attacker = allyAttacker;
        results = r;
        if (allyAttacker)
        {
            ally.sprite = r[0].sprite;
            enemy.sprite = r[1].sprite;
            animator.Play("AllyAttack");
        }
        else
        {
            ally.sprite = r[1].sprite;
            enemy.sprite = r[0].sprite;
            animator.Play("EnemyAttack");
        }
        */
    }
    public void DisplayText()
    {
        /*
        RectTransform prt;
        if (attacker)
        {
            prt = ally.GetComponent<RectTransform>();
        }
        else
        {
            prt = enemy.GetComponent<RectTransform>();
        }
        foreach(Effect.EffectResult amount in results)
        {
            Text d = Instantiate(damageText) as Text;
            d.text = amount.ToString();
            d.transform.SetParent(this.transform);
            RectTransform rt = d.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(prt.anchoredPosition.x, prt.anchoredPosition.y + prt.sizeDelta.y / 3);
            StartCoroutine(MoveText(rt));
        }
        */
    }

    IEnumerator MoveText(RectTransform t)
    {
        float angle = Random.Range(0, 90) * Mathf.Deg2Rad;
        float y = Mathf.Sin(angle);
        float x = Mathf.Cos(angle);
        Vector2 destination = new Vector2(x, y);
        float elapsed = 0f;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime;
            t.anchoredPosition = Vector2.Lerp(t.anchoredPosition, t.anchoredPosition + destination, speed);
            yield return new WaitForFixedUpdate();
        }

    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
