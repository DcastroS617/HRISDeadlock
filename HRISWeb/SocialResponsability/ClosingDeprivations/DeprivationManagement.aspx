<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DeprivationManagement.aspx.cs" Inherits="HRISWeb.SocialResponsability.ClosingDeprivations.DeprivationManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server">
            <h1 class="text-left text-primary">
                <label class="PageTitle"><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></label>
            </h1>
            <br />

            <asp:UpdatePanel runat="server" ID="main">
                <ContentTemplate>

                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblInitiativeDetails") %></h4>
                        <br />
                        <div class="row">
                            <div class="col-sm-4">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtEmployeeCode.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblCodigoEmpleado") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtEmployeeCode" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtEmployeeName.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblNombreEmpleado") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtEmployeeName" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtDivisionName.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblDivision") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtDivisionName" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        
                            <div class="col-sm-4">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtCompanyName.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblCompania") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtCompanyName" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtCostMiniZoneName.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblFinca") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtCostMiniZoneName" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtEmployeeSeniority.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblAntiguedad") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtEmployeeSeniority" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        
                            <div class="col-sm-4">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtDeprivationScore.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblPuntajeDePrivacion") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtDeprivationScore" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=chkInPoverty.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblEnPobreza") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:CheckBox ID="chkInPoverty" CssClass="control-validation" runat="server"></asp:CheckBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-5 text-left">
                        <label for="<%=txtDeprivationName.ClientID%>" class="control-label">
                            <%= GetLocalResourceObject("lblCarencias") %>
                        </label>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtDeprivationName" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
                        </div>
                        <h4><%= GetLocalResourceObject("lblIndicadoresPorDimension") %></h4>
                        <div class="row">
    <div class="col-md-6" style="border-radius: 5px; border-style: solid; border-color: #f1f1f1;">
        <table class="table">
            <thead>
                <tr>
                    <th><%= GetLocalResourceObject("lblDimension") %></th>
                    <th><%= GetLocalResourceObject("lblIndicador") %></th>
                    <th><%= GetLocalResourceObject("lblIPMe") %></th>
                    <th><%= GetLocalResourceObject("lblActual") %></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td rowspan="5"><%= GetLocalResourceObject("lblSalud") %></td>
                    <td><%= GetLocalResourceObject("lblSalud1") %></td>
                    <td><asp:Label ID="lblSalud1IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblSalud1Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblSalud2") %></td>
                    <td><asp:Label ID="lblSalud2IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblSalud2Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblSalud3") %></td>
                    <td><asp:Label ID="lblSalud3IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblSalud3Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblSalud4") %></td>
                    <td><asp:Label ID="lblSalud4IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblSalud4Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblSalud5") %></td>
                    <td><asp:Label ID="lblSalud5IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblSalud5Manage" runat="server" /></td>
                </tr>

                <tr>
                    <td rowspan="3"><%= GetLocalResourceObject("lblEducacion") %></td>
                    <td><%= GetLocalResourceObject("lblEducacion1") %></td>
                    <td><asp:Label ID="lblEducacion1IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblEducacion1Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblEducacion2") %></td>
                    <td><asp:Label ID="lblEducacion2IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblEducacion2Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblEducacion3") %></td>
                    <td><asp:Label ID="lblEducacion3IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblEducacion3Manage" runat="server" /></td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="col-md-6" style="border-radius: 5px; border-style: solid; border-color: #f1f1f1;">
        <table class="table">
            <thead>
                <tr>
                    <th><%= GetLocalResourceObject("lblDimension") %></th>
                    <th><%= GetLocalResourceObject("lblIndicador") %></th>
                    <th><%= GetLocalResourceObject("lblIPMe") %></th>
                    <th><%= GetLocalResourceObject("lblActual") %></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td rowspan="5"><%= GetLocalResourceObject("lblVivienda") %></td>
                    <td><%= GetLocalResourceObject("lblVivienda1") %></td>
                    <td><asp:Label ID="lblVivienda1IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblVivienda1Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblVivienda2") %></td>
                    <td><asp:Label ID="lblVivienda2IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblVivienda2Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblVivienda3") %></td>
                    <td><asp:Label ID="lblVivienda3IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblVivienda3Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblVivienda4") %></td>
                    <td><asp:Label ID="lblVivienda4IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblVivienda4Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblVivienda5") %></td>
                    <td><asp:Label ID="lblVivienda5IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblVivienda5Manage" runat="server" /></td>
                </tr>

                <tr>
                    <td rowspan="3"><%= GetLocalResourceObject("lblEmpleo") %></td>
                    <td><%= GetLocalResourceObject("lblEmpleo1") %></td>
                    <td><asp:Label ID="lblEmpleo1IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblEmpleo1Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblEmpleo2") %></td>
                    <td><asp:Label ID="lblEmpleo2IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblEmpleo2Manage" runat="server" /></td>
                </tr>
                <tr>
                    <td><%= GetLocalResourceObject("lblEmpleo3") %></td>
                    <td><asp:Label ID="lblEmpleo3IPMe" runat="server" /></td>
                    <td><asp:Label ID="lblEmpleo3Manage" runat="server" /></td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 text-center" style="background-color: #808080; height: 2px">
                        </div>
                    </div>
                    <br />

                    <h4><%= GetLocalResourceObject("lblCarencias") %></h4>

                    <ul class="nav nav-tabs">
                        <li class="active"><a data-toggle="tab" href="#home"><%= GetLocalResourceObject("lblIndividuos") %></a></li>
                        <li><a data-toggle="tab" href="#menu1"><%= GetLocalResourceObject("lblHogar") %></a></li>
                    </ul>

                    <div class="tab-content">
                            <div id="home" class="tab-pane fade in active">

                                <div class="row">
                                    <div class="col-sm-12 text-center">
                                        <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                                        <asp:HiddenField runat="server" ID="hdfSelectedFileIndex" Value="" />
                                        <asp:HiddenField runat="server" ID="hdfSelectedFileValues" Value="" />
                                        <asp:HiddenField runat="server" ID="hdfCurrentUser" Value="" />

                                        <div>

                                            <asp:GridView ID="grvList"
                                                Width="100%"
                                                runat="server"
                                                EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                                EmptyDataRowStyle-CssClass="emptyRow"
                                                AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                                AutoGenerateColumns="false" ShowHeader="true"
                                                CssClass="table table-striped table-hover table-bordered"
                                                DataKeyNames="DeprivationCode"
                                                OnRowCommand="grvList_RowCommand"
                                                OnPreRender="GrvList_PreRender" OnSorting="GrvList_Sorting">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DeprivationCodeSort" runat="server" CommandName="Sort" CommandArgument="DeprivationCode" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("lblDeprivationCode") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DeprivationCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDeprivationCode" data-id="PrincipalDeprivationCode" data-value="<%# Eval("DeprivationCode") %>"><%# Eval("DeprivationCode") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="IndividualCodeSort" runat="server" CommandName="Sort" CommandArgument="IndividualCode" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("IndividualCode") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IndividualCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvIndividualCode" data-id="PrincipalIndividualCode" data-value="<%# Eval("IndividualCode") %>"><%# Eval("IndividualCode") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="IndividualNameSort" runat="server" CommandName="Sort" CommandArgument="IndividualName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("IndividualName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IndividualName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvIndividualName" data-id="PrincipalIndividualName" data-value="<%# Eval("IndividualName") %>"><%# Eval("IndividualName") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="AgeSort" runat="server" CommandName="Sort" CommandArgument="Age" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("Age") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Age") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvAge" data-id="PrincipalAge" data-value="<%# Eval("Age") %>"><%# Eval("Age") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DimensionSort" runat="server" CommandName="Sort" CommandArgument="Dimension" OnClientClick="SetWaitingGrvList(true);">          
                                                            <span><%= GetLocalResourceObject("Dimension") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "Dimension") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDimension" data-id="PrincipalDimension" data-value="<%# Eval("Dimension") %>"><%# Eval("Dimension") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="IndicatorNameSort" runat="server" CommandName="Sort" CommandArgument="IndicatorName" OnClientClick="SetWaitingGrvList(true);">          
                                                        <span><%= GetLocalResourceObject("IndicatorName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "IndicatorName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvIndicatorName" data-id="PrincipalIndicatorName" data-value="<%# Eval("IndicatorName") %>"><%# Eval("IndicatorName") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText='<%$ Code:GetLocalResourceObject("lblActions") %>'>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" class="btn btn-default btnAjaxAction" Text='<%$ Code:GetLocalResourceObject("lblDeprivationManagement") %>' CommandName="ViewDetails" CommandArgument='<%# Eval("IndividualCode") %>'
                                                                OnClientClick='<%# "showDeprivationProceduresDialog(\"" + Eval("DeprivationCode") + "\", this); return false;" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>


                                            <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                                <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                            </div>
                                        </div>

                                        <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>

                                        <nav>
                                            <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPager_Click">
                                            </asp:BulletedList>
                                        </nav>
                                    </div>
                                </div>

                            </div>
                            <div id="menu1" class="tab-pane fade">
                                <div class="row">
                                    <div class="col-sm-12 text-center">
                                        <asp:HiddenField runat="server" ID="HiddenField1" Value="-1" />
                                        <asp:HiddenField runat="server" ID="HiddenField2" Value="" />
                                        <asp:HiddenField runat="server" ID="HiddenField3" Value="" />
                                        <asp:HiddenField runat="server" ID="HiddenField4" Value="" />

                                        <div>

                                            <asp:GridView ID="grvListHousehold"
                                                Width="100%"
                                                runat="server"
                                                EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                                EmptyDataRowStyle-CssClass="emptyRow"
                                                AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                                AutoGenerateColumns="false" ShowHeader="true"
                                                CssClass="table table-striped table-hover table-bordered"
                                                DataKeyNames="DeprivationCode"
                                                OnRowCommand="grvListHousehold_RowCommand"
                                                OnPreRender="grvListHousehold_PreRender" OnSorting="grvListHousehold_Sorting">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DeprivationCodeHouseholdSort" runat="server" CommandName="Sort" CommandArgument="DeprivationCode" OnClientClick="SetWaitingGrvList(true);">          
                                        <span><%= GetLocalResourceObject("lblDeprivationCode") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvList.ClientID, "DeprivationCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDeprivationCodeHousehold" data-id="PrincipalDeprivationCode" data-value="<%# Eval("DeprivationCode") %>"><%# Eval("DeprivationCode") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="IndividualCodeSort" runat="server" CommandName="Sort" CommandArgument="IndividualCode" OnClientClick="SetWaitingGrvListHousehold(true);">          
                                        <span><%= GetLocalResourceObject("IndividualCode") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListHousehold.ClientID, "IndividualCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvIndividualCodeHousehold" data-id="PrincipalIndividualCode" data-value="<%# Eval("IndividualCode") %>"><%# Eval("IndividualCode") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="IndividualNameSort" runat="server" CommandName="Sort" CommandArgument="IndividualName" OnClientClick="SetWaitingGrvListHousehold(true);">          
                                        <span><%= GetLocalResourceObject("IndividualName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListHousehold.ClientID, "IndividualName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvIndividualNameHousehold" data-id="PrincipalIndividualName" data-value="<%# Eval("IndividualName") %>"><%# Eval("IndividualName") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="AgeSort" runat="server" CommandName="Sort" CommandArgument="Age" OnClientClick="SetWaitingGrvListHousehold(true);">          
                                        <span><%= GetLocalResourceObject("Age") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListHousehold.ClientID, "Age") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvAgeHousehold" data-id="PrincipalAge" data-value="<%# Eval("Age") %>"><%# Eval("Age") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DimensionSort" runat="server" CommandName="Sort" CommandArgument="Dimension" OnClientClick="SetWaitingGrvListHousehold(true);">          
                                        <span><%= GetLocalResourceObject("Dimension") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListHousehold.ClientID, "Dimension") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDimensionHousehold" data-id="PrincipalDimension" data-value="<%# Eval("Dimension") %>"><%# Eval("Dimension") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="IndicatorNameSort" runat="server" CommandName="Sort" CommandArgument="IndicatorName" OnClientClick="SetWaitingGrvListHousehold(true);">          
                                        <span><%= GetLocalResourceObject("IndicatorName") %></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListHousehold.ClientID, "IndicatorName") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvIndicatorNameHousehold" data-id="PrincipalIndicatorName" data-value="<%# Eval("IndicatorName") %>"><%# Eval("IndicatorName") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText='<%$ Code:GetLocalResourceObject("lblActions") %>'>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" class="btn btn-default btnAjaxAction" Text='<%$ Code:GetLocalResourceObject("lblDeprivationManagement") %>' CommandName="ViewDetails" CommandArgument='<%# Eval("IndividualCode") %>'
                                                                            OnClientClick='<%# "showDeprivationProceduresHouseholdDialog(\"" + Eval("DeprivationCode") + "\", this); return false;" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>


                                            <div id="grvListHouseholdWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                                <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                            </div>
                                        </div>

                                        <h4 class="text-left text-primary" id="htmlResultsSubtitleHousehold" runat="server"></h4>

                                        <nav>
                                            <asp:BulletedList ID="blstPagerHousehold" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPagerHousehold_Click">
                                            </asp:BulletedList>
                                        </nav>
                                    </div>
                                </div>

                            </div>
                        </div>


                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <div class="ButtonsActions">
            <asp:UpdatePanel ID="uppActions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <Triggers>
                </Triggers>
                <ContentTemplate>
                    <div class="btn-group" role="group" aria-label="main-buttons">
                        <button id="btnAdd" type="button" style="display: none" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="BtnAdd_ServerClick" onclick="return ProcessAddRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSave"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnAdd"))%>'>
                            <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnSave") %>
                        </button>
                        <button id="btnEdit" type="button" runat="server" style="display: none" class="btn btn-default btnAjaxAction btns" onserverclick="btnEdit_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnCancel"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnCancel") %>
                        </button>
                        <button id="btnEditHousehold" type="button" runat="server" style="display: none" class="btn btn-default btnAjaxAction btns" onserverclick="btnEditHousehold_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnCancel"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEdit"))%>'>
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnCancel") %>
                        </button>
                        <button id="btnEditProcedure" type="button" style="display:none" runat="server" class="btn btn-default btnAjaxAction btns" onserverclick="btnEditProcedure_ServerClick" onclick="return ProcessEditRequest(this.id);" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnCancel"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnEditProcedure"))%>'>
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnCancel") %>
                        </button>
                        <button id="btnDelete"  type="button" runat="server" class="btn btn-default btns" onserverclick="btnDelete_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnDelete"))%>'>
                            <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                            <br />
                            <%= GetLocalResourceObject("btnCancel") %>
                        </button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hndInitiativeCode" Value="-1" />
    <%--  Modal for Add and Edit Deprivations--%>
    <div class="modal fade" id="MaintenanceDialog" tabindex="-1" role="dialog" aria-labelledby="MaintenanceDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog  " style="width: 1024px" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnClose" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitle"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="uppDialogControl">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfDeprivationCode" runat="server" Value="" />

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtEmployeeCodeDlg.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeCode")%></label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEmployeeCodeDlg" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtIndividualNameDlg.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIndividualName")%></label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtIndividualNameDlg" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtEmployeeNameDlg.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblEmployeeName")%></label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtEmployeeNameDlg" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtIndicatorNameDlg.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIndicator")%></label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtIndicatorNameDlg" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-5 text-left">
                                                    <label for="<%=txtStatusDlg.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblStatus")%></label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtStatusDlg" CssClass="form-control control-validation" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12 text-center">
                                        <asp:HiddenField runat="server" ID="HiddenField5" Value="-1" />
                                        <asp:HiddenField runat="server" ID="HiddenField6" Value="" />
                                        <asp:HiddenField runat="server" ID="HiddenField7" Value="" />
                                        <asp:HiddenField runat="server" ID="HiddenField8" Value="" />

                                        <div>

                                            <asp:GridView ID="grvListDeprivationProcedures"
                                                Width="100%"
                                                runat="server"
                                                EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'
                                                EmptyDataRowStyle-CssClass="emptyRow"
                                                AllowPaging="false" PagerSettings-Visible="false" AllowSorting="true"
                                                AutoGenerateColumns="false" ShowHeader="true"
                                                CssClass="table table-striped table-hover table-bordered"
                                                DataKeyNames="DeprivationManagementCode"
                                                OnRowCommand="grvListDeprivationProcedures_RowCommand"
                                                OnPreRender="grvListDeprivationProcedures_PreRender" OnSorting="grvListDeprivationProcedures_Sorting">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">

                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DeprivationManagementCodeSort" runat="server" CommandName="Sort" CommandArgument="DeprivationManagementCode" OnClientClick="SetWaitinggrvListDeprivationProcedures(true);">

                <span><%=GetLocalResourceObject("lblDeprivationProcedureNumber")%></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListDeprivationProcedures.ClientID, "DeprivationManagementCode") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDeprivationManagementCode" data-id="PrincipalDeprivationManagementCode" data-value="<%# Eval("DeprivationManagementCode") %>"><%# Eval("DeprivationManagementCode") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DeprivationStatusDesSpanishSort" runat="server" CommandName="Sort" CommandArgument="DeprivationStatusDesSpanish" OnClientClick="SetWaitinggrvListDeprivationProcedures(true);">          
                <span><%=GetLocalResourceObject("lblDeprivationStatus")%></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListDeprivationProcedures.ClientID, "DeprivationStatusDesSpanish") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDeprivationStatusDesSpanish" data-id="PrincipalDeprivationStatusDesSpanish" data-value="<%# Eval("DeprivationStatusDesSpanish") %>"><%# Eval("DeprivationStatusDesSpanish") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DeprivationInstitutionDesSpanishSort" runat="server" CommandName="Sort" CommandArgument="DeprivationInstitutionDesSpanish" OnClientClick="SetWaitinggrvListDeprivationProcedures(true);">          
                <span><%=GetLocalResourceObject("lblDeprivationInstitution")%></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListDeprivationProcedures.ClientID, "DeprivationInstitutionDesSpanish") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDeprivationInstitutionDesSpanish" data-id="PrincipalDeprivationInstitutionDesSpanish" data-value="<%# Eval("DeprivationInstitutionDesSpanish") %>"><%# Eval("DeprivationInstitutionDesSpanish") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="DeprivationProcessDesSpanishSort" runat="server" CommandName="Sort" CommandArgument="DeprivationProcessDesSpanish" OnClientClick="SetWaitinggrvListDeprivationProcedures(true);">          
                <span><%=GetLocalResourceObject("lblDeprivationProcess")%></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListDeprivationProcedures.ClientID, "DeprivationProcessDesSpanish") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvDeprivationProcessDesSpanish" data-id="PrincipalDeprivationProcessDesSpanish" data-value="<%# Eval("DeprivationProcessDesSpanish") %>"><%# Eval("DeprivationProcessDesSpanish") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="RegisterDateSort" runat="server" CommandName="Sort" CommandArgument="RegisterDate" OnClientClick="SetWaitinggrvListDeprivationProcedures(true);">          
                <span><%=GetLocalResourceObject("lblRegisterDate")%></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListDeprivationProcedures.ClientID, "RegisterDate") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvRegisterDate" data-id="PrincipalRegisterDate" data-value="<%# Eval("RegisterDate") %>"><%# Eval("RegisterDate", "{0:dd/MM/yyyy}") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-CssClass="ItemSelector ItemStyle">
                                                        <HeaderTemplate>
                                                            <div style="width: 100%; text-align: center;">
                                                                <asp:LinkButton ID="NotesSort" runat="server" CommandName="Sort" CommandArgument="Notes" OnClientClick="SetWaitinggrvListDeprivationProcedures(true);">          
                <span><%=GetLocalResourceObject("lblNotes")%></span>&nbsp;<i class='fa <%= HRISWeb.Shared.CommonFunctions.GetSortDirectionStyle(Page.ClientID, grvListDeprivationProcedures.ClientID, "Notes") %> sorterDirection' aria-hidden="true" style="float: right;margin-right: -6px;margin-top: 4px;position: relative;z-index: 2;"></i>
                                                                </asp:LinkButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <span id="dvNotes" data-id="PrincipalNotes" width="200px" style="word-break:break-all;word-wrap:break-word;" data-value="<%# Eval("Notes") %>"><%# Eval("Notes") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText='<%$ Code:GetLocalResourceObject("lblActions") %>'>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkViewProcedures" runat="server" class="btn btn-default" Text='<%$ Code:GetLocalResourceObject("btnEdit") %>' CommandName="ViewDetails" CommandArgument='<%# Eval("IndividualCode") %>'
                                                                OnClientClick='<%# "showDeprivationProceduresEditDialog(\"" + Eval("DeprivationManagementCode") + "\"); return false;" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>


                                            <div id="grvListDeprivationProceduresWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                                <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                                            </div>
                                        </div>

                                        <h4 class="text-left text-primary" id="htmlResultsSubtitleDeprivationProcedure" runat="server"></h4>

                                        <nav>
                                            <asp:BulletedList ID="blstPagerDeprivationProcedure" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="BlstPagerDeprivationProcedure_Click">
                                            </asp:BulletedList>
                                        </nav>
                                    </div>
                                </div>

                            </div>
                            <div class="btn-group" role="group" aria-label="main-buttons">
                                <button id="btnAccept" type="button" runat="server" onserverclick="btnAccept_ServerClick" class="btn btn-primary btnAjaxAction"
                                        onclick="return ProcessAcceptRequest(this.id);"
                                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnAdd"))%>'
                                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnAdd"))%>'>
                                    <span class="glyphicon glyphicon-plus glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br /><%=GetLocalResourceObject("btnAdd")%>
                                </button>
                            </div>
                            <div class="modal-footer">


                                <button id="btnCancel" type="button" class="btn btn-default">
                                    <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                                </button>
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

   
    <div class="modal fade" id="DeprivationCRUDDialog" tabindex="-1" role="dialog" aria-labelledby="DeprivationCRUDDialogTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog  " style="width: 1024px" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCloseManagementProcedure" class="close" data-dismiss="modal2" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title text-primary" id="dialogTitleManagement"></h3>
                </div>

                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdfDeprivationManagementCode" runat="server" Value="" />

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%=cboDeprivationStatus.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDeprivationStatus")%></label>
                                                </div>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="cboDeprivationStatus" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                                    <label id="cboDeprivationStatusValidation" for="<%= cboDeprivationStatus.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDeprivationStatusValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%=cboDeprivationProcess.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDeprivationProcess")%></label>
                                                </div>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="cboDeprivationProcess" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                                    <label id="cboDeprivationProcessValidation" for="<%= cboDeprivationProcess.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDeprivationProcessValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%=cboDeprivationInstitution.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblDeprivationInstitution")%></label>
                                                </div>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="cboDeprivationInstitution" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                                    <label id="cboDeprivationInstitutionValidation" for="<%= cboDeprivationInstitution.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjDeprivationInstitutionValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="dtpRegisterDate" class="control-label"><%=GetLocalResourceObject("lblRegisterDate")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="dtpRegisterDate" class="form-control date control-validation cleanPasteDigits" placeholder='<%$ Code:GetLocalResourceObject("lblDateFormatPlaceHolder") %>' type="text" autocomplete="off" />
                                                        <label id="dtpRegisterDateValidation" for="<%= dtpRegisterDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjRegisterDateValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                        <div class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label for="<%=txtInvestedHours.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblInvestedHours")%></label>
                                                </div>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtInvestedHours"  onkeypress="return isNumber(event) && checkMaxLength(this,event,4);" CssClass="form-control control-validation cleanPasteDigits" min="0" max="9999" MaxLength="4" autocomplete="off"  runat="server"></asp:TextBox>
                                                    <label id="txtInvestedHoursValidation" for="<%= txtInvestedHours.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msgInvestedHoursValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <div class="col-sm-4 text-left">
                                                    <label runat="server" visible="false" for="<%=cboInitiative.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblInitiative")%></label>
                                                </div>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="cboInitiative" visible="false" CssClass="form-control control-validation selectpicker" AutoPostBack="false" runat="server" data-live-search="true" data-actions-box="true"></asp:DropDownList>
                                                        <label id="cboInitiativeValidation" for="<%= cboInitiative.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjCourseValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="col-sm-2 text-left">
                                                    <label for="<%=txtManagementNotes.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblNotes")%></label>
                                                </div>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtManagementNotes"   TextMode="MultiLine" Rows="5" CssClass="form-control control-validation" MaxLength="500" runat="server"></asp:TextBox>
                                                    <label id="txtManagementNotesValidation" for="<%= txtManagementNotes.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjManagementNotesValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </div>
                            <div class="btn-group" role="group" aria-label="main-buttons">
                                <button id="btnAcceptManagement" type="button" runat="server"  onserverclick="btnAcceptManagement_ServerClick"  class="btn btn-primary btnAjaxAction"
                                    onclick="return ProcessAcceptProcedureRequest(this.id);"
                                    >
                                    <span class="glyphicon glyphicon-floppy-disk glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>
                                    <br />
                                    <%= GetLocalResourceObject("btnSaveProcedure") %>
                                </button>
                            </div>
                            <div class="modal-footer">
                                <button id="btnCancelProcedure" type="button" class="btn btn-default">
                                    <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnDialogCancel")) %>
                                </button>
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
 

    <script type="text/javascript">
        //Variables for table ordening
        var dataSortAttribute, dataSortType, dataSortDirection;
        $(document).ready(function () {
        });

        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            /// We prefer to bind the events here over do it on document ready event in order to auto rebind the events in page and ajax request each time
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });
            //In this section we set the dropdown state (element) for cboAjaxAction
            $('.cboAjaxAction').on('change', function () {
                var $this = $(this);
                $this.siblings(".waitingNotification").show();
                SetWaitingGrvList(true);
            });
            //And the grvList selection row functionality
            $('#<%= grvList.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    console.log(this);
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);
                }
            });
            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList(true);
            });

            //others buttons 
            $('#<%= btnAdd.ClientID %>').on('click', function (ev) {
                /// <summary>Handles the click event for button add.</summary>
                ev.preventDefault();
                setTimeout(function () {
                    $("#<%=btnAdd.ClientID%>").button('reset');
                }, 500);
                DisableToolBar();
                ClearModalForm();
                $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceAdd") %>');
                $('#MaintenanceDialog').modal('show');
                EnableToolBar();
                return false;
            });
            $('#btnCancel, #btnClose').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                ClearModalForm();
                DisableButtonsDialog();
                $('#MaintenanceDialog').modal('hide');
                EnableButtonsDialog();
            });

            $('#btnCancelProcedure, #btnCloseManagementProcedure').on('click', function (event) {
                /// <summary>Handles the click event for button cancel in user dialog.</summary>            
                event.preventDefault();
                ClearModalForm();
                DisableButtonsDialog();
                $('#DeprivationCRUDDialog').modal('hide');
                EnableButtonsDialog();
            });

            //And the clean paste manager
            $('.cleanPasteText').on('paste', function (e) {
                var $this = $(this);
                setTimeout(function (e) {
                    replacePastedInvalidCharacters($this);
                    var ml = $this.attr("maxlength");
                    if (ml) {
                        checkMaxLength($this[0], e, parseInt(ml));
                    }
                }, 50);
            });
            //And the clean paste manager
            $('.cleanPasteDigits').on('paste', function (e) {
                var $this = $(this);
                setTimeout(function (e) {
                    replacePastedInvalidDigits($this);
                    var ml = $this.attr("maxlength");
                    if (ml) {
                        checkMaxLength($this[0], e, parseInt(ml));
                    }
                }, 50);
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we initialize the sorter elements for tables.

            $(".sorter").click(function () {
                var $table = $(this).closest('table');
                var $dataSortSrc = $(this).find()
                dataSortAttribute = $(this).attr("data-sort-attr");
                dataSortType = $(this).attr("data-sort-type");
                dataSortDirection = $(this).attr("data-sort-direction");

                if (isEmptyOrSpaces(dataSortDirection)) {
                    dataSortDirection = "0";
                }
                else if (dataSortDirection == "0") {
                    dataSortDirection = "1";
                }
                else if (dataSortDirection == "1") {
                    dataSortDirection = "0";
                }
                $table.children('tbody').sortChildren(function (A, B) {
                    var a, b;
                    if (dataSortType == "int") {
                        a = parseInt($(A).find(".data-sort-src").attr(dataSortAttribute));
                        b = parseInt($(B).find(".data-sort-src").attr(dataSortAttribute));
                    }
                    if (dataSortType == "string") {
                        a = $(A).find(".data-sort-src").attr(dataSortAttribute);
                        b = $(B).find(".data-sort-src").attr(dataSortAttribute);
                    }
                    var orderA, orderB;
                    if (dataSortDirection == "0") {
                        orderA = -1;
                        orderB = 1;
                    }
                    else if (dataSortDirection == "1") {
                        orderA = 1;
                        orderB = -1;
                    }
                    return a < b ? orderA : a > b ? orderB : 0;
                });

                $table.find(".sorter").each(function () {
                    $(this).attr("data-sort-direction", "");
                    $(this).find(".sorterDirection").removeClass("fa-sort-asc");
                    $(this).find(".sorterDirection").removeClass("fa-sort-desc");
                });

                $(this).attr("data-sort-direction", dataSortDirection);
                if (dataSortDirection == "0") {
                    $(this).find(".sorterDirection").addClass("fa-sort-asc");
                }
                else if (dataSortDirection == "1") {
                    $(this).find(".sorterDirection").addClass("fa-sort-desc");
                }
            });

            $('#<%= dtpRegisterDate.ClientID %>').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the components enabled/disabled according to hdfIsFormEnabled indicator
            //Special attention to client side components that can not be enabled/disabled from server side
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //Other executions
            //each time a ajax or page load execue we need to synch the selected row with its value
            SetRowSelected();
        }

        function showDeprivationProceduresDialog(deprivationCode, ctrllnk) {
            //alert(deprivationCode);

            var row = ctrllnk.parentNode.parentNode;
            console.log(row);
            console.log(ctrllnk.parentNode);
            //console.log(row.index());

            if (!$(row).hasClass('emptyRow')) {
                console.log(row);
                $(row).toggleClass('info').siblings().removeClass('info');
                $('#<%=hdfSelectedRowIndex.ClientID%>').val($(row).hasClass("info") ? $(row).index() : -1);
            }


            $("#<%=hdfDeprivationCode.ClientID%>").val(deprivationCode);


            __doPostBack('<%= btnEdit.UniqueID %>', '');

        }

        function showDeprivationProceduresHouseholdDialog(deprivationCode, ctrllnk) {

            var row = ctrllnk.parentNode.parentNode;

            if (!$(row).hasClass('emptyRow')) {
                console.log(row);
                $(row).toggleClass('info').siblings().removeClass('info');
                $('#<%=hdfSelectedRowIndex.ClientID%>').val($(row).hasClass("info") ? $(row).index() : -1);
            }

            $("#<%=hdfDeprivationCode.ClientID%>").val(deprivationCode);

            __doPostBack('<%= btnEditHousehold.UniqueID %>', '');
        }

        function showDeprivationProceduresDialog(deprivationCode) {
            $("#<%=hdfDeprivationCode.ClientID%>").val(deprivationCode);
            __doPostBack('<%= btnEdit.UniqueID %>', '');
        }

        function showDeprivationProceduresEditDialog(deprivationManagementCode) {
            console.log(deprivationManagementCode);
            $("#<%=hdfDeprivationManagementCode.ClientID%>").val(deprivationManagementCode);
            __doPostBack('<%= btnEditProcedure.UniqueID %>', '');
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validator = null;
        function ValidateForm() {
            jQuery.validator.addMethod("validSelect", function (value, element) {
                return ValidateSelect(value);
            }, "Please Select a valid information");

            jQuery.validator.addMethod("validDate", function (value, element) {
                return this.optional(element) || moment(value, "MM/DD/YYYY").isValid();
            }, "Please enter a valid date in the format MM/DD/YYYY");

            if (validator != null) {
                validator.destroy();
            }

            validator = $('#' + document.forms[0].id).validate({
                debug: true,
                highlight: function (element, errorClass, validClass) {
                    SetControlInvalid($(element).attr('id'));
                },

                unhighlight: function (element, errorClass, validClass) {
                    SetControlValid($(element).attr('id'));
                },

                errorPlacement: function (error, element) { },
                rules: {

                }
            });

            var result = validator.form();
            return result;
        }

        //*******************************//
        //          VALIDATION           // 
        //*******************************//
        var validatorProcedure = null;
        function ValidateProcedureForm() {
            jQuery.validator.addMethod("validSelect", function (value, element) {
                return ValidateSelect(value);
            }, "Please Select a valid information");

            jQuery.validator.addMethod("validDate", function (value, element) {
                return this.optional(element) || moment(value, "MM/DD/YYYY").isValid();
            }, "Please enter a valid date in the format MM/DD/YYYY");

            if (validatorProcedure != null) {
                validatorProcedure.destroy();
            }

            validatorProcedure = $('#' + document.forms[0].id).validate({
                debug: true,
                highlight: function (element, errorClass, validClass) {
                    SetControlInvalid($(element).attr('id'));
                },

                unhighlight: function (element, errorClass, validClass) {
                    SetControlValid($(element).attr('id'));
                },

                errorPlacement: function (error, element) { },
                rules: {
                    "<%= cboDeprivationStatus.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= cboDeprivationProcess.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= cboDeprivationInstitution.UniqueID %>": {
                        required: true,
                        validSelect: true
                    },
                    "<%= txtManagementNotes.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        },
                        minlength: 1,
                        maxlength: 500
                    },
                    "<%= dtpRegisterDate.UniqueID %>": {
                        required: true,
                        validDate: true
                    },
                    "<%= txtInvestedHours.UniqueID%>": {
                        required: true,
                        normalizer: function (value) {
                            return $.trim(value);
                        }
                    }
                }
            });

            var result = validatorProcedure.form();
            return result;
        }

        function SetControlInvalid(controlId) {
            /// <summary>Set the appearance of the control as invalid </summary>
            /// <param name="controlId" type="String">Id of the control</param>            
            $('#' + controlId).addClass("Invalid");
            $('label[for=' + controlId + '].label-validation').show();
        }
        function SetControlValid(controlId) {
            /// <summary>Set the appearance of the control as valid </summary>
            /// <param name="controlId" type="String">Id of the control</param>                        
            $('#' + controlId).removeClass("Invalid");
            $('label[for=' + controlId + '].label-validation').hide();
        }
        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }
        function SetRowSelected() {
            /// <summary>Set the class of the selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                var selectedRow = $('#<%= grvList.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
                console.log(selectedRow);
                selectedRow.siblings().removeClass('info');
                if (!selectedRow.hasClass('info')) {
                    selectedRow.addClass('info');
                }
            }

            console.log(selectedRowIndex);
        }
        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvList.ClientID %> tbody tr').removeClass('info');
        }
        //*******************************//
        //             LOGIC             // 
        //*******************************//                
        function ClearModalForm() {
            /// <summary>Clear the modal form</summary>                        

        }
        function SetWaitingGrvList(flag) {
            /// <summary>Process the request of set the grid as waiting style</summary>
            if (flag) {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvList.ClientID %>').fadeTo('fast', 0.5);
            } else {
                $('#<%= grvList.ClientID %>').find("input,button,textarea,select").removeAttr("disabled");
                $('#grvListWaiting').fadeOut('fast');
                $('#<%= grvList.ClientID %>').fadeIn('fast', 0);
                $('#<%= grvList.ClientID %>').removeAttr("style opacity");
            }
        }
        function ProcessEditRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);
            __doPostBack('<%= btnEdit.UniqueID %>', '');
            return true;
        }

        function ProcessAcceptRequest(ctrlId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 
            $(".btns").prop("disabled", true);
            $("#<%=hdfDeprivationManagementCode.ClientID%>").val("-1");
            __doPostBack('<%= btnAccept.UniqueID %>', '');
            return true;
        }

        function ProcessAddRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 

            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            //disableButton($('#btnCancel'));
            if (!ValidateForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    enableButton($('#btnCancel'));
                }, 150);
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', null);
            }
            else {
                __doPostBack('<%= btnAdd.UniqueID %>', '');
            }
            return false;
        }

        function ProcessAcceptProcedureRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param> 

            /// <summary>Process the accept request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            //disableButton($('#btnCancel'));
            console.log('Accept');
            if (!ValidateProcedureForm()) {
                setTimeout(function () {
                    ResetButton(resetId);
                    //enableButton($('#btnCancel'));
                }, 150);
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', null);
            }
            else {
                __doPostBack('<%= btnAcceptManagement.UniqueID %>', '');
            }
            return false;
        }

        function ProcessDeleteRequest(resetId) {
            /// <summary>Process the delete request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            return true;
        }
        function CloseUserDialog() {
            /// <summary>Close a user dialog.</summary>
            $('#MaintenanceDialog').modal('hide');
        }
        function DisableToolBar() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#<%= btnAdd.ClientID %>'));
            disableButton($('#<%= btnEdit.ClientID %>'));
            disableButton($('#<%= btnDelete.ClientID %>'));
        }
        function EnableToolBar() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#<%= btnAdd.ClientID %>'));
            enableButton($('#<%= btnEdit.ClientID %>'));
            enableButton($('#<%= btnDelete.ClientID %>'));
        }
        function DisableButtonsDialog() {
            ///<summary>Disables all buttons toolbar</summary>
            disableButton($('#btnAccept'));
            disableButton($('#btnCancel'));
        }
        function EnableButtonsDialog() {
            ///<summary>Enabled all buttons toolbar</summary>
            enableButton($('#btnAccept'));
            enableButton($('#btnCancel'));
        }
        function ValidateSelect(value) {

            if (value != "-1") {
                return true;
            }

            return false;
        }
        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//     
        function ReturnRequestBtnEditOpen() {
            //SetRowSelected();
            DisableToolBar();

            var deprivationCode = $("#<%=hdfDeprivationCode.ClientID%>").val()

            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("lblProceduresList") %> ' + deprivationCode);
            $('#dialogTitle').html('<%= GetLocalResourceObject("lblProceduresList") %> ' + deprivationCode);
            $('#MaintenanceDialog').modal('show');

            EnableToolBar();
        }

        function ReturnRequestBtnEditProcedureOpen() {
            //SetRowSelected();
            DisableToolBar();

            var deprivationCode = $("#<%=hdfDeprivationCode.ClientID%>").val();

            var deprivationManagementCode = $("#<%=hdfDeprivationManagementCode.ClientID%>").val()

            $('#DeprivationCRUDDialogTitle').html('<%= GetLocalResourceObject("lblProceduresList") %> ' + deprivationCode);
            $('#dialogTitle').html('<%= GetLocalResourceObject("lblProceduresList") %> ' + deprivationCode);

            if (deprivationManagementCode != "-1") {
                $('#dialogTitleManagement').html('<%= GetLocalResourceObject("lblProcedureNumber") %> ' + deprivationManagementCode);
            }
            else {
                $('#dialogTitleManagement').html('<%= GetLocalResourceObject("lblNewProcedure") %> ');
            }
            
            $('#DeprivationCRUDDialog').modal('show');
            

            //lblProcedureNumber

            EnableToolBar();           
        }

        function ReturnRequestbtnAccept() {
            //SetRowSelected();
            DisableToolBar();

            var deprivationCode = $("#<%=hdfDeprivationCode.ClientID%>").val();

            var deprivationManagementCode = $("#<%=hdfDeprivationManagementCode.ClientID%>").val()

            $('#DeprivationCRUDDialogTitle').html('<%= GetLocalResourceObject("lblProceduresList") %> ' + deprivationCode);
            $('#dialogTitle').html('<%= GetLocalResourceObject("lblProceduresList") %> ' + deprivationCode);

            if (deprivationManagementCode != "-1") {
                $('#dialogTitleManagement').html('<%= GetLocalResourceObject("lblProcedureNumber") %> ' + deprivationManagementCode);
            }
            else {
                $('#dialogTitleManagement').html('<%= GetLocalResourceObject("lblNewProcedure") %> ');
            }
            
            $('#DeprivationCRUDDialog').modal('show');           

            EnableToolBar();
        }

        function ReturnFromBtnEditClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled", false);
        }

        function ReturnFromBtnEditProcedureClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $(".btns").prop("disabled", true);
            SetRowSelected();
            DisableToolBar();
            $('#MaintenanceDialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#dialogTitle').html('<%= GetLocalResourceObject("DialogMaintenanceEdit") %>');
            $('#MaintenanceDialog').modal('show');
            EnableToolBar();
            $(".btns").prop("disabled", false);
        }

        function ReturnFromBtnAcceptClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            DisableButtonsDialog();
            SetRowSelected();

            $('#MaintenanceDialog').modal('hide');
            EnableButtonsDialog();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }
        function ReturnFromBtnAcceptActivateDeletedClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            SetRowSelected();
            DisableButtonsDialog();
            $('#ActivateDeletedDialog').modal('hide');
            $('#MaintenanceDialog').modal('hide');
            EnableButtonsDialog();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
        }
        function ReturnFromBtnAcceptClickPostBackDeleted() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');
            $('#ActivateDeletedDialog').modal('show');

        }
        function ReturnFromBtnAcceptClickPostBackDuplicated() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            $('#MaintenanceDialog').modal('hide');
            $('#DuplicatedDialog').modal('show');

        }
        function ReturnFromBtnDeleteClickPostBack() {
            /// <summary>Manage the events, ui and logic after the postback</summary>
            UnselectRow();
            ShowFooterSuccess('<%= GetLocalResourceObject("msgOperationSuccesfull") %>');
            EnableToolBar();
        }
        function ReturnDeprivationManagementResult() {
            //$('#MaintenanceDialog').modal('show'); 
            $('#DeprivationCRUDDialog').modal('hide');
        }
        //*******************************//
        // MESSAGING AND CONFIRMATION    // 
        //*******************************//
        function ShowConfirmationMessageDelete(resetId) {
            /// <summary>Show confirmation message for Save and Close funtionality</summary>
            /// <param name="resetId" type="String">Id of the button to reset if cancel</param>
            /// <returns>False</returns>
            MostrarConfirmacion(
                '<%= GetLocalResourceObject("msjDelete") %>'
                , '<%=GetLocalResourceObject("Yes")%>'
                , function () {
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                }
                , '<%=GetLocalResourceObject("No")%>'
                , function () {
                    $("#" + resetId).button('reset');
                }
            );
            return false;
        }
        //*******************************//
        //AJAX CONCURRENCY ADMINISTRATION// 
        //*******************************//
        //In this section we set the control for multiple simultaneous ajax request
        //We cancel the ajax request executed when another one is in progress
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endingRequest);


        function initializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);
                prm.abortPostBack();
                ShowFooterAlert('<%=GetLocalResourceObject("msgWaitWhileProcessing")%>');
                AddTemporaryClass($("#" + args.get_postBackElement().id), "btn-warning", 1500);
                setTimeout(function () {
                    $("#" + args.get_postBackElement().id).button('error');
                    setTimeout(function () {
                        $("#" + args.get_postBackElement().id).button('reset');
                    }, 1500)
                }, 100);
            }
        }

        function endingRequest(sender, args) {
            if (prm.get_isInAsyncPostBack()) {
                SetWaitingGrvList(false)
            }
        }
    </script>
</asp:Content>
