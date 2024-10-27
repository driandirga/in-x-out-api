using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InXOutAPI.Application.Helpers
{
    public class Generate
    {
        private static readonly Random _random = new();

        public static string RandomString(int length)
        {
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

                return new string(Enumerable.Range(0, length)
                                            .Select(_ => chars[_random.Next(chars.Length)])
                                            .ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public static string RandomNumber(int length)
        {
            try
            {
                const string chars = "1234567890";

                return new string(Enumerable.Range(0, length)
                                            .Select(_ => chars[_random.Next(chars.Length)])
                                            .ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
