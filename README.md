
# 🌾 ExportModule

**ExportModule** es un módulo backend desarrollado en C# para gestionar la exportación de productos agrícolas. Evalúa la aptitud de exportación de cultivos en base a su información y presencia de plagas, integrando una IA externa para dicha evaluación.

---

## 📦 Características

- Gestión de **cultivos** y sus respectivas **plagas**.
- Evaluación de exportación mediante un servicio de **IA externa**.
- Registro de resultados en la entidad **DatosAExportar**.
- API REST documentada con **Swagger**.
- Conexión a base de datos PostgreSQL usando **Entity Framework Core**.

---

## 🛠️ Tecnologías

- .NET 9
- ASP.NET Core
- Entity Framework Core (PostgreSQL)
- Swagger / Swashbuckle
- HttpClient
- Moq y xUnit (para testing)

---

## 🚀 Ejecución del proyecto

### Prerrequisitos

- .NET SDK 9 o superior
- PostgreSQL
- Visual Studio 2022 o VS Code

### Configuración

1. Crea la base de datos en PostgreSQL.
2. Actualiza el archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=exportdb;Username=postgres;Password=tu_password"
  },
  "IA": {
    "EndpointEvaluacion": "http://localhost:8000/evaluar-cultivo"
  }
}
```

3. Ejecuta las migraciones:

```bash
dotnet ef database update
```

### Ejecutar la aplicación

```bash
dotnet run --project ExportModule
```

Abre Swagger para probar la API:  
🔗 http://localhost:5000/swagger

---

## 🧪 Pruebas

Ejecutar tests:

```bash
dotnet test
```

Incluye pruebas unitarias para lógica de evaluación y persistencia de datos exportables.

---

## 📁 Estructura del proyecto

```
ExportModule/
│
├── Controllers/
│   ├── ExportacionController.cs
│   └── EvaluacionController.cs
│
├── Services/
│   └── Implementaciones/
│       ├── CultivoService.cs
│       ├── DatosAExportarService.cs
│       └── AgenteEvaluadorService.cs
│
├── Data/
│   └── Context/
│       └── AppDbContext.cs
│
├── Models/
│   └── Cultivo.cs, Plaga.cs, DatosAExportar.cs
│
├── appsettings.json
└── Program.cs
```

---

## 📡 Endpoints de la API

### 📍 Evaluación de Cultivo por IA

`POST /api/evaluacion/{cultivoId}`  
Evalúa si un cultivo es apto para exportación según plagas.

#### Ejemplo de respuesta:
```json
{
  "esExportable": true,
  "motivo": "Sin plagas detectadas"
}
```

---

### 📍 Exportaciones

#### `GET /api/exportacion`
Lista todos los datos exportados.

#### `GET /api/exportacion/{id}`
Obtiene una exportación específica.


##### Cuerpo JSON de ejemplo:
```json
{
  "cultivoId": 1,
  "tipoDato": "EvaluaciónIA",
  "formato": "json",
  "contenido": "{ "Apto": true, "Motivo": "Sin plagas" }"
}
```


