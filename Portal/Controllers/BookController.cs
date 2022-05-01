using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class BookController : BaseController
    {
        //
        // GET: /User/
        #region Book
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Book_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditBook(int bookId = 0, int accessMode = 3)
        {
           try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (bookId != 0)
                {
                    ViewData["bookId"] = bookId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["bookId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Book_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditBook(BookViewModel bookViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            BookBL bookBL = new BookBL();
            try
            {
                if (bookViewModel != null)
                {
                    bookViewModel.CreatedBy = ContextUser.UserId;
                    bookViewModel.CompanyId = ContextUser.CompanyId;

                    responseOut = bookBL.AddEditBook(bookViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }

            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Book_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListBook()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetBookList(string bookName = "", string bookType = "", string bookCode = "", string status = "",int companyBranchId=0)
        {
            List<BookViewModel> books = new List<BookViewModel>();
            BookBL bookBL = new BookBL();
            try
            {
                books = bookBL.GetBookList(bookName, bookType, bookCode, ContextUser.CompanyId, status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(books);
        }


        public JsonResult GetBookDetail(int bookId)
        {
            BookBL bookBL = new BookBL();
            BookViewModel book = new BookViewModel();
            try
            {
                book = bookBL.GetBookDetail(bookId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(book, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBookGLAutoCompleteList(string term)
        {
            CustomerBL customerBL = new CustomerBL();
           BookBL bookBL = new BookBL();
            List<GLViewModel> glList = new List<GLViewModel>();
            try
            {
                glList = bookBL.GetBookGLAutoCompleteList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
