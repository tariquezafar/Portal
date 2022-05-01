using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;

namespace Portal.Core
{
    public class BookBL
    {
        DBInterface dbInterface;
        public BookBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditBook(BookViewModel bookViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Book book = new Book
                {
                    BookId = bookViewModel.BookId,
                    BookName = bookViewModel.BookName,
                    BookCode = bookViewModel.BookCode,
                    BookType = bookViewModel.BookType,   
                    GLCode = bookViewModel.GLCode,
                    CompanyId = bookViewModel.CompanyId, 
                    BankBranch = bookViewModel.BankBranch,
                    BankAccountNo = bookViewModel.BankAccountNo,
                    IFSC = bookViewModel.IFSC,
                    OverDraftLimit = bookViewModel.OverDraftLimit, 
                    CreatedBy = bookViewModel.CreatedBy,
                    CompanyBranchId=bookViewModel.CompanyBranchId,
                    Status = bookViewModel.Book_Status,
                    AdCode= bookViewModel.AdCode
                };
                responseOut = dbInterface.AddEditBook(book);
            }
            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;
        } 

        public List<BookViewModel> GetBookList(string bookName = "", string bookType = "0", string bookCode = "", int companyId = 0, string status = "",int companyBranchId=0)
        {
            List<BookViewModel> books = new List<BookViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBooks = sqlDbInterface.GetBookList(bookName, bookType, bookCode, companyId, status, companyBranchId);
                if (dtBooks != null && dtBooks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBooks.Rows)
                    {
                        books.Add(new BookViewModel
                        {
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookName = Convert.ToString(dr["bookName"]),
                            BookType = Convert.ToString(dr["bookType"]),
                            BookCode = Convert.ToString(dr["bookCode"]), 
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Book_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return books;
        }

        public BookViewModel GetBookDetail(int bookId = 0)
        {
            BookViewModel book = new BookViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtBooks = sqlDbInterface.GetBookDetail(bookId);
                if (dtBooks != null && dtBooks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBooks.Rows)
                    {
                        book = new BookViewModel
                        {
                            BookId = Convert.ToInt32(dr["BookId"]),
                            BookName = Convert.ToString(dr["BookName"]),
                            BookCode = Convert.ToString(dr["BookCode"]),
                            BookType = Convert.ToString(dr["BookType"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            BankBranch = Convert.ToString(dr["BankBranch"]),
                            BankAccountNo = Convert.ToString(dr["BankAccountNo"]), 
                            IFSC = Convert.ToString(dr["IFSC"]),
                            OverDraftLimit = Convert.ToDecimal(dr["OverDraftLimit"].ToString() == "" ? "0" : dr["OverDraftLimit"].ToString()),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Book_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return book;
        }
        public List<BookViewModel> GetBookAutoCompleteList(string searchTerm, string bookType, int companyId,int companyBranchId)
        {
            List<BookViewModel> books = new List<BookViewModel>();
            try
            {
                List<Book> bookList = dbInterface.GetBookAutoCompleteList(searchTerm, bookType, companyId, companyBranchId);

                if (bookList != null && bookList.Count > 0)
                {
                    foreach (Book book in bookList)
                    {
                        books.Add(new BookViewModel { BookId = book.BookId, BookCode = book.BookCode, BookName = book.BookName, BankBranch = book.BankBranch,IFSC=book.IFSC });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return books;
        }



        public List<BookViewModel> GetBookList(int companyId)
        {
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                List<Portal.DAL.Book> books = dbInterface.GetBookList(companyId);
                if (books != null && books.Count > 0)
                {

                    foreach (Portal.DAL.Book book in books)
                    {
                       
                            bookList.Add(new BookViewModel { BookId = book.BookId, BookName = book.BookName, BankAccountNo = book.BankAccountNo, BankBranch =book.BankBranch });
                         
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }

        public List<BookViewModel> GetBookList(string bookType, int companyId,int companyBranchId)
        {
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                List<Portal.DAL.Book> books = dbInterface.GetBookList(bookType,companyId, companyBranchId);
                if (books != null && books.Count > 0)
                {

                    foreach (Portal.DAL.Book book in books)
                    {

                        bookList.Add(new BookViewModel { BookId = book.BookId, BookName = book.BookName, BankAccountNo = book.BankAccountNo, BankBranch = book.BankBranch });

                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }

        public List<BookViewModel> GetJLBookList(string bookType, int companyId)
        {
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                List<Portal.DAL.Book> books = dbInterface.GetJVBookList(bookType, companyId);
                if (books != null && books.Count > 0)
                {

                    foreach (Portal.DAL.Book book in books)
                    {

                        bookList.Add(new BookViewModel { BookId = book.BookId, BookName = book.BookName, BankAccountNo = book.BankAccountNo, BankBranch = book.BankBranch });

                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }


        public List<BookViewModel> GetDebitNoteBookList(string bookType, int companyId)
        {
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                List<Portal.DAL.Book> books = dbInterface.GetDebitNoteBookList(bookType, companyId);
                if (books != null && books.Count > 0)
                {

                    foreach (Portal.DAL.Book book in books)
                    {

                        bookList.Add(new BookViewModel { BookId = book.BookId, BookName = book.BookName, BankAccountNo = book.BankAccountNo, BankBranch = book.BankBranch });

                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        }

        public List<BookViewModel> GetCreditNoteBookList(string bookType, int companyId)
        {
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                List<Portal.DAL.Book> books = dbInterface.GetCreditNoteBookList(bookType, companyId);
                if (books != null && books.Count > 0)
                {

                    foreach (Portal.DAL.Book book in books)
                    {

                        bookList.Add(new BookViewModel { BookId = book.BookId, BookName = book.BookName, BankAccountNo = book.BankAccountNo, BankBranch = book.BankBranch });

                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookList;
        } 

        public List<GLViewModel> GetBookGLAutoCompleteList(string searchTerm, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetBookGLAutoCompleteList(searchTerm, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        gls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode, SLTypeId = Convert.ToInt16(gl.SLTypeId) });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }


        public decimal GetBookBalance(int companyId, int finYearId, long voucherId, int bookId)
        {
            decimal bookBalance = 0;
            SQLDbInterface sqlDBInterface = new SQLDbInterface();
            try
            {
                bookBalance= sqlDBInterface.GetBookBalance(companyId, finYearId, voucherId, bookId);
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalance;
        }

    }
}








