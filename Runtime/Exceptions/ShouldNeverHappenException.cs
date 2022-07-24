using System;
using UnityEngine;

namespace mtti.Funcs
{
    /// <summary>
    /// An exception to throw when something that should never happen does
    /// happen.
    /// </summary>
    public class ShouldNeverHappenException : Exception
    {
        public ShouldNeverHappenException() { }

        public ShouldNeverHappenException(string message) : base(message) { }
    }
}
