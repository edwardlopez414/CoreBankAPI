namespace CoreBankAPI.Models
{
    public class HistoryInfo
    {
        public string? Account { get; set; }
        public string? Type { get; set; }
        public int TransactionAmount { get; set; }
        public DateTime Registered { get; set; }
    }
}
