<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/COC.Master" AutoEventWireup="true" CodeBehind="COC_SCR_101.aspx.cs" Inherits="COC.Application.COC_SCR_012" %>

<%@ Register Src="Shared/TextDateMask.ascx" TagName="TextDateMask" TagPrefix="uc1" %>
<%@ Register Src="Shared/GridviewPageController.ascx" TagName="GridviewPageController" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery-1.12.3.min.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <style type="text/css">
        .ColInfo {
            font-weight: bold;
            width: 180px;
        }

        .ColInput {
            width: 250px;
        }

        .ColCheckBox {
            width: 160px;
        }
    </style>
    <script type="text/javascript">

        //console.log('1');
        function pageLoad() {
            $(document).ready(function () {
                debugger;
                $("[id$=btnSaveRanking]").hide();
                $("[id$=btnCancel]").hide();
                $("[id$=btnEdit]").show();
                $("[id$=btnEdit]").click(function () {
                    debugger;
                    $("[id$=btnSaveRanking]").show();
                    $("[id$=btnCancel]").show();
                    $("[id$=btnEdit]").hide();
                    $("[id$=gvResult]").sortable({
                        items: 'tr:not(tr:first-child, tr:nth-child(2), tr:last-child)',
                        cursor: 'pointer',
                        axis: 'y',
                        dropOnEmpty: false,
                        start: function (e, ui) {
                            ui.item.addClass("selected");
                        },
                        stop: function (e, ui) {
                            ui.item.removeClass("selected");
                            //DisplayProcessing();
                            // $("[id*=btnSaveRanking]").click();
                        },
                        receive: function (e, ui) {
                            $(this).find("tbody").append(ui.item);
                        }
                    });
                    //console.log('2');
                });
            });
        }
    </script>
    <script language="javascript" type="text/javascript">
        <%--function doToggle() {
            var pnAdvanceSearch = document.getElementById('<%=pnAdvanceSearch.ClientID%>');
            var lbAdvanceSearch = document.getElementById('<%=lbAdvanceSearch.ClientID%>');
            var txtAdvanceSearch = document.getElementById('<%=txtAdvanceSearch.ClientID%>');

            if (pnAdvanceSearch.style.display == '' || pnAdvanceSearch.style.display == 'none') {
                lbAdvanceSearch.innerHTML = "[-] <b>Advance Search</b>";
                pnAdvanceSearch.style.display = 'block';
                txtAdvanceSearch.value = "Y";
            }
            else {
                lbAdvanceSearch.innerHTML = "[+] <b>Advance Search</b>";
                pnAdvanceSearch.style.display = 'none';
                txtAdvanceSearch.value = "N";
            }
        }--%>

        <%--function callsaletool(ticketid) {
            var form = document.createElement("form");
            var input_ticketid = document.createElement("input");
            var input_username = document.createElement("input");

            form.action = '<%= System.Configuration.ConfigurationManager.AppSettings["SaleToolUrl"].ToString() %>';
            form.method = "post"
            form.setAttribute("target", "_blank");

            input_ticketid.name = "ticketid";
            input_ticketid.value = ticketid;
            form.appendChild(input_ticketid);

            input_username.name = "username";
            input_username.value = '<%= HttpContext.Current.User.Identity.Name %>';
            form.appendChild(input_username);

            document.body.appendChild(form);
            form.submit();

            document.body.removeChild(form);
        }--%>

        <%--function calladam(ticketid) {
            var form = document.createElement('form');
            var input_ticketid = document.createElement('input');
            var input_username = document.createElement('input');
            var input_product = document.createElement('input');
            var input_campaign = document.createElement('input');
            var input_name = document.createElement('input');
            var input_lastname = document.createElement('input');
            var input_license_plate = document.createElement('input');
            var input_state = document.createElement('input');
            var input_mobile = document.createElement('input');

            form.action = '<%= System.Configuration.ConfigurationManager.AppSettings["AdamlUrl"].ToString() %>';
            form.method = 'post'
            form.setAttribute('target', '_blank');

            input_ticketid.name = 'ticket_id';
            input_ticketid.value = '';
            form.appendChild(input_ticketid);

            input_username.name = 'username';
            input_username.value = '<%= HttpContext.Current.User.Identity.Name %>';
            form.appendChild(input_username);

            input_product.name = 'product';
            input_product.value = "";
            form.appendChild(input_product);

            input_campaign.name = 'campaign';
            input_campaign.value = "";
            form.appendChild(input_campaign);

            input_name.name = 'name';
            input_name.value = '';
            form.appendChild(input_name);

            input_lastname.name = 'lastname';
            input_lastname.value = '';
            form.appendChild(input_lastname);

            input_license_plate.name = 'license_plate';
            input_license_plate.value = '';
            form.appendChild(input_license_plate);

            input_state.name = 'state';
            input_state.value = '';
            form.appendChild(input_state);

            input_mobile.name = 'mobile';
            input_mobile.value = '';
            form.appendChild(input_mobile);

            document.body.appendChild(form);
            form.submit();

            document.body.removeChild(form);
        }--%>

        <%--function TestPost(ticketid) {
            var form = document.createElement("form");
            var input_ticketid = document.createElement("input");

            form.action = 'SLM_SCR_004.aspx?ReturnUrl=' + '<%= Server.UrlEncode(Request.Url.AbsoluteUri) %>';
            form.method = "post"

            input_ticketid.name = "ticketid";
            input_ticketid.value = ticketid;
            form.appendChild(input_ticketid);

            document.body.appendChild(form);
            form.submit();

            document.body.removeChild(form);
        }--%>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <%--<asp:Image ID="imgSearch" runat="server" ImageUrl="~/Images/hSearch.gif" />--%>
    <%--<asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr><td colspan="4" style="height:2px;"></td></tr>
                <tr>
                    <td class="ColInfo">
                        Ticket ID
                    </td>
                    <td class="ColInput">
                        <asp:TextBox ID="txtTicketID" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                        <asp:TextBox ID="txtEmpCode" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtStaffTypeId" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtStaffTypeDesc" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtStaffId" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtStaffBranchCode" runat="server" Visible="false"></asp:TextBox>
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
                        ประเภทบุคคล
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbCardType" runat="server" Width="203px" CssClass="Dropdownlist" >
                        </asp:DropDownList>
                    </td>
                    <td class="ColInfo">
                        เลขที่บัตร
                    </td>
                    <td>
                        <asp:TextBox ID="txtCitizenId" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="ColInfo">
                        ช่องทาง
                    </td>
                    <td class="ColInput">
                        <asp:DropDownList ID="cmbChannel" runat="server" Width="203px" CssClass="Dropdownlist"></asp:DropDownList>
                    </td>
                    <td class="ColInfo">
                        แคมเปญ
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCampaign" runat="server" Width="203px" 
                        CssClass="Dropdownlist" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="height:10px; vertical-align:bottom;">
                    </td>
                    <td colspan="3"></td>
                </tr>
            </table>
            <asp:LinkButton ID="lbAdvanceSearch" runat="server" ForeColor="Green" OnClientClick="DisplayProcessing()" 
                Text="[+] <b>Advance Search</b>"  onclick="lbAdvanceSearch_Click"></asp:LinkButton>
            <asp:TextBox ID="txtAdvanceSearch" runat="server" Text="N" Visible="false" ></asp:TextBox>
            <asp:Panel ID="pnAdvanceSearch" runat="server" style="display:none;" >
                <table cellpadding="2" cellspacing="0" border="0">
                    <tr><td colspan="4" style="height:8px;"></td></tr>
                    <tr>
                        <td class="ColInfo">
                            เลขที่สัญญาที่เคยมีกับธนาคาร
                        </td>
                        <td class="ColInput">
                            <asp:TextBox ID="txtContractNoRefer" runat="server" CssClass="Textbox" Width="200px" ></asp:TextBox>
                        </td>
                        <td class="ColInfo">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="ColInfo">
                            วันทีสร้าง Lead
                        </td>
                        <td class="ColInput">
                            <uc1:TextDateMask ID="tdmCreateDate" runat="server" Width="182px" />
                        </td>
                        <td class="ColInfo">
                            วันที่ได้รับมอบหมายล่าสุด
                        </td>
                        <td>
                            <uc1:TextDateMask ID="tdmAssignDate" runat="server" Width="182px" />
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
                </table><br />
                <table cellpadding="3" cellspacing="0" border="0">
                    <tr>
                        <td valign="top" class="ColInfo">
                                สถานะของ Lead
                        </td>
                        <td colspan="5">
                            &nbsp;<asp:CheckBox ID="cbOptionAll" runat="server" Text="ทั้งหมด" AutoPostBack="true" oncheckedchanged="cbOptionAll_CheckedChanged" />
                            <asp:CheckBoxList ID="cbOptionList" runat="server" RepeatLayout="Table" AutoPostBack="true" 
                                RepeatDirection="Horizontal" RepeatColumns="5" 
                                onselectedindexchanged="cbOptionList_SelectedIndexChanged"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="height:15px;">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel> --%>


    <%--<asp:UpdatePanel ID="upButton" runat="server" UpdateMode="Conditional">
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
    </asp:UpdatePanel>--%>
    <%--<br />
    <div class="Line"></div>
    <br />--%>
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Image ID="imgResult" runat="server" ImageUrl="~/Images/hResult.gif" ImageAlign="Top" />&nbsp;
            <asp:Button ID="btnAddRanking" runat="server" Text="เพิ่ม Ranking" Width="120px"
                CssClass="Button" Height="23px"  OnClick="btnAddRanking_Click" />
            <input type="button" id="btnEdit"  value="แก้ไข" style="width:120px; height:23px"
                class="Button"" />
            <asp:Button ID="btnSaveRanking" runat="server" style="display:none" Text="บันทึก" Width="120px"
                CssClass="Button" Height="23px" OnClick="btnSaveRanking_Click" />
            <asp:Button ID="btnCancel" runat="server" style="display:none" Text="ยกเลิก" Width="120px"
                CssClass="Button" Height="23px"  />
            <br />
            <br />
            <%--<uc2:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange" Width="2755px" />--%>
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" DataKeyNames="coc_RankingId"
                GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True"
                EmptyDataText="<span style='color:Red;'>ไม่พบข้อมูล</span>"
                AllowSorting="false" OnRowDataBound="gvResult_RowDataBound" Width="700px">
                <Columns>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <%--&nbsp;<asp:ImageButton ID="imbView" runat="server" ImageUrl="~/Images/view.gif" CommandArgument='<%# Eval("TicketId") %>' OnClick="imbView_Click" ToolTip="ดูรายละเอียดข้อมูลผู้มุ่งหวัง"  />--%>
                            <asp:ImageButton ID="imbEdit" runat="server" ImageUrl="~/Images/edit.gif" CommandArgument='<%# Eval("coc_RankingId") %>' OnClick="imbEdit_Click" ToolTip="แก้ไขข้อมูล Ranking" />
                            <input type="hidden" name="RankingId" value='<%# Eval("coc_RankingId") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="10px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderStyle Width="10px" HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="coc_Seq" HeaderText="ลำดับที่">
                        <HeaderStyle Width="10px" HorizontalAlign="Center" />
                        <ItemStyle Width="10px" HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="coc_Name" HeaderText="ชื่อลำดับ">
                        <HeaderStyle Width="300px" HorizontalAlign="Center" />
                        <ItemStyle Width="300px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="coc_UpdatedBy" HeaderText="ชื่อผู้ Update">
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="coc_UpdatedDate" HeaderText="Update ล่าสุด">
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                        <ItemStyle Width="100px" HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>

                </Columns>
                <HeaderStyle CssClass="t_rowhead" />
                <RowStyle CssClass="t_row" BorderStyle="Dashed" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
