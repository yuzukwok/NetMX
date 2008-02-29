﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Theme="Default" %>

<%@ Register Src="~/MBeanUI.ascx" TagPrefix="uc" TagName="MBeanUI" %>
<%@ Register Assembly="App_Code" Namespace="Controls" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
		<%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
		<uc:MBeanServerProxy ID="proxy" runat="server" ServiceUrl="tcp://localhost:1234/MBeanServer.tcp" />
		<div>
			<uc:MBeanUI runat="server" ID="sampleMBeanUI" ObjectName="Sample:" MBeanServerProxyID="proxy" />						
		</div>
	</form>
</body>
</html>
