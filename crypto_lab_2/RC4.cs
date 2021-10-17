using System;
using System.Collections.Generic;
using System.Text;

namespace is_lab_2
{
    public class RC4 : IChipherer
    {
        private const int N = 256;
        private int[] S;
        private byte[] key;

        public RC4(byte[] key)
        {
            this.key = key;
        }

        public byte[] Decrypt(byte[] chiphertext)
        {
            return Transform(chiphertext);
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            return Transform(plaintext);
        }

        public byte[] Transform(byte[] input)
        {
            RC4Initialize();

            int i = 0, j = 0, K;
            byte[] cipher = new byte[input.Length];
            for (int a = 0; a < input.Length; a++)
            {
                i = (i + 1) % N;
                j = (j + S[i]) % N;
                int buf = S[i];
                S[i] = S[j];
                S[j] = buf;

                int t = (S[i] + S[j]) % N;
                K = S[t];
                int cipherBy = ((int)input[a]) ^ K;
                cipher[a] = (byte)cipherBy;
            }
            return cipher;
        }

        private void RC4Initialize()
        {
            S = new int[N];
            int[] K = new int[N];
            int n = key.Length;

            for (int i = 0; i < N; i++)
            {
                K[i] = (int)key[i % n];
                S[i] = i;
            }

            int j = 0;

            for (int i = 0; i < N; i++)
            {
                j = (j + S[i] + K[i]) % N;
                int buf = S[i];
                S[i] = S[j];
                S[j] = buf;
            }
        }
    }
}
