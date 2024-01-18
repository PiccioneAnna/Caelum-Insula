using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    public class ScreenTint : MonoBehaviour
    {
        [SerializeField] Color unTintedColor;
        [SerializeField] Color tintedColor;
        float f;
        public float speed = 1f;

        Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void Tint()
        {
            StopAllCoroutines();
            f = 0f;
            StartCoroutine(TintScreen());
        }

        public void UnTint()
        {
            StopAllCoroutines();
            f = 0f;
            StartCoroutine(UnTintScreen());
        }

        private IEnumerator TintScreen()
        {
            while (f < 1f)
            {
                f += Time.deltaTime * speed;
                f = Mathf.Clamp(f, 0, 1f);

                Color c = image.color;
                c = Color.Lerp(unTintedColor, tintedColor, f);
                image.color = c;

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator UnTintScreen()
        {
            while (f < 1f)
            {
                f += Time.deltaTime * speed;
                f = Mathf.Clamp(f, 0, 1f);

                Color c = image.color;
                c = Color.Lerp(tintedColor, unTintedColor, f);
                image.color = c;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}


