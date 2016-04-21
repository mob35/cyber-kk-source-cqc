<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="COC.Application.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /><br /><br /><br />
    <script language="javascript" type="text/javascript">
        function LoginProcessing() {
            var username = document.getElementById('ContentPlaceHolder1_Login1_UserName').value.trim();
            var password = document.getElementById('ContentPlaceHolder1_Login1_Password').value.trim();
            if (username != '' && password != '') {
                DisplayProcessing();
            }
        }
    </script>
    <asp:UpdatePanel ID="upLogin" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width:330px;height:339px;" align="center" cellpadding="3" width="100%">
                <tr>
                    <td>
                        <asp:Login ID="Login1" runat="server" onauthenticate="Login1_Authenticate">
                            <LayoutTemplate>
                                <table cellpadding="3" cellspacing="0" style="border-collapse:collapse;">
                                    <tr>
                                        <td align="center" colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Font-Bold="true" Font-Size="13px">Windows Username:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" CssClass="Textbox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ForeColor="Red"  
                                                ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Font-Bold="true" Font-Size="13px">Windows Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="Textbox"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ForeColor="Red" 
                                                ControlToValidate="Password" ErrorMessage="Password is required." 
                                                ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="height:10px;">
                                        </td>
                                        <td >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                        </td>
                                        <td >
                                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="เข้าสู่ระบบ" ValidationGroup="Login1" CssClass="Button" OnClientClick="LoginProcessing()" />
                                            <asp:CheckBox ID="cbRememberMe" runat="server" Text="Remember me next time." Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                        </td>
                                        <td style="color:Red;">
                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False" Visible="false"></asp:Literal>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <span style="color:Red;">คุณสามารถเข้าระบบได้ด้วย Account Windows ของคุณ<br />และห้ามกรอกรหัสผ่านผิดเกิน 3 ครั้ง</span>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:Login>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
