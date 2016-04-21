<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab006.ascx.cs" Inherits="COC.Application.Shared.Tabs.Tab006" %>
<%@ Register src="../GridviewPageController.ascx" tagname="GridviewPageController" tagprefix="uc1" %>

<div style="font-family:Tahoma; font-size:13px;">
    <asp:UpdatePanel ID="upResult" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <asp:TextBox ID="txtCitizenId" runat="server" Visible="false" ></asp:TextBox>
            <uc1:GridviewPageController ID="pcTop" runat="server" OnPageChange="PageSearchChange" Width="1170px" />
            <asp:GridView ID="gvExistProduct" runat="server" AutoGenerateColumns="False"
                GridLines="Horizontal" BorderWidth="0px" EnableModelValidation="True" 
                EmptyDataText="<center><span style='color:Red;'>ไม่พบข้อมูล</span></center>"  >
                <Columns>
                <asp:BoundField DataField="No" HeaderText="No"  >
                    <HeaderStyle Width="40px" HorizontalAlign="Center"/>
                    <ItemStyle Width="40px" HorizontalAlign="Center"  />
                </asp:BoundField>
                <asp:BoundField DataField="CitizenId" HeaderText="รหัสบัตรประชาชน"  >
                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                    <ItemStyle Width="120px" HorizontalAlign="Center"  />
                </asp:BoundField>
                <asp:BoundField DataField="ProductGroup" HeaderText="Group Product" >
                    <HeaderStyle Width="140px" HorizontalAlign="Center" />
                    <ItemStyle Width="140px" HorizontalAlign="Left"  />
                </asp:BoundField>
                <asp:BoundField DataField="ProductName" HeaderText="Product" >
                    <HeaderStyle Width="140px" HorizontalAlign="Center"/>
                    <ItemStyle Width="140px" HorizontalAlign="Left"  />
                </asp:BoundField>
                <asp:BoundField DataField="Grade" HeaderText="Grade">
                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                    <ItemStyle Width="100px" HorizontalAlign="Center"  />
                </asp:BoundField>
                <asp:BoundField DataField="ContactNo" HeaderText="Contract No">
                    <HeaderStyle Width="150px" HorizontalAlign="Center"/>
                    <ItemStyle Width="150px" HorizontalAlign="Left"  />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <%# Eval("StartDate") != null ? Convert.ToDateTime(Eval("StartDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("StartDate")).Year.ToString() : ""%>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                    <ItemStyle Width="100px" HorizontalAlign="Center"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <%# Eval("EndDate") != null ? Convert.ToDateTime(Eval("EndDate")).ToString("dd/MM/") + Convert.ToDateTime(Eval("EndDate")).Year.ToString() : ""%>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" HorizontalAlign="Center"/>
                    <ItemStyle Width="100px" HorizontalAlign="Center"  />
                </asp:TemplateField>
                <asp:BoundField DataField="PaymentTerm" HeaderText="ผ่อนชำระมาแล้วกี่งวด">
                    <HeaderStyle Width="130px" HorizontalAlign="Center"/>
                    <ItemStyle Width="130px" HorizontalAlign="Center"  />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status">
                    <HeaderStyle Width="120px" HorizontalAlign="Center"/>
                    <ItemStyle Width="120px" HorizontalAlign="Center"  />
                </asp:BoundField>
                
                </Columns>
                <HeaderStyle CssClass="t_rowhead" />
                <RowStyle CssClass="t_row" BorderStyle="Dashed"/>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>