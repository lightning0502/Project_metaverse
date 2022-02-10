using System.Collections;
using UnityEngine;
using Extension;

public class CameraChaser : Singleton<CameraChaser>
{
    public Camera MainCamera;

    // ui object
    private Transform PlayerTransform;
    private Transform CameraTransform;

    // values
    public float CameraMoveTimer;

    // readonly
    private readonly float ReadonlyFloat_MaximumMapPositionX = 12.4f;
    private readonly float ReadonlyFloat_MaximumMapPositionY = 15.8f;
    private readonly int ReadonlyInt_MinusZPosition = -10;

    private void Awake()
    {
        CameraTransform = MainCamera.transform;
    }

    public void SetCameraSmoothMove(Transform playerObject)
    {
        if (object.ReferenceEquals(playerObject, null))
        {
            Debug.LogError("Error -> SetCameraMove : playerTransform is null");
            return;
        }

        PlayerTransform = playerObject;
        StartCoroutine(SmoothMoveCoroutine());
    }

    private IEnumerator SmoothMoveCoroutine()
    {
        Vector3 offset;
        GameObject playerObject = PlayerTransform.gameObject;

        while (playerObject.activeSelf)
        {
            if (CameraMoveTimer > 0)
            {
                // camera follow
                offset = Vector3.Lerp(CameraTransform.localPosition, PlayerTransform.localPosition, 0.02f);
                offset.z = ReadonlyInt_MinusZPosition;
                CameraTransform.localPosition = offset;

                if (offset.x < -ReadonlyFloat_MaximumMapPositionX)
                    CameraTransform.SetPosition(-ReadonlyFloat_MaximumMapPositionX, CameraTransform.localPosition.y, ReadonlyInt_MinusZPosition);

                else if (offset.x > ReadonlyFloat_MaximumMapPositionX)
                    CameraTransform.SetPosition(ReadonlyFloat_MaximumMapPositionX, CameraTransform.localPosition.y, ReadonlyInt_MinusZPosition);

                if (offset.y < -ReadonlyFloat_MaximumMapPositionY)
                    CameraTransform.SetPosition(CameraTransform.localPosition.x, -ReadonlyFloat_MaximumMapPositionY, ReadonlyInt_MinusZPosition);

                else if (offset.y > ReadonlyFloat_MaximumMapPositionY)
                    CameraTransform.SetPosition(CameraTransform.localPosition.x, ReadonlyFloat_MaximumMapPositionY, ReadonlyInt_MinusZPosition);

                CameraMoveTimer -= 0.01f;
                yield return Coop.WaitForSeconds(0.01f);
                Debug.Log(CameraMoveTimer);
            }

            else
                yield return Coop.WaitForSeconds(1);
        }
    }
}
