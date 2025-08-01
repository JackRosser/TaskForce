using DevExpress.Blazor;

namespace TaskForce.Client
{
    public static class ToastMessage
    {

        public static ToastOptions Success(string text) => new ToastOptions()
        {
            RenderStyle = ToastRenderStyle.Success,
            ThemeMode = ToastThemeMode.Pastel,
            FreezeOnClick = true,
            Text = text
        };

        public static ToastOptions Error(string text) => new ToastOptions()
        {
            RenderStyle = ToastRenderStyle.Danger,
            ThemeMode = ToastThemeMode.Pastel,
            FreezeOnClick = true,
            Text = text
        };

        public static ToastOptions Warning(string text) => new ToastOptions()
        {
            RenderStyle = ToastRenderStyle.Warning,
            ThemeMode = ToastThemeMode.Pastel,
            FreezeOnClick = true,
            Text = text
        };
    }
}
