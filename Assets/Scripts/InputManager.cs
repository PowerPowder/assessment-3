using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject item;
    private Tweener tweener;

    private Animator animator;
    private AudioSource moveSound;

    int i = 0;

    enum Direction { LEFT, RIGHT, UP, DOWN }
    Direction currentDirection = Direction.RIGHT;

    void Start()
    {
        animator = item.GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
        moveSound = item.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (tweener.waiting())
        {
            if (i > 3)
            {
                i = 0;
                currentDirection = Direction.RIGHT;
            }

            //tweener.AddTween(item.transform, item.transform.position, tweenLocations[i], 1f);
            item.transform.eulerAngles = new Vector3(0f, 0f, 0f);

            switch (currentDirection)
            {
                case Direction.UP:
                    animator.Play("UpAnim");
                    break;
                case Direction.DOWN:
                    animator.Play("UpAnim");
                    break;
                case Direction.LEFT:
                    animator.Play("RightAnim");
                    break;
                case Direction.RIGHT:
                    animator.Play("RightAnim");
                    break;
            }

            i++; // move to next tween location and get the direction pacman will go

            switch (i)
            {
                case 0:
                    currentDirection = Direction.RIGHT;
                    break;
                case 1:
                    currentDirection = Direction.DOWN;
                    break;
                case 2:
                    currentDirection = Direction.LEFT;
                    break;
                case 3:
                    currentDirection = Direction.UP;
                    break;
            }
        }
    }
}
