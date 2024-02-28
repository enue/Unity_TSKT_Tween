#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace TSKT.Tweens
{
    public abstract class Task : ITask
    {
        public enum FinishType
        {
            Completed,
            Halted,
            DestroyedGameObject,
        }

        GameObject Target { get; }
        readonly float startedTime;
        public float Duration { get; }
        readonly bool scaledTime;
        AwaitableCompletionSource<FinishType>? completion;
        public bool Halted { get; private set; }

        public Task(GameObject target, CancellationToken destroyCancellationToken, float duration, bool scaledTime)
        {
            Duration = duration;
            this.scaledTime = scaledTime;
            Target = target;

            if (scaledTime)
            {
                startedTime = Time.time;
            }
            else
            {
                startedTime = Time.realtimeSinceStartup;
            }

            _ = Update(destroyCancellationToken);
        }

        async Awaitable Update(CancellationToken destroyCancellationToken)
        {
            try
            {
                while (true)
                {
                    await UnityEngine.Awaitable.EndOfFrameAsync(destroyCancellationToken);

                    if (Halted)
                    {
                        break;
                    }
                    if (!Target)
                    {
                        completion?.TrySetResult(FinishType.DestroyedGameObject);
                        break;
                    }

                    Apply();

                    if (ElapsedTime >= Duration)
                    {
                        completion?.TrySetResult(FinishType.Completed);
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                completion?.TrySetResult(FinishType.DestroyedGameObject);
            }
        }

        protected abstract void Apply();

        public bool Finished
        {
            get
            {
                return Halted
                    || (ElapsedTime >= Duration)
                    || !Target;
            }
        }

        public float NormalizedElapsedTime
        {
            get
            {
                if (Duration == 0f)
                {
                    return 1f;
                }

                return Mathf.Clamp01(ElapsedTime / Duration);
            }
        }

        public float ElapsedTime
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
                completion ??= new();
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
