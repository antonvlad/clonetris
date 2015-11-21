using UnityEngine;
using System.Collections;

enum MoveDirection
{
    Left,
    Right,
    None
}

public class Tetrimino : MonoBehaviour
{

    // controls how fast the tetrimino is falling
    public float speed = 5.0f;
    public float moveSpeed = 0.1f;

    // controls whether the tetrimino should be falling at all
    // this is used when the piece has reached it's lowest position, but we want to give the user an opportunity to move/rotate
    public bool freeze = false;
    [HideInInspector]
    public GameController gameController;
    private float[] allowedRotations;
    private int currentRotationIndex = 0;
    private float nextDropTime = 0.0f;
    private float nextMoveTime = float.MaxValue;
    private MoveDirection moveDirection = MoveDirection.None;

    /// <summary>
    ///  We encapsulate the actual tetrimino blocks in a rotation container object to decouple the rotation of the tetrimino from the movement
    /// This simplifies move logic because the transform for the root GameObject is always correctly oriented
    /// </summary>
    public GameObject rotationContainer;

    public new bool enabled
    {
        get
        {
            return base.enabled;
        }
        set
        {
            base.enabled = value;
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = value;
            }
        }
    }

    void Awake()
    {
        PrecalculateRotations();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (Input.GetButtonDown("Rotate"))
            {
                Rotate();
            }

            bool isRightDown = Input.GetButtonDown("Right");
            bool isLeftDown = Input.GetButtonDown("Left");

            if (isRightDown && !isLeftDown && (moveDirection == MoveDirection.None))
            {
                nextMoveTime = Time.time;
                moveDirection = MoveDirection.Right;
            }
            else if (!isRightDown && isLeftDown && (moveDirection == MoveDirection.None))
            {
                nextMoveTime = Time.time;
                moveDirection = MoveDirection.Left;
            }

            if ((!Input.GetButton("Left") && moveDirection == MoveDirection.Left) || (!Input.GetButton("Right") && moveDirection == MoveDirection.Right))
            {
                moveDirection = MoveDirection.None;
            }

            ProcessMove();

            if (Input.GetButton("Drop"))
            {
                Debug.Log("Drop button is down");
            }
            MoveDown(Input.GetButton("Drop") ? 0.1f : speed);
        }
    }

    protected virtual void Rotate()
    {
        Debug.Log(string.Format("Rotating. Total rotations {0}. Current rotation {1}", allowedRotations, currentRotationIndex));
        if (allowedRotations != null)
        {
            int rotationIndex = (currentRotationIndex + 1) % allowedRotations.Length;
            rotationContainer.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, allowedRotations[rotationIndex]));
            currentRotationIndex = rotationIndex;
        }
    }

    void ProcessMove()
    {
        if (moveDirection != MoveDirection.None)
        {
            if (Time.time > nextMoveTime)
            {
                Vector2 translation = new Vector2(1.0f, 0.0f);
                if (moveDirection == MoveDirection.Left)
                {
                    translation *= -1.0f;
                }


                transform.Translate(translation);

                if (gameController.HasValidPosition(transform) == false)
                {
                    // can't go that way - move it back
                    transform.Translate(-translation);

                    // TODO: make some noise
                }

                nextMoveTime += moveSpeed;
            }
        }
    }

    void MoveDown(float effectiveSpeed)
    {
        // speed is expressed in seconds per line (i.e. it's really the delta time between successive moves)
        // all we need to do to see if we need to move is to check whether current time is larger than the next move timestamp
        if (Time.time > nextDropTime)
        {
            nextDropTime += effectiveSpeed;
            transform.Translate(new Vector2(0.0f, -1.0f));
        }
    }

    void PrecalculateRotations()
    {
        if (CompareTag("O"))
        {
            return;
        }
        else if (CompareTag("I") || CompareTag("S") || CompareTag("Z"))
        {
            allowedRotations = new float[2];
            allowedRotations[0] = 0.0f;
            allowedRotations[1] = 90.0f;
        }
        else
        {
            // these are L and J
            allowedRotations = new float[4];
            allowedRotations[0] = 0.0f;
            allowedRotations[1] = 90.0f;
            allowedRotations[2] = 180.0f;
            allowedRotations[3] = 270.0f;
        }
    }

}
