namespace GpxViewer.Modules.GpxFiles.Views.TourViews
{
    internal static class DesignData
    {
        public static SelectedToursViewModel SelectedToursVM_Normal
        {
            get
            {
                var result = new SelectedToursViewModel();
                return result;
            }
        }

        public static SelectedToursViewModel SelectedToursVM_Error
        {
            get
            {
                var result = new SelectedToursViewModel();
                result.ErrorTextCompact = "Some error happened!";
                return result;
            }
        }
    }
}
