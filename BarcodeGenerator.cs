using System;
using System.Text;

namespace PalengKart
{
    public static class BarcodeGenerator
    {
        static Random random = new Random();

        // Generates a valid EAN-13 barcode: "890" prefix + 9 random digits + check digit.
        // Pass an "exists" check to avoid generating a barcode that is already in use.
        public static string GenerateBarcodeNumber(Func<string, bool>? exists = null)
        {
            string barcode;
            do
            {
                string first12 = "890" + random.Next(0, 1000000000).ToString("D9");
                barcode = first12 + CalculateCheckDigit(first12);
            }
            while (exists != null && exists(barcode));

            return barcode;
        }

        public static int CalculateCheckDigit(string first12)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = first12[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            return (10 - (sum % 10)) % 10;
        }

        public static void DisplayBarcodeAscii(string barcode)
        {
            // Simple visual: each digit becomes a pattern of thick/thin bars
            var bars = new StringBuilder("▌▌");
            foreach (char c in barcode)
            {
                int d = c - '0';
                bars.Append(d % 2 == 0 ? "█ " : "▌ ");
                bars.Append(d >= 5 ? "█" : "▌");
            }
            bars.Append("▌▌");

            string line = bars.ToString();
            Console.WriteLine(line);
            Console.WriteLine(line);
            Console.WriteLine(line);
            Console.WriteLine(barcode.PadLeft((line.Length + barcode.Length) / 2));
        }
    }
}
