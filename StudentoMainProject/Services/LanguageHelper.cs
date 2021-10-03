using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public static class LanguageHelper
    {
        //Following code found at: https://www.itnetwork.cz/csharp/oop/zdrojove-kody/csharp-algoritmy-prevod-jmena-do-5-padu
        //Author credit: https://www.itnetwork.cz/portfolio/1246
        public static string Sklonuj(string slovo)
        {
            if (string.IsNullOrEmpty(slovo) || slovo.Length < 1)
                return string.Empty;
            slovo = slovo.Trim();
            int i = slovo.Length - 1; // Index posledniho pismene
            slovo = slovo[0].ToString().ToUpper() + slovo.Substring(1, i);

            string vysledneSlovo = slovo;

            if (!((slovo[i - 1] == 'u' && slovo[i] == 'm') ||
                (slovo[i - 1] == 'o' && slovo[i] == 'n')))
            {
                if (slovo[i - 1] == 'i' && slovo[i] == 'a') // Maria
                    // Nebo -io, -eio
                    // Záleží na výslovnosti ...
                    vysledneSlovo = slovo.Substring(0, i - 1) + "ie";

                else if (slovo[i - 1] == 'í' && slovo[i] == 'a') // María
                    vysledneSlovo = slovo.Substring(0, i - 1) + "íe";

                else if (slovo[i - 1] == 'e' && slovo[i] == 'c') // Otec
                    vysledneSlovo = slovo.Substring(0, i - 1) + "če";

                else if (slovo[i - 1] == 'e' && slovo[i] == 'k') // Bobek
                    vysledneSlovo = slovo.Substring(0, i - 1) + "ku";

                else if (slovo[i - 1] == 'e' && slovo[i] == 'l') // Bobek
                    vysledneSlovo = slovo.Substring(0, i - 1) + "le";

                else if (slovo[i - 1] == 'c' && slovo[i] == 'h')
                    vysledneSlovo += "u";

                else if (slovo[i] == 'a' ||
                    slovo[i] == 'á')
                    vysledneSlovo = slovo.Substring(0, i) + "o";

                else if (slovo[i] == 'ž' ||
                    slovo[i] == 'l' ||
                    slovo[i] == 'j' ||
                    slovo[i] == 's' ||
                    slovo[i] == 'č' ||
                    slovo[i] == 'c' ||
                    slovo[i] == 'x' ||
                    slovo[i] == 'š' ||
                    slovo[i] == 'z' ||
                    slovo[i] == 'ř')
                    vysledneSlovo += "i";

                else if (slovo[i] == 'n' ||
                    slovo[i] == 't' ||
                    slovo[i] == 'b' ||
                    slovo[i] == 'd' ||
                    slovo[i] == 'ď' ||
                    slovo[i] == 'f' ||
                    slovo[i] == 't' ||
                    slovo[i] == 'n' ||
                    slovo[i] == 'ň' ||
                    slovo[i] == 'p' ||
                    slovo[i] == 'm' ||
                    slovo[i] == 'v' ||
                    slovo[i] == 'w' ||
                    slovo[i] == 'r')
                    vysledneSlovo += "e";

                else if (slovo[i] == 'k' ||
                   slovo[i] == 'g')
                    vysledneSlovo += "u";

                else if (slovo[i] == 'é')
                    vysledneSlovo = slovo.Substring(0, i) + "e";

                else if (slovo[i] == 'ó' ||
                    slovo[i] == 'ň')
                    vysledneSlovo = slovo.Substring(0, i) + "o";

            }
            return vysledneSlovo;
        }
        //End of credited code
        public static string getRelativeTime(DateTime dateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "před sekundou" : "před " + ts.Seconds + " sekundami";

            if (delta < 2 * MINUTE)
                return "před minutou";

            if (delta < 55 * MINUTE)
                return "před " + ts.Minutes + " minutami";

            if (delta < 70 * MINUTE)
                return "před hodinou";

            if (delta < 24 * HOUR)
                return "před " + ts.Hours + " hodinami";

            if (delta < 48 * HOUR)
                return "včera";

            if (delta < 30 * DAY)
                return "před " + ts.Days + " dny";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "před měsícem" : "před " + months + " měsíci";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "před rokem" : "před " + years + " roky";
            }
        }

        public static string getAverageComparisonString(double currentAverage, double lastAverage)
        {
            if (lastAverage > currentAverage)
            {
                return $"<small> Což je <strong class=\"text-success\">lepší</strong> než minulý měsíc ({lastAverage:f2})</small>";
            }
            else if (currentAverage > lastAverage)
            {
                return $"<small> Což je <strong class=\"text-danger\">horší</strong> než minulý měsíc ({lastAverage:f2})</small>";
            }
            else
            {
                return "";
            }
        }
        public static string getShorterString(string input, int maxLength)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            if (input.Length > maxLength)
            {
                return input.Substring(0, maxLength) + "..";
            }
            return input;

        }
    }
}
