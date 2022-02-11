
/*
public class MyEnumComparer : IEqualityComparer<MyEnum>
{
    public bool Equals(MyEnum x, MyEnum y)
    {
        return x == y;
    }

    public int GetHashCode(MyEnum x)
    {
        return (int)x;
    }
}
*/
/// request, response, notice
public class ProtocolType
{
    /// <summary>
    /// 로그인 요청
    /// </summary>
    public static readonly int Request_Login = 0,

    /// <summary>
    /// 로그인 응답
    /// </summary>
    Response_Login = 1,

    /// <summary>
    /// 캐릭터정보요청
    /// </summary>
    Request_CharacterInformation = 10,

    /// <summary>
    /// 캐릭터정보응답
    /// </summary>
    Response_CharacterInformation = 11,

    /// <summary>
    /// 닉네임중복확인요청
    /// </summary>
    Request_VerifyNickname = 12,

    /// <summary>
    /// 닉네임중복확인응답
    /// </summary>
    Response_VerifyNickname = 13,

    /// <summary>
    /// 캐릭터생성요청
    /// </summary>
    Request_CreateCharacter = 14,

    /// <summary>
    /// 캐릭터생성응답
    /// </summary>
    Response_CreateCharacter = 15,

    /// <summary>
    /// 맵입장요청
    /// </summary>
    Request_EnterWorld = 20,

    /// <summary>
    /// 맵입장응답
    /// </summary>
    Response_EnterWorld = 21,

    /// <summary>
    /// 맵입장통보
    /// </summary>
    Notice_EnterWorld = 22,

    /// <summary>
    /// 맵퇴장요청
    /// </summary>
    Request_ExitWorld = 23,

    /// <summary>
    /// 맵퇴장응답
    /// </summary>
    Response_ExitWorld = 24,

    /// <summary>
    /// 맵퇴장통보
    /// </summary>
    Notice_ExitWorld = 25,

    /// <summary>
    /// 캐릭터이동요청
    /// </summary>
    Request_MoveWorld = 26,

    /// <summary>
    /// 캐릭터이동응답
    /// </summary>
    Response_MoveWorld = 27,

    /// <summary>
    /// 캐릭터이동통보
    /// </summary>
    Notice_MoveWorld = 28,

    /// <summary>
    /// 시스템채팅통보
    /// </summary>
    Notice_SystemChatting = 100,

    /// <summary>
    /// 전체채팅요청
    /// </summary>
    Request_GlobalChatting = 110,

    /// <summary>
    /// 전체채팅응답
    /// </summary>
    Response_GlobalChatting = 111,

    /// <summary>
    /// 전체채팅통보
    /// </summary>
    Notice_GlobalChatting = 112;
}
