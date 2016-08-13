<%@ Page Title="RASTREO Paraguay - Control de vehiculos y posiciones GPS" 
	EnableViewStateMac="false" 
	Language="VB" 
	Async="false" 
	AutoEventWireup="false" 
	CodeFile="Insurance_mainGPS.aspx.vb" 
	Inherits="insurance_mainGPS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<!-- Autor: Ing. Adolfo Bresanovich || Director de R&D RASTREO Paraguay -->

<head id="GPSadminHead" runat="server">

	<link rel="shortcut icon" href="~/RASTREO.ico" type="image/x-icon"/>
	<link rel="icon" href="~/RASTREO.ico" type="image/x-icon"/>
	<title>RASTREO WebSystem - Bienvenido!</title>
	<script type="text/javascript" src="FuncionesGenerales.js"></script>    
	
</head>
<body onload='MainGPS_init_map();countdown_timer(<%= txtTimeout.text %>);'>
			<!-- CABECERA--> 
			<div id="top"> 
			
			  <div class="logo"><a href="#"><img alt="RASTREAR - The tracking solution" src="App_Themes/CENTRAL/Imagenes/logo.png" width="282" height="95" /></a></div> 
			  
 
		  </div> 
			<!-- CABECERA--> 


	<form id="RASTREOmainForm" runat="server" >
		
	<ajaxToolkit:ToolkitScriptManager AsyncPostBackTimeout="3600" ID="RASTREOToolScriptManager" ScriptMode="Release" runat="server" >
		<Scripts>
			<asp:ScriptReference Path="~/WebKit.js" />
		</Scripts>
	</ajaxToolkit:ToolkitScriptManager>
	
	
	<asp:HiddenField ID="idUsuario" runat="server" Value="0"/>
	<asp:HiddenField ID="idSeguro" runat="server" Value="0"/>
	<asp:HiddenField ID="idPersona" runat="server" Value="0"/>
	<asp:HiddenField ID="idVehiculoSeleccionado" runat="server" Value="0"/>

	<div id="RASTREO_myHeader_RASTREAR" >       
			<table style="text-align:right; width:100%"  cellpadding="0" cellspacing="0">
				<tr>  
					<td style="width:40%" align="center">
						<!--<span style="font-size:xx-small">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ϐeta</span> -->
					</td>
					<td style="width:40%">Cliente: <% Response.Write(mi_cliente)%><br />
					Usuario: <% Response.Write(mi_usuario) %></td>
					<td style="width:20%; text-align:center; border: double 2px #024579; vertical-align:middle">
						<asp:LinkButton CssClass="rastreo_linkbuttons" runat="server" ID="RASTREO_LOGOUT">Salir del sistema</asp:LinkButton>
						<asp:Image ID="imgsyslogout" runat="server" ImageUrl="~/App_Themes/CENTRAL/Imagenes/system-log-out.png" />    
						<div class="rastreo_botones">
						Total de vehiculos: 
						<asp:Label runat="server" ID="lbCantidadClientes" ForeColor="White" />
						</div>
					</td>
				</tr>
			</table>    
					   
	</div>

	<div id="RASTREO_mainwrapper">
		<asp:UpdateProgress runat="server" ID="updpg_pnlGPS" AssociatedUpdatePanelID="pnlinsurance_mainGPS">
			<ProgressTemplate>
				<div style="position:absolute;left:0px;top:0px;width:100%;height:1000%" class="modalBackground">
					<img src="./App_Themes/CENTRAL/Imagenes/ajax-loader.gif" alt="-"/>
					<label class="rastreo_nowloading">Actualizando...</label>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>    
		<div id="toptop" visible="false" />
		<asp:Label ID="errMsgs" runat="server" Width="99%" CssClass="Mensaje_de_error" 
		Visible="false"></asp:Label>
		<table width="100%" cellpadding="0" cellspacing="0">
		<tr>
			<td valign="top">  
			<!--<input id="cd_timer" class="sec" value="" readonly="readonly" />-->
			<div class="menuitem2">Próxima Actualización en           
				<!--<input id="cd_timer" class="countdown" value="Bienvenido!" readonly="readonly"/>
				<!--<asp:CheckBox runat="server" ID="chkCalles" Text="Ver direcciones cercanas" 
				ToolTip="Chequee esta casilla si desea ver la calle cercana a la posicion de sus moviles, tenga en cuenta que esta opcion degrada la velocidad de carga de datos del sistema." Checked="true" />-->
				<asp:TextBox runat="server" ID="txtTimeout" CssClass="TextBox" 
				 onchange="if (isNaN(this.value)) { this.value='30'; this.focus()}" Width="30px"
				 Text="30"
				 MaxLength="3" > 
			 </asp:TextBox>
			 segundos.
			 </div>
			<asp:Button runat="server" CssClass="rastreo_botones" ID="btnAjustar"
			 Text="Ajustar intérvalo" OnClientClick="DisableSave(this);" />
			<table width="100%" cellpadding="0" cellspacing="0" class="PopUp">
				<tr>
					<td>
						<% If permiso_REFERENCIAS Then%>
							<input value="Administrar Referencias" type="button" 
							title="Haga click aqui para crear hojas de referencia que contendran puntos de referencia a ser utilizados en las hojas de ruta."
							class="rastreo_botones" 
							onclick="AbrirDialogo('Referencias.aspx?uid=<%= idUsuario.Value %>&cid=<%= idPersona.Value %>')"/>
						<%--</td>--%>
						<% End If%>
						<% If permiso_BUSES Then%>
						<%--<td>--%>
							<input value="Control de flota de Autobuses" type="button" 
							title="Haga click aqui ver la pagina de control de flota de autobuses."
							class="rastreo_botones" 
							onclick="window.location = 'bus_main.aspx'"/>
						<% End If%>
					</td>
				</tr>
			</table>
		</td>
		</tr>
		<tr>
			<td valign="top">
				<div id="map">
				</div>
			</td>
		</tr>
	  
		<tr>
			<td>
				<asp:UpdatePanel runat="server" ID="pnlinsurance_mainGPS">
				<ContentTemplate>        
				<asp:HiddenField ID="txtfile" runat="server" />
				<asp:HiddenField ID="reffile" runat="server" />
				<asp:Timer runat="server" ID="tmr_Reload" Interval="30000" />
				<asp:CheckBox runat="server" ID="chkFiltro_Por_Identificador" AutoPostBack="true" 
						Text="Filtro por Identificador" Checked="True" />
				<asp:DropDownList runat="server" ID="ddlIdentificadorSeleccionado" CssClass="TextBox">
				</asp:DropDownList>
				<asp:ImageButton runat="server" ID="btnFiltro_Por_Identificador" ImageUrl="~/App_Themes/CENTRAL/Imagenes/view-refresh.png" />
				<br />                
				<%--<asp:SqlDataSource ID="sqldsVehiculoSeleccionado" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
				SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps WHERE id_usuarios IS NOT NULL and id_usuarios = @id_usuarios AND id_vehiculo IS NOT NULL AND id_vehiculo = @idVehiculoSeleccionado">
				<SelectParameters>
					<asp:ControlParameter ControlID="idVehiculoSeleccionado" Name="idVehiculoSeleccionado" ConvertEmptyStringToNull="false" />
					<asp:ControlParameter ControlID="idUsuario" Name="id_usuarios" ConvertEmptyStringToNull="false" />
				</SelectParameters>
				</asp:SqlDataSource>
				<asp:GridView PageSize="1"  ID="gridVehiculoSeleccionado" runat="server" 
					DataSourceID="sqldsVehiculoSeleccionado" AllowPaging="True" DataKeyNames="id_vehiculo, gps_latitud, gps_longitud"
					AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
					BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
						GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Verdana" 
						HeaderStyle-Font-Names="Verdana, Nina, Segoe, Arial Narrow"
						EmptyDataRowStyle-CssClass="Mensaje_de_error"
						Font-Size="Small" EmptyDataText="- No se ha seleccionado vehiculo para realizar seguimiento fijo-">
						<EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
					<RowStyle BackColor="#EEEEEE" ForeColor="Black" />
					<Columns>
					<asp:ButtonField Text="-" HeaderText="Detener seguimiento" CommandName="selected_vehiculo_main" ButtonType="Button" ControlStyle-CssClass="rastreo_botones_cancel" />
					<asp:TemplateField>
						<HeaderTemplate>
							Vehiculo
						</HeaderTemplate>
						<ItemTemplate>
							<a title="Haga click aqui para centrar el mapa en el móvil <%#Eval("identificador_rastreo")%> " href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
							  <%#Eval("identificador_rastreo")%> 
							</a>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField  HeaderText="Fecha y hora" DataField="gps_fecha" />
					<asp:TemplateField>
						<HeaderTemplate>
							Direccion cercana  
						</HeaderTemplate>
						<ItemTemplate>
							<%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
							<br />
							<%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
						</ItemTemplate>
					</asp:TemplateField>                    
						<asp:TemplateField>
							<HeaderTemplate>
								Vel.
							</HeaderTemplate>
							<ItemTemplate>
								<%#IIf(Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("detenido") Or Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("apagado"), "0", Eval("gps_velocidad"))%>
							</ItemTemplate>
						</asp:TemplateField>
					<asp:TemplateField>
						<HeaderTemplate> Rumbo </HeaderTemplate>
						<ItemTemplate>
							<%#Eval("gps_rumbo") %>° <%# Eval("rumbo") %>
						</ItemTemplate>
					</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Evento
							</HeaderTemplate>
							<ItemTemplate>
								<label style='background-color:<%#Eval("color_evento") %>'><%#Eval("evento")%></label>
							</ItemTemplate>
							<ItemStyle ForeColor="White" />
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Referencia cercana  
							</HeaderTemplate>
							<ItemTemplate>
									<%#gGetReferenciaCercana(Convert.ToDouble(Eval("gps_latitud")), Convert.ToDouble(Eval("gps_longitud")))%>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
					<FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
					<PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
					<SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
					<HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
					<AlternatingRowStyle BackColor="#BABABA" />
					</asp:GridView>--%>
				<asp:GridView PageSize="20" ID="gv_Lista_de_Todos_los_Vehiculos" runat="server" 
						DataSourceID="sqldsTodosLosVehiculos" AllowPaging="True" 
						DataKeyNames="identificador_rastreo, id_vehiculo, gps_longitud, gps_latitud"
						AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
						BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
						GridLines="Vertical" Width="100%" Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
						HeaderStyle-Font-Names="Nina, Segoe, Arial Narrow, Arial, Verdana"
						Font-Size="Small" EmptyDataText="El usuario no tiene vehiculos asignados."
						EmptyDataRowStyle-CssClass="Mensaje_de_error">
						<EmptyDataRowStyle CssClass="Mensaje_de_error" HorizontalAlign="Center" />
						<RowStyle BackColor="#EEEEEE" ForeColor="Black" />
						<Columns>
						<%--<asp:CommandField SelectText="+" ShowSelectButton="true" ButtonType="Button" ControlStyle-Font-Size="X-Small"
						 ControlStyle-CssClass="rastreo_botones_save" HeaderText=""/>--%>
						<asp:TemplateField> 
							<HeaderTemplate> Vehiculo </HeaderTemplate>
							<ItemTemplate>
								<ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverTareas" TargetControlID="lnkTareas" PopupControlID="pnlTareas" PopupPosition="Top" OffsetX="35" OffsetY="15">
								</ajaxToolkit:HoverMenuExtender>
								<asp:Label runat="server" ID="lnkTareas">
								<a title="Haga click aqui para centrar el mapa en el móvil <%#Eval("identificador_rastreo")%> " href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>);">
								  <%#Eval("identificador_rastreo")%> 
								</a>
								</asp:Label>
								<asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="RoyalBlue" Font-Size="X-Small" ForeColor="White" ID="pnlTareas" runat="server">
									<a title="Haga click aqui para centrar el mapa en el móvil <%#Eval("identificador_rastreo")%> " class="OpcionTarea" href="#toptop" onclick="map_panTo(<%# Convert.ToString(Eval("gps_longitud")).Replace(",",".") %>,<%# Convert.ToString(Eval("gps_latitud")).Replace(",",".") %>)">
									 Posicionar
									</a>
									<br />
									<!--
									<a title="Haga click aqui para obtener el recorrido histórico del móvil <%#Eval("identificador_rastreo")%> " class="OpcionTarea" onclick="AbrirDialogo('Historico.aspx?vid=<%#Eval("id_vehiculo")%>')">
									 Histórico
									</a>
									<br />
									-->
									</asp:Panel>
								<ajaxToolkit:HoverMenuExtender runat="server" ID="masterHuverDatos" TargetControlID="lnkTareas" PopupControlID="pnlDatos" PopupPosition="Right" OffsetX="80" OffsetY="-30">
								</ajaxToolkit:HoverMenuExtender>
								<asp:Panel BorderColor="Black" BorderWidth="2px" BackColor="RoyalBlue" Font-Size="X-Small" ForeColor="White" ID="pnlDatos" runat="server">
									<table cellpadding="0" cellspacing="0">
										<tr><td>Modelo:</td><td><%#Eval("marca")%></td></tr>
										<tr><td>Marca:</td><td><%#Eval("modelo")%></td></tr>
										<tr><td>Patente:</td><td><%#Eval("matricula")%></td></tr>
									</table>
								</asp:Panel>
							</ItemTemplate>
						</asp:TemplateField>                
						<asp:BoundField HeaderText="Fecha y hora" DataField="gps_fecha" />
						<asp:TemplateField>
									<HeaderTemplate>
										Direccion cercana  
									</HeaderTemplate>
									<ItemTemplate>
										<%#GeoPos(Convert.ToString(Eval("gps_latitud")), Convert.ToString(Eval("gps_longitud")))%>
										<br />
										<%#Convert.ToString(Eval("gps_latitud")).Replace(",", ".")%>,<%#Convert.ToString(Eval("gps_longitud")).Replace(",", ".")%>
									</ItemTemplate>
								</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Vel.
							</HeaderTemplate>
							<ItemTemplate>
								<%#IIf(Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("detenido") Or Convert.ToString(Eval("evento")).ToLowerInvariant().Contains("apagado"), "0", Eval("gps_velocidad"))%>
							</ItemTemplate>
						</asp:TemplateField>
						<%--<asp:BoundField HeaderText="Velocidad" DataField="gps_velocidad" />--%>
						<asp:TemplateField>
							<HeaderTemplate> Rumbo </HeaderTemplate>
							<ItemTemplate>
								<%#Eval("gps_rumbo") %>° <%# Eval("rumbo") %>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Evento
							</HeaderTemplate>
							<ItemTemplate>
								<label style='background-color:<%#Eval("color_evento") %>'><%#Eval("evento")%></label>
							</ItemTemplate>
							<ItemStyle ForeColor="White"/>
						</asp:TemplateField>
						<asp:TemplateField>
							<HeaderTemplate>
								Referencia cercana  
							</HeaderTemplate>
							<ItemTemplate>
									<%#gGetReferenciaCercana(Eval("gps_latitud"), Eval("gps_longitud"))%>
							</ItemTemplate>
						</asp:TemplateField>
						</Columns>
						<FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
						<PagerStyle BackColor="Navy" ForeColor="White" HorizontalAlign="Center" />
						<SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
						<HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
						<AlternatingRowStyle BackColor="#BABABA" />
						</asp:GridView>
						<asp:SqlDataSource ID="sqldsTodosLosVehiculos" runat="server" ConnectionString="<%$ ConnectionStrings:CS_RASTREOSystem %>" ProviderName="<%$ ConnectionStrings:CS_RASTREOSystem.ProviderName %>"
							SelectCommand="SELECT DISTINCT ON (identificador_rastreo) * FROM rsview_vehiculo_bandejaentrada_cliente_equipogps WHERE proviene_de = @proviene_de">
							<SelectParameters>
								<asp:ControlParameter ControlID="idSeguro" Name="proviene_de"/>
							</SelectParameters>
						</asp:SqlDataSource>
			</ContentTemplate>
			</asp:UpdatePanel>
		</td>
		</tr>
		</table>
	   </div>
	</form>
	<script src="http://maps.google.com/maps/api/js?v=3.2&sensor=false"></script>
	<script type="text/javascript" src="OpenLayers.js"></script>
	<script type="text/javascript" src="FuncionesGPS.js"></script>
	<%--<script type="text/javascript" src="http://www.openlayers.net/api/OpenLayers.js"></script>--%>
	<%--<script src='http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1'></script>--%>
	<%--<script src="http://api.maps.yahoo.com/ajaxymap?v=3.0&appid=euzuro-openlayers"></script>--%>
	
<!-- PIE--> 
	<div id="pie" align="center"> 
		<div class="footnotes">&copy; 2010 Rastreo.com.py - <a href="http://www.rastreo.com.py/contactenos.php">Contactenos</a><% If not miUsuario is nothing Then%> - <a href="/manual/manual.pdf">Manual en linea<img src="App_Themes/CENTRAL/Imagenes/help-browser.png"  alt="[?]"/></a><%End If%></div> 
	</div> 
<!-- PIE-->     


</body>
</html>




