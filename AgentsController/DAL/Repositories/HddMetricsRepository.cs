using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public interface IHddMetricsRepository : IRepository<HddMetric>
    {

    }
    public class HddMetricsRepository : IHddMetricsRepository
    {
        // наше соединение с базой данных
        private SQLiteConnection connection;

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public HddMetricsRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public void Create(HddMetric item)
        {
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO hddmetrics(left) VALUES(@left)";

            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@left", item.Left);

            // подготовка команды к выполнению
            cmd.Prepare();

            // выполнение команды
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = "DELETE FROM hddmetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(HddMetric item)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE hddmetrics SET left = @left WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@left", item.Left);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<HddMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM hddmetrics";

            var returnList = new List<HddMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new HddMetric
                    {
                        Id = reader.GetInt32(0),
                        Left = reader.GetInt32(1),
                        
                    });
                }
            }

            return returnList;
        }

        public HddMetric GetById(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM hddmetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new HddMetric
                    {
                        Id = reader.GetInt32(0),
                        Left = reader.GetInt32(1),
                    };
                }
                else
                {
                    // не нашлось запись по идентификатору, не делаем ничего
                    return null;
                }
            }
        }
    }
}
