using TSKT.Tweens;
using UnityEngine;
#nullable enable

namespace TSKT
{
    public static class Tween
    {
        public static Move Move(GameObject target, float duration, bool scaledTime = true)
        {
            return new Move(target, duration, scaledTime: scaledTime);
        }

        public static Scale Scale(GameObject target, float duration, bool scaledTime = true)
        {
            return new Scale(target, duration, scaledTime: scaledTime);
        }

        public static Rotate Rotate(GameObject target, float duration, bool scaledTime = true)
        {
            return new Rotate(target, duration, scaledTime: scaledTime);
        }

        public static PositionAndRotation MoveAndRotate(GameObject target, float duration, bool scaledTime = true)
        {
            return new PositionAndRotation(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(GameObject target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(SpriteRenderer target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(UnityEngine.UI.Graphic target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(MeshRenderer target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static Tweens.Color Color(CanvasGroup target, float duration, bool scaledTime = true)
        {
            return new Tweens.Color(target, duration, scaledTime: scaledTime);
        }

        public static ShakePosition ShakePosition(GameObject target, float duration, bool scaledTime = true)
        {
            return new ShakePosition(target, duration, scaledTime: scaledTime);
        }

        public static SoundVolume SoundVolume(GameObject target, float duration, bool scaledTime = true)
        {
            return SoundVolume(target.GetComponent<AudioSource>(), duration, scaledTime);
        }

        public static SoundVolume SoundVolume(AudioSource target, float duration, bool scaledTime = true)
        {
            return new SoundVolume(target, duration, scaledTime: scaledTime);
        }

        public static Manipulator<T> Manipulate<T>(T target, float duration, bool scaledTime = true)
            where T : Component
        {
            return new Manipulator<T>(target, duration, scaledTime: scaledTime);
        }

        public static Value Value(float duration, bool scaledTime = true)
        {
            return new Value(duration, scaledTime: scaledTime);
        }
    }
}
