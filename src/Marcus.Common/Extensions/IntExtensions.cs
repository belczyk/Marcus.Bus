namespace Marcus.Common
{
    public static class IntExtensions
    {
        public static bool IsValidMonth(this int value)
        {
            return value >= 1 && value <= 12;
        }
    }
}