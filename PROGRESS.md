# Project Progress - Supercent Project

## 📌 Status Overview
- **Current Phase**: Architecture Refinement (Actual Code Analysis)
- **Last Updated**: 2026-04-02
- **Strategy**: Data Integrity & Input Decoupling

---

## 🚀 Current Task
- [ ] Refactor Joystick to remove static dependencies.
- [ ] Optimize ItemStacker offset calculation (Index-based).
- [ ] Decouple PlayerController from specific Joystick implementation.

---

## 📋 Task List
### 1. Project Foundation
- [x] Establishment of GeminiCLI guidelines.
- [x] Setup of communication and logging protocols (`Prompt.md`, `PROGRESS.md`).
- [x] Folder restructuring (Actual path: `Assets/00. Project/02. Scripts`).

### 2. Core Mechanics (Under Review)
- [x] Mining System Prototyped.
- [x] Virtual Stacking System (Initial version).
- [ ] Input Abstraction (IInputProvider).
- [ ] Stack Logic Hardening (Data Consistency).


---

## 🏗️ Architectural Decisions
| Date | Decision | Rationale |
| :--- | :--- | :--- |
| 2026-04-02 | Fail-Fast Strategy | Early detection of logic errors via `Debug.Assert`. |
| 2026-04-02 | Virtual Stacking | Performance (No SetParent) & Animation independence. |
| 2026-04-02 | DIP via .asmdef | Compilation optimization & dependency control. |
| 2026-04-02 | Prompt Sync Protocol | Persistent context across sessions. |

---

## 🔑 Context Activation Keywords
- **Project**: `Supercent_ver3`
- **Spec**: `Unity 2022.3.62f2 / C# 9.0 / URP / New Input System`
- **Arch**: `Fail-Fast & Modularity (.asmdef)`
- **Logging**: `Prompt.md` (History), `PROGRESS.md` (State)
- **Role**: `Senior Engineer Consultant`

---

## ⚠️ Strict Compliance Rules
- **Rule 1**: Architectural Thinking (Design first, Code later).
- **Rule 2**: Fail-Fast & Explicit (No silent errors, use `Debug.Assert`).
- **Rule 3**: Strategic Trade-offs (Analyze pros/cons).
- **Rule 4**: Clean & High-Performance (Avoid GC/LINQ, optimize).
- **Rule 5**: Environment Strictness (LTS version, modern APIs, `.asmdef`).
- **Rule 6**: Zero-Fluff (Technical terms only, append **Technical Note**).

---

---

## 🛠️ Technical Stack
- **Engine**: Unity 2022.3.62f2 LTS
- **Language**: C# 9.0
- **Render Pipeline**: URP
- **Input**: New Input System
- **Modularity**: Assembly Definitions
