namespace Staychill.Models.BankModel
{
    public class PaymentMethod
    {
        public int PaymentmethodId { get; set; } // Primary Key //
        public string PaymentmethodType { get; set; }
        public BankTransfer? BankTransfer { get; set; }
        public CreditCard? CreditCard { get; set; }
        public QRData? QRData { get; set; }


        // Constructor should match the class name
        public PaymentMethod()
        {
            CreditCard = new CreditCard();
            BankTransfer = new BankTransfer();
            QRData = new QRData();
        }
    }
}
