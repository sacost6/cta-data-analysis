//
// One CTA Station
//

namespace program.Models
{

  public class Station
	{
	
		// data members with auto-generated getters and setters:
	  public int StationID { get; set; }
		public string StationName { get; set; }
		public int AvgDailyRidership { get; set; }
        public int NumStops {get; set;}
        public string HandicapAccess {get; set;}
	
		// default constructor:
		public Station()
		{ }
		
		// constructor:
		public Station(int id, string name, int avgDailyRidership,
                       int NumStops, string HandicapAccess)
		{
			this.StationID = id;
			this.StationName = name;
			this.AvgDailyRidership = avgDailyRidership;
            this.NumStops = NumStops;
            this.HandicapAccess = HandicapAccess;
		}
		
	}//class

}//namespace