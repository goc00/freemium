# ENTIDAD DIMENSION

## Obtener Registros

### Descripción

Permite obtener todos los registros de la entidad "dimensions", dado un producto específico

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/{idProduct}     |
| Protocolo           | GET                                               |

### Request

Sólo vía GET

### Response

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "totalItems": 2,
        "items": [
            {
                "idDimension": 31,
                "nameDimension": "Cantidad Canjes",
                "tagDimension": "CantidadCanjesCuponesSuscripcion",
                "idDimensionCategory": 10,
                "tagDimensionCategory": "CuponesSuscripcion",
                "idDimensionType": 1,
                "nameDimensionType": "Numeric",
                "idProduct": 7575,
                "value": null,
                "switchValue": null
            },
            {
                "idDimension": 36,
                "nameDimension": "TC On Demand 2Hoyts2DM",
                "tagDimension": "OnDemand2Hoyts2DM",
                "idDimensionCategory": 16,
                "tagDimensionCategory": "OnDemand",
                "idDimensionType": 1,
                "nameDimensionType": "Numeric",
                "idProduct": 7575,
                "value": null,
                "switchValue": null
            }
        ]
    }
}
```

<br>

## Crear Registro

### Descripción

Permite registrar un nuevo registro en la entidad "dimensions"

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/{idProduct}     |
| Protocolo           | POST                                               |

### Request

**Descripción**

| Parámetro             | Tipo    | Obligatorio | Descripción |
|-----------------------|---------|-------------|-------------|
| description           | String  | Sí          | Descripción de la dimensión |
| idDimensionType       | Int     | Sí          | Id del tipo de dimensión |
| idDimensionCategory   | Int     | Sí          | Id de la categoría de dimensión |
| active                | Int     | Sí          | 0 = Inactivo; 1 = Activo |
| value                 | Int     | No          | Valor de la dimensión (permite null) |
| switchValue           | Int     | No          | Valor del switch value (permite null) |
| tagName               | String  | Sí          | Tag de la dimensión, debe ser camelCase |

**Ejemplos de Envío**

```json
{
	"description": "Mi Dimensión",
	"idDimensionType": 1,
	"idDimensionCategory": 3,
	"active": 1,
	"value": 10,
	"tagName": "miTagDimension"
}
```
### Request

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "idDimension": 90,
        "description": "Mi Dimensión",
        "idDimensionType": 1,
        "idDimensionCategory": 3,
        "value": 10,
        "switchValue": null,
        "active": true,
        "tagName": "MiTagDimension"
    }
}
```

<br>

## Actualizar Registro

### Descripción

Permite actualizar un registro de la entidad "Dimensions", la actualización se realizá dinámicamente, lo que quiere decir, que dependiendo del dato que reciba, es lo que actualizará.

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/{idDimension} |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/{idDimension}     |
| Protocolo           | PUT                                               |

### Request

**Descripción**

| Parámetro             | Tipo    | Obligatorio | Descripción |
|-----------------------|---------|-------------|-------------|
| IdProduct             | Int     | Sí          | ID del producto asociado |
| description           | String  | No          | Descripción de la dimensión |
| idDimensionType       | Int     | No          | Tipo de la dimensión |
| idDimensionCategory   | Int     | No          | Categoría de la dimensión |
| value                 | Int     | No          | Valor de la dimensión (permite null) |
| switchValue           | Int     | No          | Valor del switch (permite null) |
| active                | Int     | No          | 0 = No; 1 = Sí |
| tagName               | String  | No          | Tag de la dimensión, debe ser en formato camelCase |

**Ejemplos de Envío**

```json
{
	"idProduct": 7575,
	"value": null
}
```

### Request

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "updated": "2017-12-12T08:41:45.5240359-03:00"
    }
}
```