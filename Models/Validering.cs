using System;
using System.Text.RegularExpressions;

namespace Models
{
    public class Validering
    {

        public bool isMatch(string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            bool isValid = regex.IsMatch(input);
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isMailValid(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string regexStatement = @"^[a-zA-Z0-9æøåÆØÅ]+@[a-zA-Z0-9.-æøåÆØÅ]+\.[a-zA-Z]{2,}$";
                if (isMatch(input, regexStatement))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPhoneValid(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string regexStatement = @"^\d{8}$";
                if (isMatch(input, regexStatement))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isNameValid(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string regexStatement = @"^[a-zA-ZæøåÆØÅ]{2,}$";
                if (isMatch(input, regexStatement))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isAddressValid(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string regexStatement = @"^(?=.*[a-zA-ZæøåÆØÅ])(?=.*\d)[a-zA-ZæøåÆØÅ\d\s]+$";
                if (isMatch(input, regexStatement))
                {
                    return true;
                }
            }
            return false;
        }
    }
}