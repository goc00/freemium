# ENTIDAD DIMENSIONCATEGORIES

## Obtener Registros

### Descripción

Permite obtener todos las entidades "dimensionCategories" de un producto

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/categories/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/categories/{idProduct}     |
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
                "idDimensionCategory": 10,
                "description": "Cupones Suscripción",
                "idProduct": 7575,
                "active": true,
                "tagName": "CuponesSuscripcion"
            },
            {
                "idDimensionCategory": 14,
                "description": "Perfiles disponibles para suscripción",
                "idProduct": 7575,
                "active": true,
                "tagName": "PerfilesSuscripcion"
            }
        ]
    }
}
```

<br>

## Crear Registro

### Descripción

Permite crear un nuevo registro en la entidad "dimensioncategories"

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/categories/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/categories/{idProduct}     |
| Protocolo           | POST                                               |

### Request

**Descripción**

| Parámetro    | Tipo    | Obligatorio | Descripción |
|--------------|---------|-------------|-------------|
| description  | String  | Sí          | Descripción |
| tagName      | String  | Sí          | Tag en camelCase |
| active       | int     | Sí          | 0 = No; 1 = Sí |

**Ejemplo de Envío**

```json
{
	"description": "Categoría de Dimensión",
	"tagName": "dimensionCategory",
	"active": 1
}
```
### Request

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "idDimensionCategory": 30,
        "description": "Categoría de Dimensión",
        "idProduct": 7575,
        "active": true,
        "tagName": "dimensionCategory"
    }
}
```

<br>

## Actualizar Registro

### Descripción

Permite actualizar un registro de la entidad "dimensionCategories", la actualización se realizá dinámicamente, lo que quiere decir, que dependiendo del dato que reciba, es lo que actualizará.

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/categories/{idDimensionCategory} |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/categories/{idDimensionCategory}     |
| Protocolo           | PUT                                               |

### Request

**Descripción**

| Parámetro    | Tipo    | Obligatorio | Descripción |
|--------------|---------|-------------|-------------|
| IdProduct    | Int     | Sí          | ID del producto asociado |
| description  | String  | No          | Descripción |
| tagName      | String  | No          | Tag en camelCase |
| active       | int     | No          | 0 = No; 1 = Sí |

**Ejemplo de Envío**

```json
{
	"idProduct": 7575,
	"active": 0
}
```

### Response

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "updated": "2017-12-12T11:14:59.8464366-03:00"
    }
}
```
