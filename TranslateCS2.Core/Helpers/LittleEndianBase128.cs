﻿using System.Collections.Generic;
using System.IO;

namespace TranslateCS2.Core.Helpers;
/// <seealso href="https://en.wikipedia.org/wiki/LEB128"/>
public static class LittleEndianBase128 {

    private static readonly int _shiftCountSevenBits = 7;
    private static readonly byte _highOrderBitBitMask = 0x80;
    private static readonly byte _lowOrderBitsBitMask = 0x7f;

    /// <seealso href="https://en.wikipedia.org/wiki/LEB128#Decode_unsigned_integer"/>
    public static uint DecodeUnsignedInteger(Stream stream) {
        uint result = 0;
        int shift = 0;
        byte b;
        do {
            b = (byte) stream.ReadByte();
            result |= (uint) (b & _lowOrderBitsBitMask) << shift;
            shift += _shiftCountSevenBits;
        } while ((b & _highOrderBitBitMask) != 0);
        return result;
    }

    /// <seealso href="https://en.wikipedia.org/wiki/LEB128#Encode_unsigned_integer"/>
    public static byte[] EncodeUnsignedInteger(uint value) {
        List<byte> bytes = [];
        uint work = value;
        byte b;
        do {
            // 0x7f is 01111111 in bits
            // to get low-order 7 bits of work-value
            b = (byte) (work & _lowOrderBitsBitMask);
            work >>= _shiftCountSevenBits;
            if (work != 0) {
                // 0x80 is 10000000 in bits
                // to set high-order bit of work-value
                b |= _highOrderBitBitMask;
            }
            bytes.Add(b);
        } while (work != 0);
        return bytes.ToArray();
    }
}