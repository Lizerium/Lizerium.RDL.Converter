<h1 align="center">Lizerium.RDL.Converter</h1>

<p align="center">
    Современная реализация <b>frc.exe</b> для Freelancer (2003)<br>
    Конвертация <b>RDL (XML)</b> → <b>FRC (runtime текст)</b>
</p>

<p align="center">
    <img src="https://shields.dvurechensky.pro/badge/.NET-8.0-blue?logo=dotnet">
    <img src="https://shields.dvurechensky.pro/badge/Freelancer-2003-green">
    <img src="https://shields.dvurechensky.pro/badge/Status-Active-brightgreen">
</p>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Язык: </strong>
  
  <span style="color: #F5F752; margin: 0 10px;">
    ✅ 🇷🇺 Русский (текущий)
  </span>
  | 
  <a href="./README.md" style="color: #0891b2; margin: 0 10px;">
    🇺🇸 English
  </a>
</div>

---

> [!NOTE]
> Этот проект является частью экосистемы **Lizerium** и относится к направлению:
>
> - [`Lizerium.Tools.Structs`](https://github.com/Lizerium/Lizerium.Tools.Structs)
>
> Если вы ищете связанные инженерные и вспомогательные инструменты, начните оттуда.

## О проекте

`Lizerium.RDL.Converter` — это собственная реализация утилиты
**frc.exe (Freelancer Resource Compiler)**, переписанная с нуля на C#.

Оригинальный инструмент:

- frc.exe
- Автор: Jason Hook (Adoxa)
- Инструменты: https://adoxa.altervista.org/freelancer/tools.html#frc
- GitHub: https://github.com/adoxa

---

## Credits

> [!IMPORTANT]
> Данный проект основан на анализе оригинальной реализации `frc.exe`,
> написанной на языке C автором Jason Hook (Adoxa).
>
> Алгоритмы были переосмыслены и реализованы на C# с исправлением ограничений оригинала.

---

## Ограничения оригинального frc.exe

> [!CAUTION]
> При использовании оригинального инструмента могут наблюдаться следующие проблемы:

- Потери данных при распаковке
- Артефакты в тексте
- Некорректная работа с Unicode
- Ошибки при работе с кириллицей
- Некорректная повторная упаковка

---

## Что делает библиотека

Конвертация выполняется следующим образом:

```
RDL (XML разметка)
        ↓
FRC (строковый runtime формат)
```

Поддерживаемые элементы:

- `<TRA>` — стили (bold / italic / underline / color / font)
- `<TEXT>` — текст
- `<PARA>` — переносы строк
- `<JUST>` — выравнивание

---

## Особенности реализации

> [!TIP]
> Библиотека не просто конвертирует XML, а воспроизводит поведение runtime-формата Freelancer.

- Восстановленная логика `data / mask / def`
- Корректная обработка цветов (bitwise + nibble swap)
- Сохранение форматирования текста
- Устойчивость к повреждённому XML
- Логирование ошибок парсинга
- Присутствуют тесты каждой функции в исходном коде

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

## Назначение

Библиотека используется в проекте:

[`Freelancer Lizerium`](https://lizup.ru/)

Все текстовые ресурсы и информационные карты в проекте
генерируются с использованием данного конвертера.

---

## Примечание

> [!TIP]
> Проект может использоваться как standalone-конвертер,
> либо как часть более сложной системы обработки ресурсов Freelancer.
