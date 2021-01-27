using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace program.Pages  
{  
    public class TopTenStationsModel : PageModel  
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

							sql = string.Format(@"
	SELECT TOP 10 Stations.Name, Stations.StationID, SUM(DailyTotal) AS AvgDailyRidership
	FROM Lines
    INNER JOIN StopDetails ON Lines.LineID = StopDetails.LineID
    LEFT JOIN Stops ON StopDetails.StopID = Stops.StopID
    LEFT JOIN Stations ON Stations.StationID = Stops.StationID
    LEFT JOIN Riderships ON Stations.StationID = Riderships.StationID
    INNER JOIN StationOrder ON StationOrder.StationID = Stations.StationID
	WHERE Lines.Color LIKE '%{0}%' AND StationOrder.LineID = Lines.LineID
	GROUP BY Stations.Name, Stations.StationID
    ORDER BY SUM(DailyTotal) DESC;
    
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