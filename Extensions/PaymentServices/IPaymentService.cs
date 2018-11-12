using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Smoking.Models;

namespace Smoking.Extensions.PaymentServices
{
    public interface IPaymentService
    {
        bool Enabled { get; }
        string Name { get; }
        Mode CurrentMode { get; }
        bool IsCompleted { get; }
        void Init(Order orderForPay);
        void ProcessPayment(string currencyCode = "");
        string AutoExecuteOnRequest();
        bool IsMyNotification { get; }
        Order NotificatedOrder { get; }
    }

    public enum Mode
    {
        /// <summary>
        /// Оформление заказа.
        /// </summary>
        Default,

        /// <summary>
        /// Обработка результата, возвращённого системой оплаты.
        /// </summary>
        Result,

        /// <summary>
        /// Сообщение о успешной оплате.
        /// </summary>
        Success,

        /// <summary>
        /// Сообщение о неудачной оплате.
        /// </summary>
        Fail
    }
}