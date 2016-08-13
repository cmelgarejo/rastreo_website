
CREATE OR REPLACE FUNCTION rastreo_ciudad_load_by_primarykey
(
 integer,
 integer,
 integer
)
RETURNS "rastreo_ciudad" AS '
	SELECT
		"idrastreo_ciudad" ,
		"id_pais" ,
		"id_distrito" ,
		"ciudad" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_ciudad"
	WHERE
		 "idrastreo_ciudad" = $1 AND
		 "id_pais" = $2 AND
		 "id_distrito" = $3;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_ciudad_load_all()
RETURNS SETOF "rastreo_ciudad" AS '
	SELECT
		"idrastreo_ciudad",
		"id_pais",
		"id_distrito",
		"ciudad",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_ciudad";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_ciudad_update
(
	integer,
	integer,
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_ciudad"
	SET
		"ciudad" = $4,
		"user_ins" = $5,
		"fech_ins" = $6,
		"user_upd" = $7,
		"fech_upd" = $8
	
	WHERE
		"idrastreo_ciudad" = $1 AND 
		"id_pais" = $2 AND 
		"id_distrito" = $3;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_ciudad_insert
(
	integer,
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_ciudad" AS '
	INSERT INTO "rastreo_ciudad"
	(
		"id_pais",
		"id_distrito",
		"ciudad",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7
	);
	SELECT * FROM "rastreo_ciudad" WHERE "idrastreo_ciudad" = currval(''rastreo_ciudad_idrastreo_ciudad_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_ciudad_delete
(
	integer,
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_ciudad"
	WHERE
		"idrastreo_ciudad" = $1 AND
		"id_pais" = $2 AND
		"id_distrito" = $3;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_load_by_primarykey
(
 integer
)
RETURNS "rastreo_cliente" AS '
	SELECT
		"id_cliente" ,
		"clave_personal" ,
		"acceso_sistema" ,
		"estado_cliente" ,
		"estado_fecha" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"reporte_automatico" ,
		"ra_horaenvio" 
	FROM "rastreo_cliente"
	WHERE
		 "id_cliente" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_cliente_load_all()
RETURNS SETOF "rastreo_cliente" AS '
	SELECT
		"id_cliente",
		"clave_personal",
		"acceso_sistema",
		"estado_cliente",
		"estado_fecha",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"reporte_automatico",
		"ra_horaenvio"
	FROM "rastreo_cliente";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_cliente_update
(
	integer,
	character varying,
	boolean,
	boolean,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	boolean,
	timestamp without time zone,
)
RETURNS void AS '
	UPDATE "rastreo_cliente"
	SET
		"clave_personal" = $2,
		"acceso_sistema" = $3,
		"estado_cliente" = $4,
		"estado_fecha" = $5,
		"user_ins" = $6,
		"fech_ins" = $7,
		"user_upd" = $8,
		"fech_upd" = $9,
		"reporte_automatico" = $10,
		"ra_horaenvio" = $11,
			
	WHERE
		"id_cliente" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_insert
(
	integer,
	character varying,
	boolean,
	boolean,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	boolean,
	timestamp without time zone,
	)
RETURNS "void" AS '
	INSERT INTO "rastreo_cliente"
	(
		"id_cliente",
		"clave_personal",
		"acceso_sistema",
		"estado_cliente",
		"estado_fecha",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"reporte_automatico",
		"ra_horaenvio",
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
	);

' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_cliente"
	WHERE
		"id_cliente" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_contactos_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreo_cliente_contactos" AS '
	SELECT
		"idrastreo_cliente_contactos" ,
		"rastreo_cliente_id_cliente" ,
		"nombre_apellido_razonsocial" ,
		"nrodoc_ruc" ,
		"relacion" ,
		"telefono" ,
		"email" ,
		"titular_secundario" ,
		"autorizado" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_cliente_contactos"
	WHERE
		 "idrastreo_cliente_contactos" = $1 AND
		 "rastreo_cliente_id_cliente" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_cliente_contactos_load_all()
RETURNS SETOF "rastreo_cliente_contactos" AS '
	SELECT
		"idrastreo_cliente_contactos",
		"rastreo_cliente_id_cliente",
		"nombre_apellido_razonsocial",
		"nrodoc_ruc",
		"relacion",
		"telefono",
		"email",
		"titular_secundario",
		"autorizado",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_cliente_contactos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_cliente_contactos_update
(
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	boolean,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_cliente_contactos"
	SET
		"nombre_apellido_razonsocial" = $3,
		"nrodoc_ruc" = $4,
		"relacion" = $5,
		"telefono" = $6,
		"email" = $7,
		"titular_secundario" = $8,
		"autorizado" = $9,
		"user_ins" = $10,
		"fech_ins" = $11,
		"user_upd" = $12,
		"fech_upd" = $13
	
	WHERE
		"idrastreo_cliente_contactos" = $1 AND 
		"rastreo_cliente_id_cliente" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_contactos_insert
(
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	boolean,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_cliente_contactos" AS '
	INSERT INTO "rastreo_cliente_contactos"
	(
		"rastreo_cliente_id_cliente",
		"nombre_apellido_razonsocial",
		"nrodoc_ruc",
		"relacion",
		"telefono",
		"email",
		"titular_secundario",
		"autorizado",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12
	);
	SELECT * FROM "rastreo_cliente_contactos" WHERE "idrastreo_cliente_contactos" = currval(''rastreo_cliente_contactos_idrastreo_cliente_contactos_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_contactos_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_cliente_contactos"
	WHERE
		"idrastreo_cliente_contactos" = $1 AND
		"rastreo_cliente_id_cliente" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_documentos_load_by_primarykey
(
 integer
)
RETURNS "rastreo_cliente_documentos" AS '
	SELECT
		"idrastreo_cliente_documentos" ,
		"idcliente" ,
		"descripcion" ,
		"tipo_archivo" ,
		"nombre_archivo" ,
		"documento" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_cliente_documentos"
	WHERE
		 "idrastreo_cliente_documentos" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_cliente_documentos_load_all()
RETURNS SETOF "rastreo_cliente_documentos" AS '
	SELECT
		"idrastreo_cliente_documentos",
		"idcliente",
		"descripcion",
		"tipo_archivo",
		"nombre_archivo",
		"documento",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_cliente_documentos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_cliente_documentos_update
(
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	bytea,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_cliente_documentos"
	SET
		"idcliente" = $2,
		"descripcion" = $3,
		"tipo_archivo" = $4,
		"nombre_archivo" = $5,
		"documento" = $6,
		"user_ins" = $7,
		"fech_ins" = $8,
		"user_upd" = $9,
		"fech_upd" = $10
	
	WHERE
		"idrastreo_cliente_documentos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_documentos_insert
(
	integer,
	character varying,
	character varying,
	character varying,
	bytea,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_cliente_documentos" AS '
	INSERT INTO "rastreo_cliente_documentos"
	(
		"idcliente",
		"descripcion",
		"tipo_archivo",
		"nombre_archivo",
		"documento",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9
	);
	SELECT * FROM "rastreo_cliente_documentos" WHERE "idrastreo_cliente_documentos" = currval(''rastreo_cliente_documentos_idrastreo_cliente_documentos_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_cliente_documentos_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_cliente_documentos"
	WHERE
		"idrastreo_cliente_documentos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_distrito_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreo_distrito" AS '
	SELECT
		"idrastreo_distrito" ,
		"id_pais" ,
		"distrito" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_distrito"
	WHERE
		 "idrastreo_distrito" = $1 AND
		 "id_pais" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_distrito_load_all()
RETURNS SETOF "rastreo_distrito" AS '
	SELECT
		"idrastreo_distrito",
		"id_pais",
		"distrito",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_distrito";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_distrito_update
(
	integer,
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_distrito"
	SET
		"distrito" = $3,
		"user_ins" = $4,
		"fech_ins" = $5,
		"user_upd" = $6,
		"fech_upd" = $7
	
	WHERE
		"idrastreo_distrito" = $1 AND 
		"id_pais" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_distrito_insert
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_distrito" AS '
	INSERT INTO "rastreo_distrito"
	(
		"id_pais",
		"distrito",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6
	);
	SELECT * FROM "rastreo_distrito" WHERE "idrastreo_distrito" = currval(''rastreo_distrito_idrastreo_distrito_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_distrito_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_distrito"
	WHERE
		"idrastreo_distrito" = $1 AND
		"id_pais" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_empleados_load_by_primarykey
(
 integer
)
RETURNS "rastreo_empleados" AS '
	SELECT
		"id_empleado" ,
		"instalador" ,
		"acceso_sistema" ,
		"estado_empleado" ,
		"estado_fecha" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"vendedor" 
	FROM "rastreo_empleados"
	WHERE
		 "id_empleado" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_empleados_load_all()
RETURNS SETOF "rastreo_empleados" AS '
	SELECT
		"id_empleado",
		"instalador",
		"acceso_sistema",
		"estado_empleado",
		"estado_fecha",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"vendedor" 
	FROM "rastreo_empleados";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_empleados_update
(
	integer,
	boolean,
	boolean,
	boolean,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	boolean
	
)
RETURNS void AS '
	UPDATE "rastreo_empleados"
	SET
		"instalador" = $2,
		"acceso_sistema" = $3,
		"estado_empleado" = $4,
		"estado_fecha" = $5,
		"user_ins" = $6,
		"fech_ins" = $7,
		"user_upd" = $8,
		"fech_upd" = $9,
		"vendedor" = $10		
	WHERE
		"id_empleado" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_empleados_insert
(
	integer,
	boolean,
	boolean,
	boolean,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	boolean
)
RETURNS "void" AS '
	INSERT INTO "rastreo_empleados"
	(
		"id_empleado",
		"instalador",
		"acceso_sistema",
		"estado_empleado",
		"estado_fecha",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"vendedor"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10
	);

' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_empleados_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_empleados"
	WHERE
		"id_empleado" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_geocercas_load_by_primarykey
(
 integer
)
RETURNS "rastreo_geocercas" AS '
	SELECT
		"idrastreo_geocercas" ,
		"id_cliente" ,
		"id_vehiculo" ,
		"descripcion" ,
		"puntos" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"activa" ,
		"hora_activacion" ,
		"horas_activa" ,
		"ultimo_aviso" ,
		"avisar_entrada" ,
		"mails" ,
		"tel_movil" ,
		"activo_siempre" ,
		"dia_activacion" ,
		"avisos_activado" ,
		"id_usuario" 
	FROM "rastreo_geocercas"
	WHERE
		 "idrastreo_geocercas" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_geocercas_load_all()
RETURNS SETOF "rastreo_geocercas" AS '
	SELECT
		"idrastreo_geocercas",
		"id_cliente",
		"id_vehiculo",
		"descripcion",
		"puntos",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"activa",
		"hora_activacion",
		"horas_activa",
		"ultimo_aviso",
		"avisar_entrada",
		"mails",
		"tel_movil",
		"activo_siempre",
		"dia_activacion",
		"avisos_activado",
		"id_usuario"
	FROM "rastreo_geocercas";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_geocercas_update
(
	integer,
	integer,
	integer,
	character varying,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	boolean,
	time without time zone,
	integer,
	timestamp without time zone,
	boolean,
	text,
	text,
	boolean,
	timestamp without time zone,
	boolean,
	integer
)
RETURNS void AS '
	UPDATE "rastreo_geocercas"
	SET
		"id_cliente" = $2,
		"id_vehiculo" = $3,
		"descripcion" = $4,
		"puntos" = $5,
		"user_ins" = $6,
		"fech_ins" = $7,
		"user_upd" = $8,
		"fech_upd" = $9,
		"activa" = $10,
		"hora_activacion" = $11,
		"horas_activa" = $12,
		"ultimo_aviso" = $13,
		"avisar_entrada" = $14,
		"mails" = $15,
		"tel_movil" = $16,
		"activo_siempre" = $17,
		"dia_activacion" = $18,
		"avisos_activado" = $19,
		"id_usuario" = $20
	
	WHERE
		"idrastreo_geocercas" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_geocercas_insert
(
	integer,
	integer,
	character varying,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	boolean,
	time without time zone,
	integer,
	timestamp without time zone,
	boolean,
	text,
	text,
	boolean,
	timestamp without time zone,
	boolean,
	integer
)
RETURNS "rastreo_geocercas" AS '
	INSERT INTO "rastreo_geocercas"
	(
		"id_cliente",
		"id_vehiculo",
		"descripcion",
		"puntos",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"activa",
		"hora_activacion",
		"horas_activa",
		"ultimo_aviso",
		"avisar_entrada",
		"mails",
		"tel_movil",
		"activo_siempre",
		"dia_activacion",
		"avisos_activado",
		"id_usuario"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16,
		$17,
		$18,
		$19
	);
	SELECT * FROM "rastreo_geocercas" WHERE "idrastreo_geocercas" = currval(''rastreo_geocercas_idrastreo_geocercas_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_geocercas_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_geocercas"
	WHERE
		"idrastreo_geocercas" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_avisos_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_avisos" AS '
	SELECT
		"idrastreogps_avisos" ,
		"id_equipogps" ,
		"evento" ,
		"evento_fechahora" ,
		"atendido" ,
		"gps_fechahora_reporte" ,
		"gps_latitud" ,
		"gps_longitud" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_dir" 
	FROM "rastreogps_avisos"
	WHERE
		 "idrastreogps_avisos" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_avisos_load_all()
RETURNS SETOF "rastreogps_avisos" AS '
	SELECT
		"idrastreogps_avisos",
		"id_equipogps",
		"evento",
		"evento_fechahora",
		"atendido",
		"gps_fechahora_reporte",
		"gps_latitud",
		"gps_longitud",
		"gps_velocidad",
		"gps_rumbo",
		"gps_dir"
	FROM "rastreogps_avisos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_avisos_update
(
	integer,
	integer,
	text,
	timestamp without time zone,
	boolean,
	timestamp without time zone,
	double precision,
	double precision,
	integer,
	integer,
	text
)
RETURNS void AS '
	UPDATE "rastreogps_avisos"
	SET
		"id_equipogps" = $2,
		"evento" = $3,
		"evento_fechahora" = $4,
		"atendido" = $5,
		"gps_fechahora_reporte" = $6,
		"gps_latitud" = $7,
		"gps_longitud" = $8,
		"gps_velocidad" = $9,
		"gps_rumbo" = $10,
		"gps_dir" = $11
	
	WHERE
		"idrastreogps_avisos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_avisos_insert
(
	integer,
	text,
	timestamp without time zone,
	boolean,
	timestamp without time zone,
	double precision,
	double precision,
	integer,
	integer,
	text
)
RETURNS "rastreogps_avisos" AS '
	INSERT INTO "rastreogps_avisos"
	(
		"id_equipogps",
		"evento",
		"evento_fechahora",
		"atendido",
		"gps_fechahora_reporte",
		"gps_latitud",
		"gps_longitud",
		"gps_velocidad",
		"gps_rumbo",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10
	);
	SELECT * FROM "rastreogps_avisos" WHERE "idrastreogps_avisos" = currval(''rastreogps_avisos_idrastreogps_avisos_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_avisos_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_avisos"
	WHERE
		"idrastreogps_avisos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_bandeja_de_entrada_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_bandeja_de_entrada" AS '
	SELECT
		"idrastreogps_bandeja_de_entrada" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fecha" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "rastreogps_bandeja_de_entrada"
	WHERE
		 "idrastreogps_bandeja_de_entrada" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_bandeja_de_entrada_load_all()
RETURNS SETOF "rastreogps_bandeja_de_entrada" AS '
	SELECT
		"idrastreogps_bandeja_de_entrada",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fecha",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "rastreogps_bandeja_de_entrada";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_bandeja_de_entrada_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "rastreogps_bandeja_de_entrada"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fecha" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idrastreogps_bandeja_de_entrada" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_bandeja_de_entrada_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "rastreogps_bandeja_de_entrada" AS '
	INSERT INTO "rastreogps_bandeja_de_entrada"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fecha",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "rastreogps_bandeja_de_entrada" WHERE "idrastreogps_bandeja_de_entrada" = currval(''rastreogps_bandeja_de_entrada_idrastreogps_bandeja_de_entra_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_bandeja_de_entrada_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_bandeja_de_entrada"
	WHERE
		"idrastreogps_bandeja_de_entrada" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_cola_de_comandos_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_cola_de_comandos" AS '
	SELECT
		"idrastreogps_cola_de_comandos" ,
		"idequipogps" ,
		"descripcion" ,
		"comando" ,
		"user_ins" ,
		"fech_ins" ,
		"enviado" 
	FROM "rastreogps_cola_de_comandos"
	WHERE
		 "idrastreogps_cola_de_comandos" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_cola_de_comandos_load_all()
RETURNS SETOF "rastreogps_cola_de_comandos" AS '
	SELECT
		"idrastreogps_cola_de_comandos",
		"idequipogps",
		"descripcion",
		"comando",
		"user_ins",
		"fech_ins",
		"enviado"
	FROM "rastreogps_cola_de_comandos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_cola_de_comandos_update
(
	integer,
	integer,
	text,
	text,
	integer,
	timestamp without time zone,
	boolean
)
RETURNS void AS '
	UPDATE "rastreogps_cola_de_comandos"
	SET
		"idequipogps" = $2,
		"descripcion" = $3,
		"comando" = $4,
		"user_ins" = $5,
		"fech_ins" = $6,
		"enviado" = $7
	
	WHERE
		"idrastreogps_cola_de_comandos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_cola_de_comandos_insert
(
	integer,
	text,
	text,
	integer,
	timestamp without time zone,
	boolean
)
RETURNS "rastreogps_cola_de_comandos" AS '
	INSERT INTO "rastreogps_cola_de_comandos"
	(
		"idequipogps",
		"descripcion",
		"comando",
		"user_ins",
		"fech_ins",
		"enviado"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6
	);
	SELECT * FROM "rastreogps_cola_de_comandos" WHERE "idrastreogps_cola_de_comandos" = currval(''rastreogps_cola_de_comandos_idrastreogps_cola_de_comandos_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_cola_de_comandos_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_cola_de_comandos"
	WHERE
		"idrastreogps_cola_de_comandos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_equipo_eventos_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreogps_equipo_eventos" AS '
	SELECT
		"id_equipogps" ,
		"id_tipoevento" ,
		"habilitado" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"evento" ,
		"sonoro" ,
		"arch_sonido" ,
		"email" 
	FROM "rastreogps_equipo_eventos"
	WHERE
		 "id_equipogps" = $1 AND
		 "id_tipoevento" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_equipo_eventos_load_all()
RETURNS SETOF "rastreogps_equipo_eventos" AS '
	SELECT
		"id_equipogps",
		"id_tipoevento",
		"habilitado",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"evento",
		"sonoro",
		"arch_sonido",
		"email"
	FROM "rastreogps_equipo_eventos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_equipo_eventos_update
(
	integer,
	integer,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	text,
	boolean,
	text,
	text
)
RETURNS void AS '
	UPDATE "rastreogps_equipo_eventos"
	SET
		"habilitado" = $3,
		"user_ins" = $4,
		"fech_ins" = $5,
		"user_upd" = $6,
		"fech_upd" = $7,
		"evento" = $8,
		"sonoro" = $9,
		"arch_sonido" = $10,
		"email" = $11
	
	WHERE
		"id_equipogps" = $1 AND 
		"id_tipoevento" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_equipo_eventos_insert
(
	integer,
	integer,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	text,
	boolean,
	text,
	text
)
RETURNS "void" AS '
	INSERT INTO "rastreogps_equipo_eventos"
	(
		"id_equipogps",
		"id_tipoevento",
		"habilitado",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"evento",
		"sonoro",
		"arch_sonido",
		"email"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11
	);

' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_equipo_eventos_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_equipo_eventos"
	WHERE
		"id_equipogps" = $1 AND
		"id_tipoevento" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_equipogps_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_equipogps" AS '
	SELECT
		"idrastreogps_equipogps" ,
		"id_simcard" ,
		"tipoequipo" ,
		"id_equipo_gps" ,
		"nro_serie_gps" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreogps_equipogps"
	WHERE
		 "idrastreogps_equipogps" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_equipogps_load_all()
RETURNS SETOF "rastreogps_equipogps" AS '
	SELECT
		"idrastreogps_equipogps",
		"id_simcard",
		"tipoequipo",
		"id_equipo_gps",
		"nro_serie_gps",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreogps_equipogps";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_equipogps_update
(
	integer,
	integer,
	integer,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_equipogps"
	SET
		"id_simcard" = $2,
		"tipoequipo" = $3,
		"id_equipo_gps" = $4,
		"nro_serie_gps" = $5,
		"user_ins" = $6,
		"fech_ins" = $7,
		"user_upd" = $8,
		"fech_upd" = $9
	
	WHERE
		"idrastreogps_equipogps" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_equipogps_insert
(
	integer,
	integer,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreogps_equipogps" AS '
	INSERT INTO "rastreogps_equipogps"
	(
		"id_simcard",
		"tipoequipo",
		"id_equipo_gps",
		"nro_serie_gps",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8
	);
	SELECT * FROM "rastreogps_equipogps" WHERE "idrastreogps_equipogps" = currval(''rastreogps_equipogps_idrastreogps_equipogps_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_equipogps_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_equipogps"
	WHERE
		"idrastreogps_equipogps" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_fuel_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreogps_fuel" AS '
	SELECT
		"idrastreogps_fuel" ,
		"id_equipogps" ,
		"fuel" ,
		"consumo" ,
		"estado" ,
		"fech_ins" 
	FROM "rastreogps_fuel"
	WHERE
		 "idrastreogps_fuel" = $1 AND
		 "id_equipogps" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_fuel_load_all()
RETURNS SETOF "rastreogps_fuel" AS '
	SELECT
		"idrastreogps_fuel",
		"id_equipogps",
		"fuel",
		"consumo",
		"estado",
		"fech_ins"
	FROM "rastreogps_fuel";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_fuel_update
(
	integer,
	integer,
	character varying,
	character varying,
	boolean,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_fuel"
	SET
		"fuel" = $3,
		"consumo" = $4,
		"estado" = $5,
		"fech_ins" = $6
	
	WHERE
		"idrastreogps_fuel" = $1 AND 
		"id_equipogps" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_fuel_insert
(
	integer,
	character varying,
	character varying,
	boolean,
	timestamp without time zone
)
RETURNS "rastreogps_fuel" AS '
	INSERT INTO "rastreogps_fuel"
	(
		"id_equipogps",
		"fuel",
		"consumo",
		"estado",
		"fech_ins"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreogps_fuel" WHERE "idrastreogps_fuel" = currval(''rastreogps_fuel_idrastreogps_fuel_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_fuel_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_fuel"
	WHERE
		"idrastreogps_fuel" = $1 AND
		"id_equipogps" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_odometro_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreogps_odometro" AS '
	SELECT
		"idrastreogps_odometro" ,
		"id_equipogps" ,
		"odometro" ,
		"fech_ins" 
	FROM "rastreogps_odometro"
	WHERE
		 "idrastreogps_odometro" = $1 AND
		 "id_equipogps" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_odometro_load_all()
RETURNS SETOF "rastreogps_odometro" AS '
	SELECT
		"idrastreogps_odometro",
		"id_equipogps",
		"odometro",
		"fech_ins"
	FROM "rastreogps_odometro";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_odometro_update
(
	integer,
	integer,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_odometro"
	SET
		"odometro" = $3,
		"fech_ins" = $4
	
	WHERE
		"idrastreogps_odometro" = $1 AND 
		"id_equipogps" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_odometro_insert
(
	integer,
	integer,
	timestamp without time zone
)
RETURNS "rastreogps_odometro" AS '
	INSERT INTO "rastreogps_odometro"
	(
		"id_equipogps",
		"odometro",
		"fech_ins"
	)
	VALUES
	(
		$1,
		$2,
		$3
	);
	SELECT * FROM "rastreogps_odometro" WHERE "idrastreogps_odometro" = currval(''rastreogps_odometro_idrastreogps_odometro_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_odometro_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_odometro"
	WHERE
		"idrastreogps_odometro" = $1 AND
		"id_equipogps" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_opciones_de_pantalla_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_opciones_de_pantalla" AS '
	SELECT
		"idrastreogps_opciones_de_pantalla" ,
		"opcion_de_pantalla" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreogps_opciones_de_pantalla"
	WHERE
		 "idrastreogps_opciones_de_pantalla" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_opciones_de_pantalla_load_all()
RETURNS SETOF "rastreogps_opciones_de_pantalla" AS '
	SELECT
		"idrastreogps_opciones_de_pantalla",
		"opcion_de_pantalla",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreogps_opciones_de_pantalla";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_opciones_de_pantalla_update
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_opciones_de_pantalla"
	SET
		"opcion_de_pantalla" = $2,
		"user_ins" = $3,
		"fech_ins" = $4,
		"user_upd" = $5,
		"fech_upd" = $6
	
	WHERE
		"idrastreogps_opciones_de_pantalla" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_opciones_de_pantalla_insert
(
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreogps_opciones_de_pantalla" AS '
	INSERT INTO "rastreogps_opciones_de_pantalla"
	(
		"opcion_de_pantalla",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreogps_opciones_de_pantalla" WHERE "idrastreogps_opciones_de_pantalla" = currval(''rastreogps_opciones_de_pantal_idrastreogps_opciones_de_pant_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_opciones_de_pantalla_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_opciones_de_pantalla"
	WHERE
		"idrastreogps_opciones_de_pantalla" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_simcards_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_simcards" AS '
	SELECT
		"idrastreogps_simcards" ,
		"sim_nro" ,
		"sim_pin" ,
		"sim_pin2" ,
		"sim_puk" ,
		"sim_puk2" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreogps_simcards"
	WHERE
		 "idrastreogps_simcards" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_simcards_load_all()
RETURNS SETOF "rastreogps_simcards" AS '
	SELECT
		"idrastreogps_simcards",
		"sim_nro",
		"sim_pin",
		"sim_pin2",
		"sim_puk",
		"sim_puk2",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreogps_simcards";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_simcards_update
(
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_simcards"
	SET
		"sim_nro" = $2,
		"sim_pin" = $3,
		"sim_pin2" = $4,
		"sim_puk" = $5,
		"sim_puk2" = $6,
		"user_ins" = $7,
		"fech_ins" = $8,
		"user_upd" = $9,
		"fech_upd" = $10
	
	WHERE
		"idrastreogps_simcards" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_simcards_insert
(
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreogps_simcards" AS '
	INSERT INTO "rastreogps_simcards"
	(
		"sim_nro",
		"sim_pin",
		"sim_pin2",
		"sim_puk",
		"sim_puk2",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9
	);
	SELECT * FROM "rastreogps_simcards" WHERE "idrastreogps_simcards" = currval(''rastreogps_simcards_idrastreogps_simcards_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_simcards_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_simcards"
	WHERE
		"idrastreogps_simcards" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_tipoequipo" AS '
	SELECT
		"idrastreogps_tipoequipo" ,
		"tipo_equipo" ,
		"tipo_de_reporte" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreogps_tipoequipo"
	WHERE
		 "idrastreogps_tipoequipo" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_load_all()
RETURNS SETOF "rastreogps_tipoequipo" AS '
	SELECT
		"idrastreogps_tipoequipo",
		"tipo_equipo",
		"tipo_de_reporte",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreogps_tipoequipo";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_update
(
	integer,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_tipoequipo"
	SET
		"tipo_equipo" = $2,
		"tipo_de_reporte" = $3,
		"user_ins" = $4,
		"fech_ins" = $5,
		"user_upd" = $6,
		"fech_upd" = $7
	
	WHERE
		"idrastreogps_tipoequipo" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_insert
(
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreogps_tipoequipo" AS '
	INSERT INTO "rastreogps_tipoequipo"
	(
		"tipo_equipo",
		"tipo_de_reporte",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6
	);
	SELECT * FROM "rastreogps_tipoequipo" WHERE "idrastreogps_tipoequipo" = currval(''rastreogps_tipoequipo_idrastreogps_tipoequipo_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_tipoequipo"
	WHERE
		"idrastreogps_tipoequipo" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_comandos_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_tipoequipo_comandos" AS '
	SELECT
		"idrastreogps_tipoequipo_comandos" ,
		"id_tipoequipo" ,
		"descripcion" ,
		"comando" ,
		"user_ins" ,
		"fech_ins" 
	FROM "rastreogps_tipoequipo_comandos"
	WHERE
		 "idrastreogps_tipoequipo_comandos" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_comandos_load_all()
RETURNS SETOF "rastreogps_tipoequipo_comandos" AS '
	SELECT
		"idrastreogps_tipoequipo_comandos",
		"id_tipoequipo",
		"descripcion",
		"comando",
		"user_ins",
		"fech_ins"
	FROM "rastreogps_tipoequipo_comandos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_comandos_update
(
	integer,
	integer,
	text,
	text,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_tipoequipo_comandos"
	SET
		"id_tipoequipo" = $2,
		"descripcion" = $3,
		"comando" = $4,
		"user_ins" = $5,
		"fech_ins" = $6
	
	WHERE
		"idrastreogps_tipoequipo_comandos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_comandos_insert
(
	integer,
	text,
	text,
	integer,
	timestamp without time zone
)
RETURNS "rastreogps_tipoequipo_comandos" AS '
	INSERT INTO "rastreogps_tipoequipo_comandos"
	(
		"id_tipoequipo",
		"descripcion",
		"comando",
		"user_ins",
		"fech_ins"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreogps_tipoequipo_comandos" WHERE "idrastreogps_tipoequipo_comandos" = currval(''rastreogps_tipoequipo_comando_idrastreogps_tipoequipo_coman_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoequipo_comandos_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_tipoequipo_comandos"
	WHERE
		"idrastreogps_tipoequipo_comandos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoevento_load_by_primarykey
(
 integer
)
RETURNS "rastreogps_tipoevento" AS '
	SELECT
		"idrastreogps_tipo_evento" ,
		"id_tipoequipo" ,
		"evento" ,
		"descripcion" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"color" ,
		"sonoro" ,
		"arch_sonido" 
	FROM "rastreogps_tipoevento"
	WHERE
		 "idrastreogps_tipo_evento" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_tipoevento_load_all()
RETURNS SETOF "rastreogps_tipoevento" AS '
	SELECT
		"idrastreogps_tipo_evento",
		"id_tipoequipo",
		"evento",
		"descripcion",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"color",
		"sonoro",
		"arch_sonido"
	FROM "rastreogps_tipoevento";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_tipoevento_update
(
	integer,
	integer,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	character varying,
	boolean,
	text
)
RETURNS void AS '
	UPDATE "rastreogps_tipoevento"
	SET
		"id_tipoequipo" = $2,
		"evento" = $3,
		"descripcion" = $4,
		"user_ins" = $5,
		"fech_ins" = $6,
		"user_upd" = $7,
		"fech_upd" = $8,
		"color" = $9,
		"sonoro" = $10,
		"arch_sonido" = $11
	
	WHERE
		"idrastreogps_tipo_evento" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoevento_insert
(
	integer,
	character varying,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	character varying,
	boolean,
	text
)
RETURNS "rastreogps_tipoevento" AS '
	INSERT INTO "rastreogps_tipoevento"
	(
		"id_tipoequipo",
		"evento",
		"descripcion",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"color",
		"sonoro",
		"arch_sonido"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10
	);
	SELECT * FROM "rastreogps_tipoevento" WHERE "idrastreogps_tipo_evento" = currval(''rastreogps_tipoevento_idrastreogps_tipo_evento_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_tipoevento_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_tipoevento"
	WHERE
		"idrastreogps_tipo_evento" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_usuario_opciones_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreogps_usuario_opciones" AS '
	SELECT
		"opcion_de_pantalla" ,
		"id_usuario" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreogps_usuario_opciones"
	WHERE
		 "opcion_de_pantalla" = $1 AND
		 "id_usuario" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_usuario_opciones_load_all()
RETURNS SETOF "rastreogps_usuario_opciones" AS '
	SELECT
		"opcion_de_pantalla",
		"id_usuario",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreogps_usuario_opciones";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreogps_usuario_opciones_update
(
	integer,
	integer,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreogps_usuario_opciones"
	SET
		"user_ins" = $3,
		"fech_ins" = $4,
		"user_upd" = $5,
		"fech_upd" = $6
	
	WHERE
		"opcion_de_pantalla" = $1 AND 
		"id_usuario" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_usuario_opciones_insert
(
	integer,
	integer,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "void" AS '
	INSERT INTO "rastreogps_usuario_opciones"
	(
		"opcion_de_pantalla",
		"id_usuario",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6
	);

' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreogps_usuario_opciones_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreogps_usuario_opciones"
	WHERE
		"opcion_de_pantalla" = $1 AND
		"id_usuario" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_historial_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreo_historial" AS '
	SELECT
		"idrastreo_historial" ,
		"id_cliente" ,
		"vehiculos_involucrados" ,
		"observacion" ,
		"fecha_verificacion" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_historial"
	WHERE
		 "idrastreo_historial" = $1 AND
		 "id_cliente" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_historial_load_all()
RETURNS SETOF "rastreo_historial" AS '
	SELECT
		"idrastreo_historial",
		"id_cliente",
		"vehiculos_involucrados",
		"observacion",
		"fecha_verificacion",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_historial";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_historial_update
(
	integer,
	integer,
	text,
	text,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_historial"
	SET
		"vehiculos_involucrados" = $3,
		"observacion" = $4,
		"fecha_verificacion" = $5,
		"user_ins" = $6,
		"fech_ins" = $7,
		"user_upd" = $8,
		"fech_upd" = $9
	
	WHERE
		"idrastreo_historial" = $1 AND 
		"id_cliente" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_historial_insert
(
	integer,
	text,
	text,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_historial" AS '
	INSERT INTO "rastreo_historial"
	(
		"id_cliente",
		"vehiculos_involucrados",
		"observacion",
		"fecha_verificacion",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8
	);
	SELECT * FROM "rastreo_historial" WHERE "idrastreo_historial" = currval(''rastreo_historial_idrastreo_historial_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_historial_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_historial"
	WHERE
		"idrastreo_historial" = $1 AND
		"id_cliente" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_load_by_primarykey
(
 integer
)
RETURNS "rastreo_hoja_de_ruta" AS '
	SELECT
		"idhoja_de_ruta" ,
		"id_usuario" ,
		"descripcion" ,
		"activa" ,
		"hora_activacion" ,
		"hora_desactivacion" ,
		"mails" ,
		"tel_movil" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_hoja_de_ruta"
	WHERE
		 "idhoja_de_ruta" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_load_all()
RETURNS SETOF "rastreo_hoja_de_ruta" AS '
	SELECT
		"idhoja_de_ruta",
		"id_usuario",
		"descripcion",
		"activa",
		"hora_activacion",
		"hora_desactivacion",
		"mails",
		"tel_movil",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_hoja_de_ruta";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_update
(
	integer,
	integer,
	text,
	boolean,
	timestamp without time zone,
	timestamp without time zone,
	text,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_hoja_de_ruta"
	SET
		"id_usuario" = $2,
		"descripcion" = $3,
		"activa" = $4,
		"hora_activacion" = $5,
		"hora_desactivacion" = $6,
		"mails" = $7,
		"tel_movil" = $8,
		"user_ins" = $9,
		"fech_ins" = $10,
		"user_upd" = $11,
		"fech_upd" = $12
	
	WHERE
		"idhoja_de_ruta" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_insert
(
	integer,
	text,
	boolean,
	timestamp without time zone,
	timestamp without time zone,
	text,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_hoja_de_ruta" AS '
	INSERT INTO "rastreo_hoja_de_ruta"
	(
		"id_usuario",
		"descripcion",
		"activa",
		"hora_activacion",
		"hora_desactivacion",
		"mails",
		"tel_movil",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11
	);
	SELECT * FROM "rastreo_hoja_de_ruta" WHERE "idhoja_de_ruta" = currval(''rastreo_hoja_de_ruta_idhoja_de_ruta_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_hoja_de_ruta"
	WHERE
		"idhoja_de_ruta" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hojaderuta_has_vehiculo_load_by_primarykey
(
 integer,
 integer,
 integer
)
RETURNS "rastreo_hojaderuta_has_vehiculo" AS '
	SELECT
		"id_cliente" ,
		"id_vehiculo" ,
		"idhoja_de_ruta" 
	FROM "rastreo_hojaderuta_has_vehiculo"
	WHERE
		 "id_cliente" = $1 AND
		 "id_vehiculo" = $2 AND
		 "idhoja_de_ruta" = $3;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_hojaderuta_has_vehiculo_load_all()
RETURNS SETOF "rastreo_hojaderuta_has_vehiculo" AS '
	SELECT
		"id_cliente",
		"id_vehiculo",
		"idhoja_de_ruta"
	FROM "rastreo_hojaderuta_has_vehiculo";
' LANGUAGE 'SQL' STABLE;


-------------------------------------------
-- NO UPDATE Stored Procedure Generated    
-- All Columns are part of the Primary key 
-------------------------------------------



CREATE OR REPLACE FUNCTION rastreo_hojaderuta_has_vehiculo_insert
(
	integer,
	integer,
	integer
)
RETURNS "void" AS '
	INSERT INTO "rastreo_hojaderuta_has_vehiculo"
	(
		"id_cliente",
		"id_vehiculo",
		"idhoja_de_ruta"
	)
	VALUES
	(
		$1,
		$2,
		$3
	);

' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hojaderuta_has_vehiculo_delete
(
	integer,
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_hojaderuta_has_vehiculo"
	WHERE
		"id_cliente" = $1 AND
		"id_vehiculo" = $2 AND
		"idhoja_de_ruta" = $3;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_puntos_load_by_primarykey
(
 integer
)
RETURNS "rastreo_hoja_de_ruta_puntos" AS '
	SELECT
		"id_punto" ,
		"idhoja_de_ruta" ,
		"nombre" ,
		"descripcion" ,
		"orden" ,
		"lon" ,
		"lat" ,
		"hora_llegada" ,
		"minutos_demora" 
	FROM "rastreo_hoja_de_ruta_puntos"
	WHERE
		 "id_punto" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_puntos_load_all()
RETURNS SETOF "rastreo_hoja_de_ruta_puntos" AS '
	SELECT
		"id_punto",
		"idhoja_de_ruta",
		"nombre",
		"descripcion",
		"orden",
		"lon",
		"lat",
		"hora_llegada",
		"minutos_demora"
	FROM "rastreo_hoja_de_ruta_puntos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_puntos_update
(
	integer,
	integer,
	character varying,
	text,
	integer,
	double precision,
	double precision,
	time without time zone,
	integer
)
RETURNS void AS '
	UPDATE "rastreo_hoja_de_ruta_puntos"
	SET
		"idhoja_de_ruta" = $2,
		"nombre" = $3,
		"descripcion" = $4,
		"orden" = $5,
		"lon" = $6,
		"lat" = $7,
		"hora_llegada" = $8,
		"minutos_demora" = $9
	
	WHERE
		"id_punto" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_puntos_insert
(
	integer,
	character varying,
	text,
	integer,
	double precision,
	double precision,
	time without time zone,
	integer
)
RETURNS "rastreo_hoja_de_ruta_puntos" AS '
	INSERT INTO "rastreo_hoja_de_ruta_puntos"
	(
		"idhoja_de_ruta",
		"nombre",
		"descripcion",
		"orden",
		"lon",
		"lat",
		"hora_llegada",
		"minutos_demora"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8
	);
	SELECT * FROM "rastreo_hoja_de_ruta_puntos" WHERE "id_punto" = currval(''rastreo_hoja_de_ruta_puntos_id_punto_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_hoja_de_ruta_puntos_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_hoja_de_ruta_puntos"
	WHERE
		"id_punto" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_marca_vehiculo_load_by_primarykey
(
 integer
)
RETURNS "rastreo_marca_vehiculo" AS '
	SELECT
		"idrastreo_marca_vehiculo" ,
		"marca" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_marca_vehiculo"
	WHERE
		 "idrastreo_marca_vehiculo" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_marca_vehiculo_load_all()
RETURNS SETOF "rastreo_marca_vehiculo" AS '
	SELECT
		"idrastreo_marca_vehiculo",
		"marca",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_marca_vehiculo";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_marca_vehiculo_update
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_marca_vehiculo"
	SET
		"marca" = $2,
		"user_ins" = $3,
		"fech_ins" = $4,
		"user_upd" = $5,
		"fech_upd" = $6
	
	WHERE
		"idrastreo_marca_vehiculo" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_marca_vehiculo_insert
(
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_marca_vehiculo" AS '
	INSERT INTO "rastreo_marca_vehiculo"
	(
		"marca",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreo_marca_vehiculo" WHERE "idrastreo_marca_vehiculo" = currval(''rastreo_marca_vehiculo_idrastreo_marca_vehiculo_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_marca_vehiculo_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_marca_vehiculo"
	WHERE
		"idrastreo_marca_vehiculo" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_modelo_vehiculo_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreo_modelo_vehiculo" AS '
	SELECT
		"idrastreo_modelo_vehiculo" ,
		"id_marca" ,
		"modelo" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_modelo_vehiculo"
	WHERE
		 "idrastreo_modelo_vehiculo" = $1 AND
		 "id_marca" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_modelo_vehiculo_load_all()
RETURNS SETOF "rastreo_modelo_vehiculo" AS '
	SELECT
		"idrastreo_modelo_vehiculo",
		"id_marca",
		"modelo",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_modelo_vehiculo";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_modelo_vehiculo_update
(
	integer,
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_modelo_vehiculo"
	SET
		"modelo" = $3,
		"user_ins" = $4,
		"fech_ins" = $5,
		"user_upd" = $6,
		"fech_upd" = $7
	
	WHERE
		"idrastreo_modelo_vehiculo" = $1 AND 
		"id_marca" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_modelo_vehiculo_insert
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_modelo_vehiculo" AS '
	INSERT INTO "rastreo_modelo_vehiculo"
	(
		"id_marca",
		"modelo",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6
	);
	SELECT * FROM "rastreo_modelo_vehiculo" WHERE "idrastreo_modelo_vehiculo" = currval(''rastreo_modelo_vehiculo_idrastreo_modelo_vehiculo_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_modelo_vehiculo_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_modelo_vehiculo"
	WHERE
		"idrastreo_modelo_vehiculo" = $1 AND
		"id_marca" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_pais_load_by_primarykey
(
 integer
)
RETURNS "rastreo_pais" AS '
	SELECT
		"idrastreo_pais" ,
		"pais" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_pais"
	WHERE
		 "idrastreo_pais" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_pais_load_all()
RETURNS SETOF "rastreo_pais" AS '
	SELECT
		"idrastreo_pais",
		"pais",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_pais";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_pais_update
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_pais"
	SET
		"pais" = $2,
		"user_ins" = $3,
		"fech_ins" = $4,
		"user_upd" = $5,
		"fech_upd" = $6
	
	WHERE
		"idrastreo_pais" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_pais_insert
(
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_pais" AS '
	INSERT INTO "rastreo_pais"
	(
		"pais",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreo_pais" WHERE "idrastreo_pais" = currval(''rastreo_pais_idrastreo_pais_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_pais_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_pais"
	WHERE
		"idrastreo_pais" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_persona_load_by_primarykey
(
 integer
)
RETURNS "rastreo_persona" AS '
	SELECT
		"idrastreo_persona" ,
		"id_distrito" ,
		"id_pais" ,
		"id_ciudad" ,
		"tipo_documento" ,
		"tipo_persona" ,
		"nro_documento" ,
		"razon_social" ,
		"nombre" ,
		"apellido" ,
		"direccion" ,
		"email" ,
		"tel_movil" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"tel_ofi" ,
		"tel_part" ,
		"fecha_nacimiento" ,
		"id_seguro" 
	FROM "rastreo_persona"
	WHERE
		 "idrastreo_persona" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_persona_load_all()
RETURNS SETOF "rastreo_persona" AS '
	SELECT
		"idrastreo_persona",
		"id_distrito",
		"id_pais",
		"id_ciudad",
		"tipo_documento",
		"tipo_persona",
		"nro_documento",
		"razon_social",
		"nombre",
		"apellido",
		"direccion",
		"email",
		"tel_movil",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"tel_ofi",
		"tel_part",
		"fecha_nacimiento",
		"id_seguro"
	FROM "rastreo_persona";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_persona_update
(
	integer,
	integer,
	integer,
	integer,
	integer,
	character,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	text,
	text,
	timestamp without time zone,
	integer
)
RETURNS void AS '
	UPDATE "rastreo_persona"
	SET
		"id_distrito" = $2,
		"id_pais" = $3,
		"id_ciudad" = $4,
		"tipo_documento" = $5,
		"tipo_persona" = $6,
		"nro_documento" = $7,
		"razon_social" = $8,
		"nombre" = $9,
		"apellido" = $10,
		"direccion" = $11,
		"email" = $12,
		"tel_movil" = $13,
		"user_ins" = $14,
		"fech_ins" = $15,
		"user_upd" = $16,
		"fech_upd" = $17,
		"tel_ofi" = $18,
		"tel_part" = $19,
		"fecha_nacimiento" = $20,
		"id_seguro" = $21
	
	WHERE
		"idrastreo_persona" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_persona_insert
(
	integer,
	integer,
	integer,
	integer,
	character,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	text,
	text,
	timestamp without time zone,
	integer
)
RETURNS "rastreo_persona" AS '
	INSERT INTO "rastreo_persona"
	(
		"id_distrito",
		"id_pais",
		"id_ciudad",
		"tipo_documento",
		"tipo_persona",
		"nro_documento",
		"razon_social",
		"nombre",
		"apellido",
		"direccion",
		"email",
		"tel_movil",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"tel_ofi",
		"tel_part",
		"fecha_nacimiento",
		"id_seguro"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16,
		$17,
		$18,
		$19,
		$20
	);
	SELECT * FROM "rastreo_persona" WHERE "idrastreo_persona" = currval(''rastreo_persona_idrastreo_persona_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_persona_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_persona"
	WHERE
		"idrastreo_persona" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_proviene_de_seguro_load_by_primarykey
(
 integer
)
RETURNS "rastreo_proviene_de_seguro" AS '
	SELECT
		"idrastreo_proviene_de_seguro" ,
		"descripcion_del_seguro" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_proviene_de_seguro"
	WHERE
		 "idrastreo_proviene_de_seguro" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_proviene_de_seguro_load_all()
RETURNS SETOF "rastreo_proviene_de_seguro" AS '
	SELECT
		"idrastreo_proviene_de_seguro",
		"descripcion_del_seguro",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_proviene_de_seguro";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_proviene_de_seguro_update
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_proviene_de_seguro"
	SET
		"descripcion_del_seguro" = $2,
		"user_ins" = $3,
		"fech_ins" = $4,
		"user_upd" = $5,
		"fech_upd" = $6
	
	WHERE
		"idrastreo_proviene_de_seguro" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_proviene_de_seguro_insert
(
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_proviene_de_seguro" AS '
	INSERT INTO "rastreo_proviene_de_seguro"
	(
		"descripcion_del_seguro",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreo_proviene_de_seguro" WHERE "idrastreo_proviene_de_seguro" = currval(''rastreo_proviene_de_seguro_idrastreo_proviene_de_seguro_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_proviene_de_seguro_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_proviene_de_seguro"
	WHERE
		"idrastreo_proviene_de_seguro" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_referencias_load_by_primarykey
(
 integer
)
RETURNS "rastreo_referencias" AS '
	SELECT
		"idrastreo_referencias" ,
		"id_cliente" ,
		"nombre" ,
		"lon" ,
		"lat" ,
		"descripcion" ,
		"visible" ,
		"user_in" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_referencias"
	WHERE
		 "idrastreo_referencias" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_referencias_load_all()
RETURNS SETOF "rastreo_referencias" AS '
	SELECT
		"idrastreo_referencias",
		"id_cliente",
		"nombre",
		"lon",
		"lat",
		"descripcion",
		"visible",
		"user_in",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_referencias";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_referencias_update
(
	integer,
	integer,
	character varying,
	double precision,
	double precision,
	integer,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_referencias"
	SET
		"id_cliente" = $2,
		"nombre" = $3,
		"lon" = $4,
		"lat" = $5,
		"descripcion" = $6,
		"visible" = $7,
		"user_in" = $8,
		"fech_ins" = $9,
		"user_upd" = $10,
		"fech_upd" = $11
	
	WHERE
		"idrastreo_referencias" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_referencias_insert
(
	integer,
	character varying,
	double precision,
	double precision,
	integer,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_referencias" AS '
	INSERT INTO "rastreo_referencias"
	(
		"id_cliente",
		"nombre",
		"lon",
		"lat",
		"descripcion",
		"visible",
		"user_in",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10
	);
	SELECT * FROM "rastreo_referencias" WHERE "idrastreo_referencias" = currval(''rastreo_referencias_idrastreo_referencias_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_referencias_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_referencias"
	WHERE
		"idrastreo_referencias" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_sucursales_load_by_primarykey
(
 integer
)
RETURNS "rastreo_sucursales" AS '
	SELECT
		"idrastreo_sucursal" ,
		"descripcion" 
	FROM "rastreo_sucursales"
	WHERE
		 "idrastreo_sucursal" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_sucursales_load_all()
RETURNS SETOF "rastreo_sucursales" AS '
	SELECT
		"idrastreo_sucursal",
		"descripcion"
	FROM "rastreo_sucursales";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_sucursales_update
(
	integer,
	text
)
RETURNS void AS '
	UPDATE "rastreo_sucursales"
	SET
		"descripcion" = $2
	
	WHERE
		"idrastreo_sucursal" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_sucursales_insert
(
	text
)
RETURNS "rastreo_sucursales" AS '
	INSERT INTO "rastreo_sucursales"
	(
		"descripcion"
	)
	VALUES
	(
		$1
	);
	SELECT * FROM "rastreo_sucursales" WHERE "idrastreo_sucursal" = currval(''rastreo_sucursales_idrastreo_sucursal_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_sucursales_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_sucursales"
	WHERE
		"idrastreo_sucursal" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_template_hoja_de_ruta_load_by_primarykey
(
 integer
)
RETURNS "rastreo_template_hoja_de_ruta" AS '
	SELECT
		"id_recorridotemplate" ,
		"id_persona" ,
		"id_usuarios" ,
		"descripcion_recorrido" ,
		"publico" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_template_hoja_de_ruta"
	WHERE
		 "id_recorridotemplate" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_template_hoja_de_ruta_load_all()
RETURNS SETOF "rastreo_template_hoja_de_ruta" AS '
	SELECT
		"id_recorridotemplate",
		"id_persona",
		"id_usuarios",
		"descripcion_recorrido",
		"publico",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_template_hoja_de_ruta";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_template_hoja_de_ruta_update
(
	integer,
	integer,
	integer,
	text,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_template_hoja_de_ruta"
	SET
		"id_persona" = $2,
		"id_usuarios" = $3,
		"descripcion_recorrido" = $4,
		"publico" = $5,
		"user_ins" = $6,
		"fech_ins" = $7,
		"user_upd" = $8,
		"fech_upd" = $9
	
	WHERE
		"id_recorridotemplate" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_template_hoja_de_ruta_insert
(
	integer,
	integer,
	text,
	boolean,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_template_hoja_de_ruta" AS '
	INSERT INTO "rastreo_template_hoja_de_ruta"
	(
		"id_persona",
		"id_usuarios",
		"descripcion_recorrido",
		"publico",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8
	);
	SELECT * FROM "rastreo_template_hoja_de_ruta" WHERE "id_recorridotemplate" = currval(''rastreo_template_hoja_de_ruta_id_recorridotemplate_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_template_hoja_de_ruta_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_template_hoja_de_ruta"
	WHERE
		"id_recorridotemplate" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_template_puntos_hoja_de_ruta_load_by_primarykey
(
 integer
)
RETURNS "rastreo_template_puntos_hoja_de_ruta" AS '
	SELECT
		"id_puntostemplate" ,
		"id_recorridotemplate" ,
		"nombre" ,
		"descripcion" ,
		"lon" ,
		"lat" ,
		"metros" ,
		"avisar_ingreso" ,
		"emails" 
	FROM "rastreo_template_puntos_hoja_de_ruta"
	WHERE
		 "id_puntostemplate" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_template_puntos_hoja_de_ruta_load_all()
RETURNS SETOF "rastreo_template_puntos_hoja_de_ruta" AS '
	SELECT
		"id_puntostemplate",
		"id_recorridotemplate",
		"nombre",
		"descripcion",
		"lon",
		"lat",
		"metros",
		"avisar_ingreso",
		"emails"
	FROM "rastreo_template_puntos_hoja_de_ruta";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_template_puntos_hoja_de_ruta_update
(
	integer,
	integer,
	character varying,
	text,
	double precision,
	double precision,
	integer,
	boolean,
	text
)
RETURNS void AS '
	UPDATE "rastreo_template_puntos_hoja_de_ruta"
	SET
		"id_recorridotemplate" = $2,
		"nombre" = $3,
		"descripcion" = $4,
		"lon" = $5,
		"lat" = $6,
		"metros" = $7,
		"avisar_ingreso" = $8,
		"emails" = $9
	
	WHERE
		"id_puntostemplate" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_template_puntos_hoja_de_ruta_insert
(
	integer,
	character varying,
	text,
	double precision,
	double precision,
	integer,
	boolean,
	text
)
RETURNS "rastreo_template_puntos_hoja_de_ruta" AS '
	INSERT INTO "rastreo_template_puntos_hoja_de_ruta"
	(
		"id_recorridotemplate",
		"nombre",
		"descripcion",
		"lon",
		"lat",
		"metros",
		"avisar_ingreso",
		"emails"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8
	);
	SELECT * FROM "rastreo_template_puntos_hoja_de_ruta" WHERE "id_puntostemplate" = currval(''rastreo_template_puntos_hoja_de_ruta_id_puntostemplate_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_template_puntos_hoja_de_ruta_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_template_puntos_hoja_de_ruta"
	WHERE
		"id_puntostemplate" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_tipo_documento_load_by_primarykey
(
 integer
)
RETURNS "rastreo_tipo_documento" AS '
	SELECT
		"idrastreo_tipo_documento" ,
		"descripcion" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_tipo_documento"
	WHERE
		 "idrastreo_tipo_documento" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_tipo_documento_load_all()
RETURNS SETOF "rastreo_tipo_documento" AS '
	SELECT
		"idrastreo_tipo_documento",
		"descripcion",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_tipo_documento";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_tipo_documento_update
(
	integer,
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_tipo_documento"
	SET
		"descripcion" = $2,
		"user_ins" = $3,
		"fech_ins" = $4,
		"user_upd" = $5,
		"fech_upd" = $6
	
	WHERE
		"idrastreo_tipo_documento" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_tipo_documento_insert
(
	character varying,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_tipo_documento" AS '
	INSERT INTO "rastreo_tipo_documento"
	(
		"descripcion",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5
	);
	SELECT * FROM "rastreo_tipo_documento" WHERE "idrastreo_tipo_documento" = currval(''rastreo_tipo_documento_idrastreo_tipo_documento_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_tipo_documento_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_tipo_documento"
	WHERE
		"idrastreo_tipo_documento" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_tipo_vehiculo_load_by_primarykey
(
 integer
)
RETURNS "rastreo_tipo_vehiculo" AS '
	SELECT
		"idrastreo_tipo_vehiculo" ,
		"tipo_de_vehiculo" ,
		"icono" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_tipo_vehiculo"
	WHERE
		 "idrastreo_tipo_vehiculo" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_tipo_vehiculo_load_all()
RETURNS SETOF "rastreo_tipo_vehiculo" AS '
	SELECT
		"idrastreo_tipo_vehiculo",
		"tipo_de_vehiculo",
		"icono",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_tipo_vehiculo";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_tipo_vehiculo_update
(
	integer,
	character varying,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_tipo_vehiculo"
	SET
		"tipo_de_vehiculo" = $2,
		"icono" = $3,
		"user_ins" = $4,
		"fech_ins" = $5,
		"user_upd" = $6,
		"fech_upd" = $7
	
	WHERE
		"idrastreo_tipo_vehiculo" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_tipo_vehiculo_insert
(
	character varying,
	text,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_tipo_vehiculo" AS '
	INSERT INTO "rastreo_tipo_vehiculo"
	(
		"tipo_de_vehiculo",
		"icono",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6
	);
	SELECT * FROM "rastreo_tipo_vehiculo" WHERE "idrastreo_tipo_vehiculo" = currval(''rastreo_tipo_vehiculo_idrastreo_tipo_vehiculo_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_tipo_vehiculo_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_tipo_vehiculo"
	WHERE
		"idrastreo_tipo_vehiculo" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_usuario_lista_vehiculo_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreo_usuario_lista_vehiculo" AS '
	SELECT
		"id_listavehiculos" ,
		"id_usuarios" ,
		"id_cliente" ,
		"id_vehiculo" ,
		"visible" 
	FROM "rastreo_usuario_lista_vehiculo"
	WHERE
		 "id_listavehiculos" = $1 AND
		 "id_usuarios" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_usuario_lista_vehiculo_load_all()
RETURNS SETOF "rastreo_usuario_lista_vehiculo" AS '
	SELECT
		"id_listavehiculos",
		"id_usuarios",
		"id_cliente",
		"id_vehiculo",
		"visible"
	FROM "rastreo_usuario_lista_vehiculo";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_usuario_lista_vehiculo_update
(
	integer,
	integer,
	integer,
	integer,
	boolean
)
RETURNS void AS '
	UPDATE "rastreo_usuario_lista_vehiculo"
	SET
		"id_cliente" = $3,
		"id_vehiculo" = $4,
		"visible" = $5
	
	WHERE
		"id_listavehiculos" = $1 AND 
		"id_usuarios" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_usuario_lista_vehiculo_insert
(
	integer,
	integer,
	integer,
	boolean
)
RETURNS "rastreo_usuario_lista_vehiculo" AS '
	INSERT INTO "rastreo_usuario_lista_vehiculo"
	(
		"id_usuarios",
		"id_cliente",
		"id_vehiculo",
		"visible"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4
	);
	SELECT * FROM "rastreo_usuario_lista_vehiculo" WHERE "id_listavehiculos" = currval(''rastreo_usuario_lista_vehiculo_id_listavehiculos_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_usuario_lista_vehiculo_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_usuario_lista_vehiculo"
	WHERE
		"id_listavehiculos" = $1 AND
		"id_usuarios" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_usuarios_load_by_primarykey
(
 integer
)
RETURNS "rastreo_usuarios" AS '
	SELECT
		"idrastreo_usuarios" ,
		"id_persona" ,
		"usuario" ,
		"pass" ,
		"su" ,
		"fech_login" ,
		"intentos_login" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"nombre_completo" 
	FROM "rastreo_usuarios"
	WHERE
		 "idrastreo_usuarios" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_usuarios_load_all()
RETURNS SETOF "rastreo_usuarios" AS '
	SELECT
		"idrastreo_usuarios",
		"id_persona",
		"usuario",
		"pass",
		"su",
		"fech_login",
		"intentos_login",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"nombre_completo"
	FROM "rastreo_usuarios";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_usuarios_update
(
	integer,
	integer,
	character varying,
	character varying,
	boolean,
	timestamp without time zone,
	integer,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	character varying
)
RETURNS void AS '
	UPDATE "rastreo_usuarios"
	SET
		"id_persona" = $2,
		"usuario" = $3,
		"pass" = $4,
		"su" = $5,
		"fech_login" = $6,
		"intentos_login" = $7,
		"user_ins" = $8,
		"fech_ins" = $9,
		"user_upd" = $10,
		"fech_upd" = $11,
		"nombre_completo" = $12
	
	WHERE
		"idrastreo_usuarios" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_usuarios_insert
(
	integer,
	character varying,
	character varying,
	boolean,
	timestamp without time zone,
	integer,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	character varying
)
RETURNS "rastreo_usuarios" AS '
	INSERT INTO "rastreo_usuarios"
	(
		"id_persona",
		"usuario",
		"pass",
		"su",
		"fech_login",
		"intentos_login",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"nombre_completo"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11
	);
	SELECT * FROM "rastreo_usuarios" WHERE "idrastreo_usuarios" = currval(''rastreo_usuarios_idrastreo_usuarios_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_usuarios_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_usuarios"
	WHERE
		"idrastreo_usuarios" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_load_by_primarykey
(
 integer,
 integer
)
RETURNS "rastreo_vehiculo" AS '
	SELECT
		"idrastreo_vehiculo" ,
		"id_cliente" ,
		"id_instalador" ,
		"proviene_de" ,
		"id_equipogps" ,
		"identificador_rastreo" ,
		"poliza_nroorden" ,
		"poliza_emision" ,
		"poliza_venc" ,
		"instalacion_fechahora" ,
		"desinstalacion_fechahora" ,
		"marca" ,
		"modelo" ,
		"tipo_vehiculo" ,
		"anho" ,
		"chasis" ,
		"color" ,
		"motor" ,
		"matricula" ,
		"kilometraje" ,
		"activo" ,
		"activo_fecha" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" ,
		"consumo_aprox" ,
		"chofer" ,
		"chofer_contacto" ,
		"observacion" ,
		"sucursal_instalacion" ,
		"demo",
		"sucursal",
		"id_vendedor",
		"nroordeninstal",
		"reinstalacion_fechahora" 
	FROM "rastreo_vehiculo"
	WHERE
		 "idrastreo_vehiculo" = $1 AND
		 "id_cliente" = $2;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_load_all()
RETURNS SETOF "rastreo_vehiculo" AS '
	SELECT
		"idrastreo_vehiculo",
		"id_cliente",
		"id_instalador",
		"proviene_de",
		"id_equipogps",
		"identificador_rastreo",
		"poliza_nroorden",
		"poliza_emision",
		"poliza_venc",
		"instalacion_fechahora",
		"desinstalacion_fechahora",
		"marca",
		"modelo",
		"tipo_vehiculo",
		"anho",
		"chasis",
		"color",
		"motor",
		"matricula",
		"kilometraje",
		"activo",
		"activo_fecha",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"consumo_aprox",
		"chofer",
		"chofer_contacto",
		"observacion",
		"sucursal_instalacion",
		"demo",
		"sucursal", 
		"id_vendedor",
		"nroordeninstal",
		"reinstalacion_fechahora"
	FROM "rastreo_vehiculo";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_update
(
	integer,
	integer,
	integer,
	integer,
	integer,
	character varying,
	character varying,
	timestamp without time zone,
	timestamp without time zone,
	timestamp without time zone,
	timestamp without time zone,
	integer,
	integer,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	integer,
	boolean,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	double precision,
	text,
	text,
	text,
	integer,
	boolean,
	character varying,
	integer,
	integer,
	timestamp without time zone,
	
)
RETURNS void AS '
	UPDATE "rastreo_vehiculo"
	SET
		"id_instalador" = $3,
		"proviene_de" = $4,
		"id_equipogps" = $5,
		"identificador_rastreo" = $6,
		"poliza_nroorden" = $7,
		"poliza_emision" = $8,
		"poliza_venc" = $9,
		"instalacion_fechahora" = $10,
		"desinstalacion_fechahora" = $11,
		"marca" = $12,
		"modelo" = $13,
		"tipo_vehiculo" = $14,
		"anho" = $15,
		"chasis" = $16,
		"color" = $17,
		"motor" = $18,
		"matricula" = $19,
		"kilometraje" = $20,
		"activo" = $21,
		"activo_fecha" = $22,
		"user_ins" = $23,
		"fech_ins" = $24,
		"user_upd" = $25,
		"fech_upd" = $26,
		"consumo_aprox" = $27,
		"chofer" = $28,
		"chofer_contacto" = $29,
		"observacion" = $30,
		"sucursal_instalacion" = $31,
		"demo" = $32,
		"sucursal" = $33,
		"id_vendedor" = $34,
		"nroordeninstal" = $35,
		"reinstalacion_fechahora" = $36
	
	WHERE
		"idrastreo_vehiculo" = $1 AND 
		"id_cliente" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_insert
(
	integer,
	integer,
	integer,
	integer,
	character varying,
	character varying,
	timestamp without time zone,
	timestamp without time zone,
	timestamp without time zone,
	timestamp without time zone,
	integer,
	integer,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	integer,
	boolean,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone,
	double precision,
	text,
	text,
	text,
	integer,
	boolean,
	character varying,
	integer,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_vehiculo" AS '
	INSERT INTO "rastreo_vehiculo"
	(
		"id_cliente",
		"id_instalador",
		"proviene_de",
		"id_equipogps",
		"identificador_rastreo",
		"poliza_nroorden",
		"poliza_emision",
		"poliza_venc",
		"instalacion_fechahora",
		"desinstalacion_fechahora",
		"marca",
		"modelo",
		"tipo_vehiculo",
		"anho",
		"chasis",
		"color",
		"motor",
		"matricula",
		"kilometraje",
		"activo",
		"activo_fecha",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd",
		"consumo_aprox",
		"chofer",
		"chofer_contacto",
		"observacion",
		"sucursal_instalacion",
		"demo",
		"sucursal",  
		"id_vendedor",
		"nroordeninstal",
		"reinstalacion_fechahora"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16,
		$17,
		$18,
		$19,
		$20,
		$21,
		$22,
		$23,
		$24,
		$25,
		$26,
		$27,
		$28,
		$29,
		$30,
		$31,
		$32,
		$33,
		$34,
		$35
	);
	SELECT * FROM "rastreo_vehiculo" WHERE "idrastreo_vehiculo" = currval(''rastreo_vehiculo_idrastreo_vehiculo_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_delete
(
	integer,
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_vehiculo"
	WHERE
		"idrastreo_vehiculo" = $1 AND
		"id_cliente" = $2;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_fotos_load_by_primarykey
(
 integer
)
RETURNS "rastreo_vehiculo_fotos" AS '
	SELECT
		"idrastreo_vehiculo_fotos" ,
		"idvehiculo" ,
		"idcliente" ,
		"descripcion_foto" ,
		"nombre_archivo" ,
		"tipo_archivo" ,
		"foto" ,
		"user_ins" ,
		"fech_ins" ,
		"user_upd" ,
		"fech_upd" 
	FROM "rastreo_vehiculo_fotos"
	WHERE
		 "idrastreo_vehiculo_fotos" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_fotos_load_all()
RETURNS SETOF "rastreo_vehiculo_fotos" AS '
	SELECT
		"idrastreo_vehiculo_fotos",
		"idvehiculo",
		"idcliente",
		"descripcion_foto",
		"nombre_archivo",
		"tipo_archivo",
		"foto",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	FROM "rastreo_vehiculo_fotos";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_fotos_update
(
	integer,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	bytea,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS void AS '
	UPDATE "rastreo_vehiculo_fotos"
	SET
		"idvehiculo" = $2,
		"idcliente" = $3,
		"descripcion_foto" = $4,
		"nombre_archivo" = $5,
		"tipo_archivo" = $6,
		"foto" = $7,
		"user_ins" = $8,
		"fech_ins" = $9,
		"user_upd" = $10,
		"fech_upd" = $11
	
	WHERE
		"idrastreo_vehiculo_fotos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_fotos_insert
(
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	bytea,
	integer,
	timestamp without time zone,
	integer,
	timestamp without time zone
)
RETURNS "rastreo_vehiculo_fotos" AS '
	INSERT INTO "rastreo_vehiculo_fotos"
	(
		"idvehiculo",
		"idcliente",
		"descripcion_foto",
		"nombre_archivo",
		"tipo_archivo",
		"foto",
		"user_ins",
		"fech_ins",
		"user_upd",
		"fech_upd"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10
	);
	SELECT * FROM "rastreo_vehiculo_fotos" WHERE "idrastreo_vehiculo_fotos" = currval(''rastreo_vehiculo_fotos_idrastreo_vehiculo_fotos_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION rastreo_vehiculo_fotos_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "rastreo_vehiculo_fotos"
	WHERE
		"idrastreo_vehiculo_fotos" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps102006_load_by_primarykey
(
 integer
)
RETURNS "reportesgps102006" AS '
	SELECT
		"idreportesgps102006" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps102006"
	WHERE
		 "idreportesgps102006" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps102006_load_all()
RETURNS SETOF "reportesgps102006" AS '
	SELECT
		"idreportesgps102006",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps102006";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps102006_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps102006"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps102006" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps102006_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps102006" AS '
	INSERT INTO "reportesgps102006"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps102006" WHERE "idreportesgps102006" = currval(''reportesgps102006_idreportesgps102006_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps102006_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps102006"
	WHERE
		"idreportesgps102006" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps102009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps102009" AS '
	SELECT
		"idreportesgps102009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps102009"
	WHERE
		 "idreportesgps102009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps102009_load_all()
RETURNS SETOF "reportesgps102009" AS '
	SELECT
		"idreportesgps102009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps102009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps102009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps102009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps102009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps102009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps102009" AS '
	INSERT INTO "reportesgps102009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps102009" WHERE "idreportesgps102009" = currval(''reportesgps102009_idreportesgps102009_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps102009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps102009"
	WHERE
		"idreportesgps102009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps112000_load_by_primarykey
(
 integer
)
RETURNS "reportesgps112000" AS '
	SELECT
		"idreportesgps112000" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps112000"
	WHERE
		 "idreportesgps112000" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps112000_load_all()
RETURNS SETOF "reportesgps112000" AS '
	SELECT
		"idreportesgps112000",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps112000";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps112000_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps112000"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps112000" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps112000_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps112000" AS '
	INSERT INTO "reportesgps112000"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps112000" WHERE "idreportesgps112000" = currval(''reportesgps112000_idreportesgps112000_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps112000_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps112000"
	WHERE
		"idreportesgps112000" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps112009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps112009" AS '
	SELECT
		"idreportesgps112009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps112009"
	WHERE
		 "idreportesgps112009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps112009_load_all()
RETURNS SETOF "reportesgps112009" AS '
	SELECT
		"idreportesgps112009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps112009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps112009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps112009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps112009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps112009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps112009" AS '
	INSERT INTO "reportesgps112009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps112009" WHERE "idreportesgps112009" = currval(''reportesgps112009_idreportesgps112009_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps112009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps112009"
	WHERE
		"idreportesgps112009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps121969_load_by_primarykey
(
 integer
)
RETURNS "reportesgps121969" AS '
	SELECT
		"idreportesgps121969" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps121969"
	WHERE
		 "idreportesgps121969" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps121969_load_all()
RETURNS SETOF "reportesgps121969" AS '
	SELECT
		"idreportesgps121969",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps121969";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps121969_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps121969"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps121969" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps121969_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps121969" AS '
	INSERT INTO "reportesgps121969"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps121969" WHERE "idreportesgps121969" = currval(''reportesgps121969_idreportesgps121969_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps121969_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps121969"
	WHERE
		"idreportesgps121969" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps32009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps32009" AS '
	SELECT
		"idreportesgps32009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps32009"
	WHERE
		 "idreportesgps32009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps32009_load_all()
RETURNS SETOF "reportesgps32009" AS '
	SELECT
		"idreportesgps32009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps32009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps32009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps32009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps32009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps32009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps32009" AS '
	INSERT INTO "reportesgps32009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps32009" WHERE "idreportesgps32009" = currval(''reportesgps32009_idreportesgps32009_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps32009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps32009"
	WHERE
		"idreportesgps32009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps42009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps42009" AS '
	SELECT
		"idreportesgps42009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps42009"
	WHERE
		 "idreportesgps42009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps42009_load_all()
RETURNS SETOF "reportesgps42009" AS '
	SELECT
		"idreportesgps42009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps42009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps42009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps42009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps42009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps42009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps42009" AS '
	INSERT INTO "reportesgps42009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps42009" WHERE "idreportesgps42009" = currval(''reportesgps42009_idreportesgps42009_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps42009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps42009"
	WHERE
		"idreportesgps42009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps52009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps52009" AS '
	SELECT
		"idreportesgps52009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps52009"
	WHERE
		 "idreportesgps52009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps52009_load_all()
RETURNS SETOF "reportesgps52009" AS '
	SELECT
		"idreportesgps52009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps52009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps52009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps52009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps52009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps52009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps52009" AS '
	INSERT INTO "reportesgps52009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps52009" WHERE "idreportesgps52009" = currval(''reportesgps52009_idreportesgps52009_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps52009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps52009"
	WHERE
		"idreportesgps52009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps62009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps62009" AS '
	SELECT
		"idreportesgps62009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps62009"
	WHERE
		 "idreportesgps62009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps62009_load_all()
RETURNS SETOF "reportesgps62009" AS '
	SELECT
		"idreportesgps62009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps62009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps62009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps62009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps62009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps62009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps62009" AS '
	INSERT INTO "reportesgps62009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps62009" WHERE "idreportesgps62009" = currval(''reportesgps62009_idreportesgps62009_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps62009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps62009"
	WHERE
		"idreportesgps62009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps72009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps72009" AS '
	SELECT
		"id_equipogps" ,
		"gps_fechahora_reporte" ,
		"gps_latitud" ,
		"gps_longitud" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_edaddeldato" ,
		"gps_estado_io" ,
		"gps_evento" ,
		"gps_tipodeposicion" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_ip" ,
		"idrastreogps_reportes" ,
		"gps_dir" 
	FROM "reportesgps72009"
	WHERE
		 "idrastreogps_reportes" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps72009_load_all()
RETURNS SETOF "reportesgps72009" AS '
	SELECT
		"id_equipogps",
		"gps_fechahora_reporte",
		"gps_latitud",
		"gps_longitud",
		"gps_velocidad",
		"gps_rumbo",
		"gps_edaddeldato",
		"gps_estado_io",
		"gps_evento",
		"gps_tipodeposicion",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_ip",
		"idrastreogps_reportes",
		"gps_dir"
	FROM "reportesgps72009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps72009_update
(
	integer,
	timestamp without time zone,
	double precision,
	double precision,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	integer,
	text
)
RETURNS void AS '
	UPDATE "reportesgps72009"
	SET
		"id_equipogps" = $1,
		"gps_fechahora_reporte" = $2,
		"gps_latitud" = $3,
		"gps_longitud" = $4,
		"gps_velocidad" = $5,
		"gps_rumbo" = $6,
		"gps_edaddeldato" = $7,
		"gps_estado_io" = $8,
		"gps_evento" = $9,
		"gps_tipodeposicion" = $10,
		"gps_hdop" = $11,
		"gps_satstatus" = $12,
		"gps_gsmstatus" = $13,
		"gps_ip" = $14,
		"gps_dir" = $16
	
	WHERE
		"idrastreogps_reportes" = $15;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps72009_insert
(
	integer,
	timestamp without time zone,
	double precision,
	double precision,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text
)
RETURNS "reportesgps72009" AS '
	INSERT INTO "reportesgps72009"
	(
		"id_equipogps",
		"gps_fechahora_reporte",
		"gps_latitud",
		"gps_longitud",
		"gps_velocidad",
		"gps_rumbo",
		"gps_edaddeldato",
		"gps_estado_io",
		"gps_evento",
		"gps_tipodeposicion",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_ip",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15
	);
	SELECT * FROM "reportesgps72009" WHERE "idrastreogps_reportes" = currval(''reportesgps_07_2009_idrastreogps_reportes_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps72009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps72009"
	WHERE
		"idrastreogps_reportes" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps82009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps82009" AS '
	SELECT
		"idreportesgps82009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps82009"
	WHERE
		 "idreportesgps82009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps82009_load_all()
RETURNS SETOF "reportesgps82009" AS '
	SELECT
		"idreportesgps82009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps82009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps82009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps82009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps82009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps82009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps82009" AS '
	INSERT INTO "reportesgps82009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps82009" WHERE "idreportesgps82009" = currval(''rastreogps_reportes_idrastreogps_reportes_seq''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps82009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps82009"
	WHERE
		"idreportesgps82009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps92009_load_by_primarykey
(
 integer
)
RETURNS "reportesgps92009" AS '
	SELECT
		"idreportesgps92009" ,
		"id_equipogps" ,
		"gps_longitud" ,
		"gps_latitud" ,
		"gps_fechahora_reporte" ,
		"gps_velocidad" ,
		"gps_rumbo" ,
		"gps_evento" ,
		"gps_edaddeldato" ,
		"gps_hdop" ,
		"gps_satstatus" ,
		"gps_gsmstatus" ,
		"gps_estado_io" ,
		"gps_tipodeposicion" ,
		"gps_ip" ,
		"gps_obs" ,
		"gps_dir" 
	FROM "reportesgps92009"
	WHERE
		 "idreportesgps92009" = $1;
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps92009_load_all()
RETURNS SETOF "reportesgps92009" AS '
	SELECT
		"idreportesgps92009",
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	FROM "reportesgps92009";
' LANGUAGE 'SQL' STABLE;


CREATE OR REPLACE FUNCTION reportesgps92009_update
(
	integer,
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS void AS '
	UPDATE "reportesgps92009"
	SET
		"id_equipogps" = $2,
		"gps_longitud" = $3,
		"gps_latitud" = $4,
		"gps_fechahora_reporte" = $5,
		"gps_velocidad" = $6,
		"gps_rumbo" = $7,
		"gps_evento" = $8,
		"gps_edaddeldato" = $9,
		"gps_hdop" = $10,
		"gps_satstatus" = $11,
		"gps_gsmstatus" = $12,
		"gps_estado_io" = $13,
		"gps_tipodeposicion" = $14,
		"gps_ip" = $15,
		"gps_obs" = $16,
		"gps_dir" = $17
	
	WHERE
		"idreportesgps92009" = $1;
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps92009_insert
(
	integer,
	double precision,
	double precision,
	timestamp without time zone,
	integer,
	integer,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	character varying,
	text,
	text
)
RETURNS "reportesgps92009" AS '
	INSERT INTO "reportesgps92009"
	(
		"id_equipogps",
		"gps_longitud",
		"gps_latitud",
		"gps_fechahora_reporte",
		"gps_velocidad",
		"gps_rumbo",
		"gps_evento",
		"gps_edaddeldato",
		"gps_hdop",
		"gps_satstatus",
		"gps_gsmstatus",
		"gps_estado_io",
		"gps_tipodeposicion",
		"gps_ip",
		"gps_obs",
		"gps_dir"
	)
	VALUES
	(
		$1,
		$2,
		$3,
		$4,
		$5,
		$6,
		$7,
		$8,
		$9,
		$10,
		$11,
		$12,
		$13,
		$14,
		$15,
		$16
	);
	SELECT * FROM "reportesgps92009" WHERE "idreportesgps92009" = currval(''reportesgps92009_idreportesgps92009_seq1''::regclass);
' LANGUAGE 'SQL' VOLATILE;


CREATE OR REPLACE FUNCTION reportesgps92009_delete
(
	integer
)
RETURNS void AS '
	DELETE FROM "reportesgps92009"
	WHERE
		"idreportesgps92009" = $1;
' LANGUAGE 'SQL' VOLATILE;



