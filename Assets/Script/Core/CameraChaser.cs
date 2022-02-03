using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extension;

public class CameraChaser : Singleton<CameraChaser>
{
    public Camera MainCamera;

    // values
    private Transform PlayerTransform;
    private GameObject PlayerGameObject;
    private Transform CameraTransform;
    private Vector3 CameraPosition;

    // readonly
    private readonly float ReadonlyFloat_MaximumMapPositionX = 12.4f;
    private readonly float ReadonlyFloat_MaximumMapPositionY = 15.8f;
    private readonly int ReadonlyInt_MinusZPosition = -10;

    private void Awake()
    {
        CameraTransform = MainCamera.transform;
    }

    public void SetCameraMove(Transform playerObject)
    {
        if (object.ReferenceEquals(playerObject, null))
        {
            Debug.LogError("Error -> SetCameraMove : playerTransform is null");
            return;
        }

        PlayerTransform = playerObject;
        PlayerGameObject = PlayerTransform.gameObject;

        StartCoroutine(StartCameraMove());
    }

    private IEnumerator StartCameraMove()
    {
        float playerX, playerY;

        while (PlayerGameObject.activeSelf)
        {
            playerX = PlayerTransform.localPosition.x;
            playerY = PlayerTransform.localPosition.y;

            CameraTransform.SetPosition(playerX, playerY, ReadonlyInt_MinusZPosition);
            CameraPosition = CameraTransform.localPosition;

            if (Input.anyKey)
            {
                if (CameraPosition.x < -ReadonlyFloat_MaximumMapPositionX)
                    CameraTransform.SetPosition(-ReadonlyFloat_MaximumMapPositionX, CameraPosition.y, ReadonlyInt_MinusZPosition);

                else if (CameraPosition.x > ReadonlyFloat_MaximumMapPositionX)
                    CameraTransform.SetPosition(ReadonlyFloat_MaximumMapPositionX, playerY, ReadonlyInt_MinusZPosition);

                if (CameraPosition.y < -ReadonlyFloat_MaximumMapPositionY)
                    CameraTransform.SetPosition(CameraPosition.x, -ReadonlyFloat_MaximumMapPositionY, ReadonlyInt_MinusZPosition);

                else if (CameraPosition.y > ReadonlyFloat_MaximumMapPositionY)
                    CameraTransform.SetPosition(CameraPosition.x, ReadonlyFloat_MaximumMapPositionY, ReadonlyInt_MinusZPosition);
            }

            yield return Coop.WaitForSeconds(0.02f);
        }
    }
}
