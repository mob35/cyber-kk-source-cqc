<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_004.aspx.cs" Inherits="COC.Application.COC_SCR_004" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc1" %>
<%@ Register src="Shared/TextDateMask.ascx" tagname="TextDateMask" tagprefix="uc2" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:160px;
        }
        .ColInfo2
        {
            font-weight:bold;
            width:100px;
            text-align:right;
        }
        .ColInput
        {
            width:170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Panel id="pnlMain" runat="server" >
        <asp:UpdatePanel ID="upAppInfo" runat="server" UpdateMode="Conditional"> 
            <ContentTemplate>
                <table cellpadding="3" cellspacing="0" width="980px" >
                    <tr align="right">
                        <td >
                            <asp:Label id="Label2" runat="server" text="เวลาที่เรียกดูข้อมูล" Font-Bold ="true"></asp:Label>
                            <asp:Label id="Label1" runat="server" text="" ForeColor="Red" Font-Bold ="true"></asp:Label>
                        </td>
                    </tr>
                 </table>
                <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Forecast.gif" ImageAlign="Top" />&nbsp;
                <asp:Button id="btnForecast" runat="server" CssClass="Button" Width="80px" Text="เรียกดู" OnClick="btnForecast_Click" OnClientClick="DisplayProcessing()" />
                <asp:TextBox ID="txtTeamList" runat="server" Visible="false"></asp:TextBox>
                <br /><br />
                <div class="Line"></div>
                <br />
                <table cellpadding="0" cellspacing="0" border="0" width="952px">
                    <tr>
                        <td><asp:Image ID="Image7" runat="server" ImageUrl="~/Images/DataApp.gif" ImageAlign="Top" />&nbsp;</td>
                        <td align="right"><asp:LinkButton ID="lbRefreshAppInfo" runat="server" Text="Refresh" OnClick="lbRefreshAppInfo_Click" OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                    </tr>
                </table>
                <asp:Repeater ID="rptAppInfo" runat="server" ClientIDMode="AutoID" 
                    onitemdatabound="rptAppInfo_ItemDataBound" >
                    <HeaderTemplate>
                        <table cellpadding="3" cellspacing="0" border="0" style="border-collapse:collapse;" >
                            <tr>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:80px;">Team</td>
                                <td align="center" valign="middle" colspan="3" class="t_rowheadrepeater" style="width:210px;">
                                    <asp:Label ID="Label7" runat="server" Text="งานรอจ่าย" ToolTip="งานที่ส่งเข้าทีมแต่ยังไม่ถูกจ่ายไปยังเจ้าหน้าที่"></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:80px;">&nbsp;</td>
                                <td align="center" valign="middle" colspan="3" class="t_rowheadrepeater" style="width:180px;">
                                     <asp:Label ID="Label8" runat="server" Text="งานยังไม่กดรับ" ToolTip="งานที่เจ้าหน้าที่ยังไม่ได้กดรับงาน"></asp:Label>
                                </td>
                                <td align="center" valign="middle" colspan="3" class="t_rowheadrepeater" style="width:180px;">
                                    <asp:Label ID="Label9" runat="server" Text="งานกดรับแล้ว" ToolTip="งานที่เจ้าหน้าที่กดรับงานเรียบร้อยแล้ว"></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:80px;">
                                    <asp:Label ID="lblAppTotalAllJob" runat="server" Text="รวม" ToolTip="งานทั้งหมด (งานรอจ่าย) + งานทั้งหมด (งานยังไม่กดรับ) + งานทั้งหมด (งานกดรับแล้ว)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppInPoolNewJob" runat="server" Text="งานที่ส่งเข้าทีมครั้งแรก" ToolTip="งานที่ส่งเข้าทีมเป็นครั้งแรก"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppInPoolOldJob" runat="server" Text="งานที่เคยส่งเข้าทีม" ToolTip="งานที่เคยถูกส่งเข้าทีมมาแล้ว"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppInPoolAllJob" runat="server" Text="งานทั้งหมด" ToolTip="งานที่ส่งเข้าทีมครั้งแรก + งานที่เคยส่งเข้าทีม"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppbWaitAssignNewJob" runat="server" Text="งานที่ส่งเข้าทีมครั้งแรก" ToolTip="งานที่ส่งเข้าทีมเป็นครั้งแรก"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppWaitAssignOldJob" runat="server" Text="งานที่เคยส่งเข้าทีม" ToolTip="งานที่เคยถูกส่งเข้าทีมมาแล้ว"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppWaitAssignAllJob" runat="server" Text="งานทั้งหมด" ToolTip="งานที่ส่งเข้าทีมครั้งแรก + งานที่เคยส่งเข้าทีม"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppAssignedNewJob" runat="server" Text="งานที่ส่งเข้าทีมครั้งแรก" ToolTip="งานที่ส่งเข้าทีมเป็นครั้งแรก"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppAssignedOldJob" runat="server" Text="งานที่เคยส่งเข้าทีม" ToolTip="งานที่เคยถูกส่งเข้าทีมมาแล้ว"></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblAppAssignedAllJob" runat="server" Text="งานทั้งหมด" ToolTip="งานที่ส่งเข้าทีมครั้งแรก + งานที่เคยส่งเข้าทีม"></asp:Label>
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr class="t_rowrepeater">
                                <td><asp:Label ID="lblTeam" runat="server" Text='<%# Eval("Team")%>'></asp:Label></td>
                                <td align="center"><asp:LinkButton ID="lbAppInPoolNewJob" runat="server" Text='<%# Eval("AppInPoolNewJob") %>' OnClick="lbAppInPoolNewJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppInPoolOldJob" runat="server" Text='<%# Eval("AppInPoolOldJob") %>' OnClick="lbAppInPoolOldJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppInPoolAllJob" runat="server" Text='<%# Eval("AppInPoolAllJob") %>' OnClick="lbAppInPoolAllJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:Button ID="btnAppTranfer" runat="server" CssClass="Button" Text ="โอนงาน" OnClick="btnAppTranfer_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" /></td>
                                <td align="center"><asp:LinkButton ID="lAppbWaitAssignNewJob" runat="server" Text='<%# Eval("AppWaitAssignNewJob") %>' OnClick="lbAppWaitAssignNewJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppWaitAssignOldJob" runat="server" Text='<%# Eval("AppWaitAssignOldJob") %>' OnClick="lbAppWaitAssignOldJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppWaitAssignAllJob" runat="server" Text='<%# Eval("AppWaitAssignAllJob") %>' OnClick="lbAppWaitAssignAllJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppAssignedNewJob" runat="server" Text='<%# Eval("AppAssignedNewJob") %>' OnClick="lbAppAssignedNewJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppAssignedOldJob" runat="server" Text='<%# Eval("AppAssignedOldJob") %>' OnClick="lbAppAssignedOldJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppAssignedAllJob" runat="server" Text='<%# Eval("AppAssignedAllJob") %>' OnClick="lbAppAssignedAllJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppTotalAllJob" runat="server" Text='<%# Eval("AppTotalAllJob") %>' OnClick="lbAppTotalAllJob_Click" CommandArgument='<%# Eval("Team") %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="upUserMonitoring" runat="server" UpdateMode="Conditional"> 
            <ContentTemplate>
                <table cellpadding="3" cellspacing="0" width="980px" >
                    <tr align="right">
                        <td >
                            <asp:Label id="Label3" runat="server" text="เวลาที่เรียกดูข้อมูล" Font-Bold ="true"></asp:Label>
                            <asp:Label id="Label4" runat="server" text="" ForeColor="Red" Font-Bold ="true"></asp:Label>
                        </td>
                    </tr>
                 </table>
                 <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/MonitoringTitle5.png" ImageAlign="Top" />&nbsp;
                <br />
                <table cellpadding="3" cellspacing="0" border="0" >
                    <tr>
                        <td style="font-weight:bold;">
                            สถานะการทำงาน
                        </td>
                        <td class="ColInput">
                            <asp:DropDownList ID="cmbStaffStatus" runat="server"  Width="203px" CssClass="Dropdownlist">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                <asp:ListItem Text="Unavailable" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Available" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtStaffStatusSelected" runat="server" Visible="false" ></asp:TextBox>
                        </td>
                        <td align="right" style="width:60px; font-weight:bold;">
                            Team
                        </td>
                        <td class="ColInput">
                            <asp:DropDownList ID="cmbTeam" runat="server"  Width="203px" CssClass="Dropdownlist">
                            </asp:DropDownList>
                        </td>
                        <td >
                            <asp:Button ID="btnSearchUserMonitoring" runat="server" CssClass="Button" Text="ค้นหา" 
                                Width="100px" OnClientClick="DisplayProcessing()" onclick="btnSearchUserMonitoring_Click"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="height:1px;"></td>
                    </tr>
                </table><br />
                <asp:Repeater ID="rptUserMonitoring" runat="server" ClientIDMode="AutoID" 
                    onitemdatabound="rptUserMonitoring_ItemDataBound" >
                    <HeaderTemplate>
                        <table cellpadding="3" cellspacing="0" border="0" width="1180px" style="border-collapse:collapse;" >
                            <tr>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:80px;">Team</td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:180px;">ชื่อ-นามสกุล พนักงาน</td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:120px;">สถานะการทำงาน</td>
                                <td align="center" valign="middle" colspan="3" class="t_rowheadrepeater" style="width:210px; height:18px;">
                                    <asp:Label ID="lblNewJob" runat="server" Text="งานระหว่างดำเนินการ" ToolTip="งานที่เจ้าหน้าที่กำลังดำเนินการอยู่"></asp:Label>
                                </td>
                                <td align="center" valign="middle" colspan="4" class="t_rowheadrepeater" style="width:280px; height:18px;">
                                    <asp:Label ID="lblDoneJob" runat="server" Text="งานเสร็จ" ToolTip="งานที่เจ้าหน้าที่ดำเนินการเรียบร้อย"></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblTotal" runat="server" Text="รวม" ToolTip="งานระหว่างดำเนินการทั้งหมด + งานเสร็จทั้งหมด" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:80px;"></td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:110px; border-right:1px solid #7f9db9;"></td>
                            </tr>
                            <tr>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblNewJobNew" runat="server" Text="งานรอรับ" ToolTip="งานที่เจ้าหน้าที่ยังไม่ได้กดรับงาน" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblNewJobOnHand" runat="server" Text="งานที่<br/>รับแล้ว" ToolTip="งานที่เจ้าหน้าที่กดรับงานเรียบร้อยแล้ว แต่ยังดำเนินการไม่เสร็จ" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblNewJobAll" runat="server" Text="งานระหว่างดำเนินการทั้งหมด" ToolTip="งานที่เจ้าหน้าที่กำลังดำเนินการอยู่ทั้งหมด (งานรอรับ + งานที่รับแล้ว)" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblDoneJobForward" runat="server" Text="ส่งต่อ" ToolTip="งานที่เจ้าหน้าที่ดำเนินการส่งต่อแบบไปข้างหน้า (Forward)" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" colspan="2" class="t_rowheadrepeater" style="width:140px; height:15px;">
                                    <asp:Label ID="Label10" runat="server" Text="ส่งกลับ" ToolTip="งานที่เจ้าหน้าที่ดำเนินการส่งกลับ (Backward)" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblDoneJobAll" runat="server" Text="งานเสร็จ<br />ทั้งหมด" ToolTip="งานที่เจ้าหน้าที่ดำเนินการเรียบร้อยแล้วทั้งหมด (ส่งต่อ + ส่งกลับ)" ></asp:Label>
                                </td>
                            </tr>
                            <tr >
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px; height:18px;">
                                    <asp:Label ID="lblDoneJobRouteBackCoc" runat="server" Text="COC" ToolTip="งานที่ถูกส่งกลับมายัง COC (Routeback)" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" class="t_rowheadrepeater" style="width:70px; height:18px;">
                                    <asp:Label ID="lblDoneJobRouteBackMkt" runat="server" Text="MKT" ToolTip="งานที่ถูกส่งกลับมายัง MKT (Routeback)" ></asp:Label>
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr class="t_rowrepeater">
                                <td>
                                    <asp:Label ID="lblTeam" runat="server" Text='<%# Eval("Team")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label id="lblStaffFullname" runat="server" Text='<%# Eval("StaffFullname")%>'></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                    <asp:Image ID="imgAvailable" runat="server" ImageUrl="~/Images/enable.gif" ImageAlign="AbsMiddle" Visible='<%# Eval("Active") != null ? (Eval("Active").ToString().Trim() == "1" ? true : false) : false %>' />
                                    <asp:Image ID="imgNotAvailable" runat="server" ImageUrl="~/Images/disable.gif" ImageAlign="AbsMiddle" Visible='<%# Eval("Active") != null ? (Eval("Active").ToString().Trim() == "1" ? false : true) : false %>' />
                                    &nbsp;
                                    <asp:Label id="lblStatusDesc" runat="server" Text='<%# Eval("Active") != null ? (Eval("Active").ToString().Trim() == "1" ? "Available" : "Unavailable") : "" %>'></asp:Label>
                                </td>
                                <td align="center"><asp:LinkButton ID="lbNewJobNew" runat="server" Text='<%# Eval("AmountNewJobNew") %>' OnClick="lbNewJobNew_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbNewJobOnHand" runat="server" Text='<%# Eval("AmountNewJobOnHand") %>' OnClick="lbNewJobOnHand_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbNewJobAll" runat="server" Text='<%# Eval("AmountNewJobAll") %>' OnClick="lbNewJobAll_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobForward" runat="server" Text='<%# Eval("AmountDoneJobForward") %>' OnClick="lbDoneJobForward_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobRouteBackCoc" runat="server" Text='<%# Eval("AmountDoneJobRouteBackCoc") %>' OnClick="lbDoneJobRouteBackCoc_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobRouteBackMkt" runat="server" Text='<%# Eval("AmountDoneJobRouteBackMkt") %>' OnClick="lbDoneJobRouteBackMkt_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobAll" runat="server" Text='<%# Eval("AmountDoneJobAll") %>' OnClick="lbDoneJobAll_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton></td>
                                <td align="center">
                                    <asp:Label ID="lblAllJob" runat="server" Text='<%# Eval("AmountAllJob") %>'></asp:Label>
                                    <asp:LinkButton ID="lbAllJob" runat="server" Visible="false" Text='<%# Eval("AmountAllJob") %>' OnClick="lbAllJob_Click" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' ></asp:LinkButton>
                                </td>
                                <td align="center"><asp:Button ID="btnUserMonitoringTranferJob" runat="server" CssClass="Button" Text ="โอนงาน" OnClick="btnUserMonitoringTranferJob_Click" Visible='<%# Eval("Active").ToString() == "1" ? false : true %>' OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' /></td>
                                <td align="center">
                                    <asp:Button ID="btnSetStatus" runat="server" Text='<%# Eval("Active") != null ? (Eval("Active").ToString().Trim() == "1" ? "Unavailable" : "Available") : "" %>' OnClick="btnSetStatus_Click" CssClass="Button" Width="100px" OnClientClick="DisplayProcessing()" CommandArgument='<%# Container.ItemIndex %>' />
                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("EmpCode") %>' Visible="false" ></asp:Label>
                                </td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />

        <asp:UpdatePanel ID="upPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="btnPopup" Width="0px" CssClass="Hidden"/>
	        <asp:Panel runat="server" ID="pnPopup" style="display:none" CssClass="modalPopupMonitoring" >
                <br />
                &nbsp;&nbsp;<asp:Image ID="imgDetail" runat="server" ImageUrl="~/Images/LeadData.png" />
                <br />
                <asp:Panel ID="pnTranserJob" runat="server">
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td style="width:20px"></td>
                            <td style="width:60px; font-weight:bold;">
                                โอนให้
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbStaffTranserJob" runat="server" Width="250px" CssClass="Dropdownlist">
                                </asp:DropDownList>
                            </td>
                            <td style="width:20px"></td>
                            <td>
                                <asp:Button ID="btnPopupTranserJob" runat="server" Width="100px" CssClass="Button" Text="โอนงาน" OnClick="btnPopupTranserJob_Click" OnClientClick="if (confirm('ต้องการโอนงานใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" ></asp:Button>
                                <asp:TextBox ID="txtPopupCocTeam" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtPopupLastOwner" runat="server" Visible="false"></asp:TextBox>
                                <asp:CheckBox ID="cbDoRefreshAppInfo" runat="server" Text="DoRefreshAppInfo" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <uc3:GridviewPageController ID="pcPopup" runat="server" OnPageChange="PageSearchChangePopup" Width="945px" />
		        <table cellpadding="2" cellspacing="0" border="0">
                    <tr>
                        <td ></td>
                        <td>
                            <asp:Panel ID="pnl1"  runat="server" CssClass="modalPopupMonitoring2" ScrollBars="Auto" >
                                <asp:GridView ID="gvPopup" runat="server" AutoGenerateColumns="False" 
                                    GridLines="Horizontal" BorderWidth="0px" 
                                    EnableModelValidation="True" Width="3020px" 
                                    EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>"  >
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbTranserJob" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                                <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SLA">
                                                <ItemTemplate>
                                                    <asp:image ID="imgSla" runat="server" ImageUrl="~/Images/invalid.gif" Visible='<%# Eval("CocCounting") != null ? (Convert.ToInt32(Eval("CocCounting")) > 0 ? true : false) : false %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top"  />
                                                <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="No" HeaderText="No"  >
                                                <HeaderStyle Width="50px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="50px" HorizontalAlign="Center"  VerticalAlign="Top"/>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Ticket Id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTicketId" runat="server" Text='<%# Eval("TicketId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ประเภท<br />งานเสร็จ" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoneJobType" runat="server" Text='<%# Eval("DoneJobType") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="วันที่ Action" Visible="false">
                                                <ItemTemplate>
                                                    <%# Eval("ActionDate") != null ? Convert.ToDateTime(Eval("ActionDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("ActionDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("ActionDate")).ToString("HH:mm:ss") : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="110px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AppNo" HeaderText="เลขที่ใบ<br/>คำเสนอขอซื้อ" HtmlEncode="false"  >
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RankingId" HeaderText="Priority"   >
                                                <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RankingName" HeaderText="กลุ่ม Priority"  >
                                                <HeaderStyle Width="300px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="300px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="ประเภทงาน" >
                                                <ItemTemplate>
                                                    <%# Eval("JobType") != null ? Eval("JobType").ToString() : "" %>
                                                </ItemTemplate>
                                                <HeaderStyle Width="130px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ประเภท<br/>การจ่ายงาน" >
                                                <ItemTemplate>
                                                    <%# Eval("CocAssignTypeDesc") != null ? Eval("CocAssignTypeDesc").ToString() : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MarketingOwner" HeaderText="รหัสเจ้าหน้าที่<br/>การตลาด" HtmlEncode="false"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MarketingOwnerName" HeaderText="ชื่อ-นามสกุล<br/>เจ้าหน้าที่การตลาด" HtmlEncode="false"  >
                                                <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CocStatusDesc" HeaderText="สถานะ COC"  >
                                                <HeaderStyle Width="160px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="160px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DealerCode" HeaderText="รหัส Dealer"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DealerName" HeaderText="ชื่อ Dealer"  >
                                                <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CampaignName" HeaderText="Campaign"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Channel" HeaderText="ช่องทางการขาย"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CarType" HeaderText="ประเภทรถ"  >
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OwnerBranchName" HeaderText="สาขา"  >
                                                <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ClientFirstname" HeaderText="ชื่อลูกค้า"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ClientLastname" HeaderText="นามสกุลลูกค้า"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TelNo1" HeaderText="มือถือ"  >
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CardNo" HeaderText="หมายเลขบัตร"  >
                                                <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SlmOwnerName" HeaderText="SLM Owner"  >
                                                <HeaderStyle Width="160px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="160px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SlmDelegateName" HeaderText="SLM Delegate"  >
                                                <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LastOwnerName" HeaderText="Last Owner"  >
                                                <HeaderStyle Width="160px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="160px" HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="วันที่สร้าง Ticket">
                                                <ItemTemplate>
                                                    <%# Eval("LeadCreatedDate") != null ? Convert.ToDateTime(Eval("LeadCreatedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("LeadCreatedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("LeadCreatedDate")).ToString("HH:mm:ss") : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="110px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="วันที่เข้าส่วนกลาง">
                                                <ItemTemplate>
                                                    <%# Eval("CocFirstAssignDate") != null ? Convert.ToDateTime(Eval("CocFirstAssignDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CocFirstAssignDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("CocFirstAssignDate")).ToString("HH:mm:ss") : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="110px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="วันที่เข้า Team COC">
                                                <ItemTemplate>
                                                    <%# Eval("CocFirstTeamAssign") != null ? Convert.ToDateTime(Eval("CocFirstTeamAssign")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CocFirstTeamAssign")).Year.ToString() + " " + Convert.ToDateTime(Eval("CocFirstTeamAssign")).ToString("HH:mm:ss") : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="110px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="COC วันที่ได้รับมอบหมายล่าสุด">
                                                <ItemTemplate>
                                                    <%# Eval("CocAssignedDate") != null ? Convert.ToDateTime(Eval("CocAssignedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CocAssignedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("CocAssignedDate")).ToString("HH:mm:ss") : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="110px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AppAging" HeaderText="App Aging (วัน)"  >
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LeadAging" HeaderText="Lead Aging (วัน)"  >
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                                <asp:BoundField DataField="TeamAging" HeaderText="Team Aging (วัน)"  >
                                                <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:BoundField>
                                        </Columns> 
                                        <HeaderStyle CssClass="t_rowhead" />
                                        <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                                    </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height:15px;">
                        </td>
                    </tr>
                    <tr style="height:35px;">
                        <td class="ColIndent"></td>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnPopupTransferJobClose" runat="server" Text="ปิดหน้าจอ" Width="100px" onclick="btnPopupTransferJobClose_Click" OnClientClick="DisplayProcessing()" />&nbsp;
                        </td>
                    </tr>
                </table>
	        </asp:Panel>
	        <act:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="btnPopup" PopupControlID="pnPopup" BackgroundCssClass="modalBackground" DropShadow="True">
	        </act:ModalPopupExtender>
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upPopupForecast" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="btnPopupForecast" Width="0px" CssClass="Hidden"/>
            <asp:Panel runat="server" ID="pnPopupForecast" style="display:none" CssClass="modalPopupMonitoringForecast" >
                <br />
                &nbsp;&nbsp;<asp:Image ID="imgTitle1" runat="server" ImageUrl="~/Images/MonitoringTitle1.png" ImageAlign="Top" />&nbsp;
                <div style="float:right;">
                    <asp:Label id="Label5" runat="server" text="เวลาที่เรียกดูข้อมูล" Font-Bold ="true"></asp:Label>
                    <asp:Label id="Label6" runat="server" text="" ForeColor="Red" Font-Bold ="true"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <table cellpadding="3" cellspacing="0" >
                    <tr>
                        <td class="ColInfo">
                           &nbsp;&nbsp;&nbsp;&nbsp;เวลาเลิกงาน
                        </td>
                        <td >
                            <asp:TextBox ID="txtWorkEndHour" runat="server" CssClass="TextboxC" Width="30px" MaxLength="2" ></asp:TextBox>
                            :
                            <asp:TextBox ID="txtWorkEndMin" runat="server" CssClass="TextboxC" Width="30px" MaxLength="2" ></asp:TextBox>
                            <asp:Label id="labl1" runat="server" text="น."></asp:Label>&nbsp;
                            <asp:Button ID="btnSaveWorkEndTime" runat="server" Text="บันทึก" OnClientClick="DisplayProcessing()" 
                                CssClass="Button" Width="100px" onclick="btnSaveWorkEndTime_Click" />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2" style="height:5px"></td>
                    </tr>
                 </table><br />
                &nbsp;&nbsp;<asp:Image ID="Image8" runat="server" ImageUrl="~/Images/Forecast.gif" ImageAlign="Top" />&nbsp;
                <table>
                    <tr>
                        <td style="width:5px;"></td>
                        <td>
                            <asp:Panel ID="pnPopupForecastInside"  runat="server" CssClass="modalPopupMonitoringForecast2" ScrollBars="Auto" >
                            <asp:GridView ID="gvForecastReport" runat="server" AutoGenerateColumns="False" GridLines="Horizontal" BorderWidth="0px" 
                                EnableModelValidation="True" Width="810px" EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Team">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTeam" runat="server" Text='<%# Eval("Team") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="90px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField  >
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHeadAmountOfJob" runat="server" Text="จำนวนงานในแต่ละทีม" ToolTip="งานยังไม่กดรับทั้งหมด + งานกดรับแล้วทั้งหมด"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("AmountOfJob") != null ? Convert.ToInt32(Eval("AmountOfJob")).ToString("#,##0") : "0" %>
                                            </ItemTemplate>
                                            <ItemStyle Width="150px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SLA ต่อ App">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSla" runat="server" Text='<%# Eval("Sla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="จำนวนคนที่ Available">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountOfAvailableStaff" runat="server" Text='<%# Eval("AmountOfAvailableStaff") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHeadAmountOfPredictionSuccess" runat="server" Text="จำนวนงานที่คาดว่าจะเสร็จทันภายในเวลาเลิกงาน (งาน)" ToolTip="[(เวลาเลิกงาน - เวลาปัจจุบันที่ดูรายงาน)(นาที) / SLA(นาที)] * จำนวนคนที่ Available"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("AmountOfPredictionSuccess") != null ? Convert.ToInt32(Eval("AmountOfPredictionSuccess")).ToString("#,##0") : "N/A"%>
                                            </ItemTemplate>
                                            <ItemStyle Width="120px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHeadAmountOfJobExceedEndTime" runat="server" Text="จำนวนงานที่ต้องทำเกินเวลาเลิกงาน (งาน)" ToolTip="จำนวนงานในแต่ละทีม - จำนวนงานที่คาดว่าจะเสร็จ"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("AmountOfJobExceedEndTime") != null ? Convert.ToInt32(Eval("AmountOfJobExceedEndTime")).ToString("#,##0") : "N/A"%>
                                            </ItemTemplate>
                                            <ItemStyle Width="120px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHeadAmountOfAdditionalTime" runat="server" Text="เวลาที่ต้องใช้<br/>เพิ่มเติม(นาที)" ToolTip="(จำนวนงานในแต่ละทีมของทีมตัวเองและทีมก่อนหน้า - จำนวนงานที่คาดว่าจะเสร็จ) * SLA"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("AmountOfAdditionalTime") != null ? Convert.ToInt32(Eval("AmountOfAdditionalTime")).ToString("#,##0") : "N/A" %>
                                            </ItemTemplate>
                                            <ItemStyle Width="120px" HorizontalAlign="Center"  />
                                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="เวลาที่ต้องใช้เพิ่มเติมต่อคน(นาที)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdditionalTimePerPerson" runat="server" Text='<%# Eval("AdditionalTimePerPerson") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="150px" CssClass="Hidden"  />
                                            <HeaderStyle Width="150px" CssClass="Hidden" />
                                            <ControlStyle CssClass="Hidden" />
                                        </asp:TemplateField>
                                    </Columns> 
                                    <HeaderStyle CssClass="t_rowhead" />
                                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                                </asp:GridView>
                        </asp:Panel>
                       </td>
                    </tr>
                    <tr style="height:35px;">
                        <td></td>
                        <td align="right">
                            <asp:Button ID="btnClosePopupForecast" runat="server" Text="ปิดหน้าจอ" Width="100px" onclick="btnClosePopupForecast_Click"  />&nbsp;
                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
            <act:ModalPopupExtender ID="mpePopupForecast" runat="server" TargetControlID="btnPopupForecast" PopupControlID="pnPopupForecast" BackgroundCssClass="modalBackground" DropShadow="True">
	        </act:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
