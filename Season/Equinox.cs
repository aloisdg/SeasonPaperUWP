using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Season
{
	public class Equinox
	{
		#region Public Properties
		/// <summary>
		/// For the years -1000 to +1000
		/// </summary>
		public static IDictionary<Season, IList<double>> TableA { get; protected set; }

		/// <summary>
		/// For the years +1000 to +3000
		/// </summary>
		public static IDictionary<Season, IList<double>> TableB { get; protected set; }

		/// <summary>
		/// S = Σ[A Cos(B + (C * T))]
		/// </summary>
		public static IList<double>[] TableC { get; protected set; }
		#endregion

		#region Constructor
		static Equinox()
		{
			TableA = new Dictionary<Season, IList<double>>();
			TableB = new Dictionary<Season, IList<double>>();

			/* For the years -1000 to +1000 */
			TableA[Season.Spring] = new List<double>() { 1721139.29189, 365242.13740, 0.06134, 0.00111, -0.00071 };
			TableA[Season.Summer] = new List<double>() { 1721233.25401, 365241.72562, -0.05323, 0.00907, -0.00025 };
			TableA[Season.Autumn] = new List<double>() { 1721325.70455, 365242.49558, -0.11677, -0.00297, 0.00074 };
			TableA[Season.Winter] = new List<double>() { 1721414.39987, 365242.88257, -0.00769, -0.00933, -0.00006 };

			/* For the years +1000 to +3000 */
			TableB[Season.Spring] = new List<double>() { 2451623.80984, 365242.37404, 0.05169, -0.00411, -0.00057 };
			TableB[Season.Summer] = new List<double>() { 2451716.56767, 365241.62603, 0.00325, 0.00888, -0.00030 };
			TableB[Season.Autumn] = new List<double>() { 2451810.21715, 365242.01767, -0.11575, 0.00337, 0.00078 };
			TableB[Season.Winter] = new List<double>() { 2451900.05952, 365242.74049, -0.06223, -0.00823, 0.00032 };

			/* S = Σ[A Cos(B + (C * T))] */
			TableC = new List<double>[24] {
					new List<double> {485, 324.96,    1934.136},
					new List<double> {203, 337.23,   32964.467},
					new List<double> {199, 342.08,      20.186},
					new List<double> {182,  27.85,  445267.112},
					new List<double> {156,  73.14,   45036.886},
					new List<double> {136, 171.52,   22518.443},
					new List<double> { 77, 222.54,   65928.934},
					new List<double> { 74, 296.72,    3034.906},
					new List<double> { 70, 243.58,    9037.513},
					new List<double> { 58, 119.81,   33718.147},
					new List<double> { 52, 297.17,     150.678},
					new List<double> { 50,  21.02,    2281.226},
					new List<double> { 45, 247.54,   29929.562},
					new List<double> { 44, 325.15,   31555.956},
					new List<double> { 29,  60.93,    4443.417},
					new List<double> { 18, 155.12,   67555.328},
					new List<double> { 17, 288.79,    4562.452},
					new List<double> { 16, 198.04,   62894.029},
					new List<double> { 14, 199.76,   31436.921},
					new List<double> { 12,  95.39,   14577.848},
					new List<double> { 12, 287.11,   31931.756},
					new List<double> { 12, 320.81,   34777.259},
					new List<double> {  9, 227.73,    1222.114},
					new List<double> {  8,  15.45,   16859.074}};


		}
		#endregion

		#region Helper Functions

		protected static double CalculateJde0(IList<double> series, double y)
		{
			var i = 0;	
			return Math.Round(series.Take(5).Sum(x => x * Math.Pow(y, i++)), 5);
		}
		#endregion

		public static double Mean(int year, Season season)
		{
			double y;
			double jde0;
			Debug.WriteLine("Calculating Mean Equinox/Solstice for " + season + ", " + year);
			if (year > 3000 || year < -1000)
			{
				Debug.WriteLine("Year is outside range of accuracy. Expected value between -1000 and +3000; value was " + year, "Equinox");
			}
			if (year >= 1000)
			{
				y = (year - 2000d) / 1000d;
				jde0 = CalculateJde0(TableB[season], y);
			}
			else
			{
				y = year / 1000d;
				jde0 = CalculateJde0(TableA[season], y);
			}
			Debug.WriteLine("Y\t= " + y);
			Debug.WriteLine("JDE0\t= " + jde0);
			return jde0;
		}

		public static double Approximate(int year, Season season)
		{
			double jde0;
			double T;
			double W;
			double Δλ;
			double S;
			double jde;

			jde0 = Mean(year, season);

			Debug.WriteLine("Calculating Approximate Equinox/Solstice for " + season + ", " + year + " from JDE0");

			T = (jde0 - 2451545.0) / 36525;
			T = Math.Round(T, 9);

			W = 35999.373 * T - 2.47;
			Δλ = 1 + 0.0334 * AstroMath.Cos(W) + 0.0007 * AstroMath.Cos(2 * W);
			/* S = Σ[A Cos(B + (C * T))] */
			S = Math.Floor(TableC.Cast<List<double>>()
				.Sum(s => s[0] * AstroMath.Cos(s[1] + s[2] * T)));

			jde = Math.Round(jde0 + ((0.00001 * S) / Δλ), 5);

			/* Debug information */
			Debug.WriteLine("T\t= " + T);
			Debug.WriteLine("W\t= " + W);
			Debug.WriteLine("Δλ\t= " + Δλ);
			Debug.WriteLine("S\t= " + S);
			Debug.WriteLine("JDE\t= " + jde);

			return jde;
		}

		public static double Higher(int year, Season season)
		{
			double jde0;
			jde0 = Mean(year, season);
			return Higher(jde0, season);
		}

		public static double Higher(double jde, Season season)
		{
			throw new NotImplementedException();
			double correction;
			double λ;
			λ = 0;
			/* Formula 27.1 */
			correction = 58 * Math.Sin((int)season * 90 - λ);
			Debug.WriteLine("correction\t= " + correction);
			Debug.WriteLine("Corrected JDE\t= " + (jde + correction));
			if (correction > 0.000005)
				return Higher(jde + correction, season);
			Debug.WriteLine("Final JDE\t= " + jde);
			return jde;
		}

		public static double Exact(int year, Season season)
		{
			double jde0;
			jde0 = Mean(year, season);
			return Higher(jde0, season);
		}

		public static double Exact(double jde, Season season)
		{
			throw new NotImplementedException();
			double correction;
			double λ;
			λ = 0;
			/* Formula 27.1 */
			correction = 58 * Math.Sin((int)season * 90 - λ);
			Debug.WriteLine("correction\t= " + correction);
			Debug.WriteLine("Corrected JDE\t= " + (jde + correction));
			if (correction > 0.000005)
			{ return Higher(jde + correction, season); }
			Debug.WriteLine("Final JDE\t= " + jde);
			return jde;
		}
	}
}
