using System.Text;
using System.Text.RegularExpressions;

namespace TeduCoreApp.Utilities.Helpers
{
    public static class TextHelper
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-");
            }
            return str2.ToLower();
        }

        public static string ToString(decimal number)
        {
            string s = number.ToString("#");
            string[] numberWords = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] layer = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, unit, dozen, hundred;
            bool isNegative = false;
            string str = " ";
            decimal.TryParse(s.ToString(), out decimal decS);
            //Tung addnew
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                isNegative = true;
            }
            i = s.Length;
            if (i == 0)
            {
                str = numberWords[0] + str;
            }
            else
            {
                j = 0;
                while (i > 0)
                {
                    int.TryParse(s.Substring(i - 1, 1), out unit);
                    i--;
                    if (i > 0)
                    {
                        int.TryParse(s.Substring(i - 1, 1), out dozen);
                    }
                    else
                    {
                        dozen = -1;
                    }
                    i--;
                    if (i > 0)
                    {
                        int.TryParse(s.Substring(i - 1, 1), out hundred);
                    }
                    else
                    {
                        hundred = -1;
                    }
                    i--;
                    if ((unit > 0) || (dozen > 0) || (hundred > 0) || (j == 3))
                    {
                        str = layer[j] + str;
                    }
                    j++;
                    j = (j > 3) ? 1 : j;
                    if (unit == 1 && dozen > 1)
                    {
                        str = "một " + str;
                    }
                    else
                    {
                        if (unit == 5 && dozen > 0)
                        {
                            str = "lăm " + str;
                        }
                        else if (unit > 0)
                        {
                            str = numberWords[unit] + " " + str;
                        }
                    }
                    if (dozen < 0)
                    {
                        break;
                    }
                    else
                    {
                        str = (dozen == 0 && unit > 0) ? "lẻ " : string.Empty + str;
                        str = (dozen == 1) ? "mười  " : string.Empty + str;
                        str = (dozen > 1) ? numberWords[dozen] + " mươi " : string.Empty + str;
                    }
                    if (hundred < 0)
                    {
                        break;
                    }
                    else
                    {
                        str = ((hundred > 0) || (dozen > 0) || (unit > 0)) ? numberWords[hundred] + " trăm " : string.Empty + str;
                    }
                    str = " " + str;
                }
            }
            str = isNegative ? "Âm" : string.Empty + str;
            return str + "đồng chẵn";
        }
    }
}
