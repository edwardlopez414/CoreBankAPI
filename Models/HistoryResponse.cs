namespace CoreBankAPI.Models
{
    public class HistoryResponse
    {
        public List<HistoryInfo> Details { get; set; }
        public int AccountBalance { get; set; }
    }
}
