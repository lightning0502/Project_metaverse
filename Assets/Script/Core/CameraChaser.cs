using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extension;

public class CameraChaser : Singleton<CameraChaser>
{
    public Transform PlayerTransform;
    private GameObject PlayerGameObject;

    public Camera MainCamera;
    private Transform CameraTransform;
    private Vector3 CameraPosition;

    private readonly float ReadonlyFloat_PositionX = 12.4f;
    private readonly float ReadonlyFloat_PositionY = 15.8f;
    private readonly int ReadonlyInt_MinusZPosition = -10;

    private void Awake()
    {
        CameraTransform = MainCamera.transform;
    }

    public IEnumerator StartCameraMove()
    {
        if (object.ReferenceEquals(PlayerTransform, null))
        {
            Debug.LogError("Error -> StartCameraMove : PlayerTransform is null");
            yield break;
        }

        else
            PlayerGameObject = PlayerTransform.gameObject;

        float playerX, playerY;

        while (PlayerGameObject.activeSelf)
        {
            if (Input.anyKey)
            {
                playerX = PlayerTransform.localPosition.x;
                playerY = PlayerTransform.localPosition.y;

                CameraTransform.SetPosition(playerX, playerY, ReadonlyInt_MinusZPosition);
                CameraPosition = CameraTransform.localPosition;

                if (CameraPosition.x < -ReadonlyFloat_PositionX)
                    CameraTransform.SetPosition(-ReadonlyFloat_PositionX, CameraPosition.y, ReadonlyInt_MinusZPosition);

                else if (CameraPosition.x > ReadonlyFloat_PositionX)
                    CameraTransform.SetPosition(ReadonlyFloat_PositionX, playerY, ReadonlyInt_MinusZPosition);

                if (CameraPosition.y < -ReadonlyFloat_PositionY)
                    CameraTransform.SetPosition(CameraPosition.x, -ReadonlyFloat_PositionY, ReadonlyInt_MinusZPosition);

                else if (CameraPosition.y > ReadonlyFloat_PositionY)
                    CameraTransform.SetPosition(CameraPosition.x, ReadonlyFloat_PositionY, ReadonlyInt_MinusZPosition);

                yield return Coop.WaitForSeconds(0.015f);
            }

            else
                yield return Coop.WaitForSeconds(0.15f);
        }
    }
}
