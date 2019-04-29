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

    public void Start()
    {
        game = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
    }

    public void Set(List<Sprite> anim, bool allyAttacker)
    {
        gameObject.SetActive(true);
        if (allyAttacker)
        {
            ally.sprite = anim[0];
            enemy.sprite = anim[1];
            animator.Play("AllyAttack");
        }
        else
        {
            ally.sprite = anim[1];
            enemy.sprite = anim[0];
            animator.Play("EnemyAttack");
        }
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
