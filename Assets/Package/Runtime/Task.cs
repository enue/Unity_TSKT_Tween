#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

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
        ReactiveProperty<FinishType>? subject;
        readonly CancellationToken cancellationToken;
        public bool Halted => finishType.HasValue && finishType.Value == FinishType.Halted;
        FinishType? finishType;

        public Task(GameObject? target, CancellationToken cancellationToken, float duration, bool scaledTime)
        {
            Duration = duration;
            this.scaledTime = scaledTime;
            this.target = target;
            hasTarget = target;
            this.cancellationToken = cancellationToken;

            if (scaledTime)
            {
                startedTime = Time.time;
            }
            else
            {
                startedTime = Time.realtimeSinceStartup;
            }

            Run().LogExceptionsAndForget();
        }

        async Awaitable Run()
        {
            try
            {
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, cancellationToken);
                    if (Halted)
                    {
                        break;
                    }
                    if (hasTarget)
                    {
                        if (!target)
                        {
                            finishType = FinishType.DestroyedGameObject;
                            subject?.OnNext(finishType.Value);
                            subject?.OnCompleted();
                            break;
                        }
                        if (!target!.activeInHierarchy)
                        {
                            finishType = FinishType.DestroyedGameObject;
                            subject?.OnNext(finishType.Value);
                            subject?.OnCompleted();
                            break;
                        }
                    }

                    Apply();

                    if (ElapsedTime >= Duration)
                    {
                        finishType = FinishType.Completed;
                        subject?.OnNext(finishType.Value);
                        subject?.OnCompleted();
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                finishType = FinishType.DestroyedGameObject;
                subject?.OnNext(finishType.Value);
                subject?.OnCompleted();
            }
        }

        protected abstract void Apply();

        public bool Finished => finishType.HasValue;

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


        [Obsolete]
        public Awaitable<FinishType> Awaitable => Wait();

        public async Awaitable<FinishType> Wait(CancellationToken ct = default)
        {
            if (finishType.HasValue)
            {
                return finishType.Value;
            }
            subject ??= new();
            return await subject.LastAsync(ct);
        }

        public void Halt()
        {
            finishType = FinishType.Halted;
            subject?.OnNext(finishType.Value);
            subject?.OnCompleted();
        }
        public Task RegisterCancellationToken(CancellationToken cancellationToken)
        {
            cancellationToken.Register(Halt);
            return this;
        }
    }
}
