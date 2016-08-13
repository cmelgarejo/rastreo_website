/*
'==============================================================================================================
'  Autor: CLrSoft
'  Clase concreta de la tabla: rastreogps_reportes 
'  Base soportada: .
'  Version MyGen: # (1.3.0.9)
'==============================================================================================================
*/


using System;

namespace RASTREOmw
{
	public class rastreogps_reportes : _rastreogps_reportes
	{
		public rastreogps_reportes(String laCadenaDeConexion)
		{
			this.ConnectionString = laCadenaDeConexion;
		}
		
		public bool DataBindSqlQuery(string Proc)
        {
            return base.LoadFromRawSql(Proc);
        }
	}
}
