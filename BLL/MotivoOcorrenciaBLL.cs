using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using DAO;

namespace BLL
{
    public class MotivoOcorrenciaBLL
    {
        private MotivoOcorrenciaDAL MotivoOcorrenciaDAL;

        public MotivoOcorrenciaBLL()
        {
            MotivoOcorrenciaDAL = new MotivoOcorrenciaDAL();
        }

        public List<MotivoOcorrenciaDAO> Listar(MotivoOcorrenciaDAO motivoOcorrenciaDAO)
        {
            List<MotivoOcorrenciaDAO> motivoOcorrencias = new List<MotivoOcorrenciaDAO>();

            motivoOcorrencias = MotivoOcorrenciaDAL.Listar(motivoOcorrenciaDAO);

            return motivoOcorrencias;
        }
    }
}
