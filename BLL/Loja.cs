using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Loja
    {
        /// <summary>
        /// Obtém um registro da tabela Loja
        /// </summary>
        /// <param name="lojaDAO"></param>
        /// <returns></returns>
        public DAO.Loja Obter(DAO.Loja lojaDAO)
        {
            try
            {
                DAO.Loja retorno = null;

                DAL.Loja lojaDAL = new DAL.Loja();

                retorno = lojaDAL.ObterLojaByCnpj(lojaDAO);

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
