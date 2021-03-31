using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace APIBank.Models.ModelView
{
    public class ChangeCurrecy
    {
        public int UpdateRecordId { get; set; }

        public int SelectCurrencyId { get; set; }

        //получение актуальных данных по валютам по ссылке (всё указано в руб.)
        public List<CurrecyRate> GetActualRates() 
        {
            string URL = "http://www.cbr.ru/scripts/XML_daily.asp";

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(new XmlTextReader(URL));
            XmlElement xmlRoot = XmlDoc.DocumentElement;

            List<CurrecyRate> currecyRates = new List<CurrecyRate>();
            foreach (XmlNode xmlnode in xmlRoot)
            {
                CurrecyRate ActualRate = new CurrecyRate();
                foreach (XmlNode childnode in xmlnode.ChildNodes)
                {
                    ActualRate.XmlRateParse(childnode);
                }
                currecyRates.Add(ActualRate);
            }
            return currecyRates;
        }
    }
}
