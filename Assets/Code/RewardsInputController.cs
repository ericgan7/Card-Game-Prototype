using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsInputController : MonoBehaviour
{
    public RewardsController rewardsController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rewardsController.allyIndex = rewardsController.allyIndex == 0 ? 
                rewardsController.allies.Count - 1 : (rewardsController.allyIndex - 1) % rewardsController.allies.Count;
            rewardsController.UIController.SelectCharacter(rewardsController.allies[rewardsController.allyIndex]);
            rewardsController.DrawCurrentRewards();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rewardsController.allyIndex = (rewardsController.allyIndex + 1) % rewardsController.allies.Count;
            rewardsController.UIController.SelectCharacter(rewardsController.allies[rewardsController.allyIndex]);
            rewardsController.DrawCurrentRewards();
        }
    }
}
