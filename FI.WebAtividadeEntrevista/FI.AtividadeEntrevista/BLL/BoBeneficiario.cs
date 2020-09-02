using FI.AtividadeEntrevista.DAL.Beneficiarios;
using System.Collections.Generic;


namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiario> Listar()
        {
            DaoBeneficiario beneficiario = new DaoBeneficiario();
            return beneficiario.Listar();
        }

        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DaoBeneficiario bene = new DaoBeneficiario();
            return bene.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DaoBeneficiario bene = new DaoBeneficiario();
            bene.Alterar(beneficiario);
        }

        /// <summary>
        /// Exclui um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Excluir(DML.Beneficiario beneficiario)
        {
            DaoBeneficiario bene = new DaoBeneficiario();
            bene.Excluir(beneficiario);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        /// <returns></returns>
        public bool VerificarExistencia(DML.Beneficiario beneficiario)
        {
            DaoBeneficiario bene = new DaoBeneficiario();
            return bene.VerificarExistencia(beneficiario);
        }

    }
}
