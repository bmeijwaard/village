using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Village.Data;
using Village.Data.Domain;
using Village.Models.Example;

namespace Village.Controllers
{
    public class ExampleController : Controller
    {
        private readonly SqlContext _context;

        public ExampleController(SqlContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var example = await _context.Examples.FirstOrDefaultAsync() ?? new Example();
            return View(Mapper.Map<ExampleViewModel>(example));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new ExampleViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExampleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var example = Mapper.Map<Example>(model);
            using (var transaction = await _context.BeginTransactionAsync())
            {
                try
                {
                    await _context.AddAsync(example);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("SqlContextError", e.Message);
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
