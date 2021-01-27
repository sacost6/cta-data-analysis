using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace program.Pages  
{  
    public class LineInfoModel : PageModel  
    {  
				public List<Models.Line> StationList { get; set; }
				public string Input { get; set; }
				public Exception EX { get; set; }
  
        public void OnGet(string input)  
        {  
				  StationList = new List<Models.Line>();
					
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
	SELECT Lines.LineID, Lines.Color, StationOrder.Position, Stations.Name AS stopName,
            Stations.StationID AS StationID, StationOrder.Position,
             (SELECT COUNT(StopID) FROM Stops WHERE StationID = Stations.StationID) AS NumStops
	FROM Lines
    
    LEFT JOIN StopDetails ON Lines.LineID = StopDetails.LineID
    
    LEFT JOIN Stops ON StopDetails.StopID = Stops.StopID
    
    LEFT JOIN Stations ON Stations.StationID = Stops.StationID
    
    LEFT JOIN StationOrder ON StationOrder.StationID = Stations.StationID
    
	WHERE Lines.Color LIKE '%{0}%' AND Lines.LineID = StationOrder.LineID
    
	GROUP BY Lines.LineID, Lines.Color, Stations.Name,
            StationOrder.Position, Stations.StationID
    ORDER BY StationOrder.Position ASC;
    
	", input);

							DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

							foreach (DataRow row in ds.Tables[0].Rows)
							{
                               Models.Line l = new Models.Line();
                               l.LineColor = Convert.ToString(row["Color"]);
                               l.LineID = Convert.ToInt32(row["LineID"]);
                               l.NumStops = Convert.ToInt32(row["NumStops"]);
                               l.stopName = Convert.ToString(row["stopName"]);
                               l.StationID = Convert.ToInt32(row["StationID"]);
                               StationList.Add(l);
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