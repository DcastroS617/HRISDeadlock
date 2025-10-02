<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SynchronizeSurveys.aspx.cs" Inherits="HRISWeb.SocialResponsability.SynchronizeSurveys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />
        <div class="container" style="width: 100%">
            <div class="row">
                <div class="col-sm-12">
                    <h4 class="text-left text-primary"><%= GetLocalResourceObject("lblPendingSurveys.Text") %></h4>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <asp:UpdatePanel ID="upnList" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="blstPager" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbtnSynchronizeSurveys" EventName="Click"/>
                        </Triggers>
                        <ContentTemplate>
                            <asp:GridView ID="grvPendingSurveys" Width="100%" runat="server" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>' EmptyDataRowStyle-CssClass="emptyRow"
                                AllowPaging="false" PagerSettings-Visible="false" AllowSorting="false" AutoGenerateColumns="false" ShowHeader="true"
                                CssClass="table table-striped table-hover table-bordered" DataKeyNames="SurveyCode" OnPreRender="grvPendingSurveys_PreRender">
                                <Columns>
                                    <asp:BoundField DataField="EmployeeCode" meta:resourcekey="EmployeeCode" />
                                    <asp:BoundField DataField="EmployeeID" meta:resourcekey="EmployeeID" />
                                    <asp:BoundField DataField="EmployeeName" meta:resourcekey="EmployeeName" />
                                    <asp:BoundField DataField="SurveyStartDateTime" meta:resourcekey="SurveyStartDateTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                </Columns>
                            </asp:GridView>
                            <div id="grvListWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                            </div>
                            <h4 class="text-left text-primary" id="htmlResultsSubtitle" runat="server"></h4>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <nav>
                    <asp:BulletedList ID="blstPager" runat="server" CssClass="pagination" DisplayMode="LinkButton" OnClick="blstPager_Click">
                    </asp:BulletedList>
                </nav>
            </div>
            <div class="ButtonsActions">
                <asp:UpdatePanel ID="upnActions" runat="server">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <div class="btn-group" role="group" aria-label="main-buttons">
                            <asp:LinkButton ID="lbtnSynchronizeSurveys" runat="server" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnSynchronizeSurveys_Click"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnSynchronizeSurveys.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnSynchronizeSurveys.Text"))%>'>
                                    <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                    <%= GetLocalResourceObject("lbtnSynchronizeSurveys.Text") %>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            $('.btnAjaxAction').on('click', function () {
                    var $this = $(this);
                    $this.button('loading');

                    setTimeout(function () { $this.button('reset'); }, 1000000);

            });
            //And the grvList pager event for client side
            $('#<%= blstPager.ClientID %>').on('click', 'a', function (event) {
                SetWaitingGrvList();
            });
        }
        function SetWaitingGrvList() {
            /// <summary>Process the request of set the grid as waiting style</summary>
            setTimeout(function () {
                $('#<%= grvPendingSurveys.ClientID %>').find("input,button,textarea,select").attr("disabled", "disabled");
                $('#<%= grvPendingSurveys.ClientID %>').find("a").removeAttr('href');
                $('#<%= blstPager.ClientID %>').find("a").removeAttr('href');
                $('#grvListWaiting').fadeIn('fast');
                $('#<%= grvPendingSurveys.ClientID %>').fadeTo('fast', 0.5);
            }, 100);
        }
        function ProcessSynchronizeResponse() {
            /// <summary>Process the response of set the Synchronize button</summary>
            setTimeout(function () {                
                ResetButton($('#<%= lbtnSynchronizeSurveys.ClientID %>').id);
            }, 200);
        }
    </script>
</asp:Content>
