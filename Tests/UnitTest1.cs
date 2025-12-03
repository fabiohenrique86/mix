using BLL;
using DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod("Inserir Pedido")]
        public void InserirPedido()
        {
            var reservaBLL = new ReservaBLL();

            var reservaDAO = new ReservaDAO()
            {
                ReservaID = "TESTE12345",
                FuncionarioID = 223,
                ClienteID = 61305,
                StatusID = 2,
                LojaOrigemID = 29,
                SistemaID = 2,
                DataReserva = DateTime.Now,
                DataEntrega = new DateTime(2025, 12, 19),
                Cpf = "12345678901", // 85209007553
                ListaProduto = new List<ProdutoDAO>()
                {
                    new ProdutoDAO()
                    {
                        ProdutoID = 6040703610,
                        Quantidade = 3,
                        Preco = 1m,
                        SistemaID = 2,
                        LojaID = 3,
                        NomeFantasia = "depósito"
                    },
                    new ProdutoDAO()
                    {
                        ProdutoID = 4070955539,
                        Quantidade = 3,
                        Preco = 1m,
                        SistemaID = 2,
                        LojaID = 3,
                        NomeFantasia = "depósito"
                    }
                }
            };

            var reservaId = reservaBLL.Inserir(reservaDAO);

            Assert.IsNotNull(reservaId);
        }
    }
}
