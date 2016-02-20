using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Season
{
	public static class SeasonExtensions
	{
		// original code by riofly
		// see http://stackoverflow.com/a/12243809/1248177
		public static Season GetSeason(this DateTime date, bool ofSouthernHemisphere)
		{
			var hemisphereConst = ofSouthernHemisphere ? 2 : 0;
			Func<int, Season> handleHemisphere = northern => (Season)((northern + hemisphereConst) % 4);
			var value = date.Month * 100 + date.Day;

			if (value < 321 || value >= 1222) return handleHemisphere(3);
			if (value < 621) return handleHemisphere(0);
			if (value < 923) return handleHemisphere(1);
			return handleHemisphere(2);
		}
	}
}
