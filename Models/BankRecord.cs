using APIBank.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Models
{
    public class BankRecord
    {
        
        public int BankRecordId { get; set; }

        public PersonName PersonId { get; set; }

        public CurrencyItem CurrencyId { get; set; }

        public double Cash { get; set; }

       
        public double NewCourse(double inMoney, double outMoney, BankRecord bankRecord, CurrencyItem currencyOut, List<CurrecyRate> ActualRates) 
        {
            //узнаем, по чем у нас сейчас исходная валюта
            if (bankRecord.CurrencyId.CurrencyItemId != 2)
            {
                inMoney = Convert.ToDouble(ActualRates.FirstOrDefault(v => v.CharCode == bankRecord.CurrencyId.CurrencyShort).Value);
            }
            else
            {
                //если входная валюта - рубли, то множитель  1
                inMoney = 1;
            }
            //узнаем, по чем у нас сейчас выходная валюта
            if (currencyOut.CurrencyItemId != 2)
            {
                outMoney = Convert.ToDouble(ActualRates.FirstOrDefault(v => v.CharCode == currencyOut.CurrencyShort).Value);
            }
            else
            {
                //если выходная валюта - рубли, то делитель  1
                outMoney = 1;
            }
            return Math.Round((this.Cash * inMoney) / outMoney, 2);
        }

        public double EditCash(double delta) 
        {
            return (Cash += delta);
        }
    }
}
