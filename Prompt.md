# Professional Development Log - Supercent Project

## [Initial Setup]

**Technical Requirement**:
> 세션 복구 및 즉각적인 업무 착수를 위한 핵심 컨텍스트 키워드 정의 요청. 아울러 프로젝트 표준 준수 규칙의 엄격한 적용을 강조함.

**Engineering Response**:
- **Engineering Summary**: 프로젝트 초기 진입을 위해 `Supercent_ver3` 컨텍스트 활성화 키워드와 시니어 엔지니어 컨설턴트 역할을 정의함. 무의미한 방어적 코드보다 `Debug.Assert`를 활용한 Fail-Fast 원칙을 최우선으로 설정하여 논리적 오류를 즉각 검출하는 환경을 구축함.
- **Decision**: 초기 설계 의도와 실제 구현 간의 정렬 상태 유지를 위한 기술적 제약 사항 수립.

**Technical Requirement**:
> 대상 Unity 엔진 버전을 2022.3.62f2로 특정하고, 개발 환경의 기술 스택 명세 갱신 요청.

**Engineering Response**:
- **Engineering Summary**: 개발 환경을 Unity 2022.3.62f2 LTS 버전으로 고정하고 관련 기술 스택(C# 9.0, URP, New Input System)을 재정의함. 환경 스펙의 엄격한 준수는 구식 API 사용으로 인한 잠재적 오류를 방지하고 런타임 안정성을 보장함.
- **Decision**: 런타임 안정성 보장을 위한 엔진 버전 및 API 수준 강제.

---

## [2026-04-02] Session History

**Technical Requirement**:
> PROGRESS.md에 기록된 마일스톤 및 아키텍처 결정 사항 분석을 통한 기술적 연속성 확보 요청.

**Engineering Response**:
- **Engineering Summary**: `PROGRESS.md`의 태스크 리스트와 아키텍처 결정 사항을 분석하여 현재 진행 단계를 '핵심 게임플레이 설계'로 동기화함. 이전 세션의 상태를 정확히 복구함으로써 기술적 연속성을 확보하고 최우선 과제를 도출함.
- **Decision**: 세션 간 컨텍스트 동기화를 통한 업무 지속성 확보.

**Technical Requirement**:
> 시스템 간 결합도 최소화 및 상호 참조 방지를 위한 설계 원칙 재확인 및 아키텍처 가이드라인 정립 요청.

**Engineering Response**:
- **Engineering Summary**: 상호 참조(Cyclic Dependency)를 원천 차단하기 위해 인터페이스 기반의 의존성 역전(DIP)과 물리적 어셈블리 분리를 제안함. 코드 수정 시 발생하는 재컴파일 범위를 최소화하고 시스템 간의 결합도를 낮추는 아키텍처를 설계함.
- **Decision**: 물리적 어셈블리(.asmdef) 분리를 통한 컴파일 최적화 및 의존성 격리.

**Technical Requirement**:
> 초기 프로토타이핑 속도 극대화를 위한 'Speed-First' 개발 전략 채택 및 기술 부채의 의도적 누적 관리 요청.

**Engineering Response**:
- **Engineering Summary**: 초기 프로토타이핑 속도를 극대화하기 위해 'Architecture Hardening' 단계를 유보하고 'Speed-First' 전략을 채택함. 아이디어의 빠른 검증과 런타임 기능 구현에 자원을 집중할 수 있는 유연한 협업 모드를 설정함.
- **Decision**: 빠른 가설 검증을 위한 MVP(Minimum Viable Product) 중심 개발 패스 설정.

**Technical Requirement**:
> 고주파 물리 쿼리(OverlapBox)의 오버헤드 최적화를 위한 대안 분석 및 이벤트 주도형 탐색 시스템 설계 요청.

**Engineering Response**:
- **Engineering Summary**: 매 프레임 발생하는 `OverlapBox`의 오버헤드를 줄이기 위해 물리 엔진의 Broadphase 결과를 재사용하는 트리거(Trigger) 기반 리스트 관리 방식이 제안됨. 이벤트 주도형(Event-Driven) 접근을 통해 불필요한 물리 쿼리를 제거하여 CPU 사이클을 보존함.
- **Decision**: 물리 연산 비용 절감을 위한 트리거 기반 이벤트 시스템 채택.

**Technical Requirement**:
> 현행 코드베이스의 아키텍처 진단 및 기능 구현 효율화를 위한 최단 경로(Fast-Track) 설계 제안 요청.

**Engineering Response**:
- **Engineering Summary**: `PlayerController`에 트리거 감지 로직을 직접 삽입하고 `MiningTarget` 컴포넌트로 대상을 식별하는 고속 구현 패스(Fast-Track)를 설계함. 구현 시간을 단축하고 즉각적으로 동작 확인이 가능한 구조를 제안함.
- **Decision**: 구현 속도 향상을 위한 컴포넌트 결합형 고속 로직 배치.

**Technical Requirement**:
> 자원 수집 및 물리적 적층 시스템(Stacking) 구축을 위한 아키텍처 설계 요청. 하이어라키 오염 방지 및 정밀 좌표 연산, 런타임 예외 처리(Fail-Fast) 필수 고려.

**Engineering Response**:
- **Engineering Summary**: 하이어라키 갱신 부하를 차단하기 위해 `SetParent`를 배제하고 `LateUpdate`에서 좌표를 강제 동기화하는 'Virtual Stacking' 시스템을 설계함. 인덱스 기반의 `Y-Offset` 연산을 통해 가비지 없는 위치 제어를 구현함.
- **Decision**: 성능과 가시성을 동시에 고려한 좌표 동기화 기반 적층 시스템 구축.

**Technical Requirement**:
> 프로토타입의 기술적 성숙도 제고를 위한 .asmdef 기반 물리적 모듈화 및 계층 구조 설계 요청.

**Engineering Response**:
- **Engineering Summary**: 프로토타이핑 완료 후 아키텍처 경량화를 위해 `Core`, `UI`, `Gameplay`, `Managers` 4단계 계층으로 분리된 `.asmdef` 기반 모듈화 방안을 수립함. 인터페이스를 통한 상호 참조 제거(DIP)를 핵심 전략으로 선택함.
- **Decision**: 프로젝트 확장성 확보를 위한 모듈형 아키텍처로의 전환.

**Technical Requirement**:
> 시스템 복잡도 제어 및 유지보수성 향상을 위한 컴포넌트 기반 기능 분산(Decoupling) 및 코드 리뷰 요청.

**Engineering Response**:
- **Engineering Summary**: 현재 프로젝트의 `PlayerController` 비대화와 싱글톤 구조의 과도한 복잡도를 핵심 문제로 진단하고, 이를 해결하기 위해 컴포넌트 기반 기능 분산 방안을 제안함. 응집도를 높여 유지보수 시 영향 범위를 최소화함.
- **Decision**: 단일 책임 원칙(SRP) 준수를 위한 컨트롤러 분해 및 기능 컴포넌트화.

**Technical Requirement**:
> 소스 코드 전수 조사를 통한 전역 상태 의존성 식별 및 데이터 무결성 보장을 위한 구조적 결함 진단 요청.

**Engineering Response**:
- **Engineering Summary**: `Assets/00. Project/02. Scripts` 내 소스 코드를 전수 조사하여 입력 시스템의 인터페이스화와 인덱스 기반의 스택 위치 연산을 제안함. 이를 통해 전역 상태 오염을 방지하고 런타임 데이터 무결성을 확보함.
- **Decision**: 데이터 기반 상태 관리 방식을 통한 논리적 오류 원천 차단.

**Technical Requirement**:
> 장비 속성과 실행 로직의 분리를 위한 데이터 주입(Logic-Data Injection) 설계 및 확장성 확보 방안 모색.

**Engineering Response**:
- **Engineering Summary**: 장비를 동작 파라미터 공급자(MiningData Provider)로 재정의하는 [Logic-Data Injection] 설계를 제안함. 장비 종류에 따른 가변성을 코드 수정 없이 데이터 설정만으로 대응할 수 있는 확장성을 확보함.
- **Decision**: 확장성 극대화를 위한 ScriptableObject 기반 데이터 주입 아키텍처 채택.

**Technical Requirement**:
> 범죄자 대기 및 수감 시스템 구축을 위한 중앙 통제형 아키텍처(Producer-Consumer) 설계 및 단일 코루틴 FSM 구조 제안 요청.

**Engineering Response**:
- **Engineering Summary**: 범죄자 대기 및 수감 시스템을 위해 [Centralized Producer-Consumer Architecture]를 설계함. `WaitingLineManager`를 통한 O(1) 인덱스 기반 좌표 제어와 범죄자 AI의 단일 코루틴 FSM 구조를 통해 성능과 메모리 안정성을 확보함.
- **Decision**: 시스템 부하 최소화를 위한 중앙 관리형 대기열 알고리즘 도입.

**Technical Requirement**:
> 전체 코드베이스의 중복성 제거 및 확장성 극대화를 위한 컴포넌트 주도형 데이터 주입(Component-Driven Data Injection) 리팩토링 로드맵 수립 요청.

**Engineering Response**:
- **Engineering Summary**: 프로젝트 전수 조사를 통해 중복된 채굴 로직과 강한 컴포넌트 결합을 식별함. ScriptableObject 기반의 데이터 분리와 중앙 집중형 `MiningController` 통합을 포함한 4단계 리팩토링 로드맵을 수립함.
- **Decision**: 시스템 확장성 확보를 위한 체계적 리팩토링 프로세스 정립.

**Technical Requirement**:
> 런타임 메모리 파편화 방지 및 CPU 피크 억제를 위한 풀링 시스템(Object Pooling) 통합 전략 수립 및 라이프사이클 관리 표준화 요청.

**Engineering Response**:
- **Engineering Summary**: 사용자 구현 `PoolManager`를 시스템 전반에 통합하기 위한 전략을 수립함. `IPickupAble` 인터페이스 확장을 통한 라이프사이클 관리 표준화를 제안하여 런타임 CPU 피크를 억제하고 메모리 무결성을 보장함.
- **Decision**: 리소스 관리 효율화를 위한 전역 오브젝트 풀링 아키텍처 통합.

**Technical Requirement**:
> 모듈화 및 싱글톤 구조 개편을 포함한 [Phase 1: Architecture Hardening] 실행 요청. 상태 복구를 위한 원자적 변경 관리 병행.

**Engineering Response**:
- **Engineering Summary**: 프로젝트 모듈화 및 안정성 강화를 위한 리팩터링을 수행함. 4개의 독립 어셈블리를 구축하여 물리적 의존성을 격리하였으며, SingletonBase를 [Fail-Fast] 구조로 개편하여 런타임 오버헤드 제거 및 초기화 무결성을 확보함.
- **Decision**: 견고한 개발 기반 마련을 위한 아키텍처 하드닝 수행.

**Technical Requirement**:
> 모듈 간 순환 참조(Circular Dependency) 해결을 위한 인터페이스 추상화 및 의존성 역전(DIP) 적용 요청.

**Engineering Response**:
- **Engineering Summary**: Core 모듈이 InGame 모듈을 참조하는 순환 참조 문제를 인터페이스 추상화를 통해 해결함. IMiningTarget의 반환 타입을 상위 인터페이스로 변경함으로써 모듈 간 물리적 의존성을 제거함.
- **Decision**: 의존성 역전 원칙(DIP)을 통한 시스템 간 결합도 해소.

**Technical Requirement**:
> 인터페이스 주도형 아키텍처(Interface-Driven Architecture) 전환을 통한 모듈 간 결합도 제거 및 SRP 준수 강화 요청.

**Engineering Response**:
- **Engineering Summary**: SOLID 원칙 기반의 [Interface-Driven Architecture]를 적용함. IPlayer 인터페이스를 통해 GameManager와 PlayerController 간의 결합을 끊었으며, 모듈 성격에 맞춰 책임을 재배치하여 SRP를 강화함.
- **Decision**: 아키텍처 유연성 확보를 위한 인터페이스 기반 설계 정착.

**Technical Requirement**:
> 공간 효율성 증대를 위한 3차원 격자 적층(Grid Stacking) 알고리즘 도입 요청.

**Engineering Response**:
- **Engineering Summary**: ItemStacker에 3차원 격자 적층(Grid Stacking) 알고리즘을 도입함. 행/열/간격 설정을 통해 다양한 적재 레이아웃을 지원하며, 인덱스 기반의 좌표 계산 로직을 통해 시각적 완성도를 높임.
- **Decision**: 물리적 적재 한계 극복을 위한 그리드 알고리즘 구현.

**Technical Requirement**:
> 고수준의 물리적 적층 정합성 정밀화 및 상호작용 로직의 모듈화(StackInteractionZone 추출) 요청.

**Engineering Response**:
- **Engineering Summary**: ItemStacker의 좌표 계산 로직을 정밀화하여 비주얼 스냅 버그를 해결함. 전용 상호작용 브리지인 [StackInteractionZone]을 신규 구축하여 가마솥에 종속된 로직을 범용 모듈로 추출하여 확장성을 확보함.
- **Decision**: 상호작용 로직의 일반화를 통한 시스템 재사용성 증대.

**Technical Requirement**:
> NPC 공통 기반(Abstract Foundation) 구축 및 이원화된 대기 시스템(WaitingLine & PrisonManager) 설계 요청.

**Engineering Response**:
- **Engineering Summary**: 모든 NPC의 공통 이동 및 시각 로직을 담당하는 [Abstract Npc Foundation]을 구축함. 포션 테이블 앞의 정렬 대기열과 감옥 수용량 관리 매니저를 구축하여 체계적인 NPC 흐름 제어 시스템을 완성함.
- **Decision**: NPC 시스템의 일관성 확보를 위한 추상화 레이어 구축.

**Technical Requirement**:
> 리소스 효율 극대화를 위한 UI 임대 시스템(UI Leasing) 구축 및 상태 기반 가시성 제어 요청.

**Engineering Response**:
- **Engineering Summary**: 단일 PrisonerUI를 여러 NPC가 공유하는 [UI Leasing] 시스템을 구축함. 정지 상태에서만 UI를 요청하고 활성 타겟에게만 집중시키는 로직을 통해 런타임 드로우 콜과 메모리 오버헤드를 최적화함.
- **Decision**: UI 성능 최적화를 위한 인스턴스 공유 및 상태 기반 제어 전략 수립.

**Technical Requirement**:
> 성장 피드백 강화를 위한 전략 패턴 기반 해금 시스템(Strategy-Based Unlock) 아키텍처 설계 요청.

**Engineering Response**:
- **Engineering Summary**: 다양한 보상 유형을 유연하게 처리하기 위한 [Strategy-Based Unlock System] 아키텍처를 설계함. `UnlockZone`과 `IUnlockReward` 인터페이스를 통해 보상 로직을 캡슐화하여 시스템 간 결합도를 낮춤.
- **Decision**: 콘텐츠 확장성 보장을 위한 보상 시스템의 인터페이스 추상화.

**Technical Requirement**:
> 런타임 오버헤드 최소화를 위한 클래스 캡슐화 상태 머신(State Pattern NPC Framework) 리팩터링 요청.

**Engineering Response**:
- **Engineering Summary**: 런타임 가비지(GC) 최소화와 로직 가독성 확보를 위해 [State Pattern NPC Framework] 리팩터링을 수행함. Flyweight 패턴을 적용해 상태 객체를 재사용함으로써 매 프레임 발생하는 메모리 할당을 억제함.
- **Decision**: 고성능 AI 구현을 위한 객체 지향형 상태 머신 도입.

**Technical Requirement**:
> 고주파 연출과 논리적 상태의 불일치 해결을 위한 가상 점유 예약 시스템(Virtual Occupancy) 설계 요청.

**Engineering Response**:
- **Engineering Summary**: 고주파 채굴 시 발생하는 연출 지연과 상태 불일치를 해결하기 위해 [Virtual Occupancy & Slot Reservation] 시스템을 구축함. `pendingCount` 필드를 통해 비행 중인 자원을 예약 관리하여 데이터 무결성을 보장함.
- **Decision**: 물리 연출 지연(Latency) 극복을 위한 논리적 슬롯 예약 메커니즘 도입.

---

## [2026-04-06] Current Progress

**Technical Requirement**:
> 저비용/고효율 오디오 피드백 구축을 위한 단일 소스 기반 사운드 매니저(AudioManager) 설계 요청.

**Engineering Response**:
- **Engineering Summary**: 단일 `AudioSource` 기반의 `PlayOneShot` 믹싱 아키텍처를 도입하여 런타임 오디오 채널 부하를 최소화함. 데이터 중심 설계(`SoundDataSO`)를 통해 로직과 사운드 에셋의 의존성을 격리함.
- **Decision**: 자원 효율성을 고려한 싱글 채널 오디오 관리 시스템 확립.

**Technical Requirement**:
> 해금 이벤트의 시각적 강조를 위한 보상-카메라 연동 시네마틱 연출 구현 요청.

**Engineering Response**:
- **Engineering Summary**: `UnlockZone` 클래스에 카메라 연출 트리거를 통합하여 모든 해금 구역에서 시네마틱 피드백을 지원함. 기존 `CameraDirector` 인터페이스를 재사용하여 조이스틱 입력 차단과 연출의 동기화 무결성을 확보함.
- **Decision**: 유저 몰입감 향상을 위한 이벤트 기반 시네마틱 카메라 시스템 구축.

**Technical Requirement**:
> 오디오 피드백의 정밀도 향상을 위한 자원/동작별 사운드 세분화 및 동적 분기 로직 적용 요청.

**Engineering Response**:
- **Engineering Summary**: `SoundType` Enum을 세분화하고 각 시스템에서 장비 타입 및 아이템 타입을 체크하여 사운드를 동적으로 분기함. 기획 의도에 부합하는 정밀한 오디오 피드백 루프를 구축함.
- **Decision**: 사용자 경험(UX) 고도화를 위한 상황 인지형 오디오 피드백 정교화.

**Technical Requirement**:
> 시스템 유연성 및 성능 극대화를 위한 3단계 대규모 리팩터링(AI 상태 패턴, 데이터 주도형 자원, 연출 레이어 분리) 수행 요청.

**Engineering Response**:
- **Engineering Summary**: SOLID 원칙에 기반하여 1) Prisoner AI 상태 패턴 이식, 2) ItemDataSO 중심 데이터 주도형 자원 시스템 구축, 3) UnlockZone 연출 이벤트를 통한 프레젠테이션 레이어 분리를 완료함.
- **Decision**: 대규모 리팩터링을 통한 시스템 확장성 및 런타임 성능(Zero-GC) 극대화.

**Technical Requirement**:
> 런타임 자원 적재 결함 진단 및 데이터 바인딩 무결성 복구 요청.

**Engineering Response**:
- **Engineering Summary**: `ItemStacker`의 검증 로직을 '객체 참조'에서 'Enum ID 기반'으로 변경하여 런타임 생성 객체의 데이터 바인딩 누락 문제를 해결함. `Npc.cs` 내 `DOLookAt` 거리 임계값 체크를 추가하여 런타임 예외를 방지함.
- **Decision**: 시스템의 견고함(Robustness) 확보를 위한 예외 처리 및 검증 로직 강화.
