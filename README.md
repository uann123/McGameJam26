# Silent Serenade 🎵

[![Play on itch.io](https://img.shields.io/badge/Play%20on-itch.io-FA5C5C?style=for-the-badge&logo=itch.io&logoColor=white)](https://lina674.itch.io/silent-serenade)
<img width="347" height="237" alt="image" src="https://github.com/user-attachments/assets/1465522d-ef5b-473d-a861-574b309d397c" />

## 🎮 About The Game

**This world has fallen silent.**

Music has vanished, and the Queen is trapped in a realm where no sound remains. To restore harmony, she must hunt the lost musical notes scattered across the land while avoiding sharps—corrupted notes that seek to twist the melody.

**Silent Serenade** is a 2D platformer where every collected note matters. Navigate through beautifully crafted levels, collect musical notes to complete melodies, and avoid the dangerous sharps that hunt you. With every complete melody restored, a new instrument awakens, shaping both the sound and the theme of the next world.

## ✨ Key Features

- **Musical Progression System**: Collect notes to restore melodies and unlock new instruments
- **Dynamic Worlds**: Each completed melody transforms the environment with new themes and sounds
- **Strategic Gameplay**: Balance risk and reward as you hunt notes while avoiding sharps
- **Note Collection Mechanic**: Gather scattered musical notes across diverse platforming challenges
- **Enemy Mechanics**: Sharps spawn and hunt you as you collect notes, adding tension to your journey
- **Responsive Controls**: Smooth platforming with double jump, precise movement, and polished physics
- **Atmospheric Soundtrack**: Experience the world evolving from silence to a full symphony

## 🎯 How to Play

### Objective
Restore harmony to the silent world by collecting all musical notes in each level. Complete melodies to progress through different themed worlds.

### Controls
- **Move**: Arrow Keys / A & D
- **Jump**: Space Bar
- **Double Jump**: Press Space twice
- **Fast Fall**: Down Arrow while in air
- **Pause**: Escape

### Gameplay Mechanics
1. **Collect Notes**: Find and collect musical notes scattered throughout each level
2. **Avoid Sharps**: Enemy notes (sharps) will spawn and patrol the area. If they catch you, you'll lose your most recently collected note
3. **Complete Melodies**: Gather all notes in a level to restore a complete melody
4. **Progress**: Each completed melody unlocks a new instrument and transforms the world

### Tips
- Plan your route carefully before collecting notes
- Sharps spawn when you collect notes, so be strategic about your collection order
- Use the platforming mechanics (double jump, fast fall) to outmaneuver enemies
- Listen to the evolving soundtrack as you collect more notes

## 🛠️ Technical Information

### Built With
- **Unity** (2D Game Engine)
- **C#** for game scripting
- Universal Render Pipeline

### Project Structure
- **Assets/Scripts**: Game logic and mechanics
  - `PlayerMovement.cs`: Character controller with advanced platforming
  - `CollectibleNotes.cs`: Note collection and tracking system
  - `SharpeEnemy.cs` / `Patroller2D.cs`: Enemy AI and behavior
  - `MusicTrack.cs`: Audio and instrument progression
- **Assets/Scenes**: Game levels (LVL0, LVL1, LVL2, LVL3)
- **Assets/Sounds**: Audio assets and music tracks
- **Assets/Prefabs**: Reusable game objects

### For Developers

#### Running the Project
1. Clone this repository
2. Open the project in Unity (compatible version: check ProjectVersion.txt)
3. Open the `Start.unity` scene from `Assets/Scenes`
4. Press Play to test in the Unity Editor

#### Building the Game
1. Go to File → Build Settings
2. Select your target platform
3. Click "Build" and choose your output directory

## 🎮 Play Now

Experience Silent Serenade on [itch.io](https://lina674.itch.io/silent-serenade)

## 🏆 Credits

Created for McGameJam26

## 📝 License

This project was created as part of a game jam. Please respect the original creators' work.

---

**Restore the melody. Save the world. 🎵**
