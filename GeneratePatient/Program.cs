﻿// See https://aka.ms/new-console-template for more information
using ModelLibrary.Models;
using System;
using System.Net.Http.Json;
using System.Reflection;

string[] _fio = new string[] {"Сафонова Есения Тимофеевна",
            "Попов Александр Ильич",
            "Соколова Марьям Викторовна",
            "Вдовин Артём Матвеевич",
            "Максимов Павел Даниилович",
            "Жуков Арсений Никитич",
            "Савицкая Евгения Ярославовна",
            "Козлов Алексей Романович",
            "Косарева Кира Александровна",
            "Макаров Артём Егорович",
            "Басова Ева Егоровна",
            "Сорокина Арина Леоновна",
            "Никитина Екатерина Тимофеевна",
            "Попова Светлана Максимовна",
            "Сидоров Иван Александрович",
            "Козлова Елизавета Артёмовна",
            "Мартынов Артём Егорович",
            "Архипов Константин Елисеевич",
            "Петухова Анна Михайловна",
            "Соколова Екатерина Ивановна",
            "Куликов Максим Лукич ",
            "Ильинский Владислав Дмитриевич",
            "Горшкова Малика Максимовна",
            "Баранова Светлана Николаевна",
            "Панин Матвей Михайлович",
            "Агафонов Семён Тимофеевич",
            "Семенов Николай Артурович",
            "Ермакова Василиса Максимовна",
            "Ушакова Ариана Артёмовна",
            "Ильина Арина Ярославовна",
            "Розанова Александра Никитична",
            "Котов Михаил Владимирович",
            "Беспалов Игорь Владиславович",
            "Кожевников Максим Максимович",
            "Моисеев Даниил Владиславович",
            "Панов Тимофей Максимович",
            "Титов Степан Юрьевич",
            "Иванова Дарья Константиновна",
            "Косарева Алиса Сергеевна",
            "Большакова Анна Леоновна",
            "Фролов Михаил Филиппович",
            "Крючков Григорий Миронович",
            "Мельников Никита Львович",
            "Воронов Кирилл Андреевич",
            "Маркина Юлия Дмитриевна",
            "Королева Василиса Александровна",
            "Филиппов Андрей Викторович",
            "Васильева Анна Константиновна",
            "Филатова Василиса Олеговна",
            "Васильев Максим Ярославович",
            "Артемов Семён Романович",
            "Шишкина Полина Данииловна",
            "Кононов Тимур Ильич",
            "Прокофьева Виктория Михайловна",
            "Пирогов Максим Даниилович",
            "Мельников Роман Миронович",
            "Панов Филипп Ильич",
            "Тарасов Мирослав Ильич",
            "Софронов Борис Дамирович",
            "Алексеев Гордей Тимофеевич",
            "Гусев Никита Павлович",
            "Грачева Эмилия Артуровна",
            "Белкин Александр Богданович",
            "Калмыкова София Артемьевна",
            "Самойлова Мария Всеволодовна",
            "Исаев Илья Даниилович",
            "Леонтьев Егор Викторович",
            "Постников Всеволод Павлович",
            "Михайлов Тимур Миронович",
            "Лапшин Тимур Кириллович",
            "Михайлова Анна Данииловна",
            "Лебедев Данила Григорьевич",
            "Кузнецова Вероника Тимофеевна",
            "Воробьев Иван Захарович",
            "Никитин Арсений Маркович",
            "Свиридов Артур Глебович",
            "Попова Ульяна Марковна",
            "Анисимова Дарья Владимировна",
            "Сергеев Михаил Тимофеевич",
            "Журавлева Ольга Алексеевна",
            "Васильева Таисия Марковна",
            "Попова Диана Владимировна",
            "Лебедева Ксения Артёмовна",
            "Лазарева Елизавета Владимировна",
            "Савина Марина Дмитриевна",
            "Романова Владислава Ильинична",
            "Кириллов Макар Дмитриевич",
            "Медведева Ксения Михайловна",
            "Комарова Виктория Михайловна",
            "Александрова Ева Ивановна",
            "Карасева Есения Михайловна",
            "Королев Демид Лукич ",
            "Суханова Софья Фёдоровна",
            "Львов Лев Ильич",
            "Кузнецов Александр Владимирович",
            "Белякова Полина Алексеевна",
            "Мельникова Милана Даниэльевна",
            "Фролова Юлиана Данииловна",
            "Муратов Егор Михайлович",
            "Орлова Ангелина Юрьевна",
        };


Console.WriteLine("Генерация пациентов");

while (true)
{
    Console.WriteLine("Вы хотите сгененировать пациентов? (д/н)");
    var answer = Console.ReadLine();

    if (answer == "д")
    {
        Console.WriteLine("Запуск генерации пациентов");

        var handler = new SocketsHttpHandler
        {
            SslOptions = new()
            {
                RemoteCertificateValidationCallback = delegate { return true; }
            },
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
        using (HttpClient httpClient = new HttpClient(handler))
        {
            httpClient.BaseAddress = new Uri("http://localhost:5353");
            var index = -50;
            var countError = 0;
            foreach (var item in _fio)
            {
                var secondName = item.Split(" ")[0];
                var data = new Patient()
                {
                    Active = true,
                    BirthDate = DateTime.UtcNow.AddDays(index),
                    Gender = (secondName.Last() == char.Parse("а") || new string(secondName.Skip(secondName.Length - 2).ToArray()) == "ая") ? "female" : "male",
                    Name = new()
                    {
                        Family = secondName,
                        Use = "official",
                        Given = item.Split(' ').Skip(1).ToArray()
                    }
                };

                try
                {
                    var result = await httpClient.PostAsJsonAsync("/patient", data);
                    if (result.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Добавлен пациент {0}", item);
                    }
                    else
                    {
                        Console.WriteLine("Пациент {0} не добавлен", item);
                    }
                }
                catch
                {
                    Console.WriteLine("Ошибка добавления пациента {0}", item);
                    countError++;
                }


                if (countError > 5)
                {
                    Console.WriteLine("Превышено максимальное кол-во ошибок, проверьте доступность сервера");
                    break;
                }
                index++;
            }
        }

        Console.WriteLine("Генерации пациентов завершена");

        Console.WriteLine("Завершить работу? (д/н)");
        answer = Console.ReadLine();
        if (answer == "д")
        {
            break;
        }
    }
}
