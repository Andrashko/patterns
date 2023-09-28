using System.Collections.Generic;
using System.Text;
using System.Xml;

/*
* Розробити клас підсистеми для завантаження даних про події (дата події, опис) 
з файлів в форматі JSON та XML. Передбачити можливість існування двох видів подій:
 Зустріч, яка має час початку, час завершення та День народження, 
 яка має дату та контакти іменинника (Ім'я, номер телефону). 
 Визначення типу файлу визначається при запуску системи.
 Підсистема генерує список подій на основі наданих файлів.
*/

// var events = new List<Event>(){
//     new Event() {
//         date = new DateOnly (2023, 9, 28),
//         description = "Лабораторна робота"
//     },
//     new BirthdayEvent (){
//         date = new DateOnly(1988,6,3),
//         description = "День народження",
//         name ="Юрій",
//         phone="0501234567"
//     },
//     new MeetingEvent(){
//         date = new DateOnly (2023, 9, 28),
//         description = "Лабораторна робота 2",
//         begin = new TimeOnly(9,40),
//         end = new TimeOnly(11,00)
//     }
// };

string fileName = "events.json";


EventReaderSystem reader = EventReaderSystem.Create(fileName);
var events = reader.ReadFromFile(fileName);

foreach (var e in events)
    Console.WriteLine(e);
Console.ReadLine();