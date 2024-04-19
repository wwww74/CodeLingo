using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace language_app.Models
{
    public class DB
    {
        public NpgsqlConnection conn;
        public const string CONN_STRING = @"host=79.174.88.146; port=17713; database=CodeLingoDB; username=www74; password=ProAwpGGX_1488;";

        public DB()
        {
            try
            {
                var isConnected = CrossConnectivity.Current.IsConnected;

                if (isConnected)
                {
                    conn = new NpgsqlConnection(CONN_STRING);
                }
                else
                {
                    return;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Open();
                Console.WriteLine("CONNECTION GOOD");
            }
        }

        public async Task Add_Course(DB_Course course)
        {
            string commandText = "INSERT INTO Courses (image_course, name_course, actual_lesson, general_lesson) VALUES (@img, @name, @act, @gen)";
            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("img", course.Image);
                cmd.Parameters.AddWithValue("name", course.Name);
                cmd.Parameters.AddWithValue("act", course.Actual_lesson);
                cmd.Parameters.AddWithValue("gen", course.General_lesson);

                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }

        public async Task<DB_Course> GetCourse(string username, int id)
        {
            string commandText = "SELECT * FROM courses WHERE id_course IN (SELECT id_course FROM users_course WHERE username = @username) AND id_course = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Course course = ReadCourse(reader);
                    return course;
                }
            }
            return null;
        }
        public async Task<DB_Course> GetAllCourse(string username, int id)
        {
            string commandText = "SELECT DISTINCT * FROM courses WHERE id_course NOT IN (select case when ((SELECT id_course FROM users_course WHERE username = @username and id_course = @id) IS NULL) then 0 else (SELECT id_course FROM users_course WHERE username = @username and id_course = @id) end) and id_course = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Course course = ReadCourse(reader);
                    return course;
                }
            }
            return null;
        }

        public async Task<long?> GetCountActiveCourse(string username)
        {
            string commandText = "select COUNT(*) from users_course where username = @username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? count = reader["count"] as long?;
                    return count;
                }
            }
            return null;
        }

        public async Task<int?> GetCountInactiveCourse(string username)
        {
            string commandText = "select case when ((SELECT COUNT(*) FROM users_course, courses WHERE users_course.username = @username AND courses.id_course NOT IN (SELECT id_course FROM users_course WHERE username = @username)) = 0) then 0 else 1 end";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int? count = reader["case"] as int?;
                    return count;
                }
            }
            return null;
        }
        public async Task<long?> GetCountAllCourse()
        {
            string commandText = "select COUNT(*) from courses";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? count = reader["count"] as long?;
                    return count;
                }
            }
            return null;
        }

        private static DB_Course ReadCourse(NpgsqlDataReader reader)
        {
            int? id = reader["id_course"] as int?;
            string img = reader["image_course"] as string;
            string name = reader["name_course"] as string;
            int? act_less = reader["actual_lesson"] as int?;
            int? gen_less = reader["general_lesson"] as int?;

            DB_Course course = new DB_Course
            {
                ID = id.Value,
                Image = img,
                Name = name,
                Actual_lesson = act_less.Value,
                General_lesson = gen_less.Value,
            };
            return course;
        }

        public async Task Update(int id, DB_Course course)
        {
            var commandText = @"UPDATE Courses SET image_course = @img, name_course = @name, actual_lesson = @act_less, general_less = @gen_less WHERE id_course = @id";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("img", course.Image);
                cmd.Parameters.AddWithValue("name", course.Name);
                cmd.Parameters.AddWithValue("act_less", course.Actual_lesson);
                cmd.Parameters.AddWithValue("gen_less", course.General_lesson);

                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
        public async Task UpdateUserInfo(string first_name, string mail, string number, string username)
        {
            var commandText = @"UPDATE users SET first_name = @first_name, mail = @mail, phone = @num WHERE username = @username";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("first_name", first_name);
                cmd.Parameters.AddWithValue("mail", mail);
                cmd.Parameters.AddWithValue("num", number);
                cmd.Parameters.AddWithValue("username", username);

                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
        public async Task UpdateStatusLesson(string username, int id_part, int id_lesson)
        {
            var commandText = @"UPDATE users_lesson SET lesson_accept = true WHERE username = @username AND id_part = @id_part AND id_lesson = @id_lesson";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_part", id_part);
                cmd.Parameters.AddWithValue("id_lesson", id_lesson);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task InsertNextLesson(string username, int id_part, int id_lesson)
        {
            var commandText = @"INSERT INTO users_lesson (username, id_part, id_lesson) values (@username, @id_part, @id_lesson)";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_part", id_part);
                cmd.Parameters.AddWithValue("id_lesson", id_lesson);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task InsertCourse(string username, int id)
        {
            var commandText = @"INSERT INTO users_course (username, id_course) values (@username, @id)";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task Delete(int id)
        {
            string commandText = $"DELETE FROM Course WHERE ID=(@id)";
            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
        private static User ReadUser(NpgsqlDataReader reader)
        {
            int? id = reader["user_id"] as int?;
            string username = reader["username"] as string;
            string password = reader["user_pass"] as string;
            string mail = reader["mail"] as string;
            string phone = reader["phone"] as string;
            string first_name = reader["first_name"] as string;

            User user = new User
            {
                Id = id.Value,
                Username = username,
                Password = password,
                Email = mail,
                Phone = phone,
                First_Name = first_name
            };
            return user;
        }

        public async Task<User> GetUser(string username)
        {
            string commandText = "select * from users where username = @username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    User user = ReadUser(reader);
                    return user;
                }
            }
            return null;
        }
        public async Task Add_User(User user)
        {
            string commandText = "INSERT INTO Users (username, user_pass) VALUES (@username, @user_pass)";
            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("user_pass", user.Password);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<DB_Part> GetActivePart(string username, int id_course, int id_part)
        {
            string commandText = "SELECT * FROM parts WHERE id_part IN (SELECT id_part FROM users_parts WHERE username = @username AND id_course = @id_course AND id_part = @id_part) AND id_course = @id_course AND id_part = @id_part";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_course", id_course);
                cmd.Parameters.AddWithValue("id_part", id_part);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Part part = ReadParts(reader);
                    return part;
                }
            }
            return null;
        }
        public async Task<DB_Part> GetInactivePart(string username, int id_course, int id_part)
        {
            string commandText = "SELECT * FROM parts WHERE id_part NOT IN (SELECT id_part FROM users_parts WHERE username = @username AND id_course = @id_course AND id_part = @id_part) AND id_course = @id_course AND id_part = @id_part";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_course", id_course);
                cmd.Parameters.AddWithValue("id_part", id_part);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Part part = ReadParts(reader);
                    return part;
                }
            }
            return null;
        }
        public async Task<long?> GetCountAllPartsWhereID(int id)
        {
            string commandText = "select COUNT(*) from parts WHERE id_course = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? count = reader["count"] as long?;
                    return count;
                }
            }
            return null;
        }
        public async Task<int?> GetCountAllParts(int id_course)
        {
            string commandText = "SELECT MAX(id_part) FROM parts WHERE id_course = @id_course";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id_course", id_course);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int? count = reader["max"] as int?;
                    return count;
                }
            }
            return null;
        }
        public async Task<int> GetCountPartWithCourseID(int id)
        {
            string commandText = "SELECT MIN(id_part) FROM parts WHERE id_course = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int min_count = (int)reader["min"];
                    return min_count;
                }
            }
            return 0;
        }
        private static DB_Part ReadParts(NpgsqlDataReader reader)
        {
            int? id = reader["id_part"] as int?;
            int? id_course = reader["id_course"] as int?;
            string name = reader["name_part"] as string;

            DB_Part part = new DB_Part
            {
                ID = id.Value,
                ID_course = id_course.Value,
                Name = name
            };
            return part;
        }
        public async Task<int?> CheckStartPart(int id)
        {
            string commandText = "SELECT MIN(id_part) FROM parts WHERE id_course = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int? min_count = reader["min"] as int?;
                    return min_count;
                }
            }
            return null;
        }
        public async Task<int?> CheckStartLesson(int? id)
        {
            string commandText = "SELECT MIN(id_lesson) FROM lessons WHERE id_part = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int? min_count = reader["min"] as int?;
                    return min_count;
                }
            }
            return null;
        }
        public async Task AddFirstPart(string username, int id_course, int? id_part)
        {
            var commandText = @"INSERT INTO users_parts (username, id_course, id_part) VALUES (@username, @id_course, @id_part)";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_course", id_course);
                cmd.Parameters.AddWithValue("id_part", id_part);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task AddFirstLesson(string username, int? id_part, int? id_lesson)
        {
            var commandText = @"INSERT INTO users_lesson (username, id_part, id_lesson) VALUES (@username, @id_part, @id_lesson)";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_part", id_part);
                cmd.Parameters.AddWithValue("id_lesson", id_lesson);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task AddFavouriteCourse(string username, int id)
        {
            string commandText = "INSERT INTO users_fav (username, id_course) VALUES (@username, @id)";
            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);

                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
        public async Task RemoveFavouriteCourse(string username, int id)
        {
            string commandText = "DELETE FROM users_fav WHERE username = @username AND id_course = @id";
            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);

                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
        public async Task<bool?> CheckAddedFav(string username, int id)
        {
            string commandText = "SELECT CASE WHEN ((SELECT id_course FROM users_fav WHERE username = @username AND id_course = @id) IS NOT NULL) THEN true ELSE false END";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    bool? status = reader["case"] as bool?;
                    return status;
                }
            }
            return null;
        }
        public async Task<int?> GetCountLessonWithPartID(int id)
        {
            string commandText = "SELECT MIN(id_lesson) FROM lessons WHERE id_part = @id";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int? min_count = reader["min"] as int?;
                    return min_count;
                }
            }
            return null;
        }
        public async Task<int?> GetCountAllLesson(int id_part)
        {
            string commandText = "SELECT MAX(id_lesson) FROM lessons WHERE id_part = @id_part";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id_part", id_part);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int? min_count = reader["max"] as int?;
                    return min_count;
                }
            }
            return null;
        }
        public async Task<DB_Lesson> GetActiveLesson(string username, int id_part, int? id_lesson)
        {
            string commandText = "SELECT * FROM lessons WHERE id_lesson IN (SELECT id_lesson FROM users_lesson WHERE username = @username AND id_part = @id_part AND id_lesson = @id_lesson) AND id_lesson = @id_lesson";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_part", id_part);
                cmd.Parameters.AddWithValue("id_lesson", id_lesson);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Lesson part = ReadLesson(reader);
                    return part;
                }
            }
            return null;
        }
        public async Task<DB_Lesson> GetInactiveLesson(string username, int id_part, int? id_lesson)
        {
            string commandText = "SELECT * FROM lessons WHERE id_lesson NOT IN (SELECT id_lesson FROM users_lesson WHERE username = @username AND id_part = @id_part AND id_lesson = @id_lesson) AND id_part = @id_part AND id_lesson = @id_lesson";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_part", id_part);
                cmd.Parameters.AddWithValue("id_lesson", id_lesson);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Lesson part = ReadLesson(reader);
                    return part;
                }
            }
            return null;
        }
        public async Task<long?> GetCountPassedLesson(string username, int id_course)
        {
            string commandText = "SELECT COUNT(*) FROM users_lesson WHERE id_part IN (SELECT id_part FROM parts WHERE id_course = @id_course) AND username = @username AND lesson_accept = true";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_course", id_course);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? min_count = reader["count"] as long?;
                    return min_count;
                }
            }
            return null;
        }
        public async Task<long?> GetCountAllLessonCourse(int id_course)
        {
            string commandText = "SELECT COUNT(*) FROM lessons, parts WHERE lessons.id_part = parts.id_part AND parts.id_course = @id_course";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("id_course", id_course);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? min_count = reader["count"] as long?;
                    return min_count;
                }
            }
            return null;
        }
        private static DB_Lesson ReadLesson(NpgsqlDataReader reader)
        {
            int? id = reader["id_lesson"] as int?;
            int? id_part = reader["id_part"] as int?;
            string name = reader["name_lesson"] as string;
            bool? type = reader["type_lesson"] as bool?;

            DB_Lesson lesson = new DB_Lesson
            {
                ID = id.Value,
                ID_part = id_part.Value,
                Name = name,
                Type = type.Value
            };
            return lesson;
        }

        public async Task<DB_Course> GetFavCourse(string username, int id_course)
        {
            string commandText = "SELECT * FROM courses WHERE id_course IN (SELECT id_course FROM users_fav WHERE username = @username AND id_course = @id_course) AND id_course = @id_course";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_course", id_course);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Course fav_course = ReadCourse(reader);
                    return fav_course;
                }
            }
            return null;
        }
        public async Task<long?> GetCountAllFavCourse(string username)
        {
            string commandText = "SELECT COUNT(*) FROM users_fav WHERE username = @username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? count = reader["count"] as long?;
                    return count;
                }
            }
            return null;
        }
        public async Task<bool?> CheckFavCourse(string username, int id_course)
        {
            string commandText = "SELECT CASE WHEN ((SELECT COUNT(*) FROM users_fav WHERE username = @username AND id_course = @id_course) = 1) THEN true ELSE false END";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_course", id_course);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    bool? count = reader["case"] as bool?;
                    return count;
                }
            }
            return null;
        }
        public async Task<DB_Achiev> GetOpenAchievement(string username, int id_achiev)
        {
            string commandText = "SELECT * FROM achievements WHERE id_achiev IN (SELECT id_achiev FROM users_achiev WHERE username = @username AND id_achiev = @id_achiev) AND id_achiev = @id_achiev";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_achiev", id_achiev);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Achiev fav_course = ReadAchiev(reader);
                    return fav_course;
                }
            }
            return null;
        }
        public async Task<DB_Achiev> GetCloseAchievement(string username, int id_achiev)
        {
            string commandText = "SELECT * FROM achievements WHERE id_achiev NOT IN (SELECT id_achiev FROM users_achiev WHERE username = @username AND id_achiev = @id_achiev) AND id_achiev = @id_achiev";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id_achiev", id_achiev);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    DB_Achiev fav_course = ReadAchiev(reader);
                    return fav_course;
                }
            }
            return null;
        }
        public async Task<long?> GetCountAllAchiev()
        {
            string commandText = "select COUNT(*) from achievements";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? count = reader["count"] as long?;
                    return count;
                }
            }
            return null;
        }
        public async Task<long?> GetCountOpenAchiev(string username)
        {
            string commandText = "select COUNT(*) from users_achiev where username = @username";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long? count = reader["count"] as long?;
                    return count;
                }
            }
            return null;
        }
        public async Task AddAchiev(string username, int id)
        {
            string commandText = "INSERT INTO users_achiev (username, id_achiev) VALUES (@username, @id)";
            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("id", id);

                await cmd.ExecuteNonQueryAsync();
            }
            conn.Close();
        }
        private static DB_Achiev ReadAchiev(NpgsqlDataReader reader)
        {
            int? id = reader["id_achiev"] as int?;
            string name = reader["name"] as string;
            string image = reader["image"] as string;
            string description = reader["description"] as string;

            DB_Achiev achiev = new DB_Achiev
            {
                ID = id.Value,
                Name = name,
                Image = image,
                Description = description
            };
            return achiev;
        }
    }
}
