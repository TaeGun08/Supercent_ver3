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
