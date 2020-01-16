using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;

namespace Empower.DTO
{
    public static class Helpers
    {
        public static bool IsValidEmail(string emailId)
        {
            return Regex.IsMatch(emailId, @"^([a-zA-Z0-9_\.,\'\-+%^&;:$#\""])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", RegexOptions.IgnoreCase);
        }

        public static bool ValidateStartAndEndDate(DateTime? StartDatetime, DateTime? EndDatetime)
        {
            bool flag = true;
            if (EndDatetime.ToEmpowerDate() < StartDatetime.ToEmpowerDate())
            {
                // throw new IsInvalidException("End Datetime should be greater start datetime");
                flag = false;
            }
            return flag;
        }

        private static DateTime ToEmpowerDate(this DateTime? dateTime)
        {
            dateTime = dateTime == null ? DateTime.Now : dateTime;
            DateTime Today = dateTime.Value.ToUniversalTime();
            return dateTime.Value.ToUniversalTime();
        }

        public static string Compress(this string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            using (var memoryStream = new MemoryStream())
            {
                using (var stream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                memoryStream.Position = 0;
                var compressed = new byte[memoryStream.Length];
                memoryStream.Read(compressed, 0, compressed.Length);
                var gZipBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gZipBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
                buffer = null;
                return Convert.ToBase64String(gZipBuffer);
            }
        }

        public static string Decompress(this string compressedText)
        {
            var gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
                var buffer = new byte[dataLength];
                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }
                gZipBuffer = null;
                return Encoding.UTF8.GetString(buffer);
            }
        }

        //public static bool CheckUserAccessPermissionForView(UserAccessDTO userAccessDTO)
        //{
        //    bool isViewAll = userAccessDTO.IsAdmin;
        //    if (userAccessDTO.Permission != null)
        //        isViewAll = userAccessDTO.Permission.IsViewAll;

        //    return isViewAll;
        //}

        //public static bool CheckUserAccessPermissionForEditable(UserAccessDTO userAccessDTO)
        //{
        //    bool isEditableAll = false;
        //    if (userAccessDTO.Permission != null)
        //        isEditableAll = userAccessDTO.Permission.IsEditableAll;

        //    return isEditableAll;
        //}

        public static string GetCurrentDate()
        {
            return DateTime.UtcNow.Year + "-" + DateTime.UtcNow.Month + "-" + DateTime.UtcNow.Day + " " + DateTime.UtcNow.Hour + ":" + DateTime.UtcNow.Minute + ":" + DateTime.UtcNow.Second;
        }

        public static string GetDateFormat(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                DateTime dt = DateTime.MinValue;
                date = dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;
            }

            return date;
        }

        public static string GetFirstLetterFromStringBySplit(string inputString)
        {
            string[] splitresult = inputString.Split(' ');
            string finalstringvalue = string.Empty;
            foreach (string value in splitresult)
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    finalstringvalue += value.Trim()[0];
                }

            }
            return finalstringvalue;
        }

        public static DateTime GetDateTimeByTimeZone(string timeZone, DateTime dateTime)
        {
            if (string.IsNullOrEmpty(timeZone))
            {
                timeZone = Constants.EasternTimeZone;
            }
            var timeToConvert = dateTime;
            TimeZoneInfo timeZoneToConvert = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(timeToConvert, timeZoneToConvert);
            return convertedDateTime;
        }

        public static int GetCurrentSeason()
        {
            int currentSeason = DateTime.Now.Year;
            if (DateTime.Now.Month <= 5)
            {
                currentSeason = DateTime.Now.Year - 1;
            }
            else
            {
                currentSeason = DateTime.Now.Year;
            }

            return currentSeason;
        }
        public static string GetFirstLetterFromStringBySplit(string inputString, char splitCharacterValue)
        {
            string[] splitresult = inputString.Split(splitCharacterValue);
            string finalstringvalue = string.Empty;
            foreach (string value in splitresult)
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    finalstringvalue += value.Trim()[0];
                }

            }
            return finalstringvalue;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static IEnumerable<List<T>> Batch<T>(IEnumerable<T> collection, int batchSize)
        {
            List<T> nextbatch = new List<T>(batchSize);
            foreach (T item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>(batchSize);
                }
            }
            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

        public static string ReadHtmlTemplate(string htmlTemplateFilePath)
        {
            string baseHtmlString = string.Empty;
            string htmlfilename = string.Empty;
            htmlfilename = System.Configuration.ConfigurationManager.AppSettings[htmlTemplateFilePath].ToString();
            using (StreamReader streamReader = new StreamReader(HttpContext.Current.Server.MapPath("~/HTMLTemplates/" + htmlfilename)))
            {
                baseHtmlString = streamReader.ReadToEnd();
            }
            return baseHtmlString;
        }

        public static List<string> SplitStringToRows(string cmt, int chunkSize, string fontFamily, int fontSize)
        {
            List<string> returnList = new List<string>();
            //decimal characterwidth = 0m;
            string line = null;
            //int characterwidth = 0;
            float characterwidth = 0f;

            if (!string.IsNullOrEmpty(cmt))
            {
                Font stringFont = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                string[] lines = cmt.Split(
    new[] { "\r\n", "\r", "\n" },
    StringSplitOptions.None
);
                foreach (var comment in lines)
                {
                    for (int i = 0; i < comment.Length; i++)
                    {

                        //if (Char.IsUpper(comment[i]))
                        //{
                        //    characterwidth += 1.1647m;
                        //}
                        //else
                        //{
                        //    characterwidth += 1;
                        //}
                        if (!string.IsNullOrEmpty(line))
                        {
                            using (var graphics = Graphics.FromImage(new Bitmap(1, 1)))
                            {
                                characterwidth = graphics.MeasureString(line, stringFont).ToPointF().X;//
                            }
                        }
                        else
                        {
                            characterwidth = 0;
                        }
                        if (characterwidth < Single.Parse(chunkSize.ToString()))
                        {
                            line += comment[i];
                        }
                        else
                        {
                            characterwidth = 0;

                            if ((line.LastIndexOf(' ') != line.Length - 1) && !Char.IsWhiteSpace(comment[i]))
                            {
                                var wordarr = line.Split(' ');
                                string[] b = new string[wordarr.Length - 1];
                                Array.Copy(wordarr, b, wordarr.Length - 1);

                                returnList.Add(string.Join(" ", b));
                                line = wordarr[wordarr.Length - 1] + comment[i].ToString();
                            }
                            else
                            {

                                returnList.Add(line);
                                line = Char.IsWhiteSpace(comment[i]) ? "" : comment[i].ToString();
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(line))
                    {
                        returnList.Add(line);
                        line = null;
                    }
                }

            }

            //if (!string.IsNullOrEmpty(comment))
            //{
            //    for (int i = 0; i < comment.Length; i += chunkSize)
            //    {
            //        if (i + chunkSize > comment.Length) chunkSize = comment.Length - i;
            //        returnList.Add(comment.Substring(i, chunkSize));
            //    }
            //}
            return returnList;
        }

        private static string GetLimitedText(string fieldvalue, int maxcharacter)
        {
            string summarycontent = string.Empty;
            if (!string.IsNullOrEmpty(fieldvalue))
            {
                if (fieldvalue.Length > maxcharacter)
                {

                    summarycontent = fieldvalue.Substring(0, maxcharacter);
                }
                else
                {
                    summarycontent = fieldvalue;
                }
            }
            else
            {
                summarycontent = "";
            }
            return summarycontent;
        }


        public static string GetTextByLength(string inputString, int maxLength)
        {
            string newStringValue = string.Empty;
            if (!string.IsNullOrEmpty(inputString) && inputString.Length > maxLength)
            {

                newStringValue = GetLimitedText(inputString, maxLength);

            }
            else if (string.IsNullOrEmpty(inputString))
            {
                return newStringValue;
            }
            else
            { newStringValue = inputString; }
            return newStringValue;
        }

        public static string GeneratePassword(string rawPassword)
        {
            string cipherText = Encrypt(rawPassword);
            return cipherText;   
        }

        public static string Encrypt(string plainText)
        {
            string key = getKey();

            string cipherText;
            var rijndael = new RijndaelManaged()
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                BlockSize = 256,
                Padding = PaddingMode.Zeros,
            };
            ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, null);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                        streamWriter.Flush();
                    }                   
                    cipherText = Convert.ToBase64String(memoryStream.ToArray());                    
                }
            }

            return cipherText;
        }

        public static string getKey()
        {
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes("IKr0Qt1iimPvsOoHW9IRi14rM9p97Tj8nT7QsjnItHOxmJmRqKHfqvJdFyHocic"))).Replace("-", "").ToLower();          
        }
    }
}
