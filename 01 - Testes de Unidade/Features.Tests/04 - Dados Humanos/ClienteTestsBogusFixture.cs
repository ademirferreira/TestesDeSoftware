using System;
using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using Xunit;

namespace Features.Tests._04___Dados_Humanos
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteTestsBogusFixture>{}
    public class ClienteTestsBogusFixture : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(80, DateTime.Now.AddYears(-18)),
                    "",
                    true,
                    DateTime.Now
                )).RuleFor(c => c.Email, (f, c) =>
                    f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return cliente;
        }

        public Cliente GerarClienteInValido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(1, DateTime.Now.AddYears(1)),
                    "",
                    false,
                    DateTime.Now
                ));

            return cliente;
        }

        public void Dispose()
        {
        }
    }
}