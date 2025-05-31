namespace SrManoelLoja.SeedWork.Validate
{
    public static class Validate
    {
        public static void NotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(parameterName, $"{parameterName} cannot be null or empty.");
            }
        }

        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ValidationException(parameterName, $"{parameterName} cannot be null.");
            }
        }

        public static void Range(decimal value, int minimum, int maximum, string parameterName)
        {
            if (value < minimum || value > maximum)
            {
                throw new ValidationException(parameterName,
                    $"{parameterName} must be between {minimum} and {maximum}. Current value: {value}");
            }
        }

        public static void GreaterThan(int value, int minimum, string parameterName)
        {
            if (value <= minimum)
            {
                throw new ValidationException(parameterName, $"{parameterName} must be greater than {minimum}. Current value: {value}");
            }
        }

        public static void GreaterThanDecimal(decimal value, int minimum, string parameterName)
        {
            if (value <= minimum)
            {
                throw new ValidationException(parameterName, $"{parameterName} must be greater than {minimum}. Current value: {value}");
            }
        }

        public static void MaxLength(string value, int maxLength, string parameterName)
        {
            if (value != null && value.Length > maxLength)
            {
                throw new ValidationException(parameterName,
                    $"{parameterName} must not exceed {maxLength} characters. Current length: {value.Length}");
            }
        }
    }
}
