<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_007.aspx.cs" Inherits="COC.Application.COC_SCR_007" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="Shared/GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc3" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="upInfo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0" >
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">Windows Username <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:Label ID="lblUsername" runat="server" Font-Size="13px" BorderStyle="Solid" BorderWidth="1px" BorderColor="#7f9db9" Width="152px" BackColor="#e5edf5"></asp:Label>
                        <asp:TextBox ID="txtUsername" runat="server" Visible="false" CssClass="Textbox" ReadOnly="true" Width="150px" MaxLength="100" ></asp:TextBox>
                        <asp:TextBox ID="txtStaffId" runat="server" Width="10px" Visible="false"></asp:TextBox>
                        <asp:Label ID="vtxtUsername" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">รหัสพนักงานธนาคาร <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:TextBox ID="txtEmpCode" runat="server" CssClass="Textbox" Width="150px" MaxLength="6" ></asp:TextBox>
                        <asp:Label ID="vtxtEmpCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">รหัสเจ้าหน้าที่การตลาด</td>
                    <td class="style3">
                        <asp:TextBox ID="txtMarketingCode" runat="server" CssClass="Textbox" Width="150px" MaxLength="10" ></asp:TextBox>
                        <asp:Label ID="vtxtMarketingCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">ชื่อ-นามสกุลพนักงาน <span class="style4">*</span></td>
                    <td class="style5">
                        <asp:TextBox ID="txtStaffNameTH" runat="server" CssClass="Textbox" Width="260px" MaxLength="100" ></asp:TextBox>
                        <asp:Label ID="vtxtStaffNameTH" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">เบอร์โทรศัพท์</td>
                    <td class="style3">
                        <asp:TextBox ID="txtTellNo" runat="server" CssClass="Textbox" Width="100px" MaxLength="10" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">E-mail <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:TextBox ID="txtStaffEmail" runat="server" CssClass="Textbox" Width="260px" 
                        MaxLength="100" AutoPostBack="True" ontextchanged="txtEmail_TextChanged" ></asp:TextBox>
                        <asp:Label ID="vtxtStaffEmail" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">ตำแหน่ง <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbPosition" runat="server" CssClass="Dropdownlist" Width="263px"></asp:DropDownList>
                        <asp:Label ID="vtxtPositionName" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">Role <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbStaffType" runat="server" CssClass="Dropdownlist" AutoPostBack="true" 
                            Width="262px" onselectedindexchanged="cmbStaffType_SelectedIndexChanged"  ></asp:DropDownList>
                        <asp:Label ID="vcmbStaffType" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">ทีมการตลาด</td>
                    <td class="style3">
                        <asp:TextBox ID="txtTeam" runat="server" CssClass="Textbox" Width="260px" MaxLength="100"  ></asp:TextBox>
                        <asp:Label ID="vtxtTeam" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สาขา <span class="style4">*</span></td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbBranchCode" runat="server" CssClass="Dropdownlist"  
                            Width="262px" AutoPostBack="True" onselectedindexchanged="cmbBranchCode_SelectedIndexChanged"></asp:DropDownList>
                        <asp:TextBox ID="txtOldBranchCode" runat="server" Visible="false" ></asp:TextBox>
                        <asp:Label ID="vcmbBranchCode" runat="server" CssClass="style4"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">หัวหน้างาน</td>
                    <td class="style3">
                        <asp:DropDownList ID="cmbHeadStaffId" runat="server" CssClass="Dropdownlist" Width="262px" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สถานะ </td>
                    <td class="style3">
                        <asp:RadioButton ID="rdNormal" runat="server" GroupName="EmpStatus" Text="ปกติ" 
                            AutoPostBack="True" oncheckedchanged="rdNormal_CheckedChanged" />
                        <asp:RadioButton ID="rdRetire" runat="server" GroupName="EmpStatus" 
                            Text="ลาออก" AutoPostBack="True" oncheckedchanged="rdRetire_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="vEmpStatus" runat="server" CssClass="style4"></asp:Label>
                        <asp:TextBox ID="txtOldIsDeleted" runat="server" Visible="false" ></asp:TextBox>
                        <asp:TextBox ID="txtNewIsDeleted" runat="server" Visible="false" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">สาย <span class="style4"></span></td>
                    <td class="style3">
                       <asp:DropDownList ID="cmbDepartment" runat="server" Width="262px" CssClass="Dropdownlist" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:10px"></td>
                    <td class="style2">COC Team&nbsp;
                        <asp:Label ID="lblCocTeam" runat="server" CssClass="style4" Text="*" Visible="false"></asp:Label>
                    </td>
                    <td class="style3">
                       <asp:DropDownList ID="cmbCocTeam" runat="server" Width="262px" CssClass="Dropdownlist" >
                       </asp:DropDownList>
                       <asp:Label ID="vCocTeam" runat="server" CssClass="style4"></asp:Label>
                       <asp:TextBox ID="txtCurrentCocTeam" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height:15px;">
                    </td>
                </tr>
                <tr style="height:35px;">
                    <td style="width:10px"></td>
                    <td class="style2"></td>
                    <td  class="style3" >
                        <asp:Button ID="btnSave" runat="server" Text="บันทึก" Width="100px"  
                            OnClientClick="DisplayProcessing()" onclick="btnSave_Click" />&nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="ยกเลิก" Width="100px" OnClientClick="return confirm('ต้องการยกเลิกใช่หรือไม่')"    
                            onclick="btnClose_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:UpdatePanel ID="upJobOnHand" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                     <div class="Line"></div>
                     <br />
                    <table>
                        <tr><td colspan="5"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/OnHand.gif" /></td></tr>
                        <tr>
                            <td style="width:20px"></td>
                            <td style="width:70px; font-weight:bold;">
                                โอนให้
                            </td>
                            <td> 
                                <asp:DropDownList ID="cmbTransferee" runat="server" Width="250px" CssClass="Dropdownlist" >
                                </asp:DropDownList>
                            </td>
                            <td style="width:20px"></td>
                            <td>
                                <asp:Button ID="btnTransferJob" runat="server" Width="100px" OnClick="btnTransferJob_Click" OnClientClick="if (confirm('ต้องการโอนงานใช่หรือไม่')) { DisplayProcessing(); return true; } else { return false; }" CssClass="Button" Text="โอนงาน" ></asp:Button>
                                &nbsp;&nbsp;<asp:Button ID="btnRefreshJobOnHand" runat="server" Text="Refresh งานในมือ" Width="130px" CssClass="Button" OnClick="btnRefreshJobOnHand_Click" OnClientClick="DisplayProcessing()" />
                            </td>
                        </tr>
                    </table>
                     <br /><br />
                        <uc3:GridviewPageController ID="pcJobOnHand" runat="server" OnPageChange="PageSearchChangeJobOnHand" Width="2990px" />
                        <asp:GridView ID="gvJobOnHand" runat="server" AutoGenerateColumns="False" Width="2990px"
                            GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True" EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>" >
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
                                <asp:BoundField DataField="AppNo" HeaderText="เลขที่ใบ<br/>คำเสนอขอซื้อ" HtmlEncode="false" >
                                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MarketingOwner" HeaderText="รหัสเจ้าหน้าที่<br/>การตลาด" HtmlEncode="false"  >
                                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MarketingOwnerName" HeaderText="ชื่อ-นามสกุล<br/>เจ้าหน้าที่การตลาด" HtmlEncode="false"  >
                                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CocStatusDesc" HeaderText="สถานะ COC"  >
                                    <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DealerCode" HeaderText="รหัส Dealer"  >
                                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top"  />
                                </asp:BoundField>
                                <asp:BoundField DataField="DealerName" HeaderText="ชื่อ Dealer"  >
                                    <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="140px" HorizontalAlign="Left" VerticalAlign="Top" />
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
                                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="120px" HorizontalAlign="Left" VerticalAlign="Top" />
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
                                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CardNo" HeaderText="หมายเลขบัตร"  >
                                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SlmOwnerName" HeaderText="SLM Owner"  >
                                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SlmDelegateName" HeaderText="SLM Delegate"  >
                                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LastOwnerName" HeaderText="Last Owner"  >
                                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" VerticalAlign="Top" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>

