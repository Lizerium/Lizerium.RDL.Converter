# Lizerium.RDL.Converter [EN]

C# implementation of `frc.exe` (Freelancer Resource Compiler).
Converts RDL (XML) into FRC runtime text format.

---

## Installation

```
dotnet add package Lizerium.RDL.Converter
```

---

## Usage

```csharp
using LizeriumRDL;

var parser = new RdlParser();

string xml = File.ReadAllText("input.xml");
string frc = parser.ConvertXMLtoFRC(xml, "id", "file");

File.WriteAllText("output.frc", frc);
```

---

## API

### IRdlParser

#### ConvertXMLtoFRC

Converts RDL XML into FRC string.

```csharp
string ConvertXMLtoFRC(string xml, string id, string nameFile)
```

#### GetStyleTags

Calculates style flags based on data/mask/def values.

```csharp
string GetStyleTags(
    string prevDataStr,
    string prevMaskStr,
    string prevDefStr,
    string dataStr,
    string maskStr,
    string defStr,
    string id = "",
    string xml = "")
```

#### ConvertDataToColor

Converts internal color format (0xFFFFFFFF) into readable color value.

```csharp
string ConvertDataToColor(string hexString)
```

#### FormatText

Formats text with line breaks and indentation.

```csharp
string FormatText(string input, int maxLineLength = 111)
```

---

## Notes

- Designed for Freelancer (2003) data processing
- Reimplements behavior of original `frc.exe`
- Handles formatting, styles and color conversion

---

# Lizerium.RDL.Converter [RU]

Реализация `frc.exe` (Freelancer Resource Compiler) на C#.
Преобразует RDL (XML) в формат FRC (runtime текст).

---

## Установка

```
dotnet add package Lizerium.RDL.Converter
```

---

## Использование

```csharp
using LizeriumRDL;

var parser = new RdlParser();

string xml = File.ReadAllText("input.xml");
string frc = parser.ConvertXMLtoFRC(xml, "id", "file");

File.WriteAllText("output.frc", frc);
```

---

## API

### IRdlParser

#### ConvertXMLtoFRC

Конвертирует RDL XML в строку FRC.

```csharp
string ConvertXMLtoFRC(string xml, string id, string nameFile)
```

#### GetStyleTags

Рассчитывает стили на основе значений data/mask/def.

```csharp
string GetStyleTags(
    string prevDataStr,
    string prevMaskStr,
    string prevDefStr,
    string dataStr,
    string maskStr,
    string defStr,
    string id = "",
    string xml = "")
```

#### ConvertDataToColor

Преобразует внутренний формат цвета (0xFFFFFFFF) в читаемый цвет.

```csharp
string ConvertDataToColor(string hexString)
```

#### FormatText

Форматирует текст, добавляя переносы строк и отступы.

```csharp
string FormatText(string input, int maxLineLength = 111)
```

---

## Примечания

- Предназначено для обработки данных Freelancer (2003)
- Повторяет поведение оригинального `frc.exe`
- Обрабатывает форматирование, стили и преобразование цветов
