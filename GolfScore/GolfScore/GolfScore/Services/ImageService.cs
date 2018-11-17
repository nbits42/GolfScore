using Xamarin.Forms;

namespace TeeScore.Services
{

    public static class ImageService
    {
        private const string ImageDir = "TeeScore.Styling.Themes.{0}.Images";
        private const string DefaultTheme = "Default";
        private const string DefaultExtension = "png";
        private static string theme;

        public static string Theme { get => theme ?? DefaultTheme; set => theme = value; }

        public static ImageSource GetImage(ImageType imageType, string name)
        {
            return GetImage(imageType, DeviceType.None, name);
        }

        public static ImageSource GetImage(ImageType imageType, DeviceType deviceType, string name)
        {
            return ImageSource.FromResource(GetImageFile(imageType, deviceType, name));
        }

        public static string GetImageFile(ImageType imageType, string name)
        {
            return GetImageFile(imageType, DeviceType.None, name);
        }

        public static string GetImageFile(ImageType imageType, DeviceType deviceType, string name)
        {
            var imageDir = string.Format(ImageDir, Theme);
            var filename = $"{ImageDir}.{imageType}{(deviceType == DeviceType.None ? string.Empty : "." + deviceType)}.{name}.{DefaultExtension}";
            return filename;
        }
    }

    public enum ImageType
    {
        Icons,
        Style,
        EventInfo,
        SocialMedia
    }

    public enum DeviceType
    {
        None,
        Droid,
        Ios,
        Uwp
    }
}
