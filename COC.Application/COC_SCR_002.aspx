<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_002.aspx.cs" Inherits="COC.Application.COC_SCR_002" %>
<%@ Register src="Shared/TextDateMask.ascx" tagname="TextDateMask" tagprefix="uc1" %>
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
    <script language="javascript" type="text/javascript">
        //        function doToggle() {
        //            var pnAdvanceSearch = document.getElementById('<%=pnAdvanceSearch.ClientID%>');
        //            var lbAdvanceSearch = document.getElementById('<%=lbAdvanceSearch.ClientID%>');

        //            if (pnAdvanceSearch.style.display == '' || pnAdvanceSearch.style.display == 'none') {
        //                lbAdvanceSearch.innerHTML = "[-] <b>Advance Search</b>";
        //                pnAdvanceSearch.style.display = 'block';
        //            }
        //            else {
        //                lbAdvanceSearch.innerHTML = "[+] <b>Advance Search</b>";
        //                pnAdvanceSearch.style.display = 'none';
        //            }
        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Image ID="imgSearch" runat="server" ImageUrl="~/Images/hSearch.gif" />
    <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="ColInfo">
                        Ticket ID
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtTicketID" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                        <asp:TextBox ID="txtRecursiveList" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtTeamRecursiveList" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtLoginEmpCode" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtLoginStaffTypeId" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtLoginStaffTypeDesc" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        ชื่อ
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtFirstname" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        นามสกุล
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastname" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        รหัสบัตรประชาชน
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtCitizenId" runat="server" CssClass="Textbox" Width="200px" MaxLength="13" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        แคมเปญ
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="cmbCampaign" runat="server" Width="203px" 
                            CssClass="Dropdownlist" AutoPostBack="True" 
                            onselectedindexchanged="cmbCampaign_SelectedIndexChanged"></asp:DropDownList>--%>
                            <asp:DropDownList ID="cmbCampaign" runat="server" Width="203px" 
                            CssClass="Dropdownlist" ></asp:DropDownList>
                    </td>
                </tr>
                <!--<tr>
                    <td style="height:25px; vertical-align:bottom;">
                        <asp:LinkButton ID="lbAdvanceSearch" runat="server" ForeColor="Green" Text="[+] <b>Advance Search</b>" OnClientClick="doToggle(); return false;"></asp:LinkButton>
                    </td>
                    <td colspan="4"></td>
                </tr>-->
            </table>
            <asp:Panel ID="pnAdvanceSearch" runat="server" >
                <asp:UpdatePanel ID="upAdvanceSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="2" cellspacing="0" border="0">
                            <tr>
                                <td class="ColInfo">
                                    ช่องทาง
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbChannel" runat="server" Width="203px" CssClass="Dropdownlist"></asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                    วันทีสร้าง Lead
                                </td>
                                <td>
                                    <uc1:TextDateMask ID="tdmCreateDate" runat="server" Width="182px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ColInfo">
                                    SLM วันที่ได้รับมอบหมายล่าสุด
                                </td>
                                <td class="ColInput">
                                    <uc1:TextDateMask ID="tdmAssignDate" runat="server" Width="182px" />
                                </td>
                                <td class="ColInfo">
                                    COC วันที่ได้รับมอบหมายล่าสุด
                                </td>
                                <td>
                                    <uc1:TextDateMask ID="tdmCocAssignDate" runat="server" Width="182px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ColInfo">
                                    Owner Branch
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbOwnerBranchSearch" runat="server" Width="203px" 
                                        CssClass="Dropdownlist" AutoPostBack="true"
                                        onselectedindexchanged="cmbOwnerBranchSearch_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                     Owner Lead
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbOwnerLeadSearch" runat="server" Width="203px" CssClass="Dropdownlist"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="ColInfo">
                                    Delegate Branch
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbDelegateBranchSearch" runat="server" Width="203px" 
                                        CssClass="Dropdownlist" AutoPostBack="true"
                                        onselectedindexchanged="cmbDelegateBranchSearch_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                     Delegate Lead
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbDelegateLeadSearch" runat="server" Width="203px" CssClass="Dropdownlist"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="ColInfo">
                                   สาขาผู้สร้าง Lead
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbCreatebyBranchSearch" runat="server" Width="203px" 
                                        CssClass="Dropdownlist" AutoPostBack="true"
                                        onselectedindexchanged="cmbCreatebyBranchSearch_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                     ผู้สร้าง Lead
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbCreatebySearch" runat="server" Width="203px" CssClass="Dropdownlist"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="ColInfo">
                                   Marketing Owner
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbMarketingOwner" runat="server" Width="203px" 
                                        CssClass="Dropdownlist" ></asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                     Last Owner
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbLastOwner" runat="server" Width="203px" CssClass="Dropdownlist"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="ColInfo">
                                   COC Team
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbCocTeam" runat="server" Width="203px" CssClass="Dropdownlist" >
                                    </asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                     
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table><br />
                        <table cellpadding="3" cellspacing="0" border="0">
                            <tr>
                                <td valign="top" class="ColInfo">
                                     สถานะของ Lead
                                </td>
                                <td colspan="5">
                                    &nbsp;<asp:CheckBox ID="cbOptionAll" runat="server" Text="ทั้งหมด" AutoPostBack="true" oncheckedchanged="cbOptionAll_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbOptionList" runat="server" RepeatLayout="Table" AutoPostBack="true" 
                                        RepeatDirection="Horizontal" RepeatColumns="5" onselectedindexchanged="cbOptionList_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="height:15px;">
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="2" cellspacing="0" border="0">
                            <tr>
                                <td class="ColInfo">
                                   สถานะหลักของ COC
                                </td>
                                <td class="ColInput">
                                    <asp:DropDownList ID="cmbCocStatus" runat="server" Width="203px" 
                                        AutoPostBack="true"  CssClass="Dropdownlist" 
                                        onselectedindexchanged="cmbCocStatus_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                </td>
                                <td class="ColInfo">
                                     <asp:Label ID="lblCocSubStatus" runat="server" Text="สถานะย่อยของ COC" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbCocSubStatus" runat="server" Width="350px" CssClass="Dropdownlist" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="height:15px;">
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upButton" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td colspan="6" style="height:3px"></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                    </td>
                    <td colspan="5">
                        <asp:Button ID="btnSearch" runat="server" CssClass="Button" Width="100px" OnClientClick="DisplayProcessing()"
                            Text="ค้นหา" onclick="btnSearch_Click" />&nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="Button" Width="100px" OnClientClick="DisplayProcessing()" 
                            Text="ล้างข้อมูล" onclick="btnClear_Click" />
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
                <uc2:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange" Width="2710px" />
                <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" DataKeyNames="TicketId"
                    GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True"   
                    EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" 
                    AllowSorting="true" onsorting="gvResult_Sorting" Width="2710px" onrowdatabound="gvResult_RowDataBound" >
                    <Columns>
                    <asp:TemplateField HeaderText="SLA">
                        <ItemTemplate>
                            <asp:image ID="imgSla" runat="server" ImageUrl="~/Images/invalid.gif" Visible='<%# Eval("CocCounting") != null ? (Convert.ToInt32(Eval("CocCounting")) > 0 ? true : false) : false %>' />
                        </ItemTemplate>
                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbView" runat="server" ImageUrl="~/Images/view.gif" CommandArgument='<%# Eval("TicketId") %>' OnClick="imbView_Click" ToolTip="ดูรายละเอียดข้อมูลผู้มุ่งหวัง"  />
                        </ItemTemplate>
                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Priority">
                        <ItemTemplate>
                            <asp:Label ID="lblPriority" runat="server" ForeColor='<%# Eval("FlowType") != null ? (Eval("FlowType").ToString().Trim() == "R" ? System.Drawing.Color.Red : System.Drawing.Color.Black) : System.Drawing.Color.Black %>' Text='<%# Eval("FlowType") != null ? (Eval("FlowType").ToString().Trim() == "R" ? "High" : "Normal") : "Normal" %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Notice">
                        <ItemTemplate>
                            <asp:image ID="imgNotify" runat="server" ImageUrl="~/Images/exclamation.jpg" Visible='<%# Eval("NoteFlag") != null ? (Eval("NoteFlag").ToString() == "1" ? true : false) : false %>' />
                        </ItemTemplate>
                        <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="40px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cal">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbCal" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/Calculator.png" ToolTip="Calculator"  />
                        </ItemTemplate>
                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Doc">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbDoc" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/Document.png" ToolTip="แนบเอกสาร" OnClick="imbDocument_Click" CommandArgument='<%# Eval("TicketId") %>' OnClientClick="DisplayProcessing()" />
                        </ItemTemplate>
                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Others">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbOthers" runat="server" Width="20px" Height="20px" ImageUrl="~/Images/Others.png" ToolTip="เรียกดูข้อมูลเพิ่มเติม" OnClick="lbAolSummaryReport_Click" CommandArgument='<%# Container.DisplayIndex %>'  OnClientClick="DisplayProcessing()"  />
                        </ItemTemplate>
                        <ItemStyle Width="30px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ticket Id">
                        <ItemTemplate>
                            <asp:Label ID="lblTicketId" runat="server" Text='<%# Eval("TicketId") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CitizenId" HeaderText="รหัสบัตรประชาชน"  >
                        <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                        <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ชื่อ">
                        <ItemTemplate>
                            <asp:Label ID="lblFirstname" runat="server" Text='<%# Eval("Firstname") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="120px" HorizontalAlign="Center" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="นามสกุล">
                        <ItemTemplate>
                            <asp:Label ID="lblLastname" runat="server" Text='<%# Eval("Lastname") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="120px" HorizontalAlign="Center" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="StatusDesc" HeaderText="สถานะของ Lead" SortExpression="StatusDesc">
                        <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                        <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CocStatusDesc" HeaderText="สถานะของ COC" >
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CampaignName" HeaderText="แคมเปญ" SortExpression="CampaignName">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChannelDesc" HeaderText="ช่องทาง">
                        <HeaderStyle Width="130px" HorizontalAlign="Center"/>
                        <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OwnerName" HeaderText="Owner Lead">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DelegateName" HeaderText="Delegate Lead">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                     <asp:BoundField DataField="MarketingOwnerName" HeaderText="Marketing Owner">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastOwnerName" HeaderText="Last Owner">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                     <asp:BoundField DataField="CocTeam" HeaderText="Team">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreateName" HeaderText="ผู้สร้าง Lead">
                        <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                        <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="วันที่สร้าง Lead">
                        <ItemTemplate>
                            <%# Eval("CreatedDate") != null ? Convert.ToDateTime(Eval("CreatedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CreatedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("CreatedDate")).ToString("HH:mm:ss") : "" %>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                        <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SLM วันที่ได้รับมอบหมายล่าสุด">
                        <ItemTemplate>
                            <%# Eval("AssignedDate") != null ? Convert.ToDateTime(Eval("AssignedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("AssignedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("AssignedDate")).ToString("HH:mm:ss") : ""%>
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
                    <asp:TemplateField HeaderText="AOL Summary Report">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbAolSummaryReport" runat="server" Text="AOL Summary Report" OnClick="lbAolSummaryReport_Click" CommandArgument='<%# Container.DisplayIndex %>' OnClientClick="DisplayProcessing()" ></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" CssClass="Hidden" />
                        <ItemStyle Width="100px" CssClass="Hidden"  />
                    </asp:TemplateField>
                    <asp:BoundField DataField="OwnerBranchName" HeaderText="Owner Branch">
                        <HeaderStyle Width="130px" HorizontalAlign="Center"  />
                        <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                     <asp:BoundField DataField="DelegateBranchName" HeaderText="Delegate Branch">
                        <HeaderStyle Width="130px" HorizontalAlign="Center"  />
                        <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                     <asp:BoundField DataField="BranchCreateBranchName" HeaderText="สาขาผู้สร้าง Lead">
                        <HeaderStyle Width="130px" HorizontalAlign="Center"  />
                        <ItemStyle Width="130px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ProductName">
                        <ItemTemplate>
                            <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HasAdamUrl">
                        <ItemTemplate>
                            <asp:Label ID="lblHasAdamUrl" runat="server" Text='<%# Convert.ToBoolean(Eval("HasAdamUrl")) ? "Y" : "N" %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CampaignId">
                        <ItemTemplate>
                            <asp:Label ID="lblCampaignId" runat="server" Text='<%# Eval("CampaignId") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LicenseNo">
                        <ItemTemplate>
                            <asp:Label ID="lblLicenseNo" runat="server" Text='<%# Eval("LicenseNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TelNo1">
                        <ItemTemplate>
                            <asp:Label ID="lblTelNo1" runat="server" Text='<%# Eval("TelNo1") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ProvinceRegis">
                        <ItemTemplate>
                            <asp:Label ID="lblProvinceRegis" runat="server" Text='<%# Eval("ProvinceRegis") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CalculatorUrl">
                        <ItemTemplate>
                            <asp:Label ID="lblCalculatorUrl" runat="server" Text='<%# Eval("CalculatorUrl") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ProductGroupId">
                        <ItemTemplate>
                            <asp:Label ID="lblProductGroupId" runat="server" Text='<%# Eval("ProductGroupId") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ProductId">
                        <ItemTemplate>
                            <asp:Label ID="lblProductId" runat="server" Text='<%# Eval("ProductId") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AppNo">
                        <ItemTemplate>
                            <asp:Label ID="lblAppNo" runat="server" Text='<%# Eval("AppNo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="Hidden" />
                        <ControlStyle CssClass="Hidden" />
                        <HeaderStyle CssClass="Hidden" />
                        <FooterStyle CssClass="Hidden" />
                    </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
