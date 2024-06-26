<p align="center">
  <img src="https://cdn-icons-png.flaticon.com/512/6295/6295417.png" width="100" />
</p>
<p align="center">
    <h1 align="center">ARA-AAMAZON</h1>
</p>
<p align="center">
    <em>:construction: Proyecto en construcción :construction:</em>
</p>
<p align="center">
    <em>
      Ara-aAmazon es una herramienta de web scraping diseñada para extraer información de productos de Amazon. Utiliza CSharp y varias bibliotecas para automatizar la recopilación de datos de productos, como precios, nombres y descripciones. Esta herramienta es útil para monitorear precios, análisis de competencia y recolección de datos para investigación de mercado.
    </em>
</p>
<p align="center">
<img src="https://img.shields.io/github/last-commit/RodrigoSantosNegro/Ara-aAmazon?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
<img src="https://img.shields.io/github/languages/top/RodrigoSantosNegro/Ara-aAmazon?style=flat&color=0080ff" alt="repo-top-language">
<img src="https://img.shields.io/github/languages/count/RodrigoSantosNegro/Ara-aAmazon?style=flat&color=0080ff" alt="repo-language-count">
<p>

<hr>

## 🔗 Enlaces Rápidos

> - [📂 Estructura del Repositorio](#-estructura-del-repositorio)
> - [🧩 Módulos](#-módulos)
> - [🚀 Para poder empezar](#-comenzando)
>   - [⚙️ Instalación](#️-instalación)
>   - [🤖 Ejecutando Ara-aAmazon](#-ejecutando-ara-aamazon)
> - [🛠 Hoja de Ruta del Proyecto](#-hoja-de-ruta-del-proyecto)
> - [📄 Licencia](#-licencia)
> - [👏 Agradecimientos](#-agradecimientos)

---

## 📂 Estructura del Repositorio

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

Este script funciona sólo para sistemas operativos Windows.

Asegúrate de que tienes instalado lo siguiente:

* **Visual Studio 2022**
* **CSharp**
* **PostgreSQL**
* **Chrome**

### ⚙️ Instalación

1. Instala PostreSQL 13 y ejecútalo cuando finalice.
2. Contáctame para pedir un backup o crea una base de datos que tenga las siguientes tablas:

```sh
**Columna**            ---  **Atributos**

producto               ---  id (PK), nombre, precio, url, fecha_lectura, oferta, categoria, id_categoria (FK)
categoria              ---  id (PK), nombre, activa, frecuencia
categorias_leidas_hoy  ---  id_categoria (FK), fecha_inicio, fecha_fin, estimado, real
```

2. Clona el repositorio de Ara-aAmazon:

```sh
git clone https://github.com/RodrigoSantosNegro/Ara-aAmazon
```

3. Cambia al directorio del proyecto y abre el proyecto:

```sh
cd Ara-aAmazon
```

### 🤖 Ejecutando Ara-aAmazon

Usa el botón de ejecutar en VS Code o utiliza el siguiente comando:

```sh
dotnet run
```

***¡Ya estás realizando tu seguimiento!***
---

## 📄 Licencia

Este proyecto no tiene licencia

---

## 👏 Agradecimientos

- Don Gepeto, más conocido como ChatGPT.
- Mentores de Aracnosoft.
- Mi intelecto.

---
