using System;
using System.Globalization;
using UnityEngine;

namespace UnityHelper
{
    public class LanguageHelper
    {
        public static eLanguage language = eLanguage.Kr;

        public static eLanguage getDeviceLanguage()
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                return eLanguage.Kr;
            }
            else if (Application.systemLanguage == SystemLanguage.English)
            {
                return eLanguage.En;
            }
            else if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                return eLanguage.Jp;
            }
            else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                return eLanguage.Cn;
            }
            else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                return eLanguage.Tw;
            }
            else if (Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                return eLanguage.Vn;
            }
            else if (Application.systemLanguage == SystemLanguage.Thai)
            {
                return eLanguage.Th;
            }
            else if (Application.systemLanguage == SystemLanguage.Russian)
            {
                return eLanguage.Ru;
            }
            else if (Application.systemLanguage == SystemLanguage.Indonesian)
            {
                return eLanguage.Id;
            }
            else if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                return eLanguage.Es;
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                return eLanguage.Pt;
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                return eLanguage.De;
            }
            else if (Application.systemLanguage == SystemLanguage.Turkish)
            {
                return eLanguage.Tr;
            }
            else if (Application.systemLanguage == SystemLanguage.French)
            {
                return eLanguage.Fr;
            }
            else if (Application.systemLanguage == SystemLanguage.Italian)
            {
                return eLanguage.It;
            }

            return eLanguage.En;
        }

        public static string getCurrencySymbolValue(string price)
        {
            if (!decimal.TryParse(price, out decimal value))
            {
                return String.Format("{0:n0}", price);
            }

            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                return string.Format(new CultureInfo("ko-KR"), "{0:c0}", value); // ₩
            }
            else if (Application.systemLanguage == SystemLanguage.English)
            {
                return string.Format(new CultureInfo("en-US"), "{0:c0}", value); // $
            }
            else if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                return string.Format(new CultureInfo("ja-JP"), "{0:c0}", value); // ¥
            }
            else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                return string.Format(new CultureInfo("zh-CN"), "{0:c0}", value); // ¥ (CNY)
            }
            else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                return string.Format(new CultureInfo("zh-TW"), "{0:c0}", value); // NT$
            }
            else if (Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                return string.Format(new CultureInfo("vi-VN"), "{0:c0}", value); // ₫
            }
            else if (Application.systemLanguage == SystemLanguage.Thai)
            {
                return string.Format(new CultureInfo("th-TH"), "{0:c0}", value); // ฿
            }
            else if (Application.systemLanguage == SystemLanguage.Russian)
            {
                return string.Format(new CultureInfo("ru-RU"), "{0:c0}", value); // ₽
            }
            else if (Application.systemLanguage == SystemLanguage.Indonesian)
            {
                return string.Format(new CultureInfo("id-ID"), "{0:c0}", value); // Rp
            }
            else if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                return string.Format(new CultureInfo("es-ES"), "{0:c0}", value); // €
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                return string.Format(new CultureInfo("pt-BR"), "{0:c0}", value); // R$
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                return string.Format(new CultureInfo("de-DE"), "{0:c0}", value); // €
            }
            else if (Application.systemLanguage == SystemLanguage.Turkish)
            {
                return string.Format(new CultureInfo("tr-TR"), "{0:c0}", value); // ₺
            }
            else if (Application.systemLanguage == SystemLanguage.French)
            {
                return string.Format(new CultureInfo("fr-FR"), "{0:c0}", value); // €
            }
            else if (Application.systemLanguage == SystemLanguage.Italian)
            {
                return string.Format(new CultureInfo("it-IT"), "{0:c0}", value); // €
            }

            return String.Format("{0:n0}", price);
        }
        public static string getCurrencySymbolValue2(string price)
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                return "₩" + String.Format("{0:n0}", price); // ₩
            }
            else if (Application.systemLanguage == SystemLanguage.English)
            {
                return "$" + String.Format("{0:n0}", price); // $
            }
            else if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                return "¥" + String.Format("{0:n0}", price); // ¥
            }
            else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
            {
                return "¥(CNY)" + String.Format("{0:n0}", price); // ¥ (CNY)
            }
            else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                return "NT$" + String.Format("{0:n0}", price); // NT$
            }
            else if (Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                return "₫" + String.Format("{0:n0}", price); // ₫
            }
            else if (Application.systemLanguage == SystemLanguage.Thai)
            {
                return "฿" + String.Format("{0:n0}", price); // ฿
            }
            else if (Application.systemLanguage == SystemLanguage.Russian)
            {
                return "₽" + String.Format("{0:n0}", price); // ₽
            }
            else if (Application.systemLanguage == SystemLanguage.Indonesian)
            {
                return "Rp" + String.Format("{0:n0}", price); // Rp
            }
            else if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                return "€" + String.Format("{0:n0}", price); // €
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                return "R$" + String.Format("{0:n0}", price); // R$
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                return "€" + String.Format("{0:n0}", price); // €
            }
            else if (Application.systemLanguage == SystemLanguage.Turkish)
            {
                return "₺" + String.Format("{0:n0}", price); // ₺
            }
            else if (Application.systemLanguage == SystemLanguage.French)
            {
                return "€" + String.Format("{0:n0}", price); // €
            }
            else if (Application.systemLanguage == SystemLanguage.Italian)
            {
                return "€" + String.Format("{0:n0}", price);// €
            }

            return String.Format("{0:n0}", price);
        }
    }
}