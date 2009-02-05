<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LSharpAzure_WebRole._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LSharp in Windows Azure</title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body>
    <h1><img src="lsharp.png" alt="L Sharp Logo"/>L Sharp .NET in Windows Azure</h1>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div>
    <p>Paste your L Sharp code here and press Eval.</p>
        
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:TextBox runat="server" ID="InputTextBox" Height="122px" 
                TextMode="MultiLine" Width="740px"></asp:TextBox> &nbsp;
            <asp:Button runat="server" Text="Eval" ID="EvalButton" 
            onclick="EvalButton_Click" style="height: 26px" />
            <br />
            <p>Results below:</p>
        <asp:TextBox ID="OutputTextBox" runat="server" Height="139px" TextMode="MultiLine" 
                Width="736px"></asp:TextBox>
                </ContentTemplate>
 </asp:UpdatePanel>
                    
                <p>Here are some things you might like to try:</p>
                <pre>(+ 1 2 3)</pre> or 
                <pre>(prn "Hello World")</pre> or
                <pre>(System.Environment.OSVersion) </pre> or
                <pre>
;;; An RSS Reader in 4 lines of code?

(reference "System.Xml")

(= news (new "System.Xml.XmlDocument"))
(.load news "http://www.theregister.co.uk/headlines.rss")

(map (fn (x) (.innertext x)) (.selectnodes news  "/rss/channel/item/title"))
                </pre>
        <p>DISCLAIMER - I have no idea whether this will stay up for long - it's probably hugely hackable, so please use at your own risk.
        For more information about LSharp and source code, see <a href="http://www.lsharp.org">www.lsharp.org</a>.
        Thanks to all the folks at Microsoft for giving me early access to the <a href="http://www.microsoft.com/azure">Windows Azure Services Platform</a>.
        </p>
        <p><small>Copyright &copy; 2009 Rob Blackwell </small></p>
    </div>
    </form>
</body>
</html>
