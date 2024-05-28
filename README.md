<p align="center">
  <img src="https://cdn-icons-png.flaticon.com/512/6295/6295417.png" width="100" />
</p>
<p align="center">
    <h1 align="center">ARA-AAMAZON</h1>
</p>
<p align="center">
    <em>Haz que tu cartera pese más con el seguimiento aracnoide de mi script</em>
</p>
<p align="center">
<img src="https://img.shields.io/github/last-commit/RodrigoSantosNegro/Ara-aAmazon?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
<img src="https://img.shields.io/github/languages/top/RodrigoSantosNegro/Ara-aAmazon?style=flat&color=0080ff" alt="repo-top-language">
<img src="https://img.shields.io/github/languages/count/RodrigoSantosNegro/Ara-aAmazon?style=flat&color=0080ff" alt="repo-language-count">
<p>

<hr>

## 🔗 Enlaces Rápidos

> - [📍 Descripción General](#-descripción-general)
> - [📦 Funcionalidades](#-funcionalidades)
> - [📂 Estructura del Repositorio](#-estructura-del-repositorio)
> - [🧩 Módulos](#-módulos)
> - [🚀 Comenzando](#-comenzando)
>   - [⚙️ Instalación](#️-instalación)
>   - [🤖 Ejecutando Ara-aAmazon](#-ejecutando-ara-aamazon)
> - [🛠 Hoja de Ruta del Proyecto](#-hoja-de-ruta-del-proyecto)
> - [📄 Licencia](#-licencia)
> - [👏 Agradecimientos](#-agradecimientos)

---

## 📍 Descripción General

<p>
<em>Script que almacena en PostgreSQL el nombre, precio, url, categoria y fecha de lectura (por ahora) de un producto de Amazön.</em>
</p>

---

## 📂 Repository Structure

```sh
└── Ara-aAmazon/
    └── AranhaAmazon
        ├── .gitignore
        ├── AranhaAmazon
        │   ├── AranhaAmazon.csproj
        │   ├── Program.cs
        │   ├── Utils
        │   │   ├── CategoriasRevisadasHoy.cs
        │   │   └── Producto.cs
        │   ├── obj
        │   │   ├── AranhaAmazon.csproj.nuget.dgspec.json
        │   │   ├── AranhaAmazon.csproj.nuget.g.props
        │   │   ├── AranhaAmazon.csproj.nuget.g.targets
        │   │   ├── Debug
        │   │   │   ├── AranhaAmazon.1.0.0.nuspec
        │   │   │   └── net8.0
        │   │   ├── project.assets.json
        │   │   └── project.nuget.cache
        │   └── sql
        │       └── Postgresql.cs
        └── AranhaAmazon.sln
```

---

## 🧩 Módulos

<details closed><summary>AranhaAmazon.AranhaAmazon</summary>

| File                                                                                                                               | Summary                                                                   |
| ---                                                                                                                                | ---                                                                       |
| [Program.cs](https://github.com/RodrigoSantosNegro/Ara-aAmazon/blob/master/AranhaAmazon/AranhaAmazon/Program.cs)                   | Clase principal donde se encuentra el script con las búsquedas a Amazon   |

</details>

<details closed><summary>AranhaAmazon.AranhaAmazon.sql</summary>

| File                                                                                                                       | Summary                                                                 |
| ---                                                                                                                        | ---                                                                     |
| [Postgresql.cs](https://github.com/RodrigoSantosNegro/Ara-aAmazon/blob/master/AranhaAmazon/AranhaAmazon/sql/Postgresql.cs) | Se almacenan las funciones con las consultas SQL a Postgre              |

</details>

<details closed><summary>AranhaAmazon.AranhaAmazon.Utils</summary>

| File                                                                                                                                                 | Summary                                                                               |
| ---                                                                                                                                                  | ---                                                                                   |
| [Producto.cs](https://github.com/RodrigoSantosNegro/Ara-aAmazon/blob/master/AranhaAmazon/AranhaAmazon/Utils/Producto.cs)                             | Atributos que nos interesan de cada producto                                          |
| [CategoriasRevisadasHoy.cs](https://github.com/RodrigoSantosNegro/Ara-aAmazon/blob/master/AranhaAmazon/AranhaAmazon/Utils/CategoriasRevisadasHoy.cs) | En desarrollo, no tiene utilidad ahora mismo                                          |

</details>

---

## 🚀 Para poder empezar

***Requisitos***

Asegúrate de que tienes instalado lo siguiente:

* **Visual Studio 2022**
* **CSharp**
* **PostgreSQL**
* **Chrome**

### ⚙️ Instalación

1. Clona el repositorio de Ara-aAmazon:

```sh
git clone https://github.com/RodrigoSantosNegro/Ara-aAmazon
```

2. Cambia al directorio del proyecto:

```sh
cd Ara-aAmazon
```

### 🤖 Ejecutar Ara-aAmazon

Usa el botón de ejecutar en VS Code o utiliza el siguiente comando Ara-aAmazon:

```sh
dotnet run
```

---

## 📄 License

Este proyecto no tiene licencia jiji

---

## 👏 Créditos

- Don Gepeto, más conocido como ChatGPT.
- Mentores de Aracnosoft.
- Mi intelecto.

[**Return**](#-quick-links)

---
