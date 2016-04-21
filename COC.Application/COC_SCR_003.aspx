<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_003.aspx.cs" Inherits="COC.Application.COC_SCR_003" %>
<%@ Register src="Shared/Tabs/Tab005.ascx" tagname="Tab005" tagprefix="uc1" %>
<%@ Register src="Shared/Tabs/Tab008.ascx" tagname="Tab008" tagprefix="uc2" %>
<%@ Register src="Shared/Tabs/Tab004.ascx" tagname="Tab004" tagprefix="uc3" %>
<%@ Register src="Shared/Tabs/Tab009.ascx" tagname="Tab009" tagprefix="uc4" %>
<%@ Register src="Shared/Tabs/Tab007.ascx" tagname="Tab007" tagprefix="uc5" %>
<%@ Register src="Shared/Tabs/Tab006.ascx" tagname="Tab006" tagprefix="uc6" %>

<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ColInfo
        {
            font-weight:bold;
            width:220px;
        }
        .ColInputView
        {
            width:150px;
        }
        .ColCheckBox
        {
            width:160px;
        }
    </style>
    <script language="javascript" type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Image ID="imgSearch" runat="server" ImageUrl="~/Images/hGeneral.gif" />
    <asp:UpdatePanel ID="upMainData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="ColInfo">
                        Ticket ID
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtTicketID" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                        <asp:TextBox ID="txtCitizenId" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtTelNo1" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtChannelId" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtCampaignId" runat="server" Visible="false" Width="10px" ></asp:TextBox>
                        <asp:TextBox ID="txtUserLoginChannelId" runat="server" Visible="false"  Width="10px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สถานะของ lead
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtstatus" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        หมายเลขโทรศัพท์ 1(มือถือ)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtTelNo_1" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        ชื่อ
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtFirstname" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        นามสกุล
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtLastname" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        หมายเลขโทรศัพท์ 2
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtTelNo2" runat="server" CssClass="TextboxView" ReadOnly="true" Width="70px" ></asp:TextBox>
                        <asp:Label ID="label1" runat="server" Width="10px" CssClass="LabelC" Text="-"></asp:Label>
                        <asp:TextBox ID="txtExt2" runat="server" CssClass="TextboxView" Width="38px" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        แคมเปญ
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtCampaignName" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        Product ที่สนใจ
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtInterestedProd" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        หมายเลขโทรศัพท์ 3
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtTelNo3" runat="server" CssClass="TextboxView" ReadOnly="true" Width="70px" ></asp:TextBox>
                        <asp:Label ID="label2" runat="server" Width="10px" CssClass="LabelC" Text="-"></asp:Label>
                        <asp:TextBox ID="txtExt3" runat="server" CssClass="TextboxView" Width="38px" ReadOnly="true" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        วันเวลาที่ได้ติดต่อ Lead ล่าสุด (SLM)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtContactLatestDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        วันเวลาที่ได้รับมอบหมายล่าสุด (SLM)
                    </td>
                    <td class="ColInputView"> 
                        <asp:TextBox ID="txtAssignDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo"></td>
                    <td class="ColInputView"></td>
                </tr>
                 <tr>
                    <td class="ColInfo">
                        วันเวลาที่ติดต่อ Lead ครั้งแรก (SLM)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtContactFirstDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        วันเวลาที่ได้รับมอบหมายล่าสุด (COC)
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtCOCAssignDate" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo"></td>
                    <td class="ColInputView"></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Owner Branch
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtOwnerBranch" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        Owner Lead
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtOwnerLead" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo"></td>
                    <td class="ColInputView"></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Delegate Branch
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtDelegateBranch" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                        
                    </td>
                    <td class="ColInfo">
                        Delegate Lead
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtDelegateLead" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" ></asp:TextBox>
                    </td>
                    <td class="ColInfo"></td>
                    <td class="ColInputView"></td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        Marketing Owner
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtMarketingOwner" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px"  Text=""></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        สถานะของ COC
                    </td>
                    <td colspan="3" class="ColInputView">
                        <asp:TextBox ID="txtCocStatus" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px" Text="" ></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="ColInfo">
                        LastOwner
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtLastOwner" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px"  Text=""></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        Team
                    </td>
                    <td class="ColInputView">
                        <asp:TextBox ID="txtCocTeam" runat="server" CssClass="TextboxView" ReadOnly="true" Width="130px"  Text=""></asp:TextBox>
                    </td>
                    <td class="ColInfo">
                        <asp:Button ID="btnBack" runat="server" Text="ย้อนกลับ" CssClass="Button"  
                            Width="90px" onclick="btnBack_Click"  />
                    </td>
                    <td class="ColInputView" >
                        
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br /><br />
    <asp:UpdatePanel ID="upHistory" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="1190px" >
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="right">
                        รวมทั้งสิ้น
                        <asp:Label ID="lbSum" runat="server"></asp:Label>
                        รายการ
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlHistory" runat="server" ScrollBars ="Auto" Height="170px" Width="1190px" BorderStyle="Solid" BorderWidth="1px" >
                <asp:GridView ID="gvCampaign" runat="server" AutoGenerateColumns="False" 
                    GridLines="Horizontal" BorderWidth="0px"  Width="1160px"
                    EnableModelValidation="True" 
                    EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>" 
                    onrowdatabound="gvCampaign_RowDataBound"  > 
                    <Columns>
                        <asp:TemplateField HeaderText="CampaignFinalId">
                            <ItemTemplate>
                                <asp:Label ID="lbCampaignFinalId"  runat="server" Text='<%#Bind("CampaignFinalId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbCampaignFinalIdEdit" runat="server" Text='<%#Bind("CampaignFinalId") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemStyle CssClass="Hidden" />
                            <HeaderStyle CssClass="Hidden" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="40px" HorizontalAlign="Center" VerticalAlign="Top"/>
                            <HeaderStyle Width="40px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ชื่อ Product/Campaign">
                            <ItemTemplate>
                                <asp:Label ID="lbCampaign"  runat="server" Text='<%#Bind("CampaignName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="220px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="220px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รายละเอียด">
                            <ItemTemplate>
                                <asp:Label ID="lbCampaignDetail" runat="server" Text='<%#Bind("CampaignDetail") %>'></asp:Label>
                                <asp:LinkButton ID="lbShowCampaignDesc" runat="server" Text="อ่านต่อ" CommandArgument='<%# Eval("CampaignId") %>' Visible="false" ></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="600px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="600px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="วันที่ดำเนินการ">
                            <ItemTemplate>
                                <%# Eval("CreatedDate") != null ? Convert.ToDateTime(Eval("CreatedDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("CreatedDate")).Year.ToString() + " " + Convert.ToDateTime(Eval("CreatedDate")).ToString("HH:mm:ss") : ""%>
                            </ItemTemplate>
                            <ItemStyle Width="140px" HorizontalAlign="Center" VerticalAlign="Top"/>
                            <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="ผู้ดำเนินการ">
                            <ItemTemplate>
                                <asp:Label ID="lbCreatedByName"  runat="server" Text='<%#Bind("CreatedByName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="160px" HorizontalAlign="Left" VerticalAlign="Top"/>
                            <HeaderStyle Width="160px" HorizontalAlign="Center"/>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="t_rowhead" />
                    <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate> 
    </asp:UpdatePanel> 
    <br />
    <asp:UpdatePanel ID="upTabMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <act:TabContainer ID="tabMain" runat="server" ActiveTabIndex="0" Width="1190px" >
                <act:TabPanel ID="tab004" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader004" runat="server" Text="&nbsp;ข้อมูล Lead&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc3:Tab004 ID="tabLeadInfo" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab005" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader005" runat="server" Text="&nbsp;Existing Lead&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc1:Tab005 ID="tabExistingLead" runat="server" />
                    </ContentTemplate>         
                </act:TabPanel>
                <act:TabPanel ID="tab006" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader006" runat="server" Text="&nbsp;Existing Product&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc6:Tab006 ID="tabExistingProduct" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab007" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader007" runat="server" Text="&nbsp;Owner Logging&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc5:Tab007 ID="tabOwnerLogging" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab008" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader008" runat="server" Text="&nbsp;Activity&nbsp;" CssClass="tabHeaderText"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc2:Tab008 ID="tabPhoneCallHistory" runat="server"  />
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="tab009" runat="server" >
                    <HeaderTemplate>
                        <asp:Label ID="lblHeader009" runat="server" Text="&nbsp;Note History&nbsp;" CssClass="tabHeaderText"></asp:Label>                 
                    </HeaderTemplate>
                    <ContentTemplate>
                        <uc4:Tab009 ID="tabNoteHistory" runat="server" />
                    </ContentTemplate>
                </act:TabPanel>
            </act:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
