/*
Adapted from Java Uuid Generator (https://github.com/cowtowncoder/java-uuid-generator)
Copyright Tatu Saloranta, Apache License 2.0.

Copyright 2017-2021 Matti Hiltunen

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;

namespace mtti.Funcs.Types
{
    /// <summary>
    /// Limited pure C# UUID implementation that only supports random
    /// (version 4) UUID's and the empty UUID.
    /// </summary>
    [Serializable]
    public struct Uuid : IEquatable<Uuid>
    {
        public static readonly Uuid Empty = new Uuid(0, 0);

        private static readonly int ByteOffsetType = 6;

        private static readonly int ByteOffsetVariation = 8;

        private static Random s_random = new Random();

        /// <summary>
        /// Equality operator.
        /// </summary>
        public static bool operator ==(Uuid lhs, Uuid rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        public static bool operator !=(Uuid lhs, Uuid rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Generate a new version 4 random UUID.
        /// </summary>
        /// <returns>The generated UUID</returns>
        public static Uuid NewV4()
        {
            var data = new byte[16];
            s_random.NextBytes(data);
            SetSpecialBits(4, data);
            return new Uuid(data);
        }

        public static Uuid FromBase64String(string s)
        {
            if (s == "AAAAAAAAAAAAAAAAAAAAAA==")
            {
                return Uuid.Empty;
            }
            return new Uuid(Convert.FromBase64String(s));
        }

        public static void FromHexString(String id, out long mostSignificantBits,
            out long leastSignificantBits)
        {
            if (id == null)
            {
                throw new NullReferenceException();
            }
            if (id.Length != 36 && id.Length != 32)
            {
                throw new FormatException("UUID has to be 36 or 32 characters long");
            }

            long lo, hi;
            lo = hi = 0;

            for (int i = 0, j = 0; i < id.Length; ++j)
            {
                // Need to bypass hyphens:
                if (id.Length == 36)
                {
                    switch (i)
                    {
                        case 8:
                        case 13:
                        case 18:
                        case 23:
                            if (id[i] != '-')
                            {
                                throw new FormatException("UUID has to be represented by the standard 36-char representation");
                            }
                            ++i;
                            break;
                    }
                }

                int curr;
                char c = id[i];

                if (c >= '0' && c <= '9')
                {
                    curr = (c - '0');
                }
                else if (c >= 'a' && c <= 'f')
                {
                    curr = (c - 'a' + 10);
                }
                else if (c >= 'A' && c <= 'F')
                {
                    curr = (c - 'A' + 10);
                }
                else
                {
                    throw new FormatException("Non-hex character at #" + i + ": '" + c);
                }
                curr = (curr << 4);

                c = id[++i];

                if (c >= '0' && c <= '9')
                {
                    curr |= (c - '0');
                }
                else if (c >= 'a' && c <= 'f')
                {
                    curr |= (c - 'a' + 10);
                }
                else if (c >= 'A' && c <= 'F')
                {
                    curr |= (c - 'A' + 10);
                }
                else
                {
                    throw new FormatException("Non-hex character at #" + i + ": '" + c);
                }
                if (j < 8)
                {
                    hi = (hi << 8) | (uint)curr;
                }
                else
                {
                    lo = (lo << 8) | (uint)curr;
                }
                ++i;
            }

            mostSignificantBits = hi;
            leastSignificantBits = lo;
        }

        private static void SetSpecialBits(int type, byte[] uuidBytes)
        {
            // first, ensure type is ok
            int b = uuidBytes[ByteOffsetType] & 0xF; // clear out high nibble
            b |= type << 4;
            uuidBytes[ByteOffsetType] = (byte)b;
            // second, ensure variant is properly set too
            b = uuidBytes[ByteOffsetVariation] & 0x3F; // remove 2 MSB
            b |= 0x80; // set as '10'
            uuidBytes[ByteOffsetVariation] = (byte)b;
        }

        private static long GatherLong(byte[] buffer, int offset)
        {
            long hi = ((long)GatherInt(buffer, offset)) << 32;
            long lo = (((long)GatherInt(buffer, offset + 4)) << 32);
            lo = (long)((ulong)lo >> 32);
            return hi | lo;
        }

        private static int GatherInt(byte[] buffer, int offset)
        {
            return (buffer[offset] << 24) | ((buffer[offset + 1] & 0xFF) << 16)
                | ((buffer[offset + 2] & 0xFF) << 8) | (buffer[offset + 3] & 0xFF);
        }

        private static void AppendInt(int value, byte[] buffer, int offset)
        {
            buffer[offset++] = (byte)(value >> 24);
            buffer[offset++] = (byte)(value >> 16);
            buffer[offset++] = (byte)(value >> 8);
            buffer[offset] = (byte)value;
        }

        [UnityEngine.SerializeField]
        private long _mostSignificantBits;

        [UnityEngine.SerializeField]
        private long _leastSignificantBits;

        public long MostSignificantBits
        {
            get
            {
                return _mostSignificantBits;
            }
        }

        public long LeastSignificantBits
        {
            get
            {
                return _leastSignificantBits;
            }
        }

        /// <summary>
        /// Is this a valid empty or version 4 UUID?
        /// </summary>
        public bool IsValid
        {
            get
            {
                int version = Version;
                return version == 0 || version == 4;
            }
        }

        public int Version
        {
            get
            {
                if (_mostSignificantBits == 0L && _leastSignificantBits == 0L)
                {
                    return 0;
                }
                return (((int)_mostSignificantBits) >> 12) & 0xF;
            }
        }

        /// <summary>
        /// Construct a UUID from a string representation. Supported formats are
        /// upper or lower case hex string with or without hyphens (lengths 36
        /// or 32 characters respectively) and a 24 character Base64
        /// representation.
        /// </summary>
        public Uuid(string str)
        {
            long mostSignificantBits;
            long leastSignificantBits;

            if (str.Length == 36 || str.Length == 32)
            {
                FromHexString(str, out mostSignificantBits, out leastSignificantBits);
            }
            else if (str.Length == 24)
            {
                byte[] bytes = Convert.FromBase64String(str);
                mostSignificantBits = GatherLong(bytes, 0);
                leastSignificantBits = GatherLong(bytes, 8);
            }
            else
            {
                throw new FormatException("Unrecognized format");
            }

            _mostSignificantBits = mostSignificantBits;
            _leastSignificantBits = leastSignificantBits;
        }

        public Uuid(long mostSignificantBits, long leastSignificantBits)
        {
            _mostSignificantBits = mostSignificantBits;
            _leastSignificantBits = leastSignificantBits;
        }

        public Uuid(byte[] bytes)
        {
            _mostSignificantBits = GatherLong(bytes, 0);
            _leastSignificantBits = GatherLong(bytes, 8);
        }

        public Uuid(byte[] bytes, int offset)
        {
            _mostSignificantBits = GatherLong(bytes, offset);
            _leastSignificantBits = GatherLong(bytes, offset + 8);
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[16];
            ToByteArray(result);
            return result;
        }

        public void ToByteArray(byte[] result)
        {
            ToByteArray(result, 0);
        }

        public void ToByteArray(byte[] result, int offset)
        {
            AppendInt((int)(_mostSignificantBits >> 32), result, offset + 0);
            AppendInt((int)_mostSignificantBits, result, offset + 4);
            AppendInt((int)(_leastSignificantBits >> 32), result, offset + 8);
            AppendInt((int)_leastSignificantBits, result, offset + 12);
        }

        public string ToBase64String()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public string ToHexString()
        {
            return _mostSignificantBits.ToString("x16")
                + _leastSignificantBits.ToString("x16");
        }

        public override string ToString()
        {
            return ToHexString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Uuid))
            {
                return false;
            }
            var other = (Uuid)obj;

            return this._mostSignificantBits == other._mostSignificantBits
                && this._leastSignificantBits == other._leastSignificantBits;
        }

        public bool Equals(Uuid other)
        {
            return this._mostSignificantBits == other._mostSignificantBits
                && this._leastSignificantBits == other._leastSignificantBits;
        }

        public override int GetHashCode()
        {
            return (int)(_mostSignificantBits ^ _leastSignificantBits);
        }
    }
}
