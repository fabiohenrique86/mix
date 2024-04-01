using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public static class AdministradorDAL
    {
        // Methods
        public static bool ValidarLogin(string login, string senha)
        {
            Database db;
            bool adm = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spValidarLoginAdministrador"))
                {
                    db.AddInParameter(cmd, "@Login", DbType.String, login);
                    db.AddInParameter(cmd, "@Senha", DbType.String, senha);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        adm = true;
                    }
                }
                //return adm;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return adm;
        }
    }


}
