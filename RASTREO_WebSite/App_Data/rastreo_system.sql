



































































































































































































































































-- 
-- This script is generated automatically and should not be modified 
-- Changes should only be made through DbDesigner 
-- 

DROP TABLE IF EXISTS rastreo_usuarios CASCADE; 


DROP TABLE IF EXISTS rastreo_persona CASCADE; 


DROP TABLE IF EXISTS rastreo_tipo_documento CASCADE; 


DROP TABLE IF EXISTS rastreo_cliente CASCADE; 


DROP TABLE IF EXISTS rastreo_empleados CASCADE; 


DROP TABLE IF EXISTS rastreo_cliente_contactos CASCADE; 


DROP TABLE IF EXISTS rastreo_vehiculo CASCADE; 


DROP TABLE IF EXISTS rastreo_tipo_vehiculo CASCADE; 


DROP TABLE IF EXISTS rastreo_marca_vehiculo CASCADE; 


DROP TABLE IF EXISTS rastreo_modelo_vehiculo CASCADE; 


DROP TABLE IF EXISTS rastreogps_tipoequipo CASCADE; 


DROP TABLE IF EXISTS rastreogps_equipogps CASCADE; 


DROP TABLE IF EXISTS rastreogps_simcards CASCADE; 


DROP TABLE IF EXISTS rastreogps_bandeja_de_entrada CASCADE; 


DROP TABLE IF EXISTS rastreogps_opciones_de_pantalla CASCADE; 


DROP TABLE IF EXISTS rastreogps_usuario_opciones CASCADE; 


DROP TABLE IF EXISTS rastreo_pais CASCADE; 


DROP TABLE IF EXISTS rastreo_distrito CASCADE; 


DROP TABLE IF EXISTS rastreo_ciudad CASCADE; 


DROP TABLE IF EXISTS rastreogps_reportes CASCADE; 


DROP TABLE IF EXISTS rastreogps_panico CASCADE; 


DROP TABLE IF EXISTS rastreogps_tipoevento CASCADE; 


DROP TABLE IF EXISTS rastreo_historial CASCADE; 


DROP TABLE IF EXISTS rastreo_proviene_de_seguro CASCADE; 


DROP TABLE IF EXISTS rastreogps_cola_de_comandos CASCADE; 


DROP TABLE IF EXISTS rastreo_vehiculo_fotos CASCADE; 


DROP TABLE IF EXISTS rastreo_cliente_documentos CASCADE; 


DROP TABLE IF EXISTS rastreogps_equipo_eventos CASCADE; 


DROP TABLE IF EXISTS rastreogps_odometro CASCADE; 


DROP TABLE IF EXISTS rastreogps_fuel CASCADE; 


DROP TABLE IF EXISTS rastreo_geocercas CASCADE; 


DROP TABLE IF EXISTS rastreo_template_hoja_de_ruta CASCADE; 


DROP TABLE IF EXISTS rastreo_hoja_de_ruta_puntos CASCADE; 


DROP TABLE IF EXISTS rastreo_referencias CASCADE; 


DROP TABLE IF EXISTS rastreo_hoja_de_ruta CASCADE; 


DROP TABLE IF EXISTS rastreo_template_puntos_hoja_de_ruta CASCADE; 


DROP TABLE IF EXISTS rastreo_usuario_lista_vehiculo CASCADE; 


DROP TABLE IF EXISTS rastreo_hojaderuta_has_vehiculo CASCADE; 



 
CREATE TABLE rastreo_usuarios ( 
  idrastreo_usuarios SERIAL NOT NULL, 
  id_persona INTEGER NOT NULL, 
  nombre_completo VARCHAR(256) NULL, 
  usuario VARCHAR(128) NULL, 
  pass VARCHAR(128) NULL, 
  su BOOL DEFAULT  FALSE NULL, 
  fech_login TIMESTAMP NULL, 
  intentos_login INTEGER DEFAULT  0 NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_usuarios ADD CONSTRAINT rastreo_usuarios_PK PRIMARY KEY (idrastreo_usuarios); 
 
CREATE INDEX rastreo_usuarios_FKIndex1 ON rastreo_usuarios (id_persona); 
 
CREATE UNIQUE INDEX rastreo_unique_user ON rastreo_usuarios (usuario); 


 
CREATE TABLE rastreo_persona ( 
  idrastreo_persona SERIAL NOT NULL, 
  id_distrito INTEGER NOT NULL, 
  id_pais INTEGER NOT NULL, 
  id_ciudad INTEGER NOT NULL, 
  tipo_documento INTEGER NOT NULL, 
  tipo_persona CHAR DEFAULT  'F' NOT NULL, 
  nro_documento VARCHAR(100) NOT NULL, 
  razon_social VARCHAR(255) NULL, 
  nombre VARCHAR(255) NULL, 
  apellido VARCHAR(255) NULL, 
  direccion TEXT NULL, 
  email TEXT NULL, 
  tel_part TEXT NULL, 
  tel_ofi TEXT NULL, 
  tel_movil TEXT NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_persona ADD CONSTRAINT rastreo_persona_PK PRIMARY KEY (idrastreo_persona); 
 
CREATE INDEX rastreo_persona_FKIndex1 ON rastreo_persona (tipo_documento); 
 
CREATE INDEX rastreo_persona_FKIndex3 ON rastreo_persona (id_ciudad, id_pais, id_distrito); 
 
CREATE UNIQUE INDEX rastreo_persona_nrodoc ON rastreo_persona (nro_documento, tipo_documento); 


 
CREATE TABLE rastreo_tipo_documento ( 
  idrastreo_tipo_documento SERIAL NOT NULL, 
  descripcion VARCHAR(512) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_tipo_documento ADD CONSTRAINT rastreo_tipo_documento_PK PRIMARY KEY (idrastreo_tipo_documento); 
 


 
CREATE TABLE rastreo_cliente ( 
  id_cliente INTEGER NOT NULL, 
  clave_personal VARCHAR(1024) NULL, 
  acceso_sistema BOOL DEFAULT  FALSE NULL, 
  estado_cliente BOOL DEFAULT  TRUE NULL, 
  estado_fecha TIMESTAMP DEFAULT  now() NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_cliente ADD CONSTRAINT rastreo_cliente_PK PRIMARY KEY (id_cliente); 
 
CREATE INDEX rastreo_cliente_FKIndex2 ON rastreo_cliente (id_cliente); 
 


 
CREATE TABLE rastreo_empleados ( 
  id_empleado INTEGER NOT NULL, 
  instalador BOOL DEFAULT  FALSE NULL, 
  acceso_sistema BOOL DEFAULT  FALSE NULL, 
  estado_empleado BOOL DEFAULT  TRUE NULL, 
  estado_fecha TIMESTAMP NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_empleados ADD CONSTRAINT rastreo_empleados_PK PRIMARY KEY (id_empleado); 
 
CREATE INDEX rastreo_empleados_FKIndex1 ON rastreo_empleados (id_empleado); 
 


 
CREATE TABLE rastreo_cliente_contactos ( 
  idrastreo_cliente_contactos SERIAL NOT NULL, 
  rastreo_cliente_id_cliente INTEGER NOT NULL, 
  nombre_apellido_razonsocial VARCHAR(1024) NULL, 
  nrodoc_ruc VARCHAR(512) NULL, 
  relacion VARCHAR(512) NULL, 
  telefono VARCHAR(1024) NULL, 
  email VARCHAR(512) NULL, 
  titular_secundario BOOL DEFAULT  FALSE NULL, 
  autorizado BOOL DEFAULT  FALSE NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_cliente_contactos ADD CONSTRAINT rastreo_cliente_contactos_PK PRIMARY KEY (idrastreo_cliente_contactos, rastreo_cliente_id_cliente); 
 
CREATE INDEX rastreo_cliente_contactos_FKIndex1 ON rastreo_cliente_contactos (rastreo_cliente_id_cliente); 
 


 
CREATE TABLE rastreo_vehiculo ( 
  idrastreo_vehiculo SERIAL NOT NULL, 
  id_cliente INTEGER NOT NULL, 
  id_instalador INTEGER NOT NULL, 
  proviene_de INTEGER NOT NULL, 
  id_equipogps INTEGER NULL, 
  identificador_rastreo VARCHAR(1024) NULL, 
  poliza_nroorden VARCHAR(512) NULL, 
  poliza_emision TIMESTAMP NULL, 
  poliza_venc TIMESTAMP NULL, 
  instalacion_fechahora TIMESTAMP NULL, 
  desinstalacion_fechahora TIMESTAMP NULL, 
  marca INTEGER NOT NULL, 
  modelo INTEGER NOT NULL, 
  tipo_vehiculo INTEGER NOT NULL, 
  anho INTEGER NULL, 
  chasis VARCHAR(512) NULL, 
  color VARCHAR(32) NULL, 
  motor VARCHAR(512) NULL, 
  matricula VARCHAR(128) NULL, 
  kilometraje INT NULL, 
  activo BOOL DEFAULT  TRUE NULL, 
  activo_fecha TIMESTAMP DEFAULT  now() NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreo_vehiculo_PK PRIMARY KEY (idrastreo_vehiculo, id_cliente); 
 
CREATE INDEX rastreo_vehiculo_FKIndex1 ON rastreo_vehiculo (tipo_vehiculo); 
 
CREATE INDEX rastreo_vehiculo_FKIndex2 ON rastreo_vehiculo (modelo, marca); 
 
CREATE INDEX rastreo_vehiculo_FKIndex3 ON rastreo_vehiculo (id_cliente); 
 
CREATE UNIQUE INDEX rastreo_vehiculo_FKIndex4 ON rastreo_vehiculo (id_equipogps); 
CREATE UNIQUE INDEX rastreo_vehiculo_FKIndex5 ON rastreo_vehiculo (identificador_rastreo); 
CREATE INDEX rastreo_vehiculo_FKIndex6 ON rastreo_vehiculo (proviene_de); 
 
CREATE INDEX rastreo_vehiculo_FKIndex7 ON rastreo_vehiculo (id_instalador); 
 


 
CREATE TABLE rastreo_tipo_vehiculo ( 
  idrastreo_tipo_vehiculo SERIAL NOT NULL, 
  tipo_de_vehiculo VARCHAR(512) NULL, 
  icono TEXT NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_tipo_vehiculo ADD CONSTRAINT rastreo_tipo_vehiculo_PK PRIMARY KEY (idrastreo_tipo_vehiculo); 
 


 
CREATE TABLE rastreo_marca_vehiculo ( 
  idrastreo_marca_vehiculo SERIAL NOT NULL, 
  marca VARCHAR(1024) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_marca_vehiculo ADD CONSTRAINT rastreo_marca_vehiculo_PK PRIMARY KEY (idrastreo_marca_vehiculo); 
 


 
CREATE TABLE rastreo_modelo_vehiculo ( 
  idrastreo_modelo_vehiculo SERIAL NOT NULL, 
  id_marca INTEGER NOT NULL, 
  modelo VARCHAR(1024) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_modelo_vehiculo ADD CONSTRAINT rastreo_modelo_vehiculo_PK PRIMARY KEY (idrastreo_modelo_vehiculo, id_marca); 
 
CREATE INDEX rastreo_modelo_vehiculo_FKIndex1 ON rastreo_modelo_vehiculo (id_marca); 
 


 
CREATE TABLE rastreogps_tipoequipo ( 
  idrastreogps_tipoequipo SERIAL NOT NULL, 
  tipo_equipo VARCHAR(1024) NULL, 
  tipo_de_reporte VARCHAR(64) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_tipoequipo ADD CONSTRAINT rastreogps_tipoequipo_PK PRIMARY KEY (idrastreogps_tipoequipo); 
 


 
CREATE TABLE rastreogps_equipogps ( 
  idrastreogps_equipogps SERIAL NOT NULL, 
  id_simcard INTEGER NULL, 
  tipoequipo INTEGER NOT NULL, 
  id_equipo_gps VARCHAR(1024) NULL, 
  nro_serie_gps VARCHAR(1024) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_equipogps ADD CONSTRAINT rastreogps_equipogps_PK PRIMARY KEY (idrastreogps_equipogps); 
 
CREATE INDEX rastreogps_equipogps_FKIndex1 ON rastreogps_equipogps (tipoequipo); 
 
CREATE INDEX rastreogps_equipogps_FKIndex2 ON rastreogps_equipogps (id_simcard); 
 
CREATE UNIQUE INDEX rastreogps_equipogps_FKIndex3 ON rastreogps_equipogps (id_simcard); 
CREATE UNIQUE INDEX rastreogps_equipogps_FKIndex4 ON rastreogps_equipogps (id_equipo_gps, tipoequipo); 


 
CREATE TABLE rastreogps_simcards ( 
  idrastreogps_simcards SERIAL NOT NULL, 
  sim_nro VARCHAR(512) NULL, 
  sim_pin VARCHAR(512) NULL, 
  sim_pin2 VARCHAR(512) NULL, 
  sim_puk VARCHAR(512) NULL, 
  sim_puk2 VARCHAR(512) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_simcards ADD CONSTRAINT rastreogps_simcards_PK PRIMARY KEY (idrastreogps_simcards); 
 


 
CREATE TABLE rastreogps_bandeja_de_entrada ( 
  idrastreogps_bandeja_de_entrada SERIAL NOT NULL, 
  id_equipogps INTEGER NOT NULL, 
  gps_longitud DOUBLE PRECISION NULL, 
  gps_latitud DOUBLE PRECISION NULL, 
  gps_fecha TIMESTAMP DEFAULT  now() NULL, 
  gps_velocidad INTEGER NULL, 
  gps_rumbo INTEGER NULL, 
  gps_evento VARCHAR(32) NULL, 
  gps_edaddeldato VARCHAR(32) NULL, 
  gps_hdop VARCHAR(32) NULL, 
  gps_satstatus VARCHAR(32) NULL, 
  gps_gsmstatus VARCHAR(32) NULL, 
  gps_estado_io VARCHAR(32) NULL, 
  gps_tipodeposicion VARCHAR(32) NULL, 
  gps_ip VARCHAR(128) NULL, 
  gps_obs TEXT NULL 
); 
ALTER TABLE rastreogps_bandeja_de_entrada ADD CONSTRAINT rastreogps_bandeja_de_entrada_PK PRIMARY KEY (idrastreogps_bandeja_de_entrada); 
 


 
CREATE TABLE rastreogps_opciones_de_pantalla ( 
  idrastreogps_opciones_de_pantalla SERIAL NOT NULL, 
  opcion_de_pantalla VARCHAR(1024) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_opciones_de_pantalla ADD CONSTRAINT rastreogps_opciones_de_pantalla_PK PRIMARY KEY (idrastreogps_opciones_de_pantalla); 
 


 
CREATE TABLE rastreogps_usuario_opciones ( 
  opcion_de_pantalla INTEGER NOT NULL, 
  id_usuario INTEGER NOT NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_usuario_opciones ADD CONSTRAINT rastreogps_usuario_opciones_PK PRIMARY KEY (opcion_de_pantalla, id_usuario); 
 
CREATE INDEX rastreogps_usuario_opciones_FKIndex1 ON rastreogps_usuario_opciones (id_usuario); 
 
CREATE INDEX rastreogps_usuario_opciones_FKIndex2 ON rastreogps_usuario_opciones (opcion_de_pantalla); 
 


 
CREATE TABLE rastreo_pais ( 
  idrastreo_pais SERIAL NOT NULL, 
  pais VARCHAR(512) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_pais ADD CONSTRAINT rastreo_pais_PK PRIMARY KEY (idrastreo_pais); 
 


 
CREATE TABLE rastreo_distrito ( 
  idrastreo_distrito SERIAL NOT NULL, 
  id_pais INTEGER NOT NULL, 
  distrito VARCHAR(512) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_distrito ADD CONSTRAINT rastreo_distrito_PK PRIMARY KEY (idrastreo_distrito, id_pais); 
 
CREATE INDEX rastreo_distrito_FKIndex1 ON rastreo_distrito (id_pais); 
 


 
CREATE TABLE rastreo_ciudad ( 
  idrastreo_ciudad SERIAL NOT NULL, 
  id_pais INTEGER NOT NULL, 
  id_distrito INTEGER NOT NULL, 
  ciudad VARCHAR(512) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_ciudad ADD CONSTRAINT rastreo_ciudad_PK PRIMARY KEY (idrastreo_ciudad, id_pais, id_distrito); 
 
CREATE INDEX rastreo_ciudad_FKIndex1 ON rastreo_ciudad (id_distrito, id_pais); 
 


 
CREATE TABLE rastreogps_reportes ( 
  idrastreogps_reportes SERIAL NOT NULL, 
  id_equipogps INTEGER NOT NULL, 
  gps_longitud DOUBLE PRECISION NULL, 
  gps_latitud DOUBLE PRECISION NULL, 
  gps_fechahora_reporte TIMESTAMP DEFAULT  now() NULL, 
  gps_velocidad INTEGER NULL, 
  gps_rumbo INTEGER NULL, 
  gps_evento VARCHAR(32) NULL, 
  gps_edaddeldato VARCHAR(32) NULL, 
  gps_hdop VARCHAR(32) NULL, 
  gps_satstatus VARCHAR(32) NULL, 
  gps_gsmstatus VARCHAR(32) NULL, 
  gps_estado_io VARCHAR(32) NULL, 
  gps_tipodeposicion VARCHAR(32) NULL, 
  gps_ip VARCHAR(128) NULL 
); 
ALTER TABLE rastreogps_reportes ADD CONSTRAINT rastreogps_reportes_PK PRIMARY KEY (idrastreogps_reportes); 
 


 
CREATE TABLE rastreogps_panico ( 
  idrastreogps_panico SERIAL NOT NULL, 
  id_equipogps INTEGER NOT NULL, 
  panico_fechahora TIMESTAMP DEFAULT  now() NULL, 
  atendido BOOL DEFAULT  false NULL 
); 
ALTER TABLE rastreogps_panico ADD CONSTRAINT rastreogps_panico_PK PRIMARY KEY (idrastreogps_panico); 
 
CREATE INDEX rastreogps_panico_FKIndex1 ON rastreogps_panico (id_equipogps); 
 


 
CREATE TABLE rastreogps_tipoevento ( 
  idrastreogps_tipo_evento SERIAL NOT NULL, 
  id_tipoequipo INTEGER NOT NULL, 
  evento VARCHAR(32) NULL, 
  descripcion VARCHAR(1024) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_tipoevento ADD CONSTRAINT rastreogps_tipoevento_PK PRIMARY KEY (idrastreogps_tipo_evento); 
 
CREATE INDEX rastreogps_tipo_evento_FKIndex1 ON rastreogps_tipoevento (id_tipoequipo); 
 


 
CREATE TABLE rastreo_historial ( 
  idrastreo_historial SERIAL NOT NULL, 
  id_cliente INTEGER NOT NULL, 
  vehiculos_involucrados TEXT NULL, 
  observacion TEXT NULL, 
  fecha_verificacion TIMESTAMP NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_historial ADD CONSTRAINT rastreo_historial_PK PRIMARY KEY (idrastreo_historial, id_cliente); 
 
CREATE INDEX rastreo_vehiculo_historial_FKIndex1 ON rastreo_historial (id_cliente); 
 


 
CREATE TABLE rastreo_proviene_de_seguro ( 
  idrastreo_proviene_de_seguro SERIAL NOT NULL, 
  descripcion_del_seguro VARCHAR(512) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_proviene_de_seguro ADD CONSTRAINT rastreo_proviene_de_seguro_PK PRIMARY KEY (idrastreo_proviene_de_seguro); 
 


 
CREATE TABLE rastreogps_cola_de_comandos ( 
  idrastreogps_cola_de_comandos SERIAL NOT NULL, 
  idequipogps INTEGER NOT NULL, 
  comando VARCHAR(1024) NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL 
); 
ALTER TABLE rastreogps_cola_de_comandos ADD CONSTRAINT rastreogps_cola_de_comandos_PK PRIMARY KEY (idrastreogps_cola_de_comandos); 
 
CREATE INDEX rastreogps_cola_de_comandos_FKIndex1 ON rastreogps_cola_de_comandos (idequipogps); 
 


 
CREATE TABLE rastreo_vehiculo_fotos ( 
  idrastreo_vehiculo_fotos SERIAL NOT NULL, 
  idvehiculo INTEGER NOT NULL, 
  idcliente INTEGER NOT NULL, 
  descripcion_foto VARCHAR(1024) NULL, 
  nombre_archivo VARCHAR(1024) NULL, 
  tipo_archivo VARCHAR(512) NULL, 
  foto bytea NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP DEFAULT  now() NULL 
); 
ALTER TABLE rastreo_vehiculo_fotos ADD CONSTRAINT rastreo_vehiculo_fotos_PK PRIMARY KEY (idrastreo_vehiculo_fotos); 
 
CREATE INDEX rastreo_vehiculo_fotos_FKIndex1 ON rastreo_vehiculo_fotos (idvehiculo, idcliente); 
 


 
CREATE TABLE rastreo_cliente_documentos ( 
  idrastreo_cliente_documentos SERIAL NOT NULL, 
  idcliente INTEGER NOT NULL, 
  descripcion VARCHAR(1024) NULL, 
  tipo_archivo VARCHAR(512) NULL, 
  nombre_archivo VARCHAR(1024) NULL, 
  documento bytea NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP DEFAULT  now() NULL 
); 
ALTER TABLE rastreo_cliente_documentos ADD CONSTRAINT rastreo_cliente_documentos_PK PRIMARY KEY (idrastreo_cliente_documentos); 
 
CREATE INDEX rastreo_cliente_documentos_FKIndex1 ON rastreo_cliente_documentos (idcliente); 
 


 
CREATE TABLE rastreogps_equipo_eventos ( 
  id_equipogps INTEGER NOT NULL, 
  id_tipoevento INTEGER NOT NULL, 
  habilitado BOOL DEFAULT  TRUE NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreogps_equipo_eventos ADD CONSTRAINT rastreogps_equipo_eventos_PK PRIMARY KEY (id_equipogps, id_tipoevento); 
 
CREATE INDEX rastreogps_equipo_eventos_FKIndex1 ON rastreogps_equipo_eventos (id_equipogps); 
 
CREATE INDEX rastreogps_equipo_eventos_FKIndex2 ON rastreogps_equipo_eventos (id_tipoevento); 
 


 
CREATE TABLE rastreogps_odometro ( 
  idrastreogps_odometro SERIAL NOT NULL, 
  id_equipogps INTEGER NOT NULL, 
  odometro INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL 
); 
ALTER TABLE rastreogps_odometro ADD CONSTRAINT rastreogps_odometro_PK PRIMARY KEY (idrastreogps_odometro, id_equipogps); 
 
CREATE INDEX rastreogps_odometro_FKIndex1 ON rastreogps_odometro (id_equipogps); 
 


 
CREATE TABLE rastreogps_fuel ( 
  idrastreogps_fuel SERIAL NOT NULL, 
  id_equipogps INTEGER NOT NULL, 
  fuel VARCHAR(64) NULL, 
  consumo VARCHAR(64) NULL, 
  estado BOOL NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL 
); 
ALTER TABLE rastreogps_fuel ADD CONSTRAINT rastreogps_fuel_PK PRIMARY KEY (idrastreogps_fuel, id_equipogps); 
 
CREATE INDEX rastreogps_fuel_FKIndex1 ON rastreogps_fuel (id_equipogps); 
 


 
CREATE TABLE rastreo_geocercas ( 
  idrastreo_geocercas SERIAL NOT NULL, 
  id_cliente INTEGER NOT NULL, 
  id_vehiculo INTEGER NOT NULL, 
  descripcion VARCHAR(128) NULL, 
  puntos TEXT NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_geocercas ADD CONSTRAINT rastreo_geocercas_PK PRIMARY KEY (idrastreo_geocercas); 
 
CREATE INDEX rastreo_geocercas_FKIndex1 ON rastreo_geocercas (id_vehiculo, id_cliente); 
 


 
CREATE TABLE rastreo_template_hoja_de_ruta ( 
  id_recorridotemplate SERIAL NOT NULL, 
  id_persona INTEGER NOT NULL, 
  id_usuarios INTEGER NOT NULL, 
  descripcion_recorrido TEXT NULL, 
  publico BOOL NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_template_hoja_de_ruta ADD CONSTRAINT rastreo_template_hoja_de_ruta_PK PRIMARY KEY (id_recorridotemplate); 
 
CREATE INDEX rastreo_template_hoja_de_ruta_FKIndex1 ON rastreo_template_hoja_de_ruta (id_usuarios); 
 
CREATE INDEX rastreo_template_hoja_de_ruta_FKIndex2 ON rastreo_template_hoja_de_ruta (id_persona); 
 


 
CREATE TABLE rastreo_hoja_de_ruta_puntos ( 
  id_punto SERIAL NOT NULL, 
  idhoja_de_ruta INTEGER NOT NULL, 
  nombre VARCHAR(128) NULL, 
  descripcion TEXT NULL, 
  orden INTEGER NULL, 
  lon DOUBLE PRECISION NULL, 
  lat DOUBLE PRECISION NULL, 
  hora_llegada TIME NULL, 
  minutos_demora INTEGER NULL 
); 
ALTER TABLE rastreo_hoja_de_ruta_puntos ADD CONSTRAINT rastreo_hoja_de_ruta_puntos_PK PRIMARY KEY (id_punto); 
 
CREATE INDEX rastreo_hoja_de_ruta_puntos_FKIndex1 ON rastreo_hoja_de_ruta_puntos (idhoja_de_ruta); 
 


 
CREATE TABLE rastreo_referencias ( 
  idrastreo_referencias SERIAL NOT NULL, 
  id_cliente INTEGER NOT NULL, 
  nombre VARCHAR(128) NULL, 
  lon DOUBLE PRECISION NULL, 
  lat DOUBLE PRECISION NULL, 
  descripcion INTEGER NULL, 
  visible BOOL NULL, 
  user_in INTEGER NULL, 
  fech_ins TIMESTAMP NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_referencias ADD CONSTRAINT rastreo_referencias_PK PRIMARY KEY (idrastreo_referencias); 
 
CREATE INDEX rastreo_referencias_FKIndex1 ON rastreo_referencias (id_cliente); 
 


 
CREATE TABLE rastreo_hoja_de_ruta ( 
  idhoja_de_ruta SERIAL NOT NULL, 
  id_usuario INTEGER NOT NULL, 
  descripcion TEXT NULL, 
  activa BOOL NULL, 
  hora_activacion TIMESTAMP NULL, 
  hora_desactivacion TIMESTAMP NULL, 
  user_ins INTEGER NULL, 
  fech_ins TIMESTAMP DEFAULT  now() NULL, 
  user_upd INTEGER NULL, 
  fech_upd TIMESTAMP NULL 
); 
ALTER TABLE rastreo_hoja_de_ruta ADD CONSTRAINT rastreo_hoja_de_ruta_PK PRIMARY KEY (idhoja_de_ruta); 
 
CREATE INDEX rastreo_hoja_de_ruta_FKIndex1 ON rastreo_hoja_de_ruta (id_usuario); 
 


 
CREATE TABLE rastreo_template_puntos_hoja_de_ruta ( 
  id_puntostemplate SERIAL NOT NULL, 
  id_recorridotemplate INTEGER NOT NULL, 
  nombre VARCHAR(128) NULL, 
  descripcion TEXT NULL, 
  lon DOUBLE PRECISION NULL, 
  lat DOUBLE PRECISION NULL 
); 
ALTER TABLE rastreo_template_puntos_hoja_de_ruta ADD CONSTRAINT rastreo_template_puntos_hoja_de_ruta_PK PRIMARY KEY (id_puntostemplate); 
 
CREATE INDEX rastreo_template_puntos_hoja_de_ruta_FKIndex1 ON rastreo_template_puntos_hoja_de_ruta (id_recorridotemplate); 
 


 
CREATE TABLE rastreo_usuario_lista_vehiculo ( 
  id_listavehiculos SERIAL NOT NULL, 
  id_usuarios INTEGER NOT NULL, 
  id_cliente INTEGER NOT NULL, 
  id_vehiculo INTEGER NOT NULL, 
  visible BOOL DEFAULT  TRUE NULL 
); 
ALTER TABLE rastreo_usuario_lista_vehiculo ADD CONSTRAINT rastreo_usuario_lista_vehiculo_PK PRIMARY KEY (id_listavehiculos, id_usuarios); 
 
CREATE INDEX rastreo_usuario_lista_vehiculo_FKIndex1 ON rastreo_usuario_lista_vehiculo (id_usuarios); 
 
CREATE INDEX rastreo_usuario_lista_vehiculo_FKIndex2 ON rastreo_usuario_lista_vehiculo (id_vehiculo, id_cliente); 
 


 
CREATE TABLE rastreo_hojaderuta_has_vehiculo ( 
  id_cliente INTEGER NOT NULL, 
  id_vehiculo INTEGER NOT NULL, 
  idhoja_de_ruta INTEGER NOT NULL 
); 
ALTER TABLE rastreo_hojaderuta_has_vehiculo ADD CONSTRAINT rastreo_hojaderuta_has_vehiculo_PK PRIMARY KEY (id_cliente, id_vehiculo, idhoja_de_ruta); 
 
CREATE INDEX rastreo_hojaderuta_has_vehiculo_FKIndex1 ON rastreo_hojaderuta_has_vehiculo (id_vehiculo, id_cliente); 
 
CREATE INDEX rastreo_hojaderuta_has_vehiculo_FKIndex2 ON rastreo_hojaderuta_has_vehiculo (idhoja_de_ruta); 
 










































 
ALTER TABLE rastreo_persona ADD CONSTRAINT rastreorel_tipodoc_persona FOREIGN KEY (tipo_documento) REFERENCES rastreo_tipo_documento(idrastreo_tipo_documento);
 
ALTER TABLE rastreo_cliente ADD CONSTRAINT rastreorel_persona_cliente FOREIGN KEY (id_cliente) REFERENCES rastreo_persona(idrastreo_persona) ON DELETE CASCADE;
 
ALTER TABLE rastreo_usuarios ADD CONSTRAINT rastreorel_persona_usuario FOREIGN KEY (id_persona) REFERENCES rastreo_persona(idrastreo_persona) ON DELETE CASCADE;
 
ALTER TABLE rastreo_empleados ADD CONSTRAINT rastreorel_persona_empleado FOREIGN KEY (id_empleado) REFERENCES rastreo_persona(idrastreo_persona);
 
ALTER TABLE rastreo_cliente_contactos ADD CONSTRAINT rastreorel_cliente_contactos FOREIGN KEY (rastreo_cliente_id_cliente) REFERENCES rastreo_cliente(id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_modelo_vehiculo ADD CONSTRAINT rastreorel_marca_modelo FOREIGN KEY (id_marca) REFERENCES rastreo_marca_vehiculo(idrastreo_marca_vehiculo) ON DELETE CASCADE;
 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreorel_tipo_vehiculo FOREIGN KEY (tipo_vehiculo) REFERENCES rastreo_tipo_vehiculo(idrastreo_tipo_vehiculo);
 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreorel_marca_modelo_vehiculo FOREIGN KEY (modelo, marca) REFERENCES rastreo_modelo_vehiculo(idrastreo_modelo_vehiculo, id_marca);
 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreorel_equipo_vehiculo FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps);
 
ALTER TABLE rastreogps_usuario_opciones ADD CONSTRAINT rastreorel_usu_opc1 FOREIGN KEY (id_usuario) REFERENCES rastreo_usuarios(idrastreo_usuarios) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_usuario_opciones ADD CONSTRAINT rastreorel_usu_opc2 FOREIGN KEY (opcion_de_pantalla) REFERENCES rastreogps_opciones_de_pantalla(idrastreogps_opciones_de_pantalla) ON DELETE CASCADE;
 
ALTER TABLE rastreo_distrito ADD CONSTRAINT rastreorel_pais_distrito FOREIGN KEY (id_pais) REFERENCES rastreo_pais(idrastreo_pais);
 
ALTER TABLE rastreo_ciudad ADD CONSTRAINT rastreo_distrito_ciudad FOREIGN KEY (id_distrito, id_pais) REFERENCES rastreo_distrito(idrastreo_distrito, id_pais);
 
ALTER TABLE rastreo_persona ADD CONSTRAINT rastreorel_persona_pais_distrito_ciudad FOREIGN KEY (id_ciudad, id_pais, id_distrito) REFERENCES rastreo_ciudad(idrastreo_ciudad, id_pais, id_distrito);
 
ALTER TABLE rastreogps_reportes ADD CONSTRAINT rastreorel_equipo_reporte FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps);
 
ALTER TABLE rastreogps_bandeja_de_entrada ADD CONSTRAINT rastreorel_equipo_bandeja_de_entrada FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps);
 
ALTER TABLE rastreogps_panico ADD CONSTRAINT rastreorel_equipo_panico FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_equipogps ADD CONSTRAINT rastreorel_tipo_equipo FOREIGN KEY (tipoequipo) REFERENCES rastreogps_tipoequipo(idrastreogps_tipoequipo);
 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreo_cliente_vehiculos FOREIGN KEY (id_cliente) REFERENCES rastreo_cliente(id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_tipoevento ADD CONSTRAINT rastreorel_tipoequipo_evento FOREIGN KEY (id_tipoequipo) REFERENCES rastreogps_tipoequipo(idrastreogps_tipoequipo);
 
ALTER TABLE rastreogps_equipogps ADD CONSTRAINT rastreorel_sim_equipogps FOREIGN KEY (id_simcard) REFERENCES rastreogps_simcards(idrastreogps_simcards);
 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreorel_seguro_proviene_de FOREIGN KEY (proviene_de) REFERENCES rastreo_proviene_de_seguro(idrastreo_proviene_de_seguro);
 
ALTER TABLE rastreogps_cola_de_comandos ADD CONSTRAINT rastreogpsrel_equipo_comando FOREIGN KEY (idequipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps) ON DELETE CASCADE;
 
ALTER TABLE rastreo_vehiculo_fotos ADD CONSTRAINT rastreorel_vehiculo_fotos FOREIGN KEY (idvehiculo, idcliente) REFERENCES rastreo_vehiculo(idrastreo_vehiculo, id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_cliente_documentos ADD CONSTRAINT rastreorel_cliente_documentos FOREIGN KEY (idcliente) REFERENCES rastreo_cliente(id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_equipo_eventos ADD CONSTRAINT rastreorel_equipo_eventos FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_equipo_eventos ADD CONSTRAINT rastreorel_tipoevento_evento FOREIGN KEY (id_tipoevento) REFERENCES rastreogps_tipoevento(idrastreogps_tipo_evento) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_odometro ADD CONSTRAINT rastreorel_equipo_odometro FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps) ON DELETE CASCADE;
 
ALTER TABLE rastreogps_fuel ADD CONSTRAINT rastreorel_equipo_fuel FOREIGN KEY (id_equipogps) REFERENCES rastreogps_equipogps(idrastreogps_equipogps) ON DELETE CASCADE;
 
ALTER TABLE rastreo_historial ADD CONSTRAINT rastreorel_historial_cliente FOREIGN KEY (id_cliente) REFERENCES rastreo_cliente(id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_referencias ADD CONSTRAINT rastreorel_cliente_referencias FOREIGN KEY (id_cliente) REFERENCES rastreo_cliente(id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_vehiculo ADD CONSTRAINT rastreorel_empleado_vehiculo FOREIGN KEY (id_instalador) REFERENCES rastreo_empleados(id_empleado);
 
ALTER TABLE rastreo_geocercas ADD CONSTRAINT rastreorel_vehiculo_geocerca FOREIGN KEY (id_vehiculo, id_cliente) REFERENCES rastreo_vehiculo(idrastreo_vehiculo, id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_hoja_de_ruta ADD CONSTRAINT rastreo_hoja_de_ruta1 FOREIGN KEY (id_usuario) REFERENCES rastreo_usuarios(idrastreo_usuarios) ON DELETE CASCADE;
 
ALTER TABLE rastreo_template_puntos_hoja_de_ruta ADD CONSTRAINT template_hoja_de_ruta2 FOREIGN KEY (id_recorridotemplate) REFERENCES rastreo_template_hoja_de_ruta(id_recorridotemplate) ON DELETE CASCADE;
 
ALTER TABLE rastreo_template_hoja_de_ruta ADD CONSTRAINT template_hoja_de_ruta1 FOREIGN KEY (id_usuarios) REFERENCES rastreo_usuarios(idrastreo_usuarios) ON DELETE CASCADE;
 
ALTER TABLE rastreo_hoja_de_ruta_puntos ADD CONSTRAINT rastreo_hoja_de_ruta1 FOREIGN KEY (idhoja_de_ruta) REFERENCES rastreo_hoja_de_ruta(idhoja_de_ruta) ON DELETE CASCADE;
 
ALTER TABLE rastreo_usuario_lista_vehiculo ADD CONSTRAINT rastreorel_usuario_listavehiculos FOREIGN KEY (id_usuarios) REFERENCES rastreo_usuarios(idrastreo_usuarios) ON DELETE CASCADE;
 
ALTER TABLE rastreo_usuario_lista_vehiculo ADD CONSTRAINT rastreorel_listavehiculo_vehiculo FOREIGN KEY (id_vehiculo, id_cliente) REFERENCES rastreo_vehiculo(idrastreo_vehiculo, id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_hojaderuta_has_vehiculo ADD CONSTRAINT rastreo_vehiculo_tablaweakref1 FOREIGN KEY (id_vehiculo, id_cliente) REFERENCES rastreo_vehiculo(idrastreo_vehiculo, id_cliente) ON DELETE CASCADE;
 
ALTER TABLE rastreo_hojaderuta_has_vehiculo ADD CONSTRAINT rastreo_hijaderuta_tablaweakref1 FOREIGN KEY (idhoja_de_ruta) REFERENCES rastreo_hoja_de_ruta(idhoja_de_ruta) ON DELETE CASCADE;
 
ALTER TABLE rastreo_template_hoja_de_ruta ADD CONSTRAINT rastreo_persona_tempalte_hdr1 FOREIGN KEY (id_persona) REFERENCES rastreo_persona(idrastreo_persona) ON DELETE CASCADE;
 























 
comment on table rastreo_historial is 'Aqui tengo que crear me salga una lista de vehiculos del cliente y de ahi ir seleccionando, guardando en vehiculos_involucrados cada uno de ellos separados por , o ;';








 
comment on table rastreo_template_hoja_de_ruta is 'Esta tabla es un template de reocrrido definido por el usuario FK para aplicar a cualquier vehiculo';
 
comment on table rastreo_hoja_de_ruta_puntos is 'Esta tabla contiene los puntos del recorrido creado por el usuario';




































































































































































 
comment on column rastreo_empleados.instalador is 'Indica si es un instalador';



























































































































































































































































































































































































































































































































































































































































































































































































 
comment on column rastreogps_fuel.estado is 'estado de conexion del sensor de combustible';

























 
comment on column rastreo_geocercas.puntos is 'Deben ser separados por punto y coma cada uno de los puntos, orden lon;lat;lon;lat;lon;lat';




























 
comment on column rastreo_template_hoja_de_ruta.publico is 'Este boolean indica si el recorrido es visible o no para los demas usuarios del cliente';

















































































































































































































 

-- End of generated script 
















