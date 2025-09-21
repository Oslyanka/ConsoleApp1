using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Define o caminho para a pasta onde os dados serão armazenados
            var dataPath = Path.Combine(Environment.CurrentDirectory, "Data");

            // Verifica se a pasta de dados existe
            if (!Directory.Exists(dataPath))
            {
                // Se a pasta não existir, cria a pasta
                Directory.CreateDirectory(dataPath);
            }

            // Define o caminho para o arquivo de dados
            string filePath = Path.Combine(dataPath, "dados.txt");

            // Verifica se o arquivo de dados existe
            if (!File.Exists(filePath))
            {
                // Se o arquivo não existir, cria o arquivo e escreve uma string vazia
                File.WriteAllText(filePath, "");
            }

            // Cria uma lista para armazenar os usuários
            List<User> usuario = new List<User>();

            // Lê todas as linhas do arquivo de dados
            List<string> lines = File.ReadAllLines(filePath).ToList();

            // Itera sobre cada linha do arquivo
            foreach (var line in lines)
            {
                // Divide a linha em um array de strings, usando a vírgula como separador
                string[] entries = line.Split(',');

                // Verifica se a linha contém todos os dados necessários (Nome, Sobrenome, Senha, CPF)
                if (entries.Length < 4)
                {
                    // Se a linha não contiver todos os dados, ignora a linha
                    continue;
                }

                // Cria um novo objeto User
                User newUser = new User();

                // Preenche as propriedades do objeto User com os dados da linha
                newUser.Nome = entries[0];
                newUser.Sobrenome = entries[1];
                newUser.Senha = int.Parse(entries[2]);
                newUser.CPF = entries[3];

                // Adiciona o novo usuário à lista de usuários
                usuario.Add(newUser);
            }

            // Loop principal do programa
            bool sair = false;
            while (!sair)
            {
                // Exibe o menu de opções
                Console.WriteLine("\nBem-vindo ao sistema de cadastro de usuários!");
                Console.WriteLine("1) Adicionar uma Conta");
                Console.WriteLine("2) Remover uma Conta");
                Console.WriteLine("3) Ver as Contas");
                Console.WriteLine("4) Sair ");
                Console.Write("\nEscolha uma opção: ");

                // Lê a opção escolhida pelo usuário
                switch (Console.ReadLine())
                {
                    // Caso a opção escolhida seja 1 (Adicionar uma Conta)
                    case "1":
                        {
                            Console.WriteLine("Adicionar uma nova Conta:");

                            // Solicita o nome do usuário
                            Console.Write("Nome: ");
                            string nome = Console.ReadLine();

                            // Solicita o sobrenome do usuário
                            Console.Write("Sobrenome: ");
                            string sobrenome = Console.ReadLine();

                            // Solicita a senha do usuário  
                            Console.Write("Senha (número): ");
                            int senha;

                            // Tenta converter a senha para um número inteiro
                            if (!int.TryParse(Console.ReadLine(), out senha))
                            {
                                // Se a senha não for um número válido, exibe uma mensagem de erro e volta para o menu
                                Console.WriteLine("Senha inválida. Deve ser um número.");
                                break;
                            }

                            // Solicita o CPF do usuário
                            Console.Write("CPF (apenas números): ");
                            string cpf = Console.ReadLine();

                            // Valida o CPF
                            if (cpf == null || cpf.Length != 11 || !cpf.All(char.IsDigit))
                            {
                                // Se o CPF for inválido, exibe uma mensagem de erro e volta para o menu
                                Console.WriteLine("CPF inválido.");
                                break;
                            }

                            // Verifica se já existe um usuário com o mesmo CPF
                            if (usuario.Any(u => u.CPF == cpf))
                            {
                                // Se já existir um usuário com o mesmo CPF, exibe uma mensagem de erro e volta para o menu
                                Console.WriteLine("Já existe uma conta com esse CPF.");
                                break;
                            }

                            // Cria um novo objeto User com os dados fornecidos
                            User novoUsuario = new User { Nome = nome ?? string.Empty, Sobrenome = sobrenome ?? string.Empty, Senha = senha, CPF = cpf };

                            // Adiciona o novo usuário à lista de usuários
                            usuario.Add(novoUsuario);

                            // Escreve os dados de todos os usuários no arquivo de dados
                            File.WriteAllLines(filePath, usuario.Select(u => $"{u.Nome},{u.Sobrenome},{u.Senha},{u.CPF}"));

                            // Exibe uma mensagem de sucesso
                            Console.WriteLine("\nConta adicionada com sucesso!");
                            break;
                        }

                    // Caso a opção escolhida seja 2 (Remover uma Conta)
                    case "2":
                        {
                            Console.WriteLine("Digite o CPF da conta a ser removida (apenas 0 para retornar):");
                            string cpfToRemove = Console.ReadLine();
                            if (cpfToRemove == "0")
                            {
                                break;
                            }
                            var userToRemove = usuario.FirstOrDefault(u => u.CPF == cpfToRemove);
                            if (userToRemove != null)
                            {
                                usuario.Remove(userToRemove);
                                File.WriteAllLines(filePath, usuario.Select(u => $"{u.Nome},{u.Sobrenome},{u.Senha},{u.CPF}"));
                                Console.WriteLine("Conta removida com sucesso.");
                            }
                            else
                            {
                                Console.WriteLine("Conta não encontrada.");
                            }

                            break;
                        }
                    // Caso a opção escolhida seja 3 (Ver as Contas)
                    case "3":
                        {
                            // Exibe os dados de todos os usuários cadastrados
                            Console.WriteLine("\nUsuários cadastrados:");
                            foreach (var p in usuario)
                            {
                                Console.WriteLine($"Nome: {p.Nome} - Sobrenome: {p.Sobrenome} - Senha: {p.Senha} - CPF: {p.CPF}");
                            }
                            break;
                        }

                    // Caso a opção escolhida seja 4 (Sair)
                    case "4":
                        {
                            // Exibe uma mensagem de despedida e encerra o programa
                            Console.WriteLine("Saindo do programa.");
                            sair = true;
                            break;
                        }

                    // Caso a opção escolhida seja inválida
                    default:
                        {
                            // Exibe uma mensagem de erro
                            Console.WriteLine("Opção inválida.");
                            break;
                        }
                }
            }
        }
    }

    // Define a classe User
    internal class User
    {
        // Propriedades da classe User
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public int Senha { get; set; }
        public string CPF { get; set; }
    }
}