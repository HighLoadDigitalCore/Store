using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Smoking.Extensions;

namespace Smoking.Models
{
    public class Currency
    {
        private static int? _cacheDuration;
        private static int CACHE_DURATION
        {
            get
            {
                if (!_cacheDuration.HasValue)
                {
                    _cacheDuration = ConfigurationManager.AppSettings["CacheDuration"].ToNullInt() ?? 5;
                }
                return _cacheDuration.Value;
            }
        }

        public decimal Rate
        {
            get
            {
                if (HttpContext.Current!=null && HttpContext.Current.Cache.Get("CurrencyRate") is decimal)
                {
                    return (decimal)HttpContext.Current.Cache.Get("CurrencyRate");
                }
                decimal rate = 0;
                var sRate = SiteSetting.Get<string>("CurrencyRate");
                var auto = SiteSetting.Get<bool>("AutoCurrencyRate");
                if (auto)
                {
                    try
                    {
                        rate =
                            (decimal)
                                new CBRF.DailyInfoSoapClient().GetCursOnDate(DateTime.Now).Tables[0].Rows.Cast<DataRow>()
                                    .First(x => x[4].ToString() == "EUR")[2];
                    }
                    catch
                    {
                        decimal.TryParse(sRate, out rate);
                        if (rate == 0)
                        {
                            rate = 69;
                        }
                    }
                }
                else
                {
                    decimal.TryParse(sRate, out rate);
                    if (rate == 0)
                    {
                        try
                        {
                            rate =
                                (decimal)
                                    new CBRF.DailyInfoSoapClient().GetCursOnDate(DateTime.Now).Tables[0].Rows
                                        .Cast<DataRow>()
                                        .First(x => x[4].ToString() == "EUR")[2];
                        }
                        catch
                        {
                            rate = 69;
                        }
                    }
                }

                SiteSetting.Set("CurrencyRate", rate.ToString());
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Add("CurrencyRate", rate, null, Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, CACHE_DURATION, 0)
                        , CacheItemPriority.Normal, null);
                }

                return rate;

            }
        }
    }
}