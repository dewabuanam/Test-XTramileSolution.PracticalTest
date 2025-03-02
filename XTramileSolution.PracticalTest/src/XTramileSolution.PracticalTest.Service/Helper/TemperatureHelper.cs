namespace XTramileSolution.PracticalTest.Service.Helper
{
    public static class TemperatureHelper
    {
        public static double ConvertFahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5.0 / 9.0;
        }
    }
}