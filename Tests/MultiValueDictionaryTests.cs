/*
Copyright 2021 Matti Hiltunen

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

using mtti.Funcs.Collections;
using NUnit.Framework;
using System.Collections.Generic;

namespace mtti.Funcs.Tests
{
    public class MultiValueDictionaryTests
    {
        [Test]
        public void EnumeratorWithNoItems()
        {
            var dict = new MultiValueDictionary<string, int>();

            var result = new List<KeyValuePair<string, int>>();
            foreach (var pair in dict)
            {
                result.Add(pair);
            }

            Assert.AreEqual(
                0,
                result.Count,
                "Result contains no items"
            );
        }

        [Test]
        public void EnumeratorWithItems()
        {
            var originalPairs = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("first", 1),
                new KeyValuePair<string, int>("first", 2),
                new KeyValuePair<string, int>("first", 3),
                new KeyValuePair<string, int>("second", 4),
                new KeyValuePair<string, int>("second", 5),
                new KeyValuePair<string, int>("second", 6),
                new KeyValuePair<string, int>("third", 7),
            };

            var dict = new MultiValueDictionary<string, int>();
            for (int i = 0; i < originalPairs.Length; i++)
            {
                dict.Add(originalPairs[i].Key, originalPairs[i].Value);
            }

            var result = new List<KeyValuePair<string, int>>();
            foreach (var pair in dict)
            {
                result.Add(pair);
            }

            Assert.AreEqual(
                originalPairs.Length,
                result.Count,
                "Result is the expected length"
            );
            CollectionAssert.AreEquivalent(
                originalPairs,
                result,
                "Result contains the same pairs that were inserted"
            );
        }
    }
}
