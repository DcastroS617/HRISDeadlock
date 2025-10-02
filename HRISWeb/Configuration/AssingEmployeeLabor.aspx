<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AssingEmployeeLabor.aspx.cs" Inherits="HRISWeb.Configuration.AssingEmployeeLabor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        hr {
            border: 1px solid rgba(0,0,0,0.5);
            background-color: rgba(0,0,0,0.5);
        }

        .bootstrap-select .dropdown-toggle .filter-option {
            color: #333;
        }
    </style>

    <div class="main-content">
        <asp:Panel ID="pnlMainContent" runat="server" DefaultButton="btnSearchDefault">
            <h1 class="text-left text-primary">
                <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
            </h1>
            <br />

            <asp:UpdatePanel runat="server" ID="main">
                <Triggers>
                </Triggers>

                <ContentTemplate>
                    <div class="container" style="width: 100%">
                        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= EmployeeCodeFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("lblIdentificacion")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="EmployeeCodeFilter" type="text" class="form-control" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= EmployeeNameFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("NameLbl")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <input id="EmployeeNameFilter" type="text" class="form-control" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= CompaniesFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("CompaniesFilter")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="CompaniesFilter" class="form-control" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="DdlOnchange_CompaniesPayroll">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= PayrollClassFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("PayrollClassFilter")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="PayrollClassFilter" class="form-control" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="DdlOnchange_CompaniesPayroll2">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= CostCenterFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("CostCenterFilter")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select id="CostCenterFilter" runat="server" class="form-control">
                                                    <option value=""></option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= PositionFilter.ClientID%>" class="control-label"><%=GetLocalResourceObject("PositionFilter")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select id="PositionFilter" runat="server" class="form-control">
                                                    <option value=""></option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= RecruitmentDate.ClientID%>" class="control-label"><%=GetLocalResourceObject("FechaRecruitment")%></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <div class="input-group">
                                                    <asp:TextBox runat="server" ID="RecruitmentDate" class="dateinput form-control date control-validation cleanPasteDigits" type="text" autocomplete="off" />

                                                    <label id="dtpFechaHastalbl" for="<%= RecruitmentDate.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left"
                                                        style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">
                                                        !</label>

                                                    <div class="input-group-addon">
                                                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <button id="btnSearch" type="button" runat="server" class="btn btn-default btnAjaxAction" onserverclick="BtnSearch_ServerClick" data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                                </button>

                                <button id="btnSearchRefresh" type="button" runat="server" style="display: none;"
                                    onserverclick="BtnSearchRefresh_ServerClick">
                                </button>
                                <asp:Button ID="btnSearchDefault" runat="server" OnClick="BtnSearch_ServerClick" Style="display: none;" />
                            </div>
                        </div>
                        <hr />
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="row">
                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= LaborLevel.ClientID%>" class="control-label"><%= GetLocalResourceObject("lblAsignar") %></label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select id="LaborLevel" runat="server" class="form-control">
                                                    <option value="1">Labor 1</option>
                                                    <option value="2">Labor 2</option>
                                                    <option value="3">Labor 3</option>
                                                </select>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <label for="<%= LaborIdAsignar.ClientID%>" class="control-label">Labor</label>
                                            </div>

                                            <div class="col-sm-7">
                                                <select id="LaborIdAsignar" runat="server" class="form-control selectpicker" data-live-search="true"></select>
                                            </div>
                                        </div>

                                        <div class="form-group col-sm-12 col-md-4">
                                            <div class="col-sm-5 text-left">
                                                <button id="btnAsignarLabores" type="button" runat="server" class="btn btn-primary btnAjaxAction"
                                                    onclick="OnClickAsignarLabor()"
                                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("lblAplicar"))%>' data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("lblAplicar"))%>'>
                                                    <%= GetLocalResourceObject("lblAplicar") %>
                                                </button>
                                            </div>

                                            <div class="col-sm-7">
                                                <button id="btnExport" type="button" runat="server" class="btn btn-success btnAjaxAction" style="margin-left: 126px"
                                                    onclick="return ProcessDownloadRequest(this.id);"
                                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("lblExport"))%>'
                                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("lblExport"))%>'>
                                                    <%= GetLocalResourceObject("lblExport") %>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <br />

                        <div class="row" style="margin-top: 30px;">
                            <div class="form-group col-sm-12 col-md-5">
                                <input id="BuscarEmpleadoText" type="text" class="form-control"
                                    onkeyup="SearchTable()" placeholder="<%= GetLocalResourceObject("btnSearch") %>" value="" />
                            </div>

                            <div class="col-sm-12 col-md-7 text-right">
                                <%=GetLocalResourceObject("lblRegistrosSeleccionados").ToString() %> : <span class="recordselected">0</span>
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <table id="TableEmpleadoAsignar" class="table table-striped table-hover table-bordered">
                                    <thead>
                                        <tr>
                                            <th align="center">
                                                <input type="checkbox" class="form-control MarcarTodos" style="height: 15px; width: 15px; margin: auto;" />
                                            </th>

                                            <th><%=GetLocalResourceObject("lblIdentificacion")%></th>
                                            <th><%=GetLocalResourceObject("NameLbl")%> </th>
                                            <th>Labor 1</th>
                                            <th>Labor 2</th>
                                            <th>Labor 3</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        <% foreach (var item in AssingEmployeeLaborResults)
                                            { %>
                                        <tr>
                                            <td>
                                                <input type="checkbox" class="form-control empleadocheck" value="<%=item.EmployeeCode %>"
                                                    data-labor1="<%=item.Labor1.LaborName %>"
                                                    data-labor2="<%=item.Labor2.LaborName %>"
                                                    data-labor3="<%=item.Labor3.LaborName %>"
                                                    style="height: 15px; width: 15px; margin: auto;" />
                                            </td>
                                            <td><%=item.EmployeeCode %></td>
                                            <td><%=item.EmployeeName %></td>
                                            <td>
                                                <span id="<%=item.EmployeeCode %>_LABOR1">
                                                    <%=item.Labor1.LaborCode + " - " + item.Labor1.LaborName %>
                                                </span>
                                            </td>

                                            <td>
                                                <span id="<%=item.EmployeeCode %>_LABOR2">
                                                    <%=item.Labor2.LaborCode + " - " + item.Labor2.LaborName %>
                                                </span>
                                            </td>

                                            <td>
                                                <span id="<%=item.EmployeeCode %>_LABOR3">
                                                    <%=item.Labor3.LaborCode + " - " + item.Labor3.LaborName %>
                                                </span>
                                            </td>
                                        </tr>
                                        <%  } %>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />

    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>

    <script src="<%=ResolveUrl("~/Scripts/Excel/table2excel.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/Scripts/Excel/xlsx.full.min.js") %>" type="text/javascript"></script>

    <script type="text/javascript">   
        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//
        function pageLoad(sender, args) {
            //checkbox SearchEnabledEdit
            $('.selectpicker').selectpicker();

            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $('.dateinput').datetimepicker({
                format: 'MM/DD/YYYY'
            });

            $('.dateinput').on("blur", function () {
                var ValidDateVal = $(this).data("DateTimePicker").date().format('MM/DD/YYYY');
                $(this).val(ValidDateVal);
            });

            $(".MarcarTodos").click(function () {
                $(".empleadocheck:visible").prop("checked", $(this).is(":checked"));
                $(".recordselected").html($(".empleadocheck:checked").length);
            });

            $(".empleadocheck").change(function () {
                $(".recordselected").html($(".empleadocheck:checked").length);

                if ($(".empleadocheck:checked").length != $(".empleadocheck").length) {
                    $(".MarcarTodos").prop("checked", false);
                }
            });
        }

        //*******************************//
        //             LOGIC             //
        //*******************************//
        function OnClickAsignarLabor() {
            // Declare variables
            var isSelected, level, employeesSelected;

            document.getElementById("BuscarEmpleadoText").value = "";
            SearchTable();

            employeesSelected = [];
            isSelected = false;
            level = $("#<%=LaborLevel.ClientID%>").val();

            $('.empleadocheck:checked').each(function () {
                employeesSelected.push($(this).val());

                if ($(this).data("labor" + level) != "" && $(this).data("labor" + level) != null) {
                    isSelected = true
                }
            });

            var labor = $("#<%=LaborIdAsignar.ClientID%>").val();

            var isDupicated = CheckLaborDupicatedAsigned();

            if (employeesSelected.length < 1) {
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msjMarkEmployee").ToString()%>', null);
                ResetButton("<%=btnAsignarLabores.ClientID%>");
            }
            else {
                if (isSelected && !isDupicated) {
                    MostrarConfirmacion('<%=GetLocalResourceObject("msgLaboresEmpleadosAsignados")%>',
                            '<%=GetLocalResourceObject("Yes")%>', function () { SaveData(labor, level, employeesSelected); },
                            '<%=GetLocalResourceObject("No")%>', function () { ResetButton("<%=btnAsignarLabores.ClientID%>"); }
                    );
                }

                if (!isSelected && !isDupicated) {
                    SaveData(labor, level, employeesSelected);
                }
            }
        }

        function CheckLaborDupicatedAsigned() {
            // Declare variables
            var laborLevel, laborIdAsignar, isDupicated;

            laborLevel = $("#<%=LaborLevel.ClientID%>").val();
            laborAsignar = $("#<%=LaborIdAsignar.ClientID%> option:selected").text();

            isDupicated = false;

            // Loop through all table rows, and hide those who don't match the search query
            $('.empleadocheck:checked').each(function () {
                var labor1 = "#" + $(this).val().toString() + "_LABOR1";
                if ($(labor1)[0].outerText === laborAsignar && laborLevel !== 1) {
                    isDupicated = true;
                }

                var labor2 = "#" + $(this).val().toString() + "_LABOR2";
                if ($(labor2)[0].outerText === laborAsignar && laborLevel !== 2) {
                    isDupicated = true;
                }

                var labor3 = "#" + $(this).val().toString() + "_LABOR3";
                if ($(labor3)[0].outerText === laborAsignar && laborLevel !== 3) {
                    isDupicated = true;
                }
            });

            if (isDupicated) {
                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msjLaborDupicatedAsigned").ToString()%>', null);
            }

            ResetButton("<%=btnAsignarLabores.ClientID%>");
            return isDupicated;
        }

        function IsNullOrEmpty(eval) {
            return (eval == null || eval === "");
        }

        function SearchTable() {
            // Declare variables
            var input, filter, table, tr, td, i;

            input = document.getElementById("BuscarEmpleadoText");
            filter = input.value.toUpperCase();
            table = document.getElementById("TableEmpleadoAsignar");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td");

                for (j = 0; j < td.length; j++) {
                    let tdata = td[j];

                    if (tdata) {
                        if (tdata.innerHTML.toUpperCase().indexOf(filter) > -1) {
                            tr[i].style.display = "";
                            break;

                        } else {
                            tr[i].style.display = "none";
                        }
                    }
                }
            }

            if ($(".empleadocheck:checked").length != $(".empleadocheck").length) {
                $(".MarcarTodos").prop("checked", false);
            }
        }

        function SaveData(labor, level, employeesSelected) {
            var entity = {
                obj: {
                    Level: level,
                    LaborId1: level == "1" ? labor : -1,
                    LaborId2: level == "2" ? labor : -1,
                    LaborId3: level == "3" ? labor : -1
                },

                Employees: employeesSelected
            };

            $.ajax({
                type: "POST",
                url: "AssingEmployeeLabor.aspx/AsignarLabor",
                data: JSON.stringify(entity),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;

                    if (result.ErrorNumber == -2) {
                        MostrarMensaje(TipoMensaje.VALIDACION, "<% = GetLocalResourceObject("msjLaborDupicatedAsigned").ToString()%>", null);

                    } else if (result.ErrorNumber != 0) {
                        MostrarMensaje(TipoMensaje.ERROR, result.ErrorMessage, null);

                    } else {
                        MostrarMensaje(TipoMensaje.INFORMACION, "<% = GetLocalResourceObject("msgOperationSuccesfull").ToString()+"<br/>" + GetLocalResourceObject("lblRegistrosAfectados").ToString()%>" + result.ErrorMessage, null);
                        document.getElementById("<%= btnSearchRefresh.ClientID%>").click();
                    }

                    $('.btnAjaxAction').button('reset');
                },

                error: function () {
                    MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                    event.target.checked = false;
                }
            });
        }

        //*******************************//
        //           PROCESS             //
        //*******************************//
        function ProcessDownloadRequest(resetId) {
            /// <summary>Process the edit request according to the validation of row selected</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            var entity = {
                obj: {
                    EmployeeCode: IsNullOrEmpty($('#<%= EmployeeCodeFilter.ClientID %>').val()) ? null : $('#<%= EmployeeCodeFilter.ClientID %>').val(),
                    EmployeeName: IsNullOrEmpty($('#<%= EmployeeNameFilter.ClientID %>').val()) ? null : $('#<%= EmployeeNameFilter.ClientID %>').val(),
                    CompanyCode: IsNullOrEmpty($('#<%= CompaniesFilter.ClientID %>').val()) ? null : $('#<%= CompaniesFilter.ClientID %>').val(),
                    PayrollClassCode: IsNullOrEmpty($('#<%= PayrollClassFilter.ClientID %>').val()) ? null : $('#<%= PayrollClassFilter.ClientID %>').val(),
                    CostsCenterCode: IsNullOrEmpty($('#<%= CostCenterFilter.ClientID %>').val()) ? null : $('#<%= CostCenterFilter.ClientID %>').val(),
                    PositionCode: IsNullOrEmpty($('#<%= PositionFilter.ClientID %>').val()) ? null : $('#<%= PositionFilter.ClientID %>').val(),
                    RecruitmentDATE: IsNullOrEmpty($('#<%= RecruitmentDate.ClientID %>').val()) ? null : $('#<%= RecruitmentDate.ClientID %>').val()
                },
            };

            $.ajax({
                type: "POST",
                url: "AssingEmployeeLabor.aspx/ExportarLabor",
                data: JSON.stringify(entity),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var result = data.d;

                    ReturnFromBtnExportClickPostBack(result);

                    $('.btnAjaxAction').button('reset');
                },

                error: function () {
                    MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                    event.target.checked = false;
                }
            });
        }

        //*******************************//
        //  RETURN FROM POSTBACKS LOGIC  // 
        //*******************************//
        function ReturnFromBtnExportClickPostBack(jsonData) {
            var employees = [];
            filename = '<%= GetLocalResourceObject("ExcelName") %>.xlsx';

            // Get format data
            jsonData.forEach(row => {
                var obj = new Object({
                    "<%= GetLocalResourceObject("EmployeeCode.HeaderText") %>": row.EmployeeCode,
                    "<%= GetLocalResourceObject("GeographicDivisionCode.HeaderText") %>": row.GeographicDivisionCode,
                    "<%= GetLocalResourceObject("DivisionName.HeaderText") %>": row.DivisionName,
                    "<%= GetLocalResourceObject("EmployeeName.HeaderText") %>": row.EmployeeName,

                    "<%= GetLocalResourceObject("CompanyCode.HeaderText") %>": row.CompanyCode,

                    "<%= GetLocalResourceObject("PayrollClassCode.HeaderText") %>": row.PayrollClassCode,

                    "<%= GetLocalResourceObject("PositionCode.HeaderText") %>": row.PositionCode,
                    "<%= GetLocalResourceObject("PositionName.HeaderText") %>": row.PositionName,

                    "<%= GetLocalResourceObject("CostsCenterCode.HeaderText") %>": row.CostsCenterCode,
                    "<%= GetLocalResourceObject("CostsCenterName.HeaderText") %>": row.CostsCenterName,

                    "<%= GetLocalResourceObject("Labor1Code.HeaderText") %>": row.Labor1.LaborCode,
                    "<%= GetLocalResourceObject("Labor1Name.HeaderText") %>": row.Labor1.LaborName,
                    "<%= GetLocalResourceObject("Labor2Code.HeaderText") %>": row.Labor2.LaborCode,
                    "<%= GetLocalResourceObject("Labor2Name.HeaderText") %>": row.Labor2.LaborName,
                    "<%= GetLocalResourceObject("Labor3Code.HeaderText") %>": row.Labor3.LaborCode,
                    "<%= GetLocalResourceObject("Labor3Name.HeaderText") %>": row.Labor3.LaborName
                });

                employees.push(obj);
            });

            let objectMaxLength = []

            // Get columns length
            employees.map(arr => {
                Object.keys(arr).map(key => {
                    let value = arr[key] === null ? '' : arr[key]

                    if (typeof value === 'number') {
                        return objectMaxLength[key] = 10
                    }

                    objectMaxLength[key] = objectMaxLength[key] >= value.length ? objectMaxLength[key] : value.length
                })
            })

            // Get columns header
            const header = Object.keys(employees[0]);

            // Add columns length
            let wsCols = [];
            let col = 0;

            for (var i = 0; i < header.length; i++) {
                col = objectMaxLength[header[i]];

                if (objectMaxLength[header[i]] <= 10) {
                    col = header[i].length + 13
                }

                wsCols.push({ wch: col });
            }

            let workSheet = XLSX.utils.json_to_sheet(employees);
            workSheet['!autofilter'] = { ref: `A1:L${employees.length + 1}` }

            workSheet["!cols"] = wsCols;

            var workBook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(workBook, workSheet, "<%= GetLocalResourceObject("ExcelName") %>");
            XLSX.writeFile(workBook, filename);
        }
    </script>
</asp:Content>
