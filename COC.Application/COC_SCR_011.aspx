<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_011.aspx.cs" Inherits="COC.Application.COC_SCR011" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc2" %>

<%@ Register src="Shared/TextDateMask.ascx" tagname="TextDateMask" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:150px;
        }
        .ColInput
        {
            width:400px;
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
                        วันที่
                    </td>
                    <td class="ColInput">
                        <uc1:TextDateMask ID="tdSearchFrom" runat="server" />&nbsp;
                        ถึง&nbsp;
                        <uc1:TextDateMask ID="tdSearchTo" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <td class="ColInfo">
                        Webservice Name
                    </td>
                    <td class="ColInput">
                       <asp:DropDownList ID="cmbWSName" runat="server" Width="284px" CssClass="Dropdownlist"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        สถานะ
                    </td>
                    <td class="ColInput">
                       <asp:DropDownList ID="cmbStatus" runat="server" Width="150px" CssClass="Dropdownlist">
                       <asp:ListItem Value="" Text="ทั้งหมด"></asp:ListItem>
                        <asp:ListItem Value="SUCCESS" Text="Success"></asp:ListItem>
                        <asp:ListItem Value="NOTSUCCESS" Text ="Fail" ></asp:ListItem>
                       </asp:DropDownList>
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
                <uc2:GridviewPageController ID="pcTop" runat="server" Width="1180px"  OnPageChange="PageChange" />
                <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" Width="1180px"
                    GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True"   
                    EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" >
                    <Columns>
                         <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <%# Eval("OperationDate") != null ? Convert.ToDateTime(Eval("OperationDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("OperationDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("OperationDate")).ToString("HH:mm:ss") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle Width="140px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Service Name">
                            <ItemTemplate>
                                <asp:Label ID="lblWSName" runat="server" Text='<%# Eval("WSName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle Width="140px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Channel">
                            <ItemTemplate>
                                <asp:Label ID="lblChannel" runat="server" Text='<%# Eval("Channel") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="110px" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle Width="110px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Ticket Id">
                            <ItemTemplate>
                                <asp:Label ID="lblTicketId" runat="server" Text='<%# Eval("TicketId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response Code">
                            <ItemTemplate>
                                <asp:Label ID="lblResponseCode" runat="server" Text='<%# Eval("ResponseCode") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="90px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="90px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response Desc">
                            <ItemTemplate>
                                <asp:Label ID="lblResponseDesc" runat="server" Text='<%# Eval("ResponseDesc") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Response Datetime">
                            <ItemTemplate>
                                <%# Eval("ResponseDate") != null ? Convert.ToDateTime(Eval("ResponseDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("ResponseDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("ResponseDate")).ToString("HH:mm:ss") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="150px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CauseError" HeaderText="Error Detail" HtmlEncode="true" >
                            <ItemStyle Width="300px" HorizontalAlign="Left" VerticalAlign="Top" />
                            <HeaderStyle Width="300px" HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
