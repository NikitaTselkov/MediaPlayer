using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Models
{
    public class DataBaseControl
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Music;Integrated Security=True";

        /// <summary>
        /// Метод загружающий данные в базу данных.
        /// </summary>
        public void SaveFileToDatabase(string filePath, string playlistName)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = sqlConnection;

                command.CommandText = @"INSERT INTO Playlist VALUES (@Name, @PlaylistName, @MP3)";
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 255);
                command.Parameters.Add("@PlaylistName", SqlDbType.NVarChar, 255);
                command.Parameters.Add("@MP3", SqlDbType.VarBinary);

                // Получаем короткое имя файла для сохранения в бд.
                string shortFileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);

                // Массив для хранения бинарных данных файла.
                byte[] audioData;
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    audioData = new byte[fs.Length];
                    fs.Read(audioData, 0, audioData.Length);
                }

                // Передаем данные в команду через параметры.
                command.Parameters["@Name"].Value = shortFileName;
                command.Parameters["@PlaylistName"].Value = playlistName;
                command.Parameters["@MP3"].Value = audioData;

                command.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Метод получающий данные из базы данных.
        /// </summary>
        public Dictionary<string, List<Audio>> ReadFileFromDatabase()
        {
            var Playlists = new Dictionary<string, List<Audio>>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Playlist";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                string filename;
                string playlistName = "";
                byte[] data;

                Directory.CreateDirectory("Music");

                while (reader.Read())
                {
                    filename = reader.GetString(1);
                    playlistName = reader.GetString(2);
                    data = (byte[])reader.GetValue(3);

                    File.WriteAllBytes($@"Music\{filename}", data);

                    Audio audio = new Audio($@"Music\{filename}");

                    if (!Playlists.Keys.Any(a => a == playlistName))
                    {
                        Playlists.Add(playlistName, new List<Audio>());
                    }

                    Playlists[playlistName].Add(audio);             
                }

                return Playlists;
            }
        }
    }
}
