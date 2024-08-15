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
             "רמבם 8",
             "גולומב 40/5",
             "הרב קוק 28",
             "שדרת הפיקוסים 6",
             "נחל דולב 82 דירה 18",
             "אריה פסטרנק 1 תל אביב",
             "נתן שאול 14",
             "האחדות 6",
             "הדובדבן 3",
             "ספיר 13, ראש העין",
             "שד עמק איילון 21",
             "סעדיה גאון 6",
             "אורי צבי גרינברג 27, 12",
             "סן מרטין 4/2",
             "אהרון קציר 4/1",
             "פתח תקווה",
             "ברנדיס 3",
             "צה\"ל 74",
             "התיכון 53",
             "הזית 3 א'",
             "רש\"י 76",
             "טשרניחובסקי 21",
             "הכרכום מס 8",
             "325",
             "זרעית",
             "מוטה גור 5",
             "רמברדט 32 9",
             "נחל מישר 11/12",
             "הגאולים 11, 84417",
             "שיבולים 115",
             "עונות שנה 8/1",
             "ביאליק 8",
             "האנפה 9",
             "נעמי שמר 10",
             "זוהר 1",
             "בני נצרים 287",
             "דוד אלעזר 21/13",
             "הלפרין 6",
             "גיפסנית",
             "יעקב אבינו 2",
             "ת.ד 73",
             "משה שרת 18",
             "השדה 52",
             "אשכול 19",
             "טבריה 417/2",
             "המירון 31",
             "267",
             "החשמונאים 8/4"
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

            // דפוסים שונים לפירוק הכתובת
            var patterns = new[]
            {
        // דפוס: רחוב, מספר בית עם מספר דירה (למשל: "נחל דולב 82 דירה 18")
        @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+)(?:\s*דירה\s*(?<ApartmentNumber>\d+))?\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        // דפוס: רחוב, מספר בית עם אותיות (למשל: "הזית 3 א'")
        @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(?:,\s*\d+[א-ת]?)?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        // דפוס: רחוב, מספר בית, עיר
        @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(?:,\s*\d+[א-ת]?)?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        // דפוס: מספר בית, רחוב, עיר (אופציונלי)
        @"^(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(?:,\s*\d+[א-ת]?)?)\s+(?<StreetName>[^\d,]+?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        // דפוס: רחוב בלבד
        @"^(?<StreetName>[^\d,]+?)$"
    };

            // חפש לפי כל דפוס
            foreach (var pattern in patterns)
            {
                var match = Regex.Match(address, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    streetName = match.Groups["StreetName"].Value.Trim();
                    houseNumber = match.Groups["HouseNumber"].Value.Trim();
                    city = match.Groups["City"].Value.Trim();
                    remark = match.Groups["Remark"].Value.Trim();

                    // אם יש גם מספר דירה, שלב אותו עם מספר הבית
                    if (match.Groups["ApartmentNumber"].Success)
                    {
                        houseNumber = $"{houseNumber}/{match.Groups["ApartmentNumber"].Value.Trim()}";
                    }

                    return (streetName, houseNumber, city, remark);
                }
            }

            return (null, null, null, address);
        }


        //private (string StreetName, string HouseNumber, string City, string Remark) ParseAddress(string address)
        //{
        //    string streetName = null, houseNumber = null, city = null, remark = null;

        //    // דפוסים שונים לפירוק הכתובת
        //    var patterns = new[]
        //    {
        //        // דפוס: רחוב, מספר בית עם מספר דירה (למשל: "נחל דולב 82 דירה 18")
        //        @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+)\s*דירה\s*(?<ApartmentNumber>\d+)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        //        // דפוס: רחוב, מספר בית עם אותיות (למשל: "הזית 3 א'")
        //        @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(,\s*\d+[א-ת]?)?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        //        // דפוס: רחוב, מספר בית, עיר
        //        @"^(?<StreetName>[^\d,]+?)\s+(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(,\s*\d+[א-ת]?)?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        //        // דפוס: מספר בית, רחוב, עיר (אופציונלי)
        //        @"^(?<HouseNumber>\d+[א-ת]?(?:/\d*[א-ת]?)?(,\s*\d+[א-ת]?)?)\s+(?<StreetName>[^\d,]+?)\s*(?:[,;]\s*(?<City>[^\d,]*))?$",

        //        // דפוס: רחוב בלבד
        //        @"^(?<StreetName>[^\d,]+?)$",

        //        //// דפוס: הערות בלבד (כגון ת.ד.)
        //        //@"^(?<Remark>.+)$"
        //    };

        //    // חפש לפי כל דפוס
        //    foreach (var pattern in patterns)
        //    {
        //        var match = Regex.Match(address, pattern, RegexOptions.IgnoreCase);
        //        if (match.Success)
        //        {
        //            streetName = match.Groups["StreetName"].Value.Trim();
        //            houseNumber = match.Groups["HouseNumber"].Value.Trim();
        //            city = match.Groups["City"].Value.Trim();
        //            remark = match.Groups["Remark"].Value.Trim();

        //            //if (!string.IsNullOrEmpty(remark) && (remark.Contains("ת.ד.")))
        //            //{
        //            //    return (null, null, null, remark);
        //            //}

        //            if (!string.IsNullOrEmpty(streetName) && streetName.StartsWith("רחוב"))
        //            {
        //                streetName = streetName.Substring("רחוב".Length).Trim();
        //            }

        //            var houseNumberMatch = Regex.Match(address, @"\d+[א-ת]?(?:/\d*[א-ת]?)?(,\s*\d+[א-ת]?)?");
        //            if (houseNumberMatch.Success)
        //            {
        //                var houseNumberStart = houseNumberMatch.Index;
        //                streetName = address.Substring(0, houseNumberStart).Trim();
        //                houseNumber = houseNumberMatch.Value.Trim();
        //                var cityStart = houseNumberStart + houseNumber.Length;
        //                city = address.Substring(cityStart).Trim();

        //                if (!string.IsNullOrEmpty(city) && Regex.IsMatch(city, @"\d"))
        //                {
        //                    if (!address.Contains("דירה"))
        //                    {
        //                        houseNumber = $"{houseNumber} {city}".Trim();
        //                    }
        //                    else if (match.Groups["ApartmentNumber"].Success)
        //                    {
        //                        houseNumber = $"{houseNumber}/{match.Groups["ApartmentNumber"].Value.Trim()}";
        //                    }
        //                    city = null;
        //                }

        //                return (streetName, houseNumber, city, null);
        //            }
        //            return (streetName, houseNumber, city, remark);

        //        }
        //        else
        //        {
        //            // הדפס את הדפוס שנכשל כדי להבין מה לא תואם
        //            Console.WriteLine($"Pattern failed: {pattern}");
        //        }
        //    }
        //    return (null, null, null, address);
        //}
    }
}
