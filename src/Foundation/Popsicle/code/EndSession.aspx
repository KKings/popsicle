<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Sitecore.Analytics.Pipelines.VisitEnd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>End Session</title>
    
    <script runat="server">

        protected void EndSession(Object o, EventArgs e)
        {
            HttpContext.Current.Session.Abandon();

            this.Label.Text = $"Cleared at {DateTime.Now}";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h1>End Session</h1>
        <asp:Button runat="server" OnClick="EndSession" name="EndSessionButton" Text="End Session"/>
        <asp:Label runat="server" ID="Label" Text=""/>
    </form>
</body>
</html>
