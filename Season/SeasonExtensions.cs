using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;

namespace Season
{


	public static class SeasonExtensions
	{
		// original code by riofly see http://stackoverflow.com/a/12243809/1248177
		// equinox entry on wiki https://en.wikipedia.org/wiki/Equinox
		public static Season GetSeason(this DateTime date, bool ofSouthernHemisphere)
		{
			var hemisphereConst = ofSouthernHemisphere ? 2 : 0;
			Func<int, Season> handleHemisphere = northern =>
				(Season) ((northern + hemisphereConst) % 4);
			Func<Season, DateTime> getDay = season =>
				(DateTime) new JulianDay(Equinox.Approximate(date.Year, season));

			var winterSolstice = getDay(Season.Winter);
			var springEquinox = getDay(Season.Spring);
			var summerSolstice = getDay(Season.Summer);
			var autumnEquinox = getDay(Season.Autumn);

			if (date < springEquinox || date >= winterSolstice) return handleHemisphere(3);
			if (date < summerSolstice) return handleHemisphere(0);
			if (date < autumnEquinox) return handleHemisphere(1);
			return handleHemisphere(2);
		}
	}
}
