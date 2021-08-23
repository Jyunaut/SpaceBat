namespace Helpers
{
    public static class Extensions
    {
        public static float ToSeconds(this int frames, float animationFPS)
        {
            return frames / animationFPS;
        }

        public static float ToFrames(this float seconds, float animationFPS)
        {
            return (int)(seconds * animationFPS);
        }
    }
}