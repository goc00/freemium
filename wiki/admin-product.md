# ENTIDAD PRODUCT

## Obtener Registros

### Descripción

Permite obtener todos las entidades "product"

### Descripción Técnica

| Atributo            | Valor                                             |
|---------------------|---------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/product |
| Endpoint Producción | https://api.digevo.com/freemium/product     |
| Protocolo           | GET                                               |

### Request

Sólo vía GET

### Response

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "totalItems": 7,
        "items": [
            {
                "idProduct": 1,
                "description": "MundoGames",
                "token": "HF756anHDSda",
                "tagName": "MundoGames"
            },
            {
                "idProduct": 2,
                "description": "Susazon",
                "token": "KUUusy86shh",
                "tagName": "Susazon"
            }
        ]
    }
}
```