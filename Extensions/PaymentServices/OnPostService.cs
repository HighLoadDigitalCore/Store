using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Smoking.Models;

namespace Smoking.Extensions.PaymentServices
{
    public class OnPostService : IPaymentService
    {
        public bool Enabled { get { return false; } }
        public string Name { get { return "OnPost"; } }
        public Mode CurrentMode { get { return Mode.Default; } }
        public bool IsCompleted { get { return true; } }

        public void Init(Order orderForPay)
        {
        }

        public void ProcessPayment(string currencyCode = "")
        {
        }

        public string AutoExecuteOnRequest()
        {
            return "Наш менеджер скоро свяжется с Вами.<br />Спасибо за покупку!";
        }

        public bool IsMyNotification { get { return false; } }
        public Order NotificatedOrder { get { return (Order)HttpContext.Current.Session["LastOrder"]; } }
    }
}