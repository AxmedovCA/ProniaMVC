namespace Pronia.Helpers
{
    public static class ExtensionMethods
    {
        public static bool CheckType(this IFormFile file,string type)
        {
            return file.ContentType.Contains(type); 
        }
        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length<mb*1024*1024;
        }
        public static string SaveFile(this IFormFile file,string path)
        {
            string uniqueName = Guid.NewGuid().ToString() + file.FileName;
            string mainImagePath = Path.Combine(path,  uniqueName);
            using FileStream mainImageStrim = new FileStream(mainImagePath, FileMode.OpenOrCreate);
            file.CopyTo(mainImageStrim);
            return uniqueName;
        }
    }
}
