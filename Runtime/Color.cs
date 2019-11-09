using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TSKT.Tweens
{
    public class Color : Task
    {
        Graphic targetGraphic;
        SpriteRenderer targetSprite;
        MeshRenderer meshRenderer;
        CanvasGroup canvasGroup;

        float? toAlpha;
        float? toRed;
        float? toGreen;
        float? toBlue;
        float? fromAlpha;
        float? fromRed;
        float? fromGreen;
        float? fromBlue;

        Func<float, float, float, float> function = EasingFunction.Cubic.EaseOut;

        public Color(GameObject target, float duration, bool scaledTime) : base(target, duration, scaledTime: scaledTime)
        {
            if (!(targetGraphic = target.GetComponent<Graphic>()))
            {
                if (!(targetSprite = target.GetComponent<SpriteRenderer>()))
                {
                    if (!(meshRenderer = target.GetComponent<MeshRenderer>()))
                    {
                        canvasGroup = target.GetComponent<CanvasGroup>();
                    }
                }
            }
        }

        public Color To(UnityEngine.Color value)
        {
            toAlpha = value.a;
            toRed = value.r;
            toGreen = value.g;
            toBlue = value.b;
            return this;
        }

        public Color ToAlpha(float value)
        {
            toAlpha = value;
            return this;
        }

        public Color From(UnityEngine.Color value)
        {
            fromAlpha = value.a;
            fromRed = value.r;
            fromGreen = value.g;
            fromBlue = value.b;
            return this;
        }

        public Color FromAlpha(float value)
        {
            fromAlpha = value;
            return this;
        }

        public Color Function(Func<float, float, float, float> value)
        {
            function = value;
            return this;
        }

        protected override void Apply()
        {
            if (!toAlpha.HasValue
                && !toRed.HasValue
                && !toGreen.HasValue
                && !toBlue.HasValue)
            {
                if (targetGraphic)
                {
                    To(targetGraphic.color);
                }
                else if (targetSprite)
                {
                    To(targetSprite.color);
                }
                else if (meshRenderer)
                {
                    To(meshRenderer.material.color);
                }
                else if (canvasGroup)
                {
                    To(new UnityEngine.Color(1f, 1f, 1f, canvasGroup.alpha));
                }
            }
            if (!fromAlpha.HasValue
                && !fromRed.HasValue
                && !fromGreen.HasValue
                && !fromBlue.HasValue)
            {
                if (targetGraphic)
                {
                    From(targetGraphic.color);
                }
                else if (targetSprite)
                {
                    From(targetSprite.color);
                }
                else if (meshRenderer)
                {
                    From(meshRenderer.material.color);
                }
                else if (canvasGroup)
                {
                    From(new UnityEngine.Color(1f, 1f, 1f, canvasGroup.alpha));
                }
            }

            var color = UnityEngine.Color.white;
            if (targetGraphic)
            {
                color = targetGraphic.color;
            }
            if (targetSprite)
            {
                color = targetSprite.color;
            }
            if (meshRenderer)
            {
                color = meshRenderer.material.color;
            }
            if (canvasGroup)
            {
                color = new UnityEngine.Color(1f, 1f, 1f, canvasGroup.alpha);
            }

            if (fromAlpha.HasValue && toAlpha.HasValue)
            {
                color.a = function.Invoke(fromAlpha.Value, toAlpha.Value, NormalizdElapsedTime);
            }
            if (fromRed.HasValue && toRed.HasValue)
            {
                color.r = function.Invoke(fromRed.Value, toRed.Value, NormalizdElapsedTime);
            }
            if (fromGreen.HasValue && toGreen.HasValue)
            {
                color.g = function.Invoke(fromGreen.Value, toGreen.Value, NormalizdElapsedTime);
            }
            if (fromBlue.HasValue && toBlue.HasValue)
            {
                color.b = function.Invoke(fromBlue.Value, toBlue.Value, NormalizdElapsedTime);
            }

            if (targetGraphic)
            {
                targetGraphic.color = color;
            }
            else if (targetSprite)
            {
                targetSprite.color = color;
            }
            else if (meshRenderer)
            {
                meshRenderer.material.color = color;
            }
            else if (canvasGroup)
            {
                canvasGroup.alpha = color.a;
            }
        }
    }
}
