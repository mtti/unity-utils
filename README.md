[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-333.svg?style=flat-square&logo=unity)](https://unity.com/) [![License](https://img.shields.io/badge/license-Apache--2.0-blue.svg?style=flat-square)](https://github.com/mtti/unity-funcs/blob/master/LICENSE)

A collection of reusable utilities for my Unity projects.

Be warned that while I make this repo available publicly in the hopes it will be
useful for someone, it's intended for my own use and as such anything can be
added or removed arbitrarily and anything can break or be broken at any time.

## Installation

### As a Git dependency

Add to `manifest.json`, under `dependencies`:

```
"com.mattihiltunen.utils": "https://github.com/mtti/unity-utils.git",
```

### As an embedded Git submodule

```
git submodule add git@github.com:mtti/unity-utils.git Packages/com.mattihiltunen.utils
```

You can also add the `manifest.json` entry as above to document the dependency
to this library.

## License and acknowledgements

This library is &copy; Matti Hiltunen, released under the Apache License,
version 2.0 (the "License"). Unless required by applicable law or agreed to in
writing, software distributed under the License is distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and limitations
under the License.

### Hash functions

This project contains hash function code copyright &copy;2015
Rune Skovbo Johansen, originally from https://bitbucket.org/runevision/random-numbers-testing/
(link's broken, but I'm keeping it here for historical and legal reasons).

I've modified the original code slightly by placing it under the
`mtti.Procedural.Hashes` namespace to better fit into this library. I've also
ported the xxHash implementation to Unity Burst (see `NativeXXHash.cs`).

Original copyright notices:

* C# implementation of xxHash optimized for producing random numbers from one or more input integers. Copyright &copy;2015, Rune Skovbo Johansen. (https://bitbucket.org/runevision/random-numbers-testing/)
    * Mozilla Public License, v. 2.0. (http://mozilla.org/MPL/2.0/).
* Based on C# implementation Copyright &copy;2014, Seok-Ju, Yun. (https://github.com/noricube/xxHashSharp)
    * BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)
* Original C Implementation Copyright &copy;2012-2014, Yann Collet. (https://code.google.com/p/xxhash/)
    * BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)
