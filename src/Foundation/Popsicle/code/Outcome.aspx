<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Sitecore.Analytics.Outcome" %>
<%@ Import Namespace="Sitecore.Configuration" %>
<%@ Import Namespace="Sitecore.Analytics.Outcome.Model" %>
<%@ Import Namespace="Sitecore.Analytics" %>
<%@ Import Namespace="Sitecore.Common" %>
<%@ Import Namespace="Sitecore.Data" %>
<%@ Import Namespace="Sitecore.Analytics.Outcome.Extensions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Outcomes</title>
    
    <script runat="server">

        protected void Register(Object o, EventArgs e)
        {
            ID definitionId = null;
            if (Sitecore.Data.ID.TryParse(this.DefinitionId.Text, out definitionId))
            {
                var manager = Factory.CreateObject("outcome/outcomeManager", true) as OutcomeManager;
                var outcome = new ContactOutcome(Sitecore.Data.ID.NewID, definitionId, Tracker.Current.Contact.ContactId.ToID())
                {
                    DateTime = DateTime.UtcNow,
                    MonetaryValue = 1000
                };

                Tracker.Current.RegisterContactOutcome(outcome);

                this.Label.Text = "";
            }
            else
            {
                this.Label.Text = "Invalid ID";
            }
        }
        
        protected void RegisterImmediately(Object o, EventArgs e)
        {
            ID definitionId = null;
            if (Sitecore.Data.ID.TryParse(this.DefinitionId.Text, out definitionId))
            {
                var manager = Factory.CreateObject("outcome/outcomeManager", true) as OutcomeManager;

                var outcome = new ContactOutcome(Sitecore.Data.ID.NewID, definitionId, Tracker.Current.Contact.ContactId.ToID())
                {
                    DateTime = DateTime.UtcNow,
                    MonetaryValue = 1000
                };
                
                manager?.Save(outcome);

                this.Label.Text = "";
            }
            else
            {
                this.Label.Text = "Invalid ID";
            }
        }

        protected void GetOutcomes(object obj, EventArgs e)
        {
            var  outcomes = Tracker.Current.GetContactOutcomes();

            this.Outcomes.Text = String.Join("<br/><br/>", outcomes.Select(o => $"{o.DateTime} - {o.DefinitionId}"));
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Register an Outcome</h1>
        <asp:TextBox runat="server" ID="DefinitionId" Width="500"></asp:TextBox>
        <asp:Button runat="server" OnClick="Register" name="Register" Text="Register"/>
        <asp:Button runat="server" OnClick="RegisterImmediately" name="RegisterImmediately" Text="Register Immediately"/>
        <asp:Label runat="server" ID="Label" Text=""/>
        <hr/>
        <asp:Button runat="server" OnClick="GetOutcomes" name="GetOutcomes" Text="Get Outcomes in Session"/>
        <br/>
        <asp:Label runat="server" ID="Outcomes"></asp:Label>
    </form>
</body>
</html>
