# **RFOAuth2Client — Documentación Técnica**

## **1. Propósito del módulo**

**RFOAuth2Client** es el componente encargado de integrar **RFAuth** con proveedores externos de identidad mediante los estándares **OAuth 2.0** y **OpenID Connect (OIDC)**.  
Su función principal es permitir que los usuarios se autentiquen utilizando servicios externos (Keycloak, Google, Microsoft, etc.).

Este módulo trabaja en conjunto con:

- **RFOAuth2ClientControllers**: expone los endpoints públicos para iniciar la autorización, listar proveedores y manejar el callback.
- **RFRBAC**: asignación automática de roles.
- **RFUserEmailVerified**: verificación automática del email del usuario.

---

## **2. Arquitectura general**

### **2.1 Flujo de autorización**

1. El frontend solicita la lista de proveedores configurados.
2. El usuario selecciona un proveedor.
3. El sistema redirige al proveedor externo usando el endpoint `authorize`.
4. El proveedor devuelve un `code` al endpoint `callback`.
5. RFOAuth2Client intercambia el `code` por tokens (`token endpoint`).
6. Se obtiene información del usuario (`userinfo endpoint`).
7. Se aplican reglas de roles y registro automático.
8. Se completa el login en RFAuth.

---

## **3. Configuración de proveedores**

La configuración se realiza en **appsettings.json**, dentro de la sección:

```json
"OAuth2Providers": { ... }
```

Cada proveedor se define mediante una clave que representa su nombre interno:

### **3.1 Parámetros principales**

| Parámetro | Descripción |
|----------|-------------|
| **providerName** | Nombre interno del proveedor. Se usa en el callback y se guarda en la base de datos. No debe ser `"local"`. |
| **displayName** | Nombre mostrado en el frontend (botón de login). |
| **clientId** | Identificador del cliente en el proveedor de identidad. |
| **clientSecret** | Secreto del cliente. |
| **redirectUri** | URL del frontend donde se redirige tras la autorización. Debe coincidir con la configuración del proveedor. |
| **urlBase** | Base para construir los endpoints. Si se omite, cada endpoint debe especificarse con URL absoluta. |
| **scope** | Scope por defecto. Si se omite: `"openid email profile"`. |
| **usePkce** | Actualmente sin efecto. |

### **3.2 Ejemplo básico**

```json
"OAuth2Providers": {
  "keycloak": {
    "isEnabled": true,
    "displayName": "Keycloak",
    "scope": "openid email profile",

    "client": {
      "clientId": "myClient",
      "clientSecret": "secret",
      "redirectUri": "https://frontend/callback",
      "urlBase": "https://idp/auth",
      "usePkce": false
    }
  }
}
```

---

## **4. Configuración de endpoints**

Cada proveedor puede definir sus endpoints de autorización.  
El sistema soporta **cuatro endpoints**:

### **4.1 Endpoints automáticos**
- `authorize`
- `token`
- `userinfo`

Se generan automáticamente si no se especifican.

### **4.2 Endpoint manual**
- `logout`

Debe configurarse explícitamente.

---

### **4.3 Parámetros disponibles por endpoint**

Cada endpoint admite los siguientes parámetros:

#### **Generales**
- **name**: nombre del endpoint (clave del objeto).
- **url**: si se omite → `"/{name}"` relativo a `client.urlBase`.
- **method**:  
  - GET → `authorize`, `logout`  
  - POST → `token`, `userinfo`
- **authorizationHeader**:  
  - false → `authorize`, `token`  
  - true → `userinfo`, `logout`
- **query**: parámetros adicionales en el query.
- **body**: parámetros adicionales en el body.
- **contentType**: por defecto `FormUrlEncoded`.

#### **Parámetros automáticos en query**
- **clientIdInQuery**  
  - true → `authorize`  
  - false → resto
- **redirectUriInQuery**  
  - true → `authorize`  
  - false → resto
- **clientSecretInQuery**: por defecto false  
- **refreshTokenInQuery**: por defecto false

#### **Parámetros automáticos en body**
- **clientIdInBody**  
  - true → `token`  
  - false → resto
- **redirectUriInBody**  
  - true → `token`  
  - false → resto
- **clientSecretInBody**  
  - true → `token`  
  - false → resto
- **refreshTokenInBody**: por defecto false

#### **Scope**
- Solo para `authorize`.  
  Sobrescribe el scope por defecto del proveedor.

---

### **4.4 Formas abreviadas de definir endpoints**

- **true** → endpoint autoconfigurado.
- **false** → endpoint omitido.
- **"ruta"** → se usa como URL y el resto se toma por defecto.

Ejemplo útil para Keycloak/Google (usan `/auth` en lugar de `/authorize`):

```json
"endpoints": {
  "authorize": "/auth"
}
```

### **4.5 Ejemplo completo**

```json
"endpoints": {
  "authorize": "/auth",
  "logout": {
    "url": "/logout",
    "method": "POST",
    "authorizationHeader": false,
    "clientIdInBody": true,
    "clientSecretInBody": true,
    "refreshTokenInBody": true
  }
}
```

---

## **5. Mapeo de roles**

La sección **roles** permite definir cómo se obtienen los roles del usuario desde el proveedor.

Cada elemento contiene:

| Campo | Descripción |
|-------|-------------|
| **source** | Origen de los datos. Actualmente solo `"token"` (JWT). |
| **path** | Ruta dentro del token donde se encuentran los roles. Depende del proveedor. |

Ejemplo:

```json
"roles": [
  {
    "source": "token",
    "path": "resource_access.uccuyo-keycloak.roles"
  }
]
```

---

## **6. Características adicionales (features)**

La sección **features** controla el comportamiento del cliente:

| Parámetro | Descripción |
|-----------|-------------|
| **mandatoryRoles** | Si es true, los roles del proveedor reemplazan los roles existentes. Si es false, se agregan. |
| **allowSelfRegistration** | Si es true, se crea automáticamente un usuario si no existe. |

Ejemplo:

```json
"features": {
  "mandatoryRoles": true,
  "allowSelfRegistration": true
}
```

---

## **7. Buenas prácticas y recomendaciones**

### ✔ Elegir nombres de proveedor claros  
Evitar `"local"` porque está reservado para login interno.

### ✔ Verificar siempre el `redirectUri`  
Debe coincidir exactamente con el configurado en el proveedor.

### ✔ Usar scopes mínimos necesarios  
Por defecto: `openid email profile`.

### ✔ Revisar el path de roles según el proveedor  
Keycloak, Google y Microsoft tienen estructuras distintas.

### ✔ Definir `authorize` explícitamente si el proveedor no usa `/authorize`  
Ejemplo: Keycloak → `/auth`.

---

## **8. Ejemplo completo de proveedor**

```json
"OAuth2Providers": {
  "keycloak": {
    "isEnabled": true,
    "displayName": "Keycloak",
    "scope": "openid email profile",

    "client": {
      "clientId": "myClient",
      "clientSecret": "secret",
      "redirectUri": "https://frontend/callback",
      "urlBase": "https://idp/realms/UCCUYO/protocol/openid-connect",
      "usePkce": false
    },

    "endpoints": {
      "authorize": "/auth",
      "logout": {
        "url": "/logout",
        "method": "POST",
        "authorizationHeader": false,
        "clientIdInBody": true,
        "clientSecretInBody": true,
        "refreshTokenInBody": true
      }
    },

    "roles": [
      {
        "source": "token",
        "path": "resource_access.uccuyo-keycloak.roles"
      }
    ],

    "features": {
      "mandatoryRoles": true,
      "allowSelfRegistration": true
    }
  }
}
```