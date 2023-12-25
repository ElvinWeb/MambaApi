namespace MambaApi.Helpers
{
    public static class Helper
    {
        public async static Task<string> GetFileName(string rootPath, string folderName, IFormFile imageFile)
        {
            string fileName = imageFile.FileName.Length > 64 ? imageFile.FileName.Substring(imageFile.FileName.Length - 64, 64) : imageFile.FileName;

            fileName = Guid.NewGuid().ToString() + imageFile.FileName;
            string path = Path.Combine(rootPath, folderName, fileName);

            using (FileStream Stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(Stream);
            }

            return fileName;
        }
    }
}
