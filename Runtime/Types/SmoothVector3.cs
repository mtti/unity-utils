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

using UnityEngine;

namespace mtti.Funcs
{
    public struct SmoothVector3
    {
        public Vector3 Value;

        public Vector3 Target;

        public Vector3 CurrentVelocity;

        public float SmoothTime;

        public float MaxSpeed;

        public SmoothVector3(
            float smoothTime = 0.01f,
            float maxSpeed = Mathf.Infinity
        )
        {
            Value = Vector3.zero;
            Target = Vector3.zero;
            CurrentVelocity = Vector3.zero;
            SmoothTime = smoothTime;
            MaxSpeed = maxSpeed;
        }

        public void Reset(Vector3 value)
        {
            Value = value;
            Target = value;
            CurrentVelocity = Vector3.zero;
        }

        public void Update(Vector3 target, float deltaTime)
        {
            Target = target;
            Update(deltaTime);
        }

        public void Update(float deltaTime)
        {
            Value = Vector3.SmoothDamp(
                Value,
                Target,
                ref CurrentVelocity,
                SmoothTime,
                MaxSpeed,
                deltaTime
            );
        }
    }
}
