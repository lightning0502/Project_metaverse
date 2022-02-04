using UnityEngine;
using Extension;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    // values
    private readonly int ReadonlyInt_MinusZPosition = -2;
    private readonly int ReadonlyInt_CharacterScale_2 = 2;
    private readonly int ReadonlyInt_LayerIgnoreCollision_3 = 3;
    private Vector3 VectorPosition;

    public GameObject PlayerPrefab;
    public Transform ActivePlayerListTransform;

    private int CurrentPlayerID;

    private Dictionary<int, GameObject> DictionaryObject_Player;
    private Dictionary<int, PlayerInformation> DictionaryComponent_NetworkInformation;

    // instance
    private MessageManager MessageManagerInstance;

    private void Awake()
    {
        CurrentPlayerID = -10;
        DictionaryObject_Player = new Dictionary<int, GameObject>();
        DictionaryComponent_NetworkInformation = new Dictionary<int, PlayerInformation>();
        MessageManagerInstance = MessageManager.Instance;
        PlayerPrefab.SetActive(false);

        // ignore player layer collision
        int playerLayer = LayerMask.NameToLayer("Ignore Collision");
        if (playerLayer != 3)
            DebugText.Instance.LogError("Error -> LayerMask.NameToLayer('ignore collision') : ", playerLayer);

        Physics2D.IgnoreLayerCollision(playerLayer, ReadonlyInt_LayerIgnoreCollision_3, true);
    }

    public void InformationDivider(PlayerInformation information)
    {
        int responseID = information.Player_ID;

        Debug.Log("response id : " + responseID);

        switch (information.ProtocolType)
        {
            default:
            case 0: // undefined
            case 1: // chatting
                Debug.Log("Error -> InformationDivider information type : " + information.ProtocolType);
                return;

            case 2: // enter
                // initialize player ID
                if (CurrentPlayerID < 0)
                    CurrentPlayerID = responseID;

                // create new player object
                Information_CheckIn(responseID, information);
                return;

            case 3: // move
                if (DictionaryComponent_NetworkInformation.ContainsKey(responseID) == false || responseID == CurrentPlayerID)
                    return;

                Information_MovePosition(DictionaryComponent_NetworkInformation[responseID].MovementComponent, new Vector3(information.Player_XPosition, information.Player_YPosition, ReadonlyInt_MinusZPosition));
                return;

            case 4: // exit
                if (DictionaryComponent_NetworkInformation.ContainsKey(responseID) == false)
                    return;

                Information_Exit(responseID);
                return;
        }
    }

    private void Information_CheckIn(int responseID, PlayerInformation information)
    {
        Debug.Log("Check-In id : " + responseID);

        // initialize
        VectorPosition.x = information.Player_XPosition;
        VectorPosition.y = information.Player_YPosition;
        VectorPosition.z = ReadonlyInt_MinusZPosition;

        // create gameobject, TO DO :: object pooling here
        GameObject newPlayer = Instantiate(PlayerPrefab, VectorPosition, Quaternion.identity, ActivePlayerListTransform);
        newPlayer.name = information.Player_Nickname;
        int TEST_SCALE_VALUE = 0;
        newPlayer.layer = ReadonlyInt_LayerIgnoreCollision_3;
        newPlayer.SetActive(true);

        // sprite animation
        information.AnimationComponent = newPlayer.GetComponent<SpriteAnimationComponent>();
        information.AnimationComponent.AnimationStart(ref TEST_SCALE_VALUE);

        // input checker
        information.MovementComponent = newPlayer.GetComponent<InputMovementComponent>();
        information.MovementComponent.InputCheckerStart(information.AnimationComponent, ref responseID, DictionaryComponent_NetworkInformation.Count > 0);

        newPlayer.transform.SetEffectScale(TEST_SCALE_VALUE);

        DictionaryObject_Player.Add(responseID, newPlayer);
        DictionaryComponent_NetworkInformation.Add(responseID, information);
    }

    private void Information_Exit(int responseID)
    {
        // TO DO object pooling here
        DictionaryComponent_NetworkInformation.Remove(responseID);
        Destroy(DictionaryObject_Player[responseID]); // SetActive(false);
        Debug.Log("Check-Out id : " + responseID);
    }

    private void Information_MovePosition(InputMovementComponent component, Vector3 vectorPosition)
    {
        component.OnLerpMove(vectorPosition);
    }
}