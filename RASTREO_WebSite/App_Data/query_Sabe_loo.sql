select distinct on (gps_fecha, identificador_rastreo) gps_Fecha, identificador_rastreo, evento, id_equipo_gps 
from rsview_vehiculo_bandejaentrada_cliente_equipogps  
where gps_fecha >= to_char(now(),'dd/MM/yyyy HH24:mi')::timestamp
order by gps_fecha desc, identificador_rastreo asc;


--SELECT gps_fecha, (SELECT DISTINCT count(gps_fecha) FROM rsview_vehiculo_bandejaentrada_cliente_equipogps WHERE gps_fecha >= to_char(now(), 'dd/MM/yyyy HH24:mi')::timestamp) as count FROM rsview_vehiculo_bandejaentrada_cliente_equipogps ORDER BY gps_fecha DESC LIMIT 1