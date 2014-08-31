<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSitePal.aspx.cs" Inherits="VCS.TestSitePal" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
    <title>Oddcast Support - loadText()</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="http://www.oddcast.com/support/styles/master.css">
    <script language="JavaScript" src="http://www.oddcast.com/support/scripts/master.js"></script>
</head>
<body>
    <table width="60%" border="0" cellspacing="0" cellpadding="0" align="CENTER">
        <tr>
            <td align="CENTER">
                <!-- embed code -->
                <script language="JavaScript" type="text/javascript" src="http://vhss-d.oddcast.com/vhost_embed_functions_v2.php?acc=3376270&js=1"></script>
                <div style="height: 0px">
                    <script language="JavaScript" type="text/javascript">
                        AC_VHost_Embed(3376270, 300, 400, '', 1, 1, 2290963, 0, 1, 0, 'f75fac3c99e4aefe1f6b7052ff3a4dcf', 9);</script>
                </div>
                <!-- listener function -->
                <script language="JavaScript">
                    function vh_ttsLoaded(args) {
                        //loaded text to sppech audio =  args
                        //loaded text to speech is ready
                        alert('The text "' + args + '" is loaded');
                        sayText(args, 2, 1, 3);
                    }

                </script>
            </td>
        </tr>
    </table>
    Call the loadAudio() command:
    <a href="javascript:loadText('The text is ready',2,1,3);">
        loadText('The text is ready',2,1,3)</a>
</body>
</html>
