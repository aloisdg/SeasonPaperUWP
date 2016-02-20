using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Season;

namespace SeasonPaper.ViewModel
{
	public class MainViewModel
	{
		public string Season { get; set; }

		public MainViewModel()
		{
			Season = DateTime.Now.GetSeason(ofSouthernHemisphere: false).ToString();
		}
	}
}
