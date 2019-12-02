using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    /// <summary>Модель-представления регистрации пользователя в системе</summary>
    public class RegistrerUserViewModel
    {
        /// <summary>Имя пользователя</summary>
        [RegularExpression(@"([A-Za-z][A-Za-z0-9_]{2,255})", ErrorMessage = "Неверный формат имени пользователя")]
        [Display(Name = "Имя пользователя"), MaxLength(256, ErrorMessage = "Максимальная длина 256 символов")]
        [Remote("IsNameFree", "Account", ErrorMessage = "Данное имя пользователя уже занято")]
        public string UserName { get; set; }

        /// <summary>Пароль</summary>
        [Display(Name = "Пароль"), Required, DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>Подтверждение пароля</summary>
        [Display(Name = "Подтверждение пароля"), Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
