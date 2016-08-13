--------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------
-- Inserciones standard para las tablas de RASTREO System
-- Autor: Christian Melgarejo Bresanovich
-- Contacto: +595981566234 
----- mail: camb24@gmail.com - camb21@hotmail.com
--------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------

INSERT INTO rastreo_pais(
            pais, user_ins, fech_ins)
    VALUES ('PARAGUAY', 1, now());

INSERT INTO rastreo_distrito(
            id_pais, distrito, user_ins, fech_ins)
    VALUES (1, 'CENTRAL', 1, now());
    
INSERT INTO rastreo_ciudad(
            id_pais, id_distrito,ciudad, user_ins, fech_ins)
    VALUES (1, 1, 'ASUNCION', 1, now());
    
INSERT INTO rastreo_tipo_documento(
            descripcion, user_ins, fech_ins)
    VALUES ('CEDULA DE IDENTIDAD PARAGUAYA', 1, now());

--------------------------------------------------------------------------------------------
-----------------------Insercion de opcione de permisos para usuario------------------------
--------------------------------------------------------------------------------------------
-- Inserto un usuario empleado por defecto, para propositos de carga e inicialización de 
-- datos. conejillo de indias: Diego Recalde Dos Santos

INSERT INTO rastreo_persona(
            id_distrito, id_pais, id_ciudad, tipo_documento, 
            tipo_persona, nro_documento, nombre, apellido, 
            direccion, email, telefonos, user_ins, fech_ins)
    VALUES (1, 1, 1, 1, 
	    'F', '123', 'DIEGO EDUARDO', 'RECALDE DOS SANTOS',  
            'DIRECCION', 'dieguitoeduardo@hotmail.com', '097?', 1, now());

INSERT INTO rastreo_empleados(
            id_empleado, acceso_sistema, estado_empleado, estado_fecha, user_ins, fech_ins)
    VALUES (1, TRUE, TRUE, now(), 1, now()); 

INSERT INTO rastreo_usuarios(
            id_persona, usuario, pass, su, user_ins, fech_ins)
    VALUES (1, 'diego', '00e48a815525529ba9d33f8761a167588fe00c47bc82f515cf791c482ed99ecc', TRUE, 
            1, now()); --superusuario!

--HACER STANDARD INSERT DE LOS TIPO DE VEHICULO!!!


--------------------------------------------------------------------------------------------
-----------------------Insercion de opcione de permisos para usuario------------------------
--------------------------------------------------------------------------------------------
-- Son 4 opciones: A - B - M - C : Altas, Bajas, Modificaciones y Consultas respectivamente, 
-- se pone la opcion de modulo y lugo separado por un guion una opcion de estas 4. ej:
-- 'CONTACTOS-A'

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('MENU_ADMINISTRADOR', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('MENU_USUARIOS', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCION_HOJADERUTA', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCION_GEOCERCA', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCION_REFERENCIAS', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCION_BUSES', 1, now());
            
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('CONTACTOS_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('CONTACTOS_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('CONTACTOS_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('CONTACTOS_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('DOCUMENTOSPERSONA_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('DOCUMENTOSPERSONA_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('DOCUMENTOSPERSONA_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('DOCUMENTOSPERSONA_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('FOTOSVEHICULO_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('FOTOSVEHICULO_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('FOTOSVEHICULO_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('FOTOSVEHICULO_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('VEHICULOS_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('VEHICULOS_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('VEHICULOS_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('VEHICULOS_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('PERSONAS_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('PERSONAS_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('PERSONAS_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('PERSONAS_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EQUIPOSGPS_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EQUIPOSGPS_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EQUIPOSGPS_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EQUIPOSGPS_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EVENTOSGPS_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EVENTOSGPS_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EVENTOSGPS_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('EVENTOSGPS_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESUSUARIO_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESUSUARIO_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESUSUARIO_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESUSUARIO_ADMIN-C', 1, now());


INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESPANTALLA_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESPANTALLA_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESPANTALLA_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('OPCIONESPANTALLA_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('HISTORIAL_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('HISTORIAL_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('HISTORIAL_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('HISTORIAL_ADMIN-C', 1, now());

INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('USUARIOS_ADMIN-A', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('USUARIOS_ADMIN-B', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('USUARIOS_ADMIN-M', 1, now());
INSERT INTO rastreogps_opciones_de_pantalla(opcion_de_pantalla, user_ins, fech_ins)
    VALUES ('USUARIOS_ADMIN-C', 1, now());

--------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------

--------------------------------------------------------------------------------------------
---------------------Inserciones standard de tipo equipo RASTREO----------------------------
--------------------------------------------------------------------------------------------
INSERT INTO rastreogps_tipoequipo(
            tipo_equipo, tipo_de_reporte, user_ins, fech_ins)
    VALUES ('TRAXS4', 'RGPTRAX', 1, now());
    
INSERT INTO rastreogps_tipoequipo(
            tipo_equipo, tipo_de_reporte, user_ins, fech_ins)
    VALUES ('VIRLOC', 'RGP', 1, now());

INSERT INTO rastreogps_tipoequipo(
            tipo_equipo, tipo_de_reporte, user_ins, fech_ins)
    VALUES ('T-1000', 'RGP', 1, now());
    
INSERT INTO rastreogps_tipoequipo(
            tipo_equipo, tipo_de_reporte, user_ins, fech_ins)
    VALUES ('TK-1000', 'TK1000', 1, now());
        
INSERT INTO rastreogps_tipoequipo(
            tipo_equipo, tipo_de_reporte, user_ins, fech_ins)
    VALUES ('G831', 'G831', 1, now());



 --------------------------------------------------------------------------------------------
 --------------------------------------------------------------------------------------------
 --------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------
---------------Inserciones standard de tipo evento de cada equipo RASTREO--------------------
---------------------------------------------------------------------------------------------

 INSERT INTO rastreogps_tipoevento(
            id_tipoequipo, evento, descripcion, 
            user_ins)
    VALUES (1, '00', 'REPORTE NORMAL', 0);
    
 INSERT INTO rastreogps_tipoevento(
            id_tipoequipo, evento, descripcion, 
            user_ins)
    VALUES (1, '05', 'PANICO', 0);
    
 INSERT INTO rastreogps_tipoevento(
            id_tipoequipo, evento, descripcion, 
            user_ins)
    VALUES (2, '00', 'REPORTE NORMAL', 0);
    
 INSERT INTO rastreogps_tipoevento(
            id_tipoequipo, evento, descripcion, 
            user_ins)
    VALUES (2, '05', 'PANICO', 0);
    
 INSERT INTO rastreogps_tipoevento(
            id_tipoequipo, evento, descripcion, 
            user_ins)
    VALUES (3, '00', 'REPORTE NORMAL', 0);
    
 INSERT INTO rastreogps_tipoevento(
            id_tipoequipo, evento, descripcion, 
            user_ins)
    VALUES (3, '05', 'PANICO', 0);
