﻿    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Cinema.Infrastructure;
    using Cinema.Infrastructure.Models;

    namespace Cinema.MVC.Controllers
    {
        public class MoviesController : Controller
        {
            private readonly CinemaContext _context;

            public MoviesController(CinemaContext context)
            {
                _context = context;
            }

            // GET: Movies1
            public async Task<IActionResult> Index()
            {
                return View(await _context.Movies.ToListAsync());
            }

            // GET: Movies1/Details/5
            public async Task<IActionResult> Details(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var movie = await _context.Movies
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (movie == null)
                {
                    return NotFound();
                }

                return View(movie);
            }

            // GET: Movies1/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Movies1/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Id,Title,Genre,Year,Rating")] Movie movie)
            {
                if (ModelState.IsValid)
                {
                    movie.Id = Guid.NewGuid();
                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(movie);
            }

            // GET: Movies1/Edit/5
            public async Task<IActionResult> Edit(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    return NotFound();
                }
                return View(movie);
            }

            // POST: Movies1/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Genre,Year,Rating")] Movie movie)
            {
                if (id != movie.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(movie);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MovieExists(movie.Id))
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
                return View(movie);
            }

            // GET: Movies1/Delete/5
            public async Task<IActionResult> Delete(Guid? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var movie = await _context.Movies
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (movie == null)
                {
                    return NotFound();
                }

                return View(movie);
            }

            // POST: Movies1/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(Guid id)
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie != null)
                {
                    _context.Movies.Remove(movie);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool MovieExists(Guid id)
            {
                return _context.Movies.Any(e => e.Id == id);
            }
        }
    }
