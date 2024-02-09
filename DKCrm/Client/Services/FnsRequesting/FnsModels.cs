namespace DKCrm.Client.Services.FnsRequesting
{
    public class FnsModels
    {
        public class Rootobject
        {
            public Item[] items { get; set; } = null!;
        }

        public class Item
        {
            public ЮЛ ЮЛ { get; set; } = null!;
        }

        public class ЮЛ
        {
            public string ИНН { get; set; } = null!;
            public string КПП { get; set; } = null!;
            public string ОГРН { get; set; } = null!;
            public string ДатаОГРН { get; set; } = null!;
            public string ДатаРег { get; set; } = null!;
            public string ОКОПФ { get; set; } = null!;
            public string КодОКОПФ { get; set; } = null!;
            public string Статус { get; set; } = null!;
            public string СпОбрЮЛ { get; set; } = null!;
            public НО НО { get; set; } = null!;
            public ПФ ПФ { get; set; } = null!;
            public ФСС ФСС { get; set; } = null!;
            public Кодыстат КодыСтат { get; set; } = null!;
            public Капитал Капитал { get; set; } = null!;
            public string НаимСокрЮЛ { get; set; } = null!;
            public string НаимПолнЮЛ { get; set; } = null!;
            public Адрес Адрес { get; set; } = null!;
            public Руководитель Руководитель { get; set; } = null!;
            public Учредители[] Учредители { get; set; } = null!;
            public Контакты Контакты { get; set; } = null!;
            public Оснвиддеят ОснВидДеят { get; set; } = null!;
            public Допвиддеят[] ДопВидДеят { get; set; } = null!;
            public СПВЗ[] СПВЗ { get; set; } = null!;
            public Открсведения ОткрСведения { get; set; } = null!;
            public Лицензии[] Лицензии { get; set; } = null!;
            public Участия[] Участия { get; set; } = null!;
            public История История { get; set; } = null!;
        }

        public class НО
        {
            public string Рег { get; set; } = null!;
            public string РегДата { get; set; } = null!;
            public string Учет { get; set; } = null!;
            public string УчетДата { get; set; } = null!;
        }

        public class ПФ
        {
            public string РегНомПФ { get; set; } = null!;
            public string ДатаРегПФ { get; set; } = null!;
            public string КодПФ { get; set; } = null!;
        }   

        public class ФСС
        {
            public string РегНомФСС { get; set; } = null!;
            public string ДатаРегФСС { get; set; } = null!;
            public string КодФСС { get; set; } = null!;
        }

        public class Кодыстат
        {
            public string ОКПО { get; set; } = null!;
            public string ОКТМО { get; set; } = null!;  
            public string ОКФС { get; set; } = null!;
            public string ОКОГУ { get; set; } = null!;
        }

        public class Капитал
        {
            public string ВидКап { get; set; } = null!;
            public string СумКап { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Адрес
        {
            public string КодРегион { get; set; } = null!;
            public string Индекс { get; set; } = null!;
            public string АдресПолн { get; set; } = null!;
            public Адресдетали АдресДетали { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Адресдетали
        {
            public Регион Регион { get; set; } = null!;
            public Город Город { get; set; } = null!;
            public Улица Улица { get; set; } = null!;
            public string Дом { get; set; } = null!;
            public string Помещ { get; set; } = null!;
        }

        public class Регион
        {
            public string Наим { get; set; } = null!;
        }

        public class Город
        {
            public string Тип { get; set; } = null!;
            public string Наим { get; set; } = null!;
        }

        public class Улица
        {
            public string Тип { get; set; } = null!;
            public string Наим { get; set; } = null!;
        }

        public class Руководитель
        {
            public string ВидДолжн { get; set; } = null!;
            public string Должн { get; set; } = null!;
            public string ФИОПолн { get; set; } = null!;
            public string ИННФЛ { get; set; } = null!;
            public string Пол { get; set; } = null!;
            public string ВидГражд { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Контакты
        {
            public string[] Телефон { get; set; } = null!;
            public string[] email { get; set; } = null!;
        }

        public class Оснвиддеят
        {
            public string Код { get; set; } = null!;
            public string Текст { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Открсведения
        {
            public string КолРаб { get; set; } = null!;
            public string СведСНР { get; set; } = null!;
            public string ПризнУчКГН { get; set; } = null!;
            public Налоги[] Налоги { get; set; } = null!;
            public string СумДоход { get; set; } = null!;
            public string СумРасход { get; set; } = null!;
            public Отраслевыепок ОтраслевыеПок { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Отраслевыепок
        {
            public string НалогНагрузка { get; set; } = null!;
            public string Рентабельность { get; set; } = null!;
        }

        public class Налоги
        {
            public string НаимНалог { get; set; } = null!;
            public string СумУплНал { get; set; } = null!;
        }

        public class История
        {
            public Руководитель1 Руководитель { get; set; } = null!;
            public Номтел НомТел { get; set; } = null!;
            public EMail Email { get; set; } = null!;
            public Адрес1 Адрес { get; set; } = null!;
            public Открсведения1[] ОткрСведения { get; set; } = null!;
        }

        public class Руководитель1
        {
            public _2015032620181002 _2015032620181002 { get; set; } = null!;
        }

        public class _2015032620181002
        {
            public string ФИОПолн { get; set; } = null!;
            public string ИННФЛ { get; set; } = null!;
            public string Пол { get; set; } = null!;
            public string ВидГражд { get; set; } = null!;
            public string Должн { get; set; } = null!;
        }

        public class Номтел
        {
            public string _2015032620171211 { get; set; } = null!;
        }

        public class EMail
        {
            public string _2016092320171211 { get; set; } = null!;
        }

        public class Адрес1
        {
            public _2003090820160922 _2003090820160922 { get; set; } = null!;
            public _2016092320171211 _2016092320171211 { get; set; } = null!;
            public _2017121220200826 _2017121220200826 { get; set; } = null!;
            public _2020072720200826 _2020072720200826 { get; set; } = null!;
        }

        public class _2003090820160922
        {
            public string АдресПолн { get; set; } = null!;
        }

        public class _2016092320171211
        {
            public string АдресПолн { get; set; } = null!;
        }

        public class _2017121220200826
        {
            public string АдресПолн { get; set; } = null!;
        }

        public class _2020072720200826
        {
            public string ПризнНедАдресЮЛ { get; set; } = null!;
        }

        public class Открсведения1
        {
            public string КолРаб { get; set; } = null!;
            public string СведСНР { get; set; } = null!;
            public string ПризнУчКГН { get; set; } = null!;
            public Налоги1[] Налоги { get; set; } = null!;
            public string СумДоход { get; set; } = null!;
            public string СумРасход { get; set; } = null!;   
            public Отраслевыепок1 ОтраслевыеПок { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Отраслевыепок1
        {
            public string НалогНагрузка { get; set; } = null!;
            public string Рентабельность { get; set; } = null!;
        }

        public class Налоги1
        {
            public string НаимНалог { get; set; } = null!;
            public string СумУплНал { get; set; } = null!;
            public string СумНедНалог { get; set; } = null!;
            public string СумПени { get; set; } = null!;
            public string СумШтраф { get; set; } = null!;
            public string ОбщСумНедоим { get; set; } = null!;
        }

        public class Учредители
        {
            public Учрфл УчрФЛ { get; set; } = null!;
            public string СуммаУК { get; set; } = null!;
            public string Процент { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class Учрфл
        {
            public string ФИОПолн { get; set; } = null!;
            public string ИННФЛ { get; set; } = null!;
            public string Пол { get; set; } = null!;
            public string ВидГражд { get; set; } = null!;
        }

        public class Допвиддеят
        {
            public string Код { get; set; } = null!;
            public string Текст { get; set; } = null!;
            public string Дата { get; set; } = null!;
        }

        public class СПВЗ
        {
            public string Дата { get; set; } = null!;
            public string Текст { get; set; } = null!;
        }

        public class Лицензии
        {
            public string НомерЛиц { get; set; } = null!;
            public string ВидДеятельности { get; set; } = null!;
            public string ДатаЛиц { get; set; } = null!;
            public string ДатаНачала { get; set; } = null!;
            public string ЛицОрг { get; set; } = null!;
        }

        public class Участия
        {
            public string ОГРН { get; set; } = null!;
            public string ИНН { get; set; } = null!;
            public string НаимСокрЮЛ { get; set; } = null!;
            public string НаимПолнЮЛ { get; set; } = null!;
            public string Статус { get; set; } = null!;
            public string СуммаУК { get; set; } = null!;
        }

    }
}
