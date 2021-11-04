using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    private KeyCode lastInput;

    // RightAlt is acting as null
    private KeyCode currentInput = KeyCode.RightAlt;
    
    private int[] pos = { 1, 1 };

    void Start()
    {
        tweener = GetComponent<Tweener>();
        getMap(pos[0], pos[1]);
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.W))
            currentInput = KeyCode.W;
        if (Input.GetKeyDown(KeyCode.S))
            currentInput = KeyCode.S;
        if (Input.GetKeyDown(KeyCode.A))
            currentInput = KeyCode.A;
        if (Input.GetKeyDown(KeyCode.D))
            currentInput = KeyCode.D;

        if (tweener.waiting())
        {
            Vector3 newPos = transform.position;
            float unit = 0.16f;

            Vector3 direction = getDirection(currentInput);
            if (currentInput == KeyCode.W || currentInput == KeyCode.S)
                direction.y *= -1f;
            newPos += direction * unit;

            int x = (int)getDirection(currentInput).x;
            int y = (int)getDirection(currentInput).y;

            int old_x = (int)getDirection(lastInput).x;
            int old_y = (int)getDirection(lastInput).y;

            if ((x != 0 || y != 0) && isWalkable(pos[0] + x, pos[1] + y))
            {
                tweener.AddTween(transform, transform.position, newPos, 0.25f);
                pos[0] += x;
                pos[1] += y;
                lastInput = currentInput;
            }
            else if (isWalkable(pos[0] + old_x, pos[1] + old_y))
            {
                newPos = transform.position;

                direction = getDirection(lastInput);
                if (lastInput == KeyCode.W || lastInput == KeyCode.S)
                    direction.y *= -1f;
                newPos += direction * unit;

                tweener.AddTween(transform, transform.position, newPos, 0.25f);
                pos[0] += old_x;
                pos[1] += old_y;
            }
        }
    }

    private Vector3 getDirection(KeyCode keyCode)
    {
        Vector3 direction = Vector3.zero;

        switch (keyCode)
        {
            case KeyCode.W: direction.y = -1f; break;
            case KeyCode.S: direction.y =  1f; break;
            case KeyCode.A: direction.x = -1f; break;
            case KeyCode.D: direction.x =  1f; break;
        }

        return direction;
    }

    private bool isWalkable(int x, int y)
    {
        if ((y < 0 || x < 0) || (y >= 30 || x >= 28))
            return false;

        switch (levelMap[y, x])
        {
            case 0: case 5: case 6:
                return true;
            default:
                return false;
        }
    }

    private int[,] levelMap =
    {
        { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 7, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4, 4, 5, 4, 0, 0, 0, 4, 5, 4, 0, 0, 4, 6, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 2 },
        { 1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4, 4, 0, 3, 4, 4, 3, 4, 5, 1, 2, 2, 2, 2, 1 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3, 3, 0, 3, 4, 4, 3, 4, 5, 2, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0, 0, 4, 4, 3, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0 },
        { 2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 3, 3, 5, 1, 2, 2, 2, 2, 2 },
        { 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0 },
        { 2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 3, 3, 5, 1, 2, 2, 2, 2, 2 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0, 0, 4, 4, 3, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3, 3, 0, 3, 4, 4, 3, 4, 5, 2, 0, 0, 0, 0, 0 },
        { 1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4, 4, 0, 3, 4, 4, 3, 4, 5, 1, 2, 2, 2, 2, 1 },
        { 2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4, 4, 5, 4, 0, 0, 0, 4, 5, 4, 0, 0, 4, 6, 2 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2 },
        { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 7, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 }
    };

    void getMap(int x, int y)
    {
        string str = "";
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                str += (i == x && j == y) ? "*" : "" + levelMap[i, j];
                str += "\t";
            }
            str += "\n";
        }
        Debug.Log(str);
    }
}
