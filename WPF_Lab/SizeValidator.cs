using System;
using System.Windows.Controls;
using System.Globalization;

namespace WPF_Lab
{
    class SizeValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int size;

            if (!Int32.TryParse((string)value, out size) || size < 0)
            {
                return new ValidationResult(false,"");
            }

            return ValidationResult.ValidResult;
        }
    }
}
