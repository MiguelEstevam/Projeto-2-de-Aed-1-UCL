using System;
using System.IO;

class Conta
{
    private static int proximoNumeroConta = 1;

    private string numeroConta;
    private string tipoConta;
    private float saldo;
    public Pessoa titular;

    public Conta(string tipoConta, float saldo, Pessoa titular)
    {
        this.numeroConta = GerarNumeroConta();

        if (ValidarTipoConta(tipoConta))
        {
            this.tipoConta = tipoConta.ToUpper();
            this.saldo = saldo >= 0 ? saldo : 0;
            this.titular = titular;
        }
        else
        {
            Console.WriteLine("Tipo de conta inválido. Use 'Corrente' ou 'Poupança'.");
        }
    }

    public string GetNumeroConta()
    {
        return numeroConta;
    }

    public string GetTipoConta()
    {
        return tipoConta;
    }

    public float GetSaldo()
    {
        return saldo;
    }

    public Pessoa GetTitular()
    {
        return titular;
    }

    public void Depositar(float valor)
    {
        if (valor > 0)
        {
            saldo += valor;
            Console.WriteLine($"Depósito de R${valor} realizado com sucesso.");
            RegistrarDeposito(valor);
        }
        else
        {
            Console.WriteLine("Valor de depósito inválido.");
        }
    }

    public void Sacar(float valor)
    {
        if (valor > 0 && valor <= saldo)
        {
            saldo -= valor;
            Console.WriteLine($"Saque de R${valor} realizado com sucesso.");
            RegistrarSaque(valor);
        }
        else
        {
            Console.WriteLine("Valor de saque inválido ou saldo insuficiente.");
        }
    }

    private string GerarNumeroConta()
    {
        return $"{DateTime.Now.Year}{proximoNumeroConta++:D5}";
    }

    private bool ValidarTipoConta(string tipoConta)
    {
        return tipoConta.ToUpper() == "CORRENTE" || tipoConta.ToUpper() == "POUPANÇA";
    }

    public void ConsultarSaldo()
    {
        Console.WriteLine($"Saldo da Conta {GetNumeroConta()}: R${GetSaldo():F2}");
    }

    public void RegistrarDeposito(float valor)
    {
        string movimentacao = $"DEPÓSITO: +{valor:C2}";
        RegistrarMovimentacao(movimentacao);
    }

    public void RegistrarSaque(float valor)
    {
        string movimentacao = $"SAQUE: -{valor:C2}";
        RegistrarMovimentacao(movimentacao);
    }

    private void RegistrarMovimentacao(string movimentacao)
    {
        string nomeArquivo = $"{numeroConta}_movimentacoes.txt";
        string caminhoPasta = "movimentacoes";
        string caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

        try
        {
            if (!Directory.Exists(caminhoPasta))
            {
                Directory.CreateDirectory(caminhoPasta);
            }

            using (StreamWriter sw = new StreamWriter(caminhoCompleto, true))
            {
                sw.WriteLine($"{DateTime.Now} - {movimentacao}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao registrar movimentação: {e.Message}");
        }
    }
}
