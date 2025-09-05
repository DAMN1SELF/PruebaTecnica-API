# 🛒 Carrito de Compras API

API REST para la gestión de carritos de compras, desarrollada con **.NET 8 (ASP.NET Core Web API)**, bajo principios de **DDD (Domain Driven Design)**, **Clean Architecture**, **SOLID** e **Inyección de Dependencias**.

---

## 🚀 Funcionalidades

1. **Añadir producto al carrito**  
   `POST /api/carrito/{carritoId}/producto`  
   - Agrega un nuevo producto al carrito.  
   - Valida reglas de negocio: grupos obligatorios, cantidades máximas, atributos activos/inactivos, etc.  

2. **Actualizar producto del carrito**  
   `PUT /api/carrito/{carritoId}/producto/{productoId}`  
   - Actualiza la configuración de un producto existente en el carrito.  
   - Se aplican las mismas reglas de validación que en la creación.  

3. **Aumentar o disminuir la cantidad de un producto**  
   `PATCH /api/carrito/{carritoId}/producto/{productoId}`  
   - Permite incrementar o reducir la cantidad de un producto ya agregado.  

4. **Eliminar producto del carrito**  
   `DELETE /api/carrito/{carritoId}/producto/{productoId}`  
   - Elimina un producto específico del carrito.  

5. **Obtener contenido del carrito**  
   `GET /api/carrito/{carritoId}/producto`  
   - Devuelve todos los productos actualmente en el carrito.  
   - Incluye detalles, subtotales y total calculado.  
---

## ⚙️ Tecnologías

- **.NET 8** – ASP.NET Core Web API  
- **FluentValidation** – Validaciones declarativas  
- **Swagger / OpenAPI** – Documentación interactiva  
- **InMemory Repository** – Persistencia temporal 

---

## 📂 Estructura del Proyecto

```plaintext
Carrito_Compras/
│
├── Application/
│   ├── UseCases/               # Casos de uso (Add, Update, Patch, Remove, Get)
│   ├── Validation/             # Reglas de validación con FluentValidation
│   ├── Ports/                  # Interfaces de comandos y queries
│   └── Mappers/                # Conversión de entidades a DTOs
│
├── Domain/
│   ├── Carrito/                # Entidad Carrito y Elementos
│   ├── Producto/               # Definición de producto y atributos
│   └── Common/                 # Utilidades compartidas
│
├── Infrastructure/
│   └── Persistence/            # Repositorio en memoria
│
├── Dtos/
│   ├── Requests/               # Modelos de entrada
│   └── Responses/              # Modelos de salida
│
├── Config/
│   └── product-config.json     # Configuración de reglas del producto
│
├── Controllers/
│   └── CartsController.cs      # Controlador principal de la API
│
├── Program.cs                  # Configuración y punto de entrada
└── README.md                   # Este archivo
```
---

## ▶️ Ejecución
- [SDK .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado en tu máquina.
- Editor recomendado: **Visual Studio 2022** .
  
1. Clona el repositorio:

```bash
git clone https://github.com/DAMN1SELF/PruebaTecnica-API.git
cd CarritoCompras
```
2. Compila el proyecto
```bash
dotnet restore
dotnet run --project Carrito_Compras
```
3. Accede a Swagger en tu navegador:
http://localhost:5191/swagger


