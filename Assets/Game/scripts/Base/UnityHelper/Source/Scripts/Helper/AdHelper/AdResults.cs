using System.Collections.Generic;

namespace UnityHelper
{
    public class AdResults
    {
        private HashSet<eAdResult> m_results = new HashSet<eAdResult>();
        private eAdFormat m_adFormat = eAdFormat.Banner;

        private bool isEmpty => 0 == m_results.Count;

        public AdResults(eAdFormat adFormat)
        {
            m_adFormat = adFormat;
        }

        public void add(eAdResult result)
        {
            m_results.Add(result);
        }

        public bool isSuccess()
        {
            if (isEmpty)
                return false;

            foreach (var result in m_results)
            {
                if (eAdResult.Closed == result)
                    return true;
            }

            return false;
        }

        public bool isContain(eAdResult result)
        {
            return m_results.Contains(result);
        }
    }
}