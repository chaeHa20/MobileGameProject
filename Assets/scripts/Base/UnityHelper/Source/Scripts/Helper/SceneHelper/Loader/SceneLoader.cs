using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class SceneLoader
    {
        private List<SceneLoaderItem> m_items = new List<SceneLoaderItem>();

        public void add(SceneLoaderItem item)
        {
            if (Logx.isActive)
                Logx.assert(null != item, "Invalid SceneLoader.add parameter, item is null");

            m_items.Add(item);
        }

        public void start()
        {
            if (Logx.isActive)
                Logx.assert(0 < m_items.Count, "Failed SceneLoader run, m_items is empty");
            
            UISceneLoader uiLoader = createUILoader();
            if (null == uiLoader)
            {
                runItem(0, null);
            }
            else
            {
                uiLoader.fadeOut(() => runItem(0, uiLoader));
            }
        }

        private void runItem(int itemIndex, UISceneLoader uiLoader)
        {
            if (itemIndex + 1 > m_items.Count)
            {
                if (null != uiLoader)
                {
                    uiLoader.fadeIn();
                    uiLoader.moveBg();
                }
                return;
            }

            SceneLoaderItem item = m_items[itemIndex];

            if (null != uiLoader)
            {
                uiLoader.setTitle(item.name);
                uiLoader.moveBg();
            }

            item.run((p) =>
            {
                if (null != uiLoader)
                    uiLoader.setProgress(p);

                if (1.0f <= p)
                    runItem(itemIndex + 1, uiLoader);
            });
        }

        protected virtual UISceneLoader createUILoader()
        {
            return null;
        }
    }
}