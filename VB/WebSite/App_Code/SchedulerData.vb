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

''' <summary>
''' Summary description for SchedulerData
''' </summary>
Public Class SchedulerData
	Public Sub New()
		'
		' TODO: Add constructor logic here
		'
	End Sub
	Public Function GetData() As DataTable
		Dim dt As New DataTable("CarScheduling")
		dt.ReadXml(HttpContext.Current.Server.MapPath("carscheduling.xml"))
		Return dt
	End Function
End Class
