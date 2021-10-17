using System;
using System.Collections.Generic;
using System.Text;

namespace is_lab_2
{
    public class Salsa20 : IChipherer
    {
        private uint[] state;

        private static int blockSize = 64;
        private readonly int roundsCount = 20;
        byte[] constants = Encoding.ASCII.GetBytes("expand 32-byte k");
        private readonly byte[] key;
        private readonly byte[] iv;

        public Salsa20(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
        }

        private void InitState()
        {
            state = new uint[16];

            state[1] = ToUInt32(key, 0);
            state[2] = ToUInt32(key, 4);
            state[3] = ToUInt32(key, 8);
            state[4] = ToUInt32(key, 12);
            state[11] = ToUInt32(key, 16);
            state[12] = ToUInt32(key, 20);
            state[13] = ToUInt32(key, 24);
            state[14] = ToUInt32(key, 28);

            state[0] = ToUInt32(constants, 0);
            state[5] = ToUInt32(constants, 4);
            state[10] = ToUInt32(constants, 8);
            state[15] = ToUInt32(constants, 12);

            state[6] = ToUInt32(iv, 0);
            state[7] = ToUInt32(iv, 4);
            state[8] = 0;
            state[9] = 0;
        }

        public byte[] Transform(byte[] input)
        {
            InitState();

            byte[] res = new byte[input.Length]; 
            byte[] outputBlock = new byte[blockSize];
            int currPos = 0;

            while (currPos<input.Length)
            {
                BlockTransform(outputBlock, state);

                state[9] = AddOne(state[9]);
                if (state[9] == 0)
                {
                    state[8] = AddOne(state[8]);
                }

                int currBlockSize = blockSize > input.Length - currPos ? input.Length - currPos : blockSize;

                for (int i = 0; i < currBlockSize; i++)
                {
                    res[currPos + i] = (byte)(input[currPos + i] ^ outputBlock[i]);
                }

                currPos += blockSize;
            }

            return res;
        }

        private uint[] QR(uint[] st, int a, int b, int c, int d)
        {
            uint e = Shift(Add(st[d], st[c]), 18);
            uint f = Shift(Add(st[a], st[d]), 7);
            uint g = Shift(Add(st[b], st[a]), 9);
            uint h = Shift(Add(st[c], st[b]), 13);

            st[a] ^= e;
            st[b] ^= f;
            st[c] ^= g;
            st[d] ^= h;

            return st;
        }

        private void BlockTransform(byte[] output, uint[] state)
        {
            uint[] st = (uint[])state.Clone();

            for (int i = roundsCount; i > 0; i -= 2)
            {
                st = QR(st, 0, 4, 8, 12);
                st = QR(st, 5, 9, 13, 1);
                st = QR(st, 10, 14, 2, 6);
                st = QR(st, 15, 3, 7, 11);

                st = QR(st, 0, 1, 2, 3);
                st = QR(st, 5, 6, 7, 4);
                st = QR(st, 10, 11, 8, 9);
                st = QR(st, 15, 12, 13, 14);
            }

            for (int i = 0; i < 16; i++)
            {
                ToBytes(Add(st[i], state[i]), output, 4 * i);
            }
        }

        private static uint Shift(uint v, int c)
        {
            return (v << c) | (v >> (blockSize/2 - c));
        }

        private static uint Add(uint v, uint w)
        {
            return unchecked(v + w);
        }

        private static uint AddOne(uint v)
        {
            return unchecked(v + 1);
        }

        private static uint ToUInt32(byte[] input, int inputOffset)
        {
            unchecked
            {
                return (uint)(((input[inputOffset] |
                                (input[inputOffset + 1] << 8)) |
                                (input[inputOffset + 2] << 16)) |
                                (input[inputOffset + 3] << 24));
            }
        }

        private static void ToBytes(uint input, byte[] output, int outputOffset)
        {
            unchecked
            {
                output[outputOffset] = (byte)input;
                output[outputOffset + 1] = (byte)(input >> 8);
                output[outputOffset + 2] = (byte)(input >> 16);
                output[outputOffset + 3] = (byte)(input >> 24);
            }
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            return Transform(plaintext);
        }

        public byte[] Decrypt(byte[] chiphertext)
        {
            return Transform(chiphertext);
        }
    }
}
