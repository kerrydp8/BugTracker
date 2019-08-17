using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{
    public class ImageHelpers
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat)
                        || ImageFormat.Png.Equals(img.RawFormat)
                        || ImageFormat.Icon.Equals(img.RawFormat)
                        || ImageFormat.Tiff.Equals(img.RawFormat)
                        || ImageFormat.Bmp.Equals(img.RawFormat)
                        || ImageFormat.Gif.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }
        }

        //Watch video near end to see how he added extension values to config file

        public static bool IsValidAttachment(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    return false;
                }

                if (file.ContentLength > 5 * 1024 * 1024 || file.ContentLength < 1024)
                {
                    return false;
                }
                var valid = IsWebFriendlyImage(file);

                var extensionValid = false;

                var validExtensions = new List<string>();
                validExtensions.Add(".pdf");
                validExtensions.Add(".doc");
                validExtensions.Add(".docx");
                validExtensions.Add(".xls");
                validExtensions.Add(".xlsx");
                validExtensions.Add(".txt");
                validExtensions.Add(".html");
                validExtensions.Add(".xml");
                validExtensions.Add(".json");

                foreach (var fileExtension in validExtensions)
                {
                    if (Path.GetExtension(file.FileName) == fileExtension)
                    {
                        extensionValid = true;
                        break;
                    }
                }

                return IsWebFriendlyImage(file) || extensionValid;
            }
            catch
            {
                return false;
            }

        }
    }

}