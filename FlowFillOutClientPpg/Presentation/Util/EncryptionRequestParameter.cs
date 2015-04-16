using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presentation.Util
{
    public static class EncryptionRequestParameter
    {
        public static string Encrypt(int requestId, int groupId)
        {
            var textToEncrypt = string.Format("{0}@{1}", requestId, groupId);
            return "a";
        }
    }
}
