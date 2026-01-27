# TrucoGodot 
Este proyecto es una implementación del juego **Truco** usando **Godot + C#**, siguiendo el patrón de diseño **MVVM**, pensado para escalar a largo plazo (multiplayer, networking, reglas complejas, UI desacoplada).

La idea principal es **separar responsabilidades** y evitar que Godot “contamine” la lógica de dominio y aplicación.

---

##  Principios Clave

- **Domain** no depende de nada
- **Application** depende solo de Domain
- **Presentation** depende de Application
- **Infrastructure** implementa detalles técnicos (Godot, red, escenas)
- **Navigation** desacoplada de las escenas
- **ViewModels** no conocen a Godot
- **Views** no tienen lógica de negocio

---

## Estructura de Carpetas

```
res://
TrucoGodot
├── Assets
│   └── Sprites
│
├── Bootstrap
│   └── GameBootstrap.cs
│
├── Domain
│   ├── Game/
│   │   └── Game.cs
│   │
│   ├── Lobby/
│   │   └── Lobby.cs
│   │
│   └── Player/
│       └── Player.cs
│
├── Application
│   ├── Common/
│   │   └── Navigation/
│   │       ├── INavigationService.cs
│   │       └── NavigationRequest.cs
│   │
│   ├── Login/
│   │   └── LoginViewModel.cs
│   │  
│   ├── MainMenu/
│   │   └── MainMenuViewModel.cs
│   │  
│   ├── CreateMatch/
│   │   └── CreateMatchViewModel.cs
│   │  
│   ├── JoinMatch/
│   │   └── JoinMatchViewModel.cs
│   │  
│   └── Lobby/
│       └── LobbyViewModel.cs
│
├── Presentation
│   ├── Common/
│   │   └── ViewBase.cs
│   │
│   ├── Login/
│   │   ├── LoginView.cs
│   │   └── LoginView.tscn
│   │
│   ├── MainMenu/
│   │   ├── MainMenuView.cs
│   │   └── MainMenuView.tscn
│   │
│   ├── CreateMatch/
│   │   ├── CreateMatchView.cs
│   │   └── CreateMatchView.tscn
│   │
│   ├── JoinMatch/
│   │   ├── JoinMatchView.cs
│   │   └── JoinMatchView.tscn
│   │
│   ├── Lobby/
│   │   ├── LobbyView.cs
│   │   └── LobbyView.tscn
│   │
│   └── Game/
│       ├── GameScene.cs
│       └── Game.tscn
│
├── Infrastructure
│   ├── Navigation
│   │   ├── NavigationService.cs
│   │   ├── ScenesPaths.cs
│   │   └── SceneManager.cs
│   │
│   ├── Auth
│   │   └── AuthService.cs
│   │
│   └── Net
│       ├── Events/
│       ├── Messages/
│       ├── Protocol/
│       ├── Utils/
│       ├── WebSocket/
│       └── GameNet.cs
│
├── Shared
│   └── ServiceLocator.cs
│
├── main.tscn
├── TrucoProject.csproj
└── project.godot
```

---

##  Capas Explicadas

###  Domain

Contiene **la lógica pura del juego**. (reglas del truco, estados, validaciones, etc. )

- No conoce Godot
- No conoce red
- No conoce ViewModels

Ejemplos:
- `Game`
- `Lobby`
- `Player`

---

### Application

Orquesta el comportamiento de la app.

- Contiene **ViewModels**
- Usa entidades de Domain
- Define **interfaces** (ej: navegación)

Ejemplo:
- `LoginViewModel`
- `MainMenuViewModel`
- `LobbyViewModel`

Los ViewModels:
- Exponen eventos (`Action`)
- Exponen métodos de intención del usuario
- No renderizan UI
- No usan nodos Godot

---

### Presentation

Es la capa Godot.

- Scenes (`.tscn`)
- Views (`.cs`)
- Conecta UI ↔ ViewModel

Cada View:
- Se suscribe a eventos del VM
- Llama métodos del VM
- No tiene lógica de negocio

`ViewBase`:
- Maneja helpers como `RunOnMainThread`
- Centraliza comportamiento común

---

### Infrastructure

Implementaciones concretas.

#### Navigation
- `INavigationService` → contrato
- `NavigationService` → implementación
- `SceneManager` → cambio real de escenas Godot

El ViewModel navega **sin saber qué escena existe**.

#### Net
- WebSockets
- Mensajes
- EventBus
- Sin contaminar ViewModels

---

### Bootstrap

Punto de arranque del juego.

- Registra servicios
- Inicializa dependencias
- Configura ServiceLocator

---

### Shared

Infraestructura transversal.

- `ServiceLocator`
- Helpers globales (si hacen falta)

---
---

## 🧭 Flujo de Navegación

1. View llama a ViewModel
2. ViewModel usa `INavigationService`
3. `NavigationService` guarda un `NavigationRequest`
4. `SceneManager` cambia escena
5. La nueva escena consume el request

---

## ✅ Reglas de Oro

- ❌ No usar Godot en Domain
- ❌ No lógica en Views
- ❌ No SceneManager desde ViewModels
- ✅ ViewModels solo hablan con interfaces
- ✅ Eventos para comunicar cambios

---

## 🚀 Estado actual

- Login
- MainMenu
- Lobby
- Game (inicio)
- Infraestructura de red en progreso

---