using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.FileHandler
{
   public class FileHandlerServices : IFileHandler
    {
        public async Task<string> UploadFile(IFormFile FileDetail, string UploadPath, string FileAllowExtension,
                           int _oneMegaByte, int _fileMaxSize)
        {
            try
            {
                string msg = "";
                if (FileDetail != null)
                {
                    var extension = FileDetail.FileName.Split('.')[1];
                    var supportedTypes = FileAllowExtension.Split(',');
                    int checkExtension = 0;


                    foreach (var item in supportedTypes)
                    {

                        var kk = item.Replace("\"", "");
                        if (kk.ToLower() == extension.ToLower())

                        {
                            checkExtension = checkExtension + 1;
                        }
                    }

                    var filesize = _fileMaxSize * _oneMegaByte;

                    if (checkExtension == 0)

                    {
                        msg = CommonResponseMessage.INVALID_FILE_EXTENSION + FileAllowExtension;
                        return msg;
                    }

                    else if (FileDetail.Length == 0)
                    {
                        msg = CommonResponseMessage.NO_FILE_CONTENT;
                        return msg;
                    }



                    else if (FileDetail.Length > filesize)
                    {
                        msg = CommonResponseMessage.FILE_SIZE_EXCEEDED + _fileMaxSize + CommonResponseMessage.FILE_SIZE_UNIT;
                        return msg;
                    }


                    var fileName = Guid.NewGuid().ToString();
                    fileName += "." + extension;

                    string fullPath = Directory.GetCurrentDirectory() + UploadPath + fileName;



                    // var fullPath = Path.Combine(Directory.GetCurrentDirectory(), UploadPath, fileName);
                    using (var fileSrteam = new FileStream(fullPath, FileMode.Create))
                    {
                        await FileDetail.CopyToAsync(fileSrteam);
                    }


                    msg = fileName;

                    return "OK|"+msg;
                }

                return CommonResponseMessage.NO_FILE_UPLOADED;
            }
            catch (Exception ex)
            {
                var ec = ex;
                return CommonResponseMessage.FILE_UPLOAD_FAILED + ex.Message;
            }
        }


        public async Task<string> AcceptAllUploadFile(IFormFile FileDetail, string UploadPath, int _oneMegaByte, int _fileMaxSize, string extraParameter)
        {
            try
            {
                string msg = "";
                if (FileDetail != null)
                {
                    var extension = FileDetail.FileName.Split('.')[1];



                    var filesize = _fileMaxSize * _oneMegaByte;

                    if (FileDetail.Length == 0)
                    {
                        msg = CommonResponseMessage.NO_FILE_CONTENT;
                        return msg;
                    }



                    else if (FileDetail.Length > filesize)
                    {
                        msg = CommonResponseMessage.FILE_SIZE_EXCEEDED + _fileMaxSize + CommonResponseMessage.FILE_SIZE_UNIT;
                        return msg;
                    }


                    var fileName = Guid.NewGuid().ToString();
                    fileName += "." + extension;

                    string fullPath = Directory.GetCurrentDirectory() + UploadPath + fileName;



                    // var fullPath = Path.Combine(Directory.GetCurrentDirectory(), UploadPath, fileName);
                    using (var fileSrteam = new FileStream(fullPath, FileMode.Create))
                    {
                        await FileDetail.CopyToAsync(fileSrteam);
                    }




                    msg = fileName;

                    return msg;
                }

                return CommonResponseMessage.NO_FILE_UPLOADED;
            }
            catch (Exception ex)
            {
                var ec = ex;
                return CommonResponseMessage.FILE_UPLOAD_FAILED + ex.Message;
            }
        }

        public async Task<string> UploadFileValues(IFormFile FileDetail, string UploadPath, string FileAllowExtension,
                             int _oneMegaByte, int _fileMaxSize)
        {
            try
            {
                string msg = "";
                if (FileDetail != null)
                {
                    var extension = FileDetail.FileName.Split('.')[1];
                    var supportedTypes = FileAllowExtension.Split(',');
                    int checkExtension = 0;
                    foreach (var item in supportedTypes)
                    {

                        var kk = item.Replace("\"", "");
                        if (kk.ToLower().Trim() == extension.ToLower().Trim())

                        {
                            checkExtension = checkExtension + 1;
                        }
                    }

                    var filesize = _fileMaxSize * _oneMegaByte;

                    if (checkExtension == 0)

                    {
                        msg = CommonResponseMessage.INVALID_FILE_EXTENSION + FileAllowExtension;
                        return msg;
                    }

                    else if (FileDetail.Length == 0)
                    {
                        msg = CommonResponseMessage.NO_FILE_CONTENT;
                        return msg;
                    }



                    else if (FileDetail.Length > filesize)
                    {
                        msg = CommonResponseMessage.FILE_SIZE_EXCEEDED + _fileMaxSize + CommonResponseMessage.FILE_SIZE_UNIT;
                        return msg;
                    }


                    var fileName = Guid.NewGuid().ToString();
                    fileName += "." + extension;

                    string fullPath = Directory.GetCurrentDirectory() + UploadPath + fileName;



                    // var fullPath = Path.Combine(Directory.GetCurrentDirectory(), UploadPath, fileName);
                    using (var fileSrteam = new FileStream(fullPath, FileMode.Create))
                    {
                        await FileDetail.CopyToAsync(fileSrteam);
                    }



                    msg = fileName;

                    return msg;
                }

                return CommonResponseMessage.NO_FILE_UPLOADED;
            }
            catch (Exception ex)
            {
                var ec = ex;
                return CommonResponseMessage.FILE_UPLOAD_FAILED + ex.Message;
            }
        }
    }
}