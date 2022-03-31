using Xunit;

namespace Features.Tests._08___Skip
{
    public class TesteNaoPassandoPorMotivoEspecifico
    {
        [Fact(DisplayName = "Novo Cliente 2.0", Skip = "Nova versão 2.0 quebrando")]
        [Trait("Categoria", "Escapando Testes")]
        public void Teste_NaoEstaPasando_VersaoNovaNaoCompativel()
        {
            Assert.True(false);
        }
    }
}