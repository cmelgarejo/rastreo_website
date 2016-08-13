DROP FUNCTION IF EXISTS rastreofx_addFoto(int,int,varchar,varchar,varchar,bytea,int);

CREATE OR REPLACE FUNCTION rastreofx_addFoto(idc int, idv int, descrip varchar, nombrearchivo varchar, tipoarchivo varchar, mifoto bytea, usuario int) RETURNS BOOLEAN AS $$
DECLARE
	idexiste integer;
BEGIN
	idexiste := (SELECT idrastreo_vehiculo_fotos FROM rastreo_vehiculo_fotos WHERE idvehiculo = idv AND idcliente = idc AND nombre_archivo = nombrearchivo AND user_ins IS NOT NULL);
	IF idexiste > 0 then
		UPDATE rastreo_vehiculo_fotos SET idcliente = $1, idvehiculo = $2, descripcion_foto = $3, nombre_archivo = $4, tipo_archivo = $5, 
		foto = $6, user_upd = $7, fech_upd = now();
		RETURN TRUE;
	END IF;
	INSERT INTO rastreo_vehiculo_fotos(idcliente, idvehiculo, descripcion_foto, nombre_archivo, tipo_archivo, foto, user_ins, user_upd, fech_ins, fech_upd)
	VALUES($1,$2,$3,$4,$5,$6,$7,NULL,now(),now());
	RETURN TRUE;
END;
$$ LANGUAGE plpgsql;

--SELECT rastreofx_addFoto(1,2,'lol','lol.txt','type/lol','\\000'::bytea,1);

DROP FUNCTION IF EXISTS rastreofx_getFoto(bigint);

CREATE OR REPLACE FUNCTION rastreofx_getFoto(idfoto bigint) RETURNS RECORD AS $$
DECLARE
	myrow RECORD;
BEGIN
	SELECT INTO myrow * FROM rastreo_vehiculo_fotos WHERE idrastreo_vehiculo_fotos = idfoto;
	RETURN myrow;
END;
$$ LANGUAGE plpgsql;

select rastreofx_getFoto(6);

