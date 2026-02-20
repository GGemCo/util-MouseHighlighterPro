
# MouseHighlighterPro (WPF)

유튜브/강의 녹화 시 마우스 위치를 강조하기 위한 Windows 오버레이 프로그램입니다.

## 기본 단축키
- 오버레이 토글: Ctrl + Alt + M
- 설정 열기: Ctrl + Alt + O

## 실행
- Rider 또는 Visual Studio에서 `MouseHighlighterPro.csproj` 열기
- 실행하면 트레이 아이콘이 생성됩니다.
- 트레이 메뉴에서 오버레이 토글/설정/종료 가능

## 설정 저장 위치
- `%AppData%\MouseHighlighterPro\settings.json`

## 설계 포인트
- 최상단 투명 오버레이 + 클릭 통과(WS_EX_TRANSPARENT)
- 커서 좌표는 Win32 GetCursorPos 사용, DPI 변환 포함
- 렌더는 WPF OnRender 기반, Rendering 콜백 스로틀로 FPS 제한
- 클릭 펄스 효과는 Low-level mouse hook으로 트리거
