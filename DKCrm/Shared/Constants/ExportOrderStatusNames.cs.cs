using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Constants
{
    public static class ExportOrderStatusNames
    {
        public const string BeginFormed = "Формируется";
        public const string ExpectComponents = "Ожидаем компоненты";
        public const string Formed = "Сформирован";
        public const string OfferSentClient = "Предложение отправлено клиенту";
        public const string OfferСonfirmedClient = "Предложение подтверждено клиентом";
        public const string Delivery = "Доставка";
        public const string Completed = "Завершен";
    }
}
