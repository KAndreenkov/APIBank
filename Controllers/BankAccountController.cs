using APIBank.Models;
using APIBank.Models.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Controllers
{
    //Контроллер для администрирования и навигации по аккаунтам клиентов 
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        readonly TestAppContext db;
        public BankAccountController(TestAppContext context)
        {
            db = context;

            //Заполним базу первичными данными (Если пустая, при первой инициализации)
            if (!db.PersonNames.Any())
            {
                db.PersonNames.Add(new PersonName { PersonFam = "Ivanov" });
                db.PersonNames.Add(new PersonName { PersonFam = "Petrov" });
                db.PersonNames.Add(new PersonName { PersonFam = "Sidorov" });
                db.PersonNames.Add(new PersonName { PersonFam = "Andrerev" });

                db.CurrencyItems.Add(new CurrencyItem { CurrencyLong = "Доллар США", CurrencyShort = "USD" });
                db.CurrencyItems.Add(new CurrencyItem { CurrencyLong = "Рубль РФ", CurrencyShort = "RUB" });
                db.CurrencyItems.Add(new CurrencyItem { CurrencyLong = "Евро", CurrencyShort = "EUR" });
                db.CurrencyItems.Add(new CurrencyItem { CurrencyLong = "Казахстанских тенге", CurrencyShort = "KZT" });

                db.SaveChanges();
            }
        }

        //Показать список всех клиентов банка
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonName>>> Get()
        {
            return await db.PersonNames.ToListAsync();
        }

        //Показать список всех клиентов банка
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<CurrencyItem>>> GetCurrencies()
        {
            return await db.CurrencyItems.ToListAsync();
        }

        //Показать все счета конкретного пользователя
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            PersonName user = await db.PersonNames.FirstOrDefaultAsync(i => i.PersonNameId == id);
            if (user == null)
                return NotFound();
            else
            {
                List<CurrencyItem> currencies = await db.CurrencyItems.ToListAsync();
                IEnumerable<BankRecord> bankRecords = await db.BankRecords.Where(i => i.PersonId.PersonNameId == user.PersonNameId).ToListAsync();

                return new ObjectResult(bankRecords);
            }
        }

        //Добавление нового клиента банка
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> NewPerson(PersonName user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            user.PersonNameId = 0;
            db.PersonNames.Add(user);
            await db.SaveChangesAsync();
            await db.DisposeAsync();
            return Ok(user);
        }

        //Изменение имени клиента банка
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> EditPerson(PersonName user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.PersonNames.Any(i => i.PersonNameId == user.PersonNameId))
            {
                return NotFound();
            }

            db.Update(user);
            await db.SaveChangesAsync();
            await db.DisposeAsync();
            return Ok(user);
        }

        //Удаление клиента банка
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            PersonName user = db.PersonNames.FirstOrDefault(i => i.PersonNameId == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<BankRecord> records = db.BankRecords.Where(i => i.PersonId.PersonNameId == user.PersonNameId);
                db.PersonNames.Remove(user);
                db.BankRecords.RemoveRange(records);
                await db.SaveChangesAsync();
                await db.DisposeAsync();
                return Ok(user);
            }
        }
    }
}