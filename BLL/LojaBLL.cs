using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using DAL;

namespace BLL
{
    public class LojaBLL
    {
        /// <summary>
        /// Obtém um registro da tabela Loja
        /// </summary>
        /// <param name="lojaDAO"></param>
        /// <returns></returns>
        public LojaDAO Obter(LojaDAO lojaDAO)
        {
            try
            {
                LojaDAO retorno = null;

                LojaDAL lojaDAL = new LojaDAL();

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
