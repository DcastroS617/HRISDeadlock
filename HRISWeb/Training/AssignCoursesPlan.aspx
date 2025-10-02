 <%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AssignCoursesPlan.aspx.cs" Inherits="HRISWeb.Training.AssignCoursesPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .table-responsive {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        [v-cloak] {
            display: none;
        }

         .sticky-col {
            position: -webkit-sticky;
            position: sticky;
            background-color: white;
        }

        .sticky2-col {
          position: sticky;
          background-color: white;
          color: black;
          text-align: center;
          vertical-align: middle;
          z-index: 1; /* Asegura que esté sobre el contenido */
          padding: 10px; /* Ajusta el padding según sea necesario */
          top: 0; /* Fija la columna pegajosa en la parte superior */
  }
       
/* Estilos adicionales si es necesario */

        
        .sc1-col5 {
            left: 0px;
        }

        .sc1-col {
            left: 0px;
        }

        .sc2-col {
            left: 100px;
        }

        .sc3-col {
            left: 175px;
        }
    </style>
   
    <script src="../Includes/vue.min.js"></script>

    <div id="AppMatrix" class="container" style="width:100%" v-cloak>
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />

        <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblFiltersSubtitle") %></h4>                
        <br /> 
                                                    
        <div class="row">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="row">       
                        <div class="form-group  col-md-4">
                            <div class="col-sm-4 text-left">
                                <label for="MasterProgramIdFilter" class="control-label" style="text-align:left"><%= GetLocalResourceObject("lblPlanTraining") %></label>
                            </div>

                            <div class="col-sm-8">
                                <select id="MasterProgramIdFilter" onchange="OnChangeMasterPrograms(this)" class="form-control selectpicker OnChangeMasterPrograms" data-live-search="true" data-actions-box="true">
                                    <option v-for="item in ListMasterPrograms" :value="item.Value" v-text="item.Text"></option>
                                </select>
                            </div>
                        </div>

                        <div class="form-group  col-md-4">
                            <div class="col-sm-4 text-left">
                                <label for="ThematicAreasCodeFilter" class="control-label"><%= GetLocalResourceObject("lblThemacticArea") %></label>
                            </div>

                           <div class="col-sm-8">
                                <select id="ThematicAreasCodeFilter" @change="CargaCourses" onchange="OnChangeThematicAreas(this)" class="form-control selectpicker OnChangeThematicAreas" multiple="true" data-live-search="true" data-actions-box="true">
                                    <option v-for="item in ListThematicAreas" :value="item.Value" v-text="item.Text"></option>
                                </select>
                            </div>
                        </div>
                                        
                        <div class="form-group col-md-4" v-if="ValidacionThematicAreasCodeFiltro != 0">
                            <div class="col-sm-4 text-left">
                                <label for="CoursesByMasterProgram" class="control-label"><%= GetLocalResourceObject("lblCourse") %></label>
                            </div>

                            <div class="col-sm-8">
                                <select id="CoursesByMasterProgram" onchange="OnChangeCourses(this)" class="form-control selectpicker OnChangeCourses" multiple="true" data-live-search="true" data-actions-box="true">       
                                    <option v-for="item in ListCourses" :value="item.CourseCode" v-text="item.CourseName"></option>
                                </select>           
                            </div>
                        </div>
                    </div>
                </div>
            </div>                      
        </div>

        <div class="row" v-if="ValidacionMasterProgramFiltro != 0">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="form-group col-md-4" v-if="ValidacionMasterProgramFiltro == 1">
                            <div class="col-sm-4 text-left">
                                <label for="PositionByMasterProgram" class="control-label"><%= GetLocalResourceObject("lblPuesto") %></label>
                            </div>

                            <div class="col-sm-8">
                                 <select id="PositionByMasterProgram" onchange="OnChangePositions(this)" class="form-control selectpicker OnChangePositions" multiple="true" data-live-search="true"  data-actions-box="true">
                                    <option v-for="item in listPositions" :value="item.PositionCode" v-text="item.PositionName"></option>
                                </select>
                            </div>
                        </div>

                        <div class="form-group  col-md-4" v-if="ValidacionMasterProgramFiltro == 1">
                            <div class="col-sm-4 text-left">
                                <label for="LaborByMasterProgram" class="control-label"><%= GetLocalResourceObject("LblLabor") %></label>
                            </div>

                            <div class="col-sm-8">
                                <select id="LaborByMasterProgram" onchange="OnChangeLabor(this)" class="form-control selectpicker OnChangeLabor" multiple="true" data-live-search="true"  data-actions-box="true">
                                    <option v-for="item in ListLabor" :value="item.LaborCode" v-text="item.LaborName"></option>
                                </select>
                            </div>
                        </div>

                        <div class="form-group  col-md-4" v-if="ValidacionMasterProgramFiltro == 2">
                            <div class="col-sm-4 text-left">
                                <label for="EmployeeByMasterProgram" class="control-label"><%= GetLocalResourceObject("lblEmployee") %></label>
                            </div>

                            <div class="col-sm-8">
                                <select id="EmployeeByMasterProgram" onchange="OnChangeEmployee(this)" class="form-control selectpicker OnChangeEmployee" multiple="true" data-live-search="true"  data-actions-box="true">
                                    <option v-for="item in ListEmployee" :value="item.EmployeeCode" v-text="item.EmployeeName"></option>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12 text-center">
                <button id="btnSearch"  type="button" class="btn btn-default btnAjaxAction" @click="FilterMatrix">
                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSearch") %>
                </button>                       
            </div>
        </div>               
        <br />

        <input type="hidden" id="hdnSelectedAll" value=""/>

        <div class="row" v-if="result.Plan.RelateBy == 1">
            <div class="col-sm-12 text-center">
                <table class="table table-striped table-hover table-bordered" style="border: none;">
                    <thead>
                        <tr>
                            <th class="sticky-col" style="border: 1px solid #ddd;" colspan="1000"><%= GetLocalResourceObject("lblProgram") %>  : {{result.Plan.MasterProgramName}}</th>
                        </tr>
                        <tr>
                            <th class="sticky-col" colspan="1000"><%= GetLocalResourceObject("lblAlcance") %> : {{result.Plan.MatrixTargetName}}</th>
                        </tr>
                        <tr>
                            <th colspan="1000"><%= GetLocalResourceObject("lblVigencia") %> : {{result.Plan.IsExpiration? ConVencimiento:SinVencimiento}}</th>
                        </tr>
                        <tr>
                            <th rowspan="1" colspan="1000"><%= GetLocalResourceObject("lblThemacticArea") %> : {{result.Plan.ThematicAreaName}}</th>
                        </tr>
                        <tr style="border: none">
                            <th style="border: none" colspan="1000"></th>
                        </tr>
                        <tr style="border: none">
                            <th style="border: none" colspan="1000"></th>
                        </tr>
                        <tr style="border: none">
                            <th style="border: none" colspan="1000"></th>
                        </tr>
                    </thead>
                </table>
            </div>

            <div class="col-sm-12 text-center">
                <div class="table-responsive">
                    <table class="table table-striped table-hover table-bordered" style="border: none;">
                        <thead>
                            <tr>
                                <th class="sticky2-col sc1-col" style="background-color: #69899f; color: white; text-align:center; vertical-align: middle"><%= GetLocalResourceObject("lblTipoPuesto") %></th>
                                <th class="sticky2-col sc2-col" style="background-color: #69899f; color: white; text-align:center; vertical-align: middle"><%= GetLocalResourceObject("lblPuesto") %></th>
                                <th class="sticky2-col sc3-col" style="background-color: #69899f; color: white; text-align:center; vertical-align: middle">Labor</th>
                                
                                <th v-for="(C, Cindex) in result.Colums" style="background-color: #69899f; color: white; vertical-align: middle">
                                    <div class="col-sm-10 text-center">
                                        {{C.CourseName}}
                                    </div>

                                    <div class="col-sm-2 text-center">
                                        <input type="checkbox" :id="'chk_1_' + C.CourseCode" class="form-control chkAll" style="width: 11px; height: auto" v-model="C.IsCheckedAll" @change="OnChangeCheckAll($event, Cindex)"/>
                                    </div>
                                </th>  
                            </tr>
                        </thead>

                        <tbody>
                            <tr v-for="mt in result.Matrix">
                                <td class="sticky2-col sc1-col" style="text-align:center; vertical-align: middle">{{mt.Entity.PositionType == 1 ? lblAdministrativo : lblOperativo}}</td>
                                <td class="sticky2-col sc2-col" style="text-align:left; vertical-align: middle">{{mt.Entity.PositionName == null ?'N/A' : mt.Entity.PositionName}} </td>
                                <td class="sticky2-col sc3-col" style="text-align:left; vertical-align: middle">{{mt.Entity.LaborCode == null ? 'N/A' : mt.Entity.LaborCode + " - " + mt.Entity.LaborName}} </td>

                                <td v-for="(C, Cindex) in mt.Courses" align="center">
                                    <input type="checkbox" :id="'chk_1_' + C.CourseCode + '_' + Cindex" class="form-control" style="width: 11px; height: auto" v-model="C.IsChecked" v-on:change="OnChangeCheck($event,Cindex)" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <div class="row" v-if="result.Plan.RelateBy == 2">
            <div class="col-sm-12 text-center">
                <table class="table table-striped table-hover table-bordered" style="border: none;">
                    <thead>
                        <tr>
                            <th class="sticky-col" style="border: 1px solid #ddd;" colspan="1000"><%= GetLocalResourceObject("lblProgram") %>  : {{result.Plan.MasterProgramName}}</th>
                        </tr>
                        <tr>
                            <th class="sticky-col" colspan="1000"><%= GetLocalResourceObject("lblAlcance") %> : {{result.Plan.MatrixTargetName}}                                
                            </th>
                        </tr>
                        <tr>
                            <th colspan="1000"><%= GetLocalResourceObject("lblVigencia") %> : {{result.Plan.IsExpiration? ConVencimiento:SinVencimiento}}</th>
                        </tr>
                        <tr>
                            <th rowspan="1" colspan="1000"><%= GetLocalResourceObject("lblThemacticArea") %> : {{result.Plan.ThematicAreaName}}</th>
                        </tr>
                        <tr style="border: none">
                            <th style="border: none" colspan="1000"></th>
                        </tr>
                        <tr style="border: none">
                            <th style="border: none" colspan="1000"></th>
                        </tr>
                        <tr style="border: none">
                            <th style="border: none" colspan="1000"></th>
                        </tr>
                    </thead>
                </table>
            </div>
           
            <div class="col-sm-12 text-center">
                <div class="table-responsive">
                    <table class="table table-striped table-hover table-bordered" style="border: none;">
                        <thead>
                            <tr>
                                <th class="sticky2-col sc1-col5" style="background-color: #69899f; color: white" colspan="5" class="text-center"><%= GetLocalResourceObject("lblNombreEmpleado") %></th>

                                <th style="background-color: #69899f; color: white;" v-for="(C, Cindex) in result.Colums" >
                                    <div class="col-sm-10 text-center">
                                        {{C.CourseName}}
                                    </div>

                                    <div class="col-sm-2 text-center">
                                        <input type="checkbox" :id="'chk_2_' + C.CourseCode" class="form-control" style="width: 11px; height: auto" v-model="C.IsCheckedAll" v-on:change="OnChangeCheckAll($event,Cindex)"/>
                                    </div>
                                </th>  
                            </tr>
                        </thead>

                        <tbody>
                            <tr v-for="mt in result.Matrix">
                                <td class="sticky2-col sc1-col5 text-center" colspan="5">{{mt.Entity.EmployeeName}}</td>

                                <td v-for="(C, Cindex) in mt.Courses" align="center">
                                    <input type="checkbox" :id="'chk_2_' + C.CourseCode + '_' + Cindex" class="form-control" style="width: 11px; height: auto" v-model="C.IsChecked" v-on:change="OnChangeCheck($event,Cindex)" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <br />

        <div class="row" v-if="result.Plan.RelateBy == 1 || result.Plan.RelateBy == 2">
            <div class="col-sm-12 text-right">
                <button type="button" class="btn btn-default btnAjaxAction" @click="SaveMatrix">
                    <span class="glyphicon glyphicon-save glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%= GetLocalResourceObject("btnSave") %>
                </button>
            </div>
        </div>    
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />

    <script type="text/javascript">
        var vm = new Vue({
            data: {
                result: {
                    Plan: {
                        MasterProgramName: null,
                        MatrixTargetName: null,
                        IsExpiration: null,
                        ThematicAreaName: null,
                        RelateBy: null
                    },

                    Matrix: []
                },

                Filter: {
                    MasterProgramId: null,
                    ThematicAreasCode: null,
                    CoursesCodes: null,
                    PositionCodes: null,
                    LaborCodes: null,
                    EmployeeCodes: null,
                },

                MasterProgramSelected: { MasterProgramId: null },
                ConVencimiento:'<%= GetLocalResourceObject("lblConVencimiento") %>',
                SinVencimiento: '<%= GetLocalResourceObject("lblSinVencimiento") %>',
                lblAdministrativo:'<%= GetLocalResourceObject("lblAdministrativo") %>',
                lblOperativo: '<%= GetLocalResourceObject("lblOperativo") %>',

                ListMasterPrograms: [],
                ListThematicAreas: [],
                ListCourses: [],
                ListCoursesSelectedAll: [],
                listPositions: [],
                ListLabor: [],
                ListEmployee: [],

                ValidacionMasterProgramFiltro: null,
                ValidacionThematicAreasCodeFiltro: 0,
            },

            methods: {
                CargaMasterPrograms: function () {
                    var obj = this.Filter;
                    var $this = this;

                    this.ListMasterPrograms = [];

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/ListMasterPrograms",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $this.ListMasterPrograms = data.d;

                            $this.ListMasterPrograms.unshift(
                                { Value: -1, Text: '<%= GetLocalResourceObject("msjSelectProgram").ToString()%>', Attributes: null, Enabled: true, Selected: false }
                            );

                            $this.$nextTick().then(function () {
                                $('.selectpicker').selectpicker('refresh');
                            });
                        },

                        error: function (err) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                        }
                    });
                },

                CargaThematicAreas: function () {
                    var obj = this.Filter;
                    var $this = this;

                    this.ListThematicAreas = [];

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/ListThematicAreas",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $this.ListThematicAreas = data.d;
                            $this.$nextTick().then(function () {
                                $('.selectpicker').selectpicker('refresh');
                            });
                        },

                        error: function (err) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                        }
                    });
                },

                CargaCourses: function () {
                    var obj = this.Filter;
                    var $this = this;

                    this.ListCourses = [];

                    $("#CoursesByMasterProgram").html("");
                    $("#CoursesByMasterProgram").selectpicker("val", "");
                    $("#CoursesByMasterProgram").val('');
                    $("#CoursesByMasterProgram").selectpicker('refresh');

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/ListCoursesByMasterProgram",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $this.ListCourses = data.d;

                            $this.$nextTick().then(function () {
                                $('.selectpicker').selectpicker('refresh');
                            });
                        },

                        error: function (err) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                        }
                    });
                },

                CargaLabores: function () {
                    var obj = this.Filter
                    var $this = this;

                    $this.ListLabor = [];

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/ListLaborByMasterProgram",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $this.ListLabor = data.d;

                            $this.$nextTick().then(function () {
                                $('.selectpicker').selectpicker('refresh');
                            });
                        },

                        error: function (err) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("MsjError").ToString()%>', null);
                        }
                    });
                },

                CargaEmpleados: function () {
                    var obj = this.Filter
                    var $this = this;

                    $this.ListEmployee = [];

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/ListEmployeeByMasterProgram",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $this.ListEmployee = data.d;

                            $this.$nextTick().then(function () {
                                $('.selectpicker').selectpicker('refresh');
                            });
                        },

                        error: function (err) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("MsjError").ToString()%>', null);
                        }
                    });
                },

                CargaPuestos: function () {
                    var obj = this.Filter
                    var $this = this;

                    $this.listPositions = [];

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/ListPositionsByMasterProgram",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $this.listPositions = data.d;

                            $this.$nextTick().then(function () {
                                $('.selectpicker').selectpicker('refresh');
                            });
                        },

                        error: function (err) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("MsjError").ToString()%>', null);
                        }
                    });
                },
                OnChangeCheckAll: function (event, Cindex) {
                    const isCheckedAll = event.target.checked;
                    this.result.Matrix.forEach(matrixItem => {
                        
                        if (matrixItem.Courses && matrixItem.Courses.length > Cindex) {
                            Vue.set(matrixItem.Courses[Cindex], 'IsChecked', isCheckedAll);
                        }
                    });
                },
    


                OnChangeCheck: function (e) {
                    if (e.target.checked) {
                        return;
                    }

                    let courseCode = e.target.id.split("_");

                    const index = this.ListCoursesSelectedAll.findIndex(c => c.CourseCode === courseCode[2]);
                    if (index !== -1) {
                        this.ListCoursesSelectedAll.splice(courseCode, 1);
                    }

                    let selector = "#" + e.target.id.toString();
                    let idParent = selector.substring(0, selector.length - 2);
                    $(idParent).prop('checked', false);
                },

                ResponsiveTable: function () {
                    var max = 0;

                    $('table td').each(function () {
                        max = Math.max($(this).width(), max);
                    }).width(max);
                },

                FilterMatrix: function () {
                    $('.btnAjaxAction').button('loading');

                    var obj = this.Filter
                    $this = this;

                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/FilterMatrix",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            $('.btnAjaxAction').button('reset');
                            $this.result = data.d;

                            setTimeout(function () { $this.ResponsiveTable(); }, 500);
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                        }
                    });
                },

                SaveMatrix: function () {
                    var obj = {
                        matrix: this.result.Matrix,
                        courses: this.ListCoursesSelectedAll
                    };

                    if (this.result.Matrix == null || this.result.Matrix == undefined) {
                        MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msjCourseIdEditValidations").ToString()%>', null);
                        return;
                    }
           
                    $.ajax({
                        type: "POST",
                        url: "AssignCoursesPlan.aspx/SaveMatrix",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {                       
                            $('.btnAjaxAction').button('reset');

                            var result = data.d;

                            if (result.ErrorNumber != 0) {
                                MostrarMensaje(TipoMensaje.ERROR, result.ErrorMessage, null);
                            }
                            else {
                                MostrarMensaje(TipoMensaje.INFORMACION,"<% = GetLocalResourceObject("msgOperationSuccesfull").ToString()%>", null);
                            }
                        },

                        error: function () {
                            MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                        }
                    });
                }
            },
       
            mounted: function () {
                this.CargaMasterPrograms();

                this.CargaThematicAreas();
           
                this.$nextTick(() => {
                    this.ListCoursesSelectedAll = [];
                });
            }
        });

        vm.$mount("#AppMatrix");

        var OnChangeMasterPrograms;
        var OnChangeThematicAreas;
        var OnChangeCourses;
        var OnChangePositions;
        var OnChangeLabor;
        var OnChangeEmployee;

        var FilterDisabled;
        var ValidationSearchFilter;
        var ClearObjectVm;
        var ClearSearchFilter;

        $(document).ready(function () {
            $('.selectpicker').selectpicker();

            OnChangeMasterPrograms = function (id) {
                var selectedItems = $(".OnChangeMasterPrograms").selectpicker('val');

                if (selectedItems.length > 0) {
                    vm.Filter.MasterProgramId = selectedItems.toString();
                }
                else {
                    vm.Filter.MasterProgramId = null;
                }

                ValidationSearchFilter();
                ClearSearchFilter();
            };

            OnChangeThematicAreas = function (id) {
                var selectedItems = $(".OnChangeThematicAreas").selectpicker('val');      
                if (selectedItems.length > 0) {
                    vm.Filter.ThematicAreasCode = selectedItems.toString();
                } 
                else {
                    vm.Filter.ThematicAreasCode = null;
                }

                vm.ValidacionThematicAreasCodeFiltro = 1;
            };

            OnChangeCourses = function (id) {
                var selectedItems = $(".OnChangeCourses").selectpicker('val');
    
                if (selectedItems.length > 0) {
                    vm.Filter.CoursesCodes = selectedItems.toString();
                }
                else {
                    vm.Filter.CoursesCodes = null;
                }

                ClearObjectVm();
                FilterDisabled();
            };

            OnChangePositions = function (id) {
                var selectedItems = $(".OnChangePositions").selectpicker('val');
   
                if (selectedItems.length > 0) {
                    vm.Filter.PositionCodes = selectedItems.toString();
                }
                else {
                    vm.Filter.PositionCodes = null;
                }

                ClearObjectVm();
                FilterDisabled();
            };

            OnChangeLabor = function (id) {
                var selectedItems = $(".OnChangeLabor").selectpicker('val');
 
                if (selectedItems.length > 0) {
                    vm.Filter.LaborCodes = selectedItems.toString();
                }

                else {
                    vm.Filter.LaborCodes = null;
                }

                ClearObjectVm();
                FilterDisabled();
            };

            OnChangeEmployee = function (id) {
                var selectedItems = $(".OnChangeEmployee").selectpicker('val');
  
                if (selectedItems.length > 0) {
                    vm.Filter.EmployeeCodes = selectedItems.toString();
                }

                else {
                    vm.Filter.EmployeeCodes = null;
                }

                ClearObjectVm();
                FilterDisabled();
            };

            FilterDisabled = function (id) {
                if (vm.ValidacionMasterProgramFiltro == 1) {
                    if (vm.Filter.MasterProgramId != "" && vm.Filter.MasterProgramId != null &&
                        vm.Filter.ThematicAreasCode != "" && vm.Filter.ThematicAreasCode != null &&
                        vm.Filter.CoursesCodes != "" && vm.Filter.CoursesCodes != null &&
                        (vm.Filter.PositionCodes != "" && vm.Filter.PositionCodes != null ||
                            vm.Filter.LaborCodes != "" && vm.Filter.LaborCodes != null)) {

                        enableButton($('#btnSearch'));
                    }
                    else {
                        disableButton($('#btnSearch'));
                    }
                }

                else if (vm.ValidacionMasterProgramFiltro == 2) {
                    if (vm.Filter.MasterProgramId != "" && vm.Filter.MasterProgramId != null &&
                        vm.Filter.ThematicAreasCode != "" && vm.Filter.ThematicAreasCode != null &&
                        vm.Filter.CoursesCodes != "" && vm.Filter.CoursesCodes != null &&
                        vm.Filter.EmployeeCodes != "" && vm.Filter.EmployeeCodes != null) {

                        enableButton($('#btnSearch'));
                    }
                    else {
                        disableButton($('#btnSearch'));
                    }
                }
            }

            ValidationSearchFilter = function () {
                var obj = vm.Filter;

                $.ajax({
                    type: "POST",
                    url: "AssignCoursesPlan.aspx/MasterProgramValidationTypeSearch",
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var result = data.d;
                        vm.ValidacionMasterProgramFiltro = 0;
                    
                        if (result.Relatedby == 1) {
                            vm.CargaLabores();
                            vm.CargaPuestos();

                            vm.ValidacionMasterProgramFiltro = 1;
                        }
                        else if (result.Relatedby == 2) {
                            vm.CargaEmpleados();

                            vm.ValidacionMasterProgramFiltro = 2;
                        }
                    },

                    error: function (err) {
                        MostrarMensaje(TipoMensaje.ERROR, '<%= GetLocalResourceObject("msj006.Text").ToString()%>', null);
                    }
                });
            };

            ClearObjectVm = function () {
                vm.result = {
                    Plan: {
                        MasterProgramName: null,
                        MatrixTargetName: null,
                        IsExpiration: null,
                        ThematicAreaName: null,
                        RelateBy: null
                    },

                    Matrix: []
                };

                vm.ListCoursesSelectedAll = []
            }

            ClearSearchFilter = function () {
                disableButton($('#btnSearch'));

                $("#ThematicAreasCodeFilter").val('default');
                $("#ThematicAreasCodeFilter").selectpicker("refresh");

                $(".OnChangeCourses").val('default');
                $(".OnChangeCourses").selectpicker("refresh");

                $(".OnChangePositions").val('default');
                $(".OnChangePositions").selectpicker("refresh");

                $(".OnChangeLabor").val('default');
                $(".OnChangeLabor").selectpicker("refresh");

                $(".OnChangeEmployee").val('default');
                $(".OnChangeEmployee").selectpicker("refresh");

                ClearObjectVm();

                //let selector = "input[class*='chkAll']";
                //$(selector, table).closest("tr").prop('checked', false);

                $(".table-striped").closest("tr").remove();
            }

            ClearSearchFilter();
        });
    </script>
</asp:Content>
