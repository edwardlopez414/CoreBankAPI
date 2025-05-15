namespace CoreBankAPI.Models
{
    public class TransactionResponse
    {
        public string? Status { get; set; }
        public string? Account { get; set; }
        public int BalanceAccount { get; set; }
        public int TransactionAmount { get; set; }
    }
}
