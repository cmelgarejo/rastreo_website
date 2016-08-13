------------------------------------------------------------------------------------------------------------------------------------
------------------------------------- Vista: vehiculos asignados a los usuarios ----------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_vehiculos_asignados_a_usuarios;
CREATE OR REPLACE VIEW rsview_vehiculos_asignados_a_usuarios AS
SELECT
	rastreo_vehiculo.identificador_rastreo,
	rastreo_usuario_lista_vehiculo.id_usuarios, rastreo_usuario_lista_vehiculo.id_cliente, 
	rastreo_usuario_lista_vehiculo.id_vehiculo, rastreo_usuario_lista_vehiculo.visible,
	rastreo_usuarios.usuario,
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente, 
	rastreo_vehiculo.anho, rastreo_vehiculo.chasis, rastreo_vehiculo.color, rastreo_vehiculo.matricula, 
	rastreo_vehiculo.kilometraje, rastreo_vehiculo.activo, rastreo_marca_vehiculo.marca, 
	rastreo_modelo_vehiculo.modelo, 
	rastreogps_equipogps.idrastreogps_equipogps AS idequipogps
FROM 
	rastreo_usuario_lista_vehiculo
LEFT JOIN rastreo_persona  ON rastreo_persona.idrastreo_persona = rastreo_usuario_lista_vehiculo.id_cliente
LEFT JOIN rastreo_vehiculo ON rastreo_vehiculo.idrastreo_vehiculo = rastreo_usuario_lista_vehiculo.id_vehiculo
LEFT JOIN rastreo_usuarios ON rastreo_usuarios.idrastreo_usuarios = rastreo_usuario_lista_vehiculo.id_usuarios
LEFT JOIN rastreo_marca_vehiculo ON rastreo_marca_vehiculo.idrastreo_marca_vehiculo = rastreo_vehiculo.marca
LEFT JOIN rastreo_modelo_vehiculo ON rastreo_modelo_vehiculo.idrastreo_modelo_vehiculo = rastreo_vehiculo.modelo
LEFT JOIN rastreogps_equipogps ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps
ORDER BY rastreo_vehiculo.identificador_rastreo;
ALTER TABLE rsview_vehiculos_asignados_a_usuarios OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------- Vista: equipo gps y su lista de comandos, enviados y no enviados -----------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_equipogps_comandos;
CREATE OR REPLACE VIEW rsview_equipogps_comandos AS
	SELECT DISTINCT
		rastreo_vehiculo.identificador_rastreo,
		rastreogps_cola_de_comandos.idrastreogps_cola_de_comandos,
		rastreogps_equipogps.idrastreogps_equipogps, rastreogps_equipogps.id_equipo_gps, rastreogps_equipogps.id_simcard, 
		rastreogps_equipogps.tipoequipo, rastreogps_tipoequipo.tipo_equipo, rastreogps_simcards.sim_nro,
		rastreogps_cola_de_comandos.comando, rastreogps_cola_de_comandos.descripcion, 
		rastreogps_cola_de_comandos.enviado, rastreogps_bandeja_de_entrada.gps_ip, rastreogps_cola_de_comandos.user_ins,
		rastreo_usuarios.usuario, rastreogps_cola_de_comandos.fech_ins
	FROM rastreogps_bandeja_de_entrada
	LEFT JOIN rastreogps_cola_de_comandos      ON rastreogps_bandeja_de_entrada.id_equipogps = rastreogps_cola_de_comandos.idequipogps
	LEFT JOIN rastreo_usuarios      ON rastreogps_cola_de_comandos.user_ins = rastreo_usuarios.idrastreo_usuarios
	LEFT JOIN rastreogps_equipogps  ON rastreogps_cola_de_comandos.idequipogps = rastreogps_equipogps.idrastreogps_equipogps
	LEFT JOIN rastreogps_simcards   ON rastreogps_equipogps.id_simcard         = rastreogps_simcards.idrastreogps_simcards
	LEFT JOIN rastreogps_tipoequipo ON rastreogps_equipogps.tipoequipo         = rastreogps_tipoequipo.idrastreogps_tipoequipo
	LEFT JOIN rastreo_vehiculo      ON rastreogps_equipogps.idrastreogps_equipogps = rastreo_vehiculo.id_equipogps;	
ALTER TABLE rsview_equipogps_comandos OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------- Vista: Evento, equipogps, vehiculo y cliente que esta enviando evento  -----------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW rsview_avisos_equipo_vehiculo_cliente;
CREATE OR REPLACE VIEW rsview_avisos_equipo_vehiculo_cliente AS 
 SELECT rastreogps_equipo_eventos.habilitado, rastreogps_tipoevento.descripcion, rastreogps_tipoevento.idrastreogps_tipo_evento, 
 rastreogps_avisos.idrastreogps_avisos, rastreogps_avisos.id_equipogps, rastreogps_avisos.evento, rastreogps_avisos.evento_fechahora, 
 rastreogps_avisos.atendido, rastreogps_tipoevento.color AS color_evento, rastreogps_equipo_eventos.sonoro, 
 rastreogps_equipo_eventos.arch_sonido, rastreogps_tipoevento.sonoro AS sonoro_tipoevento, 
 rastreogps_tipoevento.arch_sonido AS soundfile_sonoro_tipoevento, rastreo_persona.idrastreo_persona AS id_persona, 
 rastreo_vehiculo.idrastreo_vehiculo AS id_vehiculo, rastreogps_equipogps.id_equipo_gps, rastreo_vehiculo.identificador_rastreo,
        rastreogps_avisos.gps_fechahora_reporte, rastreogps_avisos.gps_latitud, rastreogps_avisos.gps_longitud, 
        rastreogps_avisos.gps_velocidad, rastreogps_avisos.gps_rumbo, rastreogps_avisos.gps_dir, 
        CASE
            WHEN rastreogps_avisos.gps_rumbo >= 0  AND rastreogps_avisos.gps_rumbo <= 23  OR rastreogps_avisos.gps_rumbo >= 345 AND rastreogps_avisos.gps_rumbo <= 360 THEN 'NORTE'::text
            WHEN rastreogps_avisos.gps_rumbo > 23  AND rastreogps_avisos.gps_rumbo <= 69  THEN 'NORESTE'::text
            WHEN rastreogps_avisos.gps_rumbo > 69  AND rastreogps_avisos.gps_rumbo <= 115 THEN 'ESTE'::text
            WHEN rastreogps_avisos.gps_rumbo > 115 AND rastreogps_avisos.gps_rumbo <= 161 THEN 'SURESTE'::text
            WHEN rastreogps_avisos.gps_rumbo > 161 AND rastreogps_avisos.gps_rumbo <= 207 THEN 'SUR'::text
            WHEN rastreogps_avisos.gps_rumbo > 207 AND rastreogps_avisos.gps_rumbo <= 253 THEN 'SUROESTE'::text
            WHEN rastreogps_avisos.gps_rumbo > 253 AND rastreogps_avisos.gps_rumbo <= 299 THEN 'OESTE'::text
            WHEN rastreogps_avisos.gps_rumbo > 299 AND rastreogps_avisos.gps_rumbo <= 345 THEN 'NOROESTE'::text
            ELSE NULL::text
        END AS rumbo, 
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente, rastreo_persona.tel_movil, rastreo_persona.tel_part, rastreo_persona.tel_ofi, rastreo_persona.email, rastreo_cliente.estado_cliente, rastreo_cliente.clave_personal, rastreogps_equipo_eventos.email AS sendmail
   FROM rastreogps_avisos
   LEFT JOIN rastreogps_equipogps ON rastreogps_equipogps.idrastreogps_equipogps = rastreogps_avisos.id_equipogps
   LEFT JOIN rastreo_vehiculo ON rastreo_vehiculo.id_equipogps = rastreogps_avisos.id_equipogps
   LEFT JOIN rastreo_persona ON rastreo_persona.idrastreo_persona = rastreo_vehiculo.id_cliente
   LEFT JOIN rastreo_cliente ON rastreo_cliente.id_cliente = rastreo_vehiculo.id_cliente
   LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_avisos.id_equipogps AND btrim(rastreogps_avisos.evento) = btrim(rastreogps_equipo_eventos.evento)
   LEFT JOIN rastreogps_tipoevento ON rastreogps_tipoevento.idrastreogps_tipo_evento = rastreogps_equipo_eventos.id_tipoevento
   LEFT JOIN rastreogps_bandeja_de_entrada ON rastreogps_bandeja_de_entrada.id_equipogps = rastreogps_avisos.id_equipogps
  WHERE (rastreogps_equipo_eventos.habilitado = true OR rastreogps_tipoevento.descripcion IS NULL) AND rastreogps_avisos.atendido = false
  ORDER BY rastreo_vehiculo.identificador_rastreo;
ALTER TABLE rsview_avisos_equipo_vehiculo_cliente OWNER TO rastreo_admin;


------------------------------------------------------------------------------------------------------------------------------------
------------------------------ Vista: Cliente y sus respectivos usuarios -----------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_cliente_usuarios;
CREATE OR REPLACE VIEW rsview_cliente_usuarios AS 
 SELECT CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente,
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN 'Fisica'
            ELSE 'Juridica'
        END AS tipo_persona,
        usuarios.*        
   FROM rastreo_persona
   LEFT JOIN rastreo_usuarios AS usuarios ON rastreo_persona.idrastreo_persona = usuarios.id_persona;
ALTER TABLE rsview_cliente_usuarios OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------ Vista: Recorrido -------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--CREATE OR REPLACE VIEW rsview_vehiculo_recorrido_puntos AS 
-- SELECT reco.idrastreo_vehiculo_recorrido as id_recorrido, reco.id_cliente, reco.id_vehiculo, reco.recorrido_activo, 
--	reco.descripcion_recorrido, reco.hora_inicio, reco.hora_fin, recoptos.id_recorrido_puntos, recoptos.r_nombre, 
--	recoptos.r_descripcion, recoptos.r_orden, recoptos.r_lon, recoptos.r_lat, recoptos.r_hora_llegada, 
--	recoptos.r_minutos_demora
--   FROM rastreo_vehiculo_recorrido_puntos AS recoptos
--   LEFT JOIN rastreo_vehiculo_recorrido AS reco ON 
--   reco.idrastreo_vehiculo_recorrido = recoptos.id_recorrido;
--ALTER TABLE rsview_vehiculo_recorrido_puntos OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------ Vista: Usuario, opcion de pantalla y la descripcion de la misma -------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_datos_personas;
CREATE OR REPLACE VIEW rsview_datos_personas AS 
 SELECT 
	count(rastreo_vehiculo.idrastreo_vehiculo) as cantidad_de_vehiculos,
	rastreo_persona.idrastreo_persona,
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente,
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN 'Fisica'
            ELSE 'Juridica'
        END AS tipo_persona,
        rastreo_persona.id_pais, rastreo_pais.pais, rastreo_persona.id_distrito, rastreo_distrito.distrito, rastreo_persona.id_ciudad, 
        rastreo_ciudad.ciudad, rastreo_persona.fecha_nacimiento, rastreo_persona.tipo_documento, rastreo_persona.nro_documento, rastreo_persona.direccion, 
        rastreo_persona.email, rastreo_persona.tel_movil, rastreo_persona.tel_part, rastreo_persona.tel_ofi, rastreo_persona.user_ins, rastreo_persona.fech_ins, 
        rastreo_persona.user_upd, rastreo_persona.fech_upd              
   FROM rastreo_persona
   LEFT JOIN rastreo_pais ON rastreo_persona.id_pais = rastreo_pais.idrastreo_pais
   LEFT JOIN rastreo_distrito ON rastreo_persona.id_distrito = rastreo_distrito.idrastreo_distrito
   LEFT JOIN rastreo_ciudad ON rastreo_persona.id_ciudad = rastreo_ciudad.idrastreo_ciudad
   LEFT JOIN rastreo_vehiculo ON rastreo_persona.idrastreo_persona = rastreo_vehiculo.id_cliente
   GROUP BY rastreo_persona.idrastreo_persona,rastreo_persona.tipo_persona,rastreo_persona.nombre,rastreo_persona.apellido,
   rastreo_persona.razon_social,rastreo_persona.id_pais,rastreo_pais.pais,rastreo_persona.id_distrito,rastreo_distrito.distrito,
   rastreo_persona.id_ciudad,rastreo_ciudad.ciudad,rastreo_persona.fecha_nacimiento,rastreo_persona.tipo_documento,rastreo_persona.nro_documento,
   rastreo_persona.direccion,rastreo_persona.email,rastreo_persona.tel_movil,rastreo_persona.tel_part,rastreo_persona.tel_ofi,rastreo_persona.user_ins,
   rastreo_persona.fech_ins,rastreo_persona.user_upd,rastreo_persona.fech_upd
   ORDER BY cliente;
   ALTER TABLE rsview_datos_personas OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------ Vista: Usuario, opcion de pantalla y la descripcion de la misma -------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_opcionespantalla_usuario;
CREATE OR REPLACE VIEW rsview_opcionespantalla_usuario AS 
 SELECT rastreogps_usuario_opciones.opcion_de_pantalla, rastreogps_usuario_opciones.id_usuario,
	rastreogps_opciones_de_pantalla.opcion_de_pantalla as descripcion
   FROM rastreogps_usuario_opciones
   LEFT JOIN rastreogps_opciones_de_pantalla ON rastreogps_opciones_de_pantalla.idrastreogps_opciones_de_pantalla = rastreogps_usuario_opciones.opcion_de_pantalla;
ALTER TABLE rsview_opcionespantalla_usuario OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------ Vista: Para combobox equiposgps en carga de vehiculos -----------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rs_select_vehiculo_tipoequipo_equipogps;
CREATE OR REPLACE VIEW rs_select_vehiculo_tipoequipo_equipogps AS 
 SELECT 
	idrastreogps_equipogps,
	CASE 
		WHEN rastreo_vehiculo.idrastreo_vehiculo IS NOT NULL THEN
			rastreo_vehiculo.identificador_rastreo
		WHEN rastreo_vehiculo.idrastreo_vehiculo IS NULL THEN
			'NO INSTALADO'
	END || ' - ' ||
	rastreogps_tipoequipo.tipo_equipo || ' - ' ||
	rastreogps_equipogps.id_equipo_gps || ' - ' ||
	CASE 
		WHEN rastreogps_equipogps.id_simcard IS NOT NULL THEN
			rastreogps_simcards.sim_nro
		WHEN rastreogps_equipogps.id_simcard IS NULL THEN
			'NO SIM'
	END
	AS descripcion
   FROM rastreogps_equipogps
   LEFT JOIN rastreogps_tipoequipo ON rastreogps_tipoequipo.idrastreogps_tipoequipo = rastreogps_equipogps.tipoequipo
   LEFT JOIN rastreogps_simcards   ON rastreogps_simcards.idrastreogps_simcards = rastreogps_equipogps.id_simcard
   LEFT JOIN rastreo_vehiculo      ON rastreogps_equipogps.idrastreogps_equipogps = rastreo_vehiculo.id_equipogps
ORDER BY descripcion;
ALTER TABLE rs_select_vehiculo_tipoequipo_equipogps OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------ Vista: Para combobox equiposgps en carga de vehiculos -----------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rs_select_equipogps;
CREATE OR REPLACE VIEW rs_select_equipogps AS 
 SELECT 
	idrastreogps_equipogps,
	rastreogps_tipoequipo.tipo_equipo || ' - ' ||
	rastreogps_equipogps.id_equipo_gps || ' - ' ||
	CASE 
		WHEN rastreogps_equipogps.id_simcard IS NOT NULL THEN
			rastreogps_simcards.sim_nro
		WHEN rastreogps_equipogps.id_simcard IS NULL THEN
			'NO SIM'
	END
	AS ddlequipogps_desc
   FROM rastreogps_equipogps
   LEFT JOIN rastreogps_tipoequipo ON rastreogps_tipoequipo.idrastreogps_tipoequipo = rastreogps_equipogps.tipoequipo
   LEFT JOIN rastreogps_simcards ON rastreogps_simcards.idrastreogps_simcards = rastreogps_equipogps.id_simcard;
ALTER TABLE rs_select_equipogps OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------- Vista: Cliente, vehiculo, equipo, tipo equipo y sim ------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_cliente_vehiculo_equipogps;
CREATE OR REPLACE VIEW rsview_cliente_vehiculo_equipogps AS
SELECT DISTINCT --on (rastreo_vehiculo.identificador_rastreo)
	rastreo_vehiculo.identificador_rastreo,
	rastreo_vehiculo.idrastreo_vehiculo,
	rastreo_vehiculo.proviene_de,
	rastreo_persona.idrastreo_persona,
	rastreo_usuarios.idrastreo_usuarios,
	rastreo_usuario_lista_vehiculo.id_usuarios, rastreo_usuario_lista_vehiculo.id_cliente, rastreo_usuario_lista_vehiculo.id_vehiculo, 
	rastreo_usuario_lista_vehiculo.visible, 
	rastreo_usuarios.usuario,
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente, 
        rastreo_cliente.estado_cliente, rastreo_instaladores.idrastreo_persona as id_instalador,
	CASE
		WHEN rastreo_instaladores.tipo_persona = 'F'::bpchar THEN ((rastreo_instaladores.nombre::text || ' '::text) || rastreo_instaladores.apellido::text)::character varying
		ELSE rastreo_instaladores.razon_social
	END AS instalador,
         rastreo_vehiculo.anho, rastreo_vehiculo.poliza_nroorden,
         rastreo_vehiculo.chasis, rastreo_vehiculo.color, rastreo_vehiculo.matricula, rastreo_vehiculo.kilometraje, 
         rastreo_vehiculo.activo, rastreo_vehiculo.consumo_aprox, rastreo_vehiculo.instalacion_fechahora,
         rastreo_vehiculo.marca AS idmarca, rastreo_vehiculo.modelo AS idmodelo, rastreo_marca_vehiculo.marca, 
         rastreo_modelo_vehiculo.modelo, rastreogps_equipogps.idrastreogps_equipogps AS idequipogps, 
         rastreogps_tipoequipo.idrastreogps_tipoequipo as id_tipoequipogps, rastreogps_tipoequipo.tipo_equipo as tipoequipogps, 
         rastreogps_tipoequipo.tipo_de_reporte as tipo_de_reportegps, rastreogps_equipogps.id_equipo_gps, 
         rastreogps_equipogps.nro_serie_gps, rastreogps_simcards.idrastreogps_simcards AS idsimcard, rastreogps_simcards.sim_nro,
         CASE
            WHEN aud_insert.tipo_persona = 'F'::bpchar THEN ((aud_insert.nombre::text || ' '::text) || aud_insert.apellido::text)::character varying
            ELSE aud_insert.razon_social
         END AS empleado_insert, usu_aud_insert.id_persona, rastreo_vehiculo.sucursal_instalacion, rastreo_sucursales.descripcion as sucursal, rastreo_vehiculo.demo
   FROM rastreo_persona
   LEFT JOIN rastreo_cliente ON rastreo_persona.idrastreo_persona = rastreo_cliente.id_cliente
   LEFT JOIN rastreo_vehiculo ON rastreo_persona.idrastreo_persona = rastreo_vehiculo.id_cliente
   LEFT JOIN rastreo_sucursales ON rastreo_vehiculo.sucursal_instalacion = rastreo_sucursales.idrastreo_sucursal
   LEFT JOIN rastreo_usuarios ON rastreo_persona.idrastreo_persona = rastreo_usuarios.id_persona
   LEFT JOIN rastreo_usuario_lista_vehiculo ON rastreo_persona.idrastreo_persona = rastreo_usuario_lista_vehiculo.id_cliente
   AND rastreo_usuario_lista_vehiculo.id_vehiculo = rastreo_vehiculo.idrastreo_vehiculo
   AND rastreo_usuario_lista_vehiculo.id_usuarios = rastreo_usuarios.idrastreo_usuarios   
   LEFT JOIN rastreo_marca_vehiculo ON rastreo_marca_vehiculo.idrastreo_marca_vehiculo = rastreo_vehiculo.marca
   LEFT JOIN rastreo_modelo_vehiculo ON rastreo_modelo_vehiculo.idrastreo_modelo_vehiculo = rastreo_vehiculo.modelo
   LEFT JOIN rastreogps_equipogps ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps
   LEFT JOIN rastreogps_tipoequipo ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo
   LEFT JOIN rastreogps_simcards ON rastreogps_simcards.idrastreogps_simcards = rastreogps_equipogps.id_simcard
   LEFT JOIN rastreo_persona as rastreo_instaladores ON rastreo_vehiculo.id_instalador = rastreo_instaladores.idrastreo_persona
   LEFT JOIN rastreo_usuarios as usu_aud_insert ON rastreo_persona.user_ins = usu_aud_insert.idrastreo_usuarios
   LEFT JOIN rastreo_persona as aud_insert ON aud_insert.idrastreo_persona = usu_aud_insert.id_persona
WHERE rastreo_cliente.id_cliente NOT IN (SELECT id_empleado FROM rastreo_empleados) AND rastreo_vehiculo.idrastreo_vehiculo IS NOT NULL
ORDER BY rastreo_vehiculo.identificador_rastreo;
ALTER TABLE rsview_cliente_vehiculo_equipogps OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
--------------------------------- Vista: Eventos, equipo y tipo de equipo ----------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_equipo_eventos;
CREATE OR REPLACE VIEW rsview_equipo_eventos AS 
 SELECT 
	rastreogps_equipo_eventos.id_equipogps as id_rastreoequipogps,
	rastreogps_equipogps.id_equipo_gps, 
	rastreogps_tipoequipo.idrastreogps_tipoequipo as id_tipoequipo,
	rastreogps_tipoequipo.tipo_equipo AS tipo_de_equipo, 
	rastreogps_equipo_eventos.id_tipoevento,
	rastreogps_equipo_eventos.evento,
	rastreogps_tipoevento.descripcion AS tipoevento_evento, 
	rastreogps_tipoevento.evento as evento_raw,
	rastreogps_tipoevento.color as color_evento,
	rastreogps_equipo_eventos.habilitado
   FROM rastreogps_equipo_eventos
	LEFT JOIN rastreogps_equipogps ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps
	LEFT JOIN rastreogps_tipoevento ON rastreogps_equipo_eventos.id_tipoevento = rastreogps_tipoevento.idrastreogps_tipo_evento
	LEFT JOIN rastreogps_tipoequipo ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo;

ALTER TABLE rsview_equipo_eventos OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
--------------------------------- Vista: Equipo y tipo de equipo -------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_equipo_tipoequipo;
CREATE OR REPLACE VIEW rsview_equipo_tipoequipo AS 
 SELECT 
	rastreo_vehiculo.identificador_rastreo,
	rastreogps_equipogps.idrastreogps_equipogps,
	rastreogps_equipogps.id_equipo_gps,
	rastreogps_tipoequipo.tipo_equipo AS tipo_de_equipo,
	rastreogps_equipogps.id_simcard,
	rastreogps_simcards.sim_nro,
	rastreogps_equipogps.nro_serie_gps
   FROM rastreogps_equipogps
	LEFT JOIN rastreogps_tipoequipo ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo
	LEFT JOIN rastreogps_simcards   ON rastreogps_equipogps.id_simcard = rastreogps_simcards.idrastreogps_simcards
	LEFT JOIN rastreo_vehiculo ON id_equipogps = rastreogps_equipogps.idrastreogps_equipogps;
	
ALTER TABLE rsview_equipo_tipoequipo OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
--------------------------------- Vista: Empleados de la empresa -------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_empleados;
CREATE OR REPLACE VIEW rsview_empleados AS 
 SELECT rastreo_empleados.id_empleado,
	rastreo_empleados.instalador, rastreo_empleados.acceso_sistema, rastreo_empleados.estado_empleado, rastreo_empleados.estado_fecha,	
	CASE
	    WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
	    ELSE rastreo_persona.razon_social
        END AS nombre_completo,
        rastreo_persona.id_pais, rastreo_pais.pais, rastreo_persona.id_distrito, rastreo_distrito.distrito, 
        rastreo_persona.id_ciudad, rastreo_ciudad.ciudad, rastreo_persona.tipo_documento, rastreo_tipo_documento.descripcion as tipo_de_documento,
        rastreo_persona.tipo_persona, rastreo_persona.nro_documento, rastreo_persona.direccion, rastreo_persona.email, 
        rastreo_persona.tel_movil, rastreo_persona.tel_part, rastreo_persona.tel_ofi, rastreo_persona.user_ins, rastreo_persona.fech_ins, rastreo_persona.user_upd, rastreo_persona.fech_upd
   FROM rastreo_persona
   LEFT JOIN rastreo_empleados 		ON rastreo_persona.idrastreo_persona = rastreo_empleados.id_empleado
   LEFT JOIN rastreo_pais 		ON rastreo_persona.id_pais = rastreo_pais.idrastreo_pais
   LEFT JOIN rastreo_distrito 		ON rastreo_persona.id_distrito = rastreo_distrito.idrastreo_distrito
   LEFT JOIN rastreo_ciudad 		ON rastreo_persona.id_ciudad = rastreo_ciudad.idrastreo_ciudad
   LEFT JOIN rastreo_tipo_documento 	ON rastreo_persona.tipo_documento = rastreo_tipo_documento.idrastreo_tipo_documento
 WHERE rastreo_empleados.id_empleado NOT IN (SELECT id_cliente FROM rastreo_cliente);
ALTER TABLE rsview_empleados OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
---------------------------------- Vista: Clientes de la empresa -------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_clientes;
CREATE OR REPLACE VIEW rsview_clientes AS 
 SELECT rastreo_cliente.id_cliente,
	rastreo_cliente.acceso_sistema, rastreo_cliente.clave_personal, rastreo_cliente.estado_cliente, rastreo_cliente.estado_fecha, 
	CASE
	    WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
	    ELSE rastreo_persona.razon_social
        END AS nombre_completo,
        rastreo_persona.id_pais, rastreo_pais.pais, rastreo_persona.id_distrito, rastreo_distrito.distrito, 
        rastreo_persona.id_ciudad, rastreo_ciudad.ciudad, rastreo_persona.tipo_documento, rastreo_tipo_documento.descripcion as tipo_de_documento,
        rastreo_persona.tipo_persona, rastreo_persona.nro_documento, rastreo_persona.direccion, rastreo_persona.email, 
        rastreo_persona.tel_movil, rastreo_persona.tel_part, rastreo_persona.tel_ofi, rastreo_persona.user_ins, rastreo_persona.fech_ins, rastreo_persona.user_upd, rastreo_persona.fech_upd
   FROM rastreo_persona
   LEFT JOIN rastreo_cliente 		ON rastreo_persona.idrastreo_persona = rastreo_cliente.id_cliente
   LEFT JOIN rastreo_pais 		ON rastreo_persona.id_pais = rastreo_pais.idrastreo_pais
   LEFT JOIN rastreo_distrito 		ON rastreo_persona.id_distrito = rastreo_distrito.idrastreo_distrito
   LEFT JOIN rastreo_ciudad 		ON rastreo_persona.id_ciudad = rastreo_ciudad.idrastreo_ciudad
   LEFT JOIN rastreo_tipo_documento 	ON rastreo_persona.tipo_documento = rastreo_tipo_documento.idrastreo_tipo_documento
 WHERE rastreo_cliente.id_cliente NOT IN (SELECT id_empleado FROM rastreo_empleados);
ALTER TABLE rsview_clientes OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------   Vista: Vehiculo, equipo, cliente, ultima posicion del gps   ---------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_vehiculo_bandejaentrada_cliente_equipogps;
CREATE OR REPLACE VIEW rsview_vehiculo_bandejaentrada_cliente_equipogps AS 
 SELECT rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.idrastreo_vehiculo, rastreo_vehiculo.proviene_de, rastreogps_tipoequipo.tipo_equipo, rastreogps_tipoequipo.tipo_de_reporte, rastreogps_equipogps.id_equipo_gps, rastreogps_equipogps.nro_serie_gps, rastreogps_equipogps.id_simcard, rastreogps_simcards.sim_nro, rastreo_usuarios.usuario, rastreo_usuarios.idrastreo_usuarios, rastreo_usuario_lista_vehiculo.id_usuarios, rastreo_usuario_lista_vehiculo.id_cliente, rastreo_usuario_lista_vehiculo.id_vehiculo, rastreo_usuario_lista_vehiculo.visible, rastreogps_tipoevento.idrastreogps_tipo_evento AS id_evento, 
        CASE
            WHEN rastreogps_equipo_eventos.evento IS NULL THEN rastreogps_tipoevento.descripcion
            WHEN rastreogps_tipoevento.descripcion IS NULL THEN rastreogps_bandeja_de_entrada.gps_evento::text::character varying
            WHEN rastreogps_equipo_eventos.evento IS NOT NULL THEN rastreogps_equipo_eventos.evento::character varying
            ELSE NULL::character varying
        END AS evento, rastreogps_tipoevento.color AS color_evento, rastreo_persona.idrastreo_persona, 
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente, rastreo_cliente.estado_cliente, rastreo_persona.tel_movil, rastreo_persona.tel_part, rastreo_persona.tel_ofi, rastreo_persona.email, rastreogps_bandeja_de_entrada.idrastreogps_bandeja_de_entrada, rastreogps_bandeja_de_entrada.id_equipogps, rastreogps_bandeja_de_entrada.gps_longitud, rastreogps_bandeja_de_entrada.gps_latitud, rastreogps_bandeja_de_entrada.gps_fecha, rastreogps_bandeja_de_entrada.gps_velocidad, rastreogps_bandeja_de_entrada.gps_rumbo, 
        CASE
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo >= 0 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 23 OR rastreogps_bandeja_de_entrada.gps_rumbo >= 345 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 360 THEN 'NORTE'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 23 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 69 THEN 'NORESTE'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 69 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 115 THEN 'ESTE'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 115 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 161 THEN 'SURESTE'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 161 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 207 THEN 'SUR'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 207 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 253 THEN 'SUROESTE'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 253 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 299 THEN 'OESTE'::text
            WHEN rastreogps_bandeja_de_entrada.gps_rumbo > 299 AND rastreogps_bandeja_de_entrada.gps_rumbo <= 345 THEN 'NOROESTE'::text
            ELSE NULL::text
        END AS rumbo, rastreogps_bandeja_de_entrada.gps_evento, rastreogps_bandeja_de_entrada.gps_edaddeldato, rastreogps_bandeja_de_entrada.gps_hdop, rastreogps_bandeja_de_entrada.gps_satstatus, rastreogps_bandeja_de_entrada.gps_gsmstatus, rastreogps_bandeja_de_entrada.gps_estado_io, rastreogps_bandeja_de_entrada.gps_tipodeposicion, rastreogps_bandeja_de_entrada.gps_ip, rastreogps_bandeja_de_entrada.gps_obs, rastreogps_bandeja_de_entrada.gps_dir, rastreo_tipo_vehiculo.icono, rastreo_vehiculo.anho, rastreo_vehiculo.chasis, rastreo_vehiculo.color, rastreo_vehiculo.matricula, rastreo_vehiculo.kilometraje, rastreo_vehiculo.activo, rastreo_vehiculo.marca AS idmarca, rastreo_vehiculo.modelo AS idmodelo, rastreo_vehiculo.chofer, rastreo_vehiculo.chofer_contacto, rastreo_vehiculo.observacion, rastreo_marca_vehiculo.marca, rastreo_modelo_vehiculo.modelo, rastreo_sucursales.descripcion as sucursal
   FROM rastreogps_bandeja_de_entrada
   RIGHT JOIN rastreo_vehiculo ON rastreogps_bandeja_de_entrada.id_equipogps = rastreo_vehiculo.id_equipogps
   LEFT JOIN rastreo_sucursales ON rastreo_vehiculo.sucursal_instalacion = rastreo_sucursales.idrastreo_sucursal
   LEFT JOIN rastreo_tipo_vehiculo ON rastreo_vehiculo.tipo_vehiculo = rastreo_tipo_vehiculo.idrastreo_tipo_vehiculo
   LEFT JOIN rastreo_persona ON rastreo_vehiculo.id_cliente = rastreo_persona.idrastreo_persona
   LEFT JOIN rastreo_cliente ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente
   LEFT JOIN rastreo_marca_vehiculo ON rastreo_vehiculo.marca = rastreo_marca_vehiculo.idrastreo_marca_vehiculo
   LEFT JOIN rastreo_modelo_vehiculo ON rastreo_vehiculo.modelo = rastreo_modelo_vehiculo.idrastreo_modelo_vehiculo
   LEFT JOIN rastreogps_equipogps ON rastreogps_equipogps.idrastreogps_equipogps = rastreo_vehiculo.id_equipogps
   LEFT JOIN rastreogps_tipoequipo ON rastreogps_tipoequipo.idrastreogps_tipoequipo = rastreogps_equipogps.tipoequipo
   LEFT JOIN rastreogps_simcards ON rastreogps_equipogps.id_simcard = rastreogps_simcards.idrastreogps_simcards
   LEFT JOIN rastreogps_tipoevento ON rastreogps_tipoevento.id_tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo AND rastreogps_tipoevento.evento::text = rastreogps_bandeja_de_entrada.gps_evento::text
   LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps AND rastreogps_tipoevento.idrastreogps_tipo_evento = rastreogps_equipo_eventos.id_tipoevento
   LEFT JOIN rastreo_usuario_lista_vehiculo ON rastreo_usuario_lista_vehiculo.id_vehiculo = rastreo_vehiculo.idrastreo_vehiculo
   LEFT JOIN rastreo_usuarios ON rastreo_usuario_lista_vehiculo.id_usuarios = rastreo_usuarios.idrastreo_usuarios
  ORDER BY rastreo_vehiculo.identificador_rastreo;

ALTER TABLE rsview_vehiculo_bandejaentrada_cliente_equipogps OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------------------   Vista: Vehiculo, equipo, cliente, RECORRIDO DEL VEHICULO ORDENADO POR FECHA Y HORA   --------------
------------------------------------------------------------------------------------------------------------------------------------
--DROP VIEW IF EXISTS rsview_recorridos;
/*
CREATE OR REPLACE VIEW rsview_recorridos AS 
SELECT --DISTINCT ON (rastreogps_reportes.gps_fechahora_reporte)
	rastreogps_tipoevento.idrastreogps_tipo_evento as id_evento,
	CASE 
	    WHEN rastreogps_tipoevento.descripcion IS NULL THEN
		rastreogps_reportes.gps_evento
	    WHEN rastreogps_tipoevento.descripcion IS NOT NULL THEN
		rastreogps_tipoevento.descripcion
	END as evento,
	rastreogps_tipoevento.color as color_evento,
	rastreogps_equipo_eventos.habilitado as evento_habilitado,
	rastreogps_reportes.idrastreogps_reportes, rastreogps_reportes.id_equipogps, 
	rastreogps_reportes.gps_longitud, rastreogps_reportes.gps_latitud, 
	rastreogps_reportes.gps_fechahora_reporte, rastreogps_reportes.gps_velocidad,
	rastreogps_reportes.gps_rumbo, 
	CASE
	    WHEN (rastreogps_reportes.gps_rumbo >= 0   AND rastreogps_reportes.gps_rumbo <= 23 ) OR
		 (rastreogps_reportes.gps_rumbo >= 345 AND rastreogps_reportes.gps_rumbo <= 360) THEN 
		('NORTE'::text)
	    WHEN rastreogps_reportes.gps_rumbo > 23 AND rastreogps_reportes.gps_rumbo <= 69 THEN 
		('NORESTE'::text)
	    WHEN rastreogps_reportes.gps_rumbo > 69 AND rastreogps_reportes.gps_rumbo <= 115 THEN 
	    ('ESTE'::text)
	    WHEN rastreogps_reportes.gps_rumbo > 115 AND rastreogps_reportes.gps_rumbo <= 161 THEN 
	    ('SURESTE'::text)
	    WHEN rastreogps_reportes.gps_rumbo > 161 AND rastreogps_reportes.gps_rumbo <= 207 THEN 
	    ('SUR'::text)
	    WHEN rastreogps_reportes.gps_rumbo > 207 AND rastreogps_reportes.gps_rumbo <= 253 THEN 
	    ('SUROESTE'::text)	    
	    WHEN rastreogps_reportes.gps_rumbo > 253 AND rastreogps_reportes.gps_rumbo <= 299 THEN 
	    ('OESTE'::text)	
	    WHEN rastreogps_reportes.gps_rumbo > 299 AND rastreogps_reportes.gps_rumbo <= 345 THEN 
	    ('NOROESTE'::text)
	END as rumbo, 
	rastreogps_reportes.gps_evento,
	rastreogps_reportes.gps_edaddeldato, rastreogps_reportes.gps_hdop, 
	rastreogps_reportes.gps_satstatus, rastreogps_reportes.gps_gsmstatus, 
	rastreogps_reportes.gps_estado_io, rastreogps_reportes.gps_tipodeposicion, 
	rastreogps_reportes.gps_ip, rastreogps_reportes.gps_obs,
        rastreo_persona.idrastreo_persona AS idcliente, 
        rastreo_tipo_vehiculo.icono, 
        rastreo_tipo_vehiculo.tipo_de_vehiculo,
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS cliente, rastreo_cliente.estado_cliente, rastreo_vehiculo.idrastreo_vehiculo AS id_vehiculo, 
        rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.consumo_aprox, rastreo_vehiculo.anho,rastreo_vehiculo.chasis, 
        rastreo_vehiculo.color, rastreo_vehiculo.matricula, rastreo_vehiculo.kilometraje, rastreo_vehiculo.activo, 
        rastreo_vehiculo.marca AS idmarca, rastreo_vehiculo.modelo AS idmodelo,
        rastreo_marca_vehiculo.marca, 
        rastreo_modelo_vehiculo.modelo, 
        rastreogps_equipogps.idrastreogps_equipogps AS idequipogps, 
        rastreogps_tipoequipo.idrastreogps_tipoequipo AS id_tipoequipogps, rastreogps_tipoequipo.tipo_equipo AS tipoequipogps, 
        rastreogps_tipoequipo.tipo_de_reporte AS tipo_de_reportegps, rastreogps_equipogps.id_equipo_gps, 
        rastreogps_equipogps.nro_serie_gps
   FROM rastreogps_reportes
   LEFT JOIN rastreo_vehiculo 			ON rastreogps_reportes.id_equipogps  = rastreo_vehiculo.id_equipogps
   LEFT JOIN rastreo_persona 			ON rastreo_vehiculo.id_cliente       = rastreo_persona.idrastreo_persona
   LEFT JOIN rastreo_cliente 			ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente
   LEFT JOIN rastreo_tipo_vehiculo 		ON rastreo_vehiculo.tipo_vehiculo = rastreo_tipo_vehiculo.idrastreo_tipo_vehiculo
   LEFT JOIN rastreo_marca_vehiculo 		ON rastreo_marca_vehiculo.idrastreo_marca_vehiculo = rastreo_vehiculo.marca
   LEFT JOIN rastreo_modelo_vehiculo 		ON rastreo_modelo_vehiculo.idrastreo_modelo_vehiculo = rastreo_vehiculo.modelo
   LEFT JOIN rastreogps_equipogps 		ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps
   LEFT JOIN rastreogps_tipoequipo 		ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo
   LEFT JOIN rastreogps_tipoevento     ON rastreogps_tipoevento.id_tipoequipo = rastreogps_equipogps.tipoequipo
					AND rastreogps_tipoevento.evento = rastreogps_reportes.gps_evento
   LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps 
					AND rastreogps_tipoevento.id_tipoequipo = rastreogps_equipogps.tipoequipo
  ORDER BY rastreogps_reportes.gps_fechahora_reporte;

ALTER TABLE rsview_recorridos OWNER TO rastreo_admin;
*/
------------------------------------------------------------------------------------------------------------------------------------
------------------------------- Vista: Cliente, vehiculo, equipo, tipo equipo, sim y geocercas -------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_cliente_vehiculo_equipogps_geocercas;
CREATE OR REPLACE VIEW rsview_cliente_vehiculo_equipogps_geocercas AS 
 SELECT rastreo_vehiculo.idrastreo_vehiculo, rastreo_persona.idrastreo_persona, 
        CASE
            WHEN rastreo_persona.tipo_persona = 'F'::bpchar THEN 
            ((rastreo_persona.nombre::text || ' '::text) || rastreo_persona.apellido::text)::character varying
            ELSE rastreo_persona.razon_social
        END AS razon_social, rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.anho, rastreo_vehiculo.chasis, 
        rastreo_vehiculo.color, rastreo_vehiculo.matricula, rastreo_vehiculo.kilometraje, rastreo_vehiculo.activo, 
        rastreo_vehiculo.consumo_aprox, rastreo_marca_vehiculo.marca, rastreo_modelo_vehiculo.modelo, 
        rastreogps_equipogps.idrastreogps_equipogps, rastreogps_tipoequipo.idrastreogps_tipoequipo, 
        rastreogps_tipoequipo.tipo_equipo, rastreogps_tipoequipo.tipo_de_reporte, rastreogps_equipogps.id_equipo_gps, 
        rastreogps_equipogps.nro_serie_gps, rastreogps_simcards.idrastreogps_simcards, rastreogps_simcards.sim_nro, 
        rastreo_geocercas.idrastreo_geocercas, rastreo_geocercas.descripcion, rastreo_geocercas.puntos, rastreo_geocercas.hora_activacion, 
        rastreo_geocercas.horas_activa, rastreo_geocercas.activa AS geocerca_activa, rastreo_geocercas.activo_siempre, 
        rastreo_geocercas.dia_activacion, rastreo_geocercas.avisar_entrada, rastreo_geocercas.mails, rastreo_geocercas.tel_movil,
        rastreo_geocercas.avisos_activado, rastreo_geocercas.aviso_entradasalida
   FROM rastreo_geocercas
   LEFT JOIN rastreo_persona ON rastreo_geocercas.id_cliente = rastreo_persona.idrastreo_persona
   LEFT JOIN rastreo_vehiculo ON rastreo_geocercas.id_vehiculo = rastreo_vehiculo.idrastreo_vehiculo
   LEFT JOIN rastreo_marca_vehiculo ON rastreo_marca_vehiculo.idrastreo_marca_vehiculo = rastreo_vehiculo.marca
   LEFT JOIN rastreo_modelo_vehiculo ON rastreo_modelo_vehiculo.idrastreo_modelo_vehiculo = rastreo_vehiculo.modelo
   LEFT JOIN rastreogps_equipogps ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps
   LEFT JOIN rastreogps_tipoequipo ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo
   LEFT JOIN rastreogps_simcards ON rastreogps_simcards.idrastreogps_simcards = rastreogps_equipogps.id_simcard
  WHERE rastreo_geocercas.activa = true
  ORDER BY rastreo_vehiculo.idrastreo_vehiculo, rastreo_persona.idrastreo_persona;

ALTER TABLE rsview_cliente_vehiculo_equipogps_geocercas OWNER TO rastreo_admin;
------------------------------------------------------------------------------------------------------------------------------------
------------------ Vista: Datos necesarios para la insercion de reportes en la tabla de bandeja de entrada -------------------------
------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW IF EXISTS rsview_help_for_insert;
CREATE OR REPLACE VIEW rsview_help_for_insert AS 
SELECT
	rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.idrastreo_vehiculo, 
	rastreogps_equipogps.idrastreogps_equipogps, rastreogps_equipogps.Id_equipo_gps, 
	rastreogps_tipoequipo.tipo_de_reporte, rastreogps_tipoequipo.tipo_equipo
FROM rastreo_vehiculo
	LEFT JOIN rastreogps_equipogps  ON rastreogps_equipogps.idrastreogps_equipogps   = rastreo_vehiculo.id_equipogps
	LEFT JOIN rastreogps_tipoequipo ON rastreogps_tipoequipo.idrastreogps_tipoequipo = rastreogps_equipogps.tipoequipo
WHERE 
	rastreo_vehiculo.activo = TRUE
ORDER BY 
	rastreo_vehiculo.identificador_rastreo;
ALTER TABLE rsview_help_for_insert OWNER TO rastreo_admin;

------------------------------------------------------------------------------------------------------------------------------------
------------------ Vista: Datos necesarios para la insercion de reportes en la tabla de bandeja de entrada -------------------------
------------------------------------------------------------------------------------------------------------------------------------
--DROP VIEW IF EXISTS rsview_eventos_buses;
/*
CREATE OR REPLACE VIEW rsview_eventos_buses AS 
SELECT DISTINCT ON (rastreogps_reportes.gps_fechahora_reporte) 
	rastreogps_tipoevento.idrastreogps_tipo_evento as id_evento, 
	CASE 
	    WHEN rastreogps_equipo_eventos.evento IS NOT NULL THEN 
        rastreogps_equipo_eventos.evento 
       WHEN rastreogps_equipo_eventos.evento IS NULL THEN 
        rastreogps_tipoevento.descripcion 
	    WHEN rastreogps_tipoevento.descripcion IS NULL THEN 
        rastreogps_reportes.Gps_evento 
	END as evento, 
	rastreogps_tipoevento.color as color_evento, 
	rastreogps_equipo_eventos.habilitado as evento_habilitado, 
	rastreogps_reportes.idrastreogps_reportes, rastreogps_reportes.id_equipogps, 
	rastreogps_reportes.gps_longitud, rastreogps_reportes.gps_latitud, 
	rastreogps_reportes.gps_fechahora_reporte, rastreogps_reportes.gps_velocidad, 
	rastreogps_reportes.gps_rumbo, 
	CASE 
	    WHEN (rastreogps_reportes.gps_rumbo >= 0   AND rastreogps_reportes.gps_rumbo <= 23 ) OR 
		 (rastreogps_reportes.gps_rumbo >= 345 AND rastreogps_reportes.gps_rumbo <= 360) THEN 
		('NORTE'::text) 
	    WHEN rastreogps_reportes.gps_rumbo > 23 AND rastreogps_reportes.gps_rumbo <= 69 THEN 
		('NORESTE'::text) 
	    WHEN rastreogps_reportes.gps_rumbo > 69 AND rastreogps_reportes.gps_rumbo <= 115 THEN 
	    ('ESTE'::text) 
	    WHEN rastreogps_reportes.gps_rumbo > 115 AND rastreogps_reportes.gps_rumbo <= 161 THEN 
	    ('SURESTE'::text) 
	    WHEN rastreogps_reportes.gps_rumbo > 161 AND rastreogps_reportes.gps_rumbo <= 207 THEN 
	    ('SUR'::text) 
	    WHEN rastreogps_reportes.gps_rumbo > 207 AND rastreogps_reportes.gps_rumbo <= 253 THEN 
	    ('SUROESTE'::text) 
	    WHEN rastreogps_reportes.gps_rumbo > 253 AND rastreogps_reportes.gps_rumbo <= 299 THEN 
	    ('OESTE'::text)	 
	    WHEN rastreogps_reportes.gps_rumbo > 299 AND rastreogps_reportes.gps_rumbo <= 345 THEN 
	    ('NOROESTE'::text) 
	END as rumbo, 
	rastreogps_reportes.gps_evento, 
	rastreogps_reportes.gps_edaddeldato, rastreogps_reportes.gps_hdop, 
	rastreogps_reportes.gps_satstatus, rastreogps_reportes.gps_gsmstatus, 
	rastreogps_reportes.gps_estado_io, rastreogps_reportes.gps_tipodeposicion, 
	rastreogps_reportes.gps_ip, rastreogps_reportes.gps_obs, 
   rastreo_persona.idrastreo_persona AS idcliente, 
   rastreo_vehiculo.identificador_rastreo, rastreo_vehiculo.idrastreo_vehiculo AS id_vehiculo, 
   rastreo_vehiculo.consumo_aprox, 
   rastreogps_equipogps.idrastreogps_equipogps AS idequipogps, 
   rastreogps_tipoequipo.idrastreogps_tipoequipo AS id_tipoequipogps, rastreogps_tipoequipo.tipo_equipo AS tipoequipogps, 
   rastreogps_equipogps.id_equipo_gps, 
   rastreogps_equipogps.Nro_serie_gps 
   FROM rastreogps_reportes 
LEFT JOIN rastreo_vehiculo 			ON rastreogps_reportes.id_equipogps  = rastreo_vehiculo.id_equipogps 
LEFT JOIN rastreo_persona 			ON rastreo_vehiculo.id_cliente       = rastreo_persona.idrastreo_persona 
LEFT JOIN rastreo_cliente 			ON rastreo_vehiculo.id_cliente = rastreo_cliente.id_cliente 
LEFT JOIN rastreogps_equipogps 		ON rastreo_vehiculo.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps 
LEFT JOIN rastreogps_tipoequipo 		ON rastreogps_equipogps.tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo 
LEFT JOIN rastreogps_tipoevento ON rastreogps_tipoevento.id_tipoequipo = rastreogps_tipoequipo.idrastreogps_tipoequipo 
                               AND rastreogps_tipoevento.evento = rastreogps_reportes.gps_evento 
LEFT JOIN rastreogps_equipo_eventos ON rastreogps_equipo_eventos.id_equipogps = rastreogps_equipogps.idrastreogps_equipogps 
				AND rastreogps_tipoevento.idrastreogps_tipo_evento = rastreogps_equipo_eventos.id_tipoevento 
WHERE rastreogps_equipo_eventos.habilitado = TRUE 
 AND
  --(
  (rastreogps_equipo_eventos.evento ILIKE '%CONTROL%' OR rastreogps_equipo_eventos.evento ILIKE '%LLE%GADA%' OR rastreogps_equipo_eventos.evento ILIKE '%SALIDA%');
  --OR (rastreogps_tipoevento.descripcion ILIKE '%CONTROL%' OR rastreogps_tipoevento.descripcion ILIKE '%LLEGADA%' OR rastreogps_tipoevento.descripcion ILIKE '%SALIDA%'));
  
ALTER TABLE rsview_eventos_buses OWNER TO rastreo_admin;
*/