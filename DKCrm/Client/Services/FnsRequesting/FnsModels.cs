namespace DKCrm.Client.Services.FnsRequesting
{
    public class FnsModels
    {
        public class Rootobject
        {
            public Item[] items { get; set; }
        }

        public class Item
        {
            public ЮЛ ЮЛ { get; set; }
        }

        public class ЮЛ
        {
            public string ИНН { get; set; }
            public string КПП { get; set; }
            public string ОГРН { get; set; }
            public string ДатаОГРН { get; set; }
            public string ДатаРег { get; set; }
            public string ОКОПФ { get; set; }
            public string КодОКОПФ { get; set; }
            public string Статус { get; set; }
            public string СпОбрЮЛ { get; set; }
            public НО НО { get; set; }
            public ПФ ПФ { get; set; }
            public ФСС ФСС { get; set; }
            public Кодыстат КодыСтат { get; set; }
            public Капитал Капитал { get; set; }
            public string НаимСокрЮЛ { get; set; }
            public string НаимПолнЮЛ { get; set; }
            public Адрес Адрес { get; set; }
            public Руководитель Руководитель { get; set; }
            public Учредители[] Учредители { get; set; }
            public Контакты Контакты { get; set; }
            public Оснвиддеят ОснВидДеят { get; set; }
            public Допвиддеят[] ДопВидДеят { get; set; }
            public СПВЗ[] СПВЗ { get; set; }
            public Открсведения ОткрСведения { get; set; }
            public Лицензии[] Лицензии { get; set; }
            public Участия[] Участия { get; set; }
            public История История { get; set; }
        }

        public class НО
        {
            public string Рег { get; set; }
            public string РегДата { get; set; }
            public string Учет { get; set; }
            public string УчетДата { get; set; }
        }

        public class ПФ
        {
            public string РегНомПФ { get; set; }
            public string ДатаРегПФ { get; set; }
            public string КодПФ { get; set; }
        }

        public class ФСС
        {
            public string РегНомФСС { get; set; }
            public string ДатаРегФСС { get; set; }
            public string КодФСС { get; set; }
        }

        public class Кодыстат
        {
            public string ОКПО { get; set; }
            public string ОКТМО { get; set; }
            public string ОКФС { get; set; }
            public string ОКОГУ { get; set; }
        }

        public class Капитал
        {
            public string ВидКап { get; set; }
            public string СумКап { get; set; }
            public string Дата { get; set; }
        }

        public class Адрес
        {
            public string КодРегион { get; set; }
            public string Индекс { get; set; }
            public string АдресПолн { get; set; }
            public Адресдетали АдресДетали { get; set; }
            public string Дата { get; set; }
        }

        public class Адресдетали
        {
            public Регион Регион { get; set; }
            public Город Город { get; set; }
            public Улица Улица { get; set; }
            public string Дом { get; set; }
            public string Помещ { get; set; }
        }

        public class Регион
        {
            public string Наим { get; set; }
        }

        public class Город
        {
            public string Тип { get; set; }
            public string Наим { get; set; }
        }

        public class Улица
        {
            public string Тип { get; set; }
            public string Наим { get; set; }
        }

        public class Руководитель
        {
            public string ВидДолжн { get; set; }
            public string Должн { get; set; }
            public string ФИОПолн { get; set; }
            public string ИННФЛ { get; set; }
            public string Пол { get; set; }
            public string ВидГражд { get; set; }
            public string Дата { get; set; }
        }

        public class Контакты
        {
            public string[] Телефон { get; set; }
            public string[] email { get; set; }
        }

        public class Оснвиддеят
        {
            public string Код { get; set; }
            public string Текст { get; set; }
            public string Дата { get; set; }
        }

        public class Открсведения
        {
            public string КолРаб { get; set; }
            public string СведСНР { get; set; }
            public string ПризнУчКГН { get; set; }
            public Налоги[] Налоги { get; set; }
            public string СумДоход { get; set; }
            public string СумРасход { get; set; }
            public Отраслевыепок ОтраслевыеПок { get; set; }
            public string Дата { get; set; }
        }

        public class Отраслевыепок
        {
            public string НалогНагрузка { get; set; }
            public string Рентабельность { get; set; }
        }

        public class Налоги
        {
            public string НаимНалог { get; set; }
            public string СумУплНал { get; set; }
        }

        public class История
        {
            public Руководитель1 Руководитель { get; set; }
            public Номтел НомТел { get; set; }
            public EMail Email { get; set; }
            public Адрес1 Адрес { get; set; }
            public Открсведения1[] ОткрСведения { get; set; }
        }

        public class Руководитель1
        {
            public _2015032620181002 _2015032620181002 { get; set; }
        }

        public class _2015032620181002
        {
            public string ФИОПолн { get; set; }
            public string ИННФЛ { get; set; }
            public string Пол { get; set; }
            public string ВидГражд { get; set; }
            public string Должн { get; set; }
        }

        public class Номтел
        {
            public string _2015032620171211 { get; set; }
        }

        public class EMail
        {
            public string _2016092320171211 { get; set; }
        }

        public class Адрес1
        {
            public _2003090820160922 _2003090820160922 { get; set; }
            public _2016092320171211 _2016092320171211 { get; set; }
            public _2017121220200826 _2017121220200826 { get; set; }
            public _2020072720200826 _2020072720200826 { get; set; }
        }

        public class _2003090820160922
        {
            public string АдресПолн { get; set; }
        }

        public class _2016092320171211
        {
            public string АдресПолн { get; set; }
        }

        public class _2017121220200826
        {
            public string АдресПолн { get; set; }
        }

        public class _2020072720200826
        {
            public string ПризнНедАдресЮЛ { get; set; }
        }

        public class Открсведения1
        {
            public string КолРаб { get; set; }
            public string СведСНР { get; set; }
            public string ПризнУчКГН { get; set; }
            public Налоги1[] Налоги { get; set; }
            public string СумДоход { get; set; }
            public string СумРасход { get; set; }
            public Отраслевыепок1 ОтраслевыеПок { get; set; }
            public string Дата { get; set; }
        }

        public class Отраслевыепок1
        {
            public string НалогНагрузка { get; set; }
            public string Рентабельность { get; set; }
        }

        public class Налоги1
        {
            public string НаимНалог { get; set; }
            public string СумУплНал { get; set; }
            public string СумНедНалог { get; set; }
            public string СумПени { get; set; }
            public string СумШтраф { get; set; }
            public string ОбщСумНедоим { get; set; }
        }

        public class Учредители
        {
            public Учрфл УчрФЛ { get; set; }
            public string СуммаУК { get; set; }
            public string Процент { get; set; }
            public string Дата { get; set; }
        }

        public class Учрфл
        {
            public string ФИОПолн { get; set; }
            public string ИННФЛ { get; set; }
            public string Пол { get; set; }
            public string ВидГражд { get; set; }
        }

        public class Допвиддеят
        {
            public string Код { get; set; }
            public string Текст { get; set; }
            public string Дата { get; set; }
        }

        public class СПВЗ
        {
            public string Дата { get; set; }
            public string Текст { get; set; }
        }

        public class Лицензии
        {
            public string НомерЛиц { get; set; }
            public string ВидДеятельности { get; set; }
            public string ДатаЛиц { get; set; }
            public string ДатаНачала { get; set; }
            public string ЛицОрг { get; set; }
        }

        public class Участия
        {
            public string ОГРН { get; set; }
            public string ИНН { get; set; }
            public string НаимСокрЮЛ { get; set; }
            public string НаимПолнЮЛ { get; set; }
            public string Статус { get; set; }
            public string СуммаУК { get; set; }
        }

    }
}
