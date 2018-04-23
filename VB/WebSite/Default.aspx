<%@ Page Language="vb" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v8.2" Namespace="DevExpress.Web.ASPxScheduler"
	TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v8.2.Core, Version=8.2.2.0, Culture=neutral, PublicKeyToken=9B171C9FD64DA1D1"
	Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<div>Enter the appointment ID, which has the following pattern: NN or NN_nnn, where NN
			= 1..17, nnn is an arbitrary integer. This is the appointment identifier returned
			by relevant server-side or client-side methods.</div>
		<div>If the appointment ID contains an underscore, it indicates the
			recurrent appointment.</div>
		<div>The number, which follows the underscore specifies the occurrence index.</div>
		&nbsp;&nbsp;

		<asp:Panel ID="Panel1" runat="server" Width="450px" Height="25px">
		<asp:Label ID="Label2" runat="server" Text="Appointment ID:" Width="181px"></asp:Label><asp:TextBox ID="edtClientAppointmentId" runat="server" Width="53px">14_3</asp:TextBox>&nbsp;
			<asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text=" Get Appointment Properties"
				Width="197px" />&nbsp;
		</asp:Panel>
		&nbsp;<table style="width: 350px; position: static; text-align: left;">
			<tr>
				<td style="width: 75px">ID:</td>
				<td style="width: 400px"><asp:Label ID="lblID" runat="server" Text="&nbsp"></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 75px; height: 21px">Type:</td>
				<td style="width: 400px; height: 21px"><asp:Label ID="lblType" runat="server" Text="&nbsp"></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 75px">Subject:</td>
				<td style="width: 400px"><asp:Label ID="lblSubject" runat="server" Text="&nbsp"></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 75px">Start:</td>
				<td style="width: 400px"><asp:Label ID="lblStart" runat="server" Text="&nbsp" ></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 75px">End:</td>
				<td style="width: 400px"><asp:Label ID="lblEnd" runat="server" Text="&nbsp" ></asp:Label></td>
			</tr>
			<tr>
				<td style="width: 75px">Description:</td>
				<td style="width: 400px"><asp:Label ID="lblDescription" runat="server" Text="&nbsp"></asp:Label></td>
			</tr>

		</table>
		&nbsp;
		&nbsp;&nbsp;&nbsp;
		<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
			TypeName="SchedulerData" ></asp:ObjectDataSource>
		&nbsp;&nbsp;
	</div>
	</form>
</body>
</html>
