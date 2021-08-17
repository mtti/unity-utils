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
    public struct SmoothAngle
    {
        public float Value;

        public float Target;

        public float CurrentVelocity;

        public float MaxSpeed;

        public float SmoothTime;

        public SmoothAngle(float smoothTime)
        {
            Value = 0.0f;
            Target = 0.0f;
            CurrentVelocity = 0.0f;
            SmoothTime = smoothTime;
            MaxSpeed = Mathf.Infinity;
        }

        public void Reset(float value)
        {
            Value = value;
            Target = value;
            CurrentVelocity = 0.0f;
        }

        public void Update(float target, float deltaTime)
        {
            Target = target;
            Update(deltaTime);
        }

        public void Update(float deltaTime)
        {
            Value = Mathf.SmoothDampAngle(
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
