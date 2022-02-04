using UnityEngine;

[SerializeField]
public struct PlayerInformation // convert to ScriptableObject
{
    public int ProtocolType;
    public int Player_ID;
    public string Player_Nickname;

    public float Player_XPosition;
    public float Player_YPosition;

    public InputMovementComponent MovementComponent;
    public SpriteAnimationComponent AnimationComponent;
}