# ENTIDAD PROFILE

## Obtener Registros

### Descripción

Permite obtener todos las entidades "dimensionTypes"

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/dimensions/types |
| Endpoint Producción | https://api.digevo.com/freemium/dimensions/types     |
| Protocolo           | GET                                               |

### Request

Sólo vía GET

### Response

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "totalItems": 3,
        "items": [
            {
                "idDimensionType": 1,
                "description": "Numeric",
                "active": true,
                "tagName": "Numeric"
            },
            {
                "idDimensionType": 2,
                "description": "Switch",
                "active": true,
                "tagName": "Switch"
            },
            {
                "idDimensionType": 3,
                "description": "NumericConsumable",
                "active": true,
                "tagName": "NumericConsumable"
            }
        ]
    }
}
```