<%@ Page Title="HRIS" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HRISWeb.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=DOLE.HRIS.Shared.Messages.SiteName%></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <script type="text/javascript">
        function OpenWorkingDivisionModal() {
            ///<summary>Show the working division modal.</summary>            
            $('#workingdivisionmodal').modal('show');
        }

        function CloseWorkingDivisionModal() {
            ///<summary>Close the working division modal.</summary> 
            setTimeout(function () {
                $('#<%= btnSelectWorkingDivision.ClientID %>').button('reset');
                enableButton($('#<%= btnSelectWorkingDivision.ClientID %>'));
            }, 200);
            $('#workingdivisionmodal').modal('hide');
        }

        $(document).ready(function () {
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            $('#workingdivisionmodal').on('show.bs.modal', function (event) {
                /// <summary>Handles the Show event of the workingdivisionmodal modal.</summary>
                $('#<%= cboUserDivisions.ClientID %>').focus();
            });

            $('#<%= btnSelectWorkingDivision.ClientID %>').on('click', function (event) {
                /// <summary>Handles the Click event of the btnSelectWorkingDivision control.</summary>
                event.preventDefault();
                disableButton($('#<%= btnSelectWorkingDivision.ClientID %>'));
                var selectedDivisionCode = $('#<%= cboUserDivisions.ClientID %>').selectpicker('val');
                if (selectedDivisionCode === "0") {
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= Convert.ToString(GetLocalResourceObject("msjMustSelectADivisionToWork")) %>', function () {
                        $('#<%= btnSelectWorkingDivision.ClientID %>').button('reset');
                        enableButton($('#<%= btnSelectWorkingDivision.ClientID %>'));
                    });
                }
                else {
                    __doPostBack('<%= btnSelectWorkingDivision.UniqueID %>', 'OnClick');
                }
            });
        });
    </script>

    <asp:Image ID="imgBackground" CssClass="img-responsive center-block" ImageUrl="~/Content/images/background.png" runat="server" />

    <!-- Modal for select the working division  -->
    <div class="modal fade" id="workingdivisionmodal" tabindex="-1" role="dialog" aria-labelledby="workingdivisionmodalTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title text-primary" id="workingdivisionmodallabel"><%= Convert.ToString(GetLocalResourceObject("msjWorkinDivisionModalTitle")) %></h4>
                    <hr />
                </div>

                <div class="modal-body">
                    <asp:UpdatePanel ID="uppDivisions" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSelectWorkingDivision" EventName="Click" />
                        </Triggers>

                        <ContentTemplate>
                            <div class="form-group row">
                                <label for="<%= cboUserDivisions.ClientID %>" class="col-sm-2 col-control-label"><%= Convert.ToString(GetLocalResourceObject("msjDivisionLabel")) %></label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="cboUserDivisions" runat="server" CssClass="form-control selectpicker" data-live-search="true" AutoPostBack="false"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="modal-footer">
                    <asp:LinkButton ID="btnSelectWorkingDivision" runat="server" CssClass="btn btn-primary btnAjaxAction" OnClick="BtnSelectWorkingDivision_Click"
                        data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSelectWorkingDivision.Text"))%>'
                        data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("btnSelectWorkingDivision.Text"))%>'>
                        <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                        <%= GetLocalResourceObject("btnSelectWorkingDivision.Text") %>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
