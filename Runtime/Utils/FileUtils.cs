/*
Copyright 2017-2020 Matti Hiltunen

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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mtti.Funcs
{
    public static class FileUtils
    {
        public static string FormatArray(object[] subject)
        {
            var sb = new StringBuilder();

            sb.Append("[");
            for (var i = 0; i < subject.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(subject[i]);
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static string FormatList<T>(List<T> subject)
        {
            var sb = new StringBuilder();

            sb.Append("[");
            for (int i = 0, count = subject.Count; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(subject[i]);
            }
            sb.Append("]");

            return sb.ToString();
        }

        public static string FormatFileSize(double bytes)
        {
            if (bytes < 1000)
            {
                return string.Format("{0} bytes", bytes);
            }
            else if (bytes < 1000000)
            {
                return string.Format("{0} kilobytes", bytes / 1000);
            }
            else if (bytes < 1000000000)
            {
                return string.Format("{0} megabytes", bytes / 1000000);
            }
            else
            {
                return string.Format("{0} gigabytes", bytes / 1000000000);
            }
        }

        public static string[] SplitPath(string path)
        {
            return SplitPath(path, Path.DirectorySeparatorChar);
        }

        public static string[] SplitPath(string path, char separator)
        {
            return path.Split(separator);
        }

        public static string GetLastPathElement(string path, char separator)
        {
            var parts = SplitPath(path, separator);
            return parts[parts.Length - 1];
        }

        public static string GetRelativePath(string fromPath, string toPath)
        {
            return GetRelativePath(fromPath, toPath, Path.DirectorySeparatorChar);
        }

        public static string GetRelativePath(
            string fromPath,
            string toPath,
            char separator
        )
        {
            var fromParts = SplitPath(fromPath, separator);
            var toParts = SplitPath(toPath, separator);
            var newParts = new List<string>();

            int lastMatchingIndex = -1;
            for (
                int i = 0, count = Math.Min(fromParts.Length, toParts.Length);
                i < count;
                i++
            )
            {
                if (fromParts[i] == toParts[i])
                {
                    lastMatchingIndex = i;
                }
                else
                {
                    break;
                }
            }

            if (lastMatchingIndex == -1)
            {
                return fromPath;
            }

            int backtrackLevels = fromParts.Length - (lastMatchingIndex + 1);

            for (var i = 0; i < backtrackLevels; i++)
            {
                newParts.Add("..");
            }
            for (var i = lastMatchingIndex + 1; i < toParts.Length; i++)
            {
                newParts.Add(toParts[i]);
            }

            var sb = new StringBuilder();
            for (int i = 0, count = newParts.Count; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(newParts[i]);
            }

            return sb.ToString();
        }
    }
}
