using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace BigNums
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(BigMath.Pow(8, 51));
            //Console.WriteLine(BigMath.NOD(BigInteger.Parse("1812345223423423432122411112111111129"), 121242323423433331));
            //Console.WriteLine(BigMath.FermaTest(BigInteger.Parse("181064202359500080338127486338067489543879000932930481388396034634150146460481249847451371860827520188149663235119571615462550926899809302576041444670527349267665527036145546938930161818085229022873293621033725644584423900487312795929972988771538702372740171603809831849093789761184216949209179477728228785511")));
            //Console.WriteLine(BigMath.GetBigPrime(1024));
            //Console.WriteLine(BigMath.GetBigPrime(1024));
            //BigMath.EratosfensSieve(10);    
            Sender A = new Sender();
            A.SetNFi();
            //Console.WriteLine(BigMath.modInverse(4, 101));
        }

        public class Sender
        {
            public BigInteger n;
            public BigInteger fi;
            public BigInteger e;
            public BigInteger d;
            public void SetNFi()
            {
                BigInteger p = BigMath.GetBigPrime(1024);
                BigInteger q = BigMath.GetBigPrime(1024);
                this.n = p * q;
                this.fi = (p - 1)*(q - 1);
                Console.WriteLine(p);
                Console.WriteLine("НОВАЯ СТРОКА");
                //Console.WriteLine(BigMath.FermaTest(q));
                GenerateE();
                Console.WriteLine("Вот ниже");
                this.d = BigMath.modInverse(e, fi);
                var message = 1234;
                BigInteger c = BigMath.Pow(message, e, n);
                BigInteger m = BigMath.Pow(c, d, n);
                Console.WriteLine(c);
                Console.WriteLine(m);
            }

            public void GenerateE()
            {
                Random r = new Random();
                int decimals = r.Next(1, 2048);
                while ((e = BigMath.GetBigPrime(decimals)) > fi)
                {
                    e = BigMath.GetBigPrime(decimals);
                }
                while (BigMath.NOD(e, fi) != 1)
                {
                    e++;
                }
                Console.WriteLine(e);
            }
        }

        public static class BigMath
        {
        /*
            public static int modInverse(int a, int m)
            {
                int m0 = m;
                int y = 0, x = 1;
                if (m == 1)
                    return 0;
                while (a > 1)
                {
                    // q является частным
                    int q = a / m;
                    int t = m;
                    // m осталось, процесс
                    // То же, что и алгоритм Евклида
                    m = a % m;
                    a = t;
                    t = y;
                    // Обновляем x и y
                    y = x - q * y;
                    x = t;
                }
                // Сделать х положительным
                if (x < 0)
                    x += m0;
                return x;
            }*/
            public static BigInteger modInverse(BigInteger a, BigInteger m)
            {
                BigInteger m0 = m;
                BigInteger y = 0, x = 1;
                if (m == 1 || NOD(a, m) != 1)
                    return 0;
                BigInteger q;
                BigInteger t;
                while (a > 1)
                {
                    // q является частным
                    q = a / m;
                    t = m;
                    // m осталось, процесс
                    // То же, что и алгоритм Евклида
                    m = a % m;
                    a = t;
                    t = y;
                    // Обновляем x и y
                    y = x - q * y;
                    x = t;
                }
                // Сделать х положительным
                if (x < 0)
                    x += m0;
                return x;
            }

            public static BigInteger GetBigPrime(int dec)
            {
                Random r = new Random();
                BigInteger a = 0;
                a += Pow(2, dec);
                for (int i = dec - 1; i >= 0; i--)
                {
                    if (r.Next(0, 2) == 1)
                        a += Pow(2, i);
                }
                while (!FermaTest(a, 10))
                {
                    a++;
                }
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
                }
                return a + b;
            }

            /// <summary>
            /// Может иногда врать для больших чисел, а также бесконечно висеть.
            /// </summary>
            public static bool FermaTest(BigInteger a, int tries = 5)
            {
                var sieve = EratosfensSieve(100);
                foreach (var i in sieve)
                {
                    if (NOD(i, a) != 1)
                        return false;
                }
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

            public static List<int> EratosfensSieve(int count)
            {
                var sieve = new List<int>();
                //заполнение списка числами от 2 до n-1
                for (var i = 2; i < count; i++)
                {
                    sieve.Add(i);
                }

                for (var i = 0; i < sieve.Count; i++)
                {
                    for (var j = 2; j < count; j++)
                    {
                        sieve.Remove(sieve[i] * j);
                    }
                }
                return sieve;
            }
        }
    }
}
