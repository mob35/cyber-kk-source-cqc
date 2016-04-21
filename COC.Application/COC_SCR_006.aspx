<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_006.aspx.cs" Inherits="COC.Application.COC_SCR_006" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:160px;
        }
        .ColInput
        {
            width:250px;
        }
        .ColCheckBox
        {
            width:160px;
        }
        .style1
        {
            width: 50px;
        }
        .style2
        {
            width: 200px;
            text-align:left;
            font-weight:bold;
        }
        .style3
        {
            width: 380px;
            text-align:left;
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
    <asp:Image ID="imgSearch" runat="server" ImageUrl="~/Images/hSearch.gif" />
    <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="ColInfo">
                        Windows Username
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtUsernameSearch" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สาขา
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbBranchSearch" runat="server" CssClass="Dropdownlist" Width="204px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        รหัสพนักงานธนาคาร
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtEmpCodeSearch" runat="server" CssClass="Textbox" Width="200px" MaxLength="6" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        รหัสเจ้าหน้าที่การตลาด
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtMarketingCodeSearch" runat="server" CssClass="Textbox" Width="200px" MaxLength="10" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        ชื่อ-นามสกุลพนักงาน
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtStaffNameTHSearch" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        ตำแหน่ง
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbPositionSearch" runat="server" CssClass="Dropdownlist" Width="203px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Role
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbStaffTypeSearch" runat="server" CssClass="Dropdownlist" Width="204px"></asp:DropDownList>
                    </td>
                    <td class="ColInfo">
                        ทีมการตลาด
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtTeamSearch" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        สาย
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbDepartmentSearch" runat="server" CssClass="Dropdownlist" Width="204px"></asp:DropDownList>
                    </td>
                    <td class="ColInfo">
                        ทีม COC
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbCOCTeamSearch" runat="server" CssClass="Dropdownlist" Width="204px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height:15px;">
                    </td>
                </tr>
                 <tr>
                    <td class="ColInfo">
                    </td>
                    <td colspan="5">
                        <asp:Button ID="btnSearch" runat="server" CssClass="Button" Width="100px" 
                            OnClientClick="DisplayProcessing()" Text="ค้นหา" onclick="btnSearch_Click"  />&nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="Button" Width="100px" Text="ล้างข้อมูล" OnClick="btnClear_Click"  />
                    </td>
                </tr>
            </table><br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="Line"></div>
    <br />
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Image ID="imgResult" runat="server" ImageUrl="~/Images/hResult.gif" ImageAlign="Top" />&nbsp;
            <asp:Button ID="btnAddUser" runat="server" Text="เพิ่มพนักงาน" Width="100px" 
                CssClass="Button" Height="22px" onclick="btnAddUser_Click"  /><br /><br />
            <uc2:GridviewPageController ID="pcTop" runat="server" Width="1300px" OnPageChange="PageSearchChange" Visible="false" />
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" Width="1300px"
                GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True"  
                EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" >
                <Columns>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbAction" runat="server" ImageUrl="~/Images/edit.gif" CommandArgument='<%# Eval("StaffId") %>' ToolTip="แก้ไขข้อมูลพนักงาน" OnClick="imbAction_Click" />
                        </ItemTemplate>
                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="EmpCode" HeaderText="รหัสพนักงาน<br/>ธนาคาร" HtmlEncode="false"  >
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                        <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MarketingCode" HeaderText="รหัสเจ้าหน้าที่<br/>การตลาด" HtmlEncode="false"  >
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                        <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Username" HeaderText="Windows Username" >
                        <HeaderStyle Width="140px" HorizontalAlign="Center" />
                        <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ชื่อ-นามสกุลพนักงาน">
                        <ItemTemplate>
                            <%# Eval("StaffNameTH") != null ? Eval("StaffNameTH").ToString().Replace(" ", "&nbsp;") : "" %>
                        </ItemTemplate>
                        <HeaderStyle Width="160px" HorizontalAlign="Center"/>
                        <ItemStyle Width="160px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="PositionName" HeaderText="ตำแหน่ง" >
                        <HeaderStyle Width="130px" HorizontalAlign="Center"/>
                        <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="StaffTypeDesc" HeaderText="Role">
                        <HeaderStyle Width="110px" HorizontalAlign="Center"/>
                        <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Team" HeaderText="ทีมการตลาด">
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BranchName" HeaderText="สาขา">
                        <HeaderStyle Width="180px" HorizontalAlign="Center"/>
                        <ItemStyle Width="180px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DepartmentName" HeaderText="สาย">
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top"  />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="สถานะ">
                        <ItemTemplate>
                            <%# Eval("Is_Deleted") != null ? (Eval("Is_Deleted").ToString() == "1" ? "ลาออก" : "ปกติ") : "" %>
                        </ItemTemplate>
                        <HeaderStyle Width="80px" HorizontalAlign="Center"/>
                        <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="วันที่แก้ไขสถานะล่าสุด">
                        <ItemTemplate>
                            <%# Eval("UpdateStatusDate") != null ? Convert.ToDateTime(Eval("UpdateStatusDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("UpdateStatusDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("UpdateStatusDate")).ToString("HH:mm:ss") : ""%>
                        </ItemTemplate>
                        <HeaderStyle Width="90px" HorizontalAlign="Center"/>
                        <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="t_rowhead" />
                <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>

