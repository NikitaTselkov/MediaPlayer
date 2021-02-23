using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Models
{
    public enum SongOrPlaylist
    {
        Song,
        Playlist
    }

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

                // Получаем короткое имя файла для сохранения в бд.
                string shortFileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);

                // Массив для хранения бинарных данных файла.
                byte[] audioData;
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    audioData = new byte[fs.Length];
                    fs.Read(audioData, 0, audioData.Length);
                }

                CommandSettings(sqlConnection, shortFileName, playlistName, audioData);
            }

        }

        /// <summary>
        /// Метод загружающий данные в базу данных.
        /// </summary>
        public void SaveFileToDatabase(List<Audio> audios, string playlistName)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                for (int i = 0; i < audios.Count; i++)
                {
                    // Получаем короткое имя файла для сохранения в бд.
                    string shortFileName = $"{audios[i].Name}.mp3";

                    // Массив для хранения бинарных данных файла.
                    byte[] audioData;
                    using (var fs = new FileStream(audios[i].SourceUrl, FileMode.Open, FileAccess.Read))
                    {
                        audioData = new byte[fs.Length];
                        fs.Read(audioData, 0, audioData.Length);
                    }

                    CommandSettings(sqlConnection, shortFileName, playlistName, audioData);
                }         
            }

        }

        /// <summary>
        /// Метод удаляющий данные из базы данных.
        /// </summary>
        public void RemoveFileFromDatabase(string name, SongOrPlaylist songOrPlaylist = SongOrPlaylist.Song)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                if (name.Contains("'"))
                {
                    name = name.Replace("'", "''");
                }

                if (songOrPlaylist == SongOrPlaylist.Song)
                {
                    CommandToDataBase($"DELETE FROM Playlist WHERE Name = '{name}.mp3'", connection);
                }
                else
                {
                    CommandToDataBase($"DELETE FROM Playlist WHERE PlaylistName = '{name}'", connection);
                } 
            }
        }

        /// <summary>
        /// Метод меняющий название плейлиста в базе данных.
        /// </summary>
        public void RenameFileFromDatabase(string oldTitle, string newTitle)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                oldTitle = oldTitle.Replace("'", "''");
                newTitle = newTitle.Replace("'", "''");

                CommandToDataBase($"UPDATE Playlist SET PlaylistName='{newTitle}' WHERE PlaylistName='{oldTitle}'", connection);                        
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

                SqlDataReader reader = CommandToDataBase("SELECT * FROM Playlist", connection);

                string filename;
                string playlistName = "";
                byte[] data;

                Directory.CreateDirectory("Music");

                while (reader.Read())
                {
                    filename = reader.GetString(1);
                    playlistName = reader.GetString(2);
                    data = (byte[])reader.GetValue(3);

                    try
                    {
                        File.WriteAllBytes($@"Music\{filename}", data);

                        Audio audio = new Audio($@"Music\{filename}");

                        if (!Playlists.Keys.Any(a => a == playlistName))
                        {
                            Playlists.Add(playlistName, new List<Audio>());
                        }

                        Playlists[playlistName].Add(audio);
                    }
                    catch (Exception)
                    {
                        //TODO: Исправить.
                    }
                }

                return Playlists;
            }
        }

        private SqlDataReader CommandToDataBase(string sqlCommand, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(sqlCommand, connection);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        private void CommandSettings(SqlConnection sqlConnection, string shortFileName, string playlistName, byte[] audioData)
        {
            SqlCommand command = new SqlCommand();

            command.CommandText = @"INSERT INTO Playlist VALUES (@Name, @PlaylistName, @MP3)";
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 255);
            command.Parameters.Add("@PlaylistName", SqlDbType.NVarChar, 255);
            command.Parameters.Add("@MP3", SqlDbType.VarBinary);

            command.Connection = sqlConnection;

            // Передаем данные в команду через параметры.
            command.Parameters["@Name"].Value = shortFileName;
            command.Parameters["@PlaylistName"].Value = playlistName;
            command.Parameters["@MP3"].Value = audioData;

            command.ExecuteNonQuery();
        }
    }
}
