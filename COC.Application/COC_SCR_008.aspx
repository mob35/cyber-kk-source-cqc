<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_008.aspx.cs" Inherits="COC.Application.COC_SCR_008" %>
<%@ Register src="Shared/TextDateMask.ascx" tagname="TextDateMask" tagprefix="uc2" %>
<%@ Register src="Shared/Calendar.ascx" tagname="Calendar" tagprefix="uc1" %>
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
            width:200px;
        }
         .style4
        {
            font-family: Tahoma;
            font-size: 9pt;
            color: Red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
    <asp:Panel id="pnlMain" runat="server" >
        <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional"> 
            <ContentTemplate>
                  <table cellpadding="3" cellspacing="0" border="0" >
                    <tr>
                        <td style="width:130px; font-weight:bold;" >
                            วันที่เริ่มต้น<span class="style4">*</span>
                        </td>
                        <td class="ColInput" >
                            <uc1:Calendar ID="tdAssignDateFrom" runat="server" />
                        </td>
                        <td style="font-weight:bold;">
                            ถึง <span class="style4">*</span>
                        </td>
                        <td class="ColInput" >
                            <uc1:Calendar ID="tdAssignDateTo" runat="server" />
                        </td>
                        <td >
                            <asp:TextBox ID="txtTeamList" runat="server" Visible="false" ></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" CssClass="Button" Text="ค้นหา" 
                                Width="100px" OnClientClick="DisplayProcessing()" onclick="btnSearch_Click"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnExportExcel" runat="server" CssClass="Button" Width="100px" 
                                Text="Export Excel" onclick="btnExportExcel_Click"   OnClientClick="DisplayProcessing()" />
                        </td>
                    </tr>
                </table>
                <div class="Line"></div>
                <br />
                <table cellpadding="0" cellspacing="0" border="0" width="952px">
                    <tr>
                        <td><asp:Image ID="imgAppInfo" runat="server" ImageUrl="~/Images/DataApp.gif" ImageAlign="Top" Visible="false" />&nbsp;</td>
                    </tr>
                </table>
                <asp:Repeater ID="rptAppInfo" runat="server" ClientIDMode="AutoID" >
                    <HeaderTemplate>
                        <table cellpadding="3" cellspacing="0" border="0" style="border-collapse:collapse;" >
                            <tr>
                                <td align="center" valign="middle" rowspan="2" class="t_rowheadrepeater" style="width:80px;">Team</td>
                                <td align="center" valign="middle" colspan="3" class="t_rowheadrepeater" style="width:210px;">
                                    <asp:Label ID="Label7" runat="server" Text="งานรอจ่าย" ToolTip="งานที่ส่งเข้าทีมแต่ยังไม่ถูกจ่ายไปยังเจ้าหน้าที่"></asp:Label>
                                </td>
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
                                <td style="height:25px;"><asp:Label ID="lblTeam" runat="server" Text='<%# Eval("Team")%>'></asp:Label></td>
                                <td align="center"><asp:LinkButton ID="lbAppInPoolNewJob" runat="server" Text='<%# Eval("AppInPoolNewJob") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppInPoolOldJob" runat="server" Text='<%# Eval("AppInPoolOldJob") %>' Enabled="false"></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppInPoolAllJob" runat="server" Text='<%# Eval("AppInPoolAllJob") %>' Enabled="false"></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lAppbWaitAssignNewJob" runat="server" Text='<%# Eval("AppWaitAssignNewJob") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppWaitAssignOldJob" runat="server" Text='<%# Eval("AppWaitAssignOldJob") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppWaitAssignAllJob" runat="server" Text='<%# Eval("AppWaitAssignAllJob") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppAssignedNewJob" runat="server" Text='<%# Eval("AppAssignedNewJob") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppAssignedOldJob" runat="server" Text='<%# Eval("AppAssignedOldJob") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppAssignedAllJob" runat="server" Text='<%# Eval("AppAssignedAllJob") %>' Enabled="false"  ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbAppTotalAllJob" runat="server" Text='<%# Eval("AppTotalAllJob") %>' Enabled="false" ></asp:LinkButton></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
                <br />
                <asp:Image ID="imgUserMonitoring" runat="server" ImageUrl="~/Images/MonitoringTitle5.png" ImageAlign="Top" Visible="false" />&nbsp;
                <br />
                <asp:Repeater ID="rptUserMonitoring" runat="server" ClientIDMode="AutoID" >
                    <HeaderTemplate>
                        <table cellpadding="3" cellspacing="0" border="0" width="1180px" style="border-collapse:collapse;" >
                            <tr>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:80px;">Team</td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:180px;">ชื่อ-นามสกุล พนักงาน</td>
                                 <td align="center" valign="middle" colspan="3" class="t_rowheadrepeater" style="width:210px; height:18px;">
                                    <asp:Label ID="lblNewJob" runat="server" Text="งานระหว่างดำเนินการ" ToolTip="งานที่เจ้าหน้าที่กำลังดำเนินการอยู่"></asp:Label>
                                </td>
                                <td align="center" valign="middle" colspan="4" class="t_rowheadrepeater" style="width:280px; height:18px;">
                                    <asp:Label ID="lblDoneJob" runat="server" Text="งานเสร็จ" ToolTip="งานที่เจ้าหน้าที่ดำเนินการเรียบร้อย"></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:70px;">
                                    <asp:Label ID="lblTotal" runat="server" Text="รวม" ToolTip="งานระหว่างดำเนินการทั้งหมด + งานเสร็จทั้งหมด" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:80px;">
                                    <asp:Label ID="Label1" runat="server" Text="ระยะเวลาการทำงาน (นาที)" ToolTip="ระยะเวลาการทำงานของเจ้าหน้าที่ คิดเป็นนาทีโดยนับจาก Available Time" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:80px;">
                                    <asp:Label ID="Label2" runat="server" Text="ค่าเฉลี่ยของจำนวนงานเสร็จต่อชั่วโมง" ToolTip="ค่าเฉลี่ยของจำนวนงานเสร็จ (ส่งต่อ) ต่อชั่วโมง" ></asp:Label>
                                </td>
                                <td align="center" valign="middle" rowspan="3" class="t_rowheadrepeater" style="width:80px; border-right:1px solid #7f9db9;">
                                    <asp:Label ID="Label3" runat="server" Text="ค่าเฉลี่ยของจำนวนงานเสร็จทั้งหมดต่อชั่วโมง" ToolTip="ค่าเฉลี่ยของจำนวนงานเสร็จทั้งหมด (ส่งต่อ + ส่งกลับ) ต่อชั่วโมง" ></asp:Label>
                                </td>
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
                                <td style="height:25px;">
                                    <asp:Label ID="lblTeam" runat="server" Text='<%# Eval("Team")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label id="lblStaffFullname" runat="server" Text='<%# Eval("StaffFullname")%>'></asp:Label>
                                </td>
                                <td align="center"><asp:LinkButton ID="lbNewJobNew" runat="server" Text='<%# Eval("AmountNewJobNew") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbNewJobOnHand" runat="server" Text='<%# Eval("AmountNewJobOnHand") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbNewJobAll" runat="server" Text='<%# Eval("AmountNewJobAll") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobForward" runat="server" Text='<%# Eval("AmountDoneJobForward") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobRouteBackCoc" runat="server" Text='<%# Eval("AmountDoneJobRouteBackCoc") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobRouteBackMkt" runat="server" Text='<%# Eval("AmountDoneJobRouteBackMkt") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center"><asp:LinkButton ID="lbDoneJobAll" runat="server" Text='<%# Eval("AmountDoneJobAll") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="center">
                                    <asp:LinkButton ID="lbAllJob" runat="server" Text='<%# Eval("AmountAllJob") %>' Enabled="false" ></asp:LinkButton>
                                </td>
                                <td align="right"><asp:LinkButton ID="lbWorkingHour" runat="server" Text='<%# Eval("WorkingMinDisplay") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="right"><asp:LinkButton ID="lbAvgSuccessPerHour" runat="server" Text='<%# Eval("AvgSuccessPerHour") %>' Enabled="false" ></asp:LinkButton></td>
                                <td align="right"><asp:LinkButton ID="lbAvgTotalPerHour" runat="server" Text='<%# Eval("AvgTotalPerHour") %>' Enabled="false" ></asp:LinkButton></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
