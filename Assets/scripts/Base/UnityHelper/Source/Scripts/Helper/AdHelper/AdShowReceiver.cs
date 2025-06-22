using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class AdShowReceiver
    {
        private bool m_isShowing;
        private bool m_isClosed;
        private AdResults m_adResults = null;

        public bool isShowing => m_isShowing;
        public bool isClosed => m_isClosed;

        public void beginShow(eAdFormat adFormat)
        {
            m_isShowing = true;
            m_isClosed = false;
            m_adResults = new AdResults(adFormat);
        }

        public void setResult(eAdResult result)
        {
            m_adResults.add(result);

            if (eAdResult.Closed == result ||
                eAdResult.Exhausted == result ||
                eAdResult.InternetNotRechable == result)
            {
                m_isClosed = true;
            }
        }

        public AdResults endShow()
        {
            m_isShowing = false;

            return m_adResults;
        }
    }
}