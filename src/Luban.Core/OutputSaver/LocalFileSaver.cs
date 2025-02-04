using Luban.Core.Utils;

namespace Luban.Core.OutputSaver;

[OutputSaver("local")]
public class LocalFileSaver : OutputSaverBase
{
    private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

    protected override void BeforeSave(OutputFileManifest outputFileManifest, string outputDir)
    {
        FileCleaner.Clean(outputDir, outputFileManifest.DataFiles.Select(f => f.File).ToList());
    }

    public override void SaveFile(OutputFileManifest fileManifest, string outputDir, OutputFile outputFile)
    {
        string fullOutputPath = $"{outputDir}/{outputFile.File}";
        Directory.CreateDirectory(Path.GetDirectoryName(fullOutputPath));
        if (FileUtil.WriteAllBytes(fullOutputPath, outputFile.GetContentBytes()))
        {
            s_logger.Info("save file:{} ", fullOutputPath);
        }
    }
}