# ENTIDAD PROFILEDIMENSIONS

## Obtener Registros

### Descripción

Permite obtener todos los registros de la entidad "profiledimensions", dado un producto específico

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/profiledimensions/{idProduct} |
| Endpoint Producción | https://api.digevo.com/profiledimensions/{idProduct}     |
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
                "idProfileDimension": 85,
                "idProfile": 23,
                "idDimension": 31,
                "value": 0,
                "switchValue": null,
                "active": true,
                "idProduct": 7575
            },
            {
                "idProfileDimension": 86,
                "idProfile": 24,
                "idDimension": 31,
                "value": 2,
                "switchValue": null,
                "active": true,
                "idProduct": 7575
            }
        ]
    }
}
```

<br>

## Crear Registro

### Descripción

Permite registrar un nuevo registro en la entidad "profileDimensions"

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/profiledimensions/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/profiledimensions/{idProduct}     |
| Protocolo           | POST                                               |

### Request

**Descripción**

| Parámetro     | Tipo    | Obligatorio | Descripción |
|---------------|---------|-------------|-------------|
| idProfile     | String  | Sí          | Id del perfil |
| idDimension   | Int     | Sí          | Id de la dimensión |
| value         | Int     | Sí          | Valor |
| switchValue   | Int     | Sí          | 0 = No; 1 = Sí |
| active        | Int     | No          | 0 = No; 1 = Sí |

**Ejemplos de Envío**

```json
{
	"idProfile": 1,
	"idDimension": 1,
	"value": 10,
	"switchValue": 0,
	"active": 1
}
```
### Request

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "IdProfileDimension": 289,
        "IdProfile": 1,
        "IdDimension": 1,
        "Value": 10,
        "SwitchValue": false,
        "Active": true,
        "IsInfinite": null,
        "Dimensions": null,
        "Profiles": null
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