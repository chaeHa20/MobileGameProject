using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityHelper
{
    [Serializable]
    public class LevelExp
    {
        public int level;
        public long exp;

        public LevelExp()
        {
            level = 0;
            exp = 0;
        }

        public LevelExp(int _level, long _exp)
        {
            level = _level;
            exp = _exp;
        }

        /// <param name="getMaxExp">max exp, level</param>
        /// <param name="getNextLevel">next level, level</param>
        public void addExp(long _addExp, Func<int, int> getMaxExp, Func<int, int> getNextLevel)
        {
            exp += _addExp;

            while (true)
            {
                int maxExp = getMaxExp(level);

                if (maxExp <= exp)
                {
                    int nextLevel = getNextLevel(level);
                    if (0 == nextLevel)
                    {
                        exp = maxExp;
                        break;
                    }
                    else
                    {
                        level = nextLevel;
                        exp -= maxExp;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public LevelExp clone()
        {
            return new LevelExp(level, exp);
        }
    }
}