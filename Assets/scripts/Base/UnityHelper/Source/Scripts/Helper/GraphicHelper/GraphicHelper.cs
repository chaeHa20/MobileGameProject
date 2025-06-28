using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace UnityHelper
{
    public class GraphicHelper
    {
        private static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static Color hexToColor(string hex)
        {
            int dec = System.Convert.ToInt32(hex, 16);

            int r = (dec >> 16) & 255;
            int g = (dec >> 8) & 255;
            int b = (dec) & 255;

            Color c = new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
            return c;
        }

        /// <summary>
        /// https://www.cambiaresearch.com/articles/1/convert-dotnet-color-to-hex-string
        /// </summary>
        public static string colorToHex(Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = (byte)(color.r * 255.0f);
            bytes[1] = (byte)(color.g * 255.0f);
            bytes[2] = (byte)(color.b * 255.0f);
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }

        public static Color randColor()
        {
            return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
        }

        public static Sprite texture2DToSprite(Texture2D texture)
        {
            if (null == texture)
                return null;

            Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(texture, rect, pivot);
        }

        public static Sprite colorToSprite(int width, int height, Color[] colors, TextureFormat format = TextureFormat.RGBA32, bool mipmap = false, bool linear = true)
        {
            Texture2D texture = new Texture2D(width, height, format, mipmap, linear);
            texture.SetPixels(colors);

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, (float)width, (float)height), Vector2.zero);
            return sprite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="callback">texture, error</param>
        /// <returns></returns>
        public static IEnumerator coDownloadTexture(string imageUrl, Action<Texture2D, string> callback)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                callback(null, "imageUrl is empty");
            }
            else
            {
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
                {
                    yield return uwr.SendWebRequest();

                    string error = null;
                    Texture2D texture = null;
                    if (uwr.result == UnityWebRequest.Result.ConnectionError)
                    {
                        if (Logx.isActive)
                            Logx.error("Failed download profile, error : {0}", uwr.error);

                        error = uwr.error;
                    }
                    else
                    {
                        texture = DownloadHandlerTexture.GetContent(uwr);
                    }

                    callback?.Invoke(texture, error);
                }
            }
        }

        public static bool getPickingPoint(Camera camera, Collider collider, ref Vector3 point)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != camera, "camera is null");
                Logx.assert(null != collider, "collider is null");
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            bool isCollision = collider.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
            if (isCollision)
            {
                point = hitInfo.point;
            }

            return isCollision;
        }

        public static bool getPickingPoint(Camera camera, int layerMask, string tag, ref Vector3 point)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    if (!hitInfo.transform.CompareTag(tag))
                        return false;
                }

                point = hitInfo.point;
                return true;
            }

            return false;
        }

        public static bool isPicking(Camera camera, Collider collider)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != camera, "camera is null");
                Logx.assert(null != collider, "collider is null");
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            return collider.Raycast(ray, out _, Mathf.Infinity);
        }

        public static void findHitPoint2D(Vector3 origin, Vector3 direction, float distance, string compareTag, Action<bool, Vector3> callback)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);
            foreach (var hit in hits)
            {
                if (null != hit.collider)
                {
                    if (hit.collider.CompareTag(compareTag))
                    {
                        callback?.Invoke(true, new Vector3(hit.point.x, hit.point.y, origin.z));
                        return;
                    }
                }
            }

            callback?.Invoke(false, Vector3.zero);
        }

        /// <summary>
        /// CanvasRenderer는 material을 공유하기 때문에 material을 새로 할당해줘야 된다.
        /// https://forum.unity.com/threads/changing-colors-of-a-material-for-separate-prefab-instances.281551/
        /// </summary>
        public static void setGrayScaleMaterial(Image image, float value)
        {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            image.material = new Material(Shader.Find("UnityHelper/UIGrayScale"));
            setGrayScaleValue(image, value);
        }

        public static void setGrayScaleValue(Image image, float value)
        {
            image.material.SetFloat("_GrayScaleAmount", value);
        }

        public static List<Material> copyMaterials(Material[] srcMaterials)
        {
            var materials = new List<Material>();
            for (int i = 0; i < srcMaterials.Length; ++i)
            {
                var material = new Material(srcMaterials[i]);
                materials.Add(material);
            }

            return materials;
        }

        public static void setParent(GameObject parent, GameObject child, bool isResetLocalPosition = true, bool isResetLocalRotation = true, bool isResetLocalScale = true)
        {
            if (null == parent)
            {
                child.transform.SetParent(null);
            }
            else
            {
                child.transform.SetParent(parent.transform);
                if (isResetLocalPosition) child.transform.localPosition = Vector3.zero;
                if (isResetLocalRotation) child.transform.localRotation = Quaternion.identity;
                if (isResetLocalScale) child.transform.localScale = Vector3.one;
            }
        }

        public static GameObject findInChildWithTag(GameObject gameObject, string tag)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != gameObject, "gameObject is null");
                Logx.assert(!string.IsNullOrEmpty(tag), "tag is null or empty");
            }

            Transform transform = gameObject.transform;
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        public static GameObject findInChildsWithTag(GameObject gameObject, string tag)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != gameObject, "gameObject is null");
                Logx.assert(!string.IsNullOrEmpty(tag), "tag is null or empty");
            }

            Transform transform = gameObject.transform;
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }
                else
                {
                    GameObject c = findInChildsWithTag(child.gameObject, tag);
                    if (null != c)
                        return c;
                }
            }

            return null;
        }

        public static void findInChilds<T>(GameObject gameObject, List<T> finds) where T : MonoBehaviour
        {
            Transform transform = gameObject.transform;
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                T t = child.GetComponent<T>();
                if (null != t)
                    finds.Add(t);

                findInChilds<T>(child.gameObject, finds);
            }
        }

        public static void setLayer(GameObject gameObject, int layer, Dictionary<int, int> oldLayers = null)
        {
            if (Logx.isActive)
                Logx.assert(null != gameObject, "gameObject is null");

            if (null != oldLayers)
                oldLayers.Add(gameObject.GetInstanceID(), gameObject.layer);

            gameObject.layer = layer;

            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                Transform t = gameObject.transform.GetChild(i);
                setLayer(t.gameObject, layer, oldLayers);
            }
        }

        public static void setLayer(GameObject gameObject, Dictionary<int, int> layers)
        {
            if (Logx.isActive)
                Logx.assert(null != gameObject, "gameObject is null");

            if (layers.TryGetValue(gameObject.GetInstanceID(), out int layer))
                gameObject.layer = layer;

            for (int i = 0; i < gameObject.transform.childCount; ++i)
            {
                Transform t = gameObject.transform.GetChild(i);
                setLayer(t.gameObject, layers);
            }
        }

        public static AnimationClip findClip(Animator animator, string clipName)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != animator, "animator is null");
                Logx.assert(!string.IsNullOrEmpty(clipName), "clipName is null or empty");
            }

            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            AnimationClip[] clips = controller.animationClips;

            AnimationClip clip = (from c in clips
                                  where c.name == clipName
                                  select c).FirstOrDefault();

            return clip;
        }

        public static AnimationClip findClipContainClipName(Animator animator, string clipName)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != animator, "animator is null");
                Logx.assert(!string.IsNullOrEmpty(clipName), "clipName is null or empty");
            }

            RuntimeAnimatorController controller = animator.runtimeAnimatorController;
            AnimationClip[] clips = controller.animationClips;

            AnimationClip clip = (from c in clips
                                  where c.name.Contains(clipName)
                                  select c).FirstOrDefault();

            return clip;
        }

        public static float getLength(Animator animator, string clipName)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != animator, "animator is null");
                Logx.assert(!string.IsNullOrEmpty(clipName), "clipName is null or empty");
            }

            AnimationClip clip = findClip(animator, clipName);
            if (null == clip)
                return 0.0f;

            return clip.length;
        }

        public static float getLengthContainClipName(Animator animator, string clipName)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != animator, "animator is null");
                Logx.assert(!string.IsNullOrEmpty(clipName), "clipName is null or empty");
            }

            AnimationClip clip = findClipContainClipName(animator, clipName);
            if (null == clip)
                return 0.0f;

            return clip.length;
        }

        public static string getScreenShotFullPath(string filename)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");

            return Application.persistentDataPath + "/" + filename;
        }

        public static IEnumerator coCaptureScreenShot(string fullPathFileName, Action<Texture2D> callback)
        {
            if (Logx.isActive)
                Logx.assert(!string.IsNullOrEmpty(fullPathFileName), "fullPathFileName is null or empty");

            yield return new WaitForEndOfFrame();

            Texture2D tex = null;
            try
            {
                tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                tex.Apply();

                byte[] bytes = tex.EncodeToPNG();

                System.IO.File.WriteAllBytes(fullPathFileName, bytes);

                if (Logx.isActive)
                    Logx.trace("Screenshot to: {0}", fullPathFileName);
            }
            catch (Exception e)
            {
                if (Logx.isActive)
                    Logx.exception(e);
            }

            callback?.Invoke(tex);
        }

        public static void shareScreenShot(MonoBehaviour mono, string title, string description, string filename, bool needCapture, Action callback)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != mono, "mono is null");
                Logx.assert(!string.IsNullOrEmpty(title), "title is null or empty");
                Logx.assert(!string.IsNullOrEmpty(description), "description is null or empty");
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");
            }

#if UNITY_ANDROID
            AndroidHelper.shareScreenShot(mono, title, description, filename, needCapture, callback);
#elif UNITY_IOS
            IosHelper.shareScreenShot(mono, title, description, filename, needCapture, callback);
#endif
        }

        public static void saveTextureInProject(Texture2D texture, string filename)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != texture, "texture is null");
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");
            }

            byte[] bytes = texture.EncodeToPNG();
            string fullPath = Application.dataPath + "/../" + filename;
            System.IO.File.WriteAllBytes(fullPath, bytes);
        }

        /// <summary>
        /// https://answers.unity.com/questions/720447/if-game-object-is-in-cameras-field-of-view.html
        /// Renderer의 IsVisible 함수는 모든 카메라에 대해서 조사한다.
        /// </summary>
        public static bool isInCamera(Camera camera, GameObject obj)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != camera, "camera is null");
                Logx.assert(null != obj, "obj is null");
            }

            Vector3 screenPoint = camera.WorldToViewportPoint(obj.transform.position);
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        }

        public static float getHorizontalFov(Camera camera)
        {
            if (Logx.isActive)
                Logx.assert(null != camera, "camera is null");

            return Camera.VerticalToHorizontalFieldOfView(camera.fieldOfView, camera.aspect);
            //return camera.fieldOfView * camera.aspect;
        }

        public static bool isPointerOverGameObject()
        {
            // 이것만으로는 안될 때가 있다.(에디터에서는 되는데 폰에서는 안됨;;)
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            var currentEventData = new PointerEventData(EventSystem.current);
            currentEventData.position = Input.mousePosition;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(currentEventData, results);
            return 0 < results.Count;
        }

        /*
         * https://trialdeveloper.tistory.com/45
         */
        public static bool isPointerOverUI(in Vector2 pointer)
        {
            var pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = pointer;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            var uiLayer = LayerMask.NameToLayer("UI");
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == uiLayer)
                    return true;
            }

            return false;
        }

        public static void addOverlayCameraToStack(Camera overlayCamera, bool isFirst = false)
        {
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            if (isFirst)
                cameraData.cameraStack.Insert(0, overlayCamera);
            else
                cameraData.cameraStack.Add(overlayCamera);
        }

        public static void removeOverlayCameraToStack(Camera overlayCamera)
        {
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Remove(overlayCamera);
        }

        public static void turnOnCameraCullingMask(Camera camera, string layerName)
        {
            camera.cullingMask |= 1 << LayerMask.NameToLayer(layerName);
        }

        public static void turnOffCameraCullingMask(Camera camera, string layerName)
        {
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
        }

        public static void toggleCameraCullingMask(Camera camera, string layerName)
        {
            camera.cullingMask ^= 1 << LayerMask.NameToLayer(layerName);
        }

        public static void destroyAllChilds(Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void destroyTargetChild(Transform transform, int index)
        {
            if (null != transform.GetChild(index).gameObject)
                GameObject.Destroy(transform.GetChild(index).gameObject);
        }

        public static void setSortingOrder(TextMesh text, int order)
        {
            var meshRenderer = text.GetComponent<MeshRenderer>();
            if (null != meshRenderer)
                meshRenderer.sortingOrder = order;
        }

        public static float getParticlePlayTime(ParticleSystem particleSystem)
        {
            var main = particleSystem.main;
            return main.duration + main.startLifetime.constant;
        }

        public static float getParticlePlayTime(List<ParticleSystem> particleSystems)
        {
            var playTime = 0.0f;

            foreach(var particleSystem in particleSystems)
            {
                var p = getParticlePlayTime(particleSystem);
                if (p > playTime)
                    playTime = p;
            }

            return playTime;
        }

        /// <summary>
        /// textMesh에 설정된 현재 텍스트를 기준으로해서 size를 조절한다.
        /// </summary>
        public static void setTextMeshAutoSize(TextMesh textMesh, string text)
        {
            var meshRenderer = textMesh.GetComponent<MeshRenderer>();
            var oldBounds = meshRenderer.bounds;
            textMesh.text = text;
            var newBounds = meshRenderer.bounds;
            var rate = oldBounds.size.x / newBounds.size.x;
            if (1.0f > rate)
            {
                var localScale = textMesh.transform.localScale;
                textMesh.transform.localScale = localScale * rate;
            }
        }

        public static int getLayerMask(List<int> layers)
        {
            int mask = 0;
            foreach (var layer in layers)
            {
                mask |= 1 << layer;
            }

            return mask;
        }

#if UNITY_EDITOR
        public static void destroyImmediateAllChilds(Transform transform)
        {
            while (0 < transform.childCount)
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
#endif

        /*
        /// Derby Life에서 가져옴
        public static void checkLowQuality()
        {
            var volume = UnityEngine.Object.FindObjectOfType<Volume>();
            var cameraData = Camera.main.gameObject.GetComponent<UniversalAdditionalCameraData>();

            bool isActiveVolume = (null == volume) ? false : volume.gameObject.activeSelf;
            bool isRenderShadows = (null == cameraData) ? false : cameraData.renderShadows;
            bool isRenderPostProcessing = (null == cameraData) ? false : cameraData.renderPostProcessing;

            // beauty를 안 쓰는 경우에도 괜찮을까
            // api가 27보다 작은 경우에는 beauty가 적용 안되는 거 같다. 무조건 비활성화,
            if (SystemInfo.graphicsShaderLevel < 30 || AndroidHelper.getApiLevel() <= 27)
            {
                isActiveVolume = false;
                isRenderShadows = false;
                isRenderPostProcessing = false;
            }
            else
            {
                isActiveVolume = 0 < QualitySettings.GetQualityLevel();
            }

            if (null != volume)
            {
                volume.gameObject.SetActive(isActiveVolume);
            }

            if (null != cameraData)
            {
                cameraData.renderShadows = isRenderShadows;
                cameraData.renderPostProcessing = isRenderPostProcessing;
            }
        }
        */
    }
}