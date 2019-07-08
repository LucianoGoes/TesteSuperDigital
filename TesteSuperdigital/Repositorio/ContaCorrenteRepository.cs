using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Models;

namespace Repository
{
    public class ContaCorrenteRepository : RepositoryBase<ContaCorrente>
    {
        private RepositoryContext _repositoryContext;

        public ContaCorrenteRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void Debito(int Conta, decimal Valor)
        {
            ContaCorrente contaCorrente = GetConta(Conta);

            contaCorrente.Saldo = contaCorrente.Saldo - Valor;

            SaveSaldo(contaCorrente);
        }

        public void Credito(int Conta, decimal Valor)
        {
            ContaCorrente contaCorrente = GetConta(Conta);

            contaCorrente.Saldo = contaCorrente.Saldo + Valor;

            SaveSaldo(contaCorrente);
        }

        public void Tranferencia(int ContaOrigem, int ContaDestino, decimal Valor)
        {
            Credito(ContaOrigem, Valor);

            Debito(ContaOrigem, Valor);

            Save();
        }

        private ContaCorrente GetConta(int Conta)
        {
            return FindByCondition(o => o.NumeroConta.Equals(Conta)).DefaultIfEmpty(new ContaCorrente()).FirstOrDefault();
        }

        private void SaveSaldo(ContaCorrente Conta)
        {
            Update(Conta);
        }

        public bool CheckContaExiste(int Conta)
        {
            return _repositoryContext.contaCorrentes.Where(o => o.NumeroConta == Conta).ToList().Count == 0 ? false : true;
        }

        public bool CheckSaldoExiste(int Conta, decimal Valor)
        {
            bool bReturn = false;

            ContaCorrente contaCorrente = GetConta(Conta);

            if (contaCorrente.NumeroConta > 0)
            {
                bReturn = contaCorrente.Saldo > Valor ? true : false;
            }

            return bReturn;
        }
    }
}
