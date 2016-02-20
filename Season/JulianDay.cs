using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Season
{
	/// <summary>
	/// The JDN the number of whole and fractional days since Noon, 1st January -4712 UTC.
	/// </summary>
	public class JulianDay
	{
		#region Public Static Methods
		public static JulianDay Now()
		{
			return new JulianDay(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second, DateTime.UtcNow.Millisecond);
		}
		#endregion

		#region Public Properties
		public int Year { get; protected set; }
		public int Month { get; protected set; }
		public int Day { get; protected set; }
		public int Hour { get; protected set; }
		public int Minute { get; protected set; }
		public int Second { get; protected set; }
		public int Millisecond { get; protected set; }

		public double JulianDayNumber { get; protected set; }

		public DayOfWeek DayOfWeek
		{
			get
			{
				int jd = Int(JulianDayNumber + 1.5);
				return (DayOfWeek)(jd % 7);
			}
		}

		public int DayOfYear
		{
			get
			{
				int k;
				k = IsLeapYear ? 1 : 2;

				return Int(275 * Month / 9) - k * Int((Month + 9) / 12) + Day - 30;
			}
		}

		/// <summary>
		/// On the Julian Calendar a year divisible by 4 is a leap year
		/// On the Gregorian Calendar a year divisible by 4, except centuries (divisible by 100) not divisible by 400, is a leap year.
		/// </summary>
		public bool IsLeapYear
		{
			get
			{
				bool isCentury = (Year % 100 == 0);
				if (isCentury && Calendar == Calendar.Gregorian)
				{ return Year % 400 == 0; }
				return Year % 4 == 0;
			}

		}

		/// <summary>
		/// The Gregorian calendar reform moved changed the calendar from the Julian to Gregorian standard.
		/// This means that the day immediately after 4th October 1582 is 15th October 1582.
		/// </summary>
		public Calendar Calendar
		{
			get
			{
				if (JulianDayNumber != 0)
				{
					if (JulianDayNumber < 2299161)
					{ return Calendar.Julian; }
					else
					{ return Calendar.Gregorian; }
				}
				else
				{
					if (Year > 1582)
					{ return Calendar.Gregorian; }
					else if (Year < 1582)
					{ return Calendar.Julian; }
					else if ((Year == 1582) && (Month < 10))
					{ return Calendar.Julian; }
					else if ((Year == 1582) && (Month > 10))
					{ return Calendar.Gregorian; }
					else if ((Year == 1582) && (Month == 10) && (Day < 5))
					{ return Calendar.Julian; }
					else if ((Year == 1582) && (Month > 10) && (Day > 14))
					{ return Calendar.Gregorian; }
					else
					{ throw new IndexOutOfRangeException("The dates October 5th - 14th 1582 are not valid"); }
				}
			}
		}
		#endregion

		public JulianDay AddDays(double days)
		{ return new JulianDay(JulianDayNumber + days); }

		public JulianDay AddMinutes(double minutes)
		{ return AddDays((minutes / 60) / 24); }

		public JulianDay AddSeconds(double seconds)
		{ return AddMinutes(seconds / 60 / 60); }

		private int Int(double input)
		{ return Convert.ToInt32(Math.Floor(input)); }

		#region Constructors
		public JulianDay()
		{
			Year = -4712;
			Month = 1;
			Day = 1;
			Hour = 12;
			JulianDayNumber = 0;
		}

		public JulianDay(int year, int month, int day)
		    : this(year, month, day, 0)
		{ }

		public JulianDay(int year, int month, int day, int hour)
		    : this(year, month, day, hour, 0)
		{ }

		public JulianDay(int year, int month, int day, int hour, int minute)
		    : this(year, month, day, hour, minute, 0)
		{ }

		public JulianDay(int year, int month, int day, int hour, int minute, int second)
		    : this(year, month, day, hour, minute, second, 0)
		{ }

		public JulianDay(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{

			this.Year = year;
			this.Month = month;
			this.Day = day;
			this.Hour = hour;
			this.Minute = minute;
			this.Second = second;
			this.Millisecond = millisecond;
			CalculateFromDate();
		}

		public JulianDay(double jdn)
		{
			JulianDayNumber = jdn;
			CalculateDate();
		}
		#endregion

		private void CalculateFromDate()
		{
			JulianDayNumber = 0;
			int y = Year;
			int m = Month;
			double d = Day;
			d += (Hour / 24d);
			d += (Minute / 60d) / 24d;
			d += (Second / 60d) / 60d / 24d;
			d += (Millisecond / 1000d) / 60d / 60d / 24d;
			if (m <= 2)
			{
				y = y - 1;
				m = m + 12;
			}
			int b = 0;
			if (Calendar == Calendar.Gregorian)
			{
				int a = Int(y / 100);
				b = 2 - a + Int(a / 4);
			}

			double jd = Int(365.25 * (y + 4716)) + Int(30.6001 * (m + 1)) + d + b - 1524.5;
			JulianDayNumber = jd;
		}

		private void CalculateDate()
		{
			double jd = JulianDayNumber + 0.5;
			int z = (int)Math.Truncate(jd);
			double f = jd - z;
			int A;
			if (z < 2299161)
			{ A = z; }
			else
			{
				int a = Int((z - 1867216.25) / 36524.25);
				A = z + 1 + a - Int(a / 4);
			}
			int b = A + 1524;
			int c = Int((b - 122.1) / 365.25);
			int d = Int(365.25 * c);
			int e = Int((b - d) / 30.6001);

			Day = b - d - Int(30.6001 * e);

			if (e < 14)
			{ Month = e - 1; }
			else
			{ Month = e - 13; }

			if (Month > 2)
			{ Year = c - 4716; }
			else
			{ Year = c - 4715; }

			Hour = Int(f * 24);
			Minute = Int(((f * 24) - Hour) * 60);
			Second = Int(((((f * 24) - Hour) * 60) - Minute) * 60);
			Millisecond = Int(((((((f * 24) - Hour) * 60) - Minute) * 60) - Second) * 1000);
		}

		#region Mathematical Operators
		public static JulianDay operator +(JulianDay jde1, JulianDay jde2)
		{ return new JulianDay(jde1.JulianDayNumber + jde2.JulianDayNumber); }

		//public static JulianDay operator +(JulianDay JDE1, double Days)
		//{ return new JulianDay(JDE1.JulianDayNumber + Days); }

		public static double operator +(JulianDay jde1, double days)
		{ return jde1.JulianDayNumber + days; }

		public static JulianDay operator -(JulianDay jde1, JulianDay jde2)
		{ return new JulianDay(jde1.JulianDayNumber - jde2.JulianDayNumber); }

		//public static JulianDay operator -(JulianDay JDE1, double Days)
		//{ return new JulianDay(JDE1.JulianDayNumber - Days); }

		public static double operator -(JulianDay jde1, double days)
		{ return jde1.JulianDayNumber - days; }

		public static JulianDay operator *(JulianDay jde1, JulianDay jde2)
		{ return new JulianDay(jde1.JulianDayNumber * jde2.JulianDayNumber); }

		public static JulianDay operator *(JulianDay jde1, double days)
		{ return new JulianDay(jde1.JulianDayNumber * days); }

		public static JulianDay operator /(JulianDay jde1, JulianDay jde2)
		{ return new JulianDay(jde1.JulianDayNumber / jde2.JulianDayNumber); }

		public static JulianDay operator /(JulianDay jde1, double days)
		{ return new JulianDay(jde1.JulianDayNumber / days); }
		#endregion

		public static explicit operator DateTime(JulianDay julianDay)
		{
			return new DateTime(
				julianDay.Year, julianDay.Month, julianDay.Day,
				julianDay.Hour, julianDay.Minute, julianDay.Second,
				julianDay.Millisecond);
		}

		public override string ToString()
		{
			int year = Year;
			string era = "CE";
			if (year <= 0)
			{
				era = "BCE";
				year = Math.Abs(year) + 1;
			}
			return
				$"{DayOfWeek}, {Day} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)}, {year} {era}";
		}
	}

	public enum Calendar
	{
		Julian,
		Gregorian
	}
}
