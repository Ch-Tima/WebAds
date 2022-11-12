using System.Security.Cryptography;
using System.Text;

namespace WebAds.Helpers
{
    public static class FilesHelper
    {
        public static async Task<bool> SaveFile(this IFormFile file, string path)
        {
            try
            {
                if (file == null || path == null)
                    return false;

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool DeleteFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string RandomName()
        {
            try
            {
                using (var sha = SHA256.Create())
                {
                    var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                    var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hash;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
