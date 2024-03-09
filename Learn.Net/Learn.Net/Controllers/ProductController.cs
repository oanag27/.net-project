using Learn.Net.Helper;
using Learn.Net.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learn.Net.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    public readonly IWebHostEnvironment _hostingEnvironment;
    private readonly LearnContext _context;
    public ProductController(IWebHostEnvironment hostingEnvironment, LearnContext learnContext)
    {
        _hostingEnvironment = hostingEnvironment;
        _context = learnContext;  //for database operation
    }
    [HttpPut("UploadImage")]
    public async Task<IActionResult> UploadImage(IFormFile file, string productCode)
    {
        APIResponse aPIResponse = new APIResponse();
        //implement functionality to upload image   
        try
        {
            //access the server path
            string serverPath = GetFilePath(productCode);
            //check if we have already have this kind of file structure
            if (!Directory.Exists(serverPath)) //if not, create it
            {
                Directory.CreateDirectory(serverPath);
            }
            string imagePath = serverPath + "\\" + productCode + ".png"; //store in png format
            if (!System.IO.File.Exists(imagePath)) //if file does not exist
            {
                System.IO.File.Delete(imagePath); //delete the file
            }
            //convert to FileStream
            using (FileStream stream = System.IO.File.Create(imagePath))
            {
                await file.CopyToAsync(stream);  //use input file
                aPIResponse.ResponseCode = 200;
                aPIResponse.Result = "pass";
            }

        }
        catch (Exception ex)
        {
            aPIResponse.ErrorMessage = ex.Message;
        }
        return Ok(aPIResponse);
    }
    [HttpPut("MultiUploadImage")]
    public async Task<IActionResult> MultiUploadImage(IFormFileCollection fileCollection, string productCode)
    {
        APIResponse aPIResponse = new APIResponse();
        int passCount = 0, errorCount = 0;
        //implement functionality to upload image   
        try
        {
            //access the server path
            string serverPath = GetFilePath(productCode);
            //check if we have already have this kind of file structure
            if (!Directory.Exists(serverPath)) //if not, create it
            {
                Directory.CreateDirectory(serverPath);
            }

            foreach (var fileP in fileCollection)
            {
                string imagePath = serverPath + "\\" + fileP.FileName;
                if (!System.IO.File.Exists(imagePath)) //if file does not exist
                {
                    System.IO.File.Delete(imagePath); //delete the file
                }
                //convert to FileStream
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await fileP.CopyToAsync(stream);  //use input file
                    passCount++;

                }
            }

        }
        catch (Exception ex)
        {
            errorCount++;
            aPIResponse.ErrorMessage = ex.Message;
        }
        aPIResponse.ResponseCode = 200;
        aPIResponse.Result = "pass: " + passCount + " error: " + errorCount;
        return Ok(aPIResponse);
    }

    [HttpGet("GetImage")]
    //in order to access our files we need to enable it from middleware
    public async Task<IActionResult> GetImage(string productCode)
    {
        string imageUrl = string.Empty;
        string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        try
        {
            string filePath = GetFilePath(productCode);
            string imagePath = filePath + "\\" + productCode + ".png";
            if (System.IO.File.Exists(imagePath))
            {
                imageUrl = hostUrl + "/Upload/product/" + productCode + "/" + productCode + ".png";

            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            imageUrl = ex.Message;
        }
        return Ok(imageUrl);
    }

    [HttpGet("GetMultipleImages")]
    public async Task<IActionResult> GetMultipleImages(string productCode)
    {
        List<string> imageUrl = new List<string>();
        string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        try
        {
            string filePath = GetFilePath(productCode);

            if (Directory.Exists(filePath))
            {
                DirectoryInfo d = new DirectoryInfo(filePath);
                FileInfo[] files = d.GetFiles();
                foreach (FileInfo f in files)
                {
                    string fileName = f.Name;
                    string imagepath = filePath + "\\" + fileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        string _imageUrl = hostUrl + "/Upload/product/" + productCode + "/" + fileName;
                        imageUrl.Add(_imageUrl);
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
        return Ok(imageUrl);
    }

    [HttpGet("download")]
    public async Task<IActionResult> download(string productCode)
    {
        try
        {
            string filePath = GetFilePath(productCode);
            string imagePath = filePath + "\\" + productCode + ".png";
            if (System.IO.File.Exists(imagePath)) //if file exist convert to memory stream
            {
                MemoryStream memory = new MemoryStream();
                using (FileStream stream = new FileStream(imagePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "image/png", productCode + ".png");
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return NotFound();
        }

    }

    [HttpGet("remove")]
    public async Task<IActionResult> remove(string productCode)
    {
        try
        {
            string filePath = GetFilePath(productCode);
            string imagePath = filePath + "\\" + productCode + ".png";
            if (System.IO.File.Exists(imagePath)) //if file exist, remove it
            {
                System.IO.File.Delete(imagePath);
                return Ok("File removed");
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return NotFound();
        }

    }

    [HttpGet("multiRemove")]
    public async Task<IActionResult> multiRemove(string productCode)
    {
        try
        {
            string filePath = GetFilePath(productCode);
            if (Directory.Exists(filePath))
            {
                DirectoryInfo d = new DirectoryInfo(filePath);
                FileInfo[] files = d.GetFiles();
                foreach (FileInfo f in files)
                {
                    f.Delete();
                }
                return Ok("Files removed");
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return NotFound();
        }

    }

    [HttpPut("DBMultiUploadImage")]
    public async Task<IActionResult> DBMultiUploadImage(IFormFileCollection fileCollection, string productCode)
    {
        APIResponse aPIResponse = new APIResponse();
        int passCount = 0, errorCount = 0;
        //implement functionality to upload image   
        try
        {
            foreach (var fileP in fileCollection)
            {
                //convert file to memory string
                using (MemoryStream memory = new MemoryStream())
                {
                    await fileP.CopyToAsync(memory);
                    _context.TblProductimages.Add(new Repository.Models.TblProductimage
                    {
                        Productcode = productCode,
                        Productimage = memory.ToArray()  //convert to array
                    });
                    await _context.SaveChangesAsync();
                    passCount++;
                }
            }

        }
        catch (Exception ex)
        {
            errorCount++;
            aPIResponse.ErrorMessage = ex.Message;
        }
        aPIResponse.ResponseCode = 200;
        aPIResponse.Result = "pass: " + passCount + " error: " + errorCount;
        return Ok(aPIResponse);
    }

    [HttpGet("DBGetMultipleImages")]
    public async Task<IActionResult> DBGetMultipleImages(string productCode)
    {
        List<string> imageUrl = new List<string>();
        try
        {
            var _productImg = _context.TblProductimages
                .Where(_ => _.Productcode == productCode)
                .ToList();
            if (_productImg != null && _productImg.Count > 0)
            {
                _productImg.ForEach(_ =>
                {
                    imageUrl.Add(Convert.ToBase64String(_.Productimage));
                });
            }
            else
            {
                return NotFound();
            }

        }
        catch (Exception ex)
        {
        }
        return Ok(imageUrl);
    }

    [HttpGet("DBdownload")]
    public async Task<IActionResult> DBdownload(string productCode)
    {
        try
        {
            //get data from database
            var _productImg = await _context.TblProductimages.FirstOrDefaultAsync(_ => _.Productcode == productCode);

            if (_productImg != null)
            {
                //get image byte
                return File(_productImg.Productimage, "image/png", productCode + ".png");
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return NotFound();
        }

    }

    [NonAction]
    //for optimization
    private string GetFilePath(string productCode)
    {
        return _hostingEnvironment.WebRootPath + "\\Upload\\product\\" + productCode;
    }
}
}
