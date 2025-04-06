using TSKT.Tweens;
using UnityEngine;
#nullable enable

namespace TSKT
{
    public static class Tween
    {
        public static Move Move(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Move(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static Scale Scale(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Scale(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static Rotate Rotate(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Rotate(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static PositionAndRotation MoveAndRotate(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new PositionAndRotation(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static Tweens.Color Color(SpriteRenderer target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static Tweens.Color Color(UnityEngine.UI.Graphic target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(MeshRenderer target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static Tweens.Color Color(CanvasGroup target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static ShakePosition ShakePosition(GameObject target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new ShakePosition(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static SoundVolume SoundVolume(AudioSource target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new SoundVolume(target, duration, scaledTime: scaledTime, destroyCancellationToken);
        }

        public static Manipulator<T> Manipulate<T>(T target, float duration, bool scaledTime = true, System.Threading.CancellationToken destroyCancellationToken = default)
            where T : Component
        {
            return new Manipulator<T>(target, duration, scaledTime: scaledTime, destroyCancellationToken: destroyCancellationToken);
        }

        public static Value Value(float duration, bool scaledTime = true, GameObject? target = null, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Value(duration, scaledTime: scaledTime, target, destroyCancellationToken);
        }
        public static Value<T> Value<T>(float duration, bool scaledTime = true, GameObject? target = null, System.Threading.CancellationToken destroyCancellationToken = default)
        {
            return new Value<T>(duration, scaledTime: scaledTime, target, destroyCancellationToken);
        }
    }
}
