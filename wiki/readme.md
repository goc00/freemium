
# GetProfilesByProduct()

### Descripción General

Obtiene perfiles por el producto enviado

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/getprofilesbyproduct/{idProduct} |
| Endpoint Producción | https://api.digevo.com/freemium/getprofilesbyproduct/{idProduct}     |
| Protocolo           | GET                                               |

<br>

## Request

### Ejemplo de estructura

```
{idProduct}: Identificador del producto
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "profiles": [
			{
				"idProfile": 6,
				"name": "Chef Free",
				"description": "Free algo",
				"active": true,
				"tagName": "FreeSusazon",
				"paid": false
			},
			{
				"idProfile": 7,
				"name": "Chef Premium",
				"description": "Sin publicidad, Videos de recetas completos, Descuentos",
				"active": true,
				"tagName": "Premium",
				"paid": true
			}
		]
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "error": {
        "code": 204,
        "message": "No existen parámetros disponibles en el sistema"
    }
}
```

<br><br><br>

# GetDataUser()

### Descripción General

Obtiene información completa del perfil y dimensiones para el usuario

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/getdatauser/{idProduct}/{idClient} |
| Endpoint Producción | https://api.digevo.com/freemium/getdatauser/{idProduct}/{idClient}     |
| Protocolo           | GET                                               |

<br>

## Request

### Ejemplo de estructura

```
{idProduct}: Identificador del producto
{idClient}: Identificador maestro del usuario
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
		"idSubscription": 11128,
		"dateCreated": "2017-05-30T00:51:14.967",
		"profile": {
			"idProfile": 6,
			"name": "Chef Premium",
			"description": "Sin publicidad, Videos de recetas completos, Descuentos",
			"active": true,
			"tagName": "Premium",
			"paid": true
		},
		"dimensions": [
			{
				"idDimension": 55,
				"nameDimension": "Cyber Studio Moenne Día",
				"tagName": "CyberStudioMoenneDia",
				"idDimensionType": 1,
				"nameDimensionType": "Numeric",
				"currentValue": 30,
				"originalValue": null
			},
			{
				"idDimension": 79,
				"nameDimension": "Cyber Studio Moenne Mes",
				"tagName": "CyberStudioMoenneMes",
				"idDimensionType": 3,
				"nameDimensionType": "Consumible",
				"currentValue": 10,
				"originalValue": 15
			},
			{
				"idDimension": 80,
				"nameDimension": "Acceso Descuentos",
				"tagName": "AccesoDescuentos",
				"idDimensionType": 2,
				"nameDimensionType": "Switch",
				"currentValue": true,
				"originalValue": null
			},
			{
				"idDimension": 99,
				"nameDimension": "Tipos cupón",
				"tagName": "TipoCuponZana",
				"idDimensionType": 1,
				"nameDimensionType": "Numeric",
				"currentValue": "10,87,33,22,90"
			}
		]
	}
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "error": {
        "code": 204,
        "message": "No existen parámetros disponibles en el sistema"
    }
}
```

<br><br><br>

# SetProfileUser

### Descripción General

Permite el cambio o seteo de perfil para un usuario en particular. Puede ser "bajando" o "subiendo" de categoría, el servicio administra lo necesario para tal efecto.
<br>

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/setprofileuser |
| Endpoint Producción | https://api.digevo.com/freemium/setprofileuser     |
| Protocolo           | POST                                               |

<br>

## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idProduct 		| Integer | ID del producto  | Sí          |
| idClient	| Integer  | ID usuario maestro      | Sí          |
| idProfile			| Integer  | ID perfil al que se desea actualizar        | Sí          |


### Ejemplo estructura

```json
{
	"idProduct": 7575,
	"idClient": 1,
	"idProfile": 9
}
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
        "profile": {
            "idProfile": 9,
            "name": "CineHoyts 8 entradas 2D",
            "description": null,
            "active": true,
            "tagName": "Hoyts2d8",
            "paid": true
        }
    }
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "error": {
        "code": 204,
        "message": "No existe el perfil para el producto enviado"
    }
}
```

<br><br><br>

# GetDimension()

### Descripción General

Obtiene información de una dimensión en particular

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/getdimension/{idProduct}/{idClient}/{idProfile}/{idDimension} |
| Endpoint Producción | https://api.digevo.com/freemium/getdimension/{idProduct}/{idClient}/{idProfile}/{idDimension}     |
| Protocolo           | GET                                               |

<br>

## Request

### Ejemplo de estructura

```
{idProduct}: Identificador del producto
{idClient}: Identificador maestro del usuario
{idProfile}: Identificador de perfil
{idDimension}: Identificador de dimensión
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data": {
		"nameDimension": "Cyber Studio Moenne Día",
		"tagName": "CyberStudioMoenneDia",
		"idDimensionType": 1,
		"nameDimensionType": "Numeric",
		"currentValue": 30
	}
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "error": {
        "code": 204,
        "message": "No existen parámetros disponibles en el sistema"
    }
}
```



<br><br><br>

# UpdateValueDimension

### Descripción General

Actualiza al valor que sea, una dimensión en particular.
<br>

## Descripción técnica

| **Atributo**        | **Valor**                                          |
|---------------------|----------------------------------------------------|
| Endpoint Desarrollo | https://dev-api.digevo.com/freemium/updatevaluedimension |
| Endpoint Producción | https://api.digevo.com/freemium/updatevaluedimension     |
| Protocolo           | PUT                                               |

<br>

## Request

### Parámetros

| Atributo          | Tipo    | Descripción               | Obligatorio |
|-------------------|---------|---------------------------|:-----------:|
| idProduct 		| Integer | ID del producto  | Sí          |
| idProfile	| Integer  | ID perfil      | Sí          |
| idClient	| Integer  | ID usuario maestro      | Sí          |
| idDimension			| Integer  | ID dimensión        | Sí          |
| value			| String  | Valor para actualizar dimensión        | Sí          |

### Ejemplo estructura

```json
{
	"idProduct": 1,
	"idProfile": 22,
	"idClient": 100,
	"idDimension": 55,
	"value": 10
}
```

<br>

## Responses

### Estructura respuesta de éxito

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "data":
		{
			"idDimension": 55,
			"nameDimension": "Cyber Studio Moenne Día",
			"tagName": "CyberStudioMoenneDia",
			"idDimensionType": 3,
			"nameDimensionType": "Consumible",
			"oldValue": 40,
			"currentValue": 30
		}
}
```

### Estructura respuesta de error

```json
{
    "apiVersion": "1.0",
    "context": "freemium",
    "error": {
        "code": 204,
        "message": "No existe el perfil para el producto enviado"
    }
}
```

<br><br><br>
