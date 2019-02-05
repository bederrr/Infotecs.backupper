namespace FirstTask
{
    public interface IFileManager
    {
        void CreateCurrentDirectory(string sourceDir, string targetDir);
        void Copy(string sourceDir, string targetDir);
    }
}
