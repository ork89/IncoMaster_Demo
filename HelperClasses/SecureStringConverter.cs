using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace HelperClasses
{
    public class SecureStringConverter
    {        
        public string ConvertToString(SecureString stringToConvert)
        {
            return new NetworkCredential("", stringToConvert).Password;
        }

        public SecureString ConvertToSecureString(string stringToConvert)
        {
            return new NetworkCredential("", stringToConvert).SecurePassword;
        }
    }
}
