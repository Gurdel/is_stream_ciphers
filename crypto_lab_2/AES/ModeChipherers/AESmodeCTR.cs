using System;
using System.Collections.Generic;
using System.Text;

namespace is_lab_2.AES.ModeChipherers
{
    class AESmodeCTR : IChipherer
    {
        AESAlgo aes;
        byte[] key;
        byte[] iv;
        static int BLOCK_SIZE = 16;

        public AESmodeCTR(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
            aes = new AESAlgo(key);
        }


        public byte[] Decrypt(byte[] chiphertext)
        {
            byte[] res = new byte[chiphertext.Length];
            int currpos = 0;
            byte[] Y = new byte[BLOCK_SIZE];
            byte[] C = new byte[BLOCK_SIZE];
            byte[] N = new byte[BLOCK_SIZE];

            Array.Copy(iv, 0, N, 0, BLOCK_SIZE);

            while (currpos < chiphertext.Length)
            {
                Array.Copy(chiphertext, currpos, C, 0, BLOCK_SIZE);

                Y = aes.Encrypt(N);

                arrXOR(Y, C);

                Array.Copy(Y, 0, res, currpos, BLOCK_SIZE);
                currpos += BLOCK_SIZE;
                Inc(N);
            }

            return res;
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] res = new byte[plaintext.Length];
            int currpos = 0;
            byte[] P = new byte[BLOCK_SIZE];
            byte[] Y = new byte[BLOCK_SIZE];
            byte[] N = new byte[BLOCK_SIZE];

            Array.Copy(iv, 0, N, 0, BLOCK_SIZE);

            while (currpos < plaintext.Length)
            {
                Array.Copy(plaintext, currpos, P, 0, BLOCK_SIZE);

                Y = aes.Encrypt(N);

                arrXOR(Y, P);

                Array.Copy(Y, 0, res, currpos, BLOCK_SIZE);
                currpos += BLOCK_SIZE;
                Inc(N);
            }

            return res;
        }

        static void arrXOR(byte[] arr1, byte[] arr2)
        {
            for (int i = 0; i < BLOCK_SIZE; ++i)
                arr1[i] ^= arr2[i];
        }

        static void Inc(byte[] N)
        {
            for (int i = 0; i < N.Length; ++i)
            {
                if (N[i] < 255)
                {
                    N[i]++;
                    return;
                }
            }
            
            // N[i] = 255, i=0..N.Length
            for (int i = 0; i<N.Length; ++i)
            {
                N[i] = 0;
            }
        }
    }
}
