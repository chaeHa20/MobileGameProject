using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class MsgHelper<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        public void add(Msg msg, float delay = 0.0f)
        {
            if (Logx.isActive)
            {
                Logx.assert(null != msg, "msg is null");
                Logx.trace("<color=yellow>Msg -> {0}</color>", msg.toString());
            }

            if (0.0f < delay)
            {
                StartCoroutine(coLazyMsgAction(msg, delay));
            }
            else
            {
                msg.action();
            }
        }

        IEnumerator coLazyMsgAction(Msg msg, float delay)
        {
            yield return new WaitForSeconds(delay);

            msg.action();
        }
    }
}