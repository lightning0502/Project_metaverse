using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Network
{
    public enum ProtocolType
    {
        /// <summary>
        /// 로그인 요청
        /// </summary>
        LoginRequest = 0,

        /// <summary>
        /// 로그인 응답
        /// </summary>
        LoginResponse = 1,

        /// <summary>
        /// 캐릭터정보요청
        /// </summary>
        CharacterInfoRequest = 10,

        /// <summary>
        /// 캐릭터정보응답
        /// </summary>
        CharacterInfoResponse = 11,

        /// <summary>
        /// 닉네임중복확인요청
        /// </summary>
        VerifyNicknameRequest = 12,

        /// <summary>
        /// 닉네임중복확인응답
        /// </summary>
        VerifyNicknameResponse = 13,

        /// <summary>
        /// 캐릭터생성요청
        /// </summary>
        CreateCharacterRequest = 14,

        /// <summary>
        /// 캐릭터생성응답
        /// </summary>
        CreateCharacterResponse = 15,

        /// <summary>
        /// 맵입장요청
        /// </summary>
        EnterMapRequest = 20,

        /// <summary>
        /// 맵입장응답
        /// </summary>
        EnterMapResponse = 21,

        /// <summary>
        /// 맵입장통보
        /// </summary>
        EnterMapNotice = 22,

        /// <summary>
        /// 맵퇴장요청
        /// </summary>
        ExitMapRequest = 23,

        /// <summary>
        /// 맵퇴장응답
        /// </summary>
        ExitMapResponse = 24,

        /// <summary>
        /// 맵퇴장통보
        /// </summary>
        ExitMapNotice = 25,

        /// <summary>
        /// 캐릭터이동요청
        /// </summary>
        MoveMapRequest = 26,

        /// <summary>
        /// 캐릭터이동응답
        /// </summary>
        MoveMapResponse = 27,

        /// <summary>
        /// 캐릭터이동통보
        /// </summary>
        MoveMapNotice = 28,

        /// <summary>
        /// 시스템채팅통보
        /// </summary>
        SystemChattingNotice = 100,

        /// <summary>
        /// 전체채팅요청
        /// </summary>
        GlobalChattingRequest = 110,

        /// <summary>
        /// 전체채팅응답
        /// </summary>
        GlobalChattingResponse = 111,

        /// <summary>
        /// 전체채팅통보
        /// </summary>
        GlobalChattingNotice = 112
    }
}
