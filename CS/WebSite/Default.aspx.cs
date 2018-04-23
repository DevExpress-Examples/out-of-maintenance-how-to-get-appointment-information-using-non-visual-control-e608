using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.ASPxScheduler.Internal;
using System.Collections.Generic;
public partial class _Default : System.Web.UI.Page 
{
	const char OccurenceDivider = '_';
	ASPxScheduler ASPxScheduler1;

	protected void Page_Load(object sender, EventArgs e) {
		this.ASPxScheduler1 = new ASPxScheduler();
		// Enhance performance by switching navbuttons calculations off.
		this.ASPxScheduler1.ActiveView.NavigationButtonVisibility = NavigationButtonVisibility.Never;
		// Use invisible scheduler control.
        ASPxScheduler1.Visible = false;

		ASPxAppointmentStorage appointments = ASPxScheduler1.Storage.Appointments;
		appointments.Mappings.AppointmentId = "ID";
		appointments.Mappings.Description = "Description";
		appointments.Mappings.End = "EndTime";
		appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
		appointments.Mappings.Start = "StartTime";
		appointments.Mappings.Subject = "Subject";
		appointments.Mappings.Type = "EventType";
		
		ASPxScheduler1.AppointmentDataSource = ObjectDataSource1;
		ASPxScheduler1.DataBind();
	}

	private void ShowAppointment(string appointmentId) {
		string[] parts = appointmentId.Split(new char[] { OccurenceDivider });
		string baseId = parts[0];

		ASPxAppointmentStorage storage = ASPxScheduler1.Storage.Appointments;
		Appointment apt;
		if (parts.Length == 2) {
			int occurrenceIndex = Convert.ToInt32(parts[1]);
			apt = AppointmentSearchHelper.FindAppointment(storage, baseId, occurrenceIndex);
		} else
			apt = AppointmentSearchHelper.FindAppointment(storage, baseId);
		
		UpdateControls(apt, appointmentId);
	}
	
	private void UpdateControls(Appointment apt, string appointmentId) {
        if (apt == null) {
			lblID.Text = "Appointment not found";
			lblSubject.Text = string.Empty;
			lblStart.Text = string.Empty;
			lblEnd.Text = string.Empty;
			lblType.Text = string.Empty;
			lblDescription.Text = string.Empty;
			return;
		}
        // Use DevExpress.Web.ASPxScheduler.Internal.AppointmentIdHelper class methods for versions prior to v2008 vol. 2.
        // For higher versions the DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage.GetAppointmentId method may be used.
        lblID.Text = Convert.ToString(AppointmentIdHelper.GetAppointmentId(apt));
		lblType.Text = apt.Type.ToString();
		lblSubject.Text = apt.Subject;
		lblStart.Text = apt.Start.ToShortDateString() + " - " + apt.Start.ToShortTimeString();
		lblEnd.Text = apt.End.ToShortDateString() + " - " + apt.End.ToShortTimeString();
		lblDescription.Text = apt.Description;
	}
	protected void Button1_Click1(object sender, EventArgs e) {
		ShowAppointment(edtClientAppointmentId.Text);
	}


    
}
#region AppointmentSearchHelper
// Implements a class that performs a search and expands recurrent appointment patterns when needed.
// For better performance restrict the number of appointments in a storage 
//using parametrized query or a filter to fill the datasource to which the storage is bound.
public class AppointmentSearchHelper {
	public static Appointment FindAppointment(ASPxAppointmentStorage storage, string id) {
		return FindAppointment(storage, id, -1);
	}
	public static Appointment FindAppointment(ASPxAppointmentStorage storage, string id, int occurenceIndex) {
        // Iterate through normal appointments, patterns and exceptions.
        Appointment apt = FindBaseAppointment(storage.Items, id);
		if (apt != null && occurenceIndex >= 0) {
            // Calculate occurrence for a given pattern.
            return GetRecurringAppointment(apt, occurenceIndex);
		}
		return apt;
	}
	static Appointment FindBaseAppointment(AppointmentCollection appointments, string id) {
        foreach (Appointment apt in appointments) {
			if (CompareAppointmentIds(apt, id))
				return apt;

			if (apt.Type == AppointmentType.Pattern) {
				Appointment exception = FindPatternException(apt, id);
				if (exception != null)
					return exception;
			}
		}
		return null;
	}
	static bool CompareAppointmentIds(Appointment apt, string id) {
		return id == AppointmentIdHelper.GetAppointmentId(apt).ToString();
	}
	static Appointment FindPatternException(Appointment pattern, string id) {
		foreach (Appointment exception in pattern.GetExceptions()) {
			if (CompareAppointmentIds(exception, id))
				return exception;
		}
		return null;
	}
	static Appointment GetRecurringAppointment(Appointment apt, int occurenceIndex) {
        if (apt.Type != AppointmentType.Pattern)
			return null;
		return apt.GetOccurrence(occurenceIndex);
	}
}
#endregion


