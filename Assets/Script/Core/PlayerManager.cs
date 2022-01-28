using UnityEngine.UI;
using System;
using System.Text;
using System.IO;
using UnityEngine;
using Extension;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    // values
    private readonly int ReadonlyInt_MinusZPosition = -10;
    private readonly int ReadonlyInt_CharacterScale_2 = 2;
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
    }

    public void InformationDivider(PlayerInformation information)
    {
        int responsID = information.Player_ID;

        Debug.Log("response id : " + responsID);

        switch (information.ProtocolType)
        {
            default:
            case 0: // undefined
            case 1: // chatting
                Debug.Log("Error -> InformationDivider information type : " + information.ProtocolType);
                return;

            case 2: // enter
                if (CurrentPlayerID < 0)
                    CurrentPlayerID = responsID;

                Information_CheckIn(responsID, information);
                return;

            case 3: // move
                if (DictionaryComponent_NetworkInformation.ContainsKey(responsID) == false || responsID == CurrentPlayerID)
                    return;

                Information_MovePosition(responsID, DictionaryComponent_NetworkInformation[responsID].MovementComponent, new Vector2(information.Player_XPosition, information.Player_YPosition));
                return;

            case 4: // exit
                if (DictionaryComponent_NetworkInformation.ContainsKey(responsID) == false)
                    return;

                Information_Exit(responsID);
                return;
        }
    }

    private void Information_CheckIn(int id, PlayerInformation information)
    {
        // create new player object
        Debug.Log("Check-In Player : " + id);

        // initialize
        VectorPosition.x = information.Player_XPosition;
        VectorPosition.y = information.Player_YPosition;
        VectorPosition.z = ReadonlyInt_MinusZPosition;

        // create gameobject, TO DO :: object pooling here
        GameObject newPlayer = Instantiate(PlayerPrefab, VectorPosition, Quaternion.identity, ActivePlayerListTransform);
        newPlayer.name = information.Player_Nickname;
        int TEST_SCALE_VALUE = 0;
        newPlayer.SetActive(true);

        // sprite animation
        information.AnimationComponent = newPlayer.GetComponent<SpriteAnimationComponent>();
        information.AnimationComponent.AnimationStart(ref TEST_SCALE_VALUE);

        // input checker
        information.MovementComponent = newPlayer.GetComponent<InputMovementComponent>();
        information.MovementComponent.InputCheckerStart(information.AnimationComponent, ref id, DictionaryComponent_NetworkInformation.Count > 0);

        newPlayer.transform.SetEffectScale(TEST_SCALE_VALUE);

        DictionaryObject_Player.Add(id, newPlayer);
        DictionaryComponent_NetworkInformation.Add(id, information);
    }

    private void Information_Exit(int id)
    {
        // TO DO object pooling here
        DictionaryComponent_NetworkInformation.Remove(id);
        Destroy(DictionaryObject_Player[id]); // SetActive(false);
        Debug.Log("remove id : " + id);
    }

    private void Information_MovePosition(int id, InputMovementComponent component, Vector2 vectorPosition)
    {
        component.SetLocalPosition(id, vectorPosition);
    }
}