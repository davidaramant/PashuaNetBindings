namespace Pashua.BooleanExtensions
{
    public static class Extensions
    {
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static int ToInt(this bool? value)
        {
            return value == true ? 1 : 0;
        }
    }
}