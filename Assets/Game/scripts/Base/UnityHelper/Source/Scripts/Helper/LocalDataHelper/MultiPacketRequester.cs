using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    public class MultiPacketRequester
    {
        public class Buffer
        {
            public Req_LocalData req;
            public Action<Res_LocalData> callback;
        }

        private List<Buffer> m_buffers = new List<Buffer>();

        public int bufferCount => m_buffers.Count;

        public MultiPacketRequester add(Req_LocalData req, Action<Res_LocalData> callback = null)
        {
            var buffer = new Buffer
            {
                req = req,
                callback = callback,
            };
            m_buffers.Add(buffer);

            return this;
        }

        public Buffer getBuffer(int bufferIndex)
        {
            return m_buffers[bufferIndex];
        }
    }
}