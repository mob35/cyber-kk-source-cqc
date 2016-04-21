<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true"
    CodeBehind="COC_SCR_102.aspx.cs" Inherits="COC.Application.COC_SCR_013" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight: bold;
            width: 160px;
        }
        
        .ColInput
        {
            width: 250px;
        }
        
        .ColCheckBox
        {
            width: 160px;
        }
        
        .style1
        {
            width: 50px;
        }
        
        .style2
        {
            width: 200px;
            text-align: left;
            font-weight: bold;
        }
        
        .style3
        {
            width: 450px;
            text-align: left;
        }
        
        .style4
        {
            font-family: Tahoma;
            font-size: 9pt;
            color: Red;
        }
        
        .style5
        {
            width: 500px;
            text-align: left;
        }
        
        .style6
        {
            width: 500px;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="upInfo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td style="width: 10px">
                    </td>
                    <td class="style2">
                        ลำดับที่
                    </td>
                    <td class="style3">
                        <asp:HiddenField ID="hidRankingId" runat="server" />
                        <asp:TextBox ID="txtSeq" runat="server" disabled="disabled" CssClass="Textbox" Width="150px"
                            MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10px">
                    </td>
                    <td class="style2">
                        ชื่อลำดับที่ <span class="style4">*</span>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtName" runat="server" CssClass="Textbox" Width="500px" MaxLength="100"></asp:TextBox>
                        <asp:Label ID="vtxtName" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr id="trSkip" runat="server">
                    <td style="width: 10px">
                    </td>
                    <td class="style2">
                        วันที่ข้าม(ตั้งแต่)<span class="style4">*</span>
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtSkip" runat="server" CssClass="Textbox" Width="150px" MaxLength="2"></asp:TextBox>
                        <asp:Label ID="Label1" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 15px;">
                    </td>
                </tr>
                <tr id="trAdd" runat="server" style="height: 35px;">
                    <td style="width: 10px">
                    </td>
                    <td class="style2">
                    </td>
                    <td class="style3">
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" Width="100px" OnClick="btnSave_Click"
                            OnClientClick="if (confirm('ต้องการบันทึกข้อมูลใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />&nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="ยกเลิก" Width="100px" OnClick="btnClose_Click"
                            OnClientClick="return confirm('ต้องการยกเลิกใช่หรือไม่')" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divEdit" runat="server">
        <asp:UpdatePanel ID="upResultCampaign" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <div class="Line">
                </div>
                <br />
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #c3e2f9; border: 1px solid #1578c0; width: 1200px; font-weight: bold;
                            font-size: 14px; height: 22px; color: #0c2f48;">
                            &nbsp;Campaign Infomation
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                </table>
                <%--<asp:Image ID="Image2" runat="server" ImageUrl="~/Images/hResult.gif" ImageAlign="Top" />&nbsp;--%>
                <asp:Button ID="btnAddCampaign" runat="server" Text="เพิ่ม Campaign" CssClass="Button"
                    Width="150px" OnClick="btnAddCampaign_Click" />
                <br />
                <br />
                <%--<uc2:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange_AddCampaign" Width="660px" />--%>
                <asp:GridView ID="gvAddCampaign" runat="server" AutoGenerateColumns="False" Width="660px"
                    GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True" HeaderStyle-BorderColor="#f995f8"
                    EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>">
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="imbEditCampaign" runat="server" ImageUrl="~/Images/edit.gif" CommandArgument='<%# Container.DisplayIndex %>' OnClick="imbEditCampaign_Click" ToolTip="แก้ไข Campaign" OnClientClick="DisplayProcessing();" />--%>
                                <asp:ImageButton ID="imbDeleteCampaign" runat="server" ImageUrl="~/Images/delete.gif"
                                    CommandArgument='<%# Eval("coc_RankingCampaignId") %>' ToolTip="ลบ Campaign"
                                    OnClick="imbDeleteCampaign_Click" OnClientClick="if (confirm('ต้องการลบข้อมูล ใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="20px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="coc_CampaignCode" HeaderText="รหัส Campaign">
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="coc_CampaignName" HeaderText="ชื่อ Campaign">
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                            <ItemStyle Width="100px" HorizontalAlign="left" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="ID">
                            <ItemTemplate>
                                <asp:Label ID="lblRankingCampaignId" runat="server" Text='<%# Eval("coc_RankingCampaignId") %>'></asp:Label>
                            </ItemTemplate>
                            <ControlStyle CssClass="Hidden" />
                            <ItemStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                            <FooterStyle CssClass="Hidden" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Begin Campaign Popup Section -->
        <asp:UpdatePanel ID="upPopupAddCampaign" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnPopupAddCampaign" Width="0px" CssClass="Hidden" />
                <asp:Panel runat="server" ID="pnPopupAddCampaign" Style="display: none" CssClass="modalPopupAddCampaign">
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td colspan="3" style="height: 20px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15px;">
                            </td>
                            <td style="font-weight: bold; width: 140px;">
                                รหัส Campaign<span style="color: Red;">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbCampaignCode" runat="server" CssClass="Dropdownlist" Width="203px"
                                    AutoPostBack="true" OnSelectedIndexChanged="cmbCampaignCode_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lblAlertCampaignCode" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: 15px;">
                            </td>
                            <td style="font-weight: bold; width: 140px;">
                                ชื่อ Campaign<span style="color: Red;">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbCampaignName" runat="server" CssClass="Dropdownlist" Width="203px"
                                    AutoPostBack="true" OnSelectedIndexChanged="cmbCampaignName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lblAlertCampaignName" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15px;">
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td style="width: 15px;">
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSavePopupAddCampaign" runat="server" Text="บันทึก" Width="90px"
                                    CssClass="Button" OnClick="btnSavePopupAddCampaign_Click" OnClientClick="DisplayProcessing();" />
                                <asp:Button ID="btnCancelPopupAddCampaign" runat="server" Text="ยกเลิก" Width="90px"
                                    CssClass="Button" OnClick="btnCancelPopupAddCampaign_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <act:ModalPopupExtender ID="mpePopupAddCampaign" runat="server" TargetControlID="btnPopupAddCampaign"
                    PopupControlID="pnPopupAddCampaign" BackgroundCssClass="modalBackground" DropShadow="True">
                </act:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upResultDealer" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <div class="Line">
                </div>
                <br />
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #c3e2f9; border: 1px solid #1578c0; width: 1200px; font-weight: bold;
                            font-size: 14px; height: 22px; color: #0c2f48;">
                            &nbsp;Dealer Infomation
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 5px;">
                        </td>
                    </tr>
                </table>
                <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/hResult.gif" ImageAlign="Top" />&nbsp;--%>
                <asp:Button ID="btnAddDealer" runat="server" Text="เพิ่ม Dealer" CssClass="Button"
                    Width="150px" OnClick="btnAddDealer_Click" />&nbsp;
                <asp:CheckBox runat="server" ID="chkAllDealer" OnCheckedChanged="chkAllDealer_CheckedChanged"
                    AutoPostBack="true"  onClick=" if(this.checked){if (confirm('ต้องการเลือกข้อมูล All Dealer ใช่หรือไม่')) {  } else { return false; }}" />
                All Dealer

                <br />
                <br />
                <%--<uc2:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange_AddDealer" Width="660px" />--%>
                <asp:GridView ID="gvAddDealer" runat="server" AutoGenerateColumns="False" Width="660px"
                    GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True" HeaderStyle-BorderColor="#f995f8"
                    EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>">
                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="imbEditDealer" runat="server" ImageUrl="~/Images/edit.gif" CommandArgument='<%# Container.DisplayIndex %>' OnClick="imbEditDealer_Click" ToolTip="แก้ไข Dealer" OnClientClick="DisplayProcessing();" />--%>
                                <asp:ImageButton ID="imbDeleteDealer" runat="server" ImageUrl="~/Images/delete.gif"
                                    CommandArgument='<%# Eval("coc_RankingDealerId") %>' ToolTip="ลบ Dealer" OnClick="imbDeleteDealer_Click"
                                    OnClientClick="if (confirm('ต้องการลบข้อมูล ใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderStyle Width="20px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="coc_DealerCode" HeaderText="รหัส Dealer">
                            <HeaderStyle Width="50px" HorizontalAlign="Center" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="coc_DealerName" HeaderText="ชื่อ Dealer">
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                            <ItemStyle Width="100px" HorizontalAlign="left" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="ID">
                            <ItemTemplate>
                                <asp:Label ID="lblRankingDealerId" runat="server" Text='<%# Eval("coc_RankingDealerId") %>'></asp:Label>
                            </ItemTemplate>
                            <ControlStyle CssClass="Hidden" />
                            <ItemStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                            <FooterStyle CssClass="Hidden" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Begin Dealer Popup Section -->
        <asp:UpdatePanel ID="upPopupAddDealer" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnPopupAddDealer" Width="0px" CssClass="Hidden" />
                <asp:Panel runat="server" ID="pnPopupAddDealer" Style="display: none" CssClass="modalPopupAddDealer">
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td colspan="3" style="height: 20px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15px;">
                            </td>
                            <td style="font-weight: bold; width: 140px;">
                                รหัส Dealer<span style="color: Red;">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="Textbox" Width="150px" MaxLength="10"></asp:TextBox>
                                <asp:Label ID="lblAlertDealerCode" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: 15px;">
                            </td>
                            <td style="font-weight: bold; width: 140px;">
                                ชื่อ Dealer<span style="color: Red;">*</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDealerName" runat="server" CssClass="Textbox" Width="150px" MaxLength="100"></asp:TextBox>
                                <asp:Label ID="lblAlertDealerName" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15px;">
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td style="width: 15px;">
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSavePopupAddDealer" runat="server" Text="บันทึก" Width="90px"
                                    CssClass="Button" OnClick="btnSavePopupAddDealer_Click" OnClientClick="DisplayProcessing();" />
                                <asp:Button ID="btnCancelPopupAddDealer" runat="server" Text="ยกเลิก" Width="90px"
                                    CssClass="Button" OnClick="btnCancelPopupAddDealer_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <act:ModalPopupExtender ID="mpePopupAddDealer" runat="server" TargetControlID="btnPopupAddDealer"
                    PopupControlID="pnPopupAddDealer" BackgroundCssClass="modalBackground" DropShadow="True">
                </act:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upButton" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table cellpadding="2" cellspacing="0" border="0" width="1200px">
                    <tr style="height: 35px;">
                        <td style="width: 10px">
                        </td>
                        <td class="style2">
                        </td>
                        <td class="style6">
                            <asp:Button ID="btnDeleteAll" runat="server" Text="ลบ" Width="100px" OnClick="btnDeleteAll_Click"
                                OnClientClick="if ( confirm('ต้องการลบใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />&nbsp;
                            <asp:Button ID="btnSaveAll" runat="server" Text="บันทึก" Width="100px" OnClick="btnSaveAll_Click"
                                OnClientClick="if (confirm('ต้องการบันทึกข้อมูลใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" />&nbsp;
                            <asp:Button ID="btnCancelAll" runat="server" Text="ยกเลิก" Width="100px" OnClick="btnClose_Click"
                                OnClientClick="return confirm('ต้องการยกเลิกใช่หรือไม่')" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
