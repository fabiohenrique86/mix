using DAO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class FreteBLL
    {
        public List<FreteDAO> Listar (FreteDAO freteDAO)
        {
            try
            {
                List<FreteDAO> retorno = null;

                var freteDAL = new DAL.FreteDAL();

                retorno = freteDAL.Listar(freteDAO);

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
