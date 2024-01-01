using System;
using System.Linq;

class Pessoa
{
    private string nome;
    private string cpf;
    private int idade;

    public Pessoa(string nome, string cpf)
    {
        SetNome(nome);
        SetCpf(cpf);
    }

    public string GetNome()
    {
        return nome;
    }

    public string GetCpf()
    {
        return cpf;
    }

    public int GetIdade()
    {
        return idade;
    }

    public void SetNome(string nome)
    {
        this.nome = nome.ToUpper();
    }

    public void SetCpf(string cpf)
    {
        string cpfNumerico = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpfNumerico.Length == 11 && cpf.All(char.IsDigit))
        {
            this.cpf = cpfNumerico;
        }
        else
        {
            Console.WriteLine("O CPF informado é inválido!");
        }
    }

    public void SetIdade(int idade)
    {
        if (idade >= 18 && idade <= 150)
        {
            this.idade = idade;
        }
        else
        {
            Console.WriteLine("Idade fora da faixa permitida (18 a 150 anos).");
        }
    }
}
