using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for SchedulerData
/// </summary>
public class SchedulerData {
	public SchedulerData() {
		//
		// TODO: Add constructor logic here
		//
	}
	public DataTable GetData() {
		DataTable dt = new DataTable("CarScheduling");
		dt.ReadXml(HttpContext.Current.Server.MapPath("carscheduling.xml"));
		return dt;
	}
}
