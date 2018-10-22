# ENTIDAD PROFILES

## Obtener Registros

### Descripción

Permite obtener todos las entidades "profiles" de un producto

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/profiles/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/profiles/{idProduct}     |
| Protocolo           | GET                                               |

### Request

Sólo vía GET

### Response

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "totalItems": 10,
        "items": [
            {
                "idProfile": 23,
                "name": "TupaseFree",
                "description": "Accede a los grandes descuentos en entradas y confitería que tenemos para ti. Si quieres aún mejores descuentos considera suscribirte a una membresía mensual y disfuta el cine todos los meses!",
                "active": true,
                "tagName": "Free",
                "paid": false,
                "dimensions": [
                    {
                        "idDimension": 31,
                        "nameDimension": "Cantidad Canjes",
                        "tagDimension": "CantidadCanjesCuponesSuscripcion",
                        "idDimensionCategory": 10,
                        "tagDimensionCategory": "CuponesSuscripcion",
                        "idDimensionType": 1,
                        "nameDimensionType": "NUMERIC",
                        "currentValue": null,
                        "originalValue": 0
                    }
                ]
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
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/profiles/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/profiles/{idProduct}     |
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
	"Name": "name_20171211-9:16-1",
	"Description": "desc_20171211-9:16-1",
	"TagName": "tag_20171211-9:16-1",
	"AnonDefault": 0,
	"UserDefault": 1,
	"Paid": 1
}
```
### Request

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "idProfile": 1033,
        "name": "name_20171211-9:16-1",
        "description": "desc_20171211-9:16-1",
        "active": true,
        "tagName": "tag_20171211-9:16-1",
        "paid": true,
        "dimensions": null
    }
}
```

<br>

## Actualizar Registro

### Descripción

Permite actualizar un registro de la entidad "profiles", la actualización se realizá dinámicamente, lo que quiere decir, que dependiendo del dato que reciba, es lo que actualizará.

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/profiles/{idProfile} |
| Endpoint Producción | https://api.digevo.com/freemium/profiles/{idProfile}     |
| Protocolo           | PUT                                               |

### Request

**Descripción**

| Parámetro    | Tipo    | Obligatorio | Descripción |
|--------------|---------|-------------|-------------|
| IdProduct    | Int     | Sí          | ID del producto asociado |
| name         | String  | No          | Nombre |
| description  | String  | No          | Descripción |
| active       | int     | No          | 0 = No; 1 = Sí |
| tagName      | String  | No          | Tag en camelCase |

**Ejemplo de Envío**

```json
{
	"idProduct": 7575,
	"description": "Mi Nueva Descripción"
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
