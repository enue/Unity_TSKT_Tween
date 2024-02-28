﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
#nullable enable

namespace TSKT.Tweens
{
    public abstract class StandaloneTask : ITask
    {
        public enum FinishType
        {
            Completed,
            Halted,
        }

        readonly float startedTime;
        readonly float duration;
        readonly bool scaledTime;
        AwaitableCompletionSource<FinishType>? completion;
        public bool Halted { get; private set; }

        public StandaloneTask(float duration, bool scaledTime)
        {
            this.duration = duration;
            this.scaledTime = scaledTime;

            if (scaledTime)
            {
                startedTime = Time.time;
            }
            else
            {
                startedTime = Time.realtimeSinceStartup;
            }

            _ = Update();
        }

        async Awaitable Update()
        {
            while (true)
            {
                await UnityEngine.Awaitable.EndOfFrameAsync();

                if (Halted)
                {
                    break;
                }

                Apply();

                if (ElapsedTime >= duration)
                {
                    completion?.TrySetResult(FinishType.Completed);
                    break;
                }
            }
        }

        abstract protected void Apply();

        public bool Finished
        {
            get
            {
                return Halted
                    || (ElapsedTime >= duration);
            }
        }

        protected float NormalizedElapsedTime
        {
            get
            {
                if (duration == 0f)
                {
                    return 1f;
                }

                return Mathf.Clamp01(ElapsedTime / duration);
            }
        }

        float ElapsedTime
        {
            get
            {
                if (scaledTime)
                {
                    return Time.time - startedTime;
                }
                else
                {
                    return Time.realtimeSinceStartup - startedTime;
                }
            }
        }


        public UniTask<FinishType> UniTask => Awaitable.AsUniTask();

        public Awaitable<FinishType> Awaitable
        {
            get
            {
                completion ??= new AwaitableCompletionSource<FinishType>();
                return completion.Awaitable;
            }
        }

        public void Halt()
        {
            Halted = true;
            completion?.TrySetResult(FinishType.Halted);
        }
    }
}
