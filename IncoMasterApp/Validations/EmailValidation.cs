using System.Globalization;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace IncoMasterApp.Validations
{
    public class EmailValidation : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, Message ?? "Email address can not be empty");

            var emailAddress = new EmailAddressAttribute().IsValid(value.ToString());

            if (!emailAddress)
            {
                return new ValidationResult(false, Message ?? "Email address is not valid");
            }
            return ValidationResult.ValidResult;
        }
    }
}
