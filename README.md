<h1 align="center">Lizerium.RDL.Converter</h1>

<p align="center">
    Modern implementation of <b>frc.exe</b> for Freelancer (2003)<br>
    Conversion of <b>RDL (XML)</b> → <b>FRC (runtime text)</b>
</p>

<p align="center">
    <img src="https://shields.dvurechensky.pro/badge/.NET-8.0-blue?logo=dotnet">
    <img src="https://shields.dvurechensky.pro/badge/Freelancer-2003-green">
    <img src="https://shields.dvurechensky.pro/badge/Status-Active-brightgreen">
</p>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>
  
  <a href="./README.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  | 
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

---

> [!NOTE]
> This project is part of the **Lizerium** ecosystem and belongs to the following project:
>
> - [`Lizerium.Tools.Structs`](https://github.com/Lizerium/Lizerium.Tools.Structs)
>
> If you're looking for related engineering and support tools, start there.

## About

`Lizerium.RDL.Converter` is a custom implementation of
**frc.exe (Freelancer Resource Compiler)**, rewritten from scratch in C#.

Original tool:

- frc.exe
- Author: Jason Hook (Adoxa)
- Tools: https://adoxa.altervista.org/freelancer/tools.html#frc
- GitHub: https://github.com/adoxa

---

## Credits

> [!IMPORTANT]
> This project is based on the analysis of the original `frc.exe` implementation,
> written in C by Jason Hook (Adoxa).
>
> The algorithms were rethought and reimplemented in C#, addressing the limitations of the original tool.

---

## Limitations of the original frc.exe

> [!CAUTION]
> The original tool may exhibit the following issues:

- Data loss during extraction
- Text artifacts
- Improper Unicode handling
- Issues with Cyrillic encoding
- Incorrect repacking behavior

---

## What the library does

The conversion process:

```
RDL (XML markup)
        ↓
FRC (string-based runtime format)
```

Supported elements:

- `<TRA>` — styles (bold / italic / underline / color / font)
- `<TEXT>` — text
- `<PARA>` — line breaks
- `<JUST>` — alignment

---

## Implementation details

> [!TIP]
> The library does not simply convert XML — it reproduces the runtime behavior of Freelancer’s text system.

- Reconstructed `data / mask / def` logic
- Correct color handling (bitwise + nibble swap)
- Preserves text formatting
- Fault-tolerant XML parsing
- Error logging
- Unit tests for core functions included

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

## Purpose

The library is used in the project:

[`Freelancer Lizerium`](https://lizup.ru/)

All text resources and information cards in the project
are generated using this converter.

---

## Notes

> [!TIP]
> The project can be used as a standalone converter
> or as part of a larger Freelancer resource processing pipeline.
