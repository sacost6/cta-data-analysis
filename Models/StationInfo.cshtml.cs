using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace program.Pages  
{  
    public class StationInfoModel : PageModel  
    {  
				public List<Models.Station> StationList { get; set; }
				public string Input { get; set; }
				public Exception EX { get; set; }
  
        public void OnGet(string input)  
        {  
				  StationList = new List<Models.Station>();
					
					// make input available to web page:
					Input = input;
					
					// clear exception:
					EX = null;
					
					try
					{
						//
						// Do we have an input argument?  If not, there's nothing to do:
						//
						if (input == null)
						{
							//
							// there's no page argument, perhaps user surfed to the page directly?  
							// In this case, nothing to do.
							//
						}
						else  
						{
							// 
							// Lookup movie(s) based on input, which could be id or a partial name:
							// 
							string sql;

						  // lookup station(s) by partial name match:
							input = input.Replace("'", "''");

							sql = string.Format(@"
	SELECT Stations.StationID, Stations.Name, AVG(DailyTotal) AS AvgDailyRidership,COUNT(DISTINCT StopID) AS NumStops, ADA AS HandicapAccess
	FROM Stations
	LEFT JOIN Riderships ON Stations.StationID = Riderships.StationID 
    FULL OUTER JOIN Stops ON Stations.StationID = Stops.StationID
	WHERE Stations.Name LIKE '%{0}%'
	GROUP BY Stations.StationID, Stations.Name, ADA
	ORDER BY Stations.Name ASC;
	", input);

							DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

							foreach (DataRow row in ds.Tables[0].Rows)
							{
                               
								Models.Station s = new Models.Station();
                                
								s.StationID = Convert.ToInt32(row["StationID"]);
								s.StationName = Convert.ToString(row["Name"]);
                            
								// avg could be null if there is no ridership data:
								if (row["AvgDailyRidership"] == System.DBNull.Value)
									s.AvgDailyRidership = 0;
								else
									s.AvgDailyRidership = Convert.ToInt32(row["AvgDailyRidership"]);
      
                                s.NumStops = Convert.ToInt32(row["NumStops"]);
                                
                                if (row["HandicapAccess"] == System.DBNull.Value)
									s.HandicapAccess = "All";
								else if(Convert.ToInt32(row["HandicapAccess"]) == 0)
                                    s.HandicapAccess = "None";
                                else if(Convert.ToInt32(row["HandicapAccess"]) == 1)
                                    s.HandicapAccess = "All";

                           
								StationList.Add(s);
							}
						}//else
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
					  // nothing at the moment
				  }
				}
			
    }//class  
}//namespace