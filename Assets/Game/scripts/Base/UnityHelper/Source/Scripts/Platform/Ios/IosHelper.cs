using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace UnityHelper
{
    public class IosHelper
    {
        /// <summary>
        /// An Enum to be used when comparing two versions.
        ///
        /// If:
        ///     A &lt; B    return <see cref="Lesser"/>
        ///     A == B      return <see cref="Equal"/>
        ///     A &gt; B    return <see cref="Greater"/>
        /// </summary>
        public enum VersionComparisonResult
        {
            Lesser = -1,
            Equal = 0,
            Greater = 1
        }

        /// <summary>
        /// Taken from max sdk
        /// </summary>
        /// <param name="versionA"></param>
        /// <param name="versionB"></param>
        /// <returns></returns>
        public static VersionComparisonResult CompareVersions(string versionA, string versionB)
        {
            if (versionA.Equals(versionB)) return VersionComparisonResult.Equal;

            // Check if either of the versions are beta versions. Beta versions could be of format x.y.z-beta or x.y.z-betaX.
            // Split the version string into beta component and the underlying version.
            int piece;
            var isVersionABeta = versionA.Contains("-beta");
            var versionABetaNumber = 0;
            if (isVersionABeta)
            {
                var components = versionA.Split(new[] { "-beta" }, StringSplitOptions.None);
                versionA = components[0];
                versionABetaNumber = int.TryParse(components[1], out piece) ? piece : 0;
            }

            var isVersionBBeta = versionB.Contains("-beta");
            var versionBBetaNumber = 0;
            if (isVersionBBeta)
            {
                var components = versionB.Split(new[] { "-beta" }, StringSplitOptions.None);
                versionB = components[0];
                versionBBetaNumber = int.TryParse(components[1], out piece) ? piece : 0;
            }

            // Now that we have separated the beta component, check if the underlying versions are the same.
            if (versionA.Equals(versionB))
            {
                // The versions are the same, compare the beta components.
                if (isVersionABeta && isVersionBBeta)
                {
                    if (versionABetaNumber < versionBBetaNumber) return VersionComparisonResult.Lesser;

                    if (versionABetaNumber > versionBBetaNumber) return VersionComparisonResult.Greater;
                }
                // Only VersionA is beta, so A is older.
                else if (isVersionABeta)
                {
                    return VersionComparisonResult.Lesser;
                }
                // Only VersionB is beta, A is newer.
                else
                {
                    return VersionComparisonResult.Greater;
                }
            }

            // Compare the non beta component of the version string.
            var versionAComponents = versionA.Split('.').Select(version => int.TryParse(version, out piece) ? piece : 0).ToArray();
            var versionBComponents = versionB.Split('.').Select(version => int.TryParse(version, out piece) ? piece : 0).ToArray();
            var length = Mathf.Max(versionAComponents.Length, versionBComponents.Length);
            for (var i = 0; i < length; i++)
            {
                var aComponent = i < versionAComponents.Length ? versionAComponents[i] : 0;
                var bComponent = i < versionBComponents.Length ? versionBComponents[i] : 0;

                if (aComponent < bComponent) return VersionComparisonResult.Lesser;

                if (aComponent > bComponent) return VersionComparisonResult.Greater;
            }

            return VersionComparisonResult.Equal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="filename"></param>
        /// <param name="needCapture"></param>
        /// <param name="shareFunc">fullPath, title</param>
        /// <param name="callback"></param>
        public static void shareScreenShot(MonoBehaviour mono, string title, string description, string filename, bool needCapture, Action callback)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != mono, "mono is null");
                Logx.assert(!string.IsNullOrEmpty(title), "title is null or empty");
                Logx.assert(!string.IsNullOrEmpty(description), "description is null or empty");
                Logx.assert(!string.IsNullOrEmpty(filename), "filename is null or empty");
            }

            mono.StartCoroutine(coShareScreenShot(mono, title, description, filename, needCapture, callback));
        }

        private static IEnumerator coShareScreenShot(MonoBehaviour mono, string title, string description, string filename, bool needCapture, Action callback)
        {
            yield return new WaitForEndOfFrame();

            string fullPath = GraphicHelper.getScreenShotFullPath(filename);
            if (needCapture)
                yield return mono.StartCoroutine(GraphicHelper.coCaptureScreenShot(fullPath, null));

            if (System.IO.File.Exists(fullPath) == true)
            {
                if (Logx.isActive)
                    Logx.trace("share exist file {0}", fullPath);

                try
                {
                    NativeShare.Share(title, fullPath, "", "", "image/png", true, "");
                }
                catch (Exception e)
                {
                    if (Logx.isActive)
                        Logx.exception(e);
                }
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace("Not exist screen shopt {0}", fullPath);
            }

            if (null != callback)
                callback();

            yield return null;
        }
    }
}