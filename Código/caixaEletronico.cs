using System;
using System.Collections.Generic;
using System.IO;

class CaixaEletronico
{
    private List<Conta> contas;

    // Construtor
    public CaixaEletronico()
    {
        contas = new List<Conta>();
        CarregarContasDoArquivo("contas.txt");
    }

    // Método para carregar contas a partir de um arquivo
    private void CarregarContasDoArquivo(string nomeArquivo)
    {
        try
        {
            if (File.Exists(nomeArquivo))
            {
                using (StreamReader sr = new StreamReader(nomeArquivo))
                {
                    string linha;
                    while ((linha = sr.ReadLine()) != null)
                    {
                        string[] dadosConta = linha.Split(';');
                        if (dadosConta.Length == 5)
                        {
                            string numeroConta = dadosConta[0];
                            string tipoConta = dadosConta[1];
                            float saldo = float.Parse(dadosConta[2]);
                            string cpfTitular = dadosConta[3];
                            string nomeTitular = dadosConta[4];

                            Pessoa titular = new Pessoa(nomeTitular, cpfTitular);
                            Conta conta = new Conta(tipoConta, saldo, titular);
                            contas.Add(conta);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Arquivo de contas não encontrado. Criando um novo arquivo...");
                File.Create(nomeArquivo).Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao carregar contas: {e.Message}");
        }
    }

    // Método para salvar contas no arquivo
    private void SalvarContasNoArquivo(string nomeArquivo)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(nomeArquivo))
            {
                foreach (Conta conta in contas)
                {
                    Pessoa titular = conta.GetTitular();
                    sw.WriteLine($"{conta.GetNumeroConta()};{conta.GetTipoConta()};{conta.GetSaldo()};{titular.GetCpf()};{titular.GetNome()}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao salvar contas: {e.Message}");
        }
    }

    // Método para gerar o relatório HTML
    private void GerarRelatorioHTML()
    {
        string htmlPath = "relatorio_contas.html";
        try
        {
            using (StreamWriter sw = new StreamWriter(htmlPath))
            {
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<title>Relatório de Contas</title>");
                sw.WriteLine("<style>");
                sw.WriteLine("body { font-family: 'Arial', sans-serif; }");
                sw.WriteLine("table { border-collapse: collapse; width: 80%; margin: 20px auto; }");
                sw.WriteLine("th, td { border: 1px solid #333; padding: 10px; text-align: left; }");
                sw.WriteLine("th { background-color: #8e44ad; color: #fff; }");
                sw.WriteLine("td { background-color: #ecf0f1; color: #333; }");
                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");

                sw.WriteLine("<p style=\"text-align:center\"><span style=\"font-size:26px\"><span style=\"color:#8e44ad\"><strong>Relatório de Contas</strong></span></span></p>");

                // Cabeçalho da tabela
                sw.WriteLine("<table align=\"center\">");
                sw.WriteLine("<thead>");
                sw.WriteLine("<tr>");
                sw.WriteLine("<th scope=\"col\">Número da Conta</th>");
                sw.WriteLine("<th scope=\"col\">Tipo de Conta</th>");
                sw.WriteLine("<th scope=\"col\">Saldo</th>");
                sw.WriteLine("<th scope=\"col\">Titular</th>");
                sw.WriteLine("</tr>");
                sw.WriteLine("</thead>");
                sw.WriteLine("<tbody>");

                // Preenchimento da tabela com dados das contas
                foreach (Conta conta in contas)
                {
                    sw.WriteLine("<tr>");
                    sw.WriteLine($"<td>{conta.GetNumeroConta()}</td>");
                    sw.WriteLine($"<td>{conta.GetTipoConta()}</td>");
                    sw.WriteLine($"<td>{conta.GetSaldo():C2}</td>");
                    sw.WriteLine($"<td>{conta.GetTitular().GetNome()} (CPF: {conta.GetTitular().GetCpf()})</td>");
                    sw.WriteLine("</tr>");
                }

                sw.WriteLine("</tbody>");
                sw.WriteLine("</table>");

                sw.WriteLine("</body>");
                sw.WriteLine("</html>");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao gerar relatório HTML: {e.Message}");
        }
    }

    // Método para incluir uma nova conta
    private void IncluirNovaConta()
    {
        Console.WriteLine("Incluir Nova Conta");

        Console.Write("Digite o tipo da conta (Corrente ou Poupança): ");
        string tipoConta = Console.ReadLine();

        Console.Write("Digite o saldo inicial: ");
        if (float.TryParse(Console.ReadLine(), out float saldoInicial))
        {
            Console.Write("Digite o nome do titular: ");
            string nomeTitular = Console.ReadLine();

            Console.Write("Digite o CPF do titular: ");
            string cpfTitular = Console.ReadLine();

            Pessoa novoTitular = new Pessoa(nomeTitular, cpfTitular);
            Conta novaConta = new Conta(tipoConta, saldoInicial, novoTitular);

            contas.Add(novaConta);
            SalvarContasNoArquivo("contas.txt");

            // Mostra o número da conta ao cliente
            Console.WriteLine($"Nova conta criada com sucesso! Seu número de conta é: {novaConta.GetNumeroConta()}");

            // Atualiza automaticamente o relatório HTML
            GerarRelatorioHTML();
        }
        else
        {
            Console.WriteLine("Valor inválido para o saldo inicial.");
        }
    }

    // Método para consultar o saldo
    private void ConsultarSaldo()
    {
        Console.Write("Digite o número da conta: ");
        string numeroConta = Console.ReadLine();

        Conta conta = BuscarConta(numeroConta);
        if (conta != null)
        {
            conta.ConsultarSaldo();
        }
        else
        {
            Console.WriteLine("Conta não encontrada.");
        }
    }

    private void Depositar()
    {
        Console.Write("Digite o número da conta: ");
        string numeroConta = Console.ReadLine();

        Conta conta = BuscarConta(numeroConta);
        if (conta != null)
        {
            Console.Write("Digite o valor do depósito: ");
            if (float.TryParse(Console.ReadLine(), out float valorDeposito))
            {
                conta.RegistrarDeposito(valorDeposito);
                SalvarContasNoArquivo("contas.txt");
            }
            else
            {
                Console.WriteLine("Valor inválido.");
            }
        }
        else
        {
            Console.WriteLine("Conta não encontrada.");
        }
    }

    private void Sacar()
    {
        Console.Write("Digite o número da conta: ");
        string numeroConta = Console.ReadLine();

        Conta conta = BuscarConta(numeroConta);
        if (conta != null)
        {
            Console.Write("Digite o valor do saque: ");
            if (float.TryParse(Console.ReadLine(), out float valorSaque))
            {
                conta.RegistrarSaque(valorSaque);
                SalvarContasNoArquivo("contas.txt");
            }
            else
            {
                Console.WriteLine("Valor inválido.");
            }
        }
        else
        {
            Console.WriteLine("Conta não encontrada.");
        }
    }

    // Método para exibir o menu de forma mais estilizada
    private void ExibirMenu()
    {
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║            CAIXA ELETRÔNICO          ║");
        Console.WriteLine("╠══════════════════════════════════════╣");
        Console.WriteLine("║ 1 - Consultar Saldo                  ║");
        Console.WriteLine("║ 2 - Depositar                        ║");
        Console.WriteLine("║ 3 - Sacar                            ║");
        Console.WriteLine("║ 4 - Incluir Nova Conta               ║");
        Console.WriteLine("║ 5 - Sair                             ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
    }

    // Método principal de execução do caixa eletrônico
    public void Executar()
    {
        int opcao = 0;

        do
        {
            Console.Clear();
            ExibirMenu();

            Console.Write("Escolha uma opção: ");
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        ConsultarSaldo();
                        break;
                    case 2:
                        Depositar();
                        break;
                    case 3:
                        Sacar();
                        break;
                    case 4:
                        IncluirNovaConta();
                        break;
                    case 5:
                        Sair();
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }

            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();

        } while (opcao != 5);
    }

    // Método para encerrar a execução do caixa eletrônico
    private void Sair()
    {
        GerarRelatorioHTML(); // Atualiza o relatório HTML antes de sair
        SalvarContasNoArquivo("contas.txt"); // Salvar as contas antes de sair
        Console.WriteLine("Obrigado por utilizar o Caixa Eletrônico. Até mais!");
        Environment.Exit(0); // Encerra o programa
    }

    // Método para buscar uma conta pelo número
    private Conta BuscarConta(string numeroConta)
    {
        Conta contaEncontrada = contas.Find(c => c.GetNumeroConta() == numeroConta);

        if (contaEncontrada != null)
        {
            return contaEncontrada;
        }
        else
        {
            Console.WriteLine("Conta não encontrada.");
            // Se a conta não for encontrada, você pode retornar null ou lançar uma exceção, dependendo dos requisitos do seu programa.
            return null;
        }
    }
}
