<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_005.aspx.cs" Inherits="COC.Application.COC_SCR_005" %>
<%@ Register src="Shared/TextDateMask.ascx" tagname="TextDateMask" tagprefix="uc1" %>
<%@ Register src="Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td style="vertical-align:top; width:100px; font-weight:bold;">
                        ประเภทรายงาน
                    </td>
                    <td>
                        <asp:RadioButton ID="rbAppReport" runat="server" Text="ข้อมูลแสดงรายละเอียดการทำงานของพนักงาน"  GroupName="report" Checked="true" /><br />
                        <asp:RadioButton ID="rbAccessReport" runat="server" Text="ข้อมูลการเข้าใช้ระบบของพนักงาน"  GroupName="report" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:10px;"></td>
                </tr>
                <tr>
                    <td style="font-weight:bold;">
                        วันที่เริ่มต้น
                    </td>
                    <td>
                        <uc2:Calendar ID="tdmStartDate" runat="server" />&nbsp;&nbsp;
                        <b>ถึง</b>&nbsp;<uc2:Calendar ID="tdmEndDate" runat="server" />
                    
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height:5px;"></td>
                </tr>
                <tr>
                    <td style="font-weight:bold;">
                    </td>
                    <td>
                        <asp:Button ID="btnGenReport" runat="server" Text="ออกรายงาน" Width="90px" 
                            onclick="btnGenReport_Click"  OnClientClick="DisplayProcessing()" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="Line"></div>
</asp:Content>
