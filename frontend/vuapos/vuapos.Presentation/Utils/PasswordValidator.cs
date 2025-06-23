using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Utils
    {
    public static class PasswordValidator
    {
        public static bool IsValidPassword(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password,
                @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
        }
    }
    }
