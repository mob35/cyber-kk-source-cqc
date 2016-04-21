<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calendar.ascx.cs" Inherits="COC.Application.Shared.Calendar" %>

<style type="text/css"> .ajax__calendar { z-index:500000 !important; position:relative; }</style>

<script language="javascript" type="text/javascript">

</script>
<asp:TextBox runat="server" ID="txtDate" Enabled="false" Width="100px" BackColor="White" CssClass="Textbox" />
<asp:ImageButton runat="Server" ID="imgCalendar" ImageUrl="~/Images/calendar.gif" ToolTip="Click to show calendar" ImageAlign="Absmiddle"/>
<act:CalendarExtender ID="calendar" runat="server" TargetControlID="txtDate"  Format="dd/MM/yyyy" PopupButtonID="imgCalendar" ClearTime="False" />
<asp:ImageButton runat="Server" ID="imbClear" ImageUrl="~/Images/bDelete.gif" ToolTip="Clear calendar" ImageAlign="Absmiddle"  />