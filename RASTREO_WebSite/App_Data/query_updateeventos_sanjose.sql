UPDATE rastreogps_equipo_eventos SET evento = 'LLEGADA SAJONIA' 
WHERE evento = 'LLEGADA 1'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'SALIDA SAJONIA' 
WHERE evento = 'SALIDA 1'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'LLEGADA A COLONIA' 
WHERE evento = 'LLEGADA 2'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'SALIDA DE COLONIA' 
WHERE evento = 'SALIDA 2'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'LLEGADA A SALADO' 
WHERE evento = 'LLEGADA 3'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'SALIDA DE SALADO' 
WHERE evento = 'SALIDA 3'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'LLEGADA A VILLA JARDIN' 
WHERE evento = 'LLEGADA 4'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'SALIDA DE VILLA JARDIN' 
WHERE evento = 'SALIDA 4'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'LLEGADA A PIQUETE' 
WHERE evento = 'LLEGADA 5'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'SALIDA DE PIQUETE' 
WHERE evento = 'SALIDA 5'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'CONTROL BOTANICO' 
WHERE evento = 'CONTROL 1'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'CONTROL REMANSO' 
WHERE evento = 'CONTROL 2'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'CONTROL LIMPIO VUELTA' 
WHERE evento = 'CONTROL 3'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'CONTROL RINCON' 
WHERE evento = 'CONTROL 4'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);

UPDATE rastreogps_equipo_eventos SET evento = 'CONTROL LIMPIO IDA' 
WHERE evento = 'CONTROL 5'
AND id_equipogps IN (SELECT id_equipogps FROM rastreo_vehiculo WHERE id_cliente = 366);
