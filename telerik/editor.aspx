<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editor.aspx.cs" Inherits="editor" %>

<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/telerik/jquery-1.10.2.js" type="text/javascript"></script>
    <style>
        .RadWindow {
            left: 10px !important;
        }
        p {
            margin: 0;
            border: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <rad:RadScriptManager runat="server" ID="scriptManager"></rad:RadScriptManager>
        <div>
            <asp:UpdatePanel runat="server" ID="updatePanel">
                <ContentTemplate>
                    <rad:RadEditor DialogHandlerUrl="Telerik.Web.UI.DialogHandler.axd" Width="682px" Height="550px" OnClientCommandExecuted="OnClientCommandExecuted" OnClientDomChange="OnClientCommandExecuted" OnClientPasteHtml="OnClientCommandExecuted" OnClientModeChange="OnClientCommandExecuted" OnClientSelectionChange="OnClientCommandExecuted" OnClientLoad="OnClientLoad" AllowScripts="true" ID="textBlock" runat="server"
                        NewLineMode="P">
                        <SpellCheckSettings SpellCheckProvider="EditDistanceProvider"></SpellCheckSettings>
                        <Languages>
                            <rad:SpellCheckerLanguage Code="ru-RU" Title="Russian" />
                        </Languages>
                        <Content></Content>
                        <ImageManager ViewPaths="~/content/Catalog/Editor/Images" UploadPaths="~/content/Catalog/Editor/Images" DeletePaths="~/content/Catalog/Editor/Images" MaxUploadFileSize="209715200"></ImageManager>
                        <DocumentManager ViewPaths="~/content/Catalog/Editor/Docs" UploadPaths="~/content/Catalog/Editor/Docs" DeletePaths="~/content/Catalog/Editor/Docs" MaxUploadFileSize="209715200" />
                        <FlashManager ViewPaths="~/content/Catalog/Editor/Flash" UploadPaths="~/content/Catalog/Editor/Flash" DeletePaths="~/content/Catalog/Editor/Flash" MaxUploadFileSize="209715200" />
                        <MediaManager ViewPaths="~/content/Catalog/Editor/Media" UploadPaths="~/content/Catalog/Editor/Media" DeletePaths="~/content/Catalog/Editor/Media" MaxUploadFileSize="209715200" />
                    </rad:RadEditor>
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </form>
    <script type="text/javascript">

        function saveChanges(editor) {
            var editor = $find("<%=textBlock.ClientID%>");
            
            parent.saveTelerikContent('<%=Request["targetcolumn"]%>', editor.get_html());
        }

        function OnClientCommandExecuted(editor, commandName, oTool) {
            saveChanges(editor);
        }

        function OnClientLoad(editor) {
            
            var htmlArea = editor.get_textArea();
            var contentArea = editor.get_contentArea();

            if (htmlArea) {
                $(htmlArea).bind('keyup propertychange change paste click', function () {
                    saveChanges(editor);
                });
/*
                htmlArea.attachEvent('onkeydown', function () { saveChanges(editor); });
                htmlArea.attachEvent('onchange', function () { saveChanges(editor); });
                htmlArea.attachEvent('onpaste', function () { saveChanges(editor); });
*/
            }

            if (contentArea) {
                $(contentArea).on('keyup propertychange change paste click', function () {
                    saveChanges(editor);
                });
                /*contentArea.attachEvent('onkeydown', function () { saveChanges(editor); });
                contentArea.attachEvent('onchange', function () { saveChanges(editor); });
                contentArea.attachEvent('onpaste', function () { saveChanges(editor); });*/
            }
        }

    </script>
</body>
</html>
