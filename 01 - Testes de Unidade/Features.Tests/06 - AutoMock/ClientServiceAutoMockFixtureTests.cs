using System.Linq;
using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Features.Tests._06___AutoMock
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClientServiceAutoMockFixturesTests
    {
        private readonly ClienteTestsAutoMockerFixture _clienteTestsFixture;
        private readonly ClienteService _clienteService;

        public ClientServiceAutoMockFixturesTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
        {
            _clienteTestsFixture = clienteTestsFixture;
            _clienteService = _clienteTestsFixture.ObterClienteService();
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service AutoMockFixture Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = _clienteTestsFixture.GerarClienteValido();
            
            // Act
            _clienteService.Adicionar(cliente);

            // Assert
            Assert.True(cliente.EhValido());
            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Once);
            _clienteTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once());
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service AutoMockFixture Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = _clienteTestsFixture.GerarClienteInvalido();
            // Act
            _clienteService.Adicionar(cliente);

            // Assert
            Assert.False(cliente.EhValido());
            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never);
            _clienteTestsFixture.Mocker.GetMock<IMediator>().Verify(m =>
                m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never());
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service AutoMockFixture Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
                .Returns(_clienteTestsFixture.ObterClientesVariados());

            // Act
            var clientes = _clienteService.ObterTodosAtivos();
            // Assert 
            _clienteTestsFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);
        }

    }
}