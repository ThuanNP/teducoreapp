using System.ComponentModel;

namespace TeduCoreApp.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Cash on delivery")]
        CashOnDelivery,

        [Description("Online banking")]
        OnlinBanking,

        [Description("Payment gateway")]
        PaymentGateway,

        [Description("Visa")]
        Visa,

        [Description("Master card")]
        MasterCard,

        [Description("PayPal")]
        PayPal,

        [Description("ATM")]
        Atm
    }
}