using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;

namespace Portal.Common
{
    public static class CommonHelper
    {
        public static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }
        public static IQueryable<T> LimitAndOffset<T>(this IQueryable<T> q,
                            int pageSize, int pageOffset)
        {
            return q.Skip((pageOffset - 1) * pageSize).Take(pageSize);
        }

        public static void ResizeAndSave(string savePath, string fileName, Stream imageBuffer, int maxSideSize, bool makeItSquare)
        {
            int newWidth;
            int newHeight;
            Image image = Image.FromStream(imageBuffer);
            int oldWidth = image.Width;
            int oldHeight = image.Height;
            Bitmap newImage;
            try
            {
                if (makeItSquare)
                {
                    int smallerSide = oldWidth >= oldHeight ? oldHeight : oldWidth;
                    double coeficient = maxSideSize / (double)smallerSide;
                    newWidth = Convert.ToInt32(coeficient * oldWidth);
                    newHeight = Convert.ToInt32(coeficient * oldHeight);
                    Bitmap tempImage = new Bitmap(image, newWidth, newHeight);
                    int cropX = (newWidth - maxSideSize) / 2;
                    int cropY = (newHeight - maxSideSize) / 2;
                    newImage = new Bitmap(maxSideSize, maxSideSize);
                    Graphics tempGraphic = Graphics.FromImage(newImage);
                    tempGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                    tempGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    tempGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    tempGraphic.DrawImage(tempImage, new Rectangle(0, 0, maxSideSize, maxSideSize), cropX, cropY, maxSideSize, maxSideSize, GraphicsUnit.Pixel);
                }
                else
                {
                    int maxSide = oldWidth >= oldHeight ? oldWidth : oldHeight;

                    if (maxSide > maxSideSize)
                    {
                        double coeficient = maxSideSize / (double)maxSide;
                        newWidth = Convert.ToInt32(coeficient * oldWidth);
                        newHeight = Convert.ToInt32(coeficient * oldHeight);
                    }
                    else
                    {
                        newWidth = oldWidth;
                        newHeight = oldHeight;
                    }
                    newImage = new Bitmap(image, newWidth, newHeight);
                }
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                newImage.Save(savePath + fileName + ".jpg", ImageFormat.Jpeg);
                image.Dispose();
                newImage.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        public static string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };
                var tensMap = new[]
                {
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }


        public static String changeToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = ("Only.");
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        //int p = points.Length;
                        //char[] TPot = points.ToCharArray();
                        andStr = (" and ");// just to separate whole numbers from points/Rupees                  
                        //for (int i = 0; i < p; i++)
                        //{
                        //    andStr += ones(Convert.ToString(TPot[i])) + " ";
                        //}
                        andStr += translateWholeNumber(points).Trim() + " Paise";

                    }
                }
                val = String.Format("{0} {1}{2} {3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch
            {
                ;
            }
            return val;
        }

        private static String translateWholeNumber(String number)
        {

           // number = "3000";
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))

                if (dblAmt > 0)
                {
                    //test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc...


                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range


                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 6:

                           

                        case 7://millions' range
                            pos = (numDigits % 6) + 1;
                            // place = " Million ";
                            place = " Lakh ";
                            break;
                        case 8:
                        case 9:

                        case 10://Billions's range
                            pos = (numDigits % 8) + 1;
                            place = " Core ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {
                        //if transalation is not done, continue...(Recursion comes in now!!)
                        if (beginsZero && place == " and ") place = "" ;

                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));

                       

                        //check for trailing zeros
                        if (beginsZero) word = " and " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch
            {
                ;
            }
            return word.Trim();
        }

        private static String tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static String ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {
                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        //end function

        public static string FilterText(string text)
        {
            string filterString = string.Empty;
            filterString = text.Replace("<script>", "");
            filterString = text.Replace("</script>", "");
            return filterString;
        }
        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
        public static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }
        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {

                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }




        public static string ConvertMyword(int number)

        {

            int flag = 0;

            int lflag = 0;

            string words = String.Empty;

            string[] places = { "ones", "ten", "hundred", "thousand", "ten thousand", "lac", "tenlacs", "crore", "tencrore", "Billon" };

            string rawnumber = number.ToString();

            char[] a = rawnumber.ToCharArray();

            Array.Reverse(a);

            if (a.Length >= 2)

            {

                for (int i = a.Length - 1; i >= 0; i--)

                {

                    if (i % 2 == 0 && i > 2)

                    {

                        if (int.Parse(a[i].ToString()) > 1)

                        {

                            if (int.Parse(a[i - 1].ToString()) == 0)

                            {

                                words = words + getNumberStringty(int.Parse(a[i].ToString())) + " " + places[i - 1] + " ";

                            }

                            else

                            {

                                words = words + getNumberStringty(int.Parse(a[i].ToString())) + " ";

                            }

                        }

                        else if (int.Parse(a[i].ToString()) == 1)

                        {

                            if (int.Parse(a[i - 1].ToString()) == 0)

                            {

                                words = words + "Ten" + " ";

                            }

                            else

                            {

                                words = words + getNumberStringteen(int.Parse(a[i - 1].ToString())) + " ";

                            }

                            flag = 1;

                        }

                    }

                    else

                    {

                        if (i == 1 || i == 0)

                        {

                            if (int.Parse(a[i].ToString()) > 1)

                            {

                                words = words + getNumberStringty(int.Parse(a[i].ToString())) + " " + getNumberString(int.Parse(a[0].ToString())) + " ";

                                break;

                            }

                            else if (int.Parse(a[i].ToString()) == 1)

                            {

                                if (int.Parse(a[i - 1].ToString()) == 0)

                                {

                                    words = words + "Ten" + " ";

                                }

                                else

                                {

                                    words = words + getNumberStringteen(int.Parse(a[i - 1].ToString())) + " ";

                                }



                                break;

                            }

                            else if (int.Parse(a[i - 1].ToString()) != 0)

                            {

                                words = words + getNumberString(int.Parse(a[i - 1].ToString())) + " ";

                                break;

                            }

                            else

                            {

                                break;

                            }

                        }

                        else

                        {

                            if (flag == 0)

                            {

                                for (int l = i; l >= 0; l--)

                                {

                                    if (int.Parse(a[l].ToString()) != 0)

                                    {

                                        lflag = 1;

                                    }

                                }

                                if (lflag == 1 && int.Parse(a[i].ToString()) != 0)

                                {



                                    words = words + getNumberString(int.Parse(a[i].ToString())) + " " + places[i] + " ";

                                    lflag = 0;





                                }

                                else if (lflag == 0)

                                {



                                    lflag = 0;

                                    break;

                                }



                            }

                            else

                            {

                                words = words + " " + places[i] + " ";

                                flag = 0;

                            }



                        }

                    }

                }

            }

            else

            {

                words = getNumberString(int.Parse(a[0].ToString()));

            }


            return words +"Only.";
            //Console.WriteLine(words);

        }

        public static string getNumberString(int num)

        {

            string Word = String.Empty;

            switch (num)

            {

                case 1:

                    Word = "one";

                    break;

                case 2:

                    Word = "two";

                    break;



                case 3:

                    Word = "three";

                    break;



                case 4:

                    Word = "four";

                    break;



                case 5:

                    Word = "five";

                    break;



                case 6:

                    Word = "six";

                    break;

                case 7:

                    Word = "seven";

                    break;



                case 8:

                    Word = "eight";

                    break;



                case 9:

                    Word = "nine";

                    break;





            }

            return Word;

        }

        public static string getNumberStringty(int num)

        {

            string Word = String.Empty;

            switch (num)

            {



                case 2:

                    Word = "twenty";

                    break;



                case 3:

                    Word = "thirty";

                    break;



                case 4:

                    Word = "fourty";

                    break;



                case 5:

                    Word = "fifty";

                    break;



                case 6:

                    Word = "sixty";

                    break;

                case 7:

                    Word = "seventy";

                    break;



                case 8:

                    Word = "eighty";

                    break;



                case 9:

                    Word = "ninty";

                    break;





            }

            return Word;

        }

        public static string getNumberStringteen(int num)

        {

            string Word = String.Empty;

            switch (num)

            {

                case 1:

                    Word = "eleven";

                    break;

                case 2:

                    Word = "tewlve";

                    break;



                case 3:

                    Word = "thirteen";

                    break;



                case 4:

                    Word = "fourteen";

                    break;



                case 5:

                    Word = "fifteen";

                    break;



                case 6:

                    Word = "sixteen";

                    break;

                case 7:

                    Word = "seventeen";

                    break;



                case 8:

                    Word = "eighteen";

                    break;



                case 9:

                    Word = "ninteen";

                    break;





            }

            return Word;

        }

    }


}




