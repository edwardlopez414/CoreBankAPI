using Xunit;
using Moq;
using CoreBankAPI.Logic;
using CoreBankAPI.Models;
using CoreBankAPI.Logic.Validator;
using CoreBankAPI.Data;
using CoreBankAPI.CoreDbContext;
using CoreBankAPI.Logic.Interfaces;
namespace CoreBankAPI.Test
{
    public class TransactionManagerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepoMock;
        private readonly Mock<IAccountRepository> _accountRepoMock;
        private readonly Mock<CoreDb> _dbContextMock;
        private readonly ValidateRequest _validator;
        private readonly TransactionManager _manager;

        public TransactionManagerTests()
        {
            _transactionRepoMock = new Mock<ITransactionRepository>();
            _accountRepoMock = new Mock<IAccountRepository>();
            _dbContextMock = new Mock<CoreDb>();
            _validator = new ValidateRequest();

            _manager = new TransactionManager(
                _dbContextMock.Object,
                _validator,
                _transactionRepoMock.Object,
                _accountRepoMock.Object
            );
        }

        [Fact]
        public void Deposito_DeberiaRetornarCompletado()
        {
            // Arrange
            var cuenta = new AccoutDta
            {
                Id = 1,
                Balance = 100,
                identifier = "ABC123"
            };

            var dto = new TransactionDto
            {
                Identifier = "ABC123",
                Amount = 50,
                Description = "Depósito"
            };

            _accountRepoMock.Setup(r => r.GetByIdentifier("ABC123")).Returns(cuenta);

            // Act
            var (esError, error, respuesta) = _manager.deposit(dto);

            // Assert
            Assert.False(esError);
            Assert.Equal("completed", respuesta.Status);
            Assert.Equal(150, respuesta.BalanceAccount); // 100 + 50
            Assert.Equal("ABC123", respuesta.Account);
        }

        [Fact]
        public void Retiro_DeberiaRetornarCompletado()
        {
            // Arrange
            var cuenta = new AccoutDta
            {
                Id = 1,
                Balance = 200,
                identifier = "XYZ789"
            };

            var dto = new TransactionDto
            {
                Identifier = "XYZ789",
                Amount = 80,
                Description = "Retiro"
            };

            _accountRepoMock.Setup(r => r.GetByIdentifier("XYZ789")).Returns(cuenta);

            // Act
            var (esError, error, respuesta) = _manager.withdrawals(dto);

            // Assert
            Assert.False(esError);
            Assert.Equal("completed", respuesta.Status);
            Assert.Equal(120, respuesta.BalanceAccount); // 200 - 80
            Assert.Equal("XYZ789", respuesta.Account);
        }
    }
}
