using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedOrg.Models
{
    public class RegisterModel
    {
        [Required]
        [DisplayName("Логин")]

        public string Login { get; set; }

        [Required]
        [DisplayName("Пароль")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [DisplayName("Подтверждение пароля")]

        public string PasswordConfirm { get; set; }
        [Required]
        [DisplayName("Медицинская организация")]
        public string MedOrganization { get; set; }
        [DisplayName("Доступ к модулю прикрепления пациентов")]
        public bool PatientAttach { get; set; }
        [DisplayName("Доступ к контролирующему модулю")]
        public bool Control { get; set; }
        [DisplayName("Доступ к отчетам о прикрепленных пациентах")]
        public bool Reports { get; set; }
    }
}