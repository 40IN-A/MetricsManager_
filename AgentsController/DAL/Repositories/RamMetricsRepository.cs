using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {

    }
    public class RamMetricsRepository : IRamMetricsRepository
    {
        // наше соединение с базой данных
        private SQLiteConnection connection;

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public RamMetricsRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public void Create(RamMetric item)
        {
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO rammetrics(available) VALUES(@available)";

            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@available", item.Available);

            // подготовка команды к выполнению
            cmd.Prepare();

            // выполнение команды
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = "DELETE FROM rammetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(RamMetric item)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE rammetrics SET available = @available WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@available", item.Available);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<RamMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM rammetrics";

            var returnList = new List<RamMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Available = reader.GetInt32(1),

                    });
                }
            }

            return returnList;
        }

        public RamMetric GetById(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM rammetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new RamMetric
                    {
                        Id = reader.GetInt32(0),
                        Available = reader.GetInt32(0),
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
