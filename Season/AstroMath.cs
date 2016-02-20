using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Season
{
	public static class AstroMath
	{
		/// <summary>
		/// Convert between degrees and radians.
		/// </summary>
		public const double DegRad = Math.PI / 180.0;

		/// <summary>
		/// Convert between hours and radians.
		/// </summary>
		public const double HourRad = 15.0 * DegRad;

		public static double Sin(double angle)
		{
			return Math.Sin(angle * DegRad);
		}

		public static double Cos(double angle)
		{
			return Math.Cos(angle * DegRad);
		}

		public static double Mod(double a, double b)
		{
			var result = a % b;
			return result + result < 0 ? b : 0;
		}

		public static int Mod(int a, int b)
		{
			return (int)Mod((double)a, b);
		}

		public static int ToInt(this double input)
		{
			return Convert.ToInt32(Math.Floor(input));
		}
	}
}
