using is_lab_2;
using System;
using System.Diagnostics;
using System.Text;

namespace ConsoleForTests
{
    class Program
    {
        static void Test(FileChipherer chipherer, string algo)
        {
            string rootpath = "C:\\Users\\maksy\\Documents\\git\\is_stream_ciphers\\";
            string inpFileName = "input_long.txt";

            Console.WriteLine(algo);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            chipherer.Encrypt(rootpath + inpFileName, rootpath + "Encr_" + inpFileName);
            stopwatch.Stop();

            Console.WriteLine($"\tEncr: {stopwatch.ElapsedMilliseconds}");
            stopwatch.Reset();

            stopwatch.Start();
            chipherer.Decrypt(rootpath + "Encr_" + inpFileName, rootpath + "Decr_" + inpFileName);
            stopwatch.Stop();

            Console.WriteLine($"\tDecr: {stopwatch.ElapsedMilliseconds}");
        }

        static void Main(string[] args)
        {
            if (false)
            {
                string key = "aaaaaaaabbbbbbbbccccccccdddddddd";
                string input = "some text for testing rc4 and salsa20";

                byte[] plain = Encoding.ASCII.GetBytes(input);
                RC4 encr = new RC4(Encoding.ASCII.GetBytes(key));
                byte[] chiphered = encr.Transform(plain);
                byte[] decoded = encr.Transform(chiphered);


                Salsa20 salsa20 = new Salsa20(Encoding.ASCII.GetBytes(key), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                byte[] chiphered1 = salsa20.Transform(plain);
                byte[] decoded1 = salsa20.Transform(chiphered1);

                Console.WriteLine(Encoding.ASCII.GetString(chiphered));
                Console.WriteLine(Encoding.ASCII.GetString(decoded));
                Console.WriteLine(Encoding.ASCII.GetString(chiphered1));
                Console.WriteLine(Encoding.ASCII.GetString(decoded1));
            }
            

            Test(new FileChipherer(ChiphererAlgo.AES_ECB), "AES_ECB");
            Test(new FileChipherer(ChiphererAlgo.AES_CBC), "AES_CBC");
            Test(new FileChipherer(ChiphererAlgo.AES_CFB), "AES_CFB");
            Test(new FileChipherer(ChiphererAlgo.AES_OFB), "AES_OFB");
            Test(new FileChipherer(ChiphererAlgo.AES_CTR), "AES_CTR");
            Test(new FileChipherer(ChiphererAlgo.RC4), "RC4");
            Test(new FileChipherer(ChiphererAlgo.Salsa20), "Salsa20");
        }
    }
}
