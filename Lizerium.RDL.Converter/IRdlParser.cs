/*
 * Author: Nikolay Dvurechensky, Jason Hood (adoxa)
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 18 апреля 2026 14:44:47
 * Version: 1.0.4
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizeriumRDL
{
    public interface IRdlParser
    {
        /// <summary>
        /// Конвертирует XML в FRC текст
        /// </summary>
        /// <param name="xml">Данные информационной карты</param>
        /// <param name="id">Идентификатор информационноый карты (для логирования)</param>
        /// <param name="nameFile">Имя файла информационной карты (для логирования)</param>
        /// <returns>FRC текст</returns>
        public string ConvertXMLtoFRC(string xml, string id, string nameFile);

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
            string defStr, string id = "", string xml = "");

        /// <summary>
        /// Конвертирует специфично заданный цвет в  (0xFFFFFFFF в 0xFFFFFF переворачия наоборот)
        /// Чтобы посмотреть реальный цвет нужно FF3902 каждый бит поменять местами #FF9320 (HTMLHEX)
        /// 0x2093FF08 в FF3902 
        /// </summary>
        /// <param name="hexString">Строка формата 0xFFFFFFFF</param>
        /// <returns>0xFFFFFF</returns>
        public string ConvertDataToColor(string hexString);

        /// <summary>
        /// Каждые N символов разделяет текст знаком `\` добавляя перенос строки и табуляцию
        /// 
        /// \r\n - перевод строки в Windows
        /// \t - табуляция
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="maxLineLength">Длинна максимальная (по словам рассчитает)</param>
        /// <returns>string</returns>
        public string FormatText(string input, int maxLineLength = 111);
    }
}
