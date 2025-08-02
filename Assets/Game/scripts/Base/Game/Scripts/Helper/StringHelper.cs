using UnityEngine;

namespace UnityHelper
{
    public partial class StringHelper
    {
        public static string get(int id)
        {
            return GameTableHelper.instance.getString(id);
        }

        public static string get(int id, params object[] args)
        {
            return GameTableHelper.instance.getString(id, args);
        }

        public static string get(string code)
        {
            return GameTableHelper.instance.getString(code);
        }

        public static string get(string code, params object[] args)
        {
            return GameTableHelper.instance.getString(code, args);
        }

        public static string get(eGrade grade)
        {
            string code = null;
            switch (grade)
            {
                case eGrade.Normal: code = "grade_normal"; break;
                case eGrade.Rare: code = "grade_rare"; break;
                case eGrade.Epic: code = "grade_epic"; break;
                case eGrade.Legendary: code = "grade_legend"; break;
                case eGrade.Ultimate: code = "grade_ultimate"; break;
                case eGrade.Mythic: code = "grade_myth"; break;
            }

            return get(code);
        }

        public static string get(eCurrency currencyType)
        {
            string code = null;
            switch (currencyType)
            {
                case eCurrency.Gem: code = "gem"; break;
                case eCurrency.Gold: code = "gold"; break;
            }

            return get(code);
        }


        public static string get(eAbilityOwner owner)
        {
            string code = null;

            switch (owner)
            {
                case eAbilityOwner.Player: code = "player"; break;
                default: return null;
            }

            return get(code);
        }



        public static string get(eLanguage language)
        {
            string code = null;
            switch (language)
            {
                case eLanguage.Kr: code = "language_ko"; break;
                case eLanguage.En: code = "language_en"; break;
                case eLanguage.Jp: code = "language_jp"; break;
                case eLanguage.Cn: code = "language_cn"; break;
                case eLanguage.Tw: code = "language_tw"; break;
                case eLanguage.Vn: code = "language_vn"; break;
                case eLanguage.Th: code = "language_th"; break;
                case eLanguage.Ru: code = "language_ru"; break;
                case eLanguage.Id: code = "language_id"; break;
                case eLanguage.Es: code = "language_es"; break;
                case eLanguage.Pt: code = "language_pt"; break;
                case eLanguage.De: code = "language_de"; break;
                case eLanguage.Tr: code = "language_tr"; break;
                case eLanguage.Hi: code = "language_hi"; break;
                case eLanguage.Fr: code = "language_fr"; break;
                case eLanguage.My: code = "language_my"; break;
                case eLanguage.It: code = "language_it"; break;
            }

            return get(code);
        }

        public static string toStr(long value, bool isDigit)
        {
            return (isDigit) ? toString(value, "N0") : toString(value);
        }

        public static string toSignStr(int value)
        {
            return string.Format("{0:+#;-#;0}", value);
        }

        public static string toUnsignStr(int value)
        {
            return string.Format("{0:+#;#;0}", value);
        }

        public static string timeToStr(int h, int m, int s)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", h, m, s);
        }

        public static string toX<T>(T value)
        {
            return string.Format("X {0}", value);
        }

        public static string toRatio<T>(T curValue, T maxValue)
        {
            return string.Format("{0:0}/{1:0}", curValue, maxValue);
        }

        public static string toCollectionRatio(int curValue, int maxValue)
        {
            return string.Format("[{0:0}/{1:0}]", curValue, maxValue);
        }
        public static string toQuestRatio(int curValue, int maxValue)
        {
            return string.Format("({0:0}/{1:0})", curValue, maxValue);
        }

        public static string toX(iMinMax values)
        {
            if (values.isSingle())
                return toX(values.min);
            else
                return string.Format("X {0}~{1}", values.min, values.max);
        }

        public static string toPercent(int value)
        {
            return string.Format("{0}%", value);
        }
        public static string toPercent(float value)
        {
            return string.Format("{0:0.00}%", value);
        }
        public static string toPercent(string value)
        {
            return string.Format("{0}%", value);
        }

        public static string toLevel(int level)
        {
            return get("level_1", level);
        }

        public static string toLevelRatio(int curLevel, int maxLevel)
        {
            return string.Format("Lv. <color=#ffaa4e><b>{0}</b></color>/{1}", curLevel, maxLevel);
        }

        public static string toMultiple(double multiple)
        {
            return string.Format("{0}x", multiple);
        }

        public static string toNotEnoughCurrency(eCurrency currencyType)
        {
            var currencyName = get(currencyType);
            var stringCode = (eCurrency.Gem == currencyType) ? "not_enough_currency02" : "not_enough_currency";
            return get(stringCode, currencyName);
        }

        public static string toNotEnoughCurrencyDesc(eCurrency currencyType)
        {
            var str = toNotEnoughCurrency(currencyType);
            str += "\n";
            str += get("move_to_shop");
            return str;
        }

        public static string toNotEnoughTicketDesc(int nameId)
        {
            var str = get("not_enough_currency02", get(nameId));
            str += "\n";
            str += get("move_to_shop");
            return str;
        }

        public static string getTypeName<T>(int id)
        {
            return typeof(T) + "_" + id.ToString();
        }

        public static string getMsCoolTime(int d, int h, int m, int s)
        {
            m += TimeHelper.dayHourToMinute(d, h);

            if (0 < m)
                return get("cooltime_ms", m, s);
            else
                return get("cooltime_s", s);
        }

        public static string getQuestRemain(int d, int h, int m, int s)
        {
            m += TimeHelper.secondToMinute(s);
            h += TimeHelper.dayToHour(d);
            if (0 < TimeHelper.minuteToHour(m))
            {
                int addHour = TimeHelper.minuteToHour(m);
                h += addHour;
                m = m - addHour * 60;
            }

            if (0 < h + m)
                return get("quest_daily_time", h, m);
            else
                return get("cooltime_s", s);
        }


        public static string getHMSCoolTime(int d, int h, int m, int s)
        {
            h += d * 24;

            return string.Format("{00:00}:{01:00}:{02:00}", h, m, s);
        }

        public static string getVersion()
        {
            return string.Format("Ver.{0}", Application.version);
        }

        public static string getItemBoostCoolTime(int d, int h, int m, int s)
        {
            if (0 < d)
                h += d * 24;

            if (0 < h)
            {
                return get("offline_time", h, m);
            }
            else if (0 < m)
            {
                return get("cooltime_m", m);
            }
            else if (0 < s)
            {
                return get("cooltime_s", s);
            }
            else
            {
                return "";
            }
        }
    }
}