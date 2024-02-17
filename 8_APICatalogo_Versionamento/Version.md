### Querystring

* Inclui o número da versão como um parâmetro de consulta na URL.

```json
https://apiexemplo.com/resource?version=1
https://apiexemplo.com/resource?version=2
```

---

### URI 

* Inclui a versão diretamente na URL da API.

```json
https://apiexemplo.com/v1/resource
https://apiexemplo.com/v2/resource
```

---

### Headers

* Especifica a versão desejada no cabeçalho (header) do request HTTP.

```json
GET /resource HTTP/1.1
Host: api.exemplo.com
Accept: application/json
X-API-Version: 1
```

---

### Media Type

* Usar diferentes tipos de mídia para representar versões diferentes da API.

```json
Accept: application/vnd.exemplo.v1+json
Accept: application/vnd.exemplo.v2+json
```
