namespace FirstTask
{
    public interface IFileManager
    {
        void Process(string sourceDir, string targetDir);
        void CopyTo(string sourceDir, string targetDir);
    }
}
