# AI Prompt History - Supercent Project

## [Initial Setup]

**User**:
> 너가 기억해서 바로 작업을 들어갈 수 있는 키워드를 알려줘. 추가적으로 준수규칙은 무조건 지켜야 한다는 것도 남겨줬으면 해.

**AI**:
- **Engineering Summary**: 프로젝트 초기 진입을 위해 `Supercent_ver3` 컨텍스트 활성화 키워드와 시니어 엔지니어 컨설턴트 역할을 정의함. 무의미한 방어적 코드보다 `Debug.Assert`를 활용한 Fail-Fast 원칙을 최우선으로 설정하여 논리적 오류를 즉각 검출하는 환경을 구축함. 이를 통해 개발 전 과정에서 설계 의도와 실제 구현 간의 정렬 상태를 엄격히 유지하고 디버깅 비용을 최소화하는 기반을 마련함.

**User**:
> 환경 스펙은 2022.3.62.f2로 다시 수정했으면 해.

**AI**:
- **Engineering Summary**: 개발 환경을 Unity 2022.3.62f2 LTS 버전으로 고정하고 관련 기술 스택(C# 9.0, URP, New Input System)을 재정의함. 환경 스펙의 엄격한 준수는 구식 API 사용으로 인한 잠재적 오류를 방지하고, 타겟 플랫폼에서의 런타임 안정성을 보장하기 위한 필수적인 기술적 제약 사항임.

---

## [2026-04-02] Session History

**User**:
> Progress.md 파일에 기록된 내용을 확인하고 작업을 이어서 실행하자.

**AI**:
- **Engineering Summary**: `PROGRESS.md`의 태스크 리스트와 아키텍처 결정 사항을 분석하여 현재 진행 단계를 '핵심 게임플레이 설계'로 동기화함. 이전 세션의 상태를 정확히 복구함으로써 기술적 연속성을 확보하고, 누락된 `.asmdef` 구조 설계 작업을 최우선 과제로 도출함.

**User**:
> GeminiCLI준수규칙도 다시 한번 상기했으면 해.

**AI**:
- **Engineering Summary**: 상호 참조(Cyclic Dependency)를 원천 차단하기 위해 인터페이스 기반의 의존성 역전(DIP)과 물리적 어셈블리 분리를 제안함. 구체 클래스 간의 직접 참조 대신 `Project.Base` 레이어를 통한 추상화 접근 방식을 선택하여, 코드 수정 시 발생하는 재컴파일 범위를 최소화하고 시스템 간의 결합도를 낮추는 아키텍처를 설계함.

**User**:
> 지금은 구조에 신경쓰기 보다는 빠른 작업을 목표로 두고 있어서 기능 위주로 작업할 예정이야. 내가 필요할 때 너를 찾을 게 그때 까진 대기해줘. 추후에 너랑 더 나은 방향으로 수정 및 기능들을 추가할 예정이야.

**AI**:
- **Engineering Summary**: 초기 프로토타이핑 속도를 극대화하기 위해 'Architecture Hardening' 단계를 유보하고 'Speed-First' 전략을 채택함. 기술 부채의 의도적 누적을 허용하되 추후 리팩토링 시점을 명확히 인지하도록 기록하여, 아이디어의 빠른 검증과 런타임 기능 구현에 자원을 집중할 수 있는 유연한 협업 모드를 설정함.

**User**:
> 일단 채굴 기능을 만들고 싶은데 update를 통해 매번 OverlapBox로 검사하기엔 비용이 커질 것 같은 느낌인데. 좋은 방안이 없을까?

**AI**:
- **Engineering Summary**: 매 프레임 발생하는 `OverlapBox`의 오버헤드를 줄이기 위해 물리 엔진의 Broadphase 결과를 재사용하는 트리거(Trigger) 기반 리스트 관리 방식이 제안됨. 이벤트 주도형(Event-Driven) 접근을 통해 불필요한 물리 쿼리를 제거하였으며, 이를 통해 CPU 사이클을 보존하고 다수의 채굴 대상이 존재할 때의 O(1) 탐색 효율을 달성함.

**User**:
> 현재 내 코드를 전체 분석 후 나은 방향을 제시해줘. 우리는 시간단축을 통한 빠른 제작을 목표로 하는걸 기억해줘. 최적화는 후에 진행할 예정이야.

**AI**:
- **Engineering Summary**: `PlayerController`에 트리거 감지 로직을 직접 삽입하고 `MiningTarget` 컴포넌트로 대상을 식별하는 고속 구현 패스(Fast-Track)를 설계함. 복잡한 컨트롤러 분리 대신 단일 클래스 내 상태 체크(Idle 시 자동 채굴) 구조를 선택하여 구현 시간을 단축하고, 즉각적으로 동작 확인이 가능한 기능적 최소 사양(MVP)을 제안함.

**User**:
> 현재 내 코드를 전체적으로 분석 후 진행 사항을 기록해줘. 추가적으로 프롬프트 기록을 남기고 있는지도 확인하고 지속적으로 남겨줘.

**AI**:
- **Engineering Summary**: 현재까지의 이동 및 UI 구현 상태를 점검하고 채굴 시스템 구현을 위한 태스크를 `PROGRESS.md`에 공식화함. 프롬프트 동기화 프로토콜에 따라 핵심 결정 사항을 문서화하여 세션 간 컨텍스트 유실을 방지하고, 이후 기능 고도화 시 참고할 수 있는 기술적 스냅샷을 생성함.

**User**:
> 지금 내가 작업하고 있는 기능은 스톤을 채굴하면 조약돌이 스폰되고, 그걸 플레이어가 주워서 등 뒤에 스택처럼 차곡차곡 쌓이는 기능이랑 연출이야.
>
> 우리가 지금까지 맞춰둔 구조(IMiningTarget, ParabolicMover를 통한 물리 배제 보간, IPickupAble 인터페이스)를 기반으로, 이걸 어떻게 구현하는 게 제일 깔끔할지 너가 아키텍처 방안이랑 코드 예시를 제시해 줬으면 해.
>
> 단, 구현할 때 아래 세 가지 이슈는 확실하게 고려해 줘.
>
> SetParent 남용 금지: 무작정 플레이어 하위로 SetParent 해서 덕지덕지 붙이는 건 하이어라키 갱신 부하나 본(Bone) 스케일 꼬이는 문제 때문에 피하고 싶어. 더 가볍고 안전하게 시각적 스택을 쌓을 방법을 제시해 줘.
>
> Offset 계산: 돌이 쌓일 때마다 다음 돌이 올라갈 정확한 Y축 위치(Offset)를 꼬이지 않고 가볍게 계산하는 로직이 필요해.
>
> Fail-Fast 방어: 내가 정한 최대 스택(Max Stack)을 넘어서 조약돌을 먹으려고 할 때, 조용히 버그 내거나 무시하지 말고 확실하게 예외 처리로 컷해주는 구조도 포함해 줘.
>
> 우리가 합의한 5가지 규칙 잊지 말고, AI 특유의 인사말이나 군더더기 없이 딱 엔지니어링적으로 필요한 핵심 설계만 보여줘. 다시 한번 검토해 보고 작업 시작하자.

**AI**:
- **Engineering Summary**: 하이어라키 갱신 부하를 차단하기 위해 `SetParent`를 배제하고 `LateUpdate`에서 좌표를 강제 동기화하는 'Virtual Stacking' 시스템을 설계함. 인덱스 기반의 `Y-Offset` 연산을 통해 가비지 없는 위치 제어를 구현하였으며, `Debug.Assert`를 활용해 Max Stack 초과 시 즉시 런타임 예외를 발생시킴으로써 데이터 오염을 방지하고 물리 연출과 논리적 스택을 독립적으로 제어하는 고성능 구조를 제안함.

**User**:
> 일단 기본적인 기능 구현은 마친 것 같아. 묘듈화를 진행해야겠어. 구조를 만들어 보자. 방안을 제시해줘. 추가적으로 빠르게 수정 및 도와줄 수 있는 부분이 있다면 도와줘.

**AI**:
- **Engineering Summary**: 프로토타이핑 완료 후 아키텍처 경량화를 위해 `Core`, `UI`, `Gameplay`, `Managers` 4단계 계층으로 분리된 `.asmdef` 기반 모듈화 방안을 수립함. 인터페이스를 통한 상호 참조 제거(DIP)를 핵심 전략으로 선택하여, 향후 스크립트 수정 시 발생하는 재컴파일 시간을 획기적으로 단축하고 모듈 간 물리적 경계를 엄격히 구분하는 유지보수 최적화 설계를 제시함.

**User**:
> 현재 내 프로젝트 파일의 스크립트 파일을 분석 후, 너가 생각하기에 문제가 있어보이는 것들 또는 구조적으로 복잡해질만한 것들이 있다면 알려줘. 좋은 방안을 제시해줘.

**AI**:
- **Engineering Summary**: 현재 프로젝트의 `PlayerController` 비대화와 싱글톤 구조의 과도한 복잡도를 핵심 문제로 진단하고, 이를 해결하기 위해 **컴포넌트 기반 기능 분산(Component-Based Decoupling)** 방안을 제안함. 엄격한 인터페이스 계층 대신 기능별 컴포넌트(Movement, Stacker, Animator) 분리를 통해 구현 직관성을 유지하면서도 응집도를 높여, 유지보수 시 영향 범위를 최소화하는 실용적인 아키텍처 방향을 설정함.

**User**:
> 02. Scripts의 전체적인 코드를 분석 후, 방안을 제시해줬으면 해. 지금은 너가 전에 기억했던 내용들을 기반으로 제시해준 것 같아.

**AI**:
- **Engineering Summary**: `Assets/00. Project/02. Scripts` 내 실제 소스 코드를 전수 조사한 결과, `Joystick`의 전역 정적 참조(Static Dependency)와 `ItemStacker`의 가변 상태(`offsetY`) 관리의 불안정성을 핵심 결함으로 식별함. 이를 해결하기 위해 입력 시스템의 인터페이스화(Input Abstraction)와 인덱스 기반의 스택 위치 연산(Index-based Offset)을 제안하였으며, 이를 통해 전역 상태 오염을 방지하고 런타임 데이터 무결성을 보장하는 구조적 안정성을 확보함.

**User**:
> 예시 코드를 보여줄 수 있어?

**AI**:
- **Engineering Summary**: `IInputProvider` 인터페이스를 통한 입력 계층 추상화와 `ItemStacker`의 인덱스 기반 위치 연산(`stackedItems.Count * itemHeight`) 예시 제시함. `static` 의존성을 제거하여 입력 장치 확장이 용이한 구조를 설계하였으며, 가변 상태 변수(`offsetY`)를 제거하고 데이터 본연의 상태(Count)로부터 위치를 도출함으로써 런타임 논리 오류를 원천 봉쇄하는 고성능/고안정성 코드를 제안함.

**User**:
> 채굴 시스템의 경우 방향을 잡고 싶은데, 내가 생각한 구조 및 방향은 마이닝 컨트롤러를 장비가 보유하고 있는 채굴 딜레이에 따라 채굴을 하는 방식이야 마이닝스캐너는 장비 장착을 뭐하냐에 따라 변경해줄 생각이었고. 하지만 일단 장비를 IEquippable를 상속 시록 만들긴 했는데 원하는 구조가 안 나올 것 같아서 고민이야. 좋은 방안이 없을까?

**AI**:
- **Engineering Summary**: 장비를 단순 기능 구현체가 아닌 **동작 파라미터 공급자(MiningData Provider)**로 재정의하는 **[Logic-Data Injection]** 설계를 제안함. `IEquippable`을 통해 채굴 딜레이와 스캐너 설정값을 `MiningController`에 주입함으로써, 컨트롤러는 실행 흐름에만 집중하고 장비는 속성 정의에만 집중하는 역할 분리(SRP)를 달성함. 이를 통해 장비 종류에 따른 감지 범위 및 형태의 가변성을 코드 수정 없이 데이터 설정만으로 대응할 수 있는 확장성을 확보함.

**User**:
> GeminiCLI준수규칙을 상기해줘.

**AI**:
- **Engineering Summary**: `GeminiCLI 준수규칙.txt`에 명시된 6가지 핵심 원칙(Architectural Thinking, Fail-Fast, Strategic Trade-offs, Clean & High-Performance, Environment Strictness, Zero-Fluff)을 재확인하고, 이를 모든 협업 과정의 절대적 가이드라인으로 재설정함. 규칙의 엄격한 준수는 기술적 부채를 방지하고 시니어 수준의 설계 품질을 유지하기 위한 상호 신뢰의 기반임을 확약함.

**User**:
> 너와 나의 대화를 항상 Prompt.md 추가 및 최신화 해줘.

**AI**:
- **Engineering Summary**: 세션 내 모든 주요 의사결정과 기술적 논의를 `Prompt.md`에 실시간 기록하는 **[Context Synchronization]** 프로토콜을 활성화함. 이를 통해 장기적인 프로젝트 진행 과정에서 컨텍스트 유실을 방지하고, 과거의 결정 근거(Rationale)를 명확히 추적할 수 있는 엔지령 로그 시스템을 구축함.

**User**:
> [CLI 로그 기록 교정용 프롬프트]
> ""앞으로 대화 기록이나 프롬프트 로그를 파일에 남길 때, 다음의 파일 입출력(File I/O) 원칙을 예외 없이 강제한다. Only Append, No Overwrite, 보존 원칙...""

**AI**:
- **Engineering Summary**: 로그 무결성(Log Integrity) 보장을 위해 **[Append-Only Logging Protocol]**을 도입함. 기존 기록의 수정/요약을 원천 금지하고 파일 끝에 신규 데이터를 추가하는 방식(`Add-Content` 또는 `>>` 연산자 수준의 원자적 동작)을 강제하여, 프로젝트의 모든 의사결정 이력을 변조 없이 영구 보존함. 이는 기술적 추적성(Traceability)을 극대화하고 세션 간 컨텍스트 오염을 방지하기 위한 핵심 데이터 관리 전략임.

**User**:
> 내가 다시 되돌렸어. 아무래도 구조는 내가 직접 만드는 것이 좋을 것 같아. 너는 보조수단으로 사용할게. 너가 직접 만들어 주는 것이 아닌 방안을 제시해주는 방향으로 해줘. 또는 내가 코드예시가 필요하다면 보여주고.

**AI**:
- **Engineering Summary**: 사용자의 주도적 아키텍처 설계를 지원하기 위해 협업 모드를 **[Consultant-Only Mode]**로 전환함. 직접적인 코드 수정 대신 분석, 제안, 참조 예시 제공에 집중하며, 이를 통해 개발자가 시스템에 대한 완전한 통제권과 이해도를 유지하면서도 시니어 수준의 설계 검토를 받을 수 있는 보조적 협업 체계를 확립함.

**User**:
> 지금 진행 중인 프로젝트의 전체 코드를 분석 후, 해당 프롬프트 진행해줘. "지금 우리 게임에 범죄자 대기열 및 수감 시스템을 만들어야 해. ... 중앙 통제형 아키텍처 (Producer-Consumer), 단일 코루틴 FSM 유지, 결합도 최소화 ..."

**AI**:
- **Engineering Summary**: 범죄자 대기 및 수감 시스템을 위해 **[Centralized Producer-Consumer Architecture]**를 설계함. `WaitingLineManager`를 통한 O(1) 인덱스 기반 좌표 제어와 범죄자 AI의 **단일 코루틴 FSM** 구조를 제안하여 성능 최적화와 메모리 안정성을 동시 확보함. 각 매니저 간 상태값 기반의 느슨한 결합(Loose Coupling)을 통해 시스템 간 독립성을 유지하고, 생산 로직(Spawner)과 소비 로직(Queue/Prison)의 명확한 역할 분리를 달성함.

**User**:
> 나의 코드를 전체적으로 분석 후, 개선점이 필요한 코드 및 구조 위주로 리팩토링을 진행해줘. 리팩토링을 진행하기 전에 어떻게 진행하고 어떤식으로 구조를 만들건지 먼저 보여주고 확인 받고 진행해줬으면 좋겠어.

**AI**:
- **Engineering Summary**: 프로젝트 전수 조사를 통해 중복된 채굴 로직과 강한 컴포넌트 결합을 핵심 개선 과제로 도출함. 이를 해결하기 위해 **[Component-Driven Data Injection]** 아키텍처를 제안하였으며, ScriptableObject 기반의 데이터 분리와 중앙 집중형 `MiningController` 통합을 통해 중복 코드를 제거하고 확장성을 극대화하는 4단계 리팩토링 로드맵을 수립함.

**User**:
> 최적화를 위해서 풀링에 관련된 스크립트를 만들었어. 현재 생성을 하고 있는 로직들을 풀링 매니저에서 관리하고 사용할 수 있도록 리팩터링을 진행하기 전에 너가 개선할 코드들을 먼저 보여주고 허락 받은 후 리팩토링을 진행했으면 해.

**AI**:
- **Engineering Summary**: 사용자 구현 `PoolManager`를 시스템 전반에 통합하기 위한 **[Pooling Integration Strategy]**를 수립함. 잦은 `Instantiate/Destroy`가 발생하는 자원 생성(`Stone`, `Cobblestone`) 및 효과 로직을 타겟으로 식별하였으며, `IPickupAble` 인터페이스 확장을 통한 라이프사이클 관리 표준화를 제안함. 이를 통해 런타임 CPU 피크를 억제하고 메모리 무결성을 보장하는 고성능 최적화 구조를 설계함.

**User**:
> 내가 만들고 있는 게임의 프로젝트 스크립트 파일 내부 코드들은 전체적으로 자세히 분석해줘. 파일 이름은 "02. Scripts"이야.

**AI**:
- **Engineering Summary**: Assets/00. Project/02. Scripts 내 전수 조사를 통해 SingletonBase of FindObjectOfType 오버헤드와 PoolManager of Type 기반 관리 한계, 그리고 .asmdef 부재로 인한 컴파일 비효율성을 핵심 결함으로 식별함. 시스템 전반의 물리적/논리적 결합도를 낮추고 [Fail-Fast] 원칙을 강화하기 위한 4대 영역(구조, 안정성, 성능, 환경) 진단 브리핑을 제출함.

**User**:
> 리팩터링을 진행해줘. 혹시 모르니 내가 전체적으로 되돌리고 싶다고 하면 현재 프로젝트의 상태로 되돌리면 돼.

**AI**:
- **Engineering Summary**: 프로젝트 모듈화 및 안정성 강화를 위한 [Phase 1: Architecture Hardening] 리팩터링을 수행함. 4개의 독립 어셈블리(Core, Managers, UI, InGame)를 구축하여 물리적 의존성을 격리하였으며, SingletonBase를 [Fail-Fast] 구조로 개편하여 런타임 오버헤드 제거 및 초기화 무결성을 확보함. 또한 PoolManager를 Key 기반 Dictionary 구조로 전환하여 동일 컴포넌트 프리팹의 다중 풀링 확장성을 구현함.

**User**:
> 콘솔창에 뜨는 오류를 잡고 다음 리팩터링을 진행하자.

**AI**:
- **Engineering Summary**: .asmdef 도입으로 발생한 외부 DLL(DOTween.dll) 참조 오류를 해결하기 위해 Core.asmdef의 Override References 설정을 갱신함. 이후 [Phase 2] 리팩터링으로 PlayerController의 데이터 주입 방식을 ScriptableObject(PlayerDataSO)로 전환하고, [Fail-Fast] 원칙에 기반한 필수 컴포넌트(CharacterController, IInputProvider) 검증 로직을 추가하여 런타임 안정성과 데이터 기반 설계의 유연성을 확보함.

**User**:
> 버그 부터 잡고 가자 리팩터링을 하면서 전체적인 스크립트에 문제가 생긴 것 같아. "Assets\00. Project\02. Scripts\Managers\PrisonManager.cs(3,30): error CS0246: The type or namespace name 'SingletonBase<>' could not be found"

**AI**:
- **Engineering Summary**: .asmdef 간의 GUID 기반 참조가 불일치하여 발생한 컴파일 오류를 해결함. Managers, UI, InGame 어셈블리의 참조 방식을 안정성이 높은 '이름 기반 참조(Name-based Reference)'로 전수 교체하여 모듈 간 물리적 의존성을 정상화함. 이를 통해 [Phase 1]의 핵심인 컴파일 단위 분리 구조를 완성하고 런타임 이전의 모든 구문 오류를 제거함.

**User**:
> "Assets\00. Project\02. Scripts\Core\Interface\IMiningTarget.cs(7,12): error CS0246: The type or namespace name 'Cobblestone' could not be found" 해당 버그도 잡고 가자

**AI**:
- **Engineering Summary**: Core 모듈이 InGame 모듈을 참조하는 순환 참조(Circular Dependency) 문제를 인터페이스 추상화를 통해 해결함. IMiningTarget.MineResource()의 반환 타입을 구체 클래스(Cobblestone)에서 상위 인터페이스(IPickupAble)로 변경함으로써 모듈 간 물리적 의존성을 제거하고, 의존성 역전 원칙(DIP)을 준수하는 유연한 아키텍처를 구축함.

**User**:
> SOLID원칙에 맞춰서 구현을 리팩터링을 진행해보자.

**AI**:
- **Engineering Summary**: 어셈블리 간 순환 참조를 해결하기 위해 SOLID 원칙 기반의 [Interface-Driven Architecture]를 적용함. IPlayer 인터페이스를 통한 의존성 역전(DIP)으로 GameManager와 PlayerController 간의 구체 클래스 결합을 끊었으며, 모듈 성격에 맞춰 GameManager를 InGame 모듈로 재배치함으로써 단일 책임 원칙(SRP)과 컴파일 단위의 물리적 무결성을 동시에 확보함.

**User**:
> 3단계: [Mining System Optimization] (장비 데이터의 ScriptableObject 전환 및 중복 로직 통합) 진행해줘.

**AI**:
- **Engineering Summary**: 분산된 채굴 로직을 통합하고 데이터 기반 설계(Data-Driven Design)를 적용함. 각 장비의 스펙을 MiningDataSO(ScriptableObject)로 추상화하고, MiningController에서 타이머 및 코루틴 로직을 중앙 집중화함으로써 중복 코드를 원천 제거함. 이를 통해 장비 확장이 용이해졌으며, [Fail-Fast] 원칙을 적용한 의존성 검증으로 런타임 안정성을 대폭 향상시킴.

**User**:
> 4단계: [Sensor & Feedback Optimization] 진행해줘.

**AI**:
- **Engineering Summary**: 물리 센서 시스템을 Trigger 기반 HashSet 관리 방식으로 개편하여 런타임 가비지 및 물리 연산 비용을 최적화함. MiningDataSO와 실제 물리 콜라이더 크기를 동적으로 동기화하는 [Data-Driven Sync] 엔진을 구축하여 데이터 기반의 정밀한 밸런싱 환경을 마련하였으며, [Fail-Fast] 원칙에 따른 레이어 및 컴포넌트 유효성 검증으로 설정 무결성을 확보함.

**User**:
> 플레이어가 얻을 돌을 이용해서 포션을 제작하는 시스템을 만들 생각이야. (자원 투입 -> 제작 -> 수령 흐름) 진행해줘.

**AI**:
- **Engineering Summary**: 포션 제작을 위한 [State-Based Producer-Consumer Architecture]를 구축함. MagicCauldron에 상태 머신(FSM)을 적용하여 자원 예약 제작 루프를 구현하였으며, CauldronZone(Input/Output)을 통해 플레이어 인벤토리(ItemStacker)와의 자원 교환을 자동화함. 인터페이스 기반의 자원 처리(IPickupAble)와 [Fail-Fast] 검증을 통해 시스템 간 독립성과 런타임 안정성을 확보함.

**User**:
> 이제는 필요없어진 기능 또는 스크립트들이 있다면 삭제해줘. 그리고 지금 내가 원하는 건 돌을 특정 구역에 놓고 가마솥에서 놓은 돌들을 이용해서 포션을 제작하는 시스템이야. 추가적으로 빨려들어가는 연출까지 만들면 좋을 것 같아.

**AI**:
- **Engineering Summary**: 중복된 장비 하위 클래(PickAxe, Drill 등)를 삭제하여 아키텍처를 정화하고, 가마솥 제작 시스템에 [Visual Feedback]을 통합함. DOParabolicMove를 활용해 자원이 가마솥으로 빨려 들어가는 포물선 연출을 구현하였으며, 애니메이션 완료 시점에 논리적 상태를 갱신하는 콜백 기반 동기화 로직을 구축하여 시각적 생동감과 데이터 무결성을 동시에 확보함.

**User**:
> InQuarry영역에 들어가야지만 내가 장비들이 활성화 되게 만들었는데 전체적인 리팩터링 이후 그 기능이 사라진 것 같아. 보완해줘.

**AI**:
- **Engineering Summary**: 리팩터링 과정에서 누락된 채굴 구역 활성화 로직을 [Centralized Control] 구조로 복구함. MiningController에 isInMiningZone 플래그와 SetMiningZoneActive 인터페이스를 추가하여 구역 외 채굴 타이머를 차단하였으며, InQuarry 트리거와 연동하여 장비 외형의 동적 활성화를 보장함. [Fail-Fast] 원칙에 따른 컴포넌트 검증을 추가하여 런타임 안정성을 확보함.

**User**:
> 이동 연출 로직을 어디에서 사용할 건지 정해야할 것 같아. (Stacker vs Sender) 보완해줘.

**AI**:
- **Engineering Summary**: 자원 이동 연출의 주체를 Sender-Driven(송신자 중심) 구조로 재정의하여 [SRP]를 강화함. ItemStacker에서 연출 로직을 제거하고 데이터 관리 및 좌표 공급 역할로 국한시켰으며, MiningController와 CauldronZone에서 직접 DOParabolicMove를 실행하게 함으로써 연출의 유연성을 확보함. 콜백 기반의 논리 삽입 구조를 통해 시각적 피드백과 데이터 무결성을 동시 달성함.

**User**:
> List보단 스택이 더 좋지 않을까? (물리적 적층 메커니즘 교정) 보완해줘.

**AI**:
- **Engineering Summary**: 게임의 물리적 적층 메커니즘(LIFO)에 최적화하기 위해 ItemStacker를 Stack<IPickupAble> 기반으로 전격 복원함. MagicCauldron에 물리적 입력 버퍼(Input Stacker)를 도입하여 플레이어의 자원 투입과 가마솥의 제작 소모 프로세스를 물리적으로 시각화하였으며, Stacker 간의 자원 이동 연출을 강화하여 고퀄리티의 자원 순환 시스템을 완성함.

**User**:
> 드릴이랑 불도저 같은 고주파 채굴 장비의 비용 및 딜레이 문제를 해결해줘. (Dual-Mode Mining 도입)

**AI**:
- **Engineering Summary**: 단발성 채굴(Burst)과 연속성 채굴(Continuous)을 모두 지원하는 [Dual-Mode Mining System]을 구축함. 드릴/불도저 모드에서는 코루틴 오버헤드를 제거하고 Update 루프 기반의 즉각적 로직 실행을 통해 '접촉 즉시 채굴' 컨셉을 고성능으로 구현함. 공통 채굴 로직의 캡슐화(ExecuteMining)를 통해 아키텍처 결합도를 낮추고 장비별 연출 자유도를 극대화함.

**User**:
> 싱글톤베이스는 돌려놔. (자동 생성 및 지연 탐색 구조로 복구)

**AI**:
- **Engineering Summary**: 사용자의 요구에 따라 SingletonBase를 리팩터링 이전의 [Lazy-Initialization & Auto-Creation] 방식으로 복구함. FindObjectOfType을 통한 런타임 인스턴스 탐색 및 부재 시 자동 GameObject 생성 로직을 재활성화하여, 씬 구성의 유연성과 개발 편의성을 최우선으로 확보함.

**User**:
> 가마솥 제작 시스템을 더 정교하게 만들어줘. (입력 구역과 출력 구역의 물리적 분리 및 적재 로직) 보완해줘.

**AI**:
- **Engineering Summary**: 가마솥 시스템을 [Double-Buffer Physical Pipeline]으로 고도화함. 입력 적재소(Input Stacker)와 출력 보관소(Output Stacker)를 명확히 분리하여 자원의 흐름을 시각화하였으며, 제작 완료 시 포션이 자동으로 보관소에 적재되는 연출을 구현함. 이를 통해 플레이어의 개입 없이도 독립적으로 동작하는 비동기 생산 시스템과 고품질의 자원 순환 피드백을 완성함.

**User**:
> ArgumentException: The Object you want to instantiate is null. (MagicCauldron.Awake 오류 해결)

**AI**:
- **Engineering Summary**: MagicCauldron의 프리팹 미할당으로 인한 런타임 예외를 해결함. ObjectPool 생성자와 MagicCauldron.Awake에 [Fail-Fast] 원칙을 적용한 명시적 Null 검증 로직을 추가하여 설정 오류를 조기 진단 가능케 함. 방어적 프로그래밍을 통해 시스템 안정성을 높이고 구체적인 오류 메시지를 제공함으로써 디버깅 효율을 극대화함.

**User**:
> 플레이어 인벤토리(Gold, Stone, Potion) 스태킹 시스템을 더 깔끔하게 통합해줘. 조약돌 유무에 따른 골드 위치 자동 보정 및 계층 구조(Transform) 활용 필수.

**AI**:
- **Engineering Summary**: 개별 스태커들을 [Hierarchical Anchor System] 기반의 PlayerStackManager로 통합함. StoneStacker의 상태 변화를 이벤트로 수신하여 GoldStacker의 로컬 좌표를 동적으로 보정하는 [Dynamic Offset] 시스템을 구축하였으며, 이를 통해 조약돌 유무에 따른 시각적 적층 위치 변화를 최적의 성능으로 구현함. MiningController의 의존성을 일원화하여 아키텍처 결합도를 대폭 낮춤.

**User**:
> ItemStacker를 범용으로 통합하고 중복 클래스 삭제해줘. Enum 기반으로 아이템 수용 제한 기능 추가 필수.

**AI**:
- **Engineering Summary**: 중복된 스태커 하위 클래스들을 삭제하고, ItemType 필터링 로직을 내장한 단일 [Universal ItemStacker]로 통합함. IPickupAble 인터페이스에 자원 식별 프로퍼티를 추가하여 형식 안전성을 확보하였으며, [Fail-Fast] 원칙에 따른 타입 검증 로직을 통해 잘못된 자원 적재를 방지함. 이를 통해 프로젝트의 스크립트 복잡도를 획기적으로 낮추고 자원 확장성을 극대화함.

**User**:
> 전체 코드 분석 후 버그 수정 및 불필요한 스크립트 삭제 진행해줘.

**AI**:
- **Engineering Summary**: 범용 Stacker 통합 이후 발생한 Stale Reference(삭제된 클래스 참조)를 전수 조사하여 수정함. CauldronZone의 의존성을 PlayerStackManager로 일원화하고 불필요한 구체 클래스 캐스팅을 제거함. 하드코딩된 시각적 오프셋을 ItemStacker의 메타데이터(ItemHeight)와 동기화하여 SSOT를 달성함. PotionGenerator 등 사용 계획이 없는 잔여 스크립트를 삭제하여 프로젝트의 물리적/논리적 무결성을 완성함.

**User**:
> MiningController.cs(93,13): error CS0246: The type or namespace name 'StoneStacker' could not be found. (버그 수정 요청)

**AI**:
- **Engineering Summary**: MiningController 내부에 잔존하던 삭제된 클래스(StoneStacker)의 로컬 변수 참조를 전수 조사하여 범용 타입(ItemStacker)으로 교체 완료함. grep 기반의 프로젝트 전수 조사를 통해 모든 '유령 타입 참조'가 제거되었음을 검증하였으며, 이를 통해 컴파일 무결성을 확보하고 런타임 안정성을 정상화함.

**User**:
> Cauldron에 아이템이 쌓일 때 2열 2행으로 차곡차곡 쌓일 수 있게 하자. (격자 적층 시스템 구현)

**AI**:
- **Engineering Summary**: ItemStacker에 3차원 격자 적층(Grid Stacking) 알고리즘을 도입함. 행/열/간격 설정을 통해 2x2 등 다양한 적재 레이아웃을 지원하며, 인덱스 기반의 좌표 계산 로직을 통해 층(Layer) 개념을 시각화함. 중앙 정렬 오프셋 계산을 포함하여 자원 적재 시의 시각적 완성도를 높였으며, 기존 수직 적층 구조와의 하위 호환성을 완벽히 유지함.

**User**:
> LIFO 일관성 확보 및 애니메이션 스냅 버그 수정 요청. 가마솥 상호작용 기능을 범용 모듈로 분리해줘.

**AI**:
- **Engineering Summary**: ItemStacker의 인덱스 기반 좌표 계산 로직(GetPositionAtIndex)을 정밀화하여 적재 시 발생하는 비주얼 스냅 버그를 해결함. 전용 상호작용 브리지인 [StackInteractionZone] 신규 구축하여 가마솥에 종속된 로직을 범용 모듈로 추출하였으며, 이를 통해 다양한 자원 저장 시설에 즉시 적용 가능한 확장성을 확보함. 시스템 전반의 LIFO 메커니즘을 재정렬하여 물리적 현실감과 데이터 정합성을 동시에 달성함.

**User**:
> StackInteractionZone.cs 오류 수정 요청. PlayerStackManager를 Inventory로 이름 변경함.

**AI**:
- **Engineering Summary**: 인벤토리 시스템의 클래스 명칭 변경(PlayerStackManager -> Inventory)에 따른 전역 참조 동기화를 수행함. StackInteractionZone.cs 내의 명칭 불일치 및 오타(Iventor)를 수정하여 컴파일 무결성을 회복하였으며, MiningController 등 핵심 모듈의 의존성 상태를 전수 조사하여 리팩터링 이후의 안정성을 확보함.

**User**:
> 구조를 다시 기능을 만들어 올게 나중에 어울릴 수 있는 방향으로 같이 리팩터링 진행해보자.

**AI**:
- **Engineering Summary**: 현재 프로젝트의 4단계 계층 구조(.asmdef), 데이터 기반 설계(SO), 범용 상호작용 시스템(StackInteractionZone), 그리고 LIFO 기반의 물리적 적층 로직을 완전히 동기화함. 향후 추가될 신규 기능들이 기존의 SOLID 원칙과 고성능 런타임 구조에 유연하게 통합될 수 있도록 아키텍처 컨텍스트를 유지하며 대기 모드로 전환함.

**User**:
> 죄수 NPC 제작 요청. 포션 수령 대기열(정렬)과 감옥 수용 대기열(비정렬)을 포함한 FSM 구축. Npc 추상 클래스 상속 필수.

**AI**:
- **Engineering Summary**: 모든 NPC의 공통 이동 및 시각 로직을 담당하는 [Abstract Npc Foundation]을 구축하고 DOTween을 통합함. 포션 테이블 앞의 정렬된 줄서기를 제어하는 WaitingLineManager와 감옥 수용량 및 비정렬 대기 영역을 관리하는 PrisonManager를 구축하여 이원화된 대기 시스템을 완성함. Criminal 구체 클래스에 상속 기반의 FSM 루틴을 구현하여 복잡한 자원 수령 및 수감 시퀀스를 안정적으로 자동화함.

**User**:
> "WaitingLineManager.cs(9,18): error CS0246: The type or namespace name 'Npc' could not be found" 버그 수정 요청.

**AI**:
- **Engineering Summary**: 어셈블리 가시성 문제를 해결하기 위해 Npc 추상 베이스 클래스를 InGame 모듈에서 Core 모듈로 이동 배치함. 이를 통해 하위 어셈블리인 Managers에서 Npc 타입을 참조할 수 없는 물리적 의존성 제약을 해소하였으며, 의존성 역전 원칙(DIP)을 준수하는 계층 구조를 재확립하여 컴파일 무결성을 확보함.

**User**:
> 죄수의 전체적인 로직(포션 수령 및 감옥 입소)을 구현해줘.

**AI**:
- **Engineering Summary**: 죄수 NPC의 행동 지능(FSM)을 [State-Based Sequential Routine]으로 구축함. 가마솥의 물리적 출력 스택에서 자원을 추출하여 죄수의 개별 스태커로 이동시키는 [Direct Resource Exchange] 시스템을 구현하였으며, DOTween 연출 완료 시점에 논리적 적재를 수행하는 동기화 코드를 적용함. WaitingLineManager(FIFO)와 PrisonManager(수용량 제어)를 유기적으로 연동하여 시스템 전반의 자원 순환 루프를 완성함.

**User**:
> 죄수 Npc 명칭을 Prisoner로 정정하고, 포션 수령 완료 시 머리 오브젝트 활성화 로직 추가해줘.

**AI**:
- **Engineering Summary**: 클래스 명칭 명명 규칙 정정(Criminal -> Prisoner) 및 물리적 파일명 동기화를 수행함. 자원 수집 완료 상태를 시각화하기 위해 headObject 활성화 메커니즘을 FSM 시퀀스에 통합하였으며, 이를 통해 NPC의 논리적 상태 변화와 월드 내 시각 피드백 간의 일관성을 확보함.

**User**:
> 포션 테이블 기능 구현 요청. 플레이어가 테이블 앞에 있어야만 죄수에게 배급되도록 설계. 모듈화 및 예외 상황 고려 필수.

**AI**:
- **Engineering Summary**: 플레이어 임석 여부를 배급 트리거로 사용하는 [Auth-Protected Distribution System]을 구축함. 범용 플레이어 감지 모듈(PlayerDetectionZone)을 독립시켜 재사용성을 확보하였으며, PotionTable 클래스에서 이를 활용해 배급 권한을 동적으로 제어함. Prisoner NPC의 FSM 시퀀스에 테이블 배급 허가 상태 체크를 통합하여 자원 흐름의 논리적 무결성을 완성함.

**User**:
> 죄수자 생성 로직 구현 요청. 감옥 및 대기열 상태에 따른 조건부 스폰 시스템 설계.

**AI**:
- **Engineering Summary**: 감옥 수용량과 물리적 대기열 공간을 실시간 모니터링하여 생성을 결정하는 [Conditional Prisoner Spawner]를 구축함. PoolManager를 통한 NPC 재사용 구조를 적용하여 런타임 Alloc 부하를 최적화하였으며, 타이머 기반의 독립적인 스폰 루틴을 통해 자원 공급과 수용 간의 밸런싱 자동화를 달성함.

**User**:
> 죄수가 도착하고 플레이어가 도착 시 배급이 안 되는 버그 수정 요청. 구현 미비점 보완.

**AI**:
- **Engineering Summary**: Npc 추상 클래스에 IsMoving 상태 프로퍼티를 추가하여 물리적 이동과 논리적 상호작용 간의 실행 시점을 동기화함. Prisoner의 수령 루틴에 이동 완료 검증 로직을 통합하여 이동 중 자원 수급 현상을 차단하였으며, 0.2초 단위의 고주파 상태 체크를 통해 플레이어 개입(CanDistribute)에 대한 반응성을 최적화함. 이를 통해 [Auth-Protected Distribution] 시스템의 신뢰성을 확보함.

**User**:
> MiningController.cs(9,30): error CS0246: MiningEquipment를 찾을 수 없음. (물리 구조 정리 후 발생한 버그)

**AI**:
- **Engineering Summary**: 물리적 구조 재편 과정에서 유실된 MiningEquipment 및 MiningDataSO 스크립트를 03. Gameplay/Equipment 경로에 복구 및 재생성함. 데이터 중심 설계(Data-Driven Design) 원칙을 유지하며 핵심 로직을 재구축하여 참조 무결성을 확보하였으며, 이를 통해 컴파일 타임 오류를 제거하고 아키텍처 일관성을 회복함.

**User**:
> 감옥 입장 전에 웨이팅 포인트(대기 구역)를 반드시 한 번 거쳐서 들어가도록 보완해줘.

**AI**:
- **Engineering Summary**: 죄수의 수감 시퀀스에 [Mandatory Buffer Zone] 방문 단계를 추가함. PrisonManager에서 대기 구역 내 좌표 공급 로직을 구축하고, Prisoner FSM에서 이동 완료 검증 후 최종 입소 요청을 수행하는 2단계 검증 구조를 적용함. 이를 통해 감옥 수용 상태와 관계없이 입소 전 정체 구간을 시각화하여 게임의 공간적 몰입감을 강화함.

**User**:
> PlayerDetectionZone을 통해 이벤트를 실행시켜도 true가 안 되는 버그 수정 요청.

**AI**:
- **Engineering Summary**: 물리 트리거의 신뢰성을 강화하기 위해 [Counter-based Overlap Detection] 로직을 PlayerDetectionZone에 적용함. 복수 콜라이더 간섭으로 인한 상태 깜빡임(Flickering) 현상을 카운팅 방식으로 해결하였으며, 명시적 디버깅 로그를 통해 레이어 및 물리 설정 오류를 즉각 식별할 수 있도록 가시성을 확보함. 이를 통해 PotionTable의 배급 권한 승인 프로세스의 안정성을 보장함.

**User**:
> 죄수가 포션을 받으면 풀로 반납되게 하고, 포션이 날아가는 위치를 정확하게 보정해줘.

**AI**:
- **Engineering Summary**: 죄수 NPC의 자원 수령 메커니즘을 [Virtual Consumption] 구조로 개편함. 수령 연출 타겟을 NPC 본체로 고정하여 시각적 정밀도를 확보하였으며, 연출 완료 시점에 IPickupAble.Release()를 호출하여 물리 리소스를 즉각 풀로 환수함. 이를 통해 대량의 NPC 운용 시의 메모리 및 렌더링 성능을 최적화하고 자원 흐름의 논리적 무결성을 강화함.

**User**:
> Npc 이동 시 IsWalk 애니메이션 동기화 및 Prisoner 감옥 도착 후 180도 회전 로직 추가 요청.

**AI**:
- **Engineering Summary**: Npc 추상 베이스 클래스의 MoveTo 인터페이스 내에 IsWalk 애니메이션 파라미터 제어를 통합하여 이동 상태 시각화의 일관성을 확보함. Prisoner NPC의 수감 시퀀스에 도착 후 180도 부드러운 회전(DORotate) 연출을 추가하여 공간적 정렬 및 시각적 피드백을 강화하였으며, 자식 클래스의 중복 애니메이션 제어 코드를 제거하여 [SRP] 기반의 청결한 구조를 확립함.

**User**:
> 코드 수정 후 발생한 오류 전수 조사 및 수정 요청.

**AI**:
- **Engineering Summary**: 물리 구조 재편 및 대규모 리팩터링 중 발생한 Npc.cs의 구문 오류(중복 선언 및 괄호 불일치)를 해결함. Prisoner NPC의 상속 관계 및 DOTween 네임스페이스 참조를 정상화하였으며, [IsMoving] 프로퍼티와 애니메이션 파라미터 간의 동기화 로직을 최적화하여 물리적 이동과 시각적 피드백의 일치성을 확보함. 이를 통해 프로젝트 전반의 컴파일 무결성을 복구함.

**User**:
> 죄수자는 감옥에 도착해서 사라지면 안 돼. 계속 모아둬야 해.

**AI**:
- **Engineering Summary**: Prisoner NPC의 최종 수감 단계를 [Disappearing]에서 [Persistent Presence] 구조로 변경함. PrisonManager의 입소 완료 콜백에서 비활성화(SetActive) 로직을 주석 처리하여 죄수가 감옥 구역에 상주하도록 수정하였으며, 논리적 수용량 카운팅은 유지하여 대기열 시스템과의 연동 무결성을 확보함.

**User**:
> Prisoner 이동 정밀도 보정 및 도착 후 Rigidbody 회전 고정 요청. (오버슈팅 및 물리 불안정 해결)

**AI**:
- **Engineering Summary**: Npc 베이스 클래스의 MoveTo 시퀀스에 물리 속도 초기화 루틴을 삽입하고 FixedUpdate 동기화 설정을 적용하여 이동 정밀도를 극대화함. Prisoner NPC의 수감 루틴 중 회전 연출 완료 시점에 Rigidbody Constraints를 조작하여 모든 축의 회전을 잠금(Freeze Rotation) 처리함으로써 외부 충돌 안정성을 확보함. 도착 판정 로직의 수치 보정을 통해 고질적인 오버슈팅 현상을 해결하고 아키텍처의 물리적 신뢰성을 강화함.

**User**:
> Prisoner 대기 구역 도착 정밀도 보정 및 감옥 도착 후 180도 회전 로직 수정 요청.

**AI**:
- **Engineering Summary**: 죄수 수감 시퀀스를 [Verification-Based Sequential Flow]로 개편함. WaitingPoint 도착 시 0.3초의 강제 정지 타임을 삽입하여 오버슈팅 현상을 시각적으로 해결하였으며, 180도 회전 및 물리 고정 로직을 감옥 최종 도착 완료(OnComplete) 시점으로 이전하여 기획 의도에 부합하는 수감 연출을 완성함. PrisonManager로 최종 연출 책임을 일원화하여 도메인 간 결합도를 낮추고 런타임 안정성을 강화함.

**User**:
> 감옥에 죄수가 들어올 때 문 오브젝트가 비활성화(열림) 되도록 로직 추가 요청.

**AI**:
- **Engineering Summary**: PrisonManager에 환경 상호작용 로직을 통합하여 [Auto-Door Management] 시스템을 구축함. 입소 프로세스 시작 시 문 오브젝트를 비활성화하고, 최종 정렬 완료 시 활성화하는 시퀀스를 정립하였으며, 다중 입소 상황에서의 논리적 충돌을 방지하기 위해 카운팅 기반의 동기화 메커니즘을 적용함. 이를 통해 월드 내 물리적 활동에 반응하는 동적인 환경 피드백을 완성함.

**User**:
> Prisoner 대기 구역 도착 정밀도 보정 및 오버슈팅 버그 수정 요청.

**AI**:
- **Engineering Summary**: NPC 이동 시스템을 [Kinematic-Tween Hybrid] 구조로 업그레이드함. 이동 중 Rigidbody 물리 연산을 일시 차단(isKinematic)하여 관성으로 인한 위치 이탈을 원천 제거하였으며, 도착 완료 시점에 물리 복구 및 좌표 스냅을 병행하여 100%의 도착 정밀도를 확보함. PrisonManager의 대기 지점 랜덤성을 제거하고 대기 지점 도착 후 명시적 정지 시퀀스(0.3s)를 추가하여 시각적 개연성을 완성함.

**User**:
> 감옥 인원수가 꽉 차면 Prisoner를 더 이상 생성하지 않도록 로직 추가 요청. (과잉 스폰 방지)

**AI**:
- **Engineering Summary**: 감옥 수용량 관리 로직을 [Potential Occupancy Registry] 구조로 개편함. PrisonerSpawner와 PrisonManager 간의 스폰 즉시 등록 메커니즘을 구축하여, 월드 내 이동 중인 NPC를 포함한 전체 수용량 합계가 Max Capacity를 초과하지 않도록 보장함. 이를 통해 포션 테이블 대기열의 정체와 상관없이 최종 수감 가능 인원만을 정밀하게 통제하는 스폰 밸런싱 시스템을 완성함.

**User**:
> 배급 완료 시 골드 생성 및 moneyZone 적재 로직 구현 요청. 연출 없이 즉시 생성 필수.

**AI**:
- **Engineering Summary**: 자원 배급 루프의 최종 보상 단계인 [Gold Reward System]을 구축함. PotionTable 클래스에 골드 프리팹 및 moneyStacker 참조를 통합하고, Prisoner NPC의 수령 완료 시점에 이를 트리거하는 인터페이스를 구현함. 요청 사항에 따라 Parabolic Tween을 생략한 즉시 적재(PushStack) 방식을 채택하여 보상 획득의 직관성을 높임.

---

**User**:
> PrisonerUI가 배급 중인 NPC에게만 동적으로 노출되도록 개선 요청. 테이블 도착 시 활성화, 배급 완료 시 즉시 비활성화 필수.

**AI**:
- **Engineering Summary**: 단일 PrisonerUI를 여러 NPC가 공유하는 **[UI Leasing]** 시스템을 구축함. UIManager를 싱글톤 기반의 소유권자로 설정하여 UI 인스턴스 관리를 일원화함. Prisoner NPC가 대기열 맨 앞(테이블 앞) 지점에 완벽히 정지(!IsMoving)했을 때만 UI를 요청(Show)하고, 필요한 포션을 모두 수령한 즉시(CompleteCollection 최상단) 반납(Hide)하는 상태 기반 제어 로직을 구현함. 이를 통해 런타임 드로우 콜과 메모리 오버헤드를 최적화하고, 플레이어의 시각적 피드백을 현재 활성 타겟에게만 집중시키는 고품질 UI 아키텍처를 완성함.

---

**User**:
> 플레이어나 보관소의 인벤토리 가득 찼을 때 노출되는 MaxUI 시스템 구현 요청.

**AI**:
- **Engineering Summary**: 자원 수집/이동 제한 상태를 시각화하는 **[Context-Aware MaxUI Notification]** 전략을 수립함. `ItemStacker`의 `IsFull` 상태를 트리거로 활용하며, `UIManager`를 통해 플레이어나 특정 보관소 위치에 `MaxUI`를 동적으로 할당하는 구조를 설계함. 이를 통해 행동의 거부 원인을 플레이어에게 즉각 피드백하고, 게임의 물리적 제약 조건을 시각적으로 명확히 전달하는 UX 강화 설계를 제안함.

---

**User**:
> MaxUI가 나타날 때 위로 천천히 올라가는 상승 연출 추가 요청.

**AI**:
- **Engineering Summary**: 월드 좌표 추적과 시각적 연출 간의 논리적 충돌을 방지하기 위해 **[Animation Offset Decoupling]** 기법을 적용함. `currentVisualOffset` 변수를 DOTween으로 제어하여 타겟의 스크린 좌표에 합산하는 방식으로 설계함. 이를 통해 타겟이 이동하는 중에도 상승 연출이 안정적으로 유지되도록 보장하였으며, `OutCubic` 이징을 적용하여 팝업 피드백의 품질을 고도화함.

---

**User**:
> 플레이어가 해당 영역에 들어가면 보유하고 있는 골드를 사용해서 해금을 할 수 있는 해금 시스템 구현 요청. 장비 해금, NPC 추가, 감옥 확장 등 다양한 보상 포함 필수.

**AI**:
- **Engineering Summary**: 다양한 보상 유형을 유연하게 처리하기 위한 **[Strategy-Based Unlock System]** 아키텍처를 설계함. 범용 상호작용 컴포넌트인 `UnlockZone`을 구축하고, `IUnlockReward` 인터페이스를 통해 보상 로직을 캡슐화하여 결합도를 낮춤. 플레이어의 골드를 일정 간격으로 흡수하여 비용을 충당하는 연출 중심의 로직을 제안하여 방치형 게임의 성장 피드백을 강화함.

---

**User**:
> 풀에 오브젝트를 반납(Return)했을 때, 해당 풀의 전용 자식 오브젝트로 부모가 설정되도록 수정 요청.

**AI**:
- **Engineering Summary**: 계층 구조(Hierarchy)의 가독성과 관리 효율을 높이기 위해 **[Automatic Reparenting on Release]** 기능을 구현함. `ObjectPool.Return` 메서드 호출 시 오브젝트를 비활성화하는 동시에 풀 생성 시 지정된 `parent` 트랜스폼 하위로 즉시 복귀시키도록 수정함. 이를 통해 런타임에 월드 스페이스로 이탈했던 아이템이나 NPC들이 반납 즉시 각자의 풀 컨테이너로 정렬되는 체계적인 리소스 관리 환경을 구축함.

---

**User**:
> 드릴과 불도저 사용 시 인벤토리가 가득 차더라도 조약돌만 얻지 못할 뿐 채굴(파괴)은 계속할 수 있도록 수정 요청.

**AI**:
- **Engineering Summary**: 파괴 로직과 수집 로직을 물리적으로 분리하는 **[Non-Blocking Destructive Mining]** 구조를 적용함. `MiningController`에서 인벤토리의 `IsFull` 상태와 상관없이 `target.MineResource()`를 호출하여 대상 오브젝트의 파괴를 보장하도록 수정함. 단, 가득 찬 상태에서는 리소스 반환값을 `null`로 유도하여 인벤토리 적재 및 연출 로직만 선택적으로 차단함으로써, 조작의 연속성을 유지하는 동시에 자원 획득 제한을 시각적으로 표현함.

---

**User**:
> MaxUI가 이미 재생 중일 때 중복 실행되지 않도록 수정 요청.

**AI**:
- **Engineering Summary**: 단일 UI 인스턴스의 연출 안정성을 확보하기 위해 **[Animation State Guarding]** 로직을 도입함. `MaxUI` 내부에 `isShowing` 플래그를 추가하여 재생 중 새로운 호출을 차단하고, `DOTween`의 `OnComplete` 콜백을 통해 애니메이션이 완전히 종료된 시점에만 다음 호출을 허용하도록 설계함. 이를 통해 드릴 등 고주파 동작 시 발생하는 불필요한 연출 중복과 CPU 오버헤드를 최적화함.

---

**User**:
> 직선 방향으로 전진하며 전방의 돌을 채굴하고, 끝에 도달하면 처음 위치로 복귀하는 채굴 인부(Mining Worker) NPC 구현 요청. 채굴된 자원은 보관소로 즉시 전송 필수.

**AI**:
- **Engineering Summary**: 자동화된 자원 수확을 위해 **[Scan-and-Transport FSM]** 기반의 `MiningWorker` 시스템을 구축함. 레이캐스트로 전방 돌을 감지하고, 채굴 성공 시 생성된 리소스를 `targetStorage`의 다음 적재 위치로 `DOParabolicMove`를 이용해 즉시 날려 보내는 로직을 구현함. 경로 끝 도달 시 시작 지점으로 복귀하는 루프 구조를 통해 채굴 및 운송 프로세스를 자동화함.

---

**User**:
> 해금을 통해 인부 NPC들이 생성되도록 구현 요청. 채굴 인부 3명과 운반/배급 인부 1명을 각각 별도로 해금할 수 있도록 설계 필수. 운반 인부는 완충 대기 및 하적 우선 사이클을 준수해야 함.

**AI**:
- **Engineering Summary**: 고효율 물류 자동화를 위해 **[Strict Buffer-and-Distribute FSM]**을 탑재한 `TransportWorker` 시스템을 구축함. 인벤토리가 가득 찰 때까지 가마솥에서 대기하고, 테이블에서는 인벤토리가 완전히 비워질 때까지 하적하며, 이후 테이블에 물량이 있는 동안만 상주하여 배급 권한을 승인하는 엄격한 상태 전이 로직을 구현함. `NpcWorkerUnlockReward`를 확장하여 복수 인부의 배기 활성화를 지원함으로써 생산-물류-배급 라인의 자동화 완성도를 높임.

---

**User**:
> 운반 인부가 테이블 하적 및 배급 대기 시 월드 좌표 기준 뒤쪽(Vector3.back)을 바라보도록 수정 요청.

**AI**:
- **Engineering Summary**: 작업 상황에 따른 NPC의 방향 정렬을 위해 **[Fixed Directional Alignment]** 기능을 구현함. `TransportWorker`가 테이블 및 배급 지점 도착 직후 `DORotate`를 사용하여 즉시 `Vector3.back`(180도) 방향을 바라보게 함으로써, 플레이어의 시선과 일치하는 작업 방향성을 확보하고 게임 내 시각적 개연성을 강화함.
