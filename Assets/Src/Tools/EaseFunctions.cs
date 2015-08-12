namespace GlobalPlay.Tools
{
    internal class EaseFunctions
    {
        static EaseFunctions()
        {
            EaseBackForIconsParam = 2.701f;
        }

        public static float EaseBackForIconsParam { get; set; }

        public static float EaseOutBackForIcons(float start, float end, float value)
        {
            end -= start;
            value = value - 1;
            return end*(value*value*((EaseBackForIconsParam + 1)*value + EaseBackForIconsParam) + 1) + start;
        }
    }
}