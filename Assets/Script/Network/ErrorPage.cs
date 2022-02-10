using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Network
{
    public class ErrorPage
    {
        public const byte Success = 0;
        public const byte SystemError = 255;

        public const byte ArgumentError = 1;

        public const byte CharacterNotFound = 11;
        public const byte NicknameSizeExceed = 12;
        public const byte NotAvailableNickname = 13;

        public const byte CharacterExistInMap = 21;
        public const byte CharacterNotExistInMap = 22;


        public static Dictionary<byte, string> Messages = new Dictionary<byte, string>();

        static ErrorPage()
        {
            Messages.Add(Success, "");

            Messages.Add(SystemError, "내부 시스템 오류");

            Messages.Add(ArgumentError, "요청 파라메터 오류");

            Messages.Add(CharacterNotFound, "캐릭터 없음");
            Messages.Add(NicknameSizeExceed, "닉네임 최대글자수 초과");
            Messages.Add(NotAvailableNickname, "사용할 수 없는 닉네임");

            Messages.Add(CharacterExistInMap, "캐릭터가 이미 맵안에 있음");
            Messages.Add(CharacterNotExistInMap, "캐릭터가 맵안에 없음");
        }
    }
}
