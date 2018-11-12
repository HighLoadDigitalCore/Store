using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smoking.Models;

namespace Smoking.Models
{
    [MetadataType(typeof(EventCalendarDataAnnotations))]
    public partial class EventCalendar
    {

        public static decimal CurrentDiscountForTrade 
        {
            get
            {
                if (HttpContext.Current.Items.Contains("EventCalendar") &&
                HttpContext.Current.Items["EventCalendar"] is decimal)
                    return (decimal)HttpContext.Current.Items["EventCalendar"];
                else
                {
                    var br = CalculateFromDB(true);
                    if (HttpContext.Current.Items.Contains("EventCalendar"))
                        HttpContext.Current.Items.Remove("EventCalendar");

                    HttpContext.Current.Items.Add("EventCalendar", br);
                    return br;
                }

            }
        }
        public static decimal CurrentDiscountReal 
        {
            get
            {
                if (HttpContext.Current.Items.Contains("CurrentDiscountReal") &&
                HttpContext.Current.Items["CurrentDiscountReal"] is decimal)
                    return (decimal)HttpContext.Current.Items["CurrentDiscountReal"];
                else
                {
                    var br = CalculateFromDB(false);
                    if (HttpContext.Current.Items.Contains("CurrentDiscountReal"))
                        HttpContext.Current.Items.Remove("CurrentDiscountReal");

                    HttpContext.Current.Items.Add("CurrentDiscountReal", br);
                    return br;
                }

            }
        }

        private static decimal CalculateFromDB(bool skipSign)
        {
            var db = new DB();
            var day = (int)DateTime.Now.DayOfWeek;
            if (day == 0)
                day = 7;

            day--;
            var events = db.EventCalendars.Where(
                x => x.DayOfWeek == day && x.StartTime <= DateTime.Now.TimeOfDay && x.EndTime >= DateTime.Now.TimeOfDay);
            return events.Any() ? events.Sum(x => x.PricePercent*(skipSign ? 1 : x.Direction*-1)) : 0;
        }

        public static readonly string[] DayNames = new[]
            {"Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"};
        private SelectList _weekDays;
        public SelectList WeekDays
        {
            get
            {
                return _weekDays ??
                       (_weekDays =
                        new SelectList(
                            new[] {0, 1, 2, 3, 4, 5, 6}.Select(x => new KeyValuePair<int, string>(x, DayNames[x])),
                            "Key", "Value", this.DayOfWeek.ToString()));
            }
        }      
        
        private SelectList _directionList;
        public SelectList DirectionList
        {
            get
            {
                return _directionList ??
                       (_directionList =
                        new SelectList(
                            new[]
                                {
                                    new KeyValuePair<int, string>(1, "Увеличение на %"),
                                    new KeyValuePair<int, string>(-1, "Уменьшение на %")
                                }
                            ,
                             "Key", "Value", (this.Direction == 0 ? 1 : this.Direction).ToString()));
            }
        }

        private TimeSpan? _timeSpan;
        public TimeSpan TimeSpan
        {
            get
            {
                if (!_timeSpan.HasValue)
                    _timeSpan = EndTime.Subtract(StartTime);
                return _timeSpan.Value;
            }
        }
        public int PricePercentInt { get { return (int)PricePercent; } set { PricePercent = value; } }

        public class EventCalendarDataAnnotations
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения"), DisplayName("День недели")]
            public int DayOfWeek { get; set; }

            [DisplayName("Величина изменения цен, %")]
            public decimal PricePercent { get; set; }

            [DisplayName("Величина изменения цен, %")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            [Range(0, int.MaxValue, ErrorMessage = "Значение поля '{0}' должно быть положительным целым числом")]
            public int PricePercentInt { get { return (int)PricePercent; } set { PricePercent = value; } }

            [DisplayName("Способ изменения цены"), DefaultValue(1)]
            public int Direction { get; set; }

            [DisplayName("Время начала действия"), Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
            public TimeSpan StartTime { get; set; }

            [DisplayName("Время завершения действия"), Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
            public TimeSpan EndTime { get; set; }

            [DisplayName("Родительский раздел"), DefaultValue(0)]
            public int EventGroup { get; set; }

            [DisplayName("В момент действия правила отображать сообщение о скидке?"), DefaultValue(true)]
            public string ShowDiscount { get; set; }

        }
    }

}
