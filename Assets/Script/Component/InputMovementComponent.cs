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
    private MessageManager MessageManagerInstance;

    // readonly
    private readonly KeyCode KeyboardArrow_Left = KeyCode.LeftArrow;
    private readonly KeyCode KeyboardArrow_Right = KeyCode.RightArrow;
    private readonly KeyCode KeyboardArrow_Up = KeyCode.UpArrow;
    private readonly KeyCode KeyboardArrow_Down = KeyCode.DownArrow;
    private readonly float ReadonlyFloat_MovePower = 0.06f;

    public int CurrentID;

    public void InputCheckerStart(SpriteAnimationComponent animationComponent, ref int id, bool onCurrentPlayer)
    {
        PlayerTransform = gameObject.transform;
        PlayerRigidbody2D = PlayerTransform.GetComponent<Rigidbody2D>();
        MessageManagerInstance = MessageManager.Instance;

        AnimationComponentInstance = animationComponent;
        CurrentID = id;

        if (onCurrentPlayer)
            return;

        StartCoroutine(InputChecker());
        StartCoroutine(CameraChaser.Instance.StartCameraMove());
        StartCoroutine(PositionChecker());
    }

    public void SetLocalPosition(int id, Vector2 vectorPosition)
    {
        Debug.Log("id : " + CurrentID + " / SetLocalPosition : " + vectorPosition);
        PlayerRigidbody2D.MovePosition(vectorPosition);
    }

    private IEnumerator InputChecker()
    {
        Vector2 offset;

        while (gameObject.activeSelf)
        {
            if (Input.anyKey)
            {
                offset = PlayerTransform.localPosition;

                if (Input.GetKey(KeyboardArrow_Left))
                {
                    offset.x -= ReadonlyFloat_MovePower;
                    PlayerRigidbody2D.MovePosition(offset);
                    AnimationComponentInstance.OnMoveAnimation = true;
                    AnimationComponentInstance.SetSpriteFlipX = false;
                }

                else if (Input.GetKey(KeyboardArrow_Right))
                {
                    offset.x += ReadonlyFloat_MovePower;
                    PlayerRigidbody2D.MovePosition(offset);
                    AnimationComponentInstance.OnMoveAnimation = true;
                    AnimationComponentInstance.SetSpriteFlipX = true;
                }

                if (Input.GetKey(KeyboardArrow_Up))
                {
                    offset.y += ReadonlyFloat_MovePower;
                    PlayerRigidbody2D.MovePosition(offset);
                    AnimationComponentInstance.OnMoveAnimation = true;
                }

                else if (Input.GetKey(KeyboardArrow_Down))
                {
                    offset.y -= ReadonlyFloat_MovePower;
                    PlayerRigidbody2D.MovePosition(offset);
                    AnimationComponentInstance.OnMoveAnimation = true;
                }

                yield return Coop.WaitForSeconds(0.015f);
            }

            else
            {
                AnimationComponentInstance.OnMoveAnimation = false;
                yield return Coop.WaitForSeconds(0.15f);
            }
        }

        Debug.Log("Input End!");
    }

    private IEnumerator PositionChecker()
    {
        while (gameObject.activeSelf)
        {
            if (Input.anyKey)
            {
                MessageManagerInstance.SendInformation(CurrentID, PlayerTransform.localPosition);
                yield return Coop.WaitForSeconds(0.25f);
            }

            yield return Coop.WaitForSeconds(1);
        }
    }
}
