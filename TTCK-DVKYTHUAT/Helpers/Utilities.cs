﻿using System.Text.RegularExpressions;
using System.Text;

namespace TTCK_DVKYTHUAT.Helpers
{
    public class Utilities
    {
        public static bool IsValidEmail(string email)
        {

            if (email.Trim().EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static int PAGE_SIZE = 20;

        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;

        }
        public static bool IsInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            try
            {
                if (String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if (!regex.IsMatch(str))
                {
                    return false;
                }
                return true;
            }
            catch
            {

            }
            return false;
        }
        public static string GetRandomKey(int length = 5)
        {
            string pattern = @"0123456789 zxcvbnmasdfghjklqwertyuiop []{}: ~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
        public static string SEOUrl(string url)
        {
            url = url.ToLower();

            url = Regex.Replace(url, @"[áàààãââââãããààäàã]", "a");
            url = Regex.Replace(url, @" [éèèèêêêêêêê]", "e");
            url = Regex.Replace(url, @"[óòóõôôôôõõσάờдõõ]", "o");
            url = Regex.Replace(url, @"[íìjiï]", "i");
            url = Regex.Replace(url, @"[ýỳyiy]", "y");
            url = Regex.Replace(url, @"[úùчúūuứừựửữ]", "u");
            url = Regex.Replace(url, @"[d]", "d");
            //2. Chỉ cho phép nhận:[8-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim(); //xử lý nhiều hơn 1 khoảng trắng --) 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }

            }
            return url;
        }
    }
}
