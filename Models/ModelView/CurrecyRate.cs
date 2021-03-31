using System.Xml;

namespace APIBank.Models
{

    /// <summary>
    /// Актуальный курс валюты. Промежуточный класс для рачетов
    /// </summary>
    public class CurrecyRate
    {
        public string CharCode { get; set; }
        public string Name { get; set; }
        public string Nominal { get; set; }
        public string Value { get; set; }
        public string NumCode { get; set; }
        
        //Парсинг XML узла в удобный класс CurrecyRate для работы
        public void XmlRateParse(XmlNode childnode) 
        {
            switch (childnode.Name)
            {
                case "CharCode":
                    this.CharCode = childnode.InnerText;
                    break;
                case "Name":
                    this.Name = childnode.InnerText;
                    break;
                case "Nominal":
                    this.Nominal = childnode.InnerText;
                    break;
                case "Value":
                    this.Value = childnode.InnerText;
                    break;
                case "NumCode":
                    this.NumCode = childnode.InnerText;
                    break;
            }
        }
    }
}
