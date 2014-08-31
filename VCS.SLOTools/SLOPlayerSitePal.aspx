<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>VCS.SLOPlayerSitePal</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
        }
        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">

        function sayTextSitePal(text, v, l, e) {

            voice = v;
            language = l;
            engine = e;

            loadText(text, voice, language, engine);
        }

        function vh_slideBegin(sceneNumber) {

            //alert("vh_slideBegin");
            
            SLOPlayer.Content.SL2JS.EngineLoaded();
        }

        function vh_sceneLoaded(sceneNumber) {

            //alert('vh_sceneLoaded');
            SLOPlayer.Content.SL2JS.EngineLoaded();

        } 

        function stopSayTextSitePal() {

            stopSpeech();

            SLOPlayer.Content.SL2JS.SayTextEnded();
        }

        function vh_audioEnded() {

            SLOPlayer.Content.SL2JS.SayTextEnded();
        }

        var voice;
        var language;
        var engine;

        function vh_ttsLoaded(args) {

//            alert("vh_ttsLoaded");

            //loaded text to sppech audio =  args
            //loaded text to speech is ready
            SLOPlayer.Content.SL2JS.SayTextStarted();

            sayText(args, voice, language, engine, 'D', -1);
        }

        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }


        //calling Silverlight method
        var SLOPlayer = null;

        function pluginLoaded(sender, args) {
            SLOPlayer = sender.getHost();
        }
    </script>
</head>
<body style="background-color: #C7D8FA">
    <form id="form1" runat="server" style="height: 100%">
    <div id="silverlightControlHost" style="margin-top: 20px">
        <div style="width: 100%; margin: 0">
            <div style="margin: auto; width: 850px; height: 590px">
                <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
                    <param name="source" value="ClientBin/SLOPlayerSitePal.xap" />
                    <param name="onError" value="onSilverlightError" />
                    <param name="background" value="white" />
                    <param name="minRuntimeVersion" value="4.0.50826.0" />
                    <param name="autoUpgrade" value="true" />
                    <param name="onLoad" value="pluginLoaded" />
                    <param name="InitParams" value="ip=<%= Request.UserHostAddress %>, serviceshost=http://localhost/VCS.Services/, affectiveEmotiveServicesUrl=http://iwtalice.crmpa.unisa.it/iwt/remoteservices/externalservices/affectiveemotiveservices.asmx, learnerModelServicesUrl=http://iwtalice.crmpa.unisa.it/iwt/remoteservices/externalservices/LearnerModelServices.asmx" />
                    <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration: none">
                        <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style: none" />
                    </a>
                </object>
                <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px; border: 0px"></iframe>
            </div>
        </div>
    </div>
    </form>
    <table width="60%" border="0" cellspacing="0" cellpadding="0" align="CENTER">
        <tr>
            <td align="CENTER">
                <!-- embed code -->
                <script language="JavaScript" type="text/javascript" src="http://vhss-d.oddcast.com/vhost_embed_functions_v2.php?acc=3376270&js=1"></script>
                <div style="height: 0px">
<%--                <script language="JavaScript" type="text/javascript">AC_VHost_Embed(3376270, 300, 400, '', 1, 1, 2290963, 0, 1, 0, 'f75fac3c99e4aefe1f6b7052ff3a4dcf', 9);</script> --%>
                    <script language="JavaScript" type="text/javascript">AC_VHost_Embed(3376270, 300, 400, '', 1, 1, 2290963, 0, 1, 0, '839dbfbf0c950d73e0996f8545a82c93', 9);</script>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>
