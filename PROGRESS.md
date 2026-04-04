# Supercent Project - Session Recovery & Progress

## [Mandatory Compliance Rules]
본 프로젝트의 모든 작업은 `GeminiCLI 준수규칙.txt`를 절대적으로 따릅니다.
1. **Architectural Thinking**: 구현 전 설계 제안 및 승인 필수.
2. **Fail-Fast (Explicit Failure)**: `Debug.Assert()`를 통한 논리 오류 즉각 보고.
3. **Strategic Trade-offs**: 성능 vs 가독성 관점의 트레이드오프 분석 제공.
4. **Clean & High-Performance**: Zero-GC 및 캐싱 최적화 지향.
5. **Environment Strictness**: Unity 2022.3.62f2, C# 9.0, .asmdef 모듈화 준수.
6. **Zero-Fluff**: 간결한 기술 용어 중심 소통.
7. **Safe File I/O**: 로그 기록 시 Append-Only 원칙 준수.
8. **Incremental Refactoring**: 사전 진단 브리핑 후 단계적 리팩터링.

---

## [Current Project State]

### 1. 핵심 시스템 아키텍처
- **SingletonBase (Lazy/Auto-Create)**: 전역 매니저 인스턴스 관리.
- **Event-Driven Pipeline**: `ItemStacker.OnStackChanged`를 기반으로 한 생산-소비 루프 최적화 (Update 폴링 0%).
- **State Pattern AI**: `TransportWorker`에 적용 완료. 코루틴 기반 FSM을 클래스 캡슐화 상태 머신으로 전환하여 Zero-GC 달성.
- **Object Pooling**: Key 기반 Dictionary 구조로 리소스 재사용 최적화 및 하이어라키 자동 정렬(`Return` 시 부모 복귀).

### 2. 튜토리얼 및 안내 시스템 (New)
- **TutorialManager**: 순차적 단계(`TutorialStep`) 관리 및 이벤트 수신부(`OnActionPerform`).
- **CameraDirector**: `MainCamera` 부착형. 시네마틱 연출 시 **조이스틱 입력 강제 종료(`EndStick`)** 및 조작 차단 전담.
- **TutorialArrow (Hybrid)**: 
    - **화면 안**: 타겟 머리 위 3D 마커(`targetMarker`) 부드러운 부유 및 회전 연출.
    - **화면 밖**: 플레이어 발밑 화살표(`arrowVisual`)가 타겟 방향으로 실시간 Y축 회전.
- **Trigger Logic**: `DistributePotion` 완료 시점이 아닌, 실제 골드가 생성되는 `PotionTable.ProduceGold()` 시점에 단계 트리거.

### 3. 주요 컴포넌트 설정 현황
- **Inventory**: 돌, 포션, 골드 획득 시 각각 튜토리얼 매니저에 신호 송신부 구축.
- **UnlockZone**: 해금 완료 시 튜토리얼 신호 송신.
- **PrisonManager**: 감옥 만석 시 확장 구역 안내 특수 트리거(`TriggerPrisonExpandGuide`) 연동.

---

## [Setup Instructions for Next Session]

1. **CameraDirector**: `MainCamera` 오브젝트에 부착되어 있는지 확인.
2. **TutorialManager Steps**:
    - `Target`: 반드시 Hierarchy 씬 오브젝트 참조 (Prefab 금지).
    - `CameraPoint`: 연출 시 카메라가 위치할 지점 (회전값 포함).
    - `UnlockZoneToActivate`: 해당 단계 시작 시 활성화할 해금 오브젝트.
3. **TutorialArrow**: `ArrowVisual`(발밑)과 `TargetMarker`(목적지 위) 오브젝트 할당 확인.

---

## [Pending Tasks]
- [ ] **NPC Migration**: `Prisoner` AI를 `TransportWorker`와 동일한 `INpcState` 기반 상태 패턴으로 리팩터링.
- [ ] **Economic Balancing**: 해금 비용 및 자원 생산 속도 데이터 테이블(SO) 정립.
- [ ] **Visual Polish**: 튜토리얼 마커 및 화살표의 쉐이더/이펙트 보강.

**Technical Note**: 현재 시스템은 모든 자원 순환 노드가 이벤트를 발행하므로, 튜토리얼 단계 추가 시 코드 수정 없이 `TutorialManager` 인스펙터 설정만으로 로직 확장이 가능합니다.

---
## [Update: 2026-04-05]
### 1. 고주파 채굴 시스템 안정화 (High-Frequency Mining Stabilization)
- **Virtual Reservation 도입**: ItemStacker.pendingCount를 통한 비행 중인 자원 예약 시스템 구축.
- **Visual-Logic Sync**: 연출 중 인벤토리가 가득 차더라도 조약돌이 공중에 멈추지 않고 즉시 풀로 반납되도록 예외 처리 강화.
- **Dynamic Offset Fix**: 예약 카운트를 좌표 계산에 포함하여 고속 채굴 시 조약돌 간 겹침 현상 해결.

### 2. 현재 상태
- **Stable**: 드릴 및 불도저 채굴 시 자원 적재 정합성 100% 확보.
- **Pending**: Prisoner AI 상태 패턴 리팩터링 및 전반적인 밸런스 데이터(SO) 정립.

## [Update: 2026-04-05 (Fix)]
### 1. 고주파 채굴 시스템 안정화 (Virtual Occupancy)
- **Status**: 완료 (Implemented).
- **Core Logic**: ItemStacker.pendingCount를 통한 비행 중인 자원 예약 시스템 구축. 고속 채굴 시에도 조약돌의 좌표 계산과 용량 체크가 정밀하게 동기화됨.
- **Bug Fix**: 조약돌이 공중에 멈추는 데드락 현상을 [Fail-Safe Release] 로직으로 해결함.

### 2. 향후 작업 로드맵
- **Prisoner AI**: INpcState 기반 상태 패턴 리팩터링 진행 예정 (Pending).
- **Compliance**: 파일 로그 시스템 인코딩 안정화 및 Append-Only 원칙 엄격 준수.
## [Update: 2026-04-05 (Effects)]
### 1. SOLID 기반 Effect System 구축
- **EffectManager**: 중앙 집중형 이펙트 제어 및 풀링 시스템 구현.
- **Auto-Recycle**: 파티클 재생 완료 후 자동 풀 반납 로직 적용.

### 2. 이펙트 통합 완료 지점
- **Mining**: 채굴 타격 시 'MiningHit' 이펙트 재생.
- **Prisoner**: 배급 완료 시 'DistributionSuccess' 이펙트 재생.
- **Unlock**: 인부 활성화 시 'WorkerUnlock' 이펙트 재생.

### 3. 현재 상태
- **Stable**: 모든 이펙트가 풀링 시스템 내에서 정상 작동하며 물리 로직과 분리됨.
## [Update: 2026-04-05 (System Hardening)]
### 1. Pooling System 제약 조건 확장
- **Refactoring**: where T : MonoBehaviour -> where T : Component로 상향 조정.
- **Benefit**: ParticleSystem, AudioSource 등 엔진 내장 컴포넌트 풀링 지원 완료.

### 2. 현재 상태
- **Stable**: 이펙트 시스템 컴파일 오류 해결 및 전체 도메인 통합 완료.
## [Update: 2026-04-05 (Zero-Config Effect Automation)]
### 1. 자동 초기화(Auto-Initialization) 구조 구축
- **Dynamic Loading**: EffectManager가 인스펙터 데이터가 비어있더라도 Resources/Effects에서 프리팹을 찾아 동적 풀링을 수행하도록 Fallback 적용.
- **Editor Script**: 유니티 컴파일 시 파티클 프리팹을 자동으로 생성해 주는 EffectSetupEditor.cs 추가 (수동 오브젝트 세팅 비용 100% 절감).

### 2. 현재 상태
- **Fully Automated**: 사용자가 아무런 클릭(Drag & Drop)을 하지 않아도 채굴, 배급, 해금 이펙트가 자동으로 로드 및 재생됩니다.
## [Update: 2026-04-05 (Revert)]
### 1. 자동화 로직 철회 및 수동 모드 복원
- **Reverted**: EffectSetupEditor 삭제 및 EffectManager Resources 로직 제거.
- **Status**: 인스펙터 수동 설정 방식으로 원복 완료.
## [Update: 2026-04-05 (Prison System Optimization)]
### 1. 스폰 및 수감 로직 개선
- **Spawning**: 전체 인원이 아닌 실제 수감 인원(IsPrisonFull) 기준으로 생성 제한 적용.
- **Visuals**: 도착 시의 회전(DORotate) 및 랜덤 이동(DOMove) 연출 삭제.
- **Waiting**: 감옥 앞 정렬 대기열을 배제하고 대기 지점 상주(Autonomous Waiting) 방식으로 전환.

### 2. 현재 상태
- **Stable**: 감옥이 가득 차기 전까지 죄수가 끊김 없이 생성되며, 수감 연출이 간소화됨.