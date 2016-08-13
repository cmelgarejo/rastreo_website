/*
'==============================================================================================================
'  Autor: CLrSoft de Christian Melgarejo.
'  Clase c# rsview_recorridos para la vista rsview_recorridos.
'  Base soportada: PostgreSqlEntity.
'  Versión: # (1.3.0.9)
'==============================================================================================================
*/

using System;
using System.Data;
using Npgsql;
using System.Collections;
using System.Collections.Specialized;

using MyGeneration.dOOdads;

namespace RASTREOmw
{
	public class rsview_recorridos : PostgreSqlEntity
	{
		public rsview_recorridos(String laCadenaDeConexion)
		{
			this.QuerySource = "rsview_recorridos";
			this.MappingName = "rsview_recorridos";
			this.ConnectionString = laCadenaDeConexion;
		}	
	
		//=================================================================
		//  	public procedure LoadAll() As Boolean
		//=================================================================
		//  Carga todos los registros en la base de datos, y coloca la propiedad currentRow en la primera fila
		//=================================================================
		public bool LoadAll() 
		{
			return base.Query.Load();
		}
		
		public override void FlushData()
		{
			this._whereClause = null;
			this._aggregateClause = null;
			base.FlushData();
		}
	
		#region Parameters
		protected class Parameters
		{
			
			public static NpgsqlParameter Id_evento
			{
				get
				{
					return new NpgsqlParameter("Id_evento", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Evento
			{
				get
				{
					return new NpgsqlParameter("Evento", NpgsqlTypes.NpgsqlDbType.Varchar, 0);
				}
			}
			
			public static NpgsqlParameter Color_evento
			{
				get
				{
					return new NpgsqlParameter("Color_evento", NpgsqlTypes.NpgsqlDbType.Varchar, 16);
				}
			}
			
			public static NpgsqlParameter Evento_habilitado
			{
				get
				{
					return new NpgsqlParameter("Evento_habilitado", NpgsqlTypes.NpgsqlDbType.Boolean, 0);
				}
			}
			
			public static NpgsqlParameter Idrastreogps_reportes
			{
				get
				{
					return new NpgsqlParameter("Idrastreogps_reportes", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Id_equipogps
			{
				get
				{
					return new NpgsqlParameter("Id_equipogps", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Gps_longitud
			{
				get
				{
					return new NpgsqlParameter("Gps_longitud", NpgsqlTypes.NpgsqlDbType.Double, 0);
				}
			}
			
			public static NpgsqlParameter Gps_latitud
			{
				get
				{
					return new NpgsqlParameter("Gps_latitud", NpgsqlTypes.NpgsqlDbType.Double, 0);
				}
			}
			
			public static NpgsqlParameter Gps_fechahora_reporte
			{
				get
				{
					return new NpgsqlParameter("Gps_fechahora_reporte", NpgsqlTypes.NpgsqlDbType.Timestamp, 0);
				}
			}
			
			public static NpgsqlParameter Gps_velocidad
			{
				get
				{
					return new NpgsqlParameter("Gps_velocidad", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Gps_rumbo
			{
				get
				{
					return new NpgsqlParameter("Gps_rumbo", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Rumbo
			{
				get
				{
					return new NpgsqlParameter("Rumbo", NpgsqlTypes.NpgsqlDbType.Text, 0);
				}
			}
			
			public static NpgsqlParameter Gps_evento
			{
				get
				{
					return new NpgsqlParameter("Gps_evento", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_edaddeldato
			{
				get
				{
					return new NpgsqlParameter("Gps_edaddeldato", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_hdop
			{
				get
				{
					return new NpgsqlParameter("Gps_hdop", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_satstatus
			{
				get
				{
					return new NpgsqlParameter("Gps_satstatus", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_gsmstatus
			{
				get
				{
					return new NpgsqlParameter("Gps_gsmstatus", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_estado_io
			{
				get
				{
					return new NpgsqlParameter("Gps_estado_io", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_tipodeposicion
			{
				get
				{
					return new NpgsqlParameter("Gps_tipodeposicion", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Gps_ip
			{
				get
				{
					return new NpgsqlParameter("Gps_ip", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
				}
			}
			
			public static NpgsqlParameter Gps_obs
			{
				get
				{
					return new NpgsqlParameter("Gps_obs", NpgsqlTypes.NpgsqlDbType.Text, 0);
				}
			}
			
			public static NpgsqlParameter Idcliente
			{
				get
				{
					return new NpgsqlParameter("Idcliente", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Icono
			{
				get
				{
					return new NpgsqlParameter("Icono", NpgsqlTypes.NpgsqlDbType.Text, 0);
				}
			}
			
			public static NpgsqlParameter Tipo_de_vehiculo
			{
				get
				{
					return new NpgsqlParameter("Tipo_de_vehiculo", NpgsqlTypes.NpgsqlDbType.Varchar, 512);
				}
			}
			
			public static NpgsqlParameter Cliente
			{
				get
				{
					return new NpgsqlParameter("Cliente", NpgsqlTypes.NpgsqlDbType.Varchar, 0);
				}
			}
			
			public static NpgsqlParameter Estado_cliente
			{
				get
				{
					return new NpgsqlParameter("Estado_cliente", NpgsqlTypes.NpgsqlDbType.Boolean, 0);
				}
			}
			
			public static NpgsqlParameter Id_vehiculo
			{
				get
				{
					return new NpgsqlParameter("Id_vehiculo", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Identificador_rastreo
			{
				get
				{
					return new NpgsqlParameter("Identificador_rastreo", NpgsqlTypes.NpgsqlDbType.Varchar, 1024);
				}
			}
			
			public static NpgsqlParameter Consumo_aprox
			{
				get
				{
					return new NpgsqlParameter("Consumo_aprox", NpgsqlTypes.NpgsqlDbType.Double, 0);
				}
			}
			
			public static NpgsqlParameter Anho
			{
				get
				{
					return new NpgsqlParameter("Anho", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Chasis
			{
				get
				{
					return new NpgsqlParameter("Chasis", NpgsqlTypes.NpgsqlDbType.Varchar, 512);
				}
			}
			
			public static NpgsqlParameter Color
			{
				get
				{
					return new NpgsqlParameter("Color", NpgsqlTypes.NpgsqlDbType.Varchar, 32);
				}
			}
			
			public static NpgsqlParameter Matricula
			{
				get
				{
					return new NpgsqlParameter("Matricula", NpgsqlTypes.NpgsqlDbType.Varchar, 128);
				}
			}
			
			public static NpgsqlParameter Kilometraje
			{
				get
				{
					return new NpgsqlParameter("Kilometraje", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Activo
			{
				get
				{
					return new NpgsqlParameter("Activo", NpgsqlTypes.NpgsqlDbType.Boolean, 0);
				}
			}
			
			public static NpgsqlParameter Idmarca
			{
				get
				{
					return new NpgsqlParameter("Idmarca", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Idmodelo
			{
				get
				{
					return new NpgsqlParameter("Idmodelo", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Marca
			{
				get
				{
					return new NpgsqlParameter("Marca", NpgsqlTypes.NpgsqlDbType.Varchar, 1024);
				}
			}
			
			public static NpgsqlParameter Modelo
			{
				get
				{
					return new NpgsqlParameter("Modelo", NpgsqlTypes.NpgsqlDbType.Varchar, 1024);
				}
			}
			
			public static NpgsqlParameter Idequipogps
			{
				get
				{
					return new NpgsqlParameter("Idequipogps", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Id_tipoequipogps
			{
				get
				{
					return new NpgsqlParameter("Id_tipoequipogps", NpgsqlTypes.NpgsqlDbType.Integer, 0);
				}
			}
			
			public static NpgsqlParameter Tipoequipogps
			{
				get
				{
					return new NpgsqlParameter("Tipoequipogps", NpgsqlTypes.NpgsqlDbType.Varchar, 1024);
				}
			}
			
			public static NpgsqlParameter Tipo_de_reportegps
			{
				get
				{
					return new NpgsqlParameter("Tipo_de_reportegps", NpgsqlTypes.NpgsqlDbType.Varchar, 64);
				}
			}
			
			public static NpgsqlParameter Id_equipo_gps
			{
				get
				{
					return new NpgsqlParameter("Id_equipo_gps", NpgsqlTypes.NpgsqlDbType.Varchar, 1024);
				}
			}
			
			public static NpgsqlParameter Nro_serie_gps
			{
				get
				{
					return new NpgsqlParameter("Nro_serie_gps", NpgsqlTypes.NpgsqlDbType.Varchar, 1024);
				}
			}
			
		}
		#endregion	
	
		#region ColumnNames
		public class ColumnNames
		{  
            public const string Id_evento = "id_evento";
            public const string Evento = "evento";
            public const string Color_evento = "color_evento";
            public const string Evento_habilitado = "evento_habilitado";
            public const string Idrastreogps_reportes = "idrastreogps_reportes";
            public const string Id_equipogps = "id_equipogps";
            public const string Gps_longitud = "gps_longitud";
            public const string Gps_latitud = "gps_latitud";
            public const string Gps_fechahora_reporte = "gps_fechahora_reporte";
            public const string Gps_velocidad = "gps_velocidad";
            public const string Gps_rumbo = "gps_rumbo";
            public const string Rumbo = "rumbo";
            public const string Gps_evento = "gps_evento";
            public const string Gps_edaddeldato = "gps_edaddeldato";
            public const string Gps_hdop = "gps_hdop";
            public const string Gps_satstatus = "gps_satstatus";
            public const string Gps_gsmstatus = "gps_gsmstatus";
            public const string Gps_estado_io = "gps_estado_io";
            public const string Gps_tipodeposicion = "gps_tipodeposicion";
            public const string Gps_ip = "gps_ip";
            public const string Gps_obs = "gps_obs";
            public const string Idcliente = "idcliente";
            public const string Icono = "icono";
            public const string Tipo_de_vehiculo = "tipo_de_vehiculo";
            public const string Cliente = "cliente";
            public const string Estado_cliente = "estado_cliente";
            public const string Id_vehiculo = "id_vehiculo";
            public const string Identificador_rastreo = "identificador_rastreo";
            public const string Consumo_aprox = "consumo_aprox";
            public const string Anho = "anho";
            public const string Chasis = "chasis";
            public const string Color = "color";
            public const string Matricula = "matricula";
            public const string Kilometraje = "kilometraje";
            public const string Activo = "activo";
            public const string Idmarca = "idmarca";
            public const string Idmodelo = "idmodelo";
            public const string Marca = "marca";
            public const string Modelo = "modelo";
            public const string Idequipogps = "idequipogps";
            public const string Id_tipoequipogps = "id_tipoequipogps";
            public const string Tipoequipogps = "tipoequipogps";
            public const string Tipo_de_reportegps = "tipo_de_reportegps";
            public const string Id_equipo_gps = "id_equipo_gps";
            public const string Nro_serie_gps = "nro_serie_gps";

			static public string ToPropertyName(string columnName)
			{
				if(ht == null)
				{
					ht = new Hashtable();
					
					ht[Id_evento] = rsview_recorridos.PropertyNames.Id_evento;
					ht[Evento] = rsview_recorridos.PropertyNames.Evento;
					ht[Color_evento] = rsview_recorridos.PropertyNames.Color_evento;
					ht[Evento_habilitado] = rsview_recorridos.PropertyNames.Evento_habilitado;
					ht[Idrastreogps_reportes] = rsview_recorridos.PropertyNames.Idrastreogps_reportes;
					ht[Id_equipogps] = rsview_recorridos.PropertyNames.Id_equipogps;
					ht[Gps_longitud] = rsview_recorridos.PropertyNames.Gps_longitud;
					ht[Gps_latitud] = rsview_recorridos.PropertyNames.Gps_latitud;
					ht[Gps_fechahora_reporte] = rsview_recorridos.PropertyNames.Gps_fechahora_reporte;
					ht[Gps_velocidad] = rsview_recorridos.PropertyNames.Gps_velocidad;
					ht[Gps_rumbo] = rsview_recorridos.PropertyNames.Gps_rumbo;
					ht[Rumbo] = rsview_recorridos.PropertyNames.Rumbo;
					ht[Gps_evento] = rsview_recorridos.PropertyNames.Gps_evento;
					ht[Gps_edaddeldato] = rsview_recorridos.PropertyNames.Gps_edaddeldato;
					ht[Gps_hdop] = rsview_recorridos.PropertyNames.Gps_hdop;
					ht[Gps_satstatus] = rsview_recorridos.PropertyNames.Gps_satstatus;
					ht[Gps_gsmstatus] = rsview_recorridos.PropertyNames.Gps_gsmstatus;
					ht[Gps_estado_io] = rsview_recorridos.PropertyNames.Gps_estado_io;
					ht[Gps_tipodeposicion] = rsview_recorridos.PropertyNames.Gps_tipodeposicion;
					ht[Gps_ip] = rsview_recorridos.PropertyNames.Gps_ip;
					ht[Gps_obs] = rsview_recorridos.PropertyNames.Gps_obs;
					ht[Idcliente] = rsview_recorridos.PropertyNames.Idcliente;
					ht[Icono] = rsview_recorridos.PropertyNames.Icono;
					ht[Tipo_de_vehiculo] = rsview_recorridos.PropertyNames.Tipo_de_vehiculo;
					ht[Cliente] = rsview_recorridos.PropertyNames.Cliente;
					ht[Estado_cliente] = rsview_recorridos.PropertyNames.Estado_cliente;
					ht[Id_vehiculo] = rsview_recorridos.PropertyNames.Id_vehiculo;
					ht[Identificador_rastreo] = rsview_recorridos.PropertyNames.Identificador_rastreo;
					ht[Consumo_aprox] = rsview_recorridos.PropertyNames.Consumo_aprox;
					ht[Anho] = rsview_recorridos.PropertyNames.Anho;
					ht[Chasis] = rsview_recorridos.PropertyNames.Chasis;
					ht[Color] = rsview_recorridos.PropertyNames.Color;
					ht[Matricula] = rsview_recorridos.PropertyNames.Matricula;
					ht[Kilometraje] = rsview_recorridos.PropertyNames.Kilometraje;
					ht[Activo] = rsview_recorridos.PropertyNames.Activo;
					ht[Idmarca] = rsview_recorridos.PropertyNames.Idmarca;
					ht[Idmodelo] = rsview_recorridos.PropertyNames.Idmodelo;
					ht[Marca] = rsview_recorridos.PropertyNames.Marca;
					ht[Modelo] = rsview_recorridos.PropertyNames.Modelo;
					ht[Idequipogps] = rsview_recorridos.PropertyNames.Idequipogps;
					ht[Id_tipoequipogps] = rsview_recorridos.PropertyNames.Id_tipoequipogps;
					ht[Tipoequipogps] = rsview_recorridos.PropertyNames.Tipoequipogps;
					ht[Tipo_de_reportegps] = rsview_recorridos.PropertyNames.Tipo_de_reportegps;
					ht[Id_equipo_gps] = rsview_recorridos.PropertyNames.Id_equipo_gps;
					ht[Nro_serie_gps] = rsview_recorridos.PropertyNames.Nro_serie_gps;

				}
				return (string)ht[columnName];
			}

			static private Hashtable ht = null;			 
		}
		#endregion
		
		#region PropertyNames
		public class PropertyNames
		{  
            public const string Id_evento = "Id_evento";
            public const string Evento = "Evento";
            public const string Color_evento = "Color_evento";
            public const string Evento_habilitado = "Evento_habilitado";
            public const string Idrastreogps_reportes = "Idrastreogps_reportes";
            public const string Id_equipogps = "Id_equipogps";
            public const string Gps_longitud = "Gps_longitud";
            public const string Gps_latitud = "Gps_latitud";
            public const string Gps_fechahora_reporte = "Gps_fechahora_reporte";
            public const string Gps_velocidad = "Gps_velocidad";
            public const string Gps_rumbo = "Gps_rumbo";
            public const string Rumbo = "Rumbo";
            public const string Gps_evento = "Gps_evento";
            public const string Gps_edaddeldato = "Gps_edaddeldato";
            public const string Gps_hdop = "Gps_hdop";
            public const string Gps_satstatus = "Gps_satstatus";
            public const string Gps_gsmstatus = "Gps_gsmstatus";
            public const string Gps_estado_io = "Gps_estado_io";
            public const string Gps_tipodeposicion = "Gps_tipodeposicion";
            public const string Gps_ip = "Gps_ip";
            public const string Gps_obs = "Gps_obs";
            public const string Idcliente = "Idcliente";
            public const string Icono = "Icono";
            public const string Tipo_de_vehiculo = "Tipo_de_vehiculo";
            public const string Cliente = "Cliente";
            public const string Estado_cliente = "Estado_cliente";
            public const string Id_vehiculo = "Id_vehiculo";
            public const string Identificador_rastreo = "Identificador_rastreo";
            public const string Consumo_aprox = "Consumo_aprox";
            public const string Anho = "Anho";
            public const string Chasis = "Chasis";
            public const string Color = "Color";
            public const string Matricula = "Matricula";
            public const string Kilometraje = "Kilometraje";
            public const string Activo = "Activo";
            public const string Idmarca = "Idmarca";
            public const string Idmodelo = "Idmodelo";
            public const string Marca = "Marca";
            public const string Modelo = "Modelo";
            public const string Idequipogps = "Idequipogps";
            public const string Id_tipoequipogps = "Id_tipoequipogps";
            public const string Tipoequipogps = "Tipoequipogps";
            public const string Tipo_de_reportegps = "Tipo_de_reportegps";
            public const string Id_equipo_gps = "Id_equipo_gps";
            public const string Nro_serie_gps = "Nro_serie_gps";

			static public string ToColumnName(string propertyName)
			{
				if(ht == null)
				{
					ht = new Hashtable();
					
					ht[Id_evento] = rsview_recorridos.ColumnNames.Id_evento;
					ht[Evento] = rsview_recorridos.ColumnNames.Evento;
					ht[Color_evento] = rsview_recorridos.ColumnNames.Color_evento;
					ht[Evento_habilitado] = rsview_recorridos.ColumnNames.Evento_habilitado;
					ht[Idrastreogps_reportes] = rsview_recorridos.ColumnNames.Idrastreogps_reportes;
					ht[Id_equipogps] = rsview_recorridos.ColumnNames.Id_equipogps;
					ht[Gps_longitud] = rsview_recorridos.ColumnNames.Gps_longitud;
					ht[Gps_latitud] = rsview_recorridos.ColumnNames.Gps_latitud;
					ht[Gps_fechahora_reporte] = rsview_recorridos.ColumnNames.Gps_fechahora_reporte;
					ht[Gps_velocidad] = rsview_recorridos.ColumnNames.Gps_velocidad;
					ht[Gps_rumbo] = rsview_recorridos.ColumnNames.Gps_rumbo;
					ht[Rumbo] = rsview_recorridos.ColumnNames.Rumbo;
					ht[Gps_evento] = rsview_recorridos.ColumnNames.Gps_evento;
					ht[Gps_edaddeldato] = rsview_recorridos.ColumnNames.Gps_edaddeldato;
					ht[Gps_hdop] = rsview_recorridos.ColumnNames.Gps_hdop;
					ht[Gps_satstatus] = rsview_recorridos.ColumnNames.Gps_satstatus;
					ht[Gps_gsmstatus] = rsview_recorridos.ColumnNames.Gps_gsmstatus;
					ht[Gps_estado_io] = rsview_recorridos.ColumnNames.Gps_estado_io;
					ht[Gps_tipodeposicion] = rsview_recorridos.ColumnNames.Gps_tipodeposicion;
					ht[Gps_ip] = rsview_recorridos.ColumnNames.Gps_ip;
					ht[Gps_obs] = rsview_recorridos.ColumnNames.Gps_obs;
					ht[Idcliente] = rsview_recorridos.ColumnNames.Idcliente;
					ht[Icono] = rsview_recorridos.ColumnNames.Icono;
					ht[Tipo_de_vehiculo] = rsview_recorridos.ColumnNames.Tipo_de_vehiculo;
					ht[Cliente] = rsview_recorridos.ColumnNames.Cliente;
					ht[Estado_cliente] = rsview_recorridos.ColumnNames.Estado_cliente;
					ht[Id_vehiculo] = rsview_recorridos.ColumnNames.Id_vehiculo;
					ht[Identificador_rastreo] = rsview_recorridos.ColumnNames.Identificador_rastreo;
					ht[Consumo_aprox] = rsview_recorridos.ColumnNames.Consumo_aprox;
					ht[Anho] = rsview_recorridos.ColumnNames.Anho;
					ht[Chasis] = rsview_recorridos.ColumnNames.Chasis;
					ht[Color] = rsview_recorridos.ColumnNames.Color;
					ht[Matricula] = rsview_recorridos.ColumnNames.Matricula;
					ht[Kilometraje] = rsview_recorridos.ColumnNames.Kilometraje;
					ht[Activo] = rsview_recorridos.ColumnNames.Activo;
					ht[Idmarca] = rsview_recorridos.ColumnNames.Idmarca;
					ht[Idmodelo] = rsview_recorridos.ColumnNames.Idmodelo;
					ht[Marca] = rsview_recorridos.ColumnNames.Marca;
					ht[Modelo] = rsview_recorridos.ColumnNames.Modelo;
					ht[Idequipogps] = rsview_recorridos.ColumnNames.Idequipogps;
					ht[Id_tipoequipogps] = rsview_recorridos.ColumnNames.Id_tipoequipogps;
					ht[Tipoequipogps] = rsview_recorridos.ColumnNames.Tipoequipogps;
					ht[Tipo_de_reportegps] = rsview_recorridos.ColumnNames.Tipo_de_reportegps;
					ht[Id_equipo_gps] = rsview_recorridos.ColumnNames.Id_equipo_gps;
					ht[Nro_serie_gps] = rsview_recorridos.ColumnNames.Nro_serie_gps;

				}
				return (string)ht[propertyName];
			}

			static private Hashtable ht = null;			 
		}			 
		#endregion

		#region StringPropertyNames
		public class StringPropertyNames
		{  
            public const string Id_evento = "s_Id_evento";
            public const string Evento = "s_Evento";
            public const string Color_evento = "s_Color_evento";
            public const string Evento_habilitado = "s_Evento_habilitado";
            public const string Idrastreogps_reportes = "s_Idrastreogps_reportes";
            public const string Id_equipogps = "s_Id_equipogps";
            public const string Gps_longitud = "s_Gps_longitud";
            public const string Gps_latitud = "s_Gps_latitud";
            public const string Gps_fechahora_reporte = "s_Gps_fechahora_reporte";
            public const string Gps_velocidad = "s_Gps_velocidad";
            public const string Gps_rumbo = "s_Gps_rumbo";
            public const string Rumbo = "s_Rumbo";
            public const string Gps_evento = "s_Gps_evento";
            public const string Gps_edaddeldato = "s_Gps_edaddeldato";
            public const string Gps_hdop = "s_Gps_hdop";
            public const string Gps_satstatus = "s_Gps_satstatus";
            public const string Gps_gsmstatus = "s_Gps_gsmstatus";
            public const string Gps_estado_io = "s_Gps_estado_io";
            public const string Gps_tipodeposicion = "s_Gps_tipodeposicion";
            public const string Gps_ip = "s_Gps_ip";
            public const string Gps_obs = "s_Gps_obs";
            public const string Idcliente = "s_Idcliente";
            public const string Icono = "s_Icono";
            public const string Tipo_de_vehiculo = "s_Tipo_de_vehiculo";
            public const string Cliente = "s_Cliente";
            public const string Estado_cliente = "s_Estado_cliente";
            public const string Id_vehiculo = "s_Id_vehiculo";
            public const string Identificador_rastreo = "s_Identificador_rastreo";
            public const string Consumo_aprox = "s_Consumo_aprox";
            public const string Anho = "s_Anho";
            public const string Chasis = "s_Chasis";
            public const string Color = "s_Color";
            public const string Matricula = "s_Matricula";
            public const string Kilometraje = "s_Kilometraje";
            public const string Activo = "s_Activo";
            public const string Idmarca = "s_Idmarca";
            public const string Idmodelo = "s_Idmodelo";
            public const string Marca = "s_Marca";
            public const string Modelo = "s_Modelo";
            public const string Idequipogps = "s_Idequipogps";
            public const string Id_tipoequipogps = "s_Id_tipoequipogps";
            public const string Tipoequipogps = "s_Tipoequipogps";
            public const string Tipo_de_reportegps = "s_Tipo_de_reportegps";
            public const string Id_equipo_gps = "s_Id_equipo_gps";
            public const string Nro_serie_gps = "s_Nro_serie_gps";

		}
		#endregion	
		
		#region Properties
			public virtual int Id_evento
	    {
			get
	        {
				return base.Getint(ColumnNames.Id_evento);
			}
			set
	        {
				base.Setint(ColumnNames.Id_evento, value);
			}
		}

		public virtual string Evento
	    {
			get
	        {
				return base.Getstring(ColumnNames.Evento);
			}
			set
	        {
				base.Setstring(ColumnNames.Evento, value);
			}
		}

		public virtual string Color_evento
	    {
			get
	        {
				return base.Getstring(ColumnNames.Color_evento);
			}
			set
	        {
				base.Setstring(ColumnNames.Color_evento, value);
			}
		}

		public virtual bool Evento_habilitado
	    {
			get
	        {
				return base.Getbool(ColumnNames.Evento_habilitado);
			}
			set
	        {
				base.Setbool(ColumnNames.Evento_habilitado, value);
			}
		}

		public virtual int Idrastreogps_reportes
	    {
			get
	        {
				return base.Getint(ColumnNames.Idrastreogps_reportes);
			}
			set
	        {
				base.Setint(ColumnNames.Idrastreogps_reportes, value);
			}
		}

		public virtual int Id_equipogps
	    {
			get
	        {
				return base.Getint(ColumnNames.Id_equipogps);
			}
			set
	        {
				base.Setint(ColumnNames.Id_equipogps, value);
			}
		}

		public virtual double Gps_longitud
	    {
			get
	        {
				return base.Getdouble(ColumnNames.Gps_longitud);
			}
			set
	        {
				base.Setdouble(ColumnNames.Gps_longitud, value);
			}
		}

		public virtual double Gps_latitud
	    {
			get
	        {
				return base.Getdouble(ColumnNames.Gps_latitud);
			}
			set
	        {
				base.Setdouble(ColumnNames.Gps_latitud, value);
			}
		}

		public virtual DateTime Gps_fechahora_reporte
	    {
			get
	        {
				return base.GetDateTime(ColumnNames.Gps_fechahora_reporte);
			}
			set
	        {
				base.SetDateTime(ColumnNames.Gps_fechahora_reporte, value);
			}
		}

		public virtual int Gps_velocidad
	    {
			get
	        {
				return base.Getint(ColumnNames.Gps_velocidad);
			}
			set
	        {
				base.Setint(ColumnNames.Gps_velocidad, value);
			}
		}

		public virtual int Gps_rumbo
	    {
			get
	        {
				return base.Getint(ColumnNames.Gps_rumbo);
			}
			set
	        {
				base.Setint(ColumnNames.Gps_rumbo, value);
			}
		}

		public virtual string Rumbo
	    {
			get
	        {
				return base.Getstring(ColumnNames.Rumbo);
			}
			set
	        {
				base.Setstring(ColumnNames.Rumbo, value);
			}
		}

		public virtual string Gps_evento
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_evento);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_evento, value);
			}
		}

		public virtual string Gps_edaddeldato
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_edaddeldato);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_edaddeldato, value);
			}
		}

		public virtual string Gps_hdop
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_hdop);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_hdop, value);
			}
		}

		public virtual string Gps_satstatus
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_satstatus);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_satstatus, value);
			}
		}

		public virtual string Gps_gsmstatus
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_gsmstatus);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_gsmstatus, value);
			}
		}

		public virtual string Gps_estado_io
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_estado_io);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_estado_io, value);
			}
		}

		public virtual string Gps_tipodeposicion
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_tipodeposicion);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_tipodeposicion, value);
			}
		}

		public virtual string Gps_ip
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_ip);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_ip, value);
			}
		}

		public virtual string Gps_obs
	    {
			get
	        {
				return base.Getstring(ColumnNames.Gps_obs);
			}
			set
	        {
				base.Setstring(ColumnNames.Gps_obs, value);
			}
		}

		public virtual int Idcliente
	    {
			get
	        {
				return base.Getint(ColumnNames.Idcliente);
			}
			set
	        {
				base.Setint(ColumnNames.Idcliente, value);
			}
		}

		public virtual string Icono
	    {
			get
	        {
				return base.Getstring(ColumnNames.Icono);
			}
			set
	        {
				base.Setstring(ColumnNames.Icono, value);
			}
		}

		public virtual string Tipo_de_vehiculo
	    {
			get
	        {
				return base.Getstring(ColumnNames.Tipo_de_vehiculo);
			}
			set
	        {
				base.Setstring(ColumnNames.Tipo_de_vehiculo, value);
			}
		}

		public virtual string Cliente
	    {
			get
	        {
				return base.Getstring(ColumnNames.Cliente);
			}
			set
	        {
				base.Setstring(ColumnNames.Cliente, value);
			}
		}

		public virtual bool Estado_cliente
	    {
			get
	        {
				return base.Getbool(ColumnNames.Estado_cliente);
			}
			set
	        {
				base.Setbool(ColumnNames.Estado_cliente, value);
			}
		}

		public virtual int Id_vehiculo
	    {
			get
	        {
				return base.Getint(ColumnNames.Id_vehiculo);
			}
			set
	        {
				base.Setint(ColumnNames.Id_vehiculo, value);
			}
		}

		public virtual string Identificador_rastreo
	    {
			get
	        {
				return base.Getstring(ColumnNames.Identificador_rastreo);
			}
			set
	        {
				base.Setstring(ColumnNames.Identificador_rastreo, value);
			}
		}

		public virtual double Consumo_aprox
	    {
			get
	        {
				return base.Getdouble(ColumnNames.Consumo_aprox);
			}
			set
	        {
				base.Setdouble(ColumnNames.Consumo_aprox, value);
			}
		}

		public virtual int Anho
	    {
			get
	        {
				return base.Getint(ColumnNames.Anho);
			}
			set
	        {
				base.Setint(ColumnNames.Anho, value);
			}
		}

		public virtual string Chasis
	    {
			get
	        {
				return base.Getstring(ColumnNames.Chasis);
			}
			set
	        {
				base.Setstring(ColumnNames.Chasis, value);
			}
		}

		public virtual string Color
	    {
			get
	        {
				return base.Getstring(ColumnNames.Color);
			}
			set
	        {
				base.Setstring(ColumnNames.Color, value);
			}
		}

		public virtual string Matricula
	    {
			get
	        {
				return base.Getstring(ColumnNames.Matricula);
			}
			set
	        {
				base.Setstring(ColumnNames.Matricula, value);
			}
		}

		public virtual int Kilometraje
	    {
			get
	        {
				return base.Getint(ColumnNames.Kilometraje);
			}
			set
	        {
				base.Setint(ColumnNames.Kilometraje, value);
			}
		}

		public virtual bool Activo
	    {
			get
	        {
				return base.Getbool(ColumnNames.Activo);
			}
			set
	        {
				base.Setbool(ColumnNames.Activo, value);
			}
		}

		public virtual int Idmarca
	    {
			get
	        {
				return base.Getint(ColumnNames.Idmarca);
			}
			set
	        {
				base.Setint(ColumnNames.Idmarca, value);
			}
		}

		public virtual int Idmodelo
	    {
			get
	        {
				return base.Getint(ColumnNames.Idmodelo);
			}
			set
	        {
				base.Setint(ColumnNames.Idmodelo, value);
			}
		}

		public virtual string Marca
	    {
			get
	        {
				return base.Getstring(ColumnNames.Marca);
			}
			set
	        {
				base.Setstring(ColumnNames.Marca, value);
			}
		}

		public virtual string Modelo
	    {
			get
	        {
				return base.Getstring(ColumnNames.Modelo);
			}
			set
	        {
				base.Setstring(ColumnNames.Modelo, value);
			}
		}

		public virtual int Idequipogps
	    {
			get
	        {
				return base.Getint(ColumnNames.Idequipogps);
			}
			set
	        {
				base.Setint(ColumnNames.Idequipogps, value);
			}
		}

		public virtual int Id_tipoequipogps
	    {
			get
	        {
				return base.Getint(ColumnNames.Id_tipoequipogps);
			}
			set
	        {
				base.Setint(ColumnNames.Id_tipoequipogps, value);
			}
		}

		public virtual string Tipoequipogps
	    {
			get
	        {
				return base.Getstring(ColumnNames.Tipoequipogps);
			}
			set
	        {
				base.Setstring(ColumnNames.Tipoequipogps, value);
			}
		}

		public virtual string Tipo_de_reportegps
	    {
			get
	        {
				return base.Getstring(ColumnNames.Tipo_de_reportegps);
			}
			set
	        {
				base.Setstring(ColumnNames.Tipo_de_reportegps, value);
			}
		}

		public virtual string Id_equipo_gps
	    {
			get
	        {
				return base.Getstring(ColumnNames.Id_equipo_gps);
			}
			set
	        {
				base.Setstring(ColumnNames.Id_equipo_gps, value);
			}
		}

		public virtual string Nro_serie_gps
	    {
			get
	        {
				return base.Getstring(ColumnNames.Nro_serie_gps);
			}
			set
	        {
				base.Setstring(ColumnNames.Nro_serie_gps, value);
			}
		}


		#endregion
		
		#region String Properties
	
		public virtual string s_Id_evento
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Id_evento) ? string.Empty : base.GetintAsString(ColumnNames.Id_evento);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Id_evento);
				else
					this.Id_evento = base.SetintAsString(ColumnNames.Id_evento, value);
			}
		}

		public virtual string s_Evento
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Evento) ? string.Empty : base.GetstringAsString(ColumnNames.Evento);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Evento);
				else
					this.Evento = base.SetstringAsString(ColumnNames.Evento, value);
			}
		}

		public virtual string s_Color_evento
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Color_evento) ? string.Empty : base.GetstringAsString(ColumnNames.Color_evento);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Color_evento);
				else
					this.Color_evento = base.SetstringAsString(ColumnNames.Color_evento, value);
			}
		}

		public virtual string s_Evento_habilitado
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Evento_habilitado) ? string.Empty : base.GetboolAsString(ColumnNames.Evento_habilitado);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Evento_habilitado);
				else
					this.Evento_habilitado = base.SetboolAsString(ColumnNames.Evento_habilitado, value);
			}
		}

		public virtual string s_Idrastreogps_reportes
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Idrastreogps_reportes) ? string.Empty : base.GetintAsString(ColumnNames.Idrastreogps_reportes);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Idrastreogps_reportes);
				else
					this.Idrastreogps_reportes = base.SetintAsString(ColumnNames.Idrastreogps_reportes, value);
			}
		}

		public virtual string s_Id_equipogps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Id_equipogps) ? string.Empty : base.GetintAsString(ColumnNames.Id_equipogps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Id_equipogps);
				else
					this.Id_equipogps = base.SetintAsString(ColumnNames.Id_equipogps, value);
			}
		}

		public virtual string s_Gps_longitud
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_longitud) ? string.Empty : base.GetdoubleAsString(ColumnNames.Gps_longitud);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_longitud);
				else
					this.Gps_longitud = base.SetdoubleAsString(ColumnNames.Gps_longitud, value);
			}
		}

		public virtual string s_Gps_latitud
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_latitud) ? string.Empty : base.GetdoubleAsString(ColumnNames.Gps_latitud);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_latitud);
				else
					this.Gps_latitud = base.SetdoubleAsString(ColumnNames.Gps_latitud, value);
			}
		}

		public virtual string s_Gps_fechahora_reporte
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_fechahora_reporte) ? string.Empty : base.GetDateTimeAsString(ColumnNames.Gps_fechahora_reporte);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_fechahora_reporte);
				else
					this.Gps_fechahora_reporte = base.SetDateTimeAsString(ColumnNames.Gps_fechahora_reporte, value);
			}
		}

		public virtual string s_Gps_velocidad
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_velocidad) ? string.Empty : base.GetintAsString(ColumnNames.Gps_velocidad);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_velocidad);
				else
					this.Gps_velocidad = base.SetintAsString(ColumnNames.Gps_velocidad, value);
			}
		}

		public virtual string s_Gps_rumbo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_rumbo) ? string.Empty : base.GetintAsString(ColumnNames.Gps_rumbo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_rumbo);
				else
					this.Gps_rumbo = base.SetintAsString(ColumnNames.Gps_rumbo, value);
			}
		}

		public virtual string s_Rumbo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Rumbo) ? string.Empty : base.GetstringAsString(ColumnNames.Rumbo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Rumbo);
				else
					this.Rumbo = base.SetstringAsString(ColumnNames.Rumbo, value);
			}
		}

		public virtual string s_Gps_evento
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_evento) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_evento);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_evento);
				else
					this.Gps_evento = base.SetstringAsString(ColumnNames.Gps_evento, value);
			}
		}

		public virtual string s_Gps_edaddeldato
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_edaddeldato) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_edaddeldato);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_edaddeldato);
				else
					this.Gps_edaddeldato = base.SetstringAsString(ColumnNames.Gps_edaddeldato, value);
			}
		}

		public virtual string s_Gps_hdop
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_hdop) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_hdop);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_hdop);
				else
					this.Gps_hdop = base.SetstringAsString(ColumnNames.Gps_hdop, value);
			}
		}

		public virtual string s_Gps_satstatus
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_satstatus) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_satstatus);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_satstatus);
				else
					this.Gps_satstatus = base.SetstringAsString(ColumnNames.Gps_satstatus, value);
			}
		}

		public virtual string s_Gps_gsmstatus
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_gsmstatus) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_gsmstatus);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_gsmstatus);
				else
					this.Gps_gsmstatus = base.SetstringAsString(ColumnNames.Gps_gsmstatus, value);
			}
		}

		public virtual string s_Gps_estado_io
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_estado_io) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_estado_io);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_estado_io);
				else
					this.Gps_estado_io = base.SetstringAsString(ColumnNames.Gps_estado_io, value);
			}
		}

		public virtual string s_Gps_tipodeposicion
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_tipodeposicion) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_tipodeposicion);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_tipodeposicion);
				else
					this.Gps_tipodeposicion = base.SetstringAsString(ColumnNames.Gps_tipodeposicion, value);
			}
		}

		public virtual string s_Gps_ip
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_ip) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_ip);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_ip);
				else
					this.Gps_ip = base.SetstringAsString(ColumnNames.Gps_ip, value);
			}
		}

		public virtual string s_Gps_obs
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Gps_obs) ? string.Empty : base.GetstringAsString(ColumnNames.Gps_obs);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Gps_obs);
				else
					this.Gps_obs = base.SetstringAsString(ColumnNames.Gps_obs, value);
			}
		}

		public virtual string s_Idcliente
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Idcliente) ? string.Empty : base.GetintAsString(ColumnNames.Idcliente);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Idcliente);
				else
					this.Idcliente = base.SetintAsString(ColumnNames.Idcliente, value);
			}
		}

		public virtual string s_Icono
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Icono) ? string.Empty : base.GetstringAsString(ColumnNames.Icono);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Icono);
				else
					this.Icono = base.SetstringAsString(ColumnNames.Icono, value);
			}
		}

		public virtual string s_Tipo_de_vehiculo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Tipo_de_vehiculo) ? string.Empty : base.GetstringAsString(ColumnNames.Tipo_de_vehiculo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Tipo_de_vehiculo);
				else
					this.Tipo_de_vehiculo = base.SetstringAsString(ColumnNames.Tipo_de_vehiculo, value);
			}
		}

		public virtual string s_Cliente
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Cliente) ? string.Empty : base.GetstringAsString(ColumnNames.Cliente);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Cliente);
				else
					this.Cliente = base.SetstringAsString(ColumnNames.Cliente, value);
			}
		}

		public virtual string s_Estado_cliente
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Estado_cliente) ? string.Empty : base.GetboolAsString(ColumnNames.Estado_cliente);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Estado_cliente);
				else
					this.Estado_cliente = base.SetboolAsString(ColumnNames.Estado_cliente, value);
			}
		}

		public virtual string s_Id_vehiculo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Id_vehiculo) ? string.Empty : base.GetintAsString(ColumnNames.Id_vehiculo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Id_vehiculo);
				else
					this.Id_vehiculo = base.SetintAsString(ColumnNames.Id_vehiculo, value);
			}
		}

		public virtual string s_Identificador_rastreo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Identificador_rastreo) ? string.Empty : base.GetstringAsString(ColumnNames.Identificador_rastreo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Identificador_rastreo);
				else
					this.Identificador_rastreo = base.SetstringAsString(ColumnNames.Identificador_rastreo, value);
			}
		}

		public virtual string s_Consumo_aprox
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Consumo_aprox) ? string.Empty : base.GetdoubleAsString(ColumnNames.Consumo_aprox);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Consumo_aprox);
				else
					this.Consumo_aprox = base.SetdoubleAsString(ColumnNames.Consumo_aprox, value);
			}
		}

		public virtual string s_Anho
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Anho) ? string.Empty : base.GetintAsString(ColumnNames.Anho);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Anho);
				else
					this.Anho = base.SetintAsString(ColumnNames.Anho, value);
			}
		}

		public virtual string s_Chasis
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Chasis) ? string.Empty : base.GetstringAsString(ColumnNames.Chasis);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Chasis);
				else
					this.Chasis = base.SetstringAsString(ColumnNames.Chasis, value);
			}
		}

		public virtual string s_Color
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Color) ? string.Empty : base.GetstringAsString(ColumnNames.Color);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Color);
				else
					this.Color = base.SetstringAsString(ColumnNames.Color, value);
			}
		}

		public virtual string s_Matricula
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Matricula) ? string.Empty : base.GetstringAsString(ColumnNames.Matricula);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Matricula);
				else
					this.Matricula = base.SetstringAsString(ColumnNames.Matricula, value);
			}
		}

		public virtual string s_Kilometraje
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Kilometraje) ? string.Empty : base.GetintAsString(ColumnNames.Kilometraje);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Kilometraje);
				else
					this.Kilometraje = base.SetintAsString(ColumnNames.Kilometraje, value);
			}
		}

		public virtual string s_Activo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Activo) ? string.Empty : base.GetboolAsString(ColumnNames.Activo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Activo);
				else
					this.Activo = base.SetboolAsString(ColumnNames.Activo, value);
			}
		}

		public virtual string s_Idmarca
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Idmarca) ? string.Empty : base.GetintAsString(ColumnNames.Idmarca);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Idmarca);
				else
					this.Idmarca = base.SetintAsString(ColumnNames.Idmarca, value);
			}
		}

		public virtual string s_Idmodelo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Idmodelo) ? string.Empty : base.GetintAsString(ColumnNames.Idmodelo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Idmodelo);
				else
					this.Idmodelo = base.SetintAsString(ColumnNames.Idmodelo, value);
			}
		}

		public virtual string s_Marca
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Marca) ? string.Empty : base.GetstringAsString(ColumnNames.Marca);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Marca);
				else
					this.Marca = base.SetstringAsString(ColumnNames.Marca, value);
			}
		}

		public virtual string s_Modelo
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Modelo) ? string.Empty : base.GetstringAsString(ColumnNames.Modelo);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Modelo);
				else
					this.Modelo = base.SetstringAsString(ColumnNames.Modelo, value);
			}
		}

		public virtual string s_Idequipogps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Idequipogps) ? string.Empty : base.GetintAsString(ColumnNames.Idequipogps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Idequipogps);
				else
					this.Idequipogps = base.SetintAsString(ColumnNames.Idequipogps, value);
			}
		}

		public virtual string s_Id_tipoequipogps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Id_tipoequipogps) ? string.Empty : base.GetintAsString(ColumnNames.Id_tipoequipogps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Id_tipoequipogps);
				else
					this.Id_tipoequipogps = base.SetintAsString(ColumnNames.Id_tipoequipogps, value);
			}
		}

		public virtual string s_Tipoequipogps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Tipoequipogps) ? string.Empty : base.GetstringAsString(ColumnNames.Tipoequipogps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Tipoequipogps);
				else
					this.Tipoequipogps = base.SetstringAsString(ColumnNames.Tipoequipogps, value);
			}
		}

		public virtual string s_Tipo_de_reportegps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Tipo_de_reportegps) ? string.Empty : base.GetstringAsString(ColumnNames.Tipo_de_reportegps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Tipo_de_reportegps);
				else
					this.Tipo_de_reportegps = base.SetstringAsString(ColumnNames.Tipo_de_reportegps, value);
			}
		}

		public virtual string s_Id_equipo_gps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Id_equipo_gps) ? string.Empty : base.GetstringAsString(ColumnNames.Id_equipo_gps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Id_equipo_gps);
				else
					this.Id_equipo_gps = base.SetstringAsString(ColumnNames.Id_equipo_gps, value);
			}
		}

		public virtual string s_Nro_serie_gps
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Nro_serie_gps) ? string.Empty : base.GetstringAsString(ColumnNames.Nro_serie_gps);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Nro_serie_gps);
				else
					this.Nro_serie_gps = base.SetstringAsString(ColumnNames.Nro_serie_gps, value);
			}
		}


		#endregion			
	
		#region Where Clause
		public class WhereClause
		{
			public WhereClause(BusinessEntity entity)
			{
				this._entity = entity;
			}
			
			public TearOffWhereParameter TearOff
			{
				get
				{
					if(_tearOff == null)
					{
						_tearOff = new TearOffWhereParameter(this);
					}

					return _tearOff;
				}
			}

			#region WhereParameter TearOff's
			public class TearOffWhereParameter
			{
				public TearOffWhereParameter(WhereClause clause)
				{
					this._clause = clause;
				}
				
				
				public WhereParameter Id_evento
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Id_evento, Parameters.Id_evento);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Evento
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Evento, Parameters.Evento);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Color_evento
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Color_evento, Parameters.Color_evento);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Evento_habilitado
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Evento_habilitado, Parameters.Evento_habilitado);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Idrastreogps_reportes
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Idrastreogps_reportes, Parameters.Idrastreogps_reportes);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Id_equipogps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Id_equipogps, Parameters.Id_equipogps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_longitud
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_longitud, Parameters.Gps_longitud);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_latitud
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_latitud, Parameters.Gps_latitud);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_fechahora_reporte
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_fechahora_reporte, Parameters.Gps_fechahora_reporte);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_velocidad
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_velocidad, Parameters.Gps_velocidad);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_rumbo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_rumbo, Parameters.Gps_rumbo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Rumbo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Rumbo, Parameters.Rumbo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_evento
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_evento, Parameters.Gps_evento);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_edaddeldato
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_edaddeldato, Parameters.Gps_edaddeldato);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_hdop
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_hdop, Parameters.Gps_hdop);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_satstatus
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_satstatus, Parameters.Gps_satstatus);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_gsmstatus
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_gsmstatus, Parameters.Gps_gsmstatus);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_estado_io
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_estado_io, Parameters.Gps_estado_io);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_tipodeposicion
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_tipodeposicion, Parameters.Gps_tipodeposicion);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_ip
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_ip, Parameters.Gps_ip);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Gps_obs
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Gps_obs, Parameters.Gps_obs);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Idcliente
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Idcliente, Parameters.Idcliente);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Icono
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Icono, Parameters.Icono);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Tipo_de_vehiculo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Tipo_de_vehiculo, Parameters.Tipo_de_vehiculo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Cliente
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Cliente, Parameters.Cliente);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Estado_cliente
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Estado_cliente, Parameters.Estado_cliente);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Id_vehiculo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Id_vehiculo, Parameters.Id_vehiculo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Identificador_rastreo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Identificador_rastreo, Parameters.Identificador_rastreo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Consumo_aprox
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Consumo_aprox, Parameters.Consumo_aprox);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Anho
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Anho, Parameters.Anho);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Chasis
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Chasis, Parameters.Chasis);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Color
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Color, Parameters.Color);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Matricula
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Matricula, Parameters.Matricula);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Kilometraje
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Kilometraje, Parameters.Kilometraje);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Activo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Activo, Parameters.Activo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Idmarca
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Idmarca, Parameters.Idmarca);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Idmodelo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Idmodelo, Parameters.Idmodelo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Marca
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Marca, Parameters.Marca);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Modelo
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Modelo, Parameters.Modelo);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Idequipogps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Idequipogps, Parameters.Idequipogps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Id_tipoequipogps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Id_tipoequipogps, Parameters.Id_tipoequipogps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Tipoequipogps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Tipoequipogps, Parameters.Tipoequipogps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Tipo_de_reportegps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Tipo_de_reportegps, Parameters.Tipo_de_reportegps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Id_equipo_gps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Id_equipo_gps, Parameters.Id_equipo_gps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Nro_serie_gps
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Nro_serie_gps, Parameters.Nro_serie_gps);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}


				private WhereClause _clause;
			}
			#endregion
		
			public WhereParameter Id_evento
		    {
				get
		        {
					if(_Id_evento_W == null)
	        	    {
						_Id_evento_W = TearOff.Id_evento;
					}
					return _Id_evento_W;
				}
			}

			public WhereParameter Evento
		    {
				get
		        {
					if(_Evento_W == null)
	        	    {
						_Evento_W = TearOff.Evento;
					}
					return _Evento_W;
				}
			}

			public WhereParameter Color_evento
		    {
				get
		        {
					if(_Color_evento_W == null)
	        	    {
						_Color_evento_W = TearOff.Color_evento;
					}
					return _Color_evento_W;
				}
			}

			public WhereParameter Evento_habilitado
		    {
				get
		        {
					if(_Evento_habilitado_W == null)
	        	    {
						_Evento_habilitado_W = TearOff.Evento_habilitado;
					}
					return _Evento_habilitado_W;
				}
			}

			public WhereParameter Idrastreogps_reportes
		    {
				get
		        {
					if(_Idrastreogps_reportes_W == null)
	        	    {
						_Idrastreogps_reportes_W = TearOff.Idrastreogps_reportes;
					}
					return _Idrastreogps_reportes_W;
				}
			}

			public WhereParameter Id_equipogps
		    {
				get
		        {
					if(_Id_equipogps_W == null)
	        	    {
						_Id_equipogps_W = TearOff.Id_equipogps;
					}
					return _Id_equipogps_W;
				}
			}

			public WhereParameter Gps_longitud
		    {
				get
		        {
					if(_Gps_longitud_W == null)
	        	    {
						_Gps_longitud_W = TearOff.Gps_longitud;
					}
					return _Gps_longitud_W;
				}
			}

			public WhereParameter Gps_latitud
		    {
				get
		        {
					if(_Gps_latitud_W == null)
	        	    {
						_Gps_latitud_W = TearOff.Gps_latitud;
					}
					return _Gps_latitud_W;
				}
			}

			public WhereParameter Gps_fechahora_reporte
		    {
				get
		        {
					if(_Gps_fechahora_reporte_W == null)
	        	    {
						_Gps_fechahora_reporte_W = TearOff.Gps_fechahora_reporte;
					}
					return _Gps_fechahora_reporte_W;
				}
			}

			public WhereParameter Gps_velocidad
		    {
				get
		        {
					if(_Gps_velocidad_W == null)
	        	    {
						_Gps_velocidad_W = TearOff.Gps_velocidad;
					}
					return _Gps_velocidad_W;
				}
			}

			public WhereParameter Gps_rumbo
		    {
				get
		        {
					if(_Gps_rumbo_W == null)
	        	    {
						_Gps_rumbo_W = TearOff.Gps_rumbo;
					}
					return _Gps_rumbo_W;
				}
			}

			public WhereParameter Rumbo
		    {
				get
		        {
					if(_Rumbo_W == null)
	        	    {
						_Rumbo_W = TearOff.Rumbo;
					}
					return _Rumbo_W;
				}
			}

			public WhereParameter Gps_evento
		    {
				get
		        {
					if(_Gps_evento_W == null)
	        	    {
						_Gps_evento_W = TearOff.Gps_evento;
					}
					return _Gps_evento_W;
				}
			}

			public WhereParameter Gps_edaddeldato
		    {
				get
		        {
					if(_Gps_edaddeldato_W == null)
	        	    {
						_Gps_edaddeldato_W = TearOff.Gps_edaddeldato;
					}
					return _Gps_edaddeldato_W;
				}
			}

			public WhereParameter Gps_hdop
		    {
				get
		        {
					if(_Gps_hdop_W == null)
	        	    {
						_Gps_hdop_W = TearOff.Gps_hdop;
					}
					return _Gps_hdop_W;
				}
			}

			public WhereParameter Gps_satstatus
		    {
				get
		        {
					if(_Gps_satstatus_W == null)
	        	    {
						_Gps_satstatus_W = TearOff.Gps_satstatus;
					}
					return _Gps_satstatus_W;
				}
			}

			public WhereParameter Gps_gsmstatus
		    {
				get
		        {
					if(_Gps_gsmstatus_W == null)
	        	    {
						_Gps_gsmstatus_W = TearOff.Gps_gsmstatus;
					}
					return _Gps_gsmstatus_W;
				}
			}

			public WhereParameter Gps_estado_io
		    {
				get
		        {
					if(_Gps_estado_io_W == null)
	        	    {
						_Gps_estado_io_W = TearOff.Gps_estado_io;
					}
					return _Gps_estado_io_W;
				}
			}

			public WhereParameter Gps_tipodeposicion
		    {
				get
		        {
					if(_Gps_tipodeposicion_W == null)
	        	    {
						_Gps_tipodeposicion_W = TearOff.Gps_tipodeposicion;
					}
					return _Gps_tipodeposicion_W;
				}
			}

			public WhereParameter Gps_ip
		    {
				get
		        {
					if(_Gps_ip_W == null)
	        	    {
						_Gps_ip_W = TearOff.Gps_ip;
					}
					return _Gps_ip_W;
				}
			}

			public WhereParameter Gps_obs
		    {
				get
		        {
					if(_Gps_obs_W == null)
	        	    {
						_Gps_obs_W = TearOff.Gps_obs;
					}
					return _Gps_obs_W;
				}
			}

			public WhereParameter Idcliente
		    {
				get
		        {
					if(_Idcliente_W == null)
	        	    {
						_Idcliente_W = TearOff.Idcliente;
					}
					return _Idcliente_W;
				}
			}

			public WhereParameter Icono
		    {
				get
		        {
					if(_Icono_W == null)
	        	    {
						_Icono_W = TearOff.Icono;
					}
					return _Icono_W;
				}
			}

			public WhereParameter Tipo_de_vehiculo
		    {
				get
		        {
					if(_Tipo_de_vehiculo_W == null)
	        	    {
						_Tipo_de_vehiculo_W = TearOff.Tipo_de_vehiculo;
					}
					return _Tipo_de_vehiculo_W;
				}
			}

			public WhereParameter Cliente
		    {
				get
		        {
					if(_Cliente_W == null)
	        	    {
						_Cliente_W = TearOff.Cliente;
					}
					return _Cliente_W;
				}
			}

			public WhereParameter Estado_cliente
		    {
				get
		        {
					if(_Estado_cliente_W == null)
	        	    {
						_Estado_cliente_W = TearOff.Estado_cliente;
					}
					return _Estado_cliente_W;
				}
			}

			public WhereParameter Id_vehiculo
		    {
				get
		        {
					if(_Id_vehiculo_W == null)
	        	    {
						_Id_vehiculo_W = TearOff.Id_vehiculo;
					}
					return _Id_vehiculo_W;
				}
			}

			public WhereParameter Identificador_rastreo
		    {
				get
		        {
					if(_Identificador_rastreo_W == null)
	        	    {
						_Identificador_rastreo_W = TearOff.Identificador_rastreo;
					}
					return _Identificador_rastreo_W;
				}
			}

			public WhereParameter Consumo_aprox
		    {
				get
		        {
					if(_Consumo_aprox_W == null)
	        	    {
						_Consumo_aprox_W = TearOff.Consumo_aprox;
					}
					return _Consumo_aprox_W;
				}
			}

			public WhereParameter Anho
		    {
				get
		        {
					if(_Anho_W == null)
	        	    {
						_Anho_W = TearOff.Anho;
					}
					return _Anho_W;
				}
			}

			public WhereParameter Chasis
		    {
				get
		        {
					if(_Chasis_W == null)
	        	    {
						_Chasis_W = TearOff.Chasis;
					}
					return _Chasis_W;
				}
			}

			public WhereParameter Color
		    {
				get
		        {
					if(_Color_W == null)
	        	    {
						_Color_W = TearOff.Color;
					}
					return _Color_W;
				}
			}

			public WhereParameter Matricula
		    {
				get
		        {
					if(_Matricula_W == null)
	        	    {
						_Matricula_W = TearOff.Matricula;
					}
					return _Matricula_W;
				}
			}

			public WhereParameter Kilometraje
		    {
				get
		        {
					if(_Kilometraje_W == null)
	        	    {
						_Kilometraje_W = TearOff.Kilometraje;
					}
					return _Kilometraje_W;
				}
			}

			public WhereParameter Activo
		    {
				get
		        {
					if(_Activo_W == null)
	        	    {
						_Activo_W = TearOff.Activo;
					}
					return _Activo_W;
				}
			}

			public WhereParameter Idmarca
		    {
				get
		        {
					if(_Idmarca_W == null)
	        	    {
						_Idmarca_W = TearOff.Idmarca;
					}
					return _Idmarca_W;
				}
			}

			public WhereParameter Idmodelo
		    {
				get
		        {
					if(_Idmodelo_W == null)
	        	    {
						_Idmodelo_W = TearOff.Idmodelo;
					}
					return _Idmodelo_W;
				}
			}

			public WhereParameter Marca
		    {
				get
		        {
					if(_Marca_W == null)
	        	    {
						_Marca_W = TearOff.Marca;
					}
					return _Marca_W;
				}
			}

			public WhereParameter Modelo
		    {
				get
		        {
					if(_Modelo_W == null)
	        	    {
						_Modelo_W = TearOff.Modelo;
					}
					return _Modelo_W;
				}
			}

			public WhereParameter Idequipogps
		    {
				get
		        {
					if(_Idequipogps_W == null)
	        	    {
						_Idequipogps_W = TearOff.Idequipogps;
					}
					return _Idequipogps_W;
				}
			}

			public WhereParameter Id_tipoequipogps
		    {
				get
		        {
					if(_Id_tipoequipogps_W == null)
	        	    {
						_Id_tipoequipogps_W = TearOff.Id_tipoequipogps;
					}
					return _Id_tipoequipogps_W;
				}
			}

			public WhereParameter Tipoequipogps
		    {
				get
		        {
					if(_Tipoequipogps_W == null)
	        	    {
						_Tipoequipogps_W = TearOff.Tipoequipogps;
					}
					return _Tipoequipogps_W;
				}
			}

			public WhereParameter Tipo_de_reportegps
		    {
				get
		        {
					if(_Tipo_de_reportegps_W == null)
	        	    {
						_Tipo_de_reportegps_W = TearOff.Tipo_de_reportegps;
					}
					return _Tipo_de_reportegps_W;
				}
			}

			public WhereParameter Id_equipo_gps
		    {
				get
		        {
					if(_Id_equipo_gps_W == null)
	        	    {
						_Id_equipo_gps_W = TearOff.Id_equipo_gps;
					}
					return _Id_equipo_gps_W;
				}
			}

			public WhereParameter Nro_serie_gps
		    {
				get
		        {
					if(_Nro_serie_gps_W == null)
	        	    {
						_Nro_serie_gps_W = TearOff.Nro_serie_gps;
					}
					return _Nro_serie_gps_W;
				}
			}

			private WhereParameter _Id_evento_W = null;
			private WhereParameter _Evento_W = null;
			private WhereParameter _Color_evento_W = null;
			private WhereParameter _Evento_habilitado_W = null;
			private WhereParameter _Idrastreogps_reportes_W = null;
			private WhereParameter _Id_equipogps_W = null;
			private WhereParameter _Gps_longitud_W = null;
			private WhereParameter _Gps_latitud_W = null;
			private WhereParameter _Gps_fechahora_reporte_W = null;
			private WhereParameter _Gps_velocidad_W = null;
			private WhereParameter _Gps_rumbo_W = null;
			private WhereParameter _Rumbo_W = null;
			private WhereParameter _Gps_evento_W = null;
			private WhereParameter _Gps_edaddeldato_W = null;
			private WhereParameter _Gps_hdop_W = null;
			private WhereParameter _Gps_satstatus_W = null;
			private WhereParameter _Gps_gsmstatus_W = null;
			private WhereParameter _Gps_estado_io_W = null;
			private WhereParameter _Gps_tipodeposicion_W = null;
			private WhereParameter _Gps_ip_W = null;
			private WhereParameter _Gps_obs_W = null;
			private WhereParameter _Idcliente_W = null;
			private WhereParameter _Icono_W = null;
			private WhereParameter _Tipo_de_vehiculo_W = null;
			private WhereParameter _Cliente_W = null;
			private WhereParameter _Estado_cliente_W = null;
			private WhereParameter _Id_vehiculo_W = null;
			private WhereParameter _Identificador_rastreo_W = null;
			private WhereParameter _Consumo_aprox_W = null;
			private WhereParameter _Anho_W = null;
			private WhereParameter _Chasis_W = null;
			private WhereParameter _Color_W = null;
			private WhereParameter _Matricula_W = null;
			private WhereParameter _Kilometraje_W = null;
			private WhereParameter _Activo_W = null;
			private WhereParameter _Idmarca_W = null;
			private WhereParameter _Idmodelo_W = null;
			private WhereParameter _Marca_W = null;
			private WhereParameter _Modelo_W = null;
			private WhereParameter _Idequipogps_W = null;
			private WhereParameter _Id_tipoequipogps_W = null;
			private WhereParameter _Tipoequipogps_W = null;
			private WhereParameter _Tipo_de_reportegps_W = null;
			private WhereParameter _Id_equipo_gps_W = null;
			private WhereParameter _Nro_serie_gps_W = null;

			public void WhereClauseReset()
			{
				_Id_evento_W = null;
				_Evento_W = null;
				_Color_evento_W = null;
				_Evento_habilitado_W = null;
				_Idrastreogps_reportes_W = null;
				_Id_equipogps_W = null;
				_Gps_longitud_W = null;
				_Gps_latitud_W = null;
				_Gps_fechahora_reporte_W = null;
				_Gps_velocidad_W = null;
				_Gps_rumbo_W = null;
				_Rumbo_W = null;
				_Gps_evento_W = null;
				_Gps_edaddeldato_W = null;
				_Gps_hdop_W = null;
				_Gps_satstatus_W = null;
				_Gps_gsmstatus_W = null;
				_Gps_estado_io_W = null;
				_Gps_tipodeposicion_W = null;
				_Gps_ip_W = null;
				_Gps_obs_W = null;
				_Idcliente_W = null;
				_Icono_W = null;
				_Tipo_de_vehiculo_W = null;
				_Cliente_W = null;
				_Estado_cliente_W = null;
				_Id_vehiculo_W = null;
				_Identificador_rastreo_W = null;
				_Consumo_aprox_W = null;
				_Anho_W = null;
				_Chasis_W = null;
				_Color_W = null;
				_Matricula_W = null;
				_Kilometraje_W = null;
				_Activo_W = null;
				_Idmarca_W = null;
				_Idmodelo_W = null;
				_Marca_W = null;
				_Modelo_W = null;
				_Idequipogps_W = null;
				_Id_tipoequipogps_W = null;
				_Tipoequipogps_W = null;
				_Tipo_de_reportegps_W = null;
				_Id_equipo_gps_W = null;
				_Nro_serie_gps_W = null;

				this._entity.Query.FlushWhereParameters();

			}
	
			private BusinessEntity _entity;
			private TearOffWhereParameter _tearOff;
			
		}
	
		public WhereClause Where
		{
			get
			{
				if(_whereClause == null)
				{
					_whereClause = new WhereClause(this);
				}
		
				return _whereClause;
			}
		}
		
		private WhereClause _whereClause = null;	
		#endregion
	
		#region Aggregate Clause
		public class AggregateClause
		{
			public AggregateClause(BusinessEntity entity)
			{
				this._entity = entity;
			}
			
			public TearOffAggregateParameter TearOff
			{
				get
				{
					if(_tearOff == null)
					{
						_tearOff = new TearOffAggregateParameter(this);
					}

					return _tearOff;
				}
			}

			#region AggregateParameter TearOff's
			public class TearOffAggregateParameter
			{
				public TearOffAggregateParameter(AggregateClause clause)
				{
					this._clause = clause;
				}
				
				
				public AggregateParameter Id_evento
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Id_evento, Parameters.Id_evento);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Evento
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Evento, Parameters.Evento);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Color_evento
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Color_evento, Parameters.Color_evento);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Evento_habilitado
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Evento_habilitado, Parameters.Evento_habilitado);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Idrastreogps_reportes
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Idrastreogps_reportes, Parameters.Idrastreogps_reportes);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Id_equipogps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Id_equipogps, Parameters.Id_equipogps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_longitud
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_longitud, Parameters.Gps_longitud);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_latitud
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_latitud, Parameters.Gps_latitud);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_fechahora_reporte
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_fechahora_reporte, Parameters.Gps_fechahora_reporte);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_velocidad
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_velocidad, Parameters.Gps_velocidad);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_rumbo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_rumbo, Parameters.Gps_rumbo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Rumbo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Rumbo, Parameters.Rumbo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_evento
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_evento, Parameters.Gps_evento);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_edaddeldato
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_edaddeldato, Parameters.Gps_edaddeldato);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_hdop
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_hdop, Parameters.Gps_hdop);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_satstatus
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_satstatus, Parameters.Gps_satstatus);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_gsmstatus
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_gsmstatus, Parameters.Gps_gsmstatus);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_estado_io
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_estado_io, Parameters.Gps_estado_io);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_tipodeposicion
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_tipodeposicion, Parameters.Gps_tipodeposicion);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_ip
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_ip, Parameters.Gps_ip);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Gps_obs
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Gps_obs, Parameters.Gps_obs);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Idcliente
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Idcliente, Parameters.Idcliente);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Icono
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Icono, Parameters.Icono);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Tipo_de_vehiculo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Tipo_de_vehiculo, Parameters.Tipo_de_vehiculo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Cliente
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Cliente, Parameters.Cliente);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Estado_cliente
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Estado_cliente, Parameters.Estado_cliente);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Id_vehiculo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Id_vehiculo, Parameters.Id_vehiculo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Identificador_rastreo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Identificador_rastreo, Parameters.Identificador_rastreo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Consumo_aprox
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Consumo_aprox, Parameters.Consumo_aprox);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Anho
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Anho, Parameters.Anho);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Chasis
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Chasis, Parameters.Chasis);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Color
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Color, Parameters.Color);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Matricula
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Matricula, Parameters.Matricula);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Kilometraje
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Kilometraje, Parameters.Kilometraje);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Activo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Activo, Parameters.Activo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Idmarca
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Idmarca, Parameters.Idmarca);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Idmodelo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Idmodelo, Parameters.Idmodelo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Marca
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Marca, Parameters.Marca);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Modelo
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Modelo, Parameters.Modelo);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Idequipogps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Idequipogps, Parameters.Idequipogps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Id_tipoequipogps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Id_tipoequipogps, Parameters.Id_tipoequipogps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Tipoequipogps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Tipoequipogps, Parameters.Tipoequipogps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Tipo_de_reportegps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Tipo_de_reportegps, Parameters.Tipo_de_reportegps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Id_equipo_gps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Id_equipo_gps, Parameters.Id_equipo_gps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Nro_serie_gps
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Nro_serie_gps, Parameters.Nro_serie_gps);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}


				private AggregateClause _clause;
			}
			#endregion
		
			public AggregateParameter Id_evento
		    {
				get
		        {
					if(_Id_evento_W == null)
	        	    {
						_Id_evento_W = TearOff.Id_evento;
					}
					return _Id_evento_W;
				}
			}

			public AggregateParameter Evento
		    {
				get
		        {
					if(_Evento_W == null)
	        	    {
						_Evento_W = TearOff.Evento;
					}
					return _Evento_W;
				}
			}

			public AggregateParameter Color_evento
		    {
				get
		        {
					if(_Color_evento_W == null)
	        	    {
						_Color_evento_W = TearOff.Color_evento;
					}
					return _Color_evento_W;
				}
			}

			public AggregateParameter Evento_habilitado
		    {
				get
		        {
					if(_Evento_habilitado_W == null)
	        	    {
						_Evento_habilitado_W = TearOff.Evento_habilitado;
					}
					return _Evento_habilitado_W;
				}
			}

			public AggregateParameter Idrastreogps_reportes
		    {
				get
		        {
					if(_Idrastreogps_reportes_W == null)
	        	    {
						_Idrastreogps_reportes_W = TearOff.Idrastreogps_reportes;
					}
					return _Idrastreogps_reportes_W;
				}
			}

			public AggregateParameter Id_equipogps
		    {
				get
		        {
					if(_Id_equipogps_W == null)
	        	    {
						_Id_equipogps_W = TearOff.Id_equipogps;
					}
					return _Id_equipogps_W;
				}
			}

			public AggregateParameter Gps_longitud
		    {
				get
		        {
					if(_Gps_longitud_W == null)
	        	    {
						_Gps_longitud_W = TearOff.Gps_longitud;
					}
					return _Gps_longitud_W;
				}
			}

			public AggregateParameter Gps_latitud
		    {
				get
		        {
					if(_Gps_latitud_W == null)
	        	    {
						_Gps_latitud_W = TearOff.Gps_latitud;
					}
					return _Gps_latitud_W;
				}
			}

			public AggregateParameter Gps_fechahora_reporte
		    {
				get
		        {
					if(_Gps_fechahora_reporte_W == null)
	        	    {
						_Gps_fechahora_reporte_W = TearOff.Gps_fechahora_reporte;
					}
					return _Gps_fechahora_reporte_W;
				}
			}

			public AggregateParameter Gps_velocidad
		    {
				get
		        {
					if(_Gps_velocidad_W == null)
	        	    {
						_Gps_velocidad_W = TearOff.Gps_velocidad;
					}
					return _Gps_velocidad_W;
				}
			}

			public AggregateParameter Gps_rumbo
		    {
				get
		        {
					if(_Gps_rumbo_W == null)
	        	    {
						_Gps_rumbo_W = TearOff.Gps_rumbo;
					}
					return _Gps_rumbo_W;
				}
			}

			public AggregateParameter Rumbo
		    {
				get
		        {
					if(_Rumbo_W == null)
	        	    {
						_Rumbo_W = TearOff.Rumbo;
					}
					return _Rumbo_W;
				}
			}

			public AggregateParameter Gps_evento
		    {
				get
		        {
					if(_Gps_evento_W == null)
	        	    {
						_Gps_evento_W = TearOff.Gps_evento;
					}
					return _Gps_evento_W;
				}
			}

			public AggregateParameter Gps_edaddeldato
		    {
				get
		        {
					if(_Gps_edaddeldato_W == null)
	        	    {
						_Gps_edaddeldato_W = TearOff.Gps_edaddeldato;
					}
					return _Gps_edaddeldato_W;
				}
			}

			public AggregateParameter Gps_hdop
		    {
				get
		        {
					if(_Gps_hdop_W == null)
	        	    {
						_Gps_hdop_W = TearOff.Gps_hdop;
					}
					return _Gps_hdop_W;
				}
			}

			public AggregateParameter Gps_satstatus
		    {
				get
		        {
					if(_Gps_satstatus_W == null)
	        	    {
						_Gps_satstatus_W = TearOff.Gps_satstatus;
					}
					return _Gps_satstatus_W;
				}
			}

			public AggregateParameter Gps_gsmstatus
		    {
				get
		        {
					if(_Gps_gsmstatus_W == null)
	        	    {
						_Gps_gsmstatus_W = TearOff.Gps_gsmstatus;
					}
					return _Gps_gsmstatus_W;
				}
			}

			public AggregateParameter Gps_estado_io
		    {
				get
		        {
					if(_Gps_estado_io_W == null)
	        	    {
						_Gps_estado_io_W = TearOff.Gps_estado_io;
					}
					return _Gps_estado_io_W;
				}
			}

			public AggregateParameter Gps_tipodeposicion
		    {
				get
		        {
					if(_Gps_tipodeposicion_W == null)
	        	    {
						_Gps_tipodeposicion_W = TearOff.Gps_tipodeposicion;
					}
					return _Gps_tipodeposicion_W;
				}
			}

			public AggregateParameter Gps_ip
		    {
				get
		        {
					if(_Gps_ip_W == null)
	        	    {
						_Gps_ip_W = TearOff.Gps_ip;
					}
					return _Gps_ip_W;
				}
			}

			public AggregateParameter Gps_obs
		    {
				get
		        {
					if(_Gps_obs_W == null)
	        	    {
						_Gps_obs_W = TearOff.Gps_obs;
					}
					return _Gps_obs_W;
				}
			}

			public AggregateParameter Idcliente
		    {
				get
		        {
					if(_Idcliente_W == null)
	        	    {
						_Idcliente_W = TearOff.Idcliente;
					}
					return _Idcliente_W;
				}
			}

			public AggregateParameter Icono
		    {
				get
		        {
					if(_Icono_W == null)
	        	    {
						_Icono_W = TearOff.Icono;
					}
					return _Icono_W;
				}
			}

			public AggregateParameter Tipo_de_vehiculo
		    {
				get
		        {
					if(_Tipo_de_vehiculo_W == null)
	        	    {
						_Tipo_de_vehiculo_W = TearOff.Tipo_de_vehiculo;
					}
					return _Tipo_de_vehiculo_W;
				}
			}

			public AggregateParameter Cliente
		    {
				get
		        {
					if(_Cliente_W == null)
	        	    {
						_Cliente_W = TearOff.Cliente;
					}
					return _Cliente_W;
				}
			}

			public AggregateParameter Estado_cliente
		    {
				get
		        {
					if(_Estado_cliente_W == null)
	        	    {
						_Estado_cliente_W = TearOff.Estado_cliente;
					}
					return _Estado_cliente_W;
				}
			}

			public AggregateParameter Id_vehiculo
		    {
				get
		        {
					if(_Id_vehiculo_W == null)
	        	    {
						_Id_vehiculo_W = TearOff.Id_vehiculo;
					}
					return _Id_vehiculo_W;
				}
			}

			public AggregateParameter Identificador_rastreo
		    {
				get
		        {
					if(_Identificador_rastreo_W == null)
	        	    {
						_Identificador_rastreo_W = TearOff.Identificador_rastreo;
					}
					return _Identificador_rastreo_W;
				}
			}

			public AggregateParameter Consumo_aprox
		    {
				get
		        {
					if(_Consumo_aprox_W == null)
	        	    {
						_Consumo_aprox_W = TearOff.Consumo_aprox;
					}
					return _Consumo_aprox_W;
				}
			}

			public AggregateParameter Anho
		    {
				get
		        {
					if(_Anho_W == null)
	        	    {
						_Anho_W = TearOff.Anho;
					}
					return _Anho_W;
				}
			}

			public AggregateParameter Chasis
		    {
				get
		        {
					if(_Chasis_W == null)
	        	    {
						_Chasis_W = TearOff.Chasis;
					}
					return _Chasis_W;
				}
			}

			public AggregateParameter Color
		    {
				get
		        {
					if(_Color_W == null)
	        	    {
						_Color_W = TearOff.Color;
					}
					return _Color_W;
				}
			}

			public AggregateParameter Matricula
		    {
				get
		        {
					if(_Matricula_W == null)
	        	    {
						_Matricula_W = TearOff.Matricula;
					}
					return _Matricula_W;
				}
			}

			public AggregateParameter Kilometraje
		    {
				get
		        {
					if(_Kilometraje_W == null)
	        	    {
						_Kilometraje_W = TearOff.Kilometraje;
					}
					return _Kilometraje_W;
				}
			}

			public AggregateParameter Activo
		    {
				get
		        {
					if(_Activo_W == null)
	        	    {
						_Activo_W = TearOff.Activo;
					}
					return _Activo_W;
				}
			}

			public AggregateParameter Idmarca
		    {
				get
		        {
					if(_Idmarca_W == null)
	        	    {
						_Idmarca_W = TearOff.Idmarca;
					}
					return _Idmarca_W;
				}
			}

			public AggregateParameter Idmodelo
		    {
				get
		        {
					if(_Idmodelo_W == null)
	        	    {
						_Idmodelo_W = TearOff.Idmodelo;
					}
					return _Idmodelo_W;
				}
			}

			public AggregateParameter Marca
		    {
				get
		        {
					if(_Marca_W == null)
	        	    {
						_Marca_W = TearOff.Marca;
					}
					return _Marca_W;
				}
			}

			public AggregateParameter Modelo
		    {
				get
		        {
					if(_Modelo_W == null)
	        	    {
						_Modelo_W = TearOff.Modelo;
					}
					return _Modelo_W;
				}
			}

			public AggregateParameter Idequipogps
		    {
				get
		        {
					if(_Idequipogps_W == null)
	        	    {
						_Idequipogps_W = TearOff.Idequipogps;
					}
					return _Idequipogps_W;
				}
			}

			public AggregateParameter Id_tipoequipogps
		    {
				get
		        {
					if(_Id_tipoequipogps_W == null)
	        	    {
						_Id_tipoequipogps_W = TearOff.Id_tipoequipogps;
					}
					return _Id_tipoequipogps_W;
				}
			}

			public AggregateParameter Tipoequipogps
		    {
				get
		        {
					if(_Tipoequipogps_W == null)
	        	    {
						_Tipoequipogps_W = TearOff.Tipoequipogps;
					}
					return _Tipoequipogps_W;
				}
			}

			public AggregateParameter Tipo_de_reportegps
		    {
				get
		        {
					if(_Tipo_de_reportegps_W == null)
	        	    {
						_Tipo_de_reportegps_W = TearOff.Tipo_de_reportegps;
					}
					return _Tipo_de_reportegps_W;
				}
			}

			public AggregateParameter Id_equipo_gps
		    {
				get
		        {
					if(_Id_equipo_gps_W == null)
	        	    {
						_Id_equipo_gps_W = TearOff.Id_equipo_gps;
					}
					return _Id_equipo_gps_W;
				}
			}

			public AggregateParameter Nro_serie_gps
		    {
				get
		        {
					if(_Nro_serie_gps_W == null)
	        	    {
						_Nro_serie_gps_W = TearOff.Nro_serie_gps;
					}
					return _Nro_serie_gps_W;
				}
			}

			private AggregateParameter _Id_evento_W = null;
			private AggregateParameter _Evento_W = null;
			private AggregateParameter _Color_evento_W = null;
			private AggregateParameter _Evento_habilitado_W = null;
			private AggregateParameter _Idrastreogps_reportes_W = null;
			private AggregateParameter _Id_equipogps_W = null;
			private AggregateParameter _Gps_longitud_W = null;
			private AggregateParameter _Gps_latitud_W = null;
			private AggregateParameter _Gps_fechahora_reporte_W = null;
			private AggregateParameter _Gps_velocidad_W = null;
			private AggregateParameter _Gps_rumbo_W = null;
			private AggregateParameter _Rumbo_W = null;
			private AggregateParameter _Gps_evento_W = null;
			private AggregateParameter _Gps_edaddeldato_W = null;
			private AggregateParameter _Gps_hdop_W = null;
			private AggregateParameter _Gps_satstatus_W = null;
			private AggregateParameter _Gps_gsmstatus_W = null;
			private AggregateParameter _Gps_estado_io_W = null;
			private AggregateParameter _Gps_tipodeposicion_W = null;
			private AggregateParameter _Gps_ip_W = null;
			private AggregateParameter _Gps_obs_W = null;
			private AggregateParameter _Idcliente_W = null;
			private AggregateParameter _Icono_W = null;
			private AggregateParameter _Tipo_de_vehiculo_W = null;
			private AggregateParameter _Cliente_W = null;
			private AggregateParameter _Estado_cliente_W = null;
			private AggregateParameter _Id_vehiculo_W = null;
			private AggregateParameter _Identificador_rastreo_W = null;
			private AggregateParameter _Consumo_aprox_W = null;
			private AggregateParameter _Anho_W = null;
			private AggregateParameter _Chasis_W = null;
			private AggregateParameter _Color_W = null;
			private AggregateParameter _Matricula_W = null;
			private AggregateParameter _Kilometraje_W = null;
			private AggregateParameter _Activo_W = null;
			private AggregateParameter _Idmarca_W = null;
			private AggregateParameter _Idmodelo_W = null;
			private AggregateParameter _Marca_W = null;
			private AggregateParameter _Modelo_W = null;
			private AggregateParameter _Idequipogps_W = null;
			private AggregateParameter _Id_tipoequipogps_W = null;
			private AggregateParameter _Tipoequipogps_W = null;
			private AggregateParameter _Tipo_de_reportegps_W = null;
			private AggregateParameter _Id_equipo_gps_W = null;
			private AggregateParameter _Nro_serie_gps_W = null;

			public void AggregateClauseReset()
			{
				_Id_evento_W = null;
				_Evento_W = null;
				_Color_evento_W = null;
				_Evento_habilitado_W = null;
				_Idrastreogps_reportes_W = null;
				_Id_equipogps_W = null;
				_Gps_longitud_W = null;
				_Gps_latitud_W = null;
				_Gps_fechahora_reporte_W = null;
				_Gps_velocidad_W = null;
				_Gps_rumbo_W = null;
				_Rumbo_W = null;
				_Gps_evento_W = null;
				_Gps_edaddeldato_W = null;
				_Gps_hdop_W = null;
				_Gps_satstatus_W = null;
				_Gps_gsmstatus_W = null;
				_Gps_estado_io_W = null;
				_Gps_tipodeposicion_W = null;
				_Gps_ip_W = null;
				_Gps_obs_W = null;
				_Idcliente_W = null;
				_Icono_W = null;
				_Tipo_de_vehiculo_W = null;
				_Cliente_W = null;
				_Estado_cliente_W = null;
				_Id_vehiculo_W = null;
				_Identificador_rastreo_W = null;
				_Consumo_aprox_W = null;
				_Anho_W = null;
				_Chasis_W = null;
				_Color_W = null;
				_Matricula_W = null;
				_Kilometraje_W = null;
				_Activo_W = null;
				_Idmarca_W = null;
				_Idmodelo_W = null;
				_Marca_W = null;
				_Modelo_W = null;
				_Idequipogps_W = null;
				_Id_tipoequipogps_W = null;
				_Tipoequipogps_W = null;
				_Tipo_de_reportegps_W = null;
				_Id_equipo_gps_W = null;
				_Nro_serie_gps_W = null;

				this._entity.Query.FlushAggregateParameters();

			}
	
			private BusinessEntity _entity;
			private TearOffAggregateParameter _tearOff;
			
		}
	
		public AggregateClause Aggregate
		{
			get
			{
				if(_aggregateClause == null)
				{
					_aggregateClause = new AggregateClause(this);
				}
		
				return _aggregateClause;
			}
		}
		
		private AggregateClause _aggregateClause = null;	
		#endregion
	
		protected override IDbCommand GetInsertCommand() 
		{
			return null;
		}
	
		protected override IDbCommand GetUpdateCommand()
		{
			return null;
		}
	
		protected override IDbCommand GetDeleteCommand()
		{
			return null;
		}
	}
}
