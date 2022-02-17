
/// type : request, response, notice
public class ProtocolType
{
    /// <summary>
    /// 로그인 요청 : 0
    /// </summary>
    public static readonly int Request_Login_0 = 0,

    /// <summary>
    /// 로그인 응답 : 1
    /// </summary>
    Response_Login_1 = 1,

    /// <summary>
    /// 캐릭터정보요청 : 10
    /// </summary>
    Request_CharacterInformation_10 = 10,

    /// <summary>
    /// 캐릭터정보응답 : 11
    /// </summary>
    Response_CharacterInformation_11 = 11,

    /// <summary>
    /// 닉네임중복확인요청 : 12
    /// </summary>
    Request_VerifyNickname_12 = 12,

    /// <summary>
    /// 닉네임중복확인응답 : 13
    /// </summary>
    Response_VerifyNickname_13 = 13,

    /// <summary>
    /// 캐릭터생성요청 : 14
    /// </summary>
    Request_CreateCharacter_14 = 14,

    /// <summary>
    /// 캐릭터생성응답 : 15
    /// </summary>
    Response_CreateCharacter_15 = 15,

    /// <summary>
    /// 맵입장요청 : 20
    /// </summary>
    Request_EnterWorld_20 = 20,

    /// <summary>
    /// 맵입장응답 : 21
    /// </summary>
    Response_EnterWorld_21 = 21,

    /// <summary>
    /// 맵입장통보 : 22
    /// </summary>
    Notice_EnterWorld_22 = 22,

    /// <summary>
    /// 맵퇴장요청 : 23
    /// </summary>
    Request_ExitWorld_23 = 23,

    /// <summary>
    /// 맵퇴장응답 : 24
    /// </summary>
    Response_ExitWorld_24 = 24,

    /// <summary>
    /// 맵퇴장통보 : 25
    /// </summary>
    Notice_ExitWorld_25 = 25,

    /// <summary>
    /// 캐릭터이동요청 : 26
    /// </summary>
    Request_MoveWorld_26 = 26,

    /// <summary>
    /// 캐릭터이동응답 : 27
    /// </summary>
    Response_MoveWorld_27 = 27,

    /// <summary>
    /// 캐릭터이동통보 : 28
    /// </summary>
    Notice_MoveWorld_28 = 28,

    /// <summary>
    /// 시스템채팅통보 : 100
    /// </summary>
    Notice_SystemChatting_100 = 100,

    /// <summary>
    /// 전체채팅요청 : 110
    /// </summary>
    Request_GlobalChatting_110 = 110,

    /// <summary>
    /// 전체채팅응답 : 111
    /// </summary>
    Response_GlobalChatting_111 = 111,

    /// <summary>
    /// 전체채팅통보 : 112
    /// </summary>
    Notice_GlobalChatting = 112;
}
