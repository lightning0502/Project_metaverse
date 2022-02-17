using UnityEngine;

[SerializeField]
public struct Information_PlayerObject // convert to ScriptableObject
{
    public readonly int Player_Number;

    public readonly string Player_Nickname;

    public float Player_XPosition;
    public float Player_YPosition;

    public InputMovementComponent MovementComponent;
    public SpriteAnimationComponent AnimationComponent;

    public Information_PlayerObject(in int number, in string name, in float x, in float y)
    {
        Player_Number = number;
        Player_Nickname = name;
        Player_XPosition = x;
        Player_YPosition = y;
        MovementComponent = null;
        AnimationComponent = null;
    }
}