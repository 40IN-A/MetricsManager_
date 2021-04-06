﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL.Models;
using System.Data.SQLite;


namespace MetricsAgent.DAL.Repositories
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
    {

    }
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        // наше соединение с базой данных
        private SQLiteConnection connection;

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public DotNetMetricsRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public void Create(DotNetMetric item)
        {
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO dotnetmetrics(count, fromtime, totime) VALUES(@count, @fromtime, @totime)";

            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@count", item.Count);

            // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
            // через свойство
            cmd.Parameters.AddWithValue("@fromtime", item.FromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", item.ToTime.TotalSeconds);//
            // подготовка команды к выполнению
            cmd.Prepare();

            // выполнение команды
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на удаление данных
            cmd.CommandText = "DELETE FROM dotnetmetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(DotNetMetric item)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE dotnetmetrics SET count = @count, fromtime = @fromtime, totime = @totime WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@value", item.Count);
            cmd.Parameters.AddWithValue("@fromtime", item.FromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", item.ToTime.TotalSeconds);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<DotNetMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM dotnetmetrics";

            var returnList = new List<DotNetMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Count = reader.GetInt32(1),
                        // налету преобразуем прочитанные секунды в метку времени
                        FromTime = TimeSpan.FromSeconds(reader.GetInt32(2)),
                        ToTime = TimeSpan.FromSeconds(reader.GetInt32(3))
                    });
                }
            }

            return returnList;
        }

        public DotNetMetric GetById(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM dotnetmetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new DotNetMetric
                    {
                        Id = reader.GetInt32(0),
                        Count = reader.GetInt32(1),
                        FromTime = TimeSpan.FromSeconds(reader.GetInt32(2)),
                        ToTime = TimeSpan.FromSeconds(reader.GetInt32(3))
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
