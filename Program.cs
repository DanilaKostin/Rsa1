using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace BigNums
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(BigMath.Pow(8, 51));
            Console.WriteLine(BigMath.NOD(18, 12));
            Console.WriteLine(BigMath.FermaTest(13));
        }

        public static class BigMath
        {
            public static BigInteger GetBigPrime(int dec)
            {
                Random r = new Random();
                BigInteger a = 0;
                a += Pow(2, dec);
                for (int i = dec-1; i >= 0; i--)
                {
                    if (r.Next(0, 2) == 1)
                        a += Pow(2, i);  
                }
                while (!FermaTest(a, 10))
                {
                    a++;
                }
                Console.WriteLine(a);
                return a;
            }

            /// <summary>
            /// Возведение в степень с вычислением остатка от деления (для возведения особо больших чисел)
            /// </summary>
            public static BigInteger Pow(BigInteger a, BigInteger n, BigInteger module)
            {
                BigInteger res = 1;
                while (n > 0)
                {
                    if (n % 2 == 0)
                    {
                        n /= 2;
                        a *= a;
                        a %= module;
                    }
                    else 
                    {
                        n--;
                        res *= a;
                        res %= module;
                    }
                }
                return res % module;
            }

            public static BigInteger Pow(BigInteger a, BigInteger n)
            {
                BigInteger res = 1;
                while (n > 0)
                {
                    if (n % 2 == 0)
                    {
                        n /= 2;
                        a *= a;
                    }
                    else
                    {
                        n--;
                        res *= a;
                    }
                }
                return res;
            }

            public static BigInteger NOD(BigInteger a, BigInteger b)
            {
                while (a != 0 && b != 0)
                {
                    if (a > b)
                    {
                        a = a % b;
                    }
                    else 
                    {
                        b = b % a;
                    }
/*                    if (a > b)
                    {
                        a = a + b;
                        b = a - b;
                        a = a - b;
                    }*/
                }
                return a + b;
            }

            /// <summary>
            /// Может иногда врать для больших чисел, а также бесконечно висеть.
            /// </summary>
            public static bool FermaTest(BigInteger a, int tries = 100)
            {
                //Random rand = new Random(DateTime.Now.Second * DateTime.Now.Minute + DateTime.Now.Millisecond);
                Random rand = new Random();
                for (int i = 0; i < tries; i++)
                {
                    BigInteger x = (rand.Next() % (a - 4)) + 2;
                    if (NOD(a, x) != 1)
                        return false;
                    if (Pow(x, a - 1, a) != 1)
                        return false;
                }
                return true;
            }
        }
    }
}
