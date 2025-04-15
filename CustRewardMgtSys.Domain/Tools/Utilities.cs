using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Domain.Tools
{
    public class Utilities
    {
        private static Random random = new Random();

        public static List<string> GenerateRandomStrings(int numberOfStrings, int lengthOfEachString)
        {
            HashSet<string> uniqueStrings = new HashSet<string>();
            string sampleCharacters = RefinedStringSample();
            while (uniqueStrings.Count < numberOfStrings)
            {
                string randomString = GenerateRandomString(sampleCharacters, lengthOfEachString);
                uniqueStrings.Add(randomString);
            }

            return new List<string>(uniqueStrings);
        }

        private static string GenerateRandomString(string sampleCharacters, int length)
        {
            char[] characters = sampleCharacters.ToCharArray();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                result[i] = characters[index];
            }

            // Shuffle the result array using Fisher-Yates Shuffle
            for (int i = result.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                char temp = result[i];
                result[i] = result[j];
                result[j] = temp;
            }

            return new string(result);
        }
        private static string RefinedStringSample()
        {
            return "ABDEFGHJLNQRTabdefghijnqrt23456789";
        }
        public static string GenerateBatchNumber(DateTime dateTime)
        {
            string year = dateTime.ToString("yy"); // Last 2 digits of the year
            string month = dateTime.ToString("MM"); // 2-digit month
            string day = dateTime.ToString("dd"); // 2-digit day of the month
            string hour = dateTime.ToString("HH"); // 2-digit hour of the day
            string minute = dateTime.ToString("mm"); // 2-digit minute of the hour

            string batchNumber = minute + hour + day + month + year;
            return batchNumber;
        }
    }
}
