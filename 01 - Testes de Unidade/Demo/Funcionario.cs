using System;
using System.Collections.Generic;

namespace Demo
{
    public class Pessoa
    {
        public string Nome { get; protected set; }
        public string Apelido { get; set; }
    }

    public class Funcionario : Pessoa
    {
        public double Salario { get; private set; }
        public NivelProfissional NivelProfissional { get; private set; }
        public IList<string> Habilidades { get; private set; }

        public Funcionario(string nome, double salario)
        {
            Nome = string.IsNullOrEmpty(nome) ? "Fulano" : nome;
            DefinirSalario(salario);
            DefinirHabilidades();
        }

        public void DefinirSalario(double salario)
        {
            if (salario < 500) throw new Exception("Salario inferior ao permitido");

            Salario = salario;
            NivelProfissional = salario switch
            {
                < 2000 => NivelProfissional.Junior,
                >= 2000 and < 8000 => NivelProfissional.Pleno,
                >= 8000 => NivelProfissional.Senior,
                _ => NivelProfissional
            };
        }

        private void DefinirHabilidades()
        {
            var habilidadesBasicas = new List<string>()
            {

                "Lógica de Programação",
                "OOP"
            };

            Habilidades = habilidadesBasicas;

            switch (NivelProfissional)
            {
                case NivelProfissional.Pleno:
                    Habilidades.Add("Testes");
                    break;
                case NivelProfissional.Senior:
                    Habilidades.Add("Testes");
                    Habilidades.Add("Microservices");
                    break;
            }
        }
    }

    public enum NivelProfissional
    {
        Junior,
        Pleno,
        Senior
    }

    public class FuncionarioFactory
    {
        public static Funcionario Criar(string nome, double salario)
        {
            return new Funcionario(nome, salario);
        }
    }
}