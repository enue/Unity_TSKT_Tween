using TSKT.Tweens;
using UnityEngine;
#nullable enable

namespace TSKT
{
    public static class Tween
    {
        public static Move Move(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Move(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static Scale Scale(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Scale(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static Rotate Rotate(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Rotate(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static PositionAndRotation MoveAndRotate(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new PositionAndRotation(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static Tweens.Color Color(SpriteRenderer target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static Tweens.Color Color(UnityEngine.UI.Graphic target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(MeshRenderer target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static Tweens.Color Color(CanvasGroup target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static ShakePosition ShakePosition(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new ShakePosition(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static SoundVolume SoundVolume(AudioSource target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
        {
            return new SoundVolume(target, duration, scaledTime: scaledTime, cancellationToken);
        }

        public static Manipulator<T> Manipulate<T>(T target, float duration, bool scaledTime = true, System.Threading.CancellationToken cancellationToken = default)
            where T : Component
        {
            return new Manipulator<T>(target, duration, scaledTime: scaledTime, cancellationToken: cancellationToken);
        }

        public static Value Value(float duration, bool scaledTime = true, GameObject? target = null, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Value(duration, scaledTime: scaledTime, target, cancellationToken);
        }
        public static Value<T> Value<T>(float duration, bool scaledTime = true, GameObject? target = null, System.Threading.CancellationToken cancellationToken = default)
        {
            return new Value<T>(duration, scaledTime: scaledTime, target, cancellationToken);
        }
    }
}
