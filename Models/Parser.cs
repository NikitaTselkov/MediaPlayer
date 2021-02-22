using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Models
{
    public class Parser
    {
        private string[] Refs;

        private List<string> DownloadedRefs = new List<string>();


        public Parser(string title)
        {
            string url, songName;

            // Получает ссылки на песни.
            Refs = GetMusicFromHotplayerRu(title);

            for (int i = 0; i < Refs.Length; i++)
            {
                // Ссылка.
                url = Refs[i];

                // Получает название песни и группы.
                Match match = Regex.Match(Refs[i], @"(.*?)//(.*?)\.hotplayer\.ru/download/(.*?)/(.*?)/(.*?)/(.*?)\.mp3");

                // Имя для файла.
                songName = Uri.UnescapeDataString(match.Groups[6].Value);

                // Сохраняет mp3 файл на компьютере.
                _ = GetUrlFile(url, $@"Music\{songName}.mp3");
            }

            while (Refs.Length != DownloadedRefs.Count)
            {
                Console.WriteLine($"Загруженно: {DownloadedRefs.Count} из {Refs.Length}");
            }
        }

        /// <summary>
        /// Метод получающий ссылки на скачивание по названию песни или группы.
        /// </summary>
        /// <param name="title"> Название песни или группы. </param>
        private string[] GetMusicFromHotplayerRu(string title)
        {
            List<string> result = new List<string>();

            using (WebClient client = new WebClient())
            {
                var line = client.DownloadString($"https://hotplayer.ru/?s={title}");

                // Находит ссылку на скачивание mp3 и записывает в match.Groups[3].Value.
                Match match = Regex.Match(line, @"<a class=(.*?) title=(.*?) href=(.*?) target=(.*?)></a>");

                while (match.Success)
                {
                    match = match.NextMatch();

                    if (match.Groups[3].Value != "")
                    {
                        result.Add(match.Groups[3].Value.Replace("\"", ""));
                    }
                }
            }

            return result.ToArray();
        }

        // Метод скачивающий файл по ссылке.
        private async Task GetUrlFile(string address, string FileNme)
        {
            using (WebClient client = new WebClient())
            {
                await StartDownload(address, FileNme);
            }
        }

        private Task StartDownload(string link, string filename)
        {
            return Task.Run(() =>
            {
                using (WebClient wc = new WebClient())
                {
                    var downloadTask = wc.DownloadFileTaskAsync(new Uri(link), filename);

                    downloadTask.ContinueWith((Task t) => DownloadedRefs.Add(filename));
                }
            });
        }
    }
}
