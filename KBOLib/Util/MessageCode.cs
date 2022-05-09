using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBOLib.Util
{
    public class MessageCode
    {
        public const string CD_OK = "100";
        public const string CD_FAIL = "200";

        public const string CD_OK_MSG = "성공";
        public const string CD_FAIL_MSG = "실패";

        public const string CD_UPDATE_XSS_FAIL_MSG = "수정할 수 없습니다. 잘못된 문자가 있습니다.";   // 게시판 수정 실패 XSS
        public const string CD_INSERT_XSS_FAIL_MSG = "등록할 수 없습니다. 잘못된 문자가 있습니다.";   // 게시판 등록 실패 XSS

        public const string CD_AUTH_FAIL_MSG = "권한이 없습니다.";
    }
}
