using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityEngine;
using UnityEngine.UI;

public class InputMovementComponent : MonoBehaviour
{
    // component
    private Rigidbody2D PlayerRigidbody2D;
    private Transform PlayerTransform;

    // instance
    private SpriteAnimationComponent AnimationComponentInstance;
    private CameraChaser CameraChaserInstance;
    private MessageManager MessageManagerInstance;

    // readonly
    private readonly KeyCode KeyboardArrow_Left = KeyCode.LeftArrow;
    private readonly KeyCode KeyboardArrow_Right = KeyCode.RightArrow;
    private readonly KeyCode KeyboardArrow_Up = KeyCode.UpArrow;
    private readonly KeyCode KeyboardArrow_Down = KeyCode.DownArrow;
    private readonly float ReadonlyFloat_MovePower = 7f;
    private readonly string ReadonlyString_Vertical = "Vertical";
    private readonly string ReadonlyString_Horizontal = "Horizontal";

    // values
    private int CurrentID;
    private bool OnLerp;
    private Vector3 GoalPosition;

    private void Awake()
    {
        MessageManagerInstance = MessageManager.Instance;
        CameraChaserInstance = CameraChaser.Instance;
        OnLerp = false;
        GoalPosition = Vector2.zero;
    }

    public void InputCheckerStart(SpriteAnimationComponent animationComponent, ref int id, bool isOtherPlayer)
    {
        PlayerTransform = gameObject.transform;
        PlayerRigidbody2D = PlayerTransform.GetComponent<Rigidbody2D>();

        AnimationComponentInstance = animationComponent;
        CurrentID = id;

        if (isOtherPlayer)
            return;

        CameraChaserInstance.SetCameraSmoothMove(PlayerTransform);
        StartCoroutine(InputChecker());
        StartCoroutine(MoveChecker());
    }

    public void OnLerpMove(Vector3 vectorPosition)
    {
        GoalPosition = vectorPosition;

        if (OnLerp)
            return;

        OnLerp = true;
        StartCoroutine(StartLerpMove());
    }

    private IEnumerator StartLerpMove()
    {
        AnimationComponentInstance.OnMoveAnimation = true;

        while (Mathf.Approximately(PlayerTransform.localPosition.x, GoalPosition.x) == false && Mathf.Approximately(PlayerTransform.localPosition.y, GoalPosition.y) == false)
        {
            AnimationComponentInstance.SpriteFlipX = PlayerTransform.localPosition.x < GoalPosition.x;
            PlayerTransform.localPosition = Vector3.MoveTowards(PlayerTransform.localPosition, GoalPosition, 0.07f);
            yield return Coop.WaitForSeconds(0.02f);
        }

        AnimationComponentInstance.OnMoveAnimation = false;
        OnLerp = false;
    }

    private bool OnArrowInputKey(ref bool onLeft, ref bool onRight)
    {
        if (Input.anyKey)
        {
            onLeft = Input.GetKey(KeyboardArrow_Left);
            onRight = Input.GetKey(KeyboardArrow_Right);
            return onLeft || onRight || Input.GetKey(KeyboardArrow_Up) || Input.GetKey(KeyboardArrow_Down);
        }

        else
            return false;
    }

    private void FlipXChecker(bool onLeft, bool onRight)
    {
        if (onLeft)
            AnimationComponentInstance.SpriteFlipX = false;

        else if (onRight)
            AnimationComponentInstance.SpriteFlipX = true;
    }

    private IEnumerator InputChecker()
    {
        Vector2 offset = Vector2.zero;
        bool onLeft = false, onRight = false;

        while (gameObject.activeSelf)
        {
            if (OnArrowInputKey(ref onLeft, ref onRight))
            {
                // Acceleration applied
                offset.x = Input.GetAxis(ReadonlyString_Horizontal) * ReadonlyFloat_MovePower;
                offset.y = Input.GetAxis(ReadonlyString_Vertical) * ReadonlyFloat_MovePower;
                PlayerRigidbody2D.velocity = offset;

                // animation controls
                FlipXChecker(onLeft, onRight);
                AnimationComponentInstance.OnMoveAnimation = true;

                // camera follow move
                CameraChaserInstance.CameraMoveTimer = 1;

                yield return Coop.WaitForSeconds(0.08f);
            }

            else
            {
                AnimationComponentInstance.OnMoveAnimation = false;
                yield return Coop.WaitForSeconds(0.2f);
            }
        }

        Debug.LogError("InputChecker End!");
    }

    private IEnumerator MoveChecker()
    {
        bool onLeft = false, onRight = false;

        while (gameObject.activeSelf)
        {
            if (OnArrowInputKey(ref onLeft, ref onRight))
            {
                MessageManagerInstance.SendInformation(CurrentID, PlayerTransform.localPosition);
                yield return Coop.WaitForSeconds(0.33f);
            }

            else
                yield return Coop.WaitForSeconds(0.5f);
        }

        Debug.LogError("MoveChecker End!");
    }
}
