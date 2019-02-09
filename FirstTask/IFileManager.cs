namespace FirstTask
{
    public interface IFileManager
    {
        void StartBackup(string sourceDir, string targetDir);
        void Copy(string sourceDir, string targetDir);
    }
}
