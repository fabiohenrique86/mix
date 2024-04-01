using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using DAO;

namespace BLL
{
    public class Cliente
    {
        public int SeExisteClienteComCnpjInformado(string cnpj, int sistemaId)
        {
            int clienteId = new DAL.Cliente().ListarByCnpj(cnpj, sistemaId);
            if (clienteId <= 0)
            {
                throw new ApplicationException("Cliente não cadastrado.");
            }
            return clienteId;
        }

        public int SeExisteClienteComCpfInformado(string cpf, int sistemaId)
        {
            int clienteId = new DAL.Cliente().ListarByCpf(cpf, sistemaId);
            if (clienteId <= 0)
            {
                throw new ApplicationException("Cliente não cadastrado.");
            }
            return clienteId;
        }
    }


}
