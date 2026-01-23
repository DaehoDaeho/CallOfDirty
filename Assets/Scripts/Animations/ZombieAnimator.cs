using UnityEngine;

public class ZombieAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool move = false;
    
    // Update is called once per frame
    void Update()
    {
        CheckTestInput();
    }

    void CheckTestInput()
    {
        if(Input.GetKeyDown(KeyCode.M) == true)
        {
            move = !move;
        }

        animator.SetBool("Move", move);

        if(Input.GetKeyDown(KeyCode.K) == true)
        {
            animator.SetTrigger("Attack");
        }
    }
}
