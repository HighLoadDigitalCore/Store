using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPOI.SS.Formula.Functions;
using Smoking.Extensions;

namespace Smoking.Models
{


/*
    [MetadataType(typeof(ShopCartProfileDataAnnotations))]
    public partial class ShopCartProfile
    {

        public string Login { get; set; }
        public string Password { get; set; }

        public string SurnameAndName
        {
            get { return "{0} {1}".FormatWith(new string[] { User.Profile.Surname, User.Profile.Name }); }
        }

        public string FullName
        {
            get
            {
                var name =
                    "{0} {1} {2}".FormatWith(new string[]
                        {User.Profile.Surname, User.Profile.Name, User.Profile.Patrinomic});
                if (name.IsNullOrEmpty()) return "[Anonimous]";
                return name;
            }
        }

        private MembershipUser user = null;

        public MembershipUser MembershipUser
        {
            get
            {
                if (user == null)
                {

                    user = Membership.GetUser(UserID);
                }
                return user;
            }
            set { user = value; }
        }

        private string _mail;

        public string Email
        {
            get
            {
                if (!_mail.IsNullOrEmpty())
                {
                    return _mail;
                }
                if (MembershipUser != null)
                {
                    return MembershipUser.Email;
                }
                return "";
            }
            set { _mail = value; }
        }

      

        public string FullAdress
        {
            get
            {
                var filled = new List<string>();
                if (ZipCode.IsFilled()) filled.Add(ZipCode);
                if (Region.IsFilled()) filled.Add(Region);
                if (Town.IsFilled()) filled.Add(Town);
                if (Street.IsFilled()) filled.Add(Street);
                if (House.IsFilled()) filled.Add(House);
                if (Building.IsFilled()) filled.Add(Building);
                if (Flat.IsFilled()) filled.Add(Flat);
                if (Doorway.IsFilled()) filled.Add("подъезд " + Doorway);
                if (Floor.IsFilled()) filled.Add("этаж " + Floor);
                return string.Join(", ", filled.ToArray());
            }
        }

        public string FullAdressForPayment
        {
            get
            {
                var filled = new List<string>();
                if (ZipCode.IsFilled()) filled.Add(ZipCode);
                if (Region.IsFilled()) filled.Add(Region);
                if (Town.IsFilled()) filled.Add(Town);
                if (Street.IsFilled()) filled.Add(Street);
                if (House.IsFilled()) filled.Add(House);
                if (Building.IsFilled() && Building != "-") filled.Add(Building);
                if (Flat.IsFilled()) filled.Add(Flat);
                return string.Join(", ", filled.ToArray());
            }
        }

        public string Name
        {
            get { return User.Profile.Name.IsNullOrEmpty() ? User.ShopCartProfile.Name : User.Profile.Name; }
        }

        public string Surname
        {
            get { return User.Profile.Surname.IsNullOrEmpty() ? User.ShopCartProfile.Surname : User.Profile.Surname; }
        }

        public string Patrinomic
        {
            get
            {
                return User.Profile.Patrinomic ?? "";
            }
        }


        public class ShopCartProfileDataAnnotations
        {

            [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            [DisplayName("Пароль *"),
             StringLength(100, ErrorMessage = "{0} должен содержать минимум {2} символов.", MinimumLength = 6)]
            public string Password { get; set; }


            [DisplayName("Логин *")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            public string Login { get; set; }

            [DisplayName("Email*")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            [RegularExpression(
                @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                , ErrorMessage = "Пожалуйста укажите правильный Email адрес")]
            public string Email { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
            [DisplayName("Имя")]
            public string Name { get; set; }

            [DisplayName("Фамилия")]
            public string Surname { get; set; }

            [DisplayName("Отчество")]
            public string Patrinomic { get; set; }

            [DisplayName("Телефон")]
            public string HomePhone { get; set; }

            [DisplayName("Мобильный телефон")]
            public string MobilePhone { get; set; }

            [DisplayName("Регион")]
            public string Region { get; set; }

            [DisplayName("Город")]
            public string Town { get; set; }

            [DisplayName("Адрес")]
            public string Address { get; set; }

            [DisplayName("Улица")]
            public string Street { get; set; }

            [DisplayName("Номер дома")]
            public string House { get; set; }

            [DisplayName("Корпус")]
            public string Building { get; set; }

            [DisplayName("Подъезд")]
            public string Doorway { get; set; }

            [DisplayName("Квартира")]
            public string Flat { get; set; }

            [DisplayName("Индекс")]
            public string ZipCode { get; set; }

            [DisplayName("Этаж")]
            public string Floor { get; set; }

            [DisplayName("Метро")]
            public string Metro { get; set; }

            [DisplayName("Название организации")]
            public string OrgName { get; set; }

            [DisplayName("ИНН")]
            public string OrgINN { get; set; }

            [DisplayName("КПП")]
            public string OrgKPP { get; set; }

            [DisplayName("К/с")]
            public string OrgKS { get; set; }

            [DisplayName("Р/с")]
            public string OrgRS { get; set; }

            [DisplayName("Банк")]
            public string OrgBank { get; set; }

            [DisplayName("БИК")]
            public string OrgBik { get; set; }

            [DisplayName("Юр. адрес")]
            public string OrgJurAddr { get; set; }

            [DisplayName("Факт. адрес")]
            public string OrgFactAddr { get; set; }

            [DisplayName("Генеральный дир.")]
            public string OrgDirector { get; set; }

            [DisplayName("Главный бух.")]
            public string OrgAccountant { get; set; }


        }
    }

*/

    public partial class User
    {
        public bool CanDelete
        {
            get { return UserName.ToLower() != "admin"; }
        }

        public UserProfile Profile
        {
            get
            {
                if (UserProfile == null)
                    return new UserProfile() { User = this };
                return UserProfile;
            }

        }

        public string Phone
        {
            get { 
                var ph = Profile.MobilePhone;
                if (ph.IsNullOrEmpty())
                    ph = Profile.HomePhone;
                if (ph.IsNullOrEmpty())
                    ph = "-не указан-";

                return ph;
            }
        }
    }


    public class ProfileDataAnnotations
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
        [DisplayName("Пароль"), StringLength(100, ErrorMessage = "{0} должен содержать минимум {2} символов.", MinimumLength = 6)]
        public string Password { get; set; }


        [DisplayName("Логин")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
        public string Login { get; set; }

        [DisplayName("Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Пожалуйста укажите правильный Email адрес")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
        [DisplayName("Имя")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле '{0}' обязательно для заполнения")]
        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Отчество")]
        public string Patrinomic { get; set; }

        [DisplayName("Телефон")]
        public string HomePhone { get; set; }

        [DisplayName("Мобильный телефон")]
        public string MobilePhone { get; set; }

/*
        [DisplayName("Регион")]
        public string Region { get; set; }

        [DisplayName("Город")]
        public string Town { get; set; }

        [DisplayName("Адрес")]
        public string Address { get; set; }

        [DisplayName("Улица")]
        public string Street { get; set; }

        [DisplayName("Номер дома")]
        public string House { get; set; }

        [DisplayName("Корпус")]
        public string Building { get; set; }

        [DisplayName("Подъезд")]
        public string Doorway { get; set; }

        [DisplayName("Квартира")]
        public string Flat { get; set; }

        [DisplayName("Индекс")]
        public string ZipCode { get; set; }

        [DisplayName("Этаж")]
        public string Floor { get; set; }


        [DisplayName("Метро")]
        public string Metro { get; set; }

        [DisplayName("Название организации")]
        public string OrgName { get; set; }

        [DisplayName("ИНН")]
        public string OrgINN { get; set; }

        [DisplayName("КПП")]
        public string OrgKPP { get; set; }

        [DisplayName("К/с")]
        public string OrgKS { get; set; }

        [DisplayName("Р/с")]
        public string OrgRS { get; set; }

        [DisplayName("Банк")]
        public string OrgBank { get; set; }

        [DisplayName("БИК")]
        public string OrgBik { get; set; }

        [DisplayName("Юр. адрес")]
        public string OrgJurAddr { get; set; }

        [DisplayName("Факт. адрес")]
        public string OrgFactAddr { get; set; }

        [DisplayName("Генеральный дир.")]
        public string OrgDirector { get; set; }

        [DisplayName("Главный бух.")]
        public string OrgAccountant { get; set; }
*/


    }

}