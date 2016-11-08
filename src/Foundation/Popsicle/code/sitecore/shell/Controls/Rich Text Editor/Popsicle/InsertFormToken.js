Telerik.Web.UI.Editor.CommandList["ShowForms"] = function (commandName, editor, args) {

    var callbackFunction = function (sender, returnValue) {
        if (returnValue) {
            editor.pasteHtml("<span class='form-token' style='width: 200px; border: 1px dashed #bb0000; color: blue; background-color: #fafafa;'>$" + returnValue + "$</span>&nbsp;");
        }
    }

    editor.showExternalDialog(
        "/sitecore/client/your apps/popsicle/dialogs/insertformdialog",
        null,
        400,
        600,
        callbackFunction,
        null,
        'Insert Marketing Automation Form',
        true,
        Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
        false,
        false);
}