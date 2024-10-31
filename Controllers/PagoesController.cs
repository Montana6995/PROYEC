using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using Rotativa.AspNetCore;

namespace MVC.Controllers
{
    public class PagoesController : Controller
    {
        private readonly MercadoContext _context;

        public PagoesController(MercadoContext context)
        {
            _context = context;
        }

        // GET: Pagoes
        public async Task<IActionResult> Index()
        {
            var mercadoContext = _context.Pagos.Include(p => p.Personal).Include(p => p.Puesto).Include(p => p.TipoServicio);
            return View(await mercadoContext.ToListAsync());
        }

        // GET: Pagoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Personal)
                .Include(p => p.Puesto)
                .Include(p => p.TipoServicio)
                .FirstOrDefaultAsync(m => m.IdPago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagoes/Create
        public IActionResult Create()
        {
            ViewData["IdPersonal"] = new SelectList(_context.Personales, "IdPersonal", "IdPersonal");
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "IdPuesto");
            ViewData["IdTipoServicio"] = new SelectList(_context.TiposServicio, "IdTipoServicio", "IdTipoServicio");
            return View();
        }

        // POST: Pagoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPago,IdPuesto,IdPersonal,IdTipoServicio,Monto,Fecha")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(pago);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar los datos: " + ex.Message);
                }
            }

            ViewData["IdPersonal"] = new SelectList(_context.Personales, "IdPersonal", "IdPersonal", pago.IdPersonal);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "IdPuesto", pago.IdPuesto);
            ViewData["IdTipoServicio"] = new SelectList(_context.TiposServicio, "IdTipoServicio", "IdTipoServicio", pago.IdTipoServicio);

            return View(pago);
        }

        // GET: Pagoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["IdPersonal"] = new SelectList(_context.Personales, "IdPersonal", "IdPersonal", pago.IdPersonal);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "IdPuesto", pago.IdPuesto);
            ViewData["IdTipoServicio"] = new SelectList(_context.TiposServicio, "IdTipoServicio", "IdTipoServicio", pago.IdTipoServicio);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPago,IdPuesto,IdPersonal,IdTipoServicio,Monto,Fecha")] Pago pago)
        {
            if (id != pago.IdPago)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.IdPago))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPersonal"] = new SelectList(_context.Personales, "IdPersonal", "IdPersonal", pago.IdPersonal);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "IdPuesto", pago.IdPuesto);
            ViewData["IdTipoServicio"] = new SelectList(_context.TiposServicio, "IdTipoServicio", "IdTipoServicio", pago.IdTipoServicio);
            return View(pago);
        }

        // GET: Pagoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Personal)
                .Include(p => p.Puesto)
                .Include(p => p.TipoServicio)
                .FirstOrDefaultAsync(m => m.IdPago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pagos/GeneratePdf/5
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var pago = await _context.Pagos
                .Include(p => p.IdPersonal)
                .FirstOrDefaultAsync(p => p.IdPago == id);

            if (pago == null)
            {
                return NotFound();
            }

            return new ViewAsPdf("PagoPlanillaPdf", pago)
            {
                FileName = $"Planilla_Pago_{id}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.IdPago == id);
        }
    }
}
