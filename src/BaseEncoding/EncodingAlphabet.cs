/*
     Copyright 2014-2016 Sedat Kapanoglu
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

namespace MeaMod.DNS.BaseEncoding
{
    /// <summary>
    /// A single encoding algorithm can support many different alphabets.
    /// EncodingAlphabet consists of a basis for implementing different
    /// alphabets for different encodings. It's suitable if you want to
    /// implement your own encoding based on the existing base classes.
    /// </summary>
    public abstract class EncodingAlphabet
    {
        /// <summary>
        /// Gets the length of the alphabet.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the characters of the alphabet.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Specifies the highest possible char value in an encoding alphabet
        /// Any char above with would raise an exception
        /// </summary>
        private const int lookupLength = 127;

        /// <summary>
        /// Holds a mapping from character to an actual byte value
        /// The values are held as "value + 1" so a zero would denote "not set"
        /// and would cause an exception.
        /// </summary>
        /// byte[] has no discernible perf impact and saves memory
        internal readonly byte[] ReverseLookupTable = new byte[lookupLength];

        /// <summary>
        /// Generates a standard invalid character exception for alphabets.
        /// </summary>
        /// <remarks>
        /// The reason this is not a throwing method itself is
        /// that the compiler has no way of knowing whether the execution
        /// will end after the method call and can incorrectly assume
        /// reachable code.
        /// </remarks>
        /// <param name="c">Characters.</param>
        /// <returns>Exception to be thrown.</returns>
        public Exception InvalidCharacter(char c)
        {
            return new ArgumentException($"Invalid character: {c}");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingAlphabet"/> class.
        /// </summary>
        /// <param name="length">Length of the alphabe.</param>
        /// <param name="alphabet">Alphabet character.</param>
        public EncodingAlphabet(int length, string alphabet)
        {
            Require.NotNull(alphabet, nameof(alphabet));
            if (alphabet.Length != length)
            {
                throw new ArgumentException($"Required alphabet length is {length} but provided alphabet is "
                                          + $"{alphabet.Length} characters long");
            }
            Length = length;
            Value = alphabet;

            for (short i = 0; i < length; i++)
            {
                Map(alphabet[i], i);
            }
        }

        /// <summary>
        /// Map a character to a value.
        /// </summary>
        /// <param name="c">Characters.</param>
        /// <param name="value">Corresponding value.</param>
        protected void Map(char c, int value)
        {
            if (c >= lookupLength)
            {
                throw new InvalidOperationException($"Alphabet contains character above {lookupLength}");
            }
            ReverseLookupTable[c] = (byte)(value + 1);
        }

        /// <summary>
        /// Get the string representation of the alphabet.
        /// </summary>
        /// <returns>The characters of the encoding alphabet.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}