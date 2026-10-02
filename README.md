# 🎮 3D-Exploration

<div align="center">

[![Unity](https://img.shields.io/badge/Unity-6000.3.10f1_LTS-black?logo=unity)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows_|_macOS-blue)]()
[![Status](https://img.shields.io/badge/Status-Prototype-orange)]()
[![License](https://img.shields.io/badge/License-Internal-red)]()

Prototype 3D Exploration

</div>

---

# 📖 Tentang Game

Game ini berfokus untuk mengimplementasikan Third Person View dan menambahkan beberapa interaction kepada npc, dan eksplorasi mengenai 2 puzzle game dengan tema
**Memory Match Card** dan **Sliding Puzzle** dan menyimpan *best score* kedalam json file

---

# ✨ Fitur

- 🎴 3D Player Exploraction with Third Person View
- 📊 Interact System
- 🎒 Puzzle Game (Memory Match Card & Sliding Puzzle)

---

# 🎮 Input

## Keyboard

| Tombol | Fungsi |
|---------|---------|
| W A S D | Movement |
| F | Interact |
---

# 📂 Struktur Folder

```text
Assets
│
├── Animation/
│   └── Player/
│
├── Prefabs/
│
├── Plugins/
│   ├── vHierarchy/
│   ├── Wingman/
│   └── Sirenix/
│
├── Scenes/
│
├── Scriptables/
|
├── Scripts/
│   └── Assemdef
|
├── Settings/
|
├── Sprites/
|
└── Third Party/
```

---

# 🧩 Design Pattern

Pattern yang digunakan dalam project:

## Core Patterns

- Singleton (Quick Prototype)
- Observer
- State Machine
- Save Decoractor (JSON, PlayerPrefs, & Encrypted JSON)

## Gameplay Patterns

- Event Driven Architecture

## Unity Patterns

- Component-Based Architecture
- Dependency Injection (Manual)

---

# 🔌 Plugins & Assets

| Plugin | Kegunaan |
|----------|-----------|
| Odin Inspector | Inspector Enhancement |
| Input System | Input Management |
| TextMeshPro | Text Rendering |
| Unitask | Async Programming |
| Newtonsoft JSON | JSON Enchancement |

## Sources
Plugin
- [Newtonsoft JSON](https://www.newtonsoft.com/json)
- [UniTask](https://github.com/Cysharp/UniTask)
- [Wingman](https://assetstore.unity.com/packages/tools/utilities/wingman-your-inspector-s-best-friend-303181)
- [vHeirarchy V2](https://assetstore.unity.com/packages/tools/utilities/vhierarchy-2-253397) 
- [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)

Third Party
- [KayKit - Adventurers Character Pack (for Unity)](https://assetstore.unity.com/packages/3d/characters/humanoids/humans/kaykit-adventurers-character-pack-for-unity-290679)
- [Gridbox Prototype Materials](https://assetstore.unity.com/packages/2d/textures-materials/gridbox-prototype-materials-129127)
- [GUI Pro - Minimal Game Dark](https://assetstore.unity.com/packages/2d/gui/gui-pro-minimal-game-dark-358058)

---

# 📊 Coding Convention

## C#

- Menggunakan `camelCase` untuk field.
- Menggunakan `PascalCase` untuk property dan method.
- Menghindari Magic Number.
- Mengutamakan Composition dibanding Inheritance.
- Menggunakan `readonly` jika memungkinkan.

Contoh:

```csharp
private int m_currentHealth; // member field

public int CurrentHealth => m_currentHealth; // property field

public void TakeDamage(int p_amount) // param
{
    m_currentHealth -= p_amount;
}
```

---

# 🧪 Development Build

## Requirements

- Unity 6000.3.10f1 LTS
- Git
- Visual Studio / Rider

## Clone Repository

```bash
git clone https://github.com/AbdulAzziz2020/3D-Exploration.git
```

## Open Project

```text
Unity Hub
↓
Add Project
↓
Select Folder
↓
Open with Unity 6000.3.10f1 LTS
```

---

# 🎮 Cara Instalasi & Bermain

## Untuk Pemain (Rilisan ZIP)

1. Unduh versi terbaru dari [Google Drive](https://drive.google.com/file/d/1A4Pz7vaETk-GjSnLCi81ZhzrpSmQ24yr/view?usp=sharing)
2. Ekstrak seluruh file ZIP ke satu folder.
3. Pastikan file `.exe` dan folder `_Data` berada pada lokasi yang sama.
4. Jalankan:

```text
3D-Exploration.exe
```

5. Selamat bermain!

---

# 🐞 Known Issues

- Beberapa animasi masih menggunakan placeholder.

---

# 👨‍💻 Developer

**The Kuro Neko Team**

Built with ❤️ using Unity.

---

# 📜 License

Project ini dibuat untuk kebutuhan pengembangan internal dan pembelajaran.

All Rights Reserved.
