using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Core_Module
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class CoreModuleService : ICoreModuleService
    {
        public Object GetObjectById(int id)
        {
            // Логика получения объекта по идентификатору
        }

        public void AddObject(Object obj)
        {
            // Логика добавления объекта
        }

        // Реализация остальных методов интерфейса
    }
}
