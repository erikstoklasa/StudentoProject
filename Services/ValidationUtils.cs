using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public static class ValidationUtils
    {
        public static bool PersonalIdentifNumberIsValid(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return false;
            }
            number = number.Trim();

            number = RemoveSlashIfExists(number);

            Int64 convertedNumber;
            try
            {
                convertedNumber = Int64.Parse(number);
            }
            catch (Exception)
            {
                return false;
            }
            if (convertedNumber % 11 != 0)
            {
                return false;
            }
            return true;
        }

        private static string RemoveSlashIfExists(string number)
        {
            int indexOfSlash = number.IndexOf('/');
            if (indexOfSlash >= 0)
            {
                number = number.Remove(indexOfSlash, 1);
            }

            return number;
        }
    }
}
