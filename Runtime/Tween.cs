using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TSKT.Tweens;
using System.Linq;

namespace TSKT
{
    public static class Tween
    {
        public static Move Move(GameObject target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.activeInHierarchy, "game object must be active : " + target.name);
            var move = new Move(target, duration, scaledTime: scaledTime);
            return move;
        }

        public static Scale Scale(GameObject target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.activeInHierarchy, "game object must be active : " + target.name);
            var scale = new Scale(target, duration, scaledTime: scaledTime);
            return scale;
        }

        public static Tweens.Color Color(GameObject target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.activeInHierarchy, "game object must be active : " + target.name);
            var color = new Tweens.Color(target, duration, scaledTime: scaledTime);
            return color;
        }

        public static Tweens.Color Color(SpriteRenderer target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.gameObject.activeInHierarchy, "game object must be active : " + target.name);
            var color = new Tweens.Color(target, duration, scaledTime: scaledTime);
            return color;
        }

        public static Tweens.Color Color(UnityEngine.UI.Graphic target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.gameObject.activeInHierarchy, "game object must be active : " + target.name);
            var color = new Tweens.Color(target, duration, scaledTime: scaledTime);
            return color;
        }

        public static Tweens.Color Color(MeshRenderer target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.gameObject.activeInHierarchy, "game object must be active : " + target.name);
            var color = new Tweens.Color(target, duration, scaledTime: scaledTime);
            return color;
        }

        public static Tweens.Color Color(CanvasGroup target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.gameObject.activeInHierarchy, "game object must be active : " + target.name);
            var color = new Tweens.Color(target, duration, scaledTime: scaledTime);
            return color;
        }

        public static ShakePosition ShakePosition(GameObject target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.activeInHierarchy, "game object must be active : " + target.name);
            var color = new ShakePosition(target, duration, scaledTime: scaledTime);
            return color;
        }

        public static SoundVolume SoundVolume(GameObject target, float duration, bool scaledTime = true)
        {
            return SoundVolume(target.GetComponent<AudioSource>(), duration, scaledTime);
        }

        public static SoundVolume SoundVolume(AudioSource target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.gameObject.activeInHierarchy, "game object must be active : " + target.name);
            var soundVolume = new SoundVolume(target, duration, scaledTime: scaledTime);
            return soundVolume;
        }

        public static Value Value(GameObject target, float duration, bool scaledTime = true)
        {
            Debug.Assert(target.activeInHierarchy, "game object must be active : " + target.name);
            var value = new Value(target, duration, scaledTime: scaledTime);
            return value;
        }
    }
}
