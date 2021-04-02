﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using MetricsAgent.DAL.Interfaces;
using System.Data.SQLite;


namespace MetricsAgent.DAL.Repositories
{
    // маркировочный интерфейс
    // необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface INetworkRepository : IRepository<NetworkMetric>
    {

    }
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        // наше соединение с базой данных
        private SQLiteConnection connection;

        // инжектируем соединение с базой данных в наш репозиторий через конструктор
        public NetworkMetricsRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public void Create(NetworkMetric item)
        {
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO networkmetrics(fromtime, totime) VALUES(@fromtime, @totime)";

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
            cmd.CommandText = "DELETE FROM networkmetrics WHERE id=@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }

        public void Update(NetworkMetric item)
        {
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на обновление данных
            cmd.CommandText = "UPDATE networkmetrics SET fromtime = @fromtime, totime = @totime WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@fromtime", item.FromTime.TotalSeconds);
            cmd.Parameters.AddWithValue("@totime", item.ToTime.TotalSeconds);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public IList<NetworkMetric> GetAll()
        {
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM networkmetrics";

            var returnList = new List<NetworkMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        // налету преобразуем прочитанные секунды в метку времени
                        FromTime = TimeSpan.FromSeconds(reader.GetInt32(2)),
                        ToTime = TimeSpan.FromSeconds(reader.GetInt32(2))
                    });
                }
            }

            return returnList;
        }

        public NetworkMetric GetById(int id)
        {
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM networkmetrics WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        FromTime = TimeSpan.FromSeconds(reader.GetInt32(0)),
                        ToTime = TimeSpan.FromSeconds(reader.GetInt32(0))
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
