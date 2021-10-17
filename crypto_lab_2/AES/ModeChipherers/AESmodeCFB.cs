using System;
using System.Collections.Generic;
using System.Text;

namespace is_lab_2.AES.ModeChipherers
{
    public class AESmodeCFB : IChipherer
    {
        AESAlgo aes;
        byte[] key;
        byte[] iv;
        static int BLOCK_SIZE = 16;

        public AESmodeCFB(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
            aes = new AESAlgo(key);
        }


        public byte[] Decrypt(byte[] chiphertext)
        {
            byte[] res = new byte[chiphertext.Length];
            int currpos = 0;
            byte[] C = new byte[BLOCK_SIZE];
            byte[] Y = new byte[BLOCK_SIZE];
            byte[] X = new byte[BLOCK_SIZE];

            Array.Copy(iv, 0, X, 0, BLOCK_SIZE);

            while (currpos < chiphertext.Length)
            {
                Array.Copy(chiphertext, currpos, C, 0, BLOCK_SIZE);

                Y = aes.Encrypt(X);
                arrXOR(Y, C);

                Array.Copy(Y, 0, res, currpos, BLOCK_SIZE);
                Array.Copy(C, 0, X, 0, BLOCK_SIZE);
                currpos += BLOCK_SIZE;
            }

            return res;
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] res = new byte[plaintext.Length];
            int currpos = 0;
            byte[] P = new byte[BLOCK_SIZE];
            byte[] Y = new byte[BLOCK_SIZE];
            byte[] X = new byte[BLOCK_SIZE];

            Array.Copy(iv, 0, X, 0, BLOCK_SIZE);

            while (currpos < plaintext.Length)
            {
                Array.Copy(plaintext, currpos, P, 0, BLOCK_SIZE);

                Y = aes.Encrypt(X);
                arrXOR(Y, P);

                Array.Copy(Y, 0, res, currpos, BLOCK_SIZE);
                Array.Copy(Y, 0, X, 0, BLOCK_SIZE);
                currpos += BLOCK_SIZE;
            }

            return res;
        }

        static void arrXOR(byte[] arr1, byte[] arr2)
        {
            for (int i = 0; i < BLOCK_SIZE; ++i)
                arr1[i] ^= arr2[i];
        }
    }
}
