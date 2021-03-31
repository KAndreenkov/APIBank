using APIBank.Models;
using APIBank.Models.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankOperationController : ControllerBase
    {
        readonly TestAppContext db;
        public BankOperationController(TestAppContext context)
        {
            db = context;
        }

        //Получить кошелек пользователя по Id
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

        //Пополнить счет
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpMoney(UpdateCash updateCash)
        {
            if (updateCash == null)
            {
                return NotFound();
            }
            if (!db.BankRecords.Any(i => i.BankRecordId == updateCash.UpdateRecordId))
            {
                return NotFound();
            }
            else
            {
                BankRecord bankRecord = await db.BankRecords.FirstOrDefaultAsync(i => i.BankRecordId == updateCash.UpdateRecordId);
                bankRecord.Cash += updateCash.DeltaCash;

                db.Update(bankRecord);
                await db.SaveChangesAsync();
                await db.DisposeAsync();
                return Ok(updateCash);
            }
        }

        //Снять деньги со счета
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> DownMoney(UpdateCash updateCash)
        {
            if (updateCash == null)
            {
                return BadRequest();
            }
            if (!db.BankRecords.Any(i => i.BankRecordId == updateCash.UpdateRecordId))
            {
                return NotFound();
            }
            else
            {

                BankRecord bankRecord = await db.BankRecords.FirstOrDefaultAsync(i => i.BankRecordId == updateCash.UpdateRecordId);
                if (bankRecord.Cash < 0) 
                {
                    return BadRequest("Невозможно снять средства. Отрицательный баланс");
                }
                if (bankRecord.Cash < updateCash.DeltaCash)
                {
                    return BadRequest("Невозможно снять средства. Недостаточно средст на счете");
                }
                else
                {
                    bankRecord.Cash -= updateCash.DeltaCash;
                }

                db.Update(bankRecord);
                await db.SaveChangesAsync();
                await db.DisposeAsync();
                return Ok(updateCash);
            }
        }

        [HttpPut]
        [Route("[action]")]
        // Перевести валюту в другую валюту
        public async Task<ActionResult> ChangeСurrency(ChangeCurrecy change)
        {
            if (change == null)
            {
                return BadRequest("Объект не найден");
            }
            if (!db.BankRecords.Any(i => i.BankRecordId == change.UpdateRecordId))
            {
                return BadRequest("Параметры изменяемой строки заданы неверно");
            }
            if (db.BankRecords.Any(i => i.BankRecordId == change.UpdateRecordId
                                    && i.CurrencyId.CurrencyItemId == change.SelectCurrencyId))
            {
                return BadRequest("Счет в такой валюте уже существует");
            }
            else
            {
                //Достаем срочку счета, которую будем изменять
                BankRecord bankRecord = await db.BankRecords.Include(p => p.CurrencyId).FirstOrDefaultAsync(i => i.BankRecordId == change.UpdateRecordId);
                //Достаем имя валюты, в которую будем менять
                CurrencyItem currencyOut = db.CurrencyItems.FirstOrDefault(c => c.CurrencyItemId == change.SelectCurrencyId);
                double moneyIn = 0;
                double moneyOut = 1;
                //Пересчитаем валюту
                bankRecord.Cash = bankRecord.NewCourse(moneyIn, moneyOut, bankRecord, currencyOut, change.GetActualRates());
                bankRecord.CurrencyId = currencyOut;

                db.Update(bankRecord);
                await db.SaveChangesAsync();
                await db.DisposeAsync();
                return Ok(change);
            }
        }

        //Добавить новый счет пользователю
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> NewScoreById(NewScoreById NewScore)
        {
            if (NewScore == null)
            {
                return BadRequest();
            }
            if (!db.BankRecords.Any(i => i.PersonId.PersonNameId == NewScore.PersonId
                                    && i.CurrencyId.CurrencyItemId == NewScore.CurrencyItem))
            {

                BankRecord bankRecord = new BankRecord() { BankRecordId = 0, Cash = 0 };
                bankRecord.PersonId = db.PersonNames.FirstOrDefault(i => i.PersonNameId == NewScore.PersonId);
                bankRecord.CurrencyId = db.CurrencyItems.FirstOrDefault(i => i.CurrencyItemId == NewScore.CurrencyItem);

                db.BankRecords.Add(bankRecord);
                await db.SaveChangesAsync();
                await db.DisposeAsync();
                return Ok(bankRecord);
            }
            else
                return BadRequest("Счет в такой валюте уже существует");
        }

    }
}