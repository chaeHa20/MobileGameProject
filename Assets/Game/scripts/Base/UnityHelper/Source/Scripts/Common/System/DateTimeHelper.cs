using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

namespace UnityHelper
{
	public class DateTimeHelper
	{
		public static bool tryParse(string s, out DateTime result)
		{
			return DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
		}

		[Obsolete("Use tryParse instead", true)]
		public static DateTime parse(string s)
        {
			return DateTime.Parse(s, CultureInfo.InvariantCulture);
        }

		public static DateTime toDateTime(string value)
		{
			return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
		}

		public static string toString(ref DateTime dt)
		{
			return dt.ToString(CultureInfo.InvariantCulture);
		}

		public static string toString(DateTime dt)
		{
			return dt.ToString(CultureInfo.InvariantCulture);
		}

		public static string toString(ref DateTime dt, string format)
		{
			return dt.ToString(format, CultureInfo.InvariantCulture);
		}

		public static string toString(DateTime dt, string format)
		{
			return dt.ToString(format, CultureInfo.InvariantCulture);
		}
	}
}