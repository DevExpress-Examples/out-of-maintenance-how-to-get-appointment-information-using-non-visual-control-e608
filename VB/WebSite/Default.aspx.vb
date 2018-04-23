Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.XtraScheduler
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.Web.ASPxScheduler.Internal
Imports System.Collections.Generic
Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private Const OccurenceDivider As Char = "_"c
	Private ASPxScheduler1 As ASPxScheduler

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		Me.ASPxScheduler1 = New ASPxScheduler()
		' Enhance performance by switching navbuttons calculations off.
		Me.ASPxScheduler1.ActiveView.NavigationButtonVisibility = NavigationButtonVisibility.Never
		' Use invisible scheduler control.
		ASPxScheduler1.Visible = False

		Dim appointments As ASPxAppointmentStorage = ASPxScheduler1.Storage.Appointments
		appointments.Mappings.AppointmentId = "ID"
		appointments.Mappings.Description = "Description"
		appointments.Mappings.End = "EndTime"
		appointments.Mappings.RecurrenceInfo = "RecurrenceInfo"
		appointments.Mappings.Start = "StartTime"
		appointments.Mappings.Subject = "Subject"
		appointments.Mappings.Type = "EventType"

		ASPxScheduler1.AppointmentDataSource = ObjectDataSource1
		ASPxScheduler1.DataBind()
	End Sub

	Private Sub ShowAppointment(ByVal appointmentId As String)
		Dim parts() As String = appointmentId.Split(New Char() { OccurenceDivider })
		Dim baseId As String = parts(0)

		Dim storage As ASPxAppointmentStorage = ASPxScheduler1.Storage.Appointments
		Dim apt As Appointment
		If parts.Length = 2 Then
			Dim occurrenceIndex As Integer = Convert.ToInt32(parts(1))
			apt = AppointmentSearchHelper.FindAppointment(storage, baseId, occurrenceIndex)
		Else
			apt = AppointmentSearchHelper.FindAppointment(storage, baseId)
		End If

		UpdateControls(apt, appointmentId)
	End Sub

	Private Sub UpdateControls(ByVal apt As Appointment, ByVal appointmentId As String)
		If apt Is Nothing Then
			lblID.Text = "Appointment not found"
			lblSubject.Text = String.Empty
			lblStart.Text = String.Empty
			lblEnd.Text = String.Empty
			lblType.Text = String.Empty
			lblDescription.Text = String.Empty
			Return
		End If
		' Use DevExpress.Web.ASPxScheduler.Internal.AppointmentIdHelper class methods for versions prior to v2008 vol. 2.
		' For higher versions the DevExpress.Web.ASPxScheduler.ASPxSchedulerStorage.GetAppointmentId method may be used.
		lblID.Text = Convert.ToString(AppointmentIdHelper.GetAppointmentId(apt))
		lblType.Text = apt.Type.ToString()
		lblSubject.Text = apt.Subject
		lblStart.Text = apt.Start.ToShortDateString() & " - " & apt.Start.ToShortTimeString()
		lblEnd.Text = apt.End.ToShortDateString() & " - " & apt.End.ToShortTimeString()
		lblDescription.Text = apt.Description
	End Sub
	Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As EventArgs)
		ShowAppointment(edtClientAppointmentId.Text)
	End Sub



End Class
#Region "AppointmentSearchHelper"
' Implements a class that performs a search and expands recurrent appointment patterns when needed.
' For better performance restrict the number of appointments in a storage 
'using parametrized query or a filter to fill the datasource to which the storage is bound.
Public Class AppointmentSearchHelper
	Public Shared Function FindAppointment(ByVal storage As ASPxAppointmentStorage, ByVal id As String) As Appointment
		Return FindAppointment(storage, id, -1)
	End Function
	Public Shared Function FindAppointment(ByVal storage As ASPxAppointmentStorage, ByVal id As String, ByVal occurenceIndex As Integer) As Appointment
		' Iterate through normal appointments, patterns and exceptions.
		Dim apt As Appointment = FindBaseAppointment(storage.Items, id)
		If apt IsNot Nothing AndAlso occurenceIndex >= 0 Then
			' Calculate occurrence for a given pattern.
			Return GetRecurringAppointment(apt, occurenceIndex)
		End If
		Return apt
	End Function
	Private Shared Function FindBaseAppointment(ByVal appointments As AppointmentCollection, ByVal id As String) As Appointment
		For Each apt As Appointment In appointments
			If CompareAppointmentIds(apt, id) Then
				Return apt
			End If

			If apt.Type = AppointmentType.Pattern Then
				Dim exception As Appointment = FindPatternException(apt, id)
				If exception IsNot Nothing Then
					Return exception
				End If
			End If
		Next apt
		Return Nothing
	End Function
	Private Shared Function CompareAppointmentIds(ByVal apt As Appointment, ByVal id As String) As Boolean
		Return id = AppointmentIdHelper.GetAppointmentId(apt).ToString()
	End Function
	Private Shared Function FindPatternException(ByVal pattern As Appointment, ByVal id As String) As Appointment
		For Each exception As Appointment In pattern.GetExceptions()
			If CompareAppointmentIds(exception, id) Then
				Return exception
			End If
		Next exception
		Return Nothing
	End Function
	Private Shared Function GetRecurringAppointment(ByVal apt As Appointment, ByVal occurenceIndex As Integer) As Appointment
		If apt.Type <> AppointmentType.Pattern Then
			Return Nothing
		End If
		Return apt.GetOccurrence(occurenceIndex)
	End Function
End Class
#End Region


