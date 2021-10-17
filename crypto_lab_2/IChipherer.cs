using System;
using System.Collections.Generic;
using System.Text;

namespace is_lab_2
{
    interface IChipherer
    {
        byte[] Encrypt(byte[] plaintext);

        byte[] Decrypt(byte[] chiphertext);
    }
}
