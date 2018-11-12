using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Configuration;
using System.Web;

namespace Smoking.Models
{

    [Serializable]
    public class UserDataFromNetwork
    {
        public string network { get; set; }
        public string identity { get; set; }
        public string uid { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string bdate { get; set; }
        public string photo { get; set; }
        public string photo_big { get; set; }
        public string city { get; set; }
        public string profile { get; set; }
        public int verified_email { get; set; }
        public int sex { get; set; }

    }


    public class RegisterModel
    {
        /*
                [Display(Name = "Подтвердите пароль"), DataType(DataType.Password), System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают.")]
                public string ConfirmPassword { get; set; }
        */

        [DataType(DataType.EmailAddress), Required(AllowEmptyStrings = false), Display(Name = "Email"), RegularExpression(@"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-zA-Z0-9]{1}[a-zA-Z0-9\-]{0,62}[a-zA-Z0-9]{1})|[a-zA-Z])\.)+[a-zA-Z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", ErrorMessage = "Необходимо указать Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false), Display(Name = "Пароль"), DataType(DataType.Password), StringLength(100, ErrorMessage = "{0} должен содержать минимум {2} символов.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Имя для отображения")]
        public string Nick { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        public string Patrinomic { get; set; }
        [Display(Name = "Телефон")]
        public string MobilePhone { get; set; }

        public string RedirectURL { get; set; }

        public bool Agreed { get; set; }
    }
    public class LogOnModel
    {
        [Display(Name = "Введите символы, указанные на картинке"), Required(ErrorMessage = "*")]
        public string CaptchaCode { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле \"{0}\""), DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле \"{0}\""), Display(Name = "Логин")]
        public string UserName { get; set; }
        public string Message { get; set; }
    }

    public class ChangePasswordModel
    {
        [Display(Name = "Подтвердите пароль"), System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Пароли не совпадают."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Новый пароль"), DataType(DataType.Password), StringLength(100, ErrorMessage = "{0} должен содержать минимум {2} символов.", MinimumLength = 6), Required]
        public string NewPassword { get; set; }

        [DataType(DataType.Password), Required, Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }
    }

    public class RegModel : AuthModel
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public bool Agreed { get; set; }
    }

    public class AuthModel : CommonFormModel
    {
        public string PageURL { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public string RedirectURL { get; set; }
    }

    public class BackCallModel : CommonFormModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public string Where { get; set; }

    }
    public class CalcModel : CommonFormModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool HasMail { get; set; }
        public string File { get; set; }
        public bool NeedClose { get; set; }
        public string FromProduct { get; set; }
    }

    public class FastOrderModel : CommonFormModel
    {
        public string Type { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string OrgName { get; set; }
        public string OrgPerson { get; set; }
        public string OrgPhone { get; set; }
        public string OrgMail { get; set; }
        public string OrgINN { get; set; }
        public string OrgKPP { get; set; }
        public string OrgOKPO { get; set; }
        public string OrgKorr { get; set; }
        public string OrgAccount { get; set; }
        public string OrgBankName { get; set; }
        public string OrgBik { get; set; }
        public string OrgJurAddr { get; set; }
        public string OrgFaktAddr { get; set; }
        public string OrgDelivAddr { get; set; }

        public string OrgNameB { get; set; }
        public string OrgPersonB { get; set; }
        public string OrgPhoneB { get; set; }
        public string OrgMailB { get; set; }
        public string OrgINNB { get; set; }
        public string OrgKPPB { get; set; }
        public string OrgOKPOB { get; set; }
        public string OrgKorrB { get; set; }
        public string OrgAccountB { get; set; }
        public string OrgBankNameB { get; set; }
        public string OrgBikB { get; set; }
        public string OrgJurAddrB { get; set; }
        public string OrgFaktAddrB { get; set; }
        public string OrgDelivAddrB { get; set; }
        public string OrgTargetPersonB { get; set; }
        public string OrgTargetPassB { get; set; }
        public string OrgTargetTransB { get; set; }


        public string FullNameA { get; set; }
        public string PhoneA { get; set; }
        public string EmailA { get; set; }
        public string AddressA { get; set; }
        public string TargetFullNameA { get; set; }
        public string TargetPassA { get; set; }
        public string TargetAddressA { get; set; }
        public string TargetTransA { get; set; }
        public string TargetFixedA { get; set; }
    }

    public class FeedBackModel:CommonFormModel
    {
        public string TypeName
        {
            get
            {
                if (Type == "offer")
                    return "Предложение";
                if (Type == "thanks")
                    return "Благодарность";
                if (Type == "abuse")
                    return "Жалоба";
                return "";
            }
        }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Order { get; set; }
        public bool Sent { get; set; }
    }

    public class RestoreModel:CommonFormModel
    {
        public string Email { get; set; }
    }

    public class CommonFormModel
    {
        public string ErrorText { get; set; }
        public bool NeedRedirect { get; set; }
    }
}