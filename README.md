# EasyGame

> 아이워너 시리즈에서 영감을 받은 하드코어 2D 플랫포머 게임

[![Steam](https://img.shields.io/badge/Steam-Available-blue?logo=steam)](https://store.steampowered.com/app/2257380/Easy_Game/)
[![Unity](https://img.shields.io/badge/Unity-6000.2.6f2-black?logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-.NET-512BD4?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)

## 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 개발 기간 | 2022.07 ~ 2023.01 (약 6개월) |
| 개발 인원 | 1인 개발 |
| 장르 | 2D 하드코어 플랫포머 |
| 플랫폼 | PC (Steam) |
| 엔진 | Unity 6000.2.6f2 |
| 언어 | C# |

어려운 장애물과 함정들을 극복하고, 각 스테이지의 보스를 클리어했을 때의 성취감을 목표로 기획한 게임입니다.

## 기술 스택

**Game Engine & Language**
- Unity, C#

**Third-Party Plugins**
- **DoTween** - 트윈 애니메이션 처리
- **Master Audio** - 오디오 시스템 관리
- **Easy Save 3** - 데이터 저장/로드
- **Steamworks.NET** - Steam API 연동 및 업적 시스템

## 핵심 기술 구현

### 1. Raycast 기반 충돌 감지 시스템

Unity의 기본 Rigidbody2D 물리 엔진 대신 **커스텀 Raycast 충돌 시스템**을 구현했습니다.

**구현 배경**
- Rigidbody2D는 빠른 속도에서 오브젝트 관통 현상 발생
- 플랫포머 게임 특성상 정밀한 충돌 감지 필수

**구현 내용**
```
RaycastController (Base)
    └── Controller2D (충돌 감지)
            └── Player (플레이어 컨트롤러)
```

- `RaycastController`: Collider 크기 기반 동적 Ray 개수 계산, Ray Origin 관리
- `Controller2D`: 수평/수직 충돌 감지, 경사면 오르내리기, 플랫폼 통과 처리
- 속도에 비례한 Ray 길이 동적 조절로 고속 이동 시에도 정확한 충돌 감지

### 2. 플랫폼 탑승자 시스템

움직이는 플랫폼 위 오브젝트들의 자연스러운 이동 처리를 구현했습니다.

**구현 내용**
- Raycast로 플랫폼 위 탑승자(Passenger) 감지
- Dictionary 기반 탑승자 Controller 캐싱으로 성능 최적화
- 플랫폼 이동 전/후 탑승자 이동 순서 분리 처리
- Waypoint 기반 경로 이동 + Easing 보간

### 3. 벽 슬라이딩 & 벽 점프

**구현 내용**
- 벽 접촉 감지 시 자동 슬라이딩 상태 전환
- 벽 점프 시 반대 방향 + 상향 속도 부여
- Wall Leap (벽 도약) 추가 구현으로 조작 다양성 확보

### 4. 특수 플랫폼 시스템

태그 기반으로 다양한 플랫폼 동작을 구현했습니다.

| 태그 | 동작 |
|------|------|
| `Through` | 단방향 플랫폼 (위에서 착지, 아래로 통과) |
| `CompulsionJump` | 스프링 플랫폼 (강제 점프) |
| `Slide` | 미끄러지는 바닥 |
| `SlideWall` | 벽 슬라이딩 가능 벽면 |

레이어 시스템으로 플레이어 상태(지상/점프/통과 중/사망) 관리

### 5. 체크포인트 & 세이브 시스템

- Easy Save 3를 활용한 진행 상황 저장
- 체크포인트 통과 시 위치 자동 저장
- 사망 시 최근 체크포인트에서 리스폰
- 사망 횟수, 리셋 횟수 통계 저장

### 6. 보스 시스템

5종의 개성 있는 보스를 구현했습니다.

- **Blue Dragon** - 낙하 오브젝트 패턴
- **Demon Skull** - 가시 솟구침 패턴
- **Evil Angel** - 해골 탄막 + 토네이도 복합 패턴
- **Minotaur** - 돌진 패턴
- **Skeleton King** - 다단계 공격 패턴

### 7. 오브젝트 풀링

투창 공격 시스템에 오브젝트 풀링을 적용하여 런타임 메모리 할당 최소화

## 프로젝트 구조

```
Assets/
├── 00_Scenes/          # Main + Stage1~6
├── 01_Scripts/
│   ├── Player/         # 플레이어 (Player, Controller2D, RaycastController, PlayerInput)
│   ├── Platform/       # 플랫폼 (15+ 종류)
│   ├── Traps/          # 트랩 (20+ 종류)
│   ├── Enemy/          # 적 AI (30+ 종류)
│   ├── Boss/           # 보스 (5종)
│   └── UI/             # UI, 카메라, 사운드
├── 02_Prefabs/         # 프리팹
├── 03_Sprite/          # 스프라이트
└── 07_Sound/           # 오디오
```

## 영상

| 티저 영상 | 플레이 영상 |
|-----------|-------------|
| [![Teaser](https://img.youtube.com/vi/OuxI5e_0pi0/mqdefault.jpg)](https://youtu.be/OuxI5e_0pi0) | [![Gameplay](https://img.youtube.com/vi/UqzPG1gSEOc/mqdefault.jpg)](https://youtu.be/UqzPG1gSEOc) |

## 회고

### 문제 해결 경험
- 해외 유저 플레이 영상에서 맵 경계 처리 미흡으로 인한 비정상 클리어 발견 → 경계 충돌 처리 보완
- 고속 이동 시 충돌 관통 문제 → Raycast 기반 커스텀 물리 시스템으로 해결

### 배운 점
- Raycast를 활용한 정밀 충돌 시스템 설계 경험
- 플랫포머 게임의 핵심 메카닉(벽 점프, 경사면 처리 등) 구현 역량 향상
- Steam 출시 및 업적 시스템 연동 경험

### 개선하고 싶은 점
- 초기 설계 단계에서의 구조적 아키텍처 설계
- 체계적인 디버깅 및 테스트 프로세스 도입

## 링크

- **Steam 스토어**: https://store.steampowered.com/app/2257380/Easy_Game/
- **티저 영상**: https://youtu.be/OuxI5e_0pi0
- **플레이 영상**: https://youtu.be/UqzPG1gSEOc
