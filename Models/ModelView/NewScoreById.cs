using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Models.ModelView
{
    public class NewScoreById
    {
        //Создание нового счета для пользователья X в валюте Y
        public int PersonId { get; set; }

        public int CurrencyItem { get; set; }

    }
}
