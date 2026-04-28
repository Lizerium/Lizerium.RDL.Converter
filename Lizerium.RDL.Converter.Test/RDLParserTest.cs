/*
 * Author: Nikolay Dvurechensky, Jason Hood (adoxa)
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 28 апреля 2026 14:26:09
 * Version: 1.0.15
 */

namespace LizeriumRDL.Test
{
    [TestClass]
    public sealed class RDLParserTest
    {
        /// <summary>
        /// <tra data=“96” mask=“-32” def=“-1”><tra data=“24” mask=“24” def=“-25”><tra data=“65280” mask=“-32” def=“31”>//Red
        /// <tra data =“65281” mask=“-31” def=“30”>//Bold Red
        /// <tra data =“2” mask=“2” def=“-3”>//Italic
        /// <tra data =“0” mask=“2” def=“-1”>//Normal
        /// <tra data =“1” mask=“1” def=“-2”>//Bold
        /// <tra data =“5” mask=“5” def=“-6”>//Bold underline
        /// <tra data =“3” mask=“3” def=“-4”>//Bold italic
        /// </summary>
        [TestMethod]
        public void ConvertXMLtoFRC_Test()
        {
            var parser = new RdlParser();

            var i9 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
                "<RDL>" +
                "<PUSH/>" +
                "<TEXT> \"Победа\" сыграла решающую роль в восстании в День Основателей, когда шахтерам, избежавшим возмездия Бретони после того, как они казнили сэра Эдмунда Грейвза, пришлось забраться глубоко в поля астероидов. Однажды, Победа служила как база для операций, но после серьезных повреждений, она стала непригодной для жилья.</TEXT>" +
                "<PARA/>" +
                "<POP/>" +
                "</RDL>\r\n\r\n\r\n\r\n";
            var c9 = "\r\n\t" +
                "\\ \"Победа\" сыграла решающую роль в восстании в День Основателей, когда шахтерам, избежавшим возмездия Бретони \\\r\n\tпосле того, как они казнили сэра Эдмунда Грейвза, пришлось забраться глубоко в поля астероидов. Однажды, \\\r\n\t" +
                "Победа служила как база для операций, но после серьезных повреждений, она стала непригодной для жилья.\r\n";

            var r9 = parser.ConvertXMLtoFRC(i9, "60", "infocards.dll");

            // Test space first sym
            Assert.IsTrue(r9 == c9);

            var i8 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?><RDL>" +
                "<PUSH/>" +
                "<TEXT>КЛАСС: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"96\" mask=\"-32\" def=\"-1\"/>" +
                "<TEXT>ГРАВИТАЦИЯ: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"96\" mask=\"-32\" def=\"-1\"/>" +
                "<TEXT>СТЫКОВКА: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"96\" mask=\"-32\" def=\"-1\"/>" +
                "<TEXT>БЛАГОПРИЯТНЫЙ КЛИМАТ: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНЫ</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"96\" mask=\"-32\" def=\"-1\"/>" +
                "<TEXT>НАСЕЛЕНИЕ: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TRA data=\"65281\" mask=\"-31\" def=\"30\"/>" +
                "<TEXT>&gt;&gt;&gt;СОВЕРШЕННО СЕКРЕТНО&lt;&lt;&lt;</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"96\" mask=\"-31\" def=\"-1\"/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<POP/>" +
                "</RDL>\r\n\r\n\r\n\r\n";

            var c8 = "\r\n\tКЛАСС: \\crЗАСЕКРЕЧЕНО\r\n\t" +
                "\\CГРАВИТАЦИЯ: \\crЗАСЕКРЕЧЕНО\r\n\t" +
                "\\CСТЫКОВКА: \\crЗАСЕКРЕЧЕНО\r\n\t" +
                "\\CБЛАГОПРИЯТНЫЙ КЛИМАТ: \\crЗАСЕКРЕЧЕНЫ\r\n\t" +
                "\\CНАСЕЛЕНИЕ: \\crЗАСЕКРЕЧЕНО\r\n\t \r\n\t" +
                "\\b>>>СОВЕРШЕННО СЕКРЕТНО<<<\r\n\t\\B\\C \r\n";

            var r8 = parser.ConvertXMLtoFRC(i8, "38", "infocards.dll");

            // Test TRA TEXT PARA bold color
            Assert.IsTrue(r8 == c8);

            var i0 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
                "<RDL>" +
                "<PUSH/>" +
                "<TEXT>КЛАСС: Тирелль</TEXT>" +
                "<PARA/>" +
                "<TEXT>ГРАВИТАЦИЯ: Полная</TEXT>" +
                "<PARA/>" +
                "<TEXT>СТЫКОВКА: Ограничена</TEXT>" +
                "<PARA/>" +
                "<TEXT>БЛАГОПРИЯТНЫЙ КЛИМАТ: Да</TEXT>" +
                "<PARA/>" +
                "<TEXT>НАСЕЛЕНИЕ: Неизвестно </TEXT>" +
                "<PARA/>" +
                "<POP/>" +
                "</RDL>\r\n\r\n\r\n\r\n";
            var c0 = "\r\n\tКЛАСС: Тирелль\r\n\t" +
                "ГРАВИТАЦИЯ: Полная\r\n\t" +
                "СТЫКОВКА: Ограничена\r\n\t" +
                "БЛАГОПРИЯТНЫЙ КЛИМАТ: Да\r\n\t" +
                "НАСЕЛЕНИЕ: Неизвестно \\n\\.\r\n";

            var r0 = parser.ConvertXMLtoFRC(i0, "3", "infocards.dll");

            // Test TRA TEXT PARA bold color
            Assert.IsTrue(r0 == c0);


            var inputData_1 = "<RDL>" +
                 "<PUSH/>" +
                 "<TEXT>СОЮЗНИКИ:  </TEXT>" +
                 "<TRA color=\"#FF9320\"/>" +
                 "<TEXT>(их пустят практически везде)</TEXT>" +
                 "<PARA/><PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT>Фараоны, Смертник фараона</TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"true\"/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TEXT>ВРАГИ: </TEXT>" +
                 "<TRA bold=\"false\" color=\"#FF9320\"/>" +
                 "<TEXT>(убивая врагов вы станете им другом)</TEXT>" +
                 "<PARA/>" +
                 "<PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT>Враждебная раса, Ангелы Тьмы</TEXT>" +
                 "<PARA/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"true\"/>" +
                 "<TEXT>ЧЁРНЫЙ СПИСОК: </TEXT><TRA bold=\"false\" color=\"#FF9320\"/>" +
                 "<TEXT>(им лучше не появляться на границе)</TEXT>" +
                 "<PARA/>" +
                 "<PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT>ВВС Свободы, Служба безопасности Свободы, Полиция Свободы, Вооруженные Силы Бретони, Полиция Бретони, ВВС Кусари, Государственная полиция Кусари, Войска Рейнландии, Полиция Рейнландии, АЛГ - переработка отходов, Бовекс, БГМ, Гейтвей, Фармацевтия Крайера, Интеркосмическая Торговля, Тяжелые конструкции Дауманна, Технологии Киширо, Шахты Крюгера, Инжиниринг глубокого космоса, Корпорация Синтофудс, Орбитальные курорты, Республиканские грузоперевозки, Индустрия Самура, Универсальные грузоперевозки, Корпорация ПЛАНЕТФОРМ, Технологии Агейра, Драконы крови, Буншуг, Корсары, Союз фермеров, Гайане, Золотые Хризантемы, Хогоша, Юнкеры, Хакеры Лейна, Бродяги Свободы, ДПФ, Моллийцы, Изгои, Красные гессенцы, Орден, Союзники, Ксеносы, Гильдия добытчиков газа, Восставшие Кочевники, Кочевники, ВВС Свободы Гвардия, ВВС Кусари Гвардия, ВВС Рейнландии Гвардия, Люди Кресса, Люди Квинтейна, Беженец, Независимая гильдия шахтеров, Зонеры, Гильдия Наемников, Амазонки, Охотники на Амазонок, Хааки, Стражники, Украинцы, Русские, Коалиция, Орден Гвардия, Беженцы</TEXT>" +
                 "<PARA/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TEXT>ОПИСАНИЕ: </TEXT>" +
                 "<TRA color=\"#FF9320\"/>" +
                 "<TEXT>(2024)</TEXT>" +
                 "<PARA/><PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT>Фараоны - повелители Египта, что стремятся к власти над всем миром. Они уничтожают Враждебную Расу, что угрожает их власти, и Ангелов Тьмы, что ищут равновесие в хаосе. Фараоны не ищут мира, они ищут власти, и они не остановятся ни перед чем, чтобы добиться своей цели. В их руках - могущественная магия, что может уничтожить всех, кто стоит на их пути. Их армия - это армия мертвых, что восстала из гробниц, чтобы служить своим повелителям. Фараоны - это угроза всему живому, и только совместными усилиями всех сил можно остановить их вторжение.</TEXT>" +
                 "<PARA/>" +
                 "<POP/>" +
                 "</RDL>";
            var correctResult_1 = "\r\n\tСОЮЗНИКИ:  \\cFF3902(их пустят практически везде)\r\n\t\r\n\t" +
                "\\CФараоны, Смертник фараона\r\n\t\\b \r\n\t" +
                "ВРАГИ: \\B\\cFF3902(убивая врагов вы станете им другом)\r\n\t\r\n\t" +
                "\\CВраждебная раса, Ангелы Тьмы\r\n\t \r\n\t" +
                "\\bЧЁРНЫЙ СПИСОК: \\B\\cFF3902(им лучше не появляться на границе)\r\n\t\r\n\t" +
                "\\CВВС Свободы, Служба безопасности Свободы, Полиция Свободы, Вооруженные Силы Бретони, Полиция Бретони, ВВС \\\r\n\t" +
                "Кусари, Государственная полиция Кусари, Войска Рейнландии, Полиция Рейнландии, АЛГ - переработка отходов, \\\r\n\t" +
                "Бовекс, БГМ, Гейтвей, Фармацевтия Крайера, Интеркосмическая Торговля, Тяжелые конструкции Дауманна, Технологии \\\r\n\t" +
                "Киширо, Шахты Крюгера, Инжиниринг глубокого космоса, Корпорация Синтофудс, Орбитальные курорты, \\\r\n\t" +
                "Республиканские грузоперевозки, Индустрия Самура, Универсальные грузоперевозки, Корпорация ПЛАНЕТФОРМ, \\\r\n\t" +
                "Технологии Агейра, Драконы крови, Буншуг, Корсары, Союз фермеров, Гайане, Золотые Хризантемы, Хогоша, Юнкеры, \\\r\n\t" +
                "Хакеры Лейна, Бродяги Свободы, ДПФ, Моллийцы, Изгои, Красные гессенцы, Орден, Союзники, Ксеносы, Гильдия \\\r\n\t" +
                "добытчиков газа, Восставшие Кочевники, Кочевники, ВВС Свободы Гвардия, ВВС Кусари Гвардия, ВВС Рейнландии \\\r\n\t" +
                "Гвардия, Люди Кресса, Люди Квинтейна, Беженец, Независимая гильдия шахтеров, Зонеры, Гильдия Наемников, \\\r\n\t" +
                "Амазонки, Охотники на Амазонок, Хааки, Стражники, Украинцы, Русские, Коалиция, Орден Гвардия, Беженцы\r\n\t \r\n\t" +
                "ОПИСАНИЕ: \\cFF3902(2024)\r\n\t\r\n\t" +
                "\\CФараоны - повелители Египта, что стремятся к власти над всем миром. Они уничтожают Враждебную Расу, что \\\r\n\t" +
                "угрожает их власти, и Ангелов Тьмы, что ищут равновесие в хаосе. Фараоны не ищут мира, они ищут власти, и они \\\r\n\t" +
                "не остановятся ни перед чем, чтобы добиться своей цели. В их руках - могущественная магия, что может \\\r\n\t" +
                "уничтожить всех, кто стоит на их пути. Их армия - это армия мертвых, что восстала из гробниц, чтобы служить \\\r\n\t" +
                "своим повелителям. Фараоны - это угроза всему живому, и только совместными усилиями всех сил можно остановить \\\r\n\t" +
                "их вторжение.\r\n";

            var result_1 = parser.ConvertXMLtoFRC(inputData_1, "1120", "infocards.dll");

            // Test TRA TEXT PARA bold color
            Assert.IsTrue(result_1 == correctResult_1);

            var inputData_2 = "<RDL>" +
                 "<PUSH/>" +
                 "<TRA bold=\"true\"/>" +
                 "<JUST loc=\"c\"/>" +
                 "<TEXT>Тяжелый транспорт</TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"false\"/>" +
                 "<JUST loc=\"l\"/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TEXT>После освоения месторождения минеральных ресурсов, для их транспортировки были построены эти корабли.</TEXT>" +
                 "<PARA/>" +
                 "<POP/>" +
                 "</RDL>";

            var correctResult_2 = "\r\n\t" +
                "\\b\\mТяжелый транспорт\r\n\t" +
                "\\B\\l \r\n\t" +
                "После освоения месторождения минеральных ресурсов, для их транспортировки были построены эти корабли.\r\n";

            var result2 = parser.ConvertXMLtoFRC(inputData_2, "1076", "infocards.dll");
            // Test TRA TEXT PARA bold loc (short values)
            Assert.IsTrue(result2 == correctResult_2);


            var inputData_3 = "<RDL>" +
                 "<PUSH/>" +
                 "<TRA bold=\"true\"/>" +
                 "<TEXT>СВОБОДА</TEXT>" +
                 "<PARA/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TRA underline=\"true\"/>" +
                 "<TEXT>Заселенные планеты</TEXT>" +
                 "<PARA/>" +
                 "<TRA data=\"65280\" mask=\"-251\" def=\"5\"/>" +
                 "<TEXT>СЕКРЕТНО</TEXT>" +
                 "<PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"true\" underline=\"true\"/>" +
                 "<TEXT>Базы</TEXT>" +
                 "<PARA/>" +
                 "<TRA data=\"65280\" mask=\"-251\" def=\"5\"/>" +
                 "<TEXT>СЕКРЕТНО</TEXT>" +
                 "<PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"true\" underline=\"true\"/>" +
                 "<TEXT>Корпорации</TEXT>" +
                 "<PARA/>" +
                 "<TRA data=\"65280\" mask=\"-251\" def=\"5\"/>" +
                 "<TEXT>СЕКРЕТНО</TEXT>" +
                 "<PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"true\" underline=\"true\"/>" +
                 "<TEXT>Преступность</TEXT>" +
                 "<PARA/>" +
                 "<TRA data=\"65280\" mask=\"-251\" def=\"5\"/>" +
                 "<TEXT>СЕКРЕТНО</TEXT>" +
                 "<PARA/>" +
                 "<TRA color=\"default\"/>" +
                 "<TEXT> </TEXT>" +
                 "<PARA/>" +
                 "<TRA bold=\"true\" underline=\"true\"/>" +
                 "<TEXT>Продукция</TEXT>" +
                 "<PARA/>" +
                 "<TRA data=\"65280\" mask=\"-251\" def=\"5\"/>" +
                 "<TEXT>СЕКРЕТНО</TEXT>" +
                 "<PARA/>" +
                 "<POP/>" +
                 "</RDL>";

            var correctResult_3 = "\r\n\t" +
                "\\bСВОБОДА\r\n\t" +
                "\\ \\n\\\r\n\t" +
                "\\uЗаселенные планеты\r\n\t" +
                "\\B\\U\\crСЕКРЕТНО\r\n\t" +
                "\\C\\ \\n\\\r\n\t" +
                "\\b\\uБазы\r\n\t" +
                "\\B\\U\\crСЕКРЕТНО\r\n\t" +
                "\\C\\ \\n\\\r\n\t" +
                "\\b\\uКорпорации\r\n\t" +
                "\\B\\U\\crСЕКРЕТНО\r\n\t" +
                "\\C\\ \\n\\\r\n\t" +
                "\\b\\uПреступность\r\n\t" +
                "\\B\\U\\crСЕКРЕТНО\r\n\t" +
                "\\C\\ \\n\\\r\n\t" +
                "\\b\\uПродукция\r\n\t" +
                "\\B\\U\\crСЕКРЕТНО\r\n";

            var result3 = parser.ConvertXMLtoFRC(inputData_3, "546", "infocards.dll");
            // Test TRA TEXT PARA bold color data\mask\def underline
            Assert.IsTrue(result3 == correctResult_3);

            var inputData_4 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
                "<RDL>" +
                "<PUSH/>" +
                "<TRA data=\"1\" mask=\"-2\" def=\"-2\"/>" +
                "<TEXT>СОЮЗНИКИ:  </TEXT>" +
                "<TRA data='0x2093FF08' mask=\"-1\"/>" +
                "<TEXT>(их пустят практически везде)</TEXT>" +
                "<PARA/><PARA/>" +
                "<TRA data=\"0\" mask=\"-2\" def=\"-2\"/>" +
                "<TEXT>Хааки, Чёрная Армия</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"1\" mask=\"1\" def=\"-2\"/>" +
                "<TEXT> </TEXT>" +
                "<PARA/><TEXT>ВРАГИ: </TEXT>" +
                "<TRA data='0x2093FF08' mask=\"-1\"/>" +
                "<TEXT>(убивая врагов вы станете им другом)</TEXT>" +
                "<PARA/><PARA/>" +
                "<TRA data=\"0\" mask=\"-2\" def=\"-2\"/>" +
                "<TEXT>Стражники, Невский Фронт, Ксеноморфы</TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TRA data=\"1\" mask=\"1\" def=\"-2\"/>" +
                "<TEXT>ЧЁРНЫЙ СПИСОК: </TEXT>" +
                "<TRA data='0x2093FF08' mask=\"-1\"/>" +
                "<TEXT>(им лучше не появляться на границе)</TEXT>" +
                "<PARA/><PARA/>" +
                "<TRA data=\"0\" mask=\"-2\" def=\"-2\"/>" +
                "<TEXT>Неизвестно</TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TEXT>ОПИСАНИЕ: </TEXT>" +
                "<TRA data='0x2093FF08' mask=\"-1\"/>" +
                "<TEXT>(2024)</TEXT>" +
                "<PARA/><PARA/>" +
                "<TRA data=\"0\" mask=\"-2\" def=\"-2\"/>" +
                "<TEXT>Хааки - гуманоиды, тысячелетиями модифицирующие свою биологическую форму путем генетических модификаций эмбрионов. Первоначальная естественная форма Хаакской расы уже неизвестна.</TEXT>" +
                "<PARA/>" +
                "<POP/>" +
                "</RDL>\r\n\r\n";

            var correctResult_4 = "\r\n\tСОЮЗНИКИ:  \\cFF3902(их пустят практически везде)\r\n\t\r\n\t" +
                "\\CХааки, Чёрная Армия\r\n\t" +
                "\\b \r\n\tВРАГИ: \\B\\cFF3902(убивая врагов вы станете им другом)\r\n\t\r\n\t" +
                "\\CСтражники, Невский Фронт, Ксеноморфы\r\n\t \r\n\t" +
                "\\bЧЁРНЫЙ СПИСОК: \\B\\cFF3902(им лучше не появляться на границе)\r\n\t\r\n\t" +
                "\\CНеизвестно\r\n\t \r\n\t" +
                "ОПИСАНИЕ: \\cFF3902(2024)\r\n\t\r\n\t" +
                "\\CХааки - гуманоиды, тысячелетиями модифицирующие свою биологическую форму путем генетических модификаций \\\r\n\t" +
                "эмбрионов. Первоначальная естественная форма Хаакской расы уже неизвестна.\r\n";


            var result4 = parser.ConvertXMLtoFRC(inputData_4, "1899", "SBM3.dll");
            // Test TRA TEXT PARA data[color hex]\mask\def
            Assert.IsTrue(result4 == correctResult_4);

            var i5 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
                "<RDL><PUSH/><TEXT>Линкор </TEXT>" +
                "<TRA data=\"2\" mask=\"2\" def=\"-3\"/>" +
                "<TEXT>Миоко</TEXT>" +
                "<TRA data=\"0\" mask=\"2\" def=\"-1\"/>" +
                "<TEXT> был дислоцирован здесь для охраны врат гиперперехода и для  пресечения Гайджинов, проникающих сюда из Приграничных Миров. Я рад, что они здесь, но, к сожалению, это не всё. Если они справятся со своей работой, то у нас не будет возможности привлечения сюда иностранцев.</TEXT>" +
                "<PARA/><POP/></RDL>\r\n\r\n\r\n";
            var c5_old = "\r\n\tЛинкор \\iМиоко\\I был дислоцирован здесь для охраны врат гиперперехода и для  пресечения Гайджинов, проникающих \\\r\n\t" +
                "сюда из Приграничных Миров. Я рад, что они здесь, но, к сожалению, это не всё. Если они справятся со своей \\\r\n\t" +
                "работой, то у нас не будет возможности привлечения сюда иностранцев.\r\n";

            var c5 = "\r\n\tЛинкор \\iМиоко\\I был дислоцирован здесь для охраны врат гиперперехода и для  пресечения Гайджинов, проникающих сюда из \\\r\n\t" +
                "Приграничных Миров. Я рад, что они здесь, но, к сожалению, это не всё. Если они справятся со своей работой, то \\\r\n\t" +
                "у нас не будет возможности привлечения сюда иностранцев.\r\n";

            var r5 = parser.ConvertXMLtoFRC(i5, "2373", "MiscText.dll");
            // Test TRA TEXT PARA data\mask\def
            Assert.IsTrue(r5 == c5);

            var i6 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
                "<RDL>" +
                "<PUSH/>" +
                "<TRA data=\"1\" mask=\"1\" def=\"-2\"/>" +
                "<JUST loc=\"center\"/>" +
                "<TEXT>Ракетная пусковая установка J63 \"Копье\"</TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TRA data=\"0\" mask=\"1\" def=\"-1\"/>" +
                "<JUST loc=\"left\"/>" +
                "<TEXT>Главным достоинством этой ракетной пусковой установки является ее надежность: доставка взрывчатки точно по адресу. Это, вкупе с ее относительно недорогой стоимостью делает установку хорошим дополнением к любому арсеналу. </TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TEXT>*Амуниция: Ракеты \"Копье\"</TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<POP/>" +
                "</RDL>";
            var c6_Old = "\r\n\t" +
                "\\b\\mРакетная пусковая установка J63 \"Копье\"\r\n\t\\ \\n\\\r\n\t" +
                "\\B\\lГлавным достоинством этой ракетной пусковой установки является ее надежность: доставка взрывчатки точно по \\\r\n\t" +
                " адресу. Это, вкупе с ее относительно недорогой стоимостью делает установку хорошим дополнением к любому арсеналу. \\n\\\r\n\t" +
                "\\ \\n\\\r\n\t" +
                "*Амуниция: Ракеты \"Копье\"\r\n\t" +
                "\\ \\n\\.\r\n";

            var c6 = "\r\n\t" +
                "\\b\\mРакетная пусковая установка J63 \"Копье\"\r\n\t\\ \\n\\\r\n\t" +
                "\\B\\lГлавным достоинством этой ракетной пусковой установки является ее надежность: доставка взрывчатки точно по \\\r\n\t" +
                "адресу. Это, вкупе с ее относительно недорогой стоимостью делает установку хорошим дополнением к любому \\\r\n\t" +
                "арсеналу. \\n\\\r\n\t" +
                "\\ \\n\\\r\n\t" +
                "*Амуниция: Ракеты \"Копье\"\r\n\t" +
                "\\ \\n\\.\r\n";

            var r6 = parser.ConvertXMLtoFRC(i6, "2002", "EqupResources.dll");
            // Test TRA TEXT PARA data\mask\def loc
            Assert.IsTrue(r6 == c6);


            var i7 = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>" +
                "<RDL>" +
                "<PUSH/>" +
                "<TRA data=\"1\" mask=\"1\" def=\"-2\"/>" +
                "<TEXT>ЛИНЕЙНЫЙ КОРАБЛЬ </TEXT>" +
                "<TRA data=\"3\" mask=\"3\" def=\"-4\"/>" +
                "<TEXT>МАТСУМОТО</TEXT>" +
                "<PARA/><TRA data=\"0\" mask=\"3\" def=\"-1\"/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TEXT>КЛАСС: Генжи</TEXT>" +
                "<PARA/>" +
                "<TEXT>ЭКИПАЖ: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-29\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<TRA data=\"96\" mask=\"-29\" def=\"-1\"/>" +
                "<TEXT>ВООРУЖЕНИЕ: </TEXT>" +
                "<TRA data=\"65280\" mask=\"-29\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<POP/>" +
                "</RDL>\r\n\r\n\r\n";
            var c7 = "\r\n\t" +
                "\\bЛИНЕЙНЫЙ КОРАБЛЬ " +
                "\\iМАТСУМОТО\r\n\t" +
                "\\B\\I\\ \\n\\\r\n\t" +
                "КЛАСС: Генжи\r\n\t" +
                "ЭКИПАЖ: \\crЗАСЕКРЕЧЕНО\r\n\t" +
                "\\CВООРУЖЕНИЕ: \\crЗАСЕКРЕЧЕНО\r\n";

            var r7 = parser.ConvertXMLtoFRC(i7, "99", "InfoCards.dll");
            // Test TRA TEXT PARA data(color value)\mask\def loc
            Assert.IsTrue(r7 == c7);
        }

        /*
"100": "<?xml version=\"1.0\" encoding=\"UTF-16\"?>
        <RDL><PUSH/>
        <TEXT>Линкор</TEXT>
        <TRA data=\"2\" mask=\"2\" def=\"-3\"/>
        <TEXT> Матсумото </TEXT>
        <TRA data=\"0\" mask=\"2\" def=\"-1\"/>
        <TEXT>является одним из самых новых и передовых кораблей ВС Кусари. Корабль дислоцирован в системе Хоккайдо для обеспечения безопасности экипажей, принимающих участие в строительстве врат гиперперехода, а также с целью пресечения действий террористических групп, которые, по слухам, используют систему как убежище.</TEXT>
        <PARA/><POP/></RDL>\r\n\r\n\r\n\n",
         
         */

        [TestMethod]
        public void FormatText_Test()
        {
            var input = "Фараоны - повелители Египта, что стремятся к власти над всем миром. Они уничтожают Враждебную Расу, что угрожает их власти, и Ангелов Тьмы, что ищут равновесие в хаосе. Фараоны не ищут мира, они ищут власти, и они не остановятся ни перед чем, чтобы добиться своей цели. В их руках - могущественная магия, что может уничтожить всех, кто стоит на их пути. Их армия - это армия мертвых, что восстала из гробниц, чтобы служить своим повелителям. Фараоны - это угроза всему живому, и только совместными усилиями всех сил можно остановить их вторжение.";
            var correct = "Фараоны - повелители Египта, что стремятся к власти над всем миром. Они уничтожают Враждебную Расу, что \\\r\n\tугрожает их власти, и Ангелов Тьмы, что ищут равновесие в хаосе. Фараоны не ищут мира, они ищут власти, и они \\\r\n\tне остановятся ни перед чем, чтобы добиться своей цели. В их руках - могущественная магия, что может \\\r\n\tуничтожить всех, кто стоит на их пути. Их армия - это армия мертвых, что восстала из гробниц, чтобы служить \\\r\n\tсвоим повелителям. Фараоны - это угроза всему живому, и только совместными усилиями всех сил можно остановить \\\r\n\tих вторжение.";

            var parser = new RdlParser();
            var res = parser.FormatText(input);

            Assert.IsTrue(correct == res);
        }

        [TestMethod]
        public void ConvertDataToColor_Test()
        {
            var input = "0x2093FF08";
            var correct = "FF3902";

            var parser = new RdlParser();
            var res = parser.ConvertDataToColor(input);

            Assert.IsTrue(correct == res);
        }

        [TestMethod]
        public void SwapPairsInHex_Test()
        {
            var input = "FF9320";
            var correct = "FF3902";

            var parser = new RdlParser();
            var res = parser.SwapPairsInHex(input);

            Assert.IsTrue(correct == res);
        }

        [TestMethod]
        public void GetStyleTags_Test()
        {
            /*
              <TRA data=\"2\" mask=\"2\" def=\"-3\"/> +
              <TEXT>Миоко</TEXT>" +
              <TRA data=\"0\" mask=\"2\" def=\"-1\"/> +
              <TEXT> был дислоцирован здесь для охраны врат гиперперехода и для  пресечения Гайджинов, проникающих сюда из Приграничных Миров. Я рад, что они здесь, но, к сожалению, это не всё. Если они справятся со своей работой, то у нас не будет возможности привлечения сюда иностранцев.</TEXT>
             */

            /*
             \r\n\tЛинкор \\iМиоко\\I был дислоцирован здесь для охраны врат гиперперехода и для  пресечения Гайджинов, проникающих \\\r\n\t
             */

            var parser = new RdlParser();

            //// <TRA data=\"2\" mask=\"2\" def=\"-3\"/>
            var prevDataStr1 = "0";
            var prevMaskStr1 = "0";
            var prevDefStr1 = "0";
            var dataStr1 = "2";
            var maskStr1 = "2";
            var defStr1 = "-3";

            var c1 = "\\i";
            var res1 = parser.GetStyleTags(prevDataStr1, prevMaskStr1, prevDefStr1,
                dataStr1, maskStr1, defStr1);
            Assert.IsTrue(res1 == c1);

            // <TRA data=\"0\" mask=\"2\" def=\"-1\"/>
            var prevDataStr2 = dataStr1;
            var prevMaskStr2 = maskStr1;
            var prevDefStr2 = defStr1;
            var dataStr2 = "0";
            var maskStr2 = "2";
            var defStr2 = "-1";

            var c2 = "\\I";
            var res2 = parser.GetStyleTags(prevDataStr2, prevMaskStr2, prevDefStr2,
                dataStr2, maskStr2, defStr2);

            Assert.IsTrue(res2 == c2);

            /*
               "<TRA data=\"1\" mask=\"1\" def=\"-2\"/>" +
                "<TEXT>ЛИНЕЙНЫЙ КОРАБЛЬ </TEXT>" +
                "<TRA data=\"3\" mask=\"3\" def=\"-4\"/>" +
                "<TEXT>МАТСУМОТО</TEXT>" +
             */
            /*
             	\bЛИНЕЙНЫЙ КОРАБЛЬ \iМАТСУМОТО
             */


            // <TRA data=\"2\" mask=\"2\" def=\"-3\"/>
            var prevDataStr3 = "0";
            var prevMaskStr3 = "0";
            var prevDefStr3 = "0";
            var dataStr3 = "1";
            var maskStr3 = "1";
            var defStr3 = "-2";

            var c3 = "\\b";
            var res3 = parser.GetStyleTags(prevDataStr3, prevMaskStr3, prevDefStr3, dataStr3, maskStr3, defStr3);
            Assert.IsTrue(res3 == c3);

            // <TRA data=\"0\" mask=\"2\" def=\"-1\"/>
            var prevDataStr4 = dataStr3;
            var prevMaskStr4 = maskStr3;
            var prevDefStr4 = defStr3;
            var dataStr4 = "3";
            var maskStr4 = "3";
            var defStr4 = "-4";

            var c4 = "\\i";
            var res4 = parser.GetStyleTags(prevDataStr4, prevMaskStr4, prevDefStr4, dataStr4, maskStr4, defStr4);

            Assert.IsTrue(res4 == c4);

            /*
                "<TRA data=\"1\" mask=\"-2\" def=\"-2\"/>" +
                "<TEXT>СОЮЗНИКИ:  </TEXT>" +
                "<TRA data='0x2093FF08' mask=\"-1\"/>" +
                "<TEXT>(их пустят практически везде)</TEXT>" +
             */
            /*
             "\r\n\tСОЮЗНИКИ:  \\cFF3902(их пустят практически везде)\r\n\t\r\n\t" +
             */


            // "<TRA data=\"1\" mask=\"-2\" def=\"-2\"/>"
            var prevDataStr5 = "0";
            var prevMaskStr5 = "0";
            var prevDefStr5 = "0";
            var dataStr5 = "1";
            var maskStr5 = "-2";
            var defStr5 = "-2";

            var c5 = "";
            var res5 = parser.GetStyleTags(prevDataStr5, prevMaskStr5, prevDefStr5,
                dataStr5, maskStr5, defStr5);
            Assert.IsTrue(res5 == c5);

            // "<TRA data='0x2093FF08' mask=\"-1\"/>"
            var prevDataStr6 = dataStr5;
            var prevMaskStr6 = maskStr5;
            var prevDefStr6 = defStr5;
            var dataStr6 = "0x2093FF08";
            var maskStr6 = "-1";
            var defStr6 = "0";

            var c6 = "\\cFF3902";
            var res6 = parser.GetStyleTags(prevDataStr6, prevMaskStr6, prevDefStr6,  dataStr6, maskStr6, defStr6);

            Assert.IsTrue(res6 == c6);

            /*
                "<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>" +
                "<TEXT>ЗАСЕКРЕЧЕНО</TEXT>" +
                "<PARA/>" +
                "<TEXT> </TEXT>" +
                "<PARA/>" +
                "<TRA data=\"65281\" mask=\"-31\" def=\"30\"/>" +
                "<TEXT>&gt;&gt;&gt;СОВЕРШЕННО СЕКРЕТНО&lt;&lt;&lt;</TEXT>" +
             */

            /*
               "\\CНАСЕЛЕНИЕ: \\crЗАСЕКРЕЧЕНО\r\n\t \r\n\t" +
                "\\b>>>СОВЕРШЕННО СЕКРЕТНО<<<\r\n\t\\B\\C \r\n\t";
             */

            //<TRA data=\"65280\" mask=\"-32\" def=\"31\"/>
            var prevDataStr7 = "0";
            var prevMaskStr7 = "0";
            var prevDefStr7 = "0";
            var dataStr7 = "65280";
            var maskStr7 = "-32";
            var defStr7 = "31";

            var c7 = "\\cr";
            var res7 = parser.GetStyleTags(prevDataStr7, prevMaskStr7, prevDefStr7,
                dataStr7, maskStr7, defStr7);
            Assert.IsTrue(res7 == c7);

            //<TRA data=\"65281\" mask=\"-31\" def=\"30\"/>
            var prevDataStr8 = dataStr7;
            var prevMaskStr8 = maskStr7;
            var prevDefStr8 = defStr7;
            var dataStr8 = "65281";
            var maskStr8 = "-31";
            var defStr8 = "30";

            var c8 = "\\b";
            var res8 = parser.GetStyleTags(prevDataStr8, prevMaskStr8, prevDefStr8,
                dataStr8, maskStr8, defStr8);
            Assert.IsTrue(res8 == c8);
        }
    }
}
