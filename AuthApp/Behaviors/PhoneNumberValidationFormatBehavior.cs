using System;
using Xamarin.Forms;

using PhoneNumbers;

namespace AuthApp.Behaviors
{
    public class PhoneNumberValidationFormatBehavior : Behavior<Entry>
    {
        AsYouTypeFormatter formatter;

        protected override void OnAttachedTo(Entry entry)
        {
            formatter = new PhoneNumbers.AsYouTypeFormatter("US");
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            formatter = null;
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (args.NewTextValue.Length == 0)
            {
                formatter.Clear();

                ((Entry)sender).Text = args.NewTextValue;
            }
            else
            {

                formatter.Clear();

                string formattedNumber = "";

                foreach (char digit in args.NewTextValue.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", ""))
                {
                    formattedNumber = formatter.InputDigit(digit);
                }



                PhoneNumber phoneNumber = new PhoneNumber.Builder()
                                                         .SetCountryCode(1)
                                                         .SetNationalNumber(ulong.Parse(formattedNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "")))
                                                         .Build();



                bool isValid = PhoneNumberUtil.GetInstance().IsValidNumber(phoneNumber);

                ((Entry)sender).TextColor = isValid ? Color.Green : Color.DarkRed;

                ((Entry)sender).Text = formattedNumber;
            }
        }
    }
}
