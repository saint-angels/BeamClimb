using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public class UIClickableElementsDebug : MonoBehaviour
    {
        public static KeyCode showKey = KeyCode.C;

        private bool showing = false;
        private readonly Dictionary<Graphic, Color> clickableColors = new Dictionary<Graphic, Color>();
        private Texture2D debugTexture;
        private GUIStyle textLabelStyle;

        void Awake()
        {
            debugTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            textLabelStyle = new GUIStyle {normal = {textColor = Color.magenta}, fontSize = 24};
            
            //showKey was already pressed, when this debug script was added
            showing = false;
        }

        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                showing = true;
            }
            
            if (Input.GetKeyUp(KeyCode.C))
            {
                showing = false;
                clickableColors.Clear();
            }
        }

        private void OnGUI()
        {
            if (showing == false)
            {
                return;
            }
            
            foreach (Graphic graphic in GetComponentsInChildren<Graphic>())
            {
                if (graphic.gameObject.activeInHierarchy && graphic.raycastTarget)
                {
                    Color color;
                    if (clickableColors.ContainsKey(graphic))
                    {
                        color = clickableColors[graphic];
                    }
                    else
                    {
                        color = UnityEngine.Random.ColorHSV();
                        color.a = .5f;
                        clickableColors.Add(graphic, color);
                    }
                    
                    debugTexture.SetPixel(0,0, color);
                    debugTexture.Apply();
                    Rect rect = RectTransformToScreenSpace(graphic.rectTransform);
                    GUI.DrawTexture(rect, debugTexture, ScaleMode.ScaleToFit, true, rect.width / rect.height);
                    GUI.Label(new Rect(rect.position.x, rect.position.y, 100, 20), graphic.gameObject.name, textLabelStyle);
                }
            }
        }

        private static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
            rect.x -= (transform.pivot.x * size.x);
            rect.y -= ((1.0f - transform.pivot.y) * size.y);
            return rect;
        }
    }
}
