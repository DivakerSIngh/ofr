using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.DataManager
{
    public class ImageDataManager
    {

        #region TempCode

        public static void ImageWork(List<string> imageUrls)
        {
            Parallel.ForEach(imageUrls, new ParallelOptions { MaxDegreeOfParallelism = 10 }, DownloadAndSaveFile);
        }

        private static void DownloadAndSaveFile(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        // If you want it as Png
                        yourImage.Save("path_to_your_file.png", ImageFormat.Png);

                        // If you want it as Jpeg
                        yourImage.Save("path_to_your_file.jpg", ImageFormat.Jpeg);
                    }
                }
            }
        }

        #endregion

    }
}
