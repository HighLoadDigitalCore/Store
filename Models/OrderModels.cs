using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using PdfGen;
using Smoking.Extensions;

namespace Smoking.Models
{
    public partial class OrderStatus
    {
        public static int GetStatusID(string engName)
        {
            var db = new DB();
            var status = db.OrderStatus.FirstOrDefault(x => x.EngName == engName) ??
                         db.OrderStatus.First(x => x.EngName == "Empty");
            return status.ID;
        }

        public static int GetStatusIDByRUS(string rusName)
        {
            var db = new DB();
            var status = db.OrderStatus.FirstOrDefault(x => x.Status == rusName.ToNiceForm().Trim()) ??
                         db.OrderStatus.First(x => x.EngName == "Empty");
            return status.ID;
        }

    }

    public partial class OrderedProduct
    {
        public decimal Sum
        {
            get { return Amount * SalePrice; }
        }
        public string Description
        {
            get
            {
                return Amount > 1
                        ? string.Format("{0} x{1}", StoreProduct.Name.ClearHTML(), Amount)
                        : StoreProduct.Name.ClearHTML();
            }
        }

        public int ProductIDOrNull
        {
            get { return ProductID ?? 0; }
        }

        public string FullUrl
        {
            get { return StoreProduct == null ? "#nogo" : StoreProduct.FullUrl; }
        }
    }

    public partial class OrderDetail
    {
        private OrderAdress _addressEntry;
        public OrderAdress AddressEntry
        {
            get
            {
                if (_addressEntry == null)
                {
                    var db = new DB();
                    _addressEntry = db.OrderAdresses.FirstOrDefault(x => x.ID == AddressID) ?? new OrderAdress();

                }
                return _addressEntry;

            }
        }
    }

    public partial class Order
    {
        public SelectList StatusList
        {
            get
            {
                var db = new DB();
                return new SelectList(db.OrderStatus, "ID", "Status", StatusID);
            }
        }

        public string OrderPdfLink
        {
            get { return string.Format("/Master/ru/Export/OrderPayment?orderID={0}", ID); }
        }

        public byte[] CreatePdfDoc(string doctype)
        {
            switch (doctype)
            {
                case "bill":
                    return CreateBill();
                default:
                    return CreateReceipt();
            }

        }

        private byte[] CreateBill()
        {

            /*
                        var bg =
                            new BillGenerator
                            {
                                OrganizationName = Configuration.Get("Requisites.OrgName"),
                                BIK = Configuration.Get("Requisites.BIK"),
                                BankName = Configuration.Get("Requisites.Bank"),
                                Account1 = Configuration.Get("Requisites.RS"),
                                Account2 = Configuration.Get("Requisites.KS"),
                                INN = Configuration.Get("Requisites.INN"),
                                KPP = Configuration.Get("Requisites.KPP"),
                                BillNumber = "{0}{1}".FormatWith(order.ID, Configuration.Get("Requisites.NumeralPostfix")),
                                Buyer = "{0} {1}, {2} {3}, {4}".FormatWith(Configuration.Get("Requisites.INN"), context.Request["inn"],
                                                                            Configuration.Get("Requisites.KPP"), context.Request["kpp"], context.Request["on"]),
                                Address = Configuration.Get("Requisites.Address"),
                                CEOName = Configuration.Get("Requisites.CEO"),
                                CAName = Configuration.Get("Requisites.CA"),
                                StampImage = GetFileContent(context.Server.MapPath(Configuration.Get("Requisites.Seal"))),
                                SignatureImage = GetFileContent(context.Server.MapPath(Configuration.Get("Requisites.Sign"))),
                                Order = order.OrderItems.Select(oi => new OrderLine { Name = oi.ProductSnapshot.Name, Quantity = oi.Quantity, Price = oi.ProductSnapshot.Price, UnitName = Configuration.Get("ProductUnits") })
                            };
            
                        var bytes = bg.Render();
                        return bytes;
            */

            return null;
        }



        private byte[] CreateReceipt()
        {
            var rg = new ReceiptGenerator()
            {
                SellerName = SiteSetting.Get<string>("Requisites.OrgName"),
                SellerINN = SiteSetting.Get<string>("Requisites.INN"),
                SellerAccount = SiteSetting.Get<string>("Requisites.RS"),
                SellerBank = SiteSetting.Get<string>("Requisites.Bank"),
                SellerBIK = SiteSetting.Get<string>("Requisites.BIK"),
                SellerBankAccount = SiteSetting.Get<string>("Requisites.KS"),
                BuyerAddress = "",
                BuyerName = User.UserProfile.SurnameAndName,
                PaymentName = "Оплата заказа №" + ID.ToString("d10"),
                PaymentTotal = FinalSumWithDelivery
            };
            var bytes = rg.Render();
            return bytes;
        }


        public decimal SummaryBookWeight
        {
            get { return 0; }
        }

        public decimal TotalSum
        {
            get { return OrderedProducts.Sum(x => x.Amount * x.SalePrice); }
        }

        public decimal FinalSumWithDelivery
        {
            get { return TotalSum + OrderDetail.DeliveryCost; }
        }

        public string OrderNumber
        {
            get { return ImportID.IsFilled() ? ImportID : ID.ToString("d9"); }
        }

        public string UserData
        {
            get
            {
                var profile = User.Profile;
                var doc = new XDocument();
                var data = new XElement("UserData");
                doc.Add(data);
                data.Add(new XElement("Email", profile.Email));
                data.Add(new XElement("Surname", profile.Surname));
                data.Add(new XElement("Name", profile.Name));
                data.Add(new XElement("Patrinomic", profile.Patrinomic));
                data.Add(new XElement("HomePhone", profile.HomePhone));
                data.Add(new XElement("MobilePhone", profile.MobilePhone));
                return doc.ToString();
            }
        }

        public string BriefDescription
        {
            get { return string.Join(", ", OrderedProducts.Select(ob => ob.Description).ToArray()); }
        }
    }
}