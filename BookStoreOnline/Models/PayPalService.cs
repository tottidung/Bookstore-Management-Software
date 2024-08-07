using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Web.Configuration;

public class PayPalService
{
    private readonly string clientId;
    private readonly string clientSecret;

    public PayPalService()
    {
        this.clientId = WebConfigurationManager.AppSettings["PayPalClientId"];
        this.clientSecret = WebConfigurationManager.AppSettings["PayPalClientSecret"];
    }

    private APIContext GetAPIContext()
    {
        var config = ConfigManager.Instance.GetProperties();
        var accessToken = new OAuthTokenCredential(this.clientId, this.clientSecret, config).GetAccessToken();
        return new APIContext(accessToken);
    }

    public Payment CreatePayment(decimal amount, string currency, string returnUrl, string cancelUrl)
    {
        var apiContext = GetAPIContext();

        var payer = new Payer() { payment_method = "paypal" };

        var redirectUrls = new RedirectUrls()
        {
            cancel_url = cancelUrl,
            return_url = returnUrl
        };

        var details = new Details()
        {
            tax = "0",
            shipping = "0",
            subtotal = amount.ToString("F")
        };

        var amountObj = new Amount()
        {
            currency = currency,
            total = amount.ToString("F"),
            details = details
        };

        var transactionList = new List<Transaction>
        {
            new Transaction()
            {
                description = "Transaction description.",
                invoice_number = new Random().Next(100000).ToString(), // Generate random invoice number
                amount = amountObj
            }
        };

        var payment = new Payment()
        {
            intent = "sale",
            payer = payer,
            transactions = transactionList,
            redirect_urls = redirectUrls
        };

        return payment.Create(apiContext);
    }

    public Payment ExecutePayment(string paymentId, string payerId)
    {
        var apiContext = GetAPIContext();
        var paymentExecution = new PaymentExecution() { payer_id = payerId };
        var payment = new Payment() { id = paymentId };
        return payment.Execute(apiContext, paymentExecution);
    }
}
