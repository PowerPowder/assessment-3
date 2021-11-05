using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    enum PacmanSound { Move, EatPellet, HitWall }

    [SerializeField]
    private AudioClip moveSound;
    [SerializeField]
    private AudioClip eatPelletSound;
    [SerializeField]
    private AudioClip hitWallSound;

    private Tweener tweener;
    private AudioSource audioSource;
    private Animator animator;

    private ParticleSystem dust;
    private ParticleSystem bumpWall;

    private KeyCode lastInput;

    private bool startedMoving = false;
    private bool canHitWall = false;

    // RightAlt is acting as null
    private KeyCode currentInput = KeyCode.RightAlt;
    
    private int[] pos = { 1, 1 };
    private int[] old_pos = { 1, 1 };

    void Start()
    {
        tweener = GetComponent<Tweener>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        dust = GameObject.FindGameObjectWithTag("Dust").GetComponent<ParticleSystem>();
        bumpWall = GameObject.FindGameObjectWithTag("BumpWall").GetComponent<ParticleSystem>();
        bumpWall.Stop();
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

        if (!startedMoving && currentInput != KeyCode.RightAlt)
            startedMoving = true;

        if (tweener.waiting())
        {
            dust.Stop();

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

            bool playSoundClip = false;
            KeyCode animationInput = KeyCode.RightAlt;
            if (startedMoving)
            {
                if ((x != 0 || y != 0) && isWalkable(pos[0] + x, pos[1] + y))
                {
                    tweener.AddTween(transform, transform.position, newPos, 0.35f);
                    pos[0] += x;
                    pos[1] += y;
                    lastInput = currentInput;
                    playSoundClip = true;
                    animationInput = currentInput;

                    canHitWall = true;
                }
                else if (isWalkable(pos[0] + old_x, pos[1] + old_y))
                {
                    newPos = transform.position;

                    direction = getDirection(lastInput);
                    if (lastInput == KeyCode.W || lastInput == KeyCode.S)
                        direction.y *= -1f;
                    newPos += direction * unit;

                    tweener.AddTween(transform, transform.position, newPos, 0.35f);
                    pos[0] += old_x;
                    pos[1] += old_y;
                    playSoundClip = true;
                    animationInput = lastInput;

                    canHitWall = true;
                }
                else if (canHitWall)
                {
                    bumpWall.Play();
                    playPacmanSound(PacmanSound.HitWall);
                    canHitWall = false;
                }
            }

            if (playSoundClip && (pos[0] != old_pos[0] || pos[1] != old_pos[1]))
            {
                dust.Play();

                int pos_item = levelMap[pos[1], pos[0]];

                PacmanSound sound;
                if (pos_item == 5 || pos_item == 6)
                    sound = PacmanSound.EatPellet;
                else
                    sound = PacmanSound.Move;

                playPacmanSound(sound);
            }

            if ((pos[0] != old_pos[0] || pos[1] != old_pos[1]) && animationInput != KeyCode.RightAlt)
            {
                gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                switch (animationInput)
                {
                    case KeyCode.W: case KeyCode.S: animator.Play("UpAnim"); break;
                    case KeyCode.A: case KeyCode.D: animator.Play("RightAnim"); break;
                }
            }
            else if (animationInput == KeyCode.RightAlt)
                animator.Play("Idle");
        }

        old_pos[0] = pos[0];
        old_pos[1] = pos[1];
    }

    private void playPacmanSound(PacmanSound p)
    {
        if (p == PacmanSound.EatPellet)
            audioSource.clip = eatPelletSound;
        else if (p == PacmanSound.Move)
            audioSource.clip = moveSound;
        else
            audioSource.clip = hitWallSound;

        audioSource.enabled = true;
        audioSource.Play();
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
