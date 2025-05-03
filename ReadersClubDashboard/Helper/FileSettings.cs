﻿namespace ReadersClubDashboard.Helper
{
    public static class FileSettings
    {
        public static string UploadFile(IFormFile file, string folderName,string webRootPath)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);

            }
            return fileName;
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", folderName, fileName);
            if (File.Exists(filepath))
                File.Delete(filepath);
        }
    }
}
