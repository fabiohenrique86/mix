using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Utils
    {
        public bool ValidarCnpj(string CNPJ)
        {
            string ftmt = "6543298765432";
            int[] digitos = new int[14];
            int[] soma = new int[] { 0, 0 };
            int[] resultado = new int[] { 0, 0 };
            bool[] CNPJOk = new bool[] { false, false };
            try
            {
                int nrDig;
                if ((((CNPJ == "00000000000000") || (CNPJ == "11111111111111")) || ((CNPJ == "22222222222222") || (CNPJ == "33333333333333"))) || ((((CNPJ == "44444444444444") || (CNPJ == "55555555555555")) || ((CNPJ == "66666666666666") || (CNPJ == "77777777777777"))) || ((CNPJ == "88888888888888") || (CNPJ == "99999999999999"))))
                {
                    return false;
                }
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                    {
                        soma[0] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1));
                    }
                    if (nrDig <= 12)
                    {
                        soma[1] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1));
                    }
                }
                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = soma[nrDig] % 11;
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                    {
                        CNPJOk[nrDig] = digitos[12 + nrDig] == 0;
                    }
                    else
                    {
                        CNPJOk[nrDig] = digitos[12 + nrDig] == (11 - resultado[nrDig]);
                    }
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        public bool ValidarCpf(string cpf)
        {
            int primeiroDigito = 0;
            int segundoDigito = 0;
            string cpfAux = cpf;

            if (string.IsNullOrEmpty(cpf))
            {
                return false;
            }

            if (cpf.Length <= 10)
            {
                return false;
            }
            else
            {
                if (cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999")
                {
                    return false;
                }
                cpf = cpf.Substring(0, 9);
            }

            int primeiroDigitoAux = Convert.ToInt32(cpfAux.Substring(9, 1));
            int segundoDigitoAux = Convert.ToInt32(cpfAux.Substring(10, 1));

            for (int cont = 0; cont <= 8; cont++)
            {
                char primeiro = cpf[cont];
                primeiroDigito += (10 - cont) * Convert.ToInt32(primeiro.ToString());
                char segundo = cpf[cont];
                segundoDigito += (11 - cont) * Convert.ToInt32(segundo.ToString());
            }

            primeiroDigito = primeiroDigito % 11;

            if (primeiroDigito < 2)
            {
                primeiroDigito = 0;
            }
            else
            {
                primeiroDigito = 11 - primeiroDigito;
            }

            segundoDigito += primeiroDigito * 2;
            segundoDigito = segundoDigito % 11;

            if (segundoDigito < 2)
            {
                segundoDigito = 0;
            }
            else
            {
                segundoDigito = 11 - segundoDigito;
            }

            return ((primeiroDigitoAux == primeiroDigito) && (segundoDigitoAux == segundoDigito));
        }
    }
}
