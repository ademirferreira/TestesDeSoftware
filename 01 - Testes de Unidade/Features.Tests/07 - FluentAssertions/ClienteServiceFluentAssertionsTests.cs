using System.Linq;
using System.Threading;
using Features.Clientes;
using Features.Tests._06___AutoMock;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests._07___FluentAssertions
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteServiceFluentAssertionsTests
    {
        private readonly ClienteTestsAutoMockerFixture _clienteTestsFixture;
        private readonly ClienteService _clienteService;

        public ClienteServiceFluentAssertionsTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
        {
            _clienteTestsFixture = clienteTestsFixture;
            _clienteService = _clienteTestsFixture.ObterClienteService();
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service Fluent Assertions Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = _clienteTestsFixture.GerarClienteValido();

            // Act
            _clienteService.Adicionar(cliente);

            // Assert
            //Assert.True(cliente.EhValido());
            cliente.EhValido().Should().BeTrue();

            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Once);
            _clienteTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once());
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service Fluent Assertions Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = _clienteTestsFixture.GerarClienteInvalido();
            // Act
            _clienteService.Adicionar(cliente);

            // Assert
            //Assert.False(cliente.EhValido());
            cliente.EhValido().Should().BeFalse("Possui inconsistências");
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1);

            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never);
            _clienteTestsFixture.Mocker.GetMock<IMediator>().Verify(m =>
                m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never());
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service Fluent Assertions Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
                .Returns(_clienteTestsFixture.ObterClientesVariados());

            // Act
            var clientes = _clienteService.ObterTodosAtivos();
            // Assert 
            //Assert.True(clientes.Any());
            //Assert.False(clientes.Count(c => !c.Ativo) > 0);
            clientes.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
            clientes.Should().NotContain(c => !c.Ativo);

            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);
            

            
        }
    }
}