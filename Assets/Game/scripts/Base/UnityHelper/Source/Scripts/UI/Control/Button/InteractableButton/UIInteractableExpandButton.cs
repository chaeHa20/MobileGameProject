using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnityHelper
{
    public class UIInteractableExpandButton : UIInteractableBaseButton
    {
        [Serializable]
        public class ImageInfo
        {
            public Image image;
            public Sprite interactableSprite;
            public Color interactableColor = Color.white;
            public Sprite notInteractableSprite;
            public Color notInteractableColor = Color.gray;            

            public void setInteractable(bool isInteractable)
            {
                if (null == image)
                    return;

                if (isInteractable)
                {
                    if (null != interactableSprite)
                        image.sprite = interactableSprite;
                    image.color = interactableColor;
                }
                else
                {
                    if (null != notInteractableSprite)
                        image.sprite = notInteractableSprite;
                    image.color = notInteractableColor;
                }
            }
        }

        [Serializable]
        public class TextInfo
        {
            public TextSelector text;
            public Color interactableColor = Color.white;
            public Color notInteractableColor = Color.gray;

            public void setInteractable(bool isInteractable)
            {
                if (null == text.gameObject)
                    return;

                if (isInteractable)
                {
                    text.color = interactableColor;
                }
                else
                {
                    text.color = notInteractableColor;
                }
            }
        }

        [SerializeField] protected List<ImageInfo> m_imageInfos = new List<ImageInfo>();
        [SerializeField] protected List<TextInfo> m_textInfos = new List<TextInfo>();

        public override void setInteractable(bool isInteractable, bool isOnlyGraphicModify = false)
        {
            base.setInteractable(isInteractable, isOnlyGraphicModify);

            foreach (var imageInfo in m_imageInfos)
            {
                imageInfo.setInteractable(isInteractable);
            }

            foreach (var textInfo in m_textInfos)
            {
                textInfo.setInteractable(isInteractable);
            }
        }

        protected void setText(int textInfoIndex, string text)
        {
            if (m_textInfos.Count <= textInfoIndex)
                return;

            if (null == m_textInfos[textInfoIndex].text.gameObject)
                return;

            m_textInfos[textInfoIndex].text.text = text;
        }

        public void changeButtonSprite(int spriteId)
        {
            if (0 == m_imageInfos.Count)
                return;

            m_imageInfos[0].image.sprite = GameResourceHelper.getInstance().getSprite(spriteId);
        }
    }
}