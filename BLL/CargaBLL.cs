using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace BLL
{
    public class CargaBLL
    {
        private CargaDAL _cargaDAL;
        private NotaFiscalBLL _notaFiscalBLL;

        public CargaBLL()
        {
            _cargaDAL = new CargaDAL();
            _notaFiscalBLL = new NotaFiscalBLL();
        }

        public List<string> Incluir(CargaDAO cargaDAO)
        {
            ValidarIncluir(cargaDAO);

            var listaRetorno = new List<string>();

            //using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
            //{
                var cargaId = _cargaDAL.Inserir(cargaDAO);

                cargaDAO.NotaFiscalDao.ForEach(x => x.CargaID = cargaId);

                listaRetorno = _notaFiscalBLL.ImportarArquivoCarga(cargaDAO.NotaFiscalDao);

            //    scope.Complete();
            //}

            return listaRetorno;
        }

        public CargaDAO Obter(CargaDAO cargaDAO)
        {
            var cargaDao = _cargaDAL.Obter(cargaDAO);

            return cargaDAO;
        }

        private void ValidarIncluir(CargaDAO cargaDAO)
        {
            if (cargaDAO == null)
                throw new ApplicationException("CargaDAO é obrigatório.");

            if (string.IsNullOrEmpty(cargaDAO.NumeroCarga))
                throw new ApplicationException("Número de carga é obrigatório.");

            if (cargaDAO.DataCadastro == DateTime.MinValue)
                throw new ApplicationException("Data de Cadastro é obrigatório");

            if (cargaDAO.SistemaID <= 0)
                throw new ApplicationException("Sistema é obrigatório");
        }
    }
}
