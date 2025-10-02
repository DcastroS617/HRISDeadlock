<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Error403.aspx.cs" Inherits="HRIS.ErrorPages.Error403" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Application Error</title>
    <link href="/Content/css/Errors.css" rel="stylesheet" />
    <link href="/Content/css/bootstrap.css" rel="stylesheet" />

</head>
<body>
    <script src="/Scripts/jquery-3.2.1.min.js"></script>
    <script src="/Scripts/bootstrap.js"></script>
    <div class="container">
        <div class="row">
            <div class="span12">
                <div class="hero-unit center">
                    <h1><%=GetLocalResourceObject("ErrorTitle") %></h1>
                    <div style="margin-top:-10px;"><font face="Tahoma" color="red">Error 403</font></div>
                    <br />
                    <p><%=GetLocalResourceObject("ErrorDescription") %></p>
                    <p><b><%=GetLocalResourceObject("ErrorInstruction") %></b></p>
                    <a href="/Default.aspx" class="btn btn-large btn-info"><i class="icon-home icon-white"></i><%=GetLocalResourceObject("home") %></a>
                </div>
                <br />

            </div>
        </div>
    </div>
</body>
</html>
