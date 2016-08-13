
-- Function: rstrigger_rastreogps_reportes_update_bandeja_entrada()

--DROP FUNCTION IF EXISTS rstrigger_rastreogps_reportes_update_bandeja_entrada() CASCADE;
CREATE OR REPLACE FUNCTION rstrigger_rastreogps_reportes_update_bandeja_entrada()
  RETURNS trigger AS
$BODY$
DECLARE
	changed integer;
	date_report timestamp without time zone;
	p_evento text;
	observacion text;
BEGIN
  if (new.gps_evento is not null) then	
	p_evento := (SELECT evento FROM rsview_equipo_eventos 
			WHERE evento_raw = new.gps_evento
			AND id_rastreoequipogps = new.id_equipogps LIMIT 1);
  end if;
  if (new.gps_obs IS NOT NULL) then 
	observacion := new.gps_obs;
  end if;
  if (upper(p_evento) ILIKE '%IDA%' OR upper(p_evento) ILIKE '%VUELTA%') then
	observacion := p_evento;
  end if;  
  if tg_op = 'INSERT' then
	changed := (SELECT id_equipogps FROM rastreogps_bandeja_de_entrada 
	WHERE rastreogps_bandeja_de_entrada.id_equipogps = new.id_equipogps);
	if (changed is null) then
		INSERT INTO rastreogps_bandeja_de_entrada(
		    id_equipogps, gps_longitud, 
		    gps_latitud, gps_fecha, gps_velocidad, gps_rumbo, gps_evento, 
		    gps_edaddeldato, gps_hdop, gps_satstatus, gps_gsmstatus, gps_estado_io, 
		    gps_tipodeposicion, gps_ip, gps_obs, gps_dir )
		VALUES (new.id_equipogps, new.gps_longitud, new.gps_latitud, 
		    new.gps_fechahora_reporte, new.gps_velocidad, new.gps_rumbo, new.gps_evento, new.gps_edaddeldato, 
		    new.gps_hdop, new.gps_satstatus, new.gps_gsmstatus, new.gps_estado_io, new.gps_tipodeposicion,
		    new.gps_ip, observacion, new.gps_dir);
	else
		date_report = (SELECT gps_fecha FROM rastreogps_bandeja_de_entrada WHERE id_equipogps = new.id_equipogps);
		if (date_report <= new.gps_fechahora_reporte) then
			if (observacion IS NOT NULL) then
				UPDATE rastreogps_bandeja_de_entrada
				SET gps_longitud=new.gps_longitud, gps_latitud=new.gps_latitud, gps_fecha=new.gps_fechahora_reporte, 
					gps_velocidad=new.gps_velocidad, gps_rumbo=new.gps_rumbo, gps_evento=new.gps_evento, 
					gps_edaddeldato=new.gps_edaddeldato, gps_hdop=new.gps_hdop, gps_satstatus=new.gps_satstatus, 
					gps_gsmstatus=new.gps_gsmstatus, gps_estado_io=new.gps_estado_io, gps_tipodeposicion=new.gps_tipodeposicion, 
					gps_ip=new.gps_ip, gps_obs=observacion, gps_dir = new.gps_dir
				WHERE id_equipogps = new.id_equipogps;
			else
				UPDATE rastreogps_bandeja_de_entrada
				SET gps_longitud=new.gps_longitud, gps_latitud=new.gps_latitud, gps_fecha=new.gps_fechahora_reporte, 
					gps_velocidad=new.gps_velocidad, gps_rumbo=new.gps_rumbo, gps_evento=new.gps_evento, 
					gps_edaddeldato=new.gps_edaddeldato, gps_hdop=new.gps_hdop, gps_satstatus=new.gps_satstatus, 
					gps_gsmstatus=new.gps_gsmstatus, gps_estado_io=new.gps_estado_io, gps_tipodeposicion=new.gps_tipodeposicion, 
					gps_ip=new.gps_ip, gps_dir = new.gps_dir
				WHERE id_equipogps = new.id_equipogps;
			end if;
		end if;
	end if;
	if (upper(p_evento) ILIKE '%P%NIC%' 
	 OR upper(p_evento) ILIKE '%BAT%DESC%'
	 OR (upper(p_evento) ILIKE '%ANTI%' AND upper(p_evento) NOT ILIKE '%DESACT%')
	 OR upper(p_evento) ILIKE '%ALERT%GEO%'
	 OR upper(p_evento) ILIKE '%EXCESO%') then
		INSERT INTO rastreogps_avisos(id_equipogps, evento_fechahora, evento, atendido, gps_fechahora_reporte, gps_latitud, gps_longitud, gps_velocidad, gps_rumbo, gps_dir)
			VALUES(new.id_equipogps, new.gps_fechahora_reporte, p_evento, false, new.gps_fechahora_reporte, new.gps_latitud, new.gps_longitud, new.gps_velocidad, new.gps_rumbo, new.gps_dir);
	end if;
  end if;
/*
  if tg_op = 'UPDATE' then
	
  end if;
*/
  RETURN new;
END
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION rstrigger_rastreogps_reportes_update_bandeja_entrada() OWNER TO rastreo_admin;

/*
CREATE TRIGGER rstrigger_rastreogps_reportes_update_bandeja_entrada AFTER INSERT ON reportesgps121969
	FOR EACH ROW EXECUTE PROCEDURE rstrigger_rastreogps_reportes_update_bandeja_entrada();
*/

-- Function: rstrigger_update_eventos()
/*
DROP FUNCTION IF EXISTS rstrigger_update_eventos() CASCADE;

CREATE OR REPLACE FUNCTION rstrigger_update_eventos()
  RETURNS trigger AS
$BODY$
BEGIN
  if tg_op = 'INSERT' OR tg_op = 'UPDATE' then
	UPDATE rastreogps_equipo_eventos SET evento = (SELECT tipoevento_evento FROM rsview_equipo_eventos 
		WHERE id_rastreoequipogps = rastreogps_equipo_eventos.id_equipogps 
		AND id_tipoevento = rastreogps_equipo_eventos.id_tipoevento)
	WHERE rastreogps_equipo_eventos.evento IS NULL;
  end if;
  RETURN new;
END
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;
ALTER FUNCTION rstrigger_rastreogps_reportes_update_bandeja_entrada() OWNER TO rastreo_admin;

CREATE TRIGGER rstrigger_update_eventos AFTER INSERT OR UPDATE ON rastreogps_tipoevento
	FOR EACH ROW EXECUTE PROCEDURE rstrigger_update_eventos();
*/