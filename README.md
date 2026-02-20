# MouseHighlighterPro (WPF)

유튜브/강의/게임 개발 녹화에서 **마우스 커서 위치가 잘 보이도록** 커서 주변에 원형 하이라이트와 클릭 펄스 효과를 표시하는 **Windows 오버레이(항상 위 / 클릭 통과)** 프로그램입니다.

- **트레이(알림 영역) 상주**: 프로그램 실행 시 트레이 아이콘이 생성됩니다.
- **오버레이 토글**: 단축키로 즉시 표시/숨김 전환
- **클릭 펄스 효과**: 좌/우/중 클릭에 따라 색상이 다른 펄스 효과
- **DPI 고려**: 멀티 모니터/배율 환경을 고려한 커서 좌표 처리
- **설정 저장**: `%AppData%\MouseHighlighterPro\settings.json`

---

## 요구 사항

- Windows 10/11
- **.NET 8 SDK (x64)**  
  - 프로젝트 타겟: `net8.0-windows`
- Git(소스 클론 시)
- 아래 IDE 중 하나
  - Visual Studio 2022
  - Visual Studio Code
  - JetBrains Rider

---

## 설정 파일(중요)

설정 저장 위치:

- `%AppData%\MouseHighlighterPro\settings.json`

대표 설정 항목(요약):

- `OverlayEnabled`: 시작 시 오버레이 표시 여부
- `TargetFps`: 렌더링 목표 FPS
- `Circle`: 커서 하이라이트 원(반지름/두께/색/그림자)
- `ClickEffect`: 클릭 펄스 효과(활성/크기/지속시간/버튼별 색)
- `Hotkeys`: 단축키(토글/설정 열기)

> 값이 꼬였을 때는 `settings.json`을 삭제하고 다시 실행하면 기본값으로 재생성됩니다.

---

# 개발 환경별 사용 메뉴얼 (비개발자도 따라할 수 있게)

아래 절차는 **소스 코드로 빌드해서 실행하는 방법**입니다.  
(추후 GitHub Releases에 실행 파일을 올리는 경우, 사용자는 “다운로드 → 실행”으로 더 간단해집니다.)

---

## 0) 공통 준비 단계 (한 번만 하면 됨)

### 0-1. .NET 8 SDK 설치
1. Windows에서 `.NET 8 SDK`를 설치합니다.
2. 설치 확인:
   - 시작 메뉴 → `Windows Terminal` 또는 `PowerShell` 실행
   - 아래 입력 후 버전이 표시되면 OK
     ```bash
     dotnet --version
     ```

### 0-2. 소스 코드 받기(클론)
1. GitHub 저장소 페이지에서 주소를 복사합니다.
2. 원하는 폴더에서 터미널을 열고 아래 실행:
   ```bash
   git clone <저장소주소>
   cd <저장소폴더>
   ```

## 1) Visual Studio 2022로 실행/빌드
### 1-1. 설치(권장 구성)
1. Visual Studio Installer 실행
2. 다음 워크로드/구성요소가 포함되도록 설치
   - .NET desktop development
   - (선택) Git 관련 구성요소

### 1-2. 프로젝트 열기
1. Visual Studio 실행
2. `파일(File) → 열기(Open) → 프로젝트/솔루션(Project/Solution)`
3. 저장소 폴더에서 `MouseHighlighterPro.csproj` 선택 후 열기

### 1-3. 실행
1. 상단 툴바에서 실행 대상이 `MouseHighlighterPro`로 선택되어 있는지 확인
2. `F5`(디버그 실행) 또는 `Ctrl + F5`(디버그 없이 실행)
3. 실행되면 트레이 아이콘이 생성됩니다.
   - 트레이 메뉴: 오버레이 표시/숨김 / 설정 / 종료
  
### 1-4. 실행 파일 만들기(배포용 Publish)
> “누구나 IDE 없이 실행”을 원하면 Publish를 사용하세요.

1. `빌드(Build) → 게시(Publish)…`
2. 대상 선택 예시
  - Folder(폴더) 게시
3. 권장 옵션(예시)
  - 구성: `Release`
  - 대상 런타임: `win-x64`
  - (선택) Self-contained: 켜면 .NET 미설치 PC에서도 실행 가능(용량 증가)
4. 게시 후 출력 폴더에서 `.exe` 실행

---

## 2) Visual Studio Code로 실행/빌드
### 2-1. 설치해야 할 것
  - Visual Studio Code 설치
  - VS Code 확장 설치(Extensions)
    - C# Dev Kit (또는 C# 관련 확장)
  - .NET 8 SDK 설치(공통 준비 단계에서 완료)

### 2-2. 폴더 열기
1. VS Code 실행
2. `File → Open Folder…`
3. 저장소 폴더(= `MouseHighlighterPro.csproj`가 있는 폴더) 선택

### 2-3. 복원/빌드/실행 (가장 확실한 방법: 터미널)
1. VS Code 상단 메뉴 `Terminal → New Terminal`
2. 아래 순서대로 입력:
  ```bash
  dotnet restore
  dotnet build -c Release
  dotnet run
  ```
3. 실행되면 트레이 아이콘이 생성됩니다.
> `dotnet run`은 디버그 편의용입니다. 배포용 실행 파일이 필요하면 “Publish”를 사용하세요.

### 2-4. Publish(배포용 실행 파일 만들기)
  ```bash
  dotnet publish -c Release -r win-x64 --self-contained false
  ```
- 출력 폴더 예시:
  - `bin/Release/net8.0-windows/win-x64/publish/`
- 위 폴더의 .exe를 실행하면 됩니다.
> .NET 미설치 PC에서도 돌리고 싶다면"
  ```bash
  dotnet publish -c Release -r win-x64 --self-contained true
  ```
(용량이 커지지만 설치 부담이 줄어듭니다.)

---

## 3) JetBrains Rider로 실행/빌드
### 3-1. Rider 준비
- Rider 설치
- .NET 8 SDK 설치(공통 준비 단계에서 완료)

### 3-2. 프로젝트 열기
1. Rider 실행
2. `Open` 선택
3. 저장소 폴더에서 `MouseHighlighterPro.csproj` 선택 후 열기
  (또는 저장소 폴더 자체를 열어도 Rider가 csproj를 인식합니다.)

### 3-3. 실행
1. 우측 상단 Run/Debug 구성에서 프로젝트가 잡혔는지 확인
2. `Run`(▶) 또는 `Debug`로 실행
3. 실행되면 트레이 아이콘이 생성됩니다.

### 3-4. Publish(배포용)
Rider에서도 터미널을 열어 아래를 실행하는 것이 가장 확실합니다.
  ```bash
  dotnet publish -c Release -r win-x64 --self-contained false
  ```

---

# 사용 방법(실행 후)
1. 프로그램 실행 → 트레이 아이콘 생성
2. 트레이 아이콘 우클릭
  - **오버레이 표시/숨김**
  - **설정**
  - **종료**

---

# 자주 발생하는 문제 / 해결

## 1) 트레이 아이콘이 안 보입니다
- Windows 트레이 숨김 아이콘(∧) 안에 들어가 있을 수 있습니다.
- `설정 → 개인 설정 → 작업 표시줄 → 작업 표시줄 모서리 오버플로`에서 표시를 켜세요.

## 2) 단축키가 동작하지 않습니다
- 다른 프로그램이 같은 전역 단축키를 선점했을 수 있습니다.
- %AppData%\MouseHighlighterPro\settings.json에서 단축키를 변경하세요.
- 변경 후 프로그램을 재시작하세요.

## 3) 오버레이가 보이지만 클릭이 안 됩니다
- 이 프로그램은 기본적으로 “클릭 통과(WS_EX_TRANSPARENT)” 오버레이입니다.
- 즉, 오버레이 자체는 입력을 막지 않고 뒤의 프로그램을 클릭하게 설계되어 있습니다.

## 4) 멀티 모니터/배율에서 위치가 어긋납니다
- Windows 디스플레이 배율(DPI) 설정에 따라 보정이 달라질 수 있습니다.
- 문제가 지속되면 이슈로 “모니터 구성(해상도/배율) + 재현 영상”을 공유해 주세요.

---

# 폴더 구조(요약)
- `Models/` : 설정 모델(원/클릭 효과/단축키)
- `Rendering/` : 오버레이 렌더링, Surface 관리
- `Services/`
  - `CursorTracker` : 커서 좌표 추적
  - `MouseHookService` : 클릭 이벤트 감지(저수준 훅)
  - `HotkeyService` : 전역 단축키
  - `SettingsRepository` : 설정 저장/로드
  - `TrayService` : 트레이 메뉴
  - `RenderLoop` : 렌더 루프/프레임 제한
- `Utils/` : 공용 유틸
