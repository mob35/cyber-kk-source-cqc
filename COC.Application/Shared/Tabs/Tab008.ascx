<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab008.ascx.cs" Inherits="COC.Application.Shared.Tabs.Tab008" %>
<%@ Register src="../GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc1" %>

<style type="text/css">
    .ColIndent
    {
        width:35px;
    }
    .ColInfo1
    {
        font-weight:bold;
        width:180px;
    }
    .ColInfo2
    {
        font-weight:bold;
        width:200px;
    }
    .ColInput
    {
        width:250px;
    }
</style>
<div style="font-family:Tahoma; font-size:13px;">
    <script language="javascript" type="text/javascript">
    </script>
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" style="width:1170px;">
                        <asp:TextBox ID="txtTelNo1" runat="server" Visible="false" Width="200px" ></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="txtCitizenId" runat="server" Visible="false" ></asp:TextBox>
            <uc1:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange" Width="1170px" />
            <asp:Panel ID="pnPhoneCallHistory" runat="server" Width="1170px" ScrollBars="Auto" >
                <asp:GridView ID="gvPhoneCallHistoty" runat="server" AutoGenerateColumns="False" DataKeyNames="TicketId" 
                GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True" EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>" >
                    <Columns>
                    <asp:TemplateField HeaderText="วันที่บันทึกข้อมูล">
                        <ItemTemplate>
                            <%# Eval("CreatedDate") != null ? Convert.ToDateTime(Eval("CreatedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CreatedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("CreatedDate")).ToString("HH:mm:ss") : ""%>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top"  />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TicketId" HeaderText="Ticket ID"  >
                        <HeaderStyle Width="80px" HorizontalAlign="Center"/>
                        <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Top"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="CampaignName" HeaderText="Campaign"  >
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="CitizenId" HeaderText="รหัสบัตรประชาชน"  >
                        <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                        <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="Firstname" HeaderText="ชื่อ" >
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="Lastname" HeaderText="นามสกุล" >
                        <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                        <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Top"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="StatusDesc" HeaderText="สถานะของ Lead">
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ContactPhone" HeaderText="หมายเลขโทรศัพท์ที่ติดต่อลูกค้า">
                        <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                        <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OwnerName" HeaderText="Owner Lead">
                        <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                        <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ContactDetail" HeaderText="รายละเอียดการติดต่อ">
                        <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                        <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreatedName" HeaderText="ผู้บันทึก">
                        <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                        <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
            </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>