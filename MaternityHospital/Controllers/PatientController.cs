using MaternityHospital.Context;
using MaternityHospital.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.Linq.Expressions;

namespace MaternityHospital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase, IDisposable
    {
        private readonly HospitalContext _DbContext;

        private readonly ILogger<PatientController> _logger;
               
        public PatientController(ILogger<PatientController> logger, HospitalContext dbContext)
        {
            _logger = logger;
            _DbContext = dbContext;
        }
        /// <summary>
        /// Получить список пациентов
        /// </summary>
        /// <param name="date">Ограничение по дате рождения</param>
        /// <returns></returns>
        /// <response code="200">Список пациентов</response>
        /// <response code="404">Пациенты не найдены</response>
        /// <response code="400">Данные не корректны</response>
        [HttpGet]
        public IActionResult Get([FromQuery] string[]? date)
        {
            var modelType = Expression.Parameter(typeof(Patient));
            var defaultt = Expression.Constant(true);
            var filtrExp = Expression.Lambda<Func<Patient, bool>>(defaultt, modelType);
            BinaryExpression filter = default!;
            if (date?.Length > 0)
            {
                var member = Expression.PropertyOrField(modelType, nameof(Patient.BirthDate));
                filter = date.CreateExpression(member) ?? default!;
                if (filter != null)
                {
                    filtrExp = Expression.Lambda<Func<Patient, bool>>(filter, modelType);
                }         
                else
                {
                    return BadRequest();
                }
            }
            var response = _DbContext.Patient.Include(x => x.Name).Where(filtrExp.Compile()).ToList();
            if (response.Count > 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Поиск пациента
        /// </summary>
        /// <param name="patientId">Идентификатор</param>
        /// <returns></returns>
        /// <response code="200">Пациент найден</response>
        /// <response code="404">Пациент не найден</response>
        /// <response code="400">Данные не корректны</response>
        [HttpGet("{patientId}")]
        public IActionResult Get(Guid patientId)
        {
            try
            {
                var first = _DbContext.Patient.Include(x => x.Name).SingleOrDefault(x => x.Id == patientId);
                if (first == null)
                {
                    return NotFound();
                }
                return Ok(first);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка поиска пациента {message}", ex.Message);
                return BadRequest();
            }

        }
        /// <summary>
        /// Добавить пациента
        /// </summary>
        /// <param name="item">Пациент</param>
        /// <returns></returns>
        /// <response code="200">Пациент добавлен</response>
        /// <response code="409">Пациент с данным идентификаторм существует</response>
        /// <response code="400">Данные не корректны</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Patient item)
        {
            try
            {
                if (!_DbContext.Patient.Any(x => x.Id == item.Name.Id))
                {
                    await _DbContext.Patient.AddAsync(item);
                    await _DbContext.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return Conflict();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка добавления пациента {message}", ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Удалить пациента
        /// </summary>
        /// <param name="patientId">Идентификатор</param>
        /// <returns></returns>
        /// <response code="200">Пациент удален</response>
        /// <response code="404">Пациент не найден</response>
        /// <response code="400">Данные не корректны</response>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid patientId)
        {
            try
            {
                var first = _DbContext.Child.SingleOrDefault(x => x.Id == patientId);
                if (first != null)
                {
                    _DbContext.Child.Remove(first);
                    await _DbContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка удаления пациента {message}", ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Обновить пациента
        /// </summary>
        /// <param name="item">Новые данные</param>
        /// <returns></returns>
        /// <response code="200">Данные изменены</response>
        /// <response code="404">Пациент не найден</response>
        /// <response code="400">Данные не корректны</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Patient item)
        {
            try
            {
                var first = _DbContext.Patient.Include(x => x.Name).SingleOrDefault(x => x.Name.Id == item.Name.Id);

                if (first != null)
                {
                    first.Active = item.Active;
                    first.BirthDate = item.BirthDate;
                    first.Gender = item.Gender;
                    first.Name.Family = item.Name.Family;
                    first.Name.Use = item.Name.Use;
                    first.Name.Given = item.Name.Given;
                    await _DbContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка обновления пациента {message}", ex.Message);
                return BadRequest();
            }
        }

        public void Dispose()
        {
            _DbContext.Dispose();
        }

    }
}
