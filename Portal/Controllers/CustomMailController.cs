using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class CustomMailController : BaseController
    {
        #region Mail

        [ValidateRequest(true, UserInterfaceHelper.CustomMail_SendCustomMail, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SendCustomMail(int mailId = 0, int accessMode = 3)
        {
            try
            {
                if (mailId != 0)
                {

                    ViewData["mailId"] = mailId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["fromMailId"] = ContextUser.Email;
                }
                else
                {
                    ViewData["activitycalenderId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["fromMailId"] = ContextUser.Email;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.CustomMail_SendCustomMail, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public JsonResult SendCustomMail(CustomMailViewModel customMailViewModel, List<MailSupportingDocumentViewModel> mailDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
          
            CustomMailBL customMailBL = new CustomMailBL();
            try

            {
                if (mailDocuments != null)
                {
                    foreach (MailSupportingDocumentViewModel md in mailDocuments)
                    {
                        var path = Path.Combine(Server.MapPath("~/Images/MailAttachmentDocument"), md.DocumentName);
                        byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                        md.file = fileBytes;
                    }
                    customMailViewModel.WithAttachment = true;
                }
                
                customMailViewModel.CreatedBy = ContextUser.UserId;
                customMailViewModel.CompanyId = ContextUser.CompanyId;

                responseOut = customMailBL.SendCustom(customMailViewModel, mailDocuments);
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);

        }
        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/MailAttachmentDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public ActionResult SaveSupportingDocument()
        {
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            Random rnd = new Random();
            try
            {
                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        string newFileName = "";
                        var fileName = Path.GetFileName(file.FileName);
                        newFileName = Convert.ToString(rnd.Next(10000, 99999)) + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/MailAttachmentDocument"), newFileName);
                        file.SaveAs(path);
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = newFileName;
                    }
                    else
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = "";
                    }
                }
                else
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = "";
                }


            }
            catch (Exception ex)
            {
                responseOut.message = "";
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetMailSupportingDocumentList(List<MailSupportingDocumentViewModel> mailDocuments)
        {
            try
            {
                CustomMailBL customMailBL = new CustomMailBL();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(mailDocuments);
        }

        
    }

    #endregion
}
