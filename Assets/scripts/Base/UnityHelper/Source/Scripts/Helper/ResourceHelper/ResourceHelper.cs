using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityHelper
{
    public class ResourceHelper : MonoSingleton<ResourceHelper>
    {
        [SerializeField] List<SpriteAtlas> m_spriteAtlas = new List<SpriteAtlas>();

        private Dictionary<string, Object> m_resourcePools = new Dictionary<string, Object>();
        private Dictionary<string, Sprite> m_spritePools = new Dictionary<string, Sprite>();
        private TempInstanceBuffer m_tempInstanceBuffer = new TempInstanceBuffer();

        public TempInstanceBuffer tempInstanceBuffer => m_tempInstanceBuffer;

        public Sprite loadSprite(string resPath)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");

            return Resources.Load<Sprite>(resPath);
        }

        public Material loadMaterial(string resPath)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");

            return Resources.Load<Material>(resPath);
        }

        public Texture loadTexture(string resPath)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");

            return Resources.Load<Texture>(resPath);
        }

        public Sprite getSprite(string resPath)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");

            if (!m_spritePools.TryGetValue(resPath, out Sprite sprite))
            {
                sprite = getSpriteInAtlas(resPath);
                if (null == sprite)
                {
                    sprite = load<Sprite>(resPath);
                }

                if (null != sprite)
                {
                    m_spritePools.Add(resPath, sprite);
                }
            }

            return sprite;
        }

        private Sprite getSpriteInAtlas(string spriteName)
        {
            foreach (SpriteAtlas spriteAtlas in m_spriteAtlas)
            {
                Sprite sprite = spriteAtlas.GetSprite(spriteName);
                if (null != sprite)
                {
                    return sprite;
                }
            }

            return null;
        }

        public R instantiate<R>(string resPath) where R : Component
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");

            GameObject obj = load<GameObject>(resPath);
            if (null == obj)
                return default;

            GameObject instant = GameObject.Instantiate<GameObject>(obj);
            if (null == instant)
                return default;

            return instant.GetComponent<R>();
        }

        public void instantiateAsync<R>(string resPath, System.Action<R> callback) where R : Component
        {
            if (Logx.isActive)
            {
                Logx.assert(!string.IsNullOrEmpty(resPath), "resPath is null or empty");
                Logx.assert(null != callback, "callback is null");
            }

            asyncLoad<GameObject>(resPath, (obj) =>
            {
                GameObject instant = GameObject.Instantiate<GameObject>(obj);

                if (null == instant)
                    callback(default);
                else
                    callback(instant.GetComponent<R>());
            });
        }

        protected R load<R>(string resPath) where R : Object
        {
            if (m_resourcePools.TryGetValue(resPath, out Object obj))
            {
                return obj as R;
            }
            else
            {
                R t = Resources.Load<R>(resPath);
                if (null == t)
                {
                    if (Logx.isActive)
                        Logx.error("Failed loadObject(), {0} is not found", resPath);
                    return null;
                }

                m_resourcePools.Add(resPath, t);
                return t;
            }
        }

        private void asyncLoad<R>(string resPath, System.Action<R> callback) where R : Object
        {
            if (m_resourcePools.TryGetValue(resPath, out Object obj))
            {
                callback(obj as R);
            }
            else
            {
                ResourceRequest req = Resources.LoadAsync<R>(resPath);
                StartCoroutine(coAsyncLoad<R>(resPath, req, callback));
            }
        }

        IEnumerator coAsyncLoad<R>(string resPath, ResourceRequest req, System.Action<R> callback) where R : Object
        {
            while (!req.isDone)
            {
                yield return null;
            }

            R t = req.asset as R;
            if (null == t)
            {
                if (Logx.isActive)
                    Logx.error("Failed loadObject(), {0} is not found", resPath);
            }
            else
            {
                m_resourcePools.Add(resPath, t);
            }

            callback(t);
        }
    }
}