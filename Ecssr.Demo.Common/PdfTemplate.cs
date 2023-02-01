namespace Ecssr.Demo.Common
{
    public interface IFileTemplate
    {
        string A4 { get; }
        string Mobile { get; }
    }

    public class FileTemplate : IFileTemplate
    {
        string _a4;
        string _mobile;
        public FileTemplate(AppSetting appSetting)
        {
            _a4 = File.ReadAllText(appSetting.PdfTemplatePath.A4);
            _mobile = File.ReadAllText(appSetting.PdfTemplatePath.Mobile);
        }

        public string A4 => _a4;
        public string Mobile => _mobile;
    }
}
