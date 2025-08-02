using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class SocialUser
    {
        private string m_id = null;
        private string m_name = null;
        private Sprite m_profile = null;

        public string id { get { return m_id; } }
        public string name { get { return m_name; } }
        public Sprite profile { get { return m_profile; } }

        public SocialUser(string _id, string _name, Texture2D texture)
        {
            m_id = _id;
            m_name = _name;
            setProfile(texture);
        }

        public void setProfile(Texture2D texture)
        {
            m_profile = GraphicHelper.texture2DToSprite(texture);
        }
    }
}