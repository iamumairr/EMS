using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context!.Students!.ToListAsync());
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                string uploadFolder = @"\uploads\";

                string fileName = GetUniqueFileName(student.PictureUpload!.FileName);

                student.ProfilePicture = fileName;

                string pathforSave = Path.Combine(rootPath + uploadFolder + fileName);

                using (var fileStream = new FileStream(pathforSave, FileMode.Create))
                {
                    student.PictureUpload.CopyTo(fileStream);
                }

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context!.Students!.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objfromDb = _context!.Students!.AsNoTracking().FirstOrDefault(a => a.Id == id);

                    if (student.PictureUpload != null)
                    {
                        string rootPath = _webHostEnvironment.WebRootPath;
                        string uploadFolder = @"\uploads\";

                        string fileName = GetUniqueFileName(student.PictureUpload.FileName);

                        student.ProfilePicture = fileName;

                        string pathForSave = Path.Combine(rootPath + uploadFolder + fileName);

                        var oldImage = Path.Combine(rootPath + uploadFolder + objfromDb!.ProfilePicture!);

                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }

                        using FileStream? fileStream = new(pathForSave, FileMode.Create);

                        student.PictureUpload.CopyTo(fileStream);
                    }
                    else
                    {
                        student.ProfilePicture = objfromDb!.ProfilePicture;
                    }
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context!.Students!
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context!.Students!.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            string path = @"\uploads\";
            string uploadPath = _webHostEnvironment.WebRootPath + path;

            var oldPicture = Path.Combine(uploadPath + student.ProfilePicture!);

            if (System.IO.File.Exists(oldPicture))
            {
                System.IO.File.Delete(oldPicture);
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context!.Students!.Any(e => e.Id == id);
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return string.Concat(Path.GetFileNameWithoutExtension(fileName),
                "_",
                Guid.NewGuid().ToString().Substring(0, 4),
                Path.GetExtension(fileName));
        }
    }
}
