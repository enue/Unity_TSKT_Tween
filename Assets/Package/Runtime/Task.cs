#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace TSKT.Tweens
{
    public abstract class Task
    {
        public enum FinishType
        {
            Completed,
            Halted,
            DisabledGameObject,
            DestroyedGameObject,
        }

        readonly GameObject? target;
        readonly bool hasTarget;
        
        readonly float startedTime;
        public float Duration { get; }
        readonly bool scaledTime;
        AwaitableCompletionSource<FinishType>? completion;
        readonly CancellationToken destroyCancellationToken;
        public bool Halted { get; private set; }

        public Task(GameObject? target, CancellationToken destroyCancellationToken, float duration, bool scaledTime)
        {
            Duration = duration;
            this.scaledTime = scaledTime;
            this.target = target;
            hasTarget = target;
            this.destroyCancellationToken = destroyCancellationToken;

            if (scaledTime)
            {
                startedTime = Time.time;
            }
            else
            {
                startedTime = Time.realtimeSinceStartup;
            }

            _ = Run();
        }

        async Awaitable Run()
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
                    if (hasTarget)
                    {
                        if (!target)
                        {
                            completion?.TrySetResult(FinishType.DestroyedGameObject);
                            break;
                        }
                        if (!target!.activeInHierarchy)
                        {
                            completion?.TrySetResult(FinishType.DisabledGameObject);
                            break;
                        }
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
                    || (hasTarget && (!target || !target!.activeInHierarchy));
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
        public Task RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);
            return this;
        }
    }
}
