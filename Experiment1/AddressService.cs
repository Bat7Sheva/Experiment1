using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Experiment1
{
    public class AddressService
    {
        string[] addresses = new string[]
        {
            "נחליאל",
            "רחוב אליהו הנביא",
            "הזית 3 א'",
            "נחל דולב 82 דירה 18",
            "אריה פסטרנק 1 תל אביב",
            "ספיר 13, ראש העין",
            "אורי צבי גרינברג 27, 12",
            "נחל מישר 11/12",
            "הגאולים 11, 84417",
            "ת.ד 73",
            "המירון 31",
            "267",
            "החשמונאים 8/4",
            "מפרשית 6 אשקלון",
            "אלהודא 100 רהט",
            "מנדלי מוכר ספרים 8",
            "כצנלסון 18"
        };

        public void CheckAddress()
        {
            foreach (string address in addresses)
            {
                ProcessAddress(address);
            }
        }

        public void ProcessAddress(string rawAddress)
        {
            // שלב 1: קבלת הנתונים מהשדה
            if (string.IsNullOrEmpty(rawAddress)) return;

            // שלב 2: ניתוח הכתובת
            var (streetName, houseNumber, city, remark) = ParseAddress(rawAddress);

            if (!string.IsNullOrEmpty(remark))
            {
                // אם הכתובת כוללת הערות (כגון ת.ד.)
                Console.WriteLine($"Remark: {remark}");
            }
            else
            {
                //Console.WriteLine($"Street Name : {streetName}, House Number: {houseNumber}, City: {city}");
                Console.WriteLine(string.Join(" ",
                                 !string.IsNullOrEmpty(streetName) ? $"Street Name: {streetName}" : null,
                                 !string.IsNullOrEmpty(houseNumber) ? $",House Number: {houseNumber}" : null,
                                 !string.IsNullOrEmpty(city) ? $",City: {city}" : null));
            }
        }

        private (string StreetName, string HouseNumber, string City, string Remark) ParseAddress(string address)
        {
            string streetName = null, houseNumber = null, city = null, remark = null;

            var patterns = new[]
            {
                  // דפוס: תיבת דואר (ת.ד)
                 @"^ת\.ד\s*(?<Remark>\d+)$",

                 // דפוס: רחוב, מספר בית עם אותיות (כולל תו ' כמו א')
                 //@"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת']*)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

                 // דפוס: רחוב, מספר בית עם מספר דירה
                 @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+)(?:\s*דירה\s*(?<ApartmentNumber>\d+))?\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

                 // דפוס: רחוב, מספר בית עם אותיות
                 @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(?:,\s*\d+[א-ת]?)?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

                 // דפוס: רחוב, מספר בית, עיר (עובד גם ללא פסיק אחרי המספר)
                 @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת']*)\s+(?<City>[^\d,]+)$",

                 // דפוס: מספר בית, רחוב, עיר
                 @"^(?<HouseNumber>\d+[א-ת'/-]*)\s+(?<StreetName>[^\d,]+?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

                 // דפוס: רחוב בלבד
                 @"^(?<StreetName>[^\d,]+?)$"
             };


            foreach (var pattern in patterns)
            {
                var match = Regex.Match(address, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    streetName = match.Groups["StreetName"].Value.Replace("רחוב", "").Trim();
                    houseNumber = match.Groups["HouseNumber"].Value.Trim();
                    city = match.Groups["City"].Value.Trim();
                    remark = match.Groups["Remark"].Value.Trim();

                    if (match.Groups["ApartmentNumber"].Success)
                    {
                        houseNumber = $"{houseNumber}/{match.Groups["ApartmentNumber"].Value.Trim()}";
                    }

                    //מס' בית + כניסה
                    if (!string.IsNullOrEmpty(city) && Regex.IsMatch(city, @"^[א-ת]('|)?$", RegexOptions.IgnoreCase))
                    {
                        houseNumber = $"{houseNumber} {city}".Trim();
                        city = null;
                    }

                    return (streetName, houseNumber, city, remark);
                }
            }

            return (null, null, null, address); // אם אין התאמה, החזר את הכתובת כהערה
        }
    }
}
