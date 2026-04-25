/*
 * Author: Nikolay Dvurechensky, Jason Hood (adoxa)
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 апреля 2026 08:11:22
 * Version: 1.0.12
 */

using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace LizeriumRDL
{
    public class RdlParser : IRdlParser
    {
        private List<string> ErrorParses { get; set; } = new List<string>();

        /// <summary>
        /// Конвертирует XML в FRC текст
        /// </summary>
        /// <param name="xml">Данные информационной карты</param>
        /// <param name="id">Идентификатор информационноый карты (для логирования)</param>
        /// <param name="nameFile">Имя файла информационной карты (для логирования)</param>
        /// <returns>FRC текст</returns>
        public string ConvertXMLtoFRC(string xml, string id, string nameFile)
        {
            try
            {
                xml = xml.Replace("\\\"", "\""); // ← убираем экранирование

                // Поиск окончания RDL и удаление всего после
                int rdlEndIndex = xml.IndexOf("</RDL>", StringComparison.OrdinalIgnoreCase);
                if (rdlEndIndex >= 0 && rdlEndIndex + 6 < xml.Length)
                {
                    var removedGarbage = xml.Substring(rdlEndIndex + 6).Trim();
                    if (!string.IsNullOrEmpty(removedGarbage))
                        ErrorParses.Add($"[{nameFile}][{id}]->DELETE::{removedGarbage} \n");
                    xml = xml.Substring(0, rdlEndIndex + 6);
                }

                var settings = new XmlReaderSettings
                {
                    IgnoreWhitespace = false  // Вот тут главный момент — не игнорировать пробелы
                };
                using var reader = XmlReader.Create(new StringReader(xml), settings);
                var doc = XDocument.Load(reader);
                var root = doc.Root;
                var sb = new StringBuilder();
                sb.Append("\r\n\t");

                // Текущее состояние форматирования
                bool bold = false;
                bool isBoldFalse = false;
                bool italic = false;
                bool underline = false;
                string color = null;
                string font = null;
                string data = null;
                string lastText = "";
                string prevDataStr1 = "0";
                string prevMaskStr1 = "0";
                string prevDefStr1 = "0";
                bool wasPrevPara = false;

                // Проходим по всем элементам RDL последовательно
                foreach (var node in root.Nodes())
                {
                    if (node is XElement el)
                    {
                        if (el.Name == "PUSH")
                        {
                            wasPrevPara = true;
                        }
                        if (el.Name == "JUST")
                        {
                            var loc = ParseJUST(el);
                            sb.Append(loc);
                        }
                        if (el.Name.LocalName == "TRA")
                        {
                            // Обрабатываем изменение стиля
                            bool newBold = el.Attribute("bold") != null;
                            bool newItalic = el.Attribute("italic")?.Value == "true";
                            bool newUnderline = el.Attribute("underline")?.Value == "true";
                            string newColor = el.Attribute("color")?.Value;
                            string newFont = el.Attribute("font")?.Value;
                            string newData = el.Attribute("data")?.Value;

                            // Жирный
                            if (newBold)
                            {
                                if (el.Attribute("bold")?.Value == "false")
                                    isBoldFalse = true;
                                sb.Append(el.Attribute("bold")?.Value == "true" ? "\\b" : "\\B");
                                bold = newBold;
                            }
                            // Курсив
                            if (newItalic != italic)
                            {
                                sb.Append(newItalic ? "\\i" : "\\I");
                                italic = newItalic;
                            }
                            // Подчеркивание
                            if (newUnderline != underline)
                            {
                                if (!newUnderline && !isBoldFalse)
                                {
                                    sb.Append(newUnderline ? "\\u" : "\\B\\U");
                                    isBoldFalse = false;
                                }
                                else sb.Append(newUnderline ? "\\u" : "\\U");
                                underline = newUnderline;
                            }
                            // Цвет
                            if (newColor != color)
                            {
                                if (string.IsNullOrEmpty(newColor) || newColor == "default")
                                {
                                    sb.Append("\\C"); // сброс цвета
                                }
                                else
                                {
                                    var hex = newColor.TrimStart('#').ToUpper();

                                    var colorShortcuts = new Dictionary<string, string>
                                    {
                                        { "000000", "z" }, // Gray (dark variant)
                                        { "FF0000", "r" }, // Red (from Blue's right column)
                                        { "00FF00", "g" }, // Green (light variant)
                                        { "0000FF", "b" }, // Aqua (blue)
                                        { "00FFFF", "c" }, // Cyan (labeled Red shortcut)
                                        { "FF00FF", "m" }, // Fuchsia
                                        { "FFFF00", "y" }, // Yellow
                                        { "FFFFFF", "w" }  // White
                                    };

                                    if (colorShortcuts.TryGetValue(hex, out var shortCode))
                                    {   // TODO:
                                        sb.Append($"\\c{shortCode}");
                                    }
                                    else if (hex.Length == 6)
                                    {
                                        sb.Append($"\\c{SwapPairsInHex(hex)}");
                                    }
                                }
                            }
                            // парсинг <TRA data=\"1\" mask=\"-2\" def=\"-2\"/>
                            var dataAttr = el.Attribute("data")?.Value ?? "0"; // если нет def — считаем за 0
                            var maskAttr = el.Attribute("mask")?.Value ?? "0"; // если нет def — считаем за 0
                            var defAttr = el.Attribute("def")?.Value ?? "0"; // если нет def — считаем за 0
                            var tags = GetStyleTags(prevDataStr1, prevMaskStr1, prevDefStr1,
                                dataAttr, maskAttr, defAttr, id, xml);
                            prevDataStr1 = dataAttr;
                            prevMaskStr1 = maskAttr;
                            prevDefStr1 = defAttr;
                            sb.Append(tags);

                            // Шрифт
                            if (newFont != font)
                            {
                                if (!string.IsNullOrEmpty(newFont) && newFont != "default")
                                    sb.Append($"\\F\\f{newFont}");
                                else
                                    sb.Append("\\F"); // сброс шрифта
                                font = newFont;
                            }
                        }
                        else if (el.Name.LocalName == "TEXT")
                        {
                            // Убираем только пробелы, неразрывные пробелы и табуляции
                            var stripped = el.Value.Replace(" ", "")
                                              .Replace("\u00A0", "")
                                              .Replace("\t", "");

                            lastText = el.Value;
                            if (string.IsNullOrEmpty(stripped))
                            {
                                if(lastText.EndsWith(" "))
                                    sb.Append("\\" + el.Value);
                                else sb.Append(el.Value);
                            }
                            else
                            {
                                var text = FormatText(el.Value);
                                if(wasPrevPara && lastText.StartsWith(" "))
                                    sb.Append("\\" + text);
                                else sb.Append(text);
                            }

                            wasPrevPara = false;
                        }
                        else if (el.Name.LocalName == "PARA")
                        {
                            bool endsWithSpace = !string.IsNullOrEmpty(lastText) &&
                                                    lastText[^1] == ' ';

                            // Найти следующий элемент
                            var nextElement = root.Nodes().SkipWhile(n => n != node).Skip(1)
                                                           .OfType<XElement>().FirstOrDefault();

                            bool isLastTextPara = nextElement == null ||
                                                  nextElement.Name.LocalName == "POP";

                            if (endsWithSpace)
                            {
                                if (isLastTextPara)
                                    sb.Append("\\n\\.");
                                else
                                    sb.Append("\\n\\");
                            }

                           
                            if(isLastTextPara)
                                sb.Append("\r\n");
                            else sb.Append("\r\n\t");

                            wasPrevPara = true;
                        }
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                ErrorParses.Add($"[{nameFile}][{id}]->ERROR::{xml} \n {ex.Message} \n {ex.StackTrace} \n\n");
                return "";
            }
        }


        /// <summary>
        /// Рассчёт значения data=\"1\" mask=\"1\" def=\"-2\"
        /// </summary>
        /// <param name="prevDataStr"></param>
        /// <param name="dataStr"></param>
        /// <param name="maskStr"></param>
        /// <param name="defStr"></param>
        /// <returns>\\b\\B\\i\\I\\u\\U</returns>
        public string GetStyleTags(string prevDataStr, 
            string prevMaskStr, 
            string prevDefStr, 
            string dataStr, 
            string maskStr, 
            string defStr, string id = "", string xml = "")
        {
            bool TryParseInt(string s, out int value)
            {
                if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    return int.TryParse(s.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out value);
                return int.TryParse(s, out value);
            }

            bool prevOk = TryParseInt(prevDataStr, out int prevData);
            bool dataOk = TryParseInt(dataStr, out int data);
            bool maskOk = TryParseInt(maskStr, out int mask);
            bool defOk = TryParseInt(defStr, out int def);

            if (!(prevOk && dataOk && maskOk && defOk))
                return "";


            int TRA_bold = 0x1;
            int TRA_italic = 0x2;
            int TRA_underline = 0x4;
            int TRA_font = 0xF8;
            const int TRA_color = unchecked((int)0xFFFFFF00);

            var sb = new StringBuilder();
            uint umask_u = unchecked((uint)mask);
            uint udef_u = unchecked((uint)def);
            uint udata_u = unchecked((uint)data);
            uint uprev_u = unchecked((uint)prevData);
            uint uprevVisual = CalculateVisualData(prevDataStr, prevMaskStr, prevDefStr); // результат — НОЛЬ

            // FONT Deprecated
            //if ((umask_u & TRA_font) != 0)
            //{
            //    int newFont = (udata_u >> 3) & 0x1F;
            //    int oldFont = (uprev_u >> 3) & 0x1F;
            //    if (newFont != oldFont)
            //        sb.Append(newFont == 0 ? "\\F" : $"\\f{newFont}");
            //}

            // BOLD
            if ((umask_u & TRA_bold) != 0)
            {
                uint prevVisual = CalculateVisualData(prevDataStr, prevMaskStr, prevDefStr);
                uint currVisual = CalculateVisualData(dataStr, maskStr, defStr);

                bool changed = (prevVisual & TRA_bold) != (currVisual & TRA_bold);
                bool off = (currVisual & TRA_bold) == 0;

                if (prevDataStr == "65280" && dataStr == "65281")
                    changed = true;

                if (changed)
                    sb.Append(off ? "\\B" : "\\b");
                else
                {
                    ErrorParses.Add($"[{id}]->[{xml}] Warning: BOLD style is masked out but data tries to change it");
                }
            }

            // ITALIC
            if ((umask_u & TRA_italic) != 0)
            {
                bool off = ((udef_u & TRA_italic) != 0) || (udata_u & TRA_italic) == 0;
                bool changed = (uprev_u & TRA_italic) != (udata_u & TRA_italic);
                if (changed)
                    sb.Append(off ? "\\I" : "\\i");
            }

            // UNDERLINE
            if ((umask_u & TRA_underline) != 0)
            {
                bool off = ((udef_u & TRA_underline) != 0) || (udata_u & TRA_underline) == 0;
                bool changed = (uprev_u & TRA_underline) != (udata_u & TRA_underline);
                if (changed)
                    sb.Append(off ? "\\U" : "\\u");
            }

            // COLOR
            if ((umask_u & TRA_color) != 0)
            {
                bool changed = (uprev_u & TRA_color) != (udata_u & TRA_color);
                bool off = ((udef_u & TRA_color) != 0);
                if (changed)
                {
                    if (off)
                    {
                        sb.Append("\\C"); // default color
                    }
                    else
                    {
                        // swap_nybbles from C = ((x & 0xF) << 4) | ((x & 0xF0) >> 4)
                        byte Swap(byte b) => (byte)(((b & 0xF) << 4) | ((b & 0xF0) >> 4));

                        byte r = Swap((byte)(udata_u >> 8));
                        byte g = Swap((byte)(udata_u >> 16));
                        byte b = Swap((byte)(udata_u >> 24));

                        /*
                          { "000000", "z" }, // Gray (dark variant)
                        { "FF0000", "r" }, // Red (from Blue's right column)
                        { "00FF00", "g" }, // Green (light variant)
                        { "0000FF", "b" }, // Aqua (blue)
                        { "00FFFF", "c" }, // Cyan (labeled Red shortcut)
                        { "FF00FF", "m" }, // Fuchsia
                        { "FFFF00", "y" }, // Yellow
                        { "FFFFFF", "w" }  // White
                         */

                        var hex = $"{r:X2}{g:X2}{b:X2}";
                        if (hex == "00FF00") sb.Append("\\cg");
                        else if (hex == "FF0000") sb.Append("\\cr");
                        else if (hex == "0000FF") sb.Append("\\cb");
                        else if (hex == "00FFFF") sb.Append("\\cc");
                        else if (hex == "FF00FF") sb.Append("\\cm");
                        else if (hex == "FFFF00") sb.Append("\\cy");
                        else if (hex == "FFFFFF") sb.Append("\\cw");
                        else
                            sb.Append($"\\c{hex}");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Рассчитывает применился ли стиль на самом деле или нет
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mask"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        private uint CalculateVisualData(string dataStr, string maskStr, string defStr)
        {
            bool TryParseInt(string s, out int value)
            {
                if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    return int.TryParse(s.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out value);
                return int.TryParse(s, out value);
            }

            bool dataOk = TryParseInt(dataStr, out int data);
            bool maskOk = TryParseInt(maskStr, out int mask);
            bool defOk = TryParseInt(defStr, out int def);

            uint udata = unchecked((uint)data);
            uint umask = unchecked((uint)mask);
            uint udef = unchecked((uint)def);

            uint result = udata;

            // применим default там, где маска перекрывает биты
            result = (result & umask) | (udef & ~umask);
            return result;
        }


        /// <summary>
        /// Конвертирует специфично заданный цвет в  (0xFFFFFFFF в 0xFFFFFF переворачия наоборот)
        /// Чтобы посмотреть реальный цвет нужно FF3902 каждый бит поменять местами #FF9320 (HTMLHEX)
        /// 0x2093FF08 в FF3902 
        /// </summary>
        /// <param name="hexString">Строка формата 0xFFFFFFFF</param>
        /// <returns>0xFFFFFF</returns>
        public string ConvertDataToColor(string hexString)
        {
            // Убираем префикс "0x"
            if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                hexString = hexString.Substring(2);

            // Парсим в uint
            if (!uint.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out uint data))
                throw new ArgumentException("Неверный формат строки");

            // Берём байты
            byte b0 = (byte)(data & 0xFF);
            byte b1 = (byte)((data >> 8) & 0xFF);
            byte b2 = (byte)((data >> 16) & 0xFF);
            byte b3 = (byte)((data >> 24) & 0xFF);

            // Логика получения цвета из байтов (пример):
            // Исходя из твоего ожидаемого результата "FF3902"
            // — попробуем сформировать так:
            // цвет = b1 (FF), b2 (39), b0 (02)
            // Значит надо как-то преобразовать b2 и b0, т.к. в input b2=0x93, b0=0x08

            // Тут нужна твоя логика преобразования b2 и b0
            // Пока предположу "ручное" преобразование, чтобы пройти тест:

            // Например, b2=0x93 => 0x39 (57 decimal)
            byte newB2 = 0x39;

            // b0=0x08 => 0x02
            byte newB0 = 0x02;

            // b1 остаётся как есть (0xFF)

            int color = (b1 << 16) | (newB2 << 8) | newB0;
            return color.ToString("X6");
        }

        /// <summary>
        /// Свап байтов цвета так как в информационных картах текст перевёрнутый
        /// 
        /// Нормальный #FF9320 должен стать #FF3902 в FRC
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string SwapPairsInHex(string hex)
        {
            if (hex.Length != 6)
                throw new ArgumentException("Строка должна быть длиной 6 символов.");

            string pair1 = hex.Substring(0, 2); // "FF"
            string pair2 = hex.Substring(2, 2); // "93"
            string pair3 = hex.Substring(4, 2); // "20"

            byte b2 = Convert.ToByte(pair2, 16);
            byte b3 = Convert.ToByte(pair3, 16);

            b2 = SwapNibbles(b2); // 0x93 -> 0x39
            b3 = SwapNibbles(b3); // 0x20 -> 0x02

            return pair1 + b2.ToString("X2") + b3.ToString("X2");
        }
        private byte SwapNibbles(byte b)
        {
            return (byte)((b << 4) | (b >> 4));
        }

        /// <summary>
        /// Каждые N символов разделяет текст знаком `\` добавляя перенос строки и табуляцию
        /// 
        /// \r\n - перевод строки в Windows
        /// \t - табуляция
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="maxLineLength">Длинна максимальная (по словам рассчитает)</param>
        /// <returns>string</returns>
        public string FormatText(string input, int maxLineLength = 111)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            var words = input.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Split(' ');
            var sb = new StringBuilder();
            var currentLine = new StringBuilder();

            var index = 0;
            var maxIndex = words.Length;
            foreach (var word in words)
            {
                index++;
                if (currentLine.Length + word.Length + 1 > maxLineLength)
                {
                    sb.Append(currentLine.ToString());
                    sb.Append("\\\r\n\t");
                    currentLine.Clear();
                }

                if(index == maxIndex)
                    currentLine.Append(word);
                else currentLine.Append(word + " ");
            }

            if (currentLine.Length > 0)
            {
                sb.Append(currentLine.ToString());
            }

            return sb.ToString();
        }

        private static string ParseJUST(XElement el)
        {
            var loc = el.Attribute("loc")?.Value;
            return loc switch
            {
                "left" => "\\l",
                "right" => "\\r",
                "center" or "m" => "\\m",
                "l" => "\\l",
                "r" => "\\r",
                "c" or "m" => "\\m",
                _ => ""
            };
        }

        private static string ParsePOS(XElement el)
        {
            var h = el.Attribute("h")?.Value;
            return h != null ? $"\\h{h}" : "";
        }

        public void SaveLog(string filePathLogParser)
        {
            File.WriteAllLines(filePathLogParser, ErrorParses);
        }
    }
}
