using System.Text;
using DKCrm.Server.Interfaces.DocumentInterfaces;

namespace DKCrm.Server.Services.DocumentServices
{
    public class PriceToStringConverter : IPriceToStringConverter
    {
        private readonly ICurrencyDictionaryService _currencyDictionaryService;
        public PriceToStringConverter(ICurrencyDictionaryService currencyDictionaryService)
        {
            _currencyDictionaryService = currencyDictionaryService;
        }

        private readonly string[] _frac20 =
        {
            "", "один ", "два ", "три ", "четыре ", "пять ", "шесть ",
            "семь ", "восемь ", "девять ", "десять ", "одиннадцать ",
            "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ",
            "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "
        };
        private readonly string[] _tens =
        {
            "", "десять ", "двадцать ", "тридцать ", "сорок ", "пятьдесят ",
            "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто "
        };
        private readonly string[] _hundreds =
        {
            "", "сто ", "двести ", "триста ", "четыреста ",
            "пятьсот ", "шестьсот ", "семьсот ", "восемьсот ", "девятьсот "
        };
        
        public async Task<string> ConvertNumber(decimal value, string charCode)
        {
            var info = await _currencyDictionaryService.GetOneByCharCodeAsync(charCode);
            return Introduction(value, info.Male,
                info.One, info.Two, info.Five,
                info.OneLoverNominal, info.TwoLoverNominal, info.FiveLoverNominal);
        }
        private string Introduction(decimal val, bool male,
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            bool minus = false;
            if (val < 0) { val = -val; minus = true; }

            int n = (int)val;
            int remainder = (int)((val - n + 0.005M) * 100);

            StringBuilder r = new StringBuilder();

            if (0 == n) r.Append("0 ");
            if (n % 1000 != 0)
                r.Append(Str(n, male, seniorOne, seniorTwo, seniorFive));
            else
                r.Append(seniorFive);

            n /= 1000;

            r.Insert(0, Str(n, false, "тысяча", "тысячи", "тысяч"));
            n /= 1000;

            r.Insert(0, Str(n, true, "миллион", "миллиона", "миллионов"));
            n /= 1000;

            r.Insert(0, Str(n, true, "миллиард", "миллиарда", "миллиардов"));
            n /= 1000;

            r.Insert(0, Str(n, true, "триллион", "триллиона", "триллионов"));
            n /= 1000;

            r.Insert(0, Str(n, true, "триллиард", "триллиарда", "триллиардов"));
            if (minus) r.Insert(0, "минус ");

            r.Append(remainder.ToString("00 "));
            r.Append(Case(remainder, juniorOne, juniorTwo, juniorFive));

            r[0] = char.ToUpper(r[0]);

            return r.ToString();
        }

        private string Str(int val, bool male, string one, string two, string five)
        {
            int num = val % 1000;
            if (0 == num) return "";
            if (num < 0) throw new ArgumentOutOfRangeException("val", "Параметр не может быть отрицательным");
            if (!male)
            {
                _frac20[1] = "одна ";
                _frac20[2] = "две ";
            }

            StringBuilder r = new StringBuilder(_hundreds[num / 100]);

            if (num % 100 < 20)
            {
                r.Append(_frac20[num % 100]);
            }
            else
            {
                r.Append(_tens[num % 100 / 10]);
                r.Append(_frac20[num % 10]);
            }

            r.Append(Case(num, one, two, five));

            if (r.Length != 0) r.Append(" ");
            return r.ToString();
        }

        private static string Case(int val, string one, string two, string five)
        {
            int t = (val % 100 > 20) ? val % 10 : val % 20;

            switch (t)
            {
                case 1: return one;
                case 2: case 3: case 4: return two;
                default: return five;
            }
        }
    }
}
