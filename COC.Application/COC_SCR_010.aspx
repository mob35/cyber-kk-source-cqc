<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_010.aspx.cs" Inherits="COC.Application.COC_SCR_010" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:180px;
        }
        .ColInput
        {
            width:250px;
        }
        .ColCheckBox
        {
            width:160px;
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
                        หน้าจอ
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbScreen" runat="server" Width="250px" CssClass="Dropdownlist"></asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td colspan="2" style="height:5px;">
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                    </td>
                    <td class="ColInput">
                       <asp:Button ID="btnSearch" runat="server" CssClass="Button" Width="100px" 
                            Text="ค้นหา" onclick="btnSearch_Click"  OnClientClick="DisplayProcessing()" />
                       <asp:TextBox ID="txtScreen" runat="server" CssClass="Hidden" ></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="Line"></div>
    <br />
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Image ID="imgResult" runat="server" ImageUrl="~/Images/hResult.gif" ImageAlign="Top" />&nbsp;
                <br /><br />
                <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" Width="450px"
                    GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True"   
                    EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" >
                    <Columns>
                        <asp:TemplateField HeaderText="No">
                            <ItemTemplate>
                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("CNT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Role">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffTypeName" runat="server" Text='<%# Eval("StaffTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle Width="200px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="มีสิทธิ">
                            <ItemTemplate>
                                <asp:RadioButton runat="server" ID="rdHavePrivilege" GroupName ="Privilege" Checked ='<%# Eval("isView") == null ? false : (Eval("isView").ToString() == "1" ? true : false) %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ไม่มีสิทธิ">
                            <ItemTemplate>
                                <asp:RadioButton runat="server" ID="rdNoPrivilege" GroupName ="Privilege" Checked ='<%# Eval("isView") == null ? false : (Eval("isView").ToString() == "0" ? true : false) %>' />
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ScreenId">
                            <ItemTemplate>
                                <asp:Label ID="lblScreenId" runat="server" Text='<%# Eval("ScreenId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="ValidateId">
                            <ItemTemplate>
                                <asp:Label ID="lblValidateId" runat="server" Text='<%# Eval("ValidateId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="StaffTypeId">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffTypeId" runat="server" Text='<%# Eval("StaffTypeId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="Hidden"  />
                            <HeaderStyle CssClass="Hidden"  />
                       </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                </asp:GridView>
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="Button" Width="100px" 
            Text="บันทึก" OnClientClick="DisplayProcessing()" onclick="btnSave_Click" />
        </ContentTemplate>
        
    </asp:UpdatePanel> 
    
</asp:Content>
