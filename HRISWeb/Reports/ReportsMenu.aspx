<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ReportsMenu.aspx.cs" Inherits="HRISWeb.Reports.ReportsMenu" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        #content {
            -webkit-transition: width 0.3s ease;
            -moz-transition: width 0.3s ease;
            -o-transition: width 0.3s ease;
            transition: width 0.3s ease;
        }

        #sidebar {
            -webkit-transition: margin 0.3s ease;
            -moz-transition: margin 0.3s ease;
            -o-transition: margin 0.3s ease;
            transition: margin 0.3s ease;
        }

        #brHeader {
            -webkit-transition: margin 0.3s ease;
            -moz-transition: margin 0.3s ease;
            -o-transition: margin 0.3s ease;
            transition: margin 0.3s ease;
        }

        #menuHeader {
            -webkit-transition: margin 0.3s ease;
            -moz-transition: margin 0.3s ease;
            -o-transition: margin 0.3s ease;
            transition: margin 0.3s ease;
        }

        .collapsed {
            display: none; /* hide it for small displays */
        }

        .headerCollapsed {
            display: none; /* hide it for small displays */
        }

        .menuCollapsed {
            display: none; /* hide it for small displays */
        }

        @media (min-width: 992px) {
            .collapsed {
                display: block;
                margin-left: -100%; /* same width as sidebar */
            }

            .menuCollapsed {
                display: none;
                margin-top: -40px; /* same width as sidebar */
            }

            .headerCollapsed {
                display: block;
                margin-top: -10%; /* same width as sidebar */
            }
        }

        .iframeCollapsed {
            display: block;
            border: none;
            height: calc(100vh - 235px);
            width: 100%;
        }

        .iframeExpanded {
            display: block;
            border: none;
            height: calc(100vh - 60px);
            width: 100%;
        }
    </style>

    <div class="main-content">
        <div class="container" style="width: 100%">
            <div class="row" id="row-main">
                <div class="col-sm-5" id="sidebar" style="border-right: 1px solid #ccc;">
                    <asp:UpdatePanel runat="server" ID="uppReportsMenu" UpdateMode="Conditional">
                        <Triggers>
                        </Triggers>

                        <ContentTemplate>
                            <%= GetLocalResourceObject("lblReportsSubtitle") %>

                            <div id="reportsMenu" class=""></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="col-sm-7" id="content">
                    <asp:UpdatePanel runat="server" ID="uppReport" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <Triggers>
                        </Triggers>

                        <ContentTemplate>
                            <div id="divReportContainer">
                                <div style="text-align: center;">
                                    <button type="button" class="btn btn-default btn-xs toggle-sidebar" style="float: left;" onclick="ToggleSideBar(); return false;"><%= GetLocalResourceObject("btnMenu") %> </button>
                                    <h4 id="lblReportTitle" runat="server"></h4>
                                    <br />
                                    <asp:Button ID="btnReport" runat="server" Text="" OnClick="BtnReport_Click" Style="display: none;" />
                                </div>

                                <rsweb:ReportViewer ID="reportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" ShowRefreshButton="False" Width="100%" ZoomMode="PageWidth" Height="100%" AsyncRendering="true" SizeToReportContent="false" Visible="false"></rsweb:ReportViewer>

                                <iframe id="frmReporte" width="100%" height="95%" class="iframeCollapsed" frameborder="0" runat="server" />
                            </div>

                            <div id="grvReportWaiting" style="position: absolute; margin: auto; display: none; bottom: 0px; top: 0px; left: 50%; transform: translate(-50%, 35%);">
                                <span class='fa fa-spinner fa-spin' style="font-size: 50px;"></span>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdfJsonTreeView" runat="server" />

    <nav class="navbar-fixed-bottom">
        <div class="container center-block text-center">
            <b>
                <div class="alert alert-autocloseable-msg" style="display: none;"></div>
            </b>
        </div>
    </nav>

    <script type="text/javascript">
        //*******************************//
        //          VARIABLES            // 
        //*******************************//
        var defaultData = jQuery.parseJSON(unescape($('#<%= hdfJsonTreeView.ClientID%>').val()));

        //*******************************//
        //       EVENT BINDING           // 
        //*******************************//   
        $(document).ready(function () {
            $('#reportsMenu').treeview({
                showBorder: false,
                showTags: true,
                levels: 1,
                expandIcon: 'glyphicon glyphicon-folder-close',
                collapseIcon: 'glyphicon glyphicon-folder-open',
                nodeIcon: 'glyphicon glyphicon-list-alt',
                data: defaultData,
                onNodeSelected: function (event, data) {
                    if (!isEmptyOrSpaces(data.href)) {
                        SetWaitingReport();
                        ToggleSideBar();
                        __doPostBack('<%= btnReport.UniqueID %>', data.text.concat(',', data.href));
                    }
                }
            });
        });

        function pageLoad(sender, args) {
            //////////////////////////////////////////////////////////////////////////////////////////////////
            //In this section we set the button state for btnAjaxAction
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            //////////////////////////////////////////////////////////////////////////////////////////////////
            InitializeTooltipsPopovers();
        }

        //*******************************//
        //             LOGIC             // 
        //*******************************//
        function SetFrameHeight() {
            var frame = $("#<%=frmReporte.ClientID%>");

            var rptViewer = $("#<%=reportViewer.ClientID%>");
            if ($("#sidebar").hasClass("collapsed")) {
                frame.removeClass("iframeCollapsed");
                frame.addClass("iframeExpanded");

                rptViewer.removeClass("iframeCollapsed");
                rptViewer.addClass("iframeExpanded");
            }
            else {
                frame.addClass("iframeCollapsed");
                frame.removeClass("iframeExpanded");

                rptViewer.addClass("iframeCollapsed");
                rptViewer.removeClass("iframeExpanded");
            }
        }

        function ToggleSideBar() {
            /// <summary>Process the request of toggle the side bar</summary>
            $("#sidebar").toggleClass("collapsed");
            $("#brHeader").toggleClass("headerCollapsed");
            $("#menuHeader").toggleClass("menuCollapsed");
            $("#content").toggleClass("col-sm-7");
            $("#content").toggleClass("col-sm-12");

            setTimeout(function () { SetFrameHeight(); }, 150);
        }

        function SetWaitingReport() {
            /// <summary>Process the request of set the grid as waiting style</summary>
            setTimeout(function () {
                $('#grvReportWaiting').fadeIn('fast');
                $('#divReportContainer').fadeTo('fast', 0.5);

            }, 100);
        }

        //*******************************//
        //AJAX CONCURRENCY ADMINISTRATION// 
        //*******************************//
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);

        function initializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack()) {
                args.set_cancel(true);

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
    </script>
</asp:Content>
